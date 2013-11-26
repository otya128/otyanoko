using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    [Serializable]
    class FormData
    {
        public string Action;
        public string Method;
        public string Enctype;
        public string Accept_Charset;
        public string Accept;
        public string Name;
        public string Target;
        public List<UI> UIList = new List<UI>();
        public List<int> UIIndexList = new List<int>();
    }
    [Serializable]
    class UI
    {
        public HtmlNode Node;
        public int CursorTop;
        public int CursorLeft;
        public int CursorTop2;
        public int CursorLeft2;
        public object Tag;
        public string Text;
        public UIType UIType;
        public StateClass State;
        public SelectUI Select;
        public LineTextBoxUI LineTextBox;
        public FormData Form;
        public string Value="";
        public UI Clone()
        {
            return (UI)MemberwiseClone();
        }
        /*public UI()
        {
            if (this.State != null) this.Form = this.State.Form;
        }*/

        public string Name="";
    }
    [Serializable]
    class SelectUI
    {
        public List<string> SelectList;
        public List<string> ValueList;
        public int Select = 0;
        public int MaxLength;
        public List<int> Length;
    }
    class LineTextBoxUI
    {
        public int Size = LineTextBoxUI.DefaultSize;
        public static int DefaultSize = 8;
    }
    enum UIType
    {
        Link = 1, Button = 2, CheckBox = 3, Select = 4, LineTextBox = 5,
        Image = 6, ButtonEx = 7, Meta = 8,
        Hidden = 0
    }
    static class UIRender
    {
        public static void RenderUI(UI ui)
        {
            switch (ui.UIType)
            {
                case UIType.Button:
                    UIRender.Button(ui);
                    break;
                case UIType.Link:
                    UIRender.Link(ui);
                    break;
                case UIType.CheckBox:
                    UIRender.CheckBox(ui);
                    break;
                case UIType.Select:
                    UIRender.Select(ui);
                    break;
                case UIType.LineTextBox:
                    UIRender.LineTextBox(ui);
                    break;
                case UIType.Image:
                    UIRender.Image(ui);
                    break;
                case UIType.ButtonEx:
                    UIRender.ButtonEx(ui);
                    break;
                case UIType.Meta:
                    UIRender.Meta(ui);
                    break;
            }
        }
        public static void RenderUISelect(UI ui)
        {
            switch (ui.UIType)
            {
                case UIType.Button:
                    UIRender.SelectButton(ui);
                    break;
                case UIType.Link:
                    UIRender.SelectLink(ui);
                    break;
                case UIType.CheckBox:
                    UIRender.SelectCheckBox(ui);
                    break;
                case UIType.Select:
                    UIRender.SelectSelect(ui);
                    break;
                case UIType.LineTextBox:
                    UIRender.SelectLineTextBox(ui);
                    break;
                case UIType.Image:
                    UIRender.SelectImage(ui);
                    break;
                case UIType.ButtonEx:
                    UIRender.SelectButtonEx(ui);
                    break;
                case UIType.Meta:
                    UIRender.SelectMeta(ui);
                    break;
            }
        }
        public static void SelectButton(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            Render.RenderButtonColor(ui.Text, Color.DarkBlue, Color.White);
        }
        public static void Button(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            Render.RenderButtonColor(ui.Text, RenderColor.DefaultForeColor, RenderColor.DefaultBackColor);
        }

        public static void Link(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.RenderHtmlA(ui.Node.ChildNodes, ui.State);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void SelectLink(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.RenderHtmlA(ui.Node.ChildNodes, ui.State);//getHtml(ui.Node.ChildNodes);
            //RenderColor.OldForegroundColor();
            RenderColor.OldBackgroundColor();
        }
        public static void CheckBox(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            Render.RenderCheckBox(true);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void SelectCheckBox(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            RenderColor.SetForegroundColor(Color.DarkBlue);
            Render.RenderCheckBox(true);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void Select(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            Render.RenderSelect(ui.Select.SelectList[ui.Select.Select], ui.Select.MaxLength, ui.Select.Length[ui.Select.Select]);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void SelectSelect(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            RenderColor.SetForegroundColor(Color.DarkBlue);
            Render.RenderSelect(ui.Select.SelectList[ui.Select.Select], ui.Select.MaxLength, ui.Select.Length[ui.Select.Select]);//Render.RenderSelect(ui.Select.SelectList[ui.Select.Select]);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void LineTextBox(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            Render.RenderTextBox(ui.Value, ui.LineTextBox.Size);
            //HtmlNodeCollection hnc = new HtmlNodeCollection(ui.Node);
            //hnc.Add(ui.Node);
            //Render.RenderHtml(hnc, ui.State);
            RenderColor.OldForegroundColor();
            RenderColor.OldBackgroundColor();
        }
        public static void SelectLineTextBox(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.RenderTextBox(ui.Value, ui.LineTextBox.Size);
            //HtmlNodeCollection hnc = new HtmlNodeCollection(ui.Node);
            //hnc.Add(ui.Node);
            //Render.RenderHtml(hnc, ui.State);//getHtml(ui.Node.ChildNodes);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void Image(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            Render.Write(ui.Text);
            RenderColor.OldForegroundColor();
        }
        public static void SelectImage(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            RenderColor.Under(true);
            Render.Write(ui.Text);
            RenderColor.OldForegroundColor();
            RenderColor.OldBackgroundColor();
            RenderColor.Under(false);
        }
        public static void ButtonEx(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetForegroundColor(RenderColor.DefaultForeColor);
            RenderColor.SetBackgroundColor(RenderColor.DefaultBackColor);
            Render.RenderHtml(ui.Node, ui.State);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void SelectButtonEx(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.RenderHtml(ui.Node, ui.State);
            RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void Meta(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.WriteLink(ui.Text);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public static void SelectMeta(UI ui)
        {
            Render.CursorTop = ui.CursorTop;
            Render.CursorLeft = ui.CursorLeft;
            RenderColor.SetBackgroundColor(Color.White);
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            Render.WriteLink(ui.Text);
            //RenderColor.OldForegroundColor();
            RenderColor.OldBackgroundColor();
        }
    }
    static class UIProcess
    {
        public static ConsoleKeyInfo TextBox(UI ui)
        {
            return new ConsoleKeyInfo();
        }
        public static string ReadLine()
        {
            return Console.ReadLine();
            string text = "";
            var Top = Render.CursorTop;
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (Render.CursorLeft < text.Length) Render.CursorLeft++;
                }
                else
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        if(Render.CursorLeft>0)Render.CursorLeft--;
                    }
                    else
                    {
                        //if(Render.CursorLeft>0)
                            text=text.Insert(Render.CursorLeft, key.KeyChar.ToString());
                        //if (Render.CursorLeft == 0)
                        //    text = key.KeyChar.ToString()+text;
                            Render.CursorLeft++;
                        var b = Render.CursorLeft;
                        Render.CursorLeft = 0;
                        Render.CursorTop = Top;
                        Render.Write(text);
                        Render.CursorLeft = b;
                        //Render.Write(key.KeyChar.ToString());
                    }
            }
            return "";
        }


        internal static void Image(UI ui)
        {
            string url;
            Connect.relativeUri(Core.url,System.Web.HttpUtility.HtmlDecode(ui.Value),out url);
            string temp = System.IO.Path.GetTempPath()+Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+Path.GetExtension(url);
            Connect.connect_file(url, temp);
            System.Diagnostics.Process p =
    System.Diagnostics.Process.Start(temp.ToUpper());
        }
        public static void OpenImage(string url,string MIMETYPE)
        {
            var mime = MIMETYPE.Split('/');
            string ext = "";
            switch (mime[1])
            {
                case "bmp":
                    ext = "jpeg";
                    break;
                case "cis-cod":
                    ext = "cod";
                    break;
                case "fif":
                    ext = "fif";
                    break;
                case "png":
                    ext = "png";
                    break;
                case "jpeg":
                    ext = "jpeg";
                    break;
            }
            string name = System.IO.Path.GetTempPath() + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "." + ext;

            Connect.connect_file(url, (name).ToUpper());
            //wc.DownloadFile(url, temp);
            System.Diagnostics.Process p =
    System.Diagnostics.Process.Start((name).ToUpper());
        }
        public static void OpenImage(string url)
        {
            string temp = System.IO.Path.GetTempPath() + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(url);
            //var wc = new WebClient();
            Connect.connect_file(url, temp.ToUpper());
            //wc.DownloadFile(url, temp);
            System.Diagnostics.Process p =
    System.Diagnostics.Process.Start(temp.ToUpper());
        }
    }
}
