#if HAP
using HtmlAgilityPack;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace otyanoko
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
#if HAP
        public HtmlNode Node;
#endif
#if Hojicha
        public HojichaHtmlNode Node;
#endif
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
    class UIRender
    {
        Render render = Core.render;
        /// <summary>
        /// 指定した既存のRenderを使用して、UIRender クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="rend">Render</param>
        public UIRender(Render rend)
        {
            this.render = rend;
        }
        /// <summary>
        /// 非選択状態のUIを描画します。
        /// </summary>
        /// <param name="ui">描画するUI</param>
        public void RenderUI(UI ui)
        {
            switch (ui.UIType)
            {
                case UIType.Button:
                    this.Button(ui);
                    break;
                case UIType.Link:
                    this.Link(ui);
                    break;
                case UIType.CheckBox:
                    this.CheckBox(ui);
                    break;
                case UIType.Select:
                    this.Select(ui);
                    break;
                case UIType.LineTextBox:
                    this.LineTextBox(ui);
                    break;
                case UIType.Image:
                    this.Image(ui);
                    break;
                case UIType.ButtonEx:
                    this.ButtonEx(ui);
                    break;
                case UIType.Meta:
                    this.Meta(ui);
                    break;
            }
        }
        /// <summary>
        /// 選択状態のUIを描画します。
        /// </summary>
        /// <param name="ui">描画するUI</param>
        public void RenderUISelect(UI ui)
        {
            switch (ui.UIType)
            {
                case UIType.Button:
                    this.SelectButton(ui);
                    break;
                case UIType.Link:
                    this.SelectLink(ui);
                    break;
                case UIType.CheckBox:
                    this.SelectCheckBox(ui);
                    break;
                case UIType.Select:
                    this.SelectSelect(ui);
                    break;
                case UIType.LineTextBox:
                    this.SelectLineTextBox(ui);
                    break;
                case UIType.Image:
                    this.SelectImage(ui);
                    break;
                case UIType.ButtonEx:
                    this.SelectButtonEx(ui);
                    break;
                case UIType.Meta:
                    this.SelectMeta(ui);
                    break;
            }
        }
        public void SelectButton(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.RenderButtonColor(ui.Text, Color.DarkBlue, Color.White);
        }
        public void Button(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.RenderButtonColor(ui.Text, RenderColor.DefaultForeColor, RenderColor.DefaultBackColor);
        }

        public void Link(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            render.RenderHtmlA(ui.Node.ChildNodes, ui.State);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public void SelectLink(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            render.RenderHtmlA(ui.Node.ChildNodes, ui.State);//getHtml(ui.Node.ChildNodes);
            //RenderColor.OldForegroundColor();
            render.Color.OldBackgroundColor();
        }
        public void CheckBox(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.RenderCheckBox(true);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void SelectCheckBox(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            render.Color.SetForegroundColor(Color.DarkBlue);
            render.RenderCheckBox(true);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void Select(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.RenderSelect(ui.Select.SelectList[ui.Select.Select], ui.Select.MaxLength, ui.Select.Length[ui.Select.Select]);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void SelectSelect(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            render.Color.SetForegroundColor(Color.DarkBlue);
            render.RenderSelect(ui.Select.SelectList[ui.Select.Select], ui.Select.MaxLength, ui.Select.Length[ui.Select.Select]);//Render.RenderSelect(ui.Select.SelectList[ui.Select.Select]);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void LineTextBox(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.RenderTextBox(ui.Value, ui.LineTextBox.Size);
            //HtmlNodeCollection hnc = new HtmlNodeCollection(ui.Node);
            //hnc.Add(ui.Node);
            //Render.RenderHtml(hnc, ui.State);
            render.Color.OldForegroundColor();
            render.Color.OldBackgroundColor();
        }
        public void SelectLineTextBox(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            render.Color.SetForegroundColor(ConsoleColor.DarkBlue);
            render.RenderTextBox(ui.Value, ui.LineTextBox.Size);
            //HtmlNodeCollection hnc = new HtmlNodeCollection(ui.Node);
            //hnc.Add(ui.Node);
            //Render.RenderHtml(hnc, ui.State);//getHtml(ui.Node.ChildNodes);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void Image(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.Write(ui.Text);
            render.Color.OldForegroundColor();
        }
        public void SelectImage(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.Color.Under(true);
            render.Write(ui.Text);
            render.Color.OldForegroundColor();
            render.Color.OldBackgroundColor();
            render.Color.Under(false);
        }
        public void ButtonEx(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetForegroundColor(RenderColor.DefaultForeColor);
            render.Color.SetBackgroundColor(RenderColor.DefaultBackColor);
            render.RenderHtml(ui.Node, ui.State);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public void SelectButtonEx(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            render.Color.SetForegroundColor(ConsoleColor.DarkBlue);
            render.RenderHtml(ui.Node, ui.State);
            render.Color.OldForegroundColor(); render.Color.OldBackgroundColor();
        }
        public void Meta(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            //render.Color.SetForegroundColor(ConsoleColor.DarkBlue);
            render.WriteLink(ui.Text);
            //RenderColor.OldForegroundColor(); RenderColor.OldBackgroundColor();
        }
        public void SelectMeta(UI ui)
        {
            render.CursorTop = ui.CursorTop;
            render.CursorLeft = ui.CursorLeft;
            render.Color.SetBackgroundColor(Color.White);
            //RenderColor.SetForegroundColor(ConsoleColor.DarkBlue);
            render.WriteLink(ui.Text);
            //RenderColor.OldForegroundColor();
            render.Color.OldBackgroundColor();
        }
    }
    static class UIProcess
    {
        static public Render render = Core.render;
        public static ConsoleKeyInfo TextBox(UI ui)
        {
            return new ConsoleKeyInfo();
        }
        public static string ReadLine()
        {
            return Console.ReadLine();
            string text = "";
            var Top = render.CursorTop;
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow)
                {
                    if (render.CursorLeft < text.Length) render.CursorLeft++;
                }
                else
                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        if(render.CursorLeft>0)render.CursorLeft--;
                    }
                    else
                    {
                        //if(Render.CursorLeft>0)
                            text=text.Insert(render.CursorLeft, key.KeyChar.ToString());
                        //if (Render.CursorLeft == 0)
                        //    text = key.KeyChar.ToString()+text;
                            render.CursorLeft++;
                        var b = render.CursorLeft;
                        render.CursorLeft = 0;
                        render.CursorTop = Top;
                        render.Write(text);
                        render.CursorLeft = b;
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
            var r = new RenderImage(new Bitmap(temp.ToUpper()), render);
            r.Render();
            Console.ReadKey(true);
            r.Dispose();
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
        public static void ScriptErrorBox(string script)
        {
            script = render.Replace(script);
            var len = Core.ScreenBuffSizeX / 3;
            int scale = Render.Scale(script);
            var scr = new string[scale / len + 1];
            for (int i = 0; i < scr.Length; i++)
            {
                try
                {
                    scr[i] = script.Substring(i * (Core.ScreenBuffSizeX / 3), Core.ScreenBuffSizeX / 3);
                }//あおおれｘ
                catch (ArgumentOutOfRangeException aoorex)
                {
                    scr[i] = script.Substring(i * (Core.ScreenBuffSizeX / 3));
                }

            }
            if (scr.Length == 0)
            {
                scr = new string[1] { script };
            }
            var rend = render.CopyToNewRender();
            rend.Color.BackgroundColor = ConsoleColor.Black;
            rend.Color.ForegroundColor = ConsoleColor.White;
            rend.CursorVisible = false;
            rend.CursorLeft = Core.ScreenBuffSizeX / 3;
            rend.CursorTop = Core.ScreenBuffSizeY / 4;
            rend.Write("+" + new string('-', Core.ScreenBuffSizeX / 3) + "+");
            var str = new string[2];
            str[0] = "Scriptを実行できません";
            str[1] = "実行できないScript:";
            int j = 0;
            for (int i = Core.ScreenBuffSizeY / 4; i < Core.ScreenBuffSizeY / 4 + 2 + scr.Length; i++)
            {

                rend.CursorLeft = Core.ScreenBuffSizeX / 3;
                rend.CursorTop++;
                if (j < str.Length)
                {
                    int length = Render.Scale(str[j]);
                    rend.WritePre("|" + str[j] + new string(' ', (Core.ScreenBuffSizeX / 3) - length) + "|");

                }
                else
                {
                    int length = Render.Scale(scr[j - 2]);
                    rend.WritePre("|" + scr[j - 2] + new string(' ', (Core.ScreenBuffSizeX / 3) - length) + "|");
                }
                j++;
            }
            rend.CursorLeft = Core.ScreenBuffSizeX / 3;
            rend.CursorTop++;
            rend.Write("+" + new string('-', Core.ScreenBuffSizeX / 3) + "+");
            rend.Active();
            rend.Color.Default();
            Console.ReadKey();
            render.Active();
            rend.CloseHandle();
        }
    }
}
