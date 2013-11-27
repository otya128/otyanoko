using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Web;
using System.Runtime.InteropServices;
using System.Diagnostics;                // for Debug
using System.Drawing;
using System.Threading;
using ConsoleClassLibrary;
using System.Collections.Specialized;
using WIN32;
using System.Reflection;
using System.Drawing.Imaging;                    // for Color (add reference to  System.Drawing.assembly)
using System.Net.Mime;
namespace otyanoko
{
    //11/25
    /// <summary>
    /// インスタンスメソッド化した理由
    /// 複数のhScreenを扱う場合は退避しないといけないため
    /// </summary>
    class Program
    {
        static Render render = Core.render;
        /*[DllImport("kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(
           long dwDesiredAccess,
           int dwShareMode,
           IntPtr lpSecurityAttributes,
           int dwFlags,
           IntPtr lpScreenBufferData
        );*/
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);
        [DllImport("Kernel32.dll")]
        static extern IntPtr CreateConsoleScreenBuffer(
             UInt32 dwDesiredAccess,
             UInt32 dwShareMode,
             IntPtr secutiryAttributes,
             UInt32 flags,
             [MarshalAs(UnmanagedType.U4)] UInt32 screenBufferData
             );
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);
        static public IntPtr NULL = IntPtr.Zero;
        static bool Proccessing = false;
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        private const Int32 MF_BYPOSITION = 0x400;
        private const Int32 MF_REMOVE = 0x1000;
        public const int WM_MENUSELECT = 0x011F;

        unsafe static void Main(string[] args)
        {
            var a=new ContentType("text/html;charset=hogejhoege;URL=hoge");
            Debug.WriteLine(a.CharSet);
            Debug.WriteLine(a.MediaType);
            Debug.WriteLine(a.Parameters["URL"]);
            //ハンドル取得すると無効と言われる
            Console.ForegroundColor = RenderColor.DefaultForeColor;
            Console.BackgroundColor = RenderColor.DefaultBackColor;
            https://github.com/stealthinu/pukiwiki_spam_filter/blob/master/spam_filter.php
            http://www.cookcomputing.com/blog/archives/000556.html HTTPヘッダがおかしいと例外が出るのを阻止
            //Get the assembly that contains the internal class
            Assembly aNetAssembly = Assembly.GetAssembly(
              typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for 
                // the internal class
                Type aSettingsType = aNetAssembly.GetType(
                  "System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance 
                    // of the internal settings class. If the static instance 
                    // isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty
                      | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the 
                        // framework is unsafe header parsing should be 
                        // allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField(
                          "useUnsafeHeaderParsing",
                          BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);

                        }
                    }
                }
            }
            //stackoverflow対策
            int stackSize = 1024 * 1024 * 64;
            Thread th = new Thread(MainThread,
                stackSize);
            th.Name = "MainThread";
            th.Start();
            th.Join();
            //char[] bars = { '/', '-', '|', '|' };
            /*Console.Write("NowLoading..."); 
            while (true)
            {
                int i = 0;
                
                while (Proccessing)
                {
                    
                    //Console.Write(bars[i]);
                    Console.CursorLeft=14;
                    //var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                    //ConsoleFunctions.GetConsoleScreenBufferInfo(Render.StdHandle, out csbi);
                    //ConsoleFunctions.SetConsoleCursorPosition(Render.StdHandle, new ConsoleFunctions.COORD { X = 0, Y = (short)csbi.dwCursorPosition.Y }); ;//Console.SetCursorPosition(0, Console.CursorTop);
                    i = (i + 1) % 4;
                }
            }*/
            CloseHandle(render.hSrceen);
            render.hSrceen = NULL;

                //Console.WriteLine("aaAhfh\r\nahg");
                /*Render.ConsoleBackColorRGB(255, 255, 255);//SetConsoleColorRGB
                Console.Write("|");*/
                //Console.WriteLine("{0},{1},{2}", Color.Silver.R, Color.Silver.G, Color.Silver.B);
                //var buf = Console.OpenStandardOutput();
                //var wr = Console.OutputEncoding.GetBytes("testtestあうい");
                //buf.Write(wr,0,wr.Length);
                // Console.MoveBufferArea(0, 0, 1, 1, 5, 5);

            
        }

        static void MainThread()
        {
            render = Core.render;//Coreのrenderが先に代入されるとは限らない
            Debug.WriteLine(Environment.OSVersion.Version.Revision);
            Debug.WriteLine(Environment.Version.Major);
            Debug.WriteLine(Connect.UserAgent);
            //Console.S = null;
            var wc = new WebClient();
            Core.url = "http://chat.kanichat.com/mobile.jsp?roomid=petcwiki";//http://uni.2ch.net/test/read.cgi/kinoko/1373445002/";
            string html = "";

            html = Connect.connect_get(Core.url, "shift_jis");//wc.DownloadString(url);
            //Console.BufferHeight = Core.ScreenBuffSizeY;
            //Console.BufferWidth = Core.ScreenBuffSizeX;
        start:
            Proccessing = true;
            //Console.CursorVisible = true;
            ConsoleFunctions.SetConsoleActiveScreenBuffer(Render.StdHandle);
            //Console.ForegroundColor = RenderColor.DefaultForeColor;
            //Console.BackgroundColor = RenderColor.DefaultBackColor;
            render.Color.ForegroundColor = RenderColor.DefaultForeColor;
            render.Color.BackgroundColor = RenderColor.DefaultBackColor;
            //Console.Clear();
            //ConsoleFunctions.SetConsoleScreenBufferSize(Render.hSrceen, new ConsoleFunctions.COORD { X=80,Y=25});
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.hSrceen, out csbi);
            IntPtr tmphScreen;
            //if (csbi.dwSize.Y != 25)
            {
                tmphScreen = render.hSrceen;

                render.hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
                render.Color.ForegroundColor = RenderColor.DefaultForeColor;
                render.Color.BackgroundColor = RenderColor.DefaultBackColor;
                ConsoleFunctions.SetConsoleScreenBufferSize(render.hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
                
            
            } 
            int k = 0;
            /*while (true)
            {
                k++;
                if (k == 100) break;
                //Console.BackgroundColor = ConsoleColor.Green;
                RenderColor.SetForegroundColor(Color.Green);
                Render.Write(((char)k).ToString());
                RenderColor.OldForegroundColor();
                Render.Write("あ");
            } */
            //ConsoleFunctions.SetConsoleActiveScreenBuffer(Render.hSrceen);
            /*var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = 80, Y = 25 });
            Render.Copy(Render.hSrceen, hSrceen);
            ConsoleFunctions.SetConsoleActiveScreenBuffer(hSrceen);
            while (true) ;*/
            
            Core.UIList = new List<UI>();
            
            HtmlNode.ElementsFlags.Remove("form"); 
            HtmlDocument doc = new HtmlDocument();// HAP Objectを生成
            //Console.WriteLine("解析中");
            re:
            doc.LoadHtml(html);// HTMLパース
            
            var node = doc.DocumentNode.ChildNodes;
            //var tw = new StreamWriter(new MemoryStream(new byte[1024]),Console.OutputEncoding);
            //Console.SetOut(new StreamWriter("hige"));
            //Console.Out.Dispose();
            if (Core.ContentType.MediaType.IndexOf("image") != -1)
            {
                string temp = System.IO.Path.GetTempPath() + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(Core.url);
                //var wc = new WebClient();
                Connect.connect_file(Core.url, temp.ToUpper());
                var r = new RenderImage(new Bitmap(temp.ToUpper()),render);
                r.Render();
                Console.ReadKey(true);
                r.Dispose();
                /*var h=RenderImage.ImageRender(new Bitmap(temp.ToUpper()));
                Console.ReadKey(true);
                RenderImage.Dispose(h, Render.hSrceen);*/
            }
            else
            {
                try
                {
                    Core.getHtml(node);
                }
                catch (ReEncodingException ex)
                {
                    html = Encoding.GetEncoding(Core.encode).GetString(htmlb);
                    try
                    {
                        doc.LoadHtml(html);// HTMLパース

                        node = doc.DocumentNode.ChildNodes;
                        Core.getHtml(node);
                    }
                    catch (ReEncodingException)
                    {
                        Core.getHtml(node, new StateClass { EncodingChange = true });
                    }
                }
            }
                int index = 0;
            bool renderFlag = false;
            
            //Console.BufferHeight = Console.CursorTop;
            //Console.CursorVisible = false;
            ConsoleKeyInfo key=new ConsoleKeyInfo();
            //Console.BufferHeight= Render.CursorTop;
            //Console.Write(Render.console);
            ConsoleFunctions.SetConsoleActiveScreenBuffer(render.hSrceen);

            render.CursorVisible = false;
            Proccessing = false;

            CloseHandle(tmphScreen);
            tmphScreen = NULL;
            while (true)
            {
                
                //continue;
                if (Core.UIList.Count > index||index<0)
                {
                    //indexがおかしい状態でenter押すと例外が出る!!!
                }
                //選択状態なのを戻すために再描画するフラグ
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow) renderFlag = true; else renderFlag = false;
                if (renderFlag && index >= 0 && index < Core.UIList.Count)
                {
                    //非選択UIを描画
                    render.UI.RenderUI(Core.UIList[index]);
                }
                #region(key)
                if (key.Key == ConsoleKey.UpArrow && index > 0)
                {
                    index -= 1;
                }
                if (key.Key == ConsoleKey.DownArrow && index < Core.UIList.Count)
                {
                    index += 1;
                }
                if (key.Key == ConsoleKey.Escape) break;
                if (key.Key == ConsoleKey.Enter)
                {
                    //これをswitchにしたい
                    if (Core.UIList[index].UIType == UIType.Link || Core.UIList[index].UIType == UIType.Meta)
                    {

                        string u0 = System.Web.HttpUtility.HtmlDecode(Core.UIList[index].Value);
                        /*var u1 = new Uri(url);
                        var u2 = new Uri(u1, u0);
                        url = u2.ToString();*/
                        try
                        {
                            html = Connect.connect_get(Core.url, Core.encode, u0, out Core.url);//wc.DownloadString(url);
                            goto start;
                        }
                        catch (CancelException)
                        { }
                        catch (WebException ex)
                        {
                            html = Connect.connect_error(ex, Core.encode, out Core.url);//wc.DownloadString(url);
                            goto start;
                        }
                        
                    }
                    //Meta TODO:Linkと同じコードにしたい->した
                    /*if (Core.UIList[index].UIType == UIType.Meta)
                    {

                        string u0 = System.Web.HttpUtility.HtmlDecode(Core.UIList[index].Value);
                        try
                        {
                            html = Connect.connect_get(Core.url, Core.encode, u0, out Core.url);//wc.DownloadString(url);
                            goto start;
                        }
                        catch (CancelException)
                        { }
                        catch (WebException ex)
                        {
                            html = Connect.connect_error(ex, Core.encode, out Core.url);//wc.DownloadString(url);
                            goto start;
                        }

                    }*/
                    if (Core.UIList[index].UIType == UIType.Button || Core.UIList[index].UIType == UIType.ButtonEx)
                    {
                        var f = Core.UIList[index].Form;
                        if (f != null)
                        {
                            if (f.Method==""||f.Method.ToUpper() == "GET")
                            {
                                if (f.Enctype == "application/x-www-form-urlencoded")
                                {
                                    var nc = new NameValueCollection();
                                    foreach (var i in f.UIList)
                                    {
                                        nc.Add(i.Name, i.Value);
                                    }
                                    foreach (var i in f.UIIndexList)
                                    {
                                        if (
                                            (!String.IsNullOrEmpty(Core.UIList[i].Name) || !String.IsNullOrEmpty(Core.UIList[i].Value))
                                            && ((Core.UIList[i].UIType != UIType.Button && Core.UIList[i].UIType != UIType.ButtonEx)
                                            || i == index))
                                            nc.Add(Core.UIList[i].Name, Core.UIList[i].Value);
                                    }
                                    try
                                    {
                                        html = Connect.connect_get(Core.url, f.Accept_Charset, f.Action, out Core.url, nc);
                                        goto start;
                                    }
                                    catch (CancelException)
                                    { }
                                    catch (WebException ex)
                                    {
                                        html = Connect.connect_error(ex, f.Accept_Charset, out Core.url);//wc.DownloadString(url);
                                        goto start;
                                    }
                                }
                            }
                            if (f.Method.ToUpper() == "POST")
                            {
                                if (f.Enctype == "application/x-www-form-urlencoded")
                                {
                                    string data = "";
                                    List<byte> dataList = new List<byte>();
                                    foreach (var i in f.UIList)
                                    {
                                        dataList.AddRange(HttpUtility.UrlEncodeToBytes(Core.EncodeB(i.Name)));
                                        dataList.AddRange(Core.EncodeB("="));
                                        dataList.AddRange(HttpUtility.UrlEncodeToBytes(Core.EncodeB(i.Value)));
                                        dataList.AddRange(Core.EncodeB("&"));
                                        data +=
                                            HttpUtility.HtmlEncode(i.Name) + "=" + HttpUtility.HtmlEncode(i.Value) + "&";
                                    }

                                    foreach (var i in f.UIIndexList)
                                    {
                                        // try
                                        if (
                                           (!String.IsNullOrEmpty(Core.UIList[i].Name) || !String.IsNullOrEmpty(Core.UIList[i].Value))
                                           && (Core.UIList[i].UIType != UIType.Button || i == index))
                                        {
                                            dataList.AddRange(HttpUtility.UrlEncodeToBytes(Core.EncodeB(Core.UIList[i].Name)));
                                            dataList.AddRange(Core.EncodeB("="));
                                            dataList.AddRange(HttpUtility.UrlEncodeToBytes(Core.EncodeB(Core.UIList[i].Value)));
                                            dataList.AddRange(Core.EncodeB("&"));
                                            data +=
                                                HttpUtility.UrlEncodeToBytes(Core.EncodeB(Core.UIList[i].Name)) + "=" + HttpUtility.UrlEncodeToBytes(Core.EncodeB(Core.UIList[i].Value)) + "&";
                                        }
                                    }
                                    Debug.WriteLine(Encoding.UTF8.GetString(dataList.ToArray()));
                                    try
                                    {
                                        html = Connect.connect(Connect.relativeUri(Core.url, f.Action, out Core.url), f.Accept_Charset, f.Method, dataList.ToArray(), out Core.url);
                                        goto start;
                                    }
                                    catch (CancelException)
                                    { }
                                    catch (WebException ex)
                                    {
                                        html = Connect.connect_error(ex, f.Accept_Charset, out Core.url);//wc.DownloadString(url);
                                        goto start;
                                    }
                                }
                            }
                            //Console.Write(f);
                        }
                    }
                    if (Core.UIList[index].UIType == UIType.Select)
                    {
                        var oldFore = render.Color.ForegroundColorOld;
                        var oldBack = render.Color.BackgroundColorOld;
                        //var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
                        //ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
                        //Render.Copy(Render.hSrceen, hSrceen, (short)Render.ScrollY);//Render.Copy(Render.hSrceen, hSrceen, (short)Core.UIList[index].CursorTop);
                        var selectRender = new Render();

                        render.CursorTop = Core.UIList[index].CursorTop;
                        var hSrceen = render.CopyToNewBuff();
                        //var OldhSrceen = render.hSrceen;
                        selectRender.hSrceen = hSrceen;

                        selectRender.CursorVisible = false;
                        selectRender.Color.BackgroundColor_ = (ConsoleColor.Black);
                        selectRender.Color.ForegroundColor_ = (ConsoleColor.White);
                        int length = 0;
                        foreach (var i in Core.UIList[index].Select.SelectList)
                        {
                            if (length < Encoding.GetEncoding(932).GetBytes(i).Length) length = Encoding.GetEncoding(932).GetBytes(i).Length;
                        }

                        int select = Core.UIList[index].Select.Select;
                        selectRender.CursorLeft = Core.UIList[index].CursorLeft;
                        selectRender.CursorTop = 1;//Core.UIList[index].CursorTop;
                        selectRender.WritePre("+" + new string('-', length) + "+");

                        int count = 0;
                        foreach (var i in Core.UIList[index].Select.SelectList)
                        {
                            selectRender.CursorLeft = Core.UIList[index].CursorLeft;
                            selectRender.CursorTop++;
                            selectRender.WritePre("|");
                            if (count == select)
                            {
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.White);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.Black);
                            }
                            selectRender.WritePre(i + new string(' ', length - Encoding.GetEncoding(932).GetBytes(i).Length));
                            if (count == select)
                            {
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.Black);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.White);
                            }
                            selectRender.WritePre("|");
                            count++;
                        }

                        selectRender.CursorLeft = Core.UIList[index].CursorLeft;
                        selectRender.CursorTop++;
                        selectRender.WritePre("+" + new string('-', length) + "+");
                        selectRender.CursorTop = 2 + select;
                        ConsoleFunctions.SetConsoleActiveScreenBuffer(hSrceen);
                        while (true)
                        {


                            var key_ = Console.ReadKey(true);
                            if (key_.Key == ConsoleKey.Enter) break;
                            if (key_.Key == ConsoleKey.UpArrow && select > 0)
                            {
                                select--;
                                selectRender.CursorLeft = Core.UIList[index].CursorLeft + 1;
                                selectRender.CursorTop = 2 + select;
                                var i = Core.UIList[index].Select.SelectList[select];
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.White);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.Black);
                                selectRender.WritePre(i + new string(' ', length - Encoding.GetEncoding(932).GetBytes(i).Length));
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.Black);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.White);
                                selectRender.CursorLeft = Core.UIList[index].CursorLeft + 1;
                                selectRender.CursorTop++;
                                i = Core.UIList[index].Select.SelectList[select + 1];
                                selectRender.WritePre(i + new string(' ', length - Encoding.GetEncoding(932).GetBytes(i).Length));

                            } if (key_.Key == ConsoleKey.DownArrow && select + 1 < Core.UIList[index].Select.SelectList.Count)
                            {
                                select++;
                                selectRender.CursorLeft = Core.UIList[index].CursorLeft + 1;
                                selectRender.CursorTop = 2 + select;
                                var i = Core.UIList[index].Select.SelectList[select];
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.White);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.Black);
                                selectRender.WritePre(i + new string(' ', length - Encoding.GetEncoding(932).GetBytes(i).Length));
                                selectRender.Color.BackgroundColor_ = (ConsoleColor.Black);
                                selectRender.Color.ForegroundColor_ = (ConsoleColor.White);
                                selectRender.CursorLeft = Core.UIList[index].CursorLeft + 1;
                                selectRender.CursorTop--;
                                i = Core.UIList[index].Select.SelectList[select - 1];
                                selectRender.WritePre(i + new string(' ', length - Encoding.GetEncoding(932).GetBytes(i).Length));
                            }
                        }

                        Core.UIList[index].Select.Select = select;

                        render.Color.BackgroundColor = (RenderColor.DefaultBackColor);
                        render.Color.ForegroundColor = (RenderColor.DefaultForeColor);
                        //render.hSrceen = OldhSrceen;

                        ConsoleFunctions.SetConsoleActiveScreenBuffer(render.hSrceen);
                        Core.UIList[index].Value = Core.UIList[index].Select.ValueList[Core.UIList[index].Select.Select];
                        //RenderColor.OldForegroundColor();
                        //RenderColor.OldBackgroundColor();
                        //RenderColor.ForegroundColor = (ConsoleColor)oldFore;
                        //RenderColor.BackgroundColor = (ConsoleColor)oldBack;
                        //RenderColor.SetBackgroundColor();
                        //RenderColor.SetForegroundColor();
                        render.AutoScreenBuff = true;
                    }
                    if (Core.UIList[index].UIType == UIType.LineTextBox)
                    {

                        render.CursorTop = Core.UIList[index].CursorTop;
                        render.AutoScreenBuff = false;
                        /*var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
                        ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
                        Render.Copy(Render.hSrceen, hSrceen, (short)Render.ScrollY, 23, 1);*/
                        var hSrceen = render.CopyToNewBuff();
                        var OldhSrceen = render.hSrceen;
                        render.hSrceen = hSrceen;
                        render.Color.SetBackgroundColor(Color.Black);
                        render.Color.SetForegroundColor(Color.White);
                        render.CursorTop = Core.ScreenBuffSizeY-1;
                        render.WriteLine("Text:");
                        //Render.WriteLine(Core.UIList[index].Value);
                        render.CursorTop = Core.ScreenBuffSizeY;
                        render.CursorLeft = 0;
                        ConsoleFunctions.SetConsoleActiveScreenBuffer(hSrceen);
                        render.CursorVisible = true;
                        //UIProcess.ReadLine();
                        Core.UIList[index].Value = Console.ReadLine();
                        render.CursorVisible = false;
                        render.Color.BackgroundColor = (RenderColor.DefaultBackColor);
                        render.Color.ForegroundColor = (RenderColor.DefaultForeColor);
                        render.hSrceen = OldhSrceen;
                        ConsoleFunctions.SetConsoleActiveScreenBuffer(render.hSrceen);
                        render.AutoScreenBuff = true;
                    }
                    if (Core.UIList[index].UIType == UIType.Image)
                    {
                        UIProcess.Image(Core.UIList[index]);
                    }
                }
                if (key.Key == ConsoleKey.T)
                {
                    var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
                    ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
                    render.Copy(render.hSrceen, hSrceen);
                    var OldhSrceen = render.hSrceen;
                    render.hSrceen = hSrceen;
                    ConsoleFunctions.SetConsoleActiveScreenBuffer(hSrceen);
                    render.Color.SetBackgroundColor(Color.Black);
                    render.Color.SetForegroundColor(Color.White);
                    render.Write("URL:");
                    render.CursorVisible = true;
                    Console.ReadLine();
                    render.CursorVisible = false;
                    render.Color.OldForegroundColor();
                    render.Color.OldBackgroundColor();
                    render.hSrceen = OldhSrceen;
                    ConsoleFunctions.SetConsoleActiveScreenBuffer(render.hSrceen);
                    /* CloseHandle(Render.hSrceen);
                     Render.hSrceen = NULL;*/
                }
                #endregion
                if (index >= 0 && index < Core.UIList.Count)
                {
                    render.UI.RenderUISelect(Core.UIList[index]);
                }
