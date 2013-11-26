using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    public class ReEncodingException : Exception
    {
    }
    class Core
    {
        static public List<UI> UIList;
        static public string encode = "utf-8";
        static public int ScreenBuffSizeX = 80;
        static public int ScreenBuffSizeY = 25;
                public static string url;
                public static string ContentType = "text/html";
        static public HtmlNodeCollection getHtml(HtmlNodeCollection node)
        {
            return getHtml(node, new StateClass());
        }

        static public HtmlNodeCollection getHtml(HtmlNodeCollection node, StateClass state, bool renderonly = false)// State nextstate = 0, int margin = 0)
        {
            int count = 0;
            ConsoleColor OldForeColor = RenderColor.ForegroundColor;
            ConsoleColor OldBackColor = RenderColor.BackgroundColor;
            bool backColor = false;
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
                            var urls = i.GetAttributeValue("content", "").ToUpper();
                            if (url.ToUpper().IndexOf("URL=") != -1)
                            {
                                if (!renderonly) UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorTop = Render.CursorTop,
                                    CursorLeft = Render.CursorLeft,
                                    UIType = UIType.Meta,
                                    State = state,
                                    Text = ("meta[" + url.Substring(urls.IndexOf("URL=") + 4) + "]").ToUpper(),
                                    Value = url.Substring(urls.IndexOf("URL=") + 4),
                                });
                                Render.WriteLink("meta[" + url.Substring(urls.IndexOf("URL=") + 4) + "]");
                            }
                        }
                    }
                    if (i.GetAttributeValue("content", "") != "")
                    {
                        var enc = i.GetAttributeValue("content", "");
                        if (enc.IndexOf("charset=") != -1)
                        {
                            var enc2 = enc.Substring(enc.IndexOf("charset=") + 8);
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
                        OldForeColor = RenderColor.ForegroundColor;
                        RenderColor.ConsoleForeColorRGB(ColorTranslator.FromHtml(i.GetAttributeValue("color", "")));
                        if (RenderColor.BackgroundColor == RenderColor.ForegroundColor)
                        {
                            if ((int)RenderColor.ForegroundColor == 0)
                            {
                                RenderColor.BackgroundColor = (ConsoleColor)15;
                            }else
                            if ((int)RenderColor.ForegroundColor == 15)
                            {
                                RenderColor.BackgroundColor = (ConsoleColor)0;
                            }else
                                RenderColor.BackgroundColor = (ConsoleColor)(int)RenderColor.ForegroundColor+1;
                            backColor = true;
                        }
                    }
                }
                #region(h)
                if (i.Name == "h1")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
                }
                if (i.Name == "h2")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
                }
                if (i.Name == "h3")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
                }
                if (i.Name == "h4")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
                }
                if (i.Name == "h5")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
                }
                if (i.Name == "h6")
                {
                    Render.WriteLine();
                    Render.WritePre(" ");
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
                    Render.WriteLine();
                    if (state.List)
                    {

                    }
                    Render.WritePre(new string(' ', state.Margin) + "+");
                    state.List = true;
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
                        state.A = true;//nextstate |= State.A;

                        if (!renderonly) UIList.Add(new UI
                        {
                            Node = i,
                            CursorTop = Render.CursorTop,
                            CursorLeft = Render.CursorLeft,
                            UIType = UIType.Link,
                            State = state.Clone()
                        });
                    }
                    //Render.WriteLink(i.InnerText);
                }

                if (i.Name == "#text")
                {
                    if (state.A) Render.WriteLink(i.InnerText);
                    else
                        if (state.Pre)
                            Render.WritePre(i.InnerText);
                        else
                            if (!state.None) Render.Write(i.InnerText.Replace("\n", ""));

                    /*if (i.NextSibling != null)
                        if (i.NextSibling.Name == "title" || i.NextSibling.Name == "option" || i.NextSibling.Name == "a"
                            ) nextstate |= State.None;
                        else nextstate = nextstate&=(~State.None);*/

                }
                if (i.Name == "br")
                {
                    Render.WriteLine();
                    if (state.Button) Render.CursorLeft = state.ButtonUI.CursorLeft;
                    Render.WritePre(new string(' ', state.Margin));
                }
                if (i.Name == "dd")
                {
                    state.Margin = 4;//margin = 4;
                    Render.WriteLine();
                    Render.WritePre(new string(' ', state.Margin));
                }
                if (i.Name == "dt")
                {
                    //Render.WriteLine();
                }
                if (i.Name == "hr")
                {
                    Render.WriteLine();
                    Render.WriteLine("―――――――――――――――――――――――――――――――――――――――");
                }


                if (i.Name == "q")
                {
                    Render.Write("\"");
                }
                if (i.Name == "img")
                {
                    string alt = i.GetAttributeValue("alt", "");
                    if (alt == "") alt = "[img]";
                    if (state.A) Render.WriteLink(alt);
                    else
                    {
                        if (!renderonly)Core.UIList.Add(new UI
                        {
                            Node = i,
                            CursorLeft = Render.CursorLeft,
                            CursorTop = Render.CursorTop,
                            CursorLeft2 = Render.CursorLeft,
                            CursorTop2 = Render.CursorTop,
                            Text = alt,
                            Name = "",
                            Value = i.GetAttributeValue("src", ""),
                            UIType = UIType.Image,
                            State = state,
                            Form = state.Form,
                        });
                        Render.Write(alt);
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
                                CursorLeft = Render.CursorLeft,
                                CursorTop = Render.CursorTop,
                                CursorLeft2 = Render.CursorLeft,
                                CursorTop2 = Render.CursorTop,
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
                                    CursorLeft = Render.CursorLeft,
                                    CursorTop = Render.CursorTop,
                                    CursorLeft2 = Render.CursorLeft,
                                    CursorTop2 = Render.CursorTop,
                                    Text = size,
                                    UIType = UIType.LineTextBox,
                                    State = state,
                                    Form = state.Form,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                                //SetBackgroundColor(ConsoleColor.White);
                                Render.Write("[________]");
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
                                        CursorLeft = Render.CursorLeft,
                                        CursorTop = Render.CursorTop,
                                        CursorLeft2 = Render.CursorLeft,
                                        CursorTop2 = Render.CursorTop,
                                        Text = size,
                                        UIType = UIType.LineTextBox,
                                        State = state,
                                        Form = state.Form,
                                        LineTextBox = new LineTextBoxUI { Size = siz },
                                        Name = i.GetAttributeValue("name", "")
                                    });
                                    Render.Write("[" + new string('_', siz) + "]");
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
                                    CursorLeft = Render.CursorLeft,
                                    CursorTop = Render.CursorTop,
                                    CursorLeft2 = Render.CursorLeft,
                                    CursorTop2 = Render.CursorTop,
                                    Text = size,
                                    UIType = UIType.LineTextBox,
                                    State = state,
                                    Form = state.Form,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                                //SetBackgroundColor(ConsoleColor.White);
                                Render.Write("[________]");
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
                                        CursorLeft = Render.CursorLeft,
                                        CursorTop = Render.CursorTop,
                                        CursorLeft2 = Render.CursorLeft,
                                        CursorTop2 = Render.CursorTop,
                                        Text = size,
                                        UIType = UIType.LineTextBox,
                                        State = state,
                                        Form = state.Form,
                                        LineTextBox = new LineTextBoxUI { Size = siz },
                                        Name = i.GetAttributeValue("name", "")
                                    });
                                    Render.Write("[" + new string('_', siz) + "]");
                                }
                                catch { }
                            }

                            //OldBackgroundColor();
                            break;
                        case "isindex":
                            Render.Write(i.GetAttributeValue("prompt", "") + "[________]");
                            break;
                        case "submit":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = Render.CursorLeft,
                                CursorTop = Render.CursorTop,
                                CursorLeft2 = Render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = Render.CursorTop,
                                Text = i.GetAttributeValue("value", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                            });
                            Render.RenderButton(i.GetAttributeValue("value", ""));
                            break;
                        case "button":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = Render.CursorLeft,
                                CursorTop = Render.CursorTop,
                                CursorLeft2 = Render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = Render.CursorTop,
                                Text = i.GetAttributeValue("value", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("value", ""),
                            });
                            Render.RenderButton(i.GetAttributeValue("value", ""));
                            break;
                        case "":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly)
                                UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = Render.CursorLeft,
                                    CursorTop = Render.CursorTop,
                                    CursorLeft2 = Render.CursorLeft + 10,
                                    CursorTop2 = Render.CursorTop,
                                    Text = "",
                                    State = state,
                                    Form = state.Form,
                                    UIType = UIType.LineTextBox,
                                    LineTextBox = new LineTextBoxUI(),
                                    Name = i.GetAttributeValue("name", "")
                                });
                            Render.Write("[________]");
                            break;
                        case "image":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly) UIList.Add(new UI
                            {
                                Node = i,
                                CursorLeft = Render.CursorLeft,
                                CursorTop = Render.CursorTop,
                                CursorLeft2 = Render.CursorLeft + i.GetAttributeValue("alt", "").Length,
                                CursorTop2 = Render.CursorTop,
                                Text = i.GetAttributeValue("alt", ""),
                                UIType = UIType.Button,
                                State = state,
                                Form = state.Form,
                                Name = i.GetAttributeValue("name", ""),
                                Value = i.GetAttributeValue("alt", ""),
                            });
                            Render.Write("[" + i.GetAttributeValue("alt", "") + "]");
                            break;
                        case "reset":
                            Render.Write("[" + i.GetAttributeValue("value", "") + "]");
                            break;
                        case "checkbox":
                            if (!renderonly && state.Form != null)
                                state.Form.UIIndexList.Add(UIList.Count);
                            if (!renderonly)
                                UIList.Add(new UI
                                {
                                    Node = i,
                                    CursorLeft = Render.CursorLeft,
                                    CursorTop = Render.CursorTop,
                                    CursorLeft2 = Render.CursorLeft,
                                    CursorTop2 = Render.CursorTop,
                                    Text = "",
                                    State = state,
                                    UIType = UIType.CheckBox,
                                    Form = state.Form,
                                    Name = i.GetAttributeValue("name", "")
                                });
                            Render.Write("[ ]");
                            break;
                        case "radio":
                            string a = i.GetAttributeValue("checked", "");
                            if (a == "checked")
                                Render.Write("(x)");
                            else
                                Render.Write("( )");
                            break;
                        case "file":
                            Render.Write("[ファイルを選択]選択されていません");
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
                                CursorLeft = Render.CursorLeft,
                                CursorTop = Render.CursorTop,
                                CursorLeft2 = Render.CursorLeft + i.GetAttributeValue("value", "").Length,
                                CursorTop2 = Render.CursorTop,
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

                    Render.Write("[");
                }
                if (i.Name == "textarea")
                {
                    string rows = i.GetAttributeValue("rows", "");
                    string cols = i.GetAttributeValue("cols", "");
                    if (rows == null || cols == null
                        || rows == "" || cols == "")
                        Render.Write("[________]");
                    else
                    {
                        try
                        {
                            int sizx = Convert.ToInt32(cols);
                            int sizy = Convert.ToInt32(rows);
                            int x = Render.CursorLeft;
                            for (int siz = 0; siz < sizy; siz++)
                            {
                                Render.CursorLeft = x;
                                Render.WriteLine("|" + new string('_', sizx) + "|");
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
                        CursorLeft = Render.CursorLeft,
                        CursorTop = Render.CursorTop,
                        CursorLeft2 = Render.CursorLeft,
                        CursorTop2 = Render.CursorTop,
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
                        state.SelectUI.Select.SelectList.Add(Render.Replace(i.NextSibling.InnerText));
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
                    Render.WriteLine();
                }
                if (i.Name == "q" && i.Closed)
                {
                    Render.Write("\"");
                }
                if (i.Name == "button" && i.Closed)
                {
                    Render.Write("]");
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
                    Render.RenderSelect(state.SelectUI.Select.SelectList[state.SelectUI.Select.Select],
                        state.SelectUI.Select.MaxLength,
                        state.SelectUI.Select.Length[state.SelectUI.Select.Select]
                        );
                }
                if (i.Name == "font"&i.Closed)
                {
                    RenderColor.ForegroundColor = OldForeColor;
                    if (backColor)
                    {
                        RenderColor.BackgroundColor = OldBackColor;
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
                    Render.WriteLine();
                }
                if (i.Name == "th" && i.Closed)
                {
                    Render.WriteTab();
                }
                #region(h)
                if (i.Name == "h1")
                {
                    Render.WriteLine();
                }
                if (i.Name == "h2")
                {
                    Render.WriteLine();
                }
                if (i.Name == "h3")
                {
                    Render.WriteLine();
                }
                if (i.Name == "h4")
                {
                    Render.WriteLine();
                }
                if (i.Name == "h5")
                {
                    Render.WriteLine();
                }
                if (i.Name == "h6")
                {
                    Render.WriteLine();
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
