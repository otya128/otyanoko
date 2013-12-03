using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace otyanoko
{

    class HojichaHtmlDocument
    {
        private enum HojichaState
        {
            None = 0,
            nameRead = 1,
            atrRead = 2, atrValueNameRead = 3, atrValueRead = 4,//属性読み取り
            atrEqualRead = 5,//=を読んでるか
            atrNameSearch = 6,//nameを探し中
            isText=7,
            isComment=8,
           // goto,case
        }
        HojichaHtmlNode _documentNode;
        public HojichaHtmlNode DocumentNode { get { return _documentNode; } }
        string _html;
        public void LoadHtml(string p)
        {
            this._html = p;
            _documentNode = new HojichaHtmlNode();
            _documentNode.Position = 0;
            var pos=ParseHtml(_documentNode);
            if (pos.Position < p.Length) ParseHtml(pos);
        }

        public HojichaHtmlNode ParseHtml(HojichaHtmlNode hhn, bool Option=false)
        {
            int i = hhn.Position;
            bool nameRead = false;
            bool atrRead = false, atrValueNameRead = false, atrValueRead = false,//属性読み取り
                atrEqualRead = false,//=を読んでるか
                atrNameSearch = false;//nameを探し中
            string name = "",atr="",atrValue="";
            HojichaAttributeValueQuote Quote = HojichaAttributeValueQuote.DoubleQuote;
            HojichaHtmlNode newChild = new HojichaHtmlNode();
            newChild.Parent = hhn;
            string Text = "";
            bool /*isText = true, isComment = false,*/ CommentType = false; ;//コメントかどうか

            bool ClosedType = false;//false=</h1>true<br/>
            var state = HojichaState.isText;
            var start = 0;
            while (i<_html.Length)
            {
                char p = _html[i];

                switch (state)
                {
                    case HojichaState.isComment:
                        Text += p;
                    if (Text == "<!--") CommentType = true;
                    if (p == '>')
                    {
                        if (Text.Length > 6 && CommentType)
                        {
                            if (Text.Substring(Text.Length - 3) == "-->")
                            {
                                //isComment = false;
                                newChild = new HojichaHtmlNode(HojichaHtmlNode.HtmlNodeTypeNameComment, Text);
                                newChild.Parent = hhn;
                                Text = "";
                                state = HojichaState.isText;//isText = true;
                                hhn.ChildNodes.Add(newChild); newChild.setIndex();
                            }
                        }
                        else
                        {
                            //isComment = false;
                            newChild = new HojichaHtmlNode(HojichaHtmlNode.HtmlNodeTypeNameComment, Text);

                            newChild.Parent = hhn;
                            Text = "";
                            state = HojichaState.isText;//isText = true;
                            hhn.ChildNodes.Add(newChild); newChild.setIndex();
                        }
                    }
                    //途中で<>がでるとおかしくなるのを防止
                    i++; hhn.Position = i; continue;
                    case HojichaState.atrNameSearch:

                        if (state == HojichaState.atrNameSearch)
                            if (p != ' '&&p!='\r'&&p!='\n' && p != '>')
                            {
                                state = HojichaState.atrRead;
                                goto case HojichaState.atrRead;
                                //atrRead = true;
                                //atrNameSearch = false;
                            }
                        break;
                    case HojichaState.atrRead:
                        if (p == '/' ||p == '\n' || p == '\r' || p == ' ' || p == '>' || p == '=')
                        {
                            state = HojichaState.None;
                            //atrRead = false;
                            //if(!newChild.Closed)
                            atr = atr.ToLower();
                            newChild.Attributes[atr] = "";//Addだと重複時に例外が出るので
                            //atr = "";//debugcode
                            atrValue = "";
                            if (atr != "")
                            {
                                state = HojichaState.atrValueNameRead;//atrValueNameRead = true;//無限ループ避け
                                goto case HojichaState.atrValueNameRead;
                            }

                        }
                        else
                        {
                            atr += p;
                        }
                        break;
                    case HojichaState.atrValueNameRead:
                        /*if (p == '/' || p == ' ' || p == '>' || p == '=')
                        {
                            atrValueRead = false;
                            //if(!newChild.Closed)
                            newChild.Attributes.Add(atr, "");
                            atrRead = true;
                        }
                        else*/
                        if (p != ' '&&p!='\r'&&p!='\n')
                        {
                            if (state!=HojichaState.atrEqualRead && p == '=')
                            {
                                state = HojichaState.atrEqualRead;
                                goto case HojichaState.atrEqualRead;
                                //atrEqualRead = true;//イコール発見
                                //atrValueNameRead = false;
                            } if (state != HojichaState.atrEqualRead && p != '=')
                            {
                                //空の属性と認識
                                if (p != '>')//閉じタグでここに来た場合はひかなくていい
                                    i--;//スペースで飛ばされている分くぉ引く
                                //コメント未実装時に!DOCTYPEで無限ループしたので注意
                                //->if (atr != "") atrValueNameRead = true;//無限ループ避けで無限ループしなくなった
                                //atrValueNameRead = false;
                                //atrRead = false;
                                //atrEqualRead = false;
                                state = HojichaState.atrNameSearch;//atrNameSearch = true;
                                atr = "";
                                atrValue = "";
                            }//=とスペース以外は違うので抜ける
                        }
                        break;
                    case HojichaState.atrValueRead:

                        if (p == '"' && Quote == HojichaAttributeValueQuote.DoubleQuote)
                        {
                            //atrValueRead = false;
                            newChild.Attributes[atr] = atrValue;
                            atrValue = "";
                            atr = "";
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else if (p == '\'' && Quote == HojichaAttributeValueQuote.SingleQuote)
                        {
                            //atrValueRead = false;
                            newChild.Attributes[atr] = atrValue;
                            atrValue = "";
                            atr = "";
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else
                            if ((p == '\n' || p == '\r' ||p == ' ' || p == '>') && Quote == HojichaAttributeValueQuote.NonQuote)
                            {
                                //atrValueRead = false;
                                newChild.Attributes[atr] = atrValue;
                                atrValue = "";
                                atr = "";
                                state = HojichaState.atrNameSearch; //atrNameSearch = true;
                            }
                            else
                            {
                                atrValue += p;
                                //途中で>がでるとおかしくなるのを防止
                                i++; hhn.Position = i; continue;
                            }
                        break;
                    case HojichaState.atrEqualRead:
                        if (p != ' ' && p != '=' && p != '\r' && p != '\n')
                        {
                            if (p == '"')
                            {
                                Quote = HojichaAttributeValueQuote.DoubleQuote;
                                //atrEqualRead = false;
                                state = HojichaState.atrValueRead;//atrValueRead = true;
                            }
                            else
                                if (p == '\'')
                                {
                                    Quote = HojichaAttributeValueQuote.SingleQuote;
                                    //atrEqualRead = false;
                                    state = HojichaState.atrValueRead;//atrValueRead = true;
                                }
                                else
                                {
                                    //何も囲ってない
                                    Quote = HojichaAttributeValueQuote.NonQuote;
                                    //atrEqualRead = false;
                                    state = HojichaState.atrValueRead;//atrValueRead = true;
                                    atrValue += p;
                                }
                        }
                        break;

                    case HojichaState.nameRead:

                        if (/*p == '/' ||*/ p == '\n' || p == '\r' || p == ' ' || p == '>')
                        {
                            //nameRead = false;
                            //if(!newChild.Closed)
                            bool closed = newChild.Closed;
                            newChild = new HojichaHtmlNode(name.ToLower());
                            newChild.Parent = hhn;
                            //debugcode
                            if (name == "a")
                                name = "a";
                            newChild.Closed = closed;
                            //if (parent == "") parent = newChild.Name;
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else
                        {
                            if (p != '/') name += p;
                            if (name == "!")
                            {
                                name = "";
                                Text = "<!";
                                CommentType = false;
                                //isComment = true;
                                state = HojichaState.isComment;//nameRead = false;
                            }
                        }

                        break;
                }
                if (p == '/')
                {
                    if (name == "" && Text == "") { newChild.Closed = true; ClosedType = false; }
                    //<img> / <div>hoge</div>で次のタグが正常に取得できないバグ修正
                    else if(Text=="")ClosedType = true;
                }
                if (state==HojichaState.isText&&p!='<') Text += p;
                if (p == '<')
                {
                    start = i;
                    if (Text != "")
                    {
                        newChild = new HojichaHtmlNode("#text", Text);
                        newChild.Parent = hhn;
                        hhn.ChildNodes.Add(newChild); newChild.setIndex();
                        Text = "";
                        
                    }
                    if (Option)
                    {
                        hhn.Position--;
                        // hhn.ChildNodes.Add(newChild);
                        return hhn;
                    } 
                    //isText = false;
                    state = HojichaState.nameRead;//nameRead = true;
                }
                

                if (p == '>')
                {

                    if (name == "")
                    {
                        state = HojichaState.isText;//isText = true;
                        i++; hhn.Position = i; continue;
                    } //atrRead = false;
                    //atrNameSearch = false;
                    //atrValueNameRead = false;
                    state = HojichaState.isText;//atrValueRead = false;
                    //isText = true;
                    Text = "";
                    name = "";
                    if (newChild.Closed/*&&(parent==""||parent==newChild.Name)*/)
                    {
                        if (HojichaHtmlNode.ElementsFlags.ContainsKey(newChild.Name))
                            if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.Empty) == HojichaHtmlElementFlag.Empty)
                            {
                                //閉じなくてもいいタグ
                                //hhn.ChildNodes.Add(newChild);
                                newChild.Closed = false;
                                i++;
                                hhn.Position = i;
                                continue;
                            }else
                                if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.EndTag) == HojichaHtmlElementFlag.EndTag)
                                {
                                    //閉じなくてもいいタグ
                                    //hhn.ChildNodes.Add(newChild);
                                    newChild.Closed = false;
                                    i++;
                                    hhn.Position = i;
                                    continue;
                                }
                        //System.Diagnostics.Debug.Write(newChild.oya.Name+"\t");
                        //System.Diagnostics.Debug.WriteLine(newChild.Name);
                        if (newChild.Parent.Name != newChild.Name)//終了タグが一致しなければタグの処理をひとつ前に戻して戻る
                        {
                            hhn.Position = start-1;
                        }
                        return hhn;
                    }
                    else
                    {
                        
                        bool opt = false;
                        if (HojichaHtmlNode.ElementsFlags.ContainsKey(newChild.Name))
                            if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.Empty) == HojichaHtmlElementFlag.Empty)
                            {
                                hhn.ChildNodes.Add(newChild); newChild.setIndex();
                                i++;hhn.Position = i;continue;
                            }
                            else if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.EndTag) == HojichaHtmlElementFlag.EndTag)
                            {
                                opt = true;
                            }
                        if (ClosedType)
                        {
                            ClosedType = false;
                            hhn.ChildNodes.Add(newChild); newChild.setIndex();
                            i++;
                            hhn.Position = i;
                            continue;
                        }

                        hhn.ChildNodes.Add(newChild);
                        newChild.setIndex();
                        HojichaHtmlNode newNode = new HojichaHtmlNode();
                        newChild.Parent = hhn;
                        //newChild.ChildNodes.Add(newNode);
                        /*newNode*/
                        newChild.Position = i + 1;
                        i = ParseHtml(newChild/*,newChild.Name*/,opt).Position;
                        /**if (opt)
                        {
                            return hhn;
                        }*/
                    }
                    ClosedType = false;
                }
                i++;
                hhn.Position = i;
            }
            if (Text != "")
            {
                newChild = new HojichaHtmlNode("#text", Text);
                newChild.Parent = hhn;
                hhn.ChildNodes.Add(newChild); newChild.setIndex();
                Text = "";
            }
            return hhn;
        }

    }
    class HojichaHtmlNode
    {
        public HojichaHtmlNode Parent;//名前をoyaからParentに
        public int _index = 0;
        public void setIndex()
        {
            this._index = this.Parent.ChildNodes.Count;
        }
        public HojichaHtmlNode NextSibling
        {
            get
            {
                try
                {
                    if (this._index != Parent.ChildNodes.Count) return Parent.ChildNodes[this._index]; //return Parent.ChildNodes[Parent.ChildNodes.IndexOf(this) + 1];
                    else return null;
                }
                catch
                {
                    return null;
                }
            }
        }
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
            //ret.Add("option", HojichaHtmlElementFlag.Empty);
            ret.Add("br", HojichaHtmlElementFlag.Empty | HojichaHtmlElementFlag.Closed);
            ret.Add("p", HojichaHtmlElementFlag.Empty | HojichaHtmlElementFlag.Closed);
            ret.Add("option", HojichaHtmlElementFlag.Empty);
            ret.Add("dt", HojichaHtmlElementFlag.Empty);
            ret.Add("dd", HojichaHtmlElementFlag.Empty);
            //ret.Add("li", HojichaHtmlElementFlag.Empty);
           // ret.Add("font", HojichaHtmlElementFlag.Empty);//とりあえず
            return ret;
        }
        Dictionary<string, string> _attributes;
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
        public static Dictionary<string, HojichaHtmlElementFlag> ElementsFlags = ctor();
        //
        // 概要:
        //     Gets the name of a comment node. It is actually defined as '#comment'.
        public static readonly string HtmlNodeTypeNameComment = "#comment";
        //
        // 概要:
        //     Gets the name of the document node. It is actually defined as '#document'.
        public static readonly string HtmlNodeTypeNameDocument = "#document";
        //
        // 概要:
        //     Gets the name of a text node. It is actually defined as '#text'.
        public static readonly string HtmlNodeTypeNameText = "#text";
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
                    return getInnerText(this._childNodes, new StringBuilder()).ToString();
                }
                return _innerText;
            }
        }

        private StringBuilder getInnerText(List<HojichaHtmlNode> hhn, StringBuilder innerText)
        {
            foreach (var i in hhn)
            {
                if (i.Name == "#text") innerText.Append(i.InnerText);
                if (i.ChildNodes != null) if (i.ChildNodes.Count != 0) innerText = getInnerText(i.ChildNodes, innerText);
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

        internal string GetAttributeValue(string p1, string p2)
        {
            try
            {
                if (this.Attributes.ContainsKey(p1))
                    return this.Attributes[p1];
                else
                    return p2;
            }
            catch
            {
                return p2;
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
        /// <summary>
        /// End tag: optional
        /// </summary>
        EndTag=16,
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