#region
                /*if (key.Key!=0&&!(key.Key >= ConsoleKey.LeftArrow && key.Key <= ConsoleKey.DownArrow))
                {
                    var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
                    ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
                    Render.Copy(Render.hSrceen, hSrceen, (short)Render.CursorTop, 23, 1);
                    var OldhSrceen = Render.hSrceen;
                    Render.hSrceen = hSrceen;
                    RenderColor.SetBackgroundColor(Color.Black);
                    RenderColor.SetForegroundColor(Color.White);
                    Render.CursorTop = Core.ScreenBuffSizeY-1;
                    Render.WriteLine("Menu:");
                    //Render.WriteLine(Core.UIList[index].Value);
                    Render.CursorTop = Core.ScreenBuffSizeY;
                    Render.CursorLeft = 0;
                    ConsoleFunctions.SetConsoleActiveScreenBuffer(hSrceen);
                    
                        key = Console.ReadKey(true);
                    

                    Render.hSrceen = OldhSrceen;
                    RenderColor.BackgroundColor = (RenderColor.DefaultBackColor);
                    RenderColor.ForegroundColor = (RenderColor.DefaultForeColor);
                    ConsoleFunctions.SetConsoleActiveScreenBuffer(Render.hSrceen);
                    continue;
                }*/
#endregion
                int oldSY = render.ScrollY;
                try
                {
                    key = Console.ReadKey(true);
                }
                catch (InvalidOperationException ex)
                {
                    //Console.WriteLine(ex.Message);
                }
                //Debug.WriteLine(oldSY - Render.ScrollY);
                //Debug.WriteLine( Render.ScrollY-oldSY);
                if (Math.Abs(oldSY - render.ScrollY) < 10) continue;
                if (oldSY != render.ScrollY&&key.Key==ConsoleKey.DownArrow)
                {
                    int jindex = 0;
                    foreach (var i in Core.UIList)
                    {
                        if (i.CursorTop >= render.ScrollY)
                        {
                            render.UI.RenderUI(Core.UIList[index]);
                            index = jindex;
                            render.UI.RenderUISelect(Core.UIList[index]);
                            key = new ConsoleKeyInfo();
                            break;
                        }
                        jindex++;
                    }
                }
                if (oldSY != render.ScrollY && key.Key == ConsoleKey.UpArrow)
                {
                    int jndex = Core.UIList.Count;
                    Core.UIList.Reverse();
                    foreach (var i in Core.UIList)
                    {
                        jndex--;
                        if (i.CursorTop <= render.ScrollY)
                        {
                            Debug.WriteLine(Core.UIList[Core.UIList.Count - 1 - index].UIType);
                            int kndex = index;
                            index = jndex;
                            //UIRender.RenderUISelect(Core.UIList[index]);
                            render.UI.RenderUI(Core.UIList[Core.UIList.Count - 1 - kndex]);
                            Debug.WriteLine(Core.UIList[Core.UIList.Count - 1 - kndex].UIType);
                            key = new ConsoleKeyInfo();
                            break;
                        }
                    }
                    Core.UIList.Reverse();
                }
            }
            render.WriteLine();
            render.Write("URL:");
            render.CursorVisible = true;
            Core.url = Console.ReadLine();
            if (Core.url == "") return;
            render.Write("ENCODING:");
            string encode = Console.ReadLine();
            try
            {
                html = Connect.connect_get(Core.url, encode);
            }
            catch (WebException ex)
            {
                html = Connect.connect_error(ex, encode,out Core.url);
            }
            /*Stream st = wc.OpenRead(url);
            
            Encoding enc = Encoding.GetEncoding(encode);
            StreamReader sr = new StreamReader(st, enc);
            html = sr.ReadToEnd();
            sr.Close();

            st.Close();*/
            goto start;

        }
        //UnhandledExceptionイベントハンドラ
        static void CurrentDomain_UnhandledException(object sender,
            UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                //エラーメッセージを表示する
                Console.WriteLine("エラー: {0}", ex.Message);
            }
            finally
            {
                //アプリケーションを終了する
                //Environment.Exit(1);
            }
        }
        static public byte[] htmlb;
        
        

        
        
        
        
    }
    class StateClass
    {
        public bool None = false;
        public bool Select = false;
        public bool RenderA = false; 
        public bool A = false;
        public bool Pre = false;
        public bool List = false;
        public int ListMargin = 0;
        public int Margin = 0;
        public UI SelectUI;
        public UI ButtonUI;
        public FormData Form;
        public bool EncodingChange = false;
        public bool Button = false;
        public StateClass Clone()
        {
            var a = (StateClass)MemberwiseClone();
            if (this.SelectUI != null) a.SelectUI = this.SelectUI.Clone();
            if (this.ButtonUI != null) a.ButtonUI = this.ButtonUI.Clone();
            return a;
        }
    }
    [Flags]
    public enum State
    {
        None=1,Select=2,A=4,Pre=8
    }
    class SetScreenColorsApp
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COLORREF
        {
            internal uint ColorDWORD;

            internal COLORREF(System.Drawing.Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            internal COLORREF(uint r, uint g, uint b)
            {
                ColorDWORD = r + (g << 8) + (b << 16);
            }

            internal Color GetColor()
            {
                return Color.FromArgb((int)(0x000000FFU & ColorDWORD),
                                      (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
            }

            internal void SetColor(Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            internal int cbSize;
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal ushort wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            internal COLORREF black;
            internal COLORREF darkBlue;
            internal COLORREF darkGreen;
            internal COLORREF darkCyan;
            internal COLORREF darkRed;
            internal COLORREF darkMagenta;
            internal COLORREF darkYellow;
            internal COLORREF gray;
            internal COLORREF darkGray;
            internal COLORREF blue;
            internal COLORREF green;
            internal COLORREF cyan;
            internal COLORREF red;
            internal COLORREF magenta;
            internal COLORREF yellow;
            internal COLORREF white;
        }

        const int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        // Set a specific console color to an RGB color
        // The default console colors used are gray (foreground) and black (background)
        public static int SetColor(ConsoleColor consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        public static int SetColor(ConsoleColor color, uint r, uint g, uint b)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            if (hConsoleOutput == INVALID_HANDLE_VALUE)
            {
                return Marshal.GetLastWin32Error();
            }
            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }

            switch (color)
            {
                case ConsoleColor.Black:
                    csbe.black = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    csbe.darkBlue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    csbe.darkGreen = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    csbe.darkCyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    csbe.darkRed = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    csbe.darkMagenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    csbe.darkYellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    csbe.gray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    csbe.darkGray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    csbe.blue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    csbe.green = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    csbe.cyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    csbe.red = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    csbe.magenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    csbe.yellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    csbe.white = new COLORREF(r, g, b);
                    break;
            }
            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }

        public static int SetScreenColors(Color foregroundColor, Color backgroundColor)
        {
            int irc;
            irc = SetColor(ConsoleColor.Gray, foregroundColor);
            if (irc != 0) return irc;
            irc = SetColor(ConsoleColor.Black, backgroundColor);
            if (irc != 0) return irc;

            return 0;
        }
        public static int SetForeColors(Color foregroundColor)
        {
            int irc;
            /*irc = SetColor(ConsoleColor.Gray, Color.Gray);
            if (irc != 0) return irc;*/
            irc = SetColor(ConsoleColor.Black, foregroundColor);
            if (irc != 0) return irc;
            return 0;
        }
    }




}
