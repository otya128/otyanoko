using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace otyanoko
{
    partial class HojichaHtmlDocument
    {
        byte[] _htmlb;
        Encoding enc;
        public void LoadHtml(byte[] p,string encode)
        {
            this.enc = Encoding.GetEncoding(encode);
            this._htmlb = p;
            _documentNode = new HojichaHtmlNode();
            _documentNode.Position = 0;
            var pos = ParseHtmlByte(_documentNode);
            if (pos.Position < p.Length) ParseHtmlByte(pos);
        }

        public HojichaHtmlNode ParseHtmlByte(HojichaHtmlNode hhn, bool Option = false)
        {
            int i = hhn.Position;
            bool nameRead = false;
            bool atrRead = false, atrValueNameRead = false, atrValueRead = false,//属性読み取り
                atrEqualRead = false,//=を読んでるか
                atrNameSearch = false;//nameを探し中
            //string name = "", atr = "", atrValue = "";
            HojichaAttributeValueQuote Quote = HojichaAttributeValueQuote.DoubleQuote;
            HojichaHtmlNode newChild = new HojichaHtmlNode();
            newChild.Parent = hhn;
            //string Text = "";
            List<byte> name = new List<byte>(), atr = new List<byte>(), atrValue = new List<byte>(), Text = new List<byte>();
            bool /*isText = true, isComment = false,*/ CommentType = false; ;//コメントかどうか

            bool ClosedType = false;//false=</h1>true<br/>
            var state = HojichaState.isText;
            var start = 0;
            while (i < _htmlb.Length)
            {
                char p = (char)_htmlb[i];

                switch (state)
                {
                    case HojichaState.isComment:
                        Text.Add((byte)p);//Text += p;
                        if (enc.GetString(Text.ToArray()) == "<!--") CommentType = true;
                        if (p == '>')
                        {
                            if (enc.GetString(Text.ToArray()).Length > 6 && CommentType)
                            {
                                if (enc.GetString(Text.ToArray()).Substring(enc.GetString(Text.ToArray()).Length - 3) == "-->")
                                {
                                    //isComment = false;
                                    newChild = new HojichaHtmlNode(HojichaHtmlNode.HtmlNodeTypeNameComment, enc.GetString(Text.ToArray()));
                                    newChild.Parent = hhn;
                                    Text = new List<byte>();
                                    state = HojichaState.isText;//isText = true;
                                    hhn.ChildNodes.Add(newChild); newChild.setIndex();
                                }
                            }
                            else
                            {
                                //isComment = false;
                                newChild = new HojichaHtmlNode(HojichaHtmlNode.HtmlNodeTypeNameComment, enc.GetString(Text.ToArray()));

                                newChild.Parent = hhn;
                                Text = new List<byte>();
                                state = HojichaState.isText;//isText = true;
                                hhn.ChildNodes.Add(newChild); newChild.setIndex();
                            }
                        }
                        //途中で<>がでるとおかしくなるのを防止
                        i++; hhn.Position = i; continue;
                    case HojichaState.atrNameSearch:

                        if (state == HojichaState.atrNameSearch)
                            if (p != ' ' && p != '\r' && p != '\n' && p != '>')
                            {
                                state = HojichaState.atrRead;
                                goto case HojichaState.atrRead;
                                //atrRead = true;
                                //atrNameSearch = false;
                            }
                        break;
                    case HojichaState.atrRead:
                        if (p == '/' || p == '\n' || p == '\r' || p == ' ' || p == '>' || p == '=')
                        {
                            state = HojichaState.None;
                            //atrRead = false;
                            //if(!newChild.Closed)
                            atr = new List<byte>(enc.GetBytes(enc.GetString(atr.ToArray()).ToLower()));
                            newChild.Attributes[enc.GetString(atr.ToArray())] = "";//Addだと重複時に例外が出るので
                            //atr = "";//debugcode
                            atrValue = new List<byte>();
                            if (atr.Count != 0)
                            {
                                state = HojichaState.atrValueNameRead;//atrValueNameRead = true;//無限ループ避け
                                goto case HojichaState.atrValueNameRead;
                            }

                        }
                        else
                        {
                            atr.Add((byte)p);//atr += p;
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
                        if (p != ' ' && p != '\r' && p != '\n')
                        {
                            if (state != HojichaState.atrEqualRead && p == '=')
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
                                atr = new List<byte>();
                                atrValue = new List<byte>();
                            }//=とスペース以外は違うので抜ける
                        }
                        break;
                    case HojichaState.atrValueRead:

                        if (p == '"' && Quote == HojichaAttributeValueQuote.DoubleQuote)
                        {
                            //atrValueRead = false;
                            newChild.Attributes[enc.GetString(atr.ToArray())] = enc.GetString(atrValue.ToArray());//newChild.Attributes[enc.GetString(atr.ToArray())] = atrValue;
                            atrValue = new List<byte>();
                            atr = new List<byte>();
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else if (p == '\'' && Quote == HojichaAttributeValueQuote.SingleQuote)
                        {
                            //atrValueRead = false;
                            newChild.Attributes[enc.GetString(atr.ToArray())] = enc.GetString(atrValue.ToArray());
                            atrValue = new List<byte>();
                            atr = new List<byte>();
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else
                            if ((p == '\n' || p == '\r' || p == ' ' || p == '>') && Quote == HojichaAttributeValueQuote.NonQuote)
                            {
                                //atrValueRead = false;
                                newChild.Attributes[enc.GetString(atr.ToArray())] = enc.GetString(atrValue.ToArray());//newChild.Attributes[atr] = atrValue;
                                atrValue = new List<byte>();
                                atr = new List<byte>();
                                state = HojichaState.atrNameSearch; //atrNameSearch = true;
                            }
                            else
                            {
                                atrValue.Add((byte)p);//atrValue += p;
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
                                    atrValue.Add((byte)p);//atrValue += p;
                                }
                        }
                        break;

                    case HojichaState.nameRead:

                        if (/*p == '/' ||*/ p == '\n' || p == '\r' || p == ' ' || p == '>')
                        {
                            //nameRead = false;
                            //if(!newChild.Closed)
                            bool closed = newChild.Closed;
                            newChild = new HojichaHtmlNode(enc.GetString(name.ToArray()).ToLower());
                            newChild.Parent = hhn;
                            //debugcode
                            newChild.Closed = closed;
                            //if (parent == "") parent = newChild.Name;
                            state = HojichaState.atrNameSearch;//atrNameSearch = true;
                        }
                        else
                        {
                            if (p != '/') name.Add((byte)p);//name += p;
                            if (enc.GetString(name.ToArray()) == "!")
                            {
                                name = new List<byte>();
                                Text = new List<byte>(enc.GetBytes("<!"));
                                CommentType = false;
                                //isComment = true;
                                state = HojichaState.isComment;//nameRead = false;
                            }
                        }

                        break;
                }
                if (p == '/')
                {
                    if (name.Count==0 && Text.Count==0)
                    {
                        newChild.Closed = true; ClosedType = false;
                    }
                    //<img> / <div>hoge</div>で次のタグが正常に取得できないバグ修正
                    else if (Text.Count==0)
                    {
                        newChild.Closed = false; ClosedType = true;
                    }
                }
                if (state == HojichaState.isText && p != '<') Text.Add((byte)p);//Text += p;
                if (p == '<')
                {
                    start = i;
                    if (Text.Count != 0)
                    {
                        newChild = new HojichaHtmlNode("#text", enc.GetString(Text.ToArray()));
                        newChild.Parent = hhn;
                        hhn.ChildNodes.Add(newChild); newChild.setIndex();
                        Text = new List<byte>();

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

                    if (name.Count==0)
                    {
                        state = HojichaState.isText;//isText = true;
                        i++; hhn.Position = i; continue;
                    } //atrRead = false;
                    //atrNameSearch = false;
                    //atrValueNameRead = false;
                    state = HojichaState.isText;//atrValueRead = false;
                    //isText = true;
                    Text = new List<byte>();
                    name = new List<byte>();
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
                            }
                            else
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
                            //System.Diagnostics.Debug.Write(newChild.Parent.Name + "\t");
                            //System.Diagnostics.Debug.WriteLine(newChild.Name);
                            //親タグに合ったら帰れば解決？
                            //解決しない
                            //した
                            var parent = newChild.Parent;
                            while (parent != null)
                            {

                                if (parent.Name == newChild.Name)
                                {
                                    hhn.Position = start - 1;
                                    return hhn;
                                }
                                parent = parent.Parent;
                            }
                            newChild.Closed = false;
                            i++;
                            hhn.Position = i;
                            continue;
                        }
                        return hhn;
                    }
                    else
                    {

                        bool opt = false;
                        if (HojichaHtmlNode.ElementsFlags.ContainsKey(newChild.Name))
                            if ((HojichaHtmlNode.ElementsFlags[newChild.Name] & HojichaHtmlElementFlag.Empty) == HojichaHtmlElementFlag.Empty)
                            {
                                hhn.ChildNodes.Add(newChild); newChild.setIndex(); ClosedType = false;//Emptyで元に戻してなかった
                                //newChild.Html = _htmlb.Substring(start, i - start + 1);//TEST
                                i++; hhn.Position = i; continue;
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
                        //newChild.Html = _htmlb.Substring(start, i - start+1);//TEST
                        hhn.ChildNodes.Add(newChild);
                        newChild.setIndex();
                        HojichaHtmlNode newNode = new HojichaHtmlNode();
                        newChild.Parent = hhn;
                        //newChild.ChildNodes.Add(newNode);
                        /*newNode*/
                        newChild.Position = i + 1;
                        i = ParseHtmlByte(newChild/*,newChild.Name*/, opt).Position;
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
            if (Text.Count != 0)
            {
                newChild = new HojichaHtmlNode("#text", enc.GetString(Text.ToArray()));
                newChild.Parent = hhn;
                hhn.ChildNodes.Add(newChild); newChild.setIndex();
            }
            return hhn;
        }
    }
}
