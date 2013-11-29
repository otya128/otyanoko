using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Net.Mime;
namespace otyanoko
{
    public class ReEncodingException : Exception
    {
    }
    class Core
    {
        public static Render render = new Render();
        static public List<UI> UIList;
        static public string encode = "utf-8";
        static public int ScreenBuffSizeX = 80;
        static public int ScreenBuffSizeY = 25;
                public static string url;
                public static ContentType ContentType = new ContentType("text/html");
        static public HtmlNodeCollection getHtml(HtmlNodeCollection node)
        {
            return getHtml(node, new StateClass());
        }

        static public HtmlNodeCollection getHtml(HtmlNodeCollection node, StateClass state, bool renderonly = false)// State nextstate = 0, int margin = 0)
        {
            int count = 0;
            ConsoleColor OldForeColor = render.Color.ForegroundColor;
            ConsoleColor OldBackColor = render.Color.BackgroundColor;
            bool backColor = false;
            bool textNone = false;
            foreach (var i in node)
            {
                
                if (i.Name == "?xml")
                {
                    if (i.GetAttributeValue("encoding", "") != "")
                    {
                        var enc = i.GetAttributeValue("encoding", "");
                        try
                        {
                            if (Encoding.GetEncoding(encode).CodePage != Encoding.GetEncoding(enc).CodePage)
                            {
                                encode = enc;
                                if(!state.EncodingChange)throw new ReEncodingException();
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            //encodingが不正
                        }
                    }
                }
                if (i.Name == "meta")
                {
                    if (i.GetAttributeValue("http-equiv", "") != "")
                    {
                        if (i.GetAttributeValue("content", "") != "")
                        {
                            var url = i.GetAttributeValue("content", "");
                            try
                            {
                                var content = new ContentType(url);
                                //var urls = i.GetAttributeValue("content", "").ToUpper();
                                if (content.Parameters.ContainsKey("URL"))
                                {
                                    if (!renderonly) UIList.Add(new UI
                                    {
                                        Node = i,
                                        CursorTop = render.CursorTop,
                                        CursorLeft = render.CursorLeft,
                                        UIType = UIType.Meta,
                                        State = state,
                                        Text = ("meta[" + content.Parameters["URL"] + "]"),
                                        Value = content.Parameters["URL"],
                                    });
                                    render.WriteLink("meta[" + content.Parameters["URL"] + "]");
                                }
                            }
                            catch
                            {
                                //IE=edgeなどでも例外
                                //指定されたコンテンツ タイプは無効です。
                            }
                        }
                        //FormatExceptionが大量に出てたのでhttp-equivがContentTypeの時に変更
                        if (i.GetAttributeValue("http-equiv", "").ToUpper() == "CONTENT-TYPE" ||
                            i.GetAttributeValue("http-equiv", "").ToUpper() == "CONTENTTYPE")//一応
                        {
                            var enc = i.GetAttributeValue("content", "");
                            try
                            {
                                var content = new ContentType(enc);
                                if (!String.IsNullOrEmpty(content.CharSet))
                                {
                                    var enc2 = content.CharSet;
                                    try
                                    {
                                        if (Encoding.GetEncoding(encode).CodePage != Encoding.GetEncoding(enc2).CodePage)
                                        {
                                            if (!state.EncodingChange) encode = enc2;
                                            throw new ReEncodingException();
                                        }
                                    }
                                    catch (ArgumentException) { }
                                }
                                //encodingが不正
                            }
                            catch (FormatException)//ReEncodingExceptionまでcatchしてた
                            {
                                //指定されたコンテンツ タイプは無効です。
                            }
                        }
                    }

                    if (i.GetAttributeValue("charset", "") != "")
                    {
                        var enc = i.GetAttributeValue("charset", "");
                        try
                        {
                            if (Encoding.GetEncoding(encode).CodePage != Encoding.GetEncoding(enc).CodePage)
                            {
                                if (!state.EncodingChange) encode = enc;
                                throw new ReEncodingException();
                            }
                        }
                        catch (ArgumentException)
                        {
                        }

                    }
                }
                //if(i.InnerHtml))
                //i.FirstChild
                //if (i.Name != "#text"&&i.ChildNodes != null) getHtml(i.ChildNodes);
                if (i.Name == "title")
                {
                    Console.Title = i.InnerText;
                    state.None = true;//nextstate |= State.None;
                }
                if (i.Name == "font")
                {
                    if (i.GetAttributeValue("color", "") != "")
                    {
                        OldForeColor = render.Color.ForegroundColor;
                        try
                        {
                            render.Color.ConsoleForeColorRGB(ColorTranslator.FromHtml(i.GetAttributeValue("color", "")));
                            if (render.Color.BackgroundColor == render.Color.ForegroundColor)
                            {
                                if ((int)render.Color.ForegroundColor == 0)
                                {
                                    render.Color.BackgroundColor = (ConsoleColor)15;
                                }
                                else
                                    if ((int)render.Color.ForegroundColor == 15)
                                    {
                                        render.Color.BackgroundColor = (ConsoleColor)0;
                                    }
                                    else
                                        render.Color.BackgroundColor = (ConsoleColor)(int)render.Color.ForegroundColor + 1;
                                backColor = true;
                            }
                        }catch
                        {
                            // blown は Int32 の有効な値ではありません。
                        }
                    }
                }
                #region(h)
                
                //switch化
                switch (i.Name)
                {
                    case "h1":
                        render.WriteLine();
                        render.WritePre(" ");
                        break;
                    case "h2":
                        render.WriteLine();
                        render.WritePre(" ");
                        break;
                    case "h3":
                        render.WriteLine();
                        render.WritePre(" ");
                        break;
                    case "h4":
                        render.WriteLine();
                        render.WritePre(" "); 
                        break;
                    case "h5":
                        render.WriteLine();
                        render.WritePre(" "); 
                        break;
                    case "h6":
                        render.WriteLine();
                        render.WritePre(" ");

                        break;
                }
                #endregion
                if (i.Name == "table")
                {

                }
                if (i.Name == "ul")
                {
                    state.ListMargin += 1;
                    state.Margin += 2;
                }
                if (i.Name == "li")
                {
                    if (i.InnerText != "")
                    {
                        render.WriteLine();
                        if (state.List)
                        {

                        }
                        render.WritePre(new string(' ', state.Margin) + "+");
                        state.List = true;
                    }
                }
                if (i.Name == "pre")
                {
                    state.Pre = true;//nextstate |= State.Pre;
                }
                if (i.Name == "a")
                {
                    if (i.GetAttributeValue("name", "") != "")
                    {
                        //<a name は#の指定に使う #は実装予定

                    }
                    else
                    {
                        if (i.InnerText != "")
                        {
                            state.A = true;//nextstate |= State.A;

                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorTop = render.CursorTop,
                                CursorLeft = render.CursorLeft,
                                UIType = UIType.Link,
                                Value = i.GetAttributeValue("href", ""),
                                State = state.Clone()
                            });
                        }
                    }
                    //Render.WriteLink(i.InnerText);
                }

                if (i.Name == "#text")
                {
                    textNone = true;
                    if (state.A) render.WriteLink(i.InnerText);
                    else
                        if (state.Pre)
                            render.WritePre(i.InnerText);
                        else
                            if (!state.None) render.Write(i.InnerText.Replace("\n", ""));

                    /*if (i.NextSibling != null)
                        if (i.NextSibling.Name == "title" || i.NextSibling.Name == "option" || i.NextSibling.Name == "a"
                            ) nextstate |= State.None;
                        else nextstate = nextstate&=(~State.None);*/

                }
                if (i.Name == "br")
                {
                    render.WriteLine();
                    if (state.Button) render.CursorLeft = state.ButtonUI.CursorLeft;
                    if(state.Margin>0)render.WritePre(new string(' ', state.Margin));
                }
                if (i.Name == "dd")
                {
                    state.Margin = 4;//margin = 4;
                    render.WriteLine();
                    if (state.Margin > 0) render.WritePre(new string(' ', state.Margin));
                }
                if (i.Name == "dt")
                {
                    //Render.WriteLine();
                }
                if (i.Name == "hr")
                {
                    render.WriteLine();
                    render.WriteLine("―――――――――――――――――――――――――――――――――――――――");
                }


                if (i.Name == "q")
                {
                    render.Write("\"");
                }
                if (i.Name == "img")
                {
                    string alt = i.GetAttributeValue("alt", "");
                    if (alt == "") alt = "[img]";
                    if (state.A) render.WriteLink(alt);
                    else
                    {
                        if (!renderonly)Core.UIList.Add(new UI
                        {
                            Node = i,
                            CursorLeft = render.CursorLeft,
                            CursorTop = render.CursorTop,
                            CursorLeft2 = render.CursorLeft,
                            CursorTop2 = render.CursorTop,
                            Text = alt,
                            Name = "",
                            Value = i.GetAttributeValue("src", ""),
                            UIType = UIType.Image,
                            State = state,
                            Form = state.Form,
                        });
                        render.Write(alt);
                    }
                }
                if (i.Name == "form")
                {
                    state.Form = new FormData
                    {
                        Accept = i.GetAttributeValue("accept", ""),
                        Action = i.GetAttributeValue("action", ""),
                        Accept_Charset = i.GetAttributeValue("accept-charset", ""),
                        Enctype = i.GetAttributeValue("enctype", ""),
                        Method = i.GetAttributeValue("method", ""),
                        Name = i.GetAttributeValue("name", ""),
                        Target = i.GetAttributeValue("target", ""),
                    };
                    if (state.Form.Accept_Charset == "")
                        state.Form.Accept_Charset = encode;
                    if (state.Form.Enctype == "")
                        state.Form.Enctype = DefaultEnctype;
                }
                #region(form)
                if (i.Name == "input")
                {

                    string type = i.GetAttributeValue("type", "");
                    switch (type)
                    {
                        case "hidden":
                            if (!renderonly && state.Form!=null) state.Form.UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = render.CursorLeft,
                                CursorTop = render.CursorTop,
                                CursorLeft2 = render.CursorLeft,
                                CursorTop2 = render.CursorTop,
                                Text = "",
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                                UIType = UIType.Hidden,
                                State = state,
                                Form = state.Form,
                            });
                            break;
                        case "text":

                            string size = i.GetAttributeValue("size", "");

                            if (size == null || size == "")
                            {
                                if (!renderonly && state.Form != null)
                                    state.Form.UIIndexList.Add(UIList.Count);
                                if (!renderonly) UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = render.CursorLeft,
                                    CursorTop = render.CursorTop,
                                    CursorLeft2 = render.CursorLeft,
                                    CursorTop2 = render.CursorTop,
                                    Text = size,
                                    UIType = UIType.LineTextBox,
                                    State = state,
                                    Form = state.Form,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                                //SetBackgroundColor(ConsoleColor.White);
                                render.Write("[________]");
                            }
                            else
                            {
                                try
                                {
                                    int siz = Convert.ToInt32(size);
                                    if (!renderonly && state.Form != null)
                                        state.Form.UIIndexList.Add(UIList.Count);
                                    if (!renderonly) UIList.Add(new UI
                                    {
                                        Node = i,
                                        CursorLeft = render.CursorLeft,
                                        CursorTop = render.CursorTop,
                                        CursorLeft2 = render.CursorLeft,
                                        CursorTop2 = render.CursorTop,
                                        Text = size,
                                        UIType = UIType.LineTextBox,
                                        State = state,
                                        Form = state.Form,
                                        LineTextBox = new LineTextBoxUI { Size = siz },
                                        Name = i.GetAttributeValue("name", "")
                                    });
                                    render.Write("[" + new string('_', siz) + "]");
                                }
                                catch { }
                            }
                            
                            //OldBackgroundColor();
                            break;
                        case "password":

                            size = i.GetAttributeValue("size", "");

                            if (size == null || size == "")
                            {
                                if (!renderonly && state.Form != null)
                                    state.Form.UIIndexList.Add(UIList.Count);
                                if (!renderonly) UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = render.CursorLeft,
                                    CursorTop = render.CursorTop,
                                    CursorLeft2 = render.CursorLeft,
                                    CursorTop2 = render.CursorTop,
                                    Text = size,
                                    UIType = UIType.LineTextBox,
                                    State = state,
                                    Form = state.Form,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                                //SetBackgroundColor(ConsoleColor.White);
                                render.Write("[________]");
                            }
                            else
                            {
                                try
                                {
                                    int siz = Convert.ToInt32(size);
                                    if (!renderonly && state.Form != null)
                                        state.Form.UIIndexList.Add(UIList.Count);
                                    if (!renderonly) UIList.Add(new UI
                                    {
                                        Node = i,
                                        CursorLeft = render.CursorLeft,
                                        CursorTop = render.CursorTop,
                                        CursorLeft2 = render.CursorLeft,
                                        CursorTop2 = render.CursorTop,
                                        Text = size,
                                        UIType = UIType.LineTextBox,
                                        State = state,
                                        Form = state.Form,
                                        LineTextBox = new LineTextBoxUI { Size = siz },
                                        Name = i.GetAttributeValue("name", "")
                                    });
                                    render.Write("[" + new string('_', siz) + "]");
                                }
                                catch { }
                            }

                            //OldBackgroundColor();
                            break;
                        case "isindex":
                            render.Write(i.GetAttributeValue("prompt", "") + "[________]");
                            break;
                        case "submit":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = render.CursorLeft,
                                CursorTop = render.CursorTop,
                                CursorLeft2 = render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = render.CursorTop,
                                Text = i.GetAttributeValue("value", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                            });
                            render.RenderButton(i.GetAttributeValue("value", ""));
                            break;
                        case "button":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = render.CursorLeft,
                                CursorTop = render.CursorTop,
                                CursorLeft2 = render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = render.CursorTop,
                                Text = i.GetAttributeValue("value", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                            });
                            render.RenderButton(i.GetAttributeValue("value", ""));
                            break;
                        case "":
                            //サイズ適用忘れてた20131127googleのtextboxが短かった原因

