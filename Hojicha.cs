using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace otyanoko
{
    class HojichaHtmlDocument
    {
        HojichaHtmlNode _documentNode;
        public HojichaHtmlNode DocumentNode { get { return _documentNode; } }
        string _html;
        public void LoadHtml(string p)
        {
            this._html = p;
            _documentNode = new HojichaHtmlNode();
            _documentNode.Position = 0;
            ParseHtml(_documentNode);
        }

        public HojichaHtmlNode ParseHtml(HojichaHtmlNode hhn, string parent="")
        {
            int i = hhn.Position;
            bool nameRead = false;
            bool atrRead = false, atrValueNameRead = false, atrValueRead = false,//属性読み取り
                atrEqualRead = false,//=を読んでるか
                atrNameSearch = false;//nameを探し中
            string name = "",atr="",atrValue="";
            HojichaAttributeValueQuote Quote = HojichaAttributeValueQuote.DoubleQuote;
            HojichaHtmlNode newChild = new HojichaHtmlNode();
            string Text = "";
            bool isText = true;
            bool ClosedType = false;//false=</h1>true<br/>
            while (i<_html.Length)
            {
                char p = _html[i];
                if (p == '/')
                {
                    if (name == "" && Text == "") { newChild.Closed = true; ClosedType = false; } else ClosedType = true;
                }
                if (atrNameSearch)
                    if (p != ' '&&p!='>')
                    {
                        atrRead = true;
                        atrNameSearch = false;
                    }
                if (atrRead)
                {
                    if (p == '/' || p == ' ' || p == '>' || p == '=')
                    {
                        atrRead = false;
                        //if(!newChild.Closed)
                        newChild.Attributes[atr]="";//Addだと重複時に例外が出るので
                        //atr = "";//debugcode
                        atrValue = "";
                        atrValueNameRead = true;
                    }
                    else
                    {
                        atr += p;
                    }
                }
                if (atrValueNameRead)
                {
                    /*if (p == '/' || p == ' ' || p == '>' || p == '=')
                    {
                        atrValueRead = false;
                        //if(!newChild.Closed)
                        newChild.Attributes.Add(atr, "");
                        atrRead = true;
                    }
                    else*/
                    if (p != ' ')
                    {
                        if (!atrEqualRead && p == '=')
                        {
                            atrEqualRead = true;//イコール発見
                            atrValueNameRead = false;
                        } if (!atrEqualRead && p != '=')
                        {
                            //空の属性と認識
                            if(p != '>')//閉じタグでここに来た場合はひかなくていい
                                i--;//スペースで飛ばされている分くぉ引く
                            atrValueNameRead = false;
                            atrRead = false;
                            atrEqualRead = false;
                            atrNameSearch = true;
                            atr = "";
                            atrValue = "";
                        }//=とスペース以外は違うので抜ける
                    }
                }
                if (atrValueRead)
                {
                    if (p == '"' && Quote == HojichaAttributeValueQuote.DoubleQuote)
                    {
                        atrValueRead = false;
                        newChild.Attributes[atr] = atrValue;
                        atrValue = "";
                        atr = "";
                        atrNameSearch = true;
                    }
                    else if (p == '\'' && Quote == HojichaAttributeValueQuote.SingleQuote)
                    {
                        atrValueRead = false;
                        newChild.Attributes[atr] = atrValue;
                        atrValue = "";
                        atr = "";
                        atrNameSearch = true;
                    }else
                        if ((p == ' ' || p == '>') && Quote == HojichaAttributeValueQuote.NonQuote)
                        {
                            atrValueRead = false;
                            newChild.Attributes[atr] = atrValue;
                            atrValue = "";
                            atr = "";
                            atrNameSearch = true;
                        }else
                        atrValue += p;
                }
                if (atrEqualRead)
                {
                    if (p != ' '&&p!='=')
                    {
                        if (p == '"')
                        {
                            Quote = HojichaAttributeValueQuote.DoubleQuote;
                            atrEqualRead = false;
                            atrValueRead = true;
                        }
                        else
                            if (p == '\'')
                            {
                                Quote = HojichaAttributeValueQuote.SingleQuote;
                                atrEqualRead = false;
                                atrValueRead = true;
                            }
                            else
                            {
                                //何も囲ってない
                                Quote = HojichaAttributeValueQuote.NonQuote;
                                atrEqualRead = false;
                                atrValueRead = true;
                                atrValue += p;
                            }
                    }
                }

                if (nameRead)
                {
                    if (p == '/' || p == ' ' || p == '>')
                    {
                        nameRead = false;
                        //if(!newChild.Closed)
                        bool closed = newChild.Closed;
                            newChild = new HojichaHtmlNode(name);
                            newChild.Closed = closed;
                            if (parent == "") parent = newChild.Name;
                            atrNameSearch = true;
                    }
                    else
                    {
                        if(p!='/')name += p;
                    }
                }
                if (isText&&p!='<') Text += p;
                if (p == '<')
                {
                    if (Text != "")
                    {
                        newChild = new HojichaHtmlNode("#text",Text);
                        hhn.ChildNodes.Add(newChild);
                        Text = "";
                    } isText = false;
                    nameRead = true;
                }
                

                if (p == '>')
                {
                    atrRead = false;
                    atrNameSearch = false;
                    atrValueNameRead = false;
                    atrValueRead = false;
                    isText = true;
                    Text = "";
                    name = "";
                    if (newChild.Closed/*&&(parent==""||parent==newChild.Name)*/)
                    {
                        if (HojichaHtmlNode.ElementsFlags.ContainsKey(newChild.Name))
                            if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.Empty) == HojichaHtmlElementFlag.Empty)
                            {
                                //閉じなくてもいいタグ
                                //hhn.ChildNodes.Add(newChild);
                                i++;
                                hhn.Position = i;
                                continue;
                            }
                        return hhn;
                    }
                    else
                    {
                        if (HojichaHtmlNode.ElementsFlags.ContainsKey(newChild.Name))
                            if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.Empty) == HojichaHtmlElementFlag.Empty)
                            {
                                hhn.ChildNodes.Add(newChild);
                                i++;
                                hhn.Position = i;
                                continue;
                            }
                        if (ClosedType)
                        {
                            ClosedType = false;
                            hhn.ChildNodes.Add(newChild);
                            i++;
                            hhn.Position = i;
                            continue;
                        }
                        hhn.ChildNodes.Add(newChild);
                        HojichaHtmlNode newNode = new HojichaHtmlNode();
                        //newChild.ChildNodes.Add(newNode);
                        /*newNode*/
                        newChild.Position = i + 1;
                        i = ParseHtml(newChild/*,newChild.Name*/).Position;
                    }
                    ClosedType = false;
                }
                i++;
                hhn.Position = i;
            }
            return hhn;
        }

    }
    class HojichaHtmlNode
    {
        static Dictionary<string, HojichaHtmlElementFlag> ctor()
        {
            var ret = new Dictionary<string, HojichaHtmlElementFlag>();
            ret.Add("script", HojichaHtmlElementFlag.CData);
            ret.Add("style", HojichaHtmlElementFlag.CData);
            ret.Add("noxhtml", HojichaHtmlElementFlag.CData);
            ret.Add("base", HojichaHtmlElementFlag.Empty);
            ret.Add("link", HojichaHtmlElementFlag.Empty);
            ret.Add("meta", HojichaHtmlElementFlag.Empty);
            ret.Add("isindex", HojichaHtmlElementFlag.Empty);
            ret.Add("hr", HojichaHtmlElementFlag.Empty);
            ret.Add("col", HojichaHtmlElementFlag.Empty);
            ret.Add("img", HojichaHtmlElementFlag.Empty);
            ret.Add("embed", HojichaHtmlElementFlag.Empty);
            ret.Add("frame", HojichaHtmlElementFlag.Empty);
            ret.Add("wbr", HojichaHtmlElementFlag.Empty);
            ret.Add("bgsound", HojichaHtmlElementFlag.Empty);
            ret.Add("spacer", HojichaHtmlElementFlag.Empty);
            ret.Add("keygen", HojichaHtmlElementFlag.Empty);
            ret.Add("area", HojichaHtmlElementFlag.Empty);
            ret.Add("input", HojichaHtmlElementFlag.Empty);
            ret.Add("basefont", HojichaHtmlElementFlag.Empty);
            ret.Add("option", HojichaHtmlElementFlag.Empty);
            ret.Add("br", HojichaHtmlElementFlag.Empty | HojichaHtmlElementFlag.Closed);
            ret.Add("p", HojichaHtmlElementFlag.Empty | HojichaHtmlElementFlag.Closed);
            return ret;
        }
        Dictionary<string,string> _attributes;
        public Dictionary<string, string> Attributes
        {
            get
            {
                return _attributes;
            }
        }
        // 概要:
        //     Gets a collection of flags that define specific behaviors for specific element
        //     nodes.  The table contains a DictionaryEntry list with the lowercase tag
        //     name as the Key, and a combination of HtmlElementFlags as the Value.
        public static Dictionary<string, HojichaHtmlElementFlag> ElementsFlags=ctor();
        //
        // 概要:
        //     Gets the name of a comment node. It is actually defined as '#comment'.
        public static readonly string HtmlNodeTypeNameComment="#comment";
        //
        // 概要:
        //     Gets the name of the document node. It is actually defined as '#document'.
        public static readonly string HtmlNodeTypeNameDocument="#document";
        //
        // 概要:
        //     Gets the name of a text node. It is actually defined as '#text'.
        public static readonly string HtmlNodeTypeNameText="#text";
        public HojichaHtmlNode()
        {
            _childNodes = new List<HojichaHtmlNode>();
            _attributes = new Dictionary<string, string>();
        }
        public HojichaHtmlNode(string name)
        {
            _name = name;
            _childNodes = new List<HojichaHtmlNode>();
            _attributes = new Dictionary<string, string>();
        }

        public HojichaHtmlNode(string name, string Text)
        {
            // TODO: Complete member initialization
            this._name = name;
            _childNodes = new List<HojichaHtmlNode>();
            this._innerText = Text;
            _attributes = new Dictionary<string, string>();
        }
        string _innerText;
        public string InnerText
        {
            get
            {
                if (_innerText == null)
                {
                    return getInnerText(this._childNodes,"");
                }
                return _innerText;
            }
        }

        private string getInnerText(List<HojichaHtmlNode> hhn, string innerText)
        {
            foreach (var i in hhn)
            {
                if(i.Name=="#text")innerText+=i.InnerText;
                if (i.ChildNodes != null) innerText=getInnerText(i.ChildNodes, innerText);

            }
            return innerText;
        }
        public int Position;
        public bool _closed;
        public bool Closed
        {
            get
            {
                return _closed;
            }
            set
            {
                _closed = value;
            }
        }
        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        List<HojichaHtmlNode> _childNodes;
        public List<HojichaHtmlNode> ChildNodes
        {
            get
            {
                return _childNodes;
            }
            set
            {
                _childNodes = value;
            }
        }
    }
    //MEMO:CDataは中のhtmlを読まない、Emptyは閉じなくてもよい、Closed
    [Flags]
    public enum HojichaHtmlElementFlag
    {
        // 概要:
        //     The node is a CDATA node.
        CData = 1,
        //
        // 概要:
        //     The node is empty. META or IMG are example of such nodes.
        Empty = 2,
        //
        // 概要:
        //     The node will automatically be closed during parsing.
        Closed = 4,
        //
        // 概要:
        //     The node can overlap.
        CanOverlap = 8,
    }
    public enum HojichaAttributeValueQuote
    {
        // 概要:
        //     A single quote mark '
        SingleQuote = 0,
        //
        // 概要:
        //     A double quote mark "
        DoubleQuote = 1,
        /// <summary>
        /// なにも囲まない
        /// </summary>
        NonQuote = 2,
    }
    /*
    public class HojichaHtmlAttribute : IComparable
    {

    }
*/
}