                            size = i.GetAttributeValue("size", "");

                            if (size == null || size == "")
                            {
                                if (!renderonly && state.Form != null)
                                    state.Form.UIIndexList.Add(UIList.Count);
                                if (!renderonly) UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = render.CursorLeft,
                                    CursorTop = render.CursorTop,
                                    CursorLeft2 = render.CursorLeft,
                                    CursorTop2 = render.CursorTop,
                                    Text = size,
                                    UIType = UIType.LineTextBox,
                                    State = state,
                                    Form = state.Form,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                                //SetBackgroundColor(ConsoleColor.White);
                                render.Write("[________]");
                            }
                            else
                            {
                                try
                                {
                                    int siz = Convert.ToInt32(size);
                                    if (!renderonly && state.Form != null)
                                        state.Form.UIIndexList.Add(UIList.Count);
                                    if (!renderonly) UIList.Add(new UI
                                    {
                                        Node = i,
                                        CursorLeft = render.CursorLeft,
                                        CursorTop = render.CursorTop,
                                        CursorLeft2 = render.CursorLeft,
                                        CursorTop2 = render.CursorTop,
                                        Text = size,
                                        UIType = UIType.LineTextBox,
                                        State = state,
                                        Form = state.Form,
                                        LineTextBox = new LineTextBoxUI { Size = siz },
                                        Name = i.GetAttributeValue("name", "")
                                    });
                                    render.Write("[" + new string('_', siz) + "]");
                                }
                                catch { }
                            }
                            break;
                        case "image":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = render.CursorLeft,
                                CursorTop = render.CursorTop,
                                CursorLeft2 = render.CursorLeft + i.GetAttributeValue("alt", "").Length,
                                CursorTop2 = render.CursorTop,
                                Text = i.GetAttributeValue("alt", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("alt", ""),
                            });
                            render.Write("[" + i.GetAttributeValue("alt", "") + "]");
                            break;
                        case "reset":
                            render.Write("[" + i.GetAttributeValue("value", "") + "]");
                            break;
                        case "checkbox":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly)
                                UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = render.CursorLeft,
                                    CursorTop = render.CursorTop,
                                    CursorLeft2 = render.CursorLeft,
                                    CursorTop2 = render.CursorTop,
                                    Text = "",
                                    State = state,
                                    UIType = UIType.CheckBox,
                                    Form = state.Form,
                                    Name = i.GetAttributeValue("name", "")
                                });
                            render.Write("[ ]");
                            break;
                        case "radio":
                            string a = i.GetAttributeValue("checked", "");
                            if (a == "checked")
                                render.Write("(x)");
                            else
                                render.Write("( )");
                            break;
                        case "file":
                            render.Write("[ファイルを選択]選択されていません");
                            break;
                    }
                }
                if (i.Name == "button")
                {
                    state.Button = true;
                    if (!renderonly && state.Form != null)
                        state.Form.UIIndexList.Add(UIList.Count);
                    if (!renderonly)
                    {
                        var ui = new UI
                            {
                                Node = i,
                                CursorLeft = render.CursorLeft,
                                CursorTop = render.CursorTop,
                                CursorLeft2 = render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = render.CursorTop,
                                Text = i.GetAttributeValue("value", ""),
                                UIType = UIType.ButtonEx,

                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                            };
                        state.ButtonUI = ui;
                        ui.State = state.Clone();
                        UIList.Add(ui);
                    }

                    render.Write("[");
                    if (string.IsNullOrEmpty(i.InnerText))//無い場合valueを表示
                        render.Write(i.GetAttributeValue("value", ""));
                }
                if (i.Name == "textarea")
                {
                    string rows = i.GetAttributeValue("rows", "");
                    string cols = i.GetAttributeValue("cols", "");
                    if (rows == null || cols == null
                        || rows == "" || cols == "")
                        render.Write("[________]");
                    else
                    {
                        try
                        {
                            int sizx = Convert.ToInt32(cols);
                            int sizy = Convert.ToInt32(rows);
                            int x = render.CursorLeft;
                            for (int siz = 0; siz < sizy; siz++)
                            {
                                render.CursorLeft = x;
                                render.WriteLine("|" + new string('_', sizx) + "|");
                            }
                        }
                        catch { }
                    }
                }
                if (i.Name == "select")
                {
                    state.Select = true;//nextstate |= State.Select;
                    state.None = true;//nextstate |= State.None;
                    state.SelectUI = new UI
                    {
                        Node = i,
                        CursorLeft = render.CursorLeft,
                        CursorTop = render.CursorTop,
                        CursorLeft2 = render.CursorLeft,
                        CursorTop2 = render.CursorTop,
                        Text = "",
                        State = state,
                        UIType = UIType.Select,
                        Select = new SelectUI
                        {
                            SelectList = new List<string>(),
                            ValueList = new List<string>(),
                            Length = new List<int>(),
                            Select = 0,
                            MaxLength = 0
                        },
                        Form = state.Form,
                        Name = i.GetAttributeValue("name", "")
                    };
                    if (!renderonly && state.Form != null)
                        state.Form.UIIndexList.Add(UIList.Count);
                    if (!renderonly)
                        UIList.Add(state.SelectUI);

                    //Render.Write("[ ]");

                }
                if (i.Name == "option" && state.Select)
                {
                    if (i.NextSibling != null)
                    {
                        //Render.Write(i.NextSibling.InnerText + " ");
                        state.SelectUI.Select.SelectList.Add(render.Replace(i.NextSibling.InnerText));
                        if (i.GetAttributeValue("value", "") != "") state.SelectUI.Select.ValueList.Add(i.GetAttributeValue("value", ""));
                        if (i.GetAttributeValue("value", "") == "") state.SelectUI.Select.ValueList.Add(i.NextSibling.InnerText);
                    }
                }
                #endregion
                if (i.Name == "script" || i.Name == "style")
                {
                    state.None = true;//nextstate |= State.None;
                }

                if (i.ChildNodes != null)if(i.ChildNodes.Count>0) getHtml(i.ChildNodes, state);
                if (i.Name == "script" || i.Name == "style" || i.Name == "title")
                {
                    state.None = false;//nextstate &= ~State.None;
                }
                if (i.Name == "a" && i.Closed)
                {
                    state.A = false;//nextstate &= ~State.A;
                    if (state.RenderA) return node;
                }
                if (i.Name == "pre" && i.Closed)
                {
                    state.Pre = false;//nextstate &= ~State.Pre;
                }
                if (i.Name == "dd" && i.Closed)
                {
                    state.Margin = 0;// margin = 0;
                }
                if (i.Name == "p" && i.Closed)
                {
                    render.WriteLine();
                }
                if (i.Name == "q" && i.Closed)
                {
                    render.Write("\"");
                }
                if (i.Name == "button" && i.Closed)
                {
                    render.Write("]");
                    if(!renderonly)state.ButtonUI = null;
                    state.Button = false;
                }
                if (i.Name == "select"&&i.Closed)
                {
                    if (state.SelectUI.Select.SelectList.Count == 0)
                    {
                        state.SelectUI.Select.SelectList.Add("");
                        state.SelectUI.Select.ValueList.Add("");
                    }
                    state.Select = false;//nextstate &= ~State.Select;
                    state.None = false;//nextstate &= ~State.None;
                    //int __I__ = 0;
                    foreach (var i_ in state.SelectUI.Select.SelectList)
                    {

                        var _i_ = Encoding.GetEncoding(932).GetBytes(i_).Length;
                        state.SelectUI.Select.Length.Add(_i_);
                        if (state.SelectUI.Select.MaxLength < _i_) state.SelectUI.Select.MaxLength = _i_;
                        //__I__++;
                    }
                    state.SelectUI.Value = state.SelectUI.Select.ValueList[0];
                    render.RenderSelect(state.SelectUI.Select.SelectList[state.SelectUI.Select.Select],
                        state.SelectUI.Select.MaxLength,
                        state.SelectUI.Select.Length[state.SelectUI.Select.Select]
                        );
                }
                if (i.Name == "font"&i.Closed)
                {
                    render.Color.ForegroundColor = OldForeColor;
                    if (backColor)
                    {
                        render.Color.BackgroundColor = OldBackColor;
                    }
                }
                if (i.Name == "ul")
                {
                    state.ListMargin -= 1;
                    state.Margin -= 2;
                }
                if (i.Name == "form" && i.Closed)
                {
                    state.Form = null;
                }
                if (i.Name == "tr" && i.Closed)
                {
                    render.WriteLine();
                }
                if (i.Name == "th" && i.Closed)
                {
                    render.WriteTab();
                }
                if (i.Name == "div" && i.Closed)
                {//divはspanと違って改行する
                    if(textNone)render.WriteLine();
                }
                #region(h)
                if (i.Name == "h1")
                {
                    render.WriteLine();
                }
                if (i.Name == "h2")
                {
                    render.WriteLine();
                }
                if (i.Name == "h3")
                {
                    render.WriteLine();
                }
                if (i.Name == "h4")
                {
                    render.WriteLine();
                }
                if (i.Name == "h5")
                {
                    render.WriteLine();
                }
                if (i.Name == "h6")
                {
                    render.WriteLine();
                }
                #endregion
                count++;

            }
            return node;
        }
        public static byte[] EncodeB(string arg)
        {
            return Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(encode), Encoding.UTF8.GetBytes(arg));
        }
        public static string EncodeS(string arg)
        {
            return Encoding.GetEncoding(encode).GetString(EncodeB(arg));
        }
        public static string DefaultEnctype = "application/x-www-form-urlencoded";
    }
}
