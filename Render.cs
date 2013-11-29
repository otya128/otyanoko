using ConsoleClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WIN32;

namespace otyanoko
{
    class Render
    {
        public RenderColor Color;
        public UIRender UI;
        public Render()
        {
            this.Color = new RenderColor(this);
            this.UI = new UIRender(this);
        }
        /// <summary>
        /// 指定したコンソールハンドルを使用して、Render クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="Handle1">コンソールハンドル</param>
        public Render(IntPtr Handle1)
        {
            // TODO: Complete member initialization
            this.Handle = Handle1;
            this.Color = new RenderColor(this);
            this.UI    = new UIRender(this);
        }
        /// <summary>
        /// コンソールハンドルを初期化します。
        /// </summary>
        public Render CreateHandle()
        {
            this.Handle = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(this.Handle, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
            return this;
        }
        /// <summary>
        /// バッファサイズを指定して、コンソールハンドルを初期化します。
        /// </summary>
        public Render CreateHandle(int x,int y)
        {
            this.Handle = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(this.Handle, new ConsoleFunctions.COORD { X = (short)x, Y = (short)y });
            return this;
        }
        /// <summary>
        /// このRenderを表示します。
        /// </summary>
        public bool Active()
        {
            return ConsoleFunctions.SetConsoleActiveScreenBuffer(this.Handle);
        }
        /// <summary>
        /// このRenderのハンドルを閉じます。
        /// </summary>
        public void CloseHandle()
        {
            kernel32.CloseHandle(this.Handle);
            this.Handle = NULL;
        }
        /// <summary>
        /// バッファー領域の高さを取得または設定します。
        /// </summary>
        public int BufferHeight
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.dwSize.Y;
            }
            set
            {
                ConsoleFunctions.SetConsoleScreenBufferSize(Handle, new ConsoleFunctions.COORD { X = (short)BufferWidth, Y = (short)value });
            }
        }
        /// <summary>
        /// バッファー領域の幅を取得または設定します。
        /// </summary>
        public int BufferWidth
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.dwSize.X;
            }
            set
            {
                ConsoleFunctions.SetConsoleScreenBufferSize(Handle, new ConsoleFunctions.COORD { X = (short)value, Y = (short)BufferHeight });
            }
        }
        /// TODO:SetConsoleWindowInfo使ってスクロールできるらしい
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms685118(v=vs.85).aspx
        /// <summary>
        /// コンソール ウィンドウ領域の高さを取得または設定します。
        /// </summary>
        public int WindowHeight
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.srWindow.Bottom;
            }
            set
            {
                //Console.WindowHeight = value;
                //return;
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                /*csbi.srWindow.Bottom=-40;
    csbi.srWindow.Left = -40;
    csbi.srWindow.Right = 00;*/
                csbi.srWindow.Top = 0;// (short)value;
                csbi.srWindow.Bottom = (short)value;
                //csbi.srWindow.Top = (short)value;//X2
                //csbi.srWindow.Right += 20;//Y2
                //csbi.srWindow.Top -= 5;//X1
                //csbi.srWindow.Left += 20;//Y1
                // = new ConsoleFunctions.SMALL_RECT { Bottom = 0, Left = 0, Right = 30, Top = 30 };//(short)value;
                Debug.WriteLine(ConsoleFunctions.SetConsoleWindowInfo(Handle, true, ref csbi.srWindow));
            }
        }
        /// <summary>
        /// コンソール ウィンドウ領域の左端の位置を、画面バッファーに対する相対位置として取得または設定します。
        /// </summary>
        [Obsolete("未実装")]
        public int WindowLeft { get; set; }
        /// <summary>
        /// コンソール ウィンドウ領域の上端の位置を、画面バッファーに対する相対位置として取得または設定します。
        /// </summary>
        [Obsolete("未実装")]
        public int WindowTop { get; set; }
        /// <summary>
        /// コンソール ウィンドウの幅を取得または設定します。
        /// </summary>
        public int WindowWidth
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.srWindow.Right;
            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                csbi.srWindow.Left = 0;
                csbi.srWindow.Right = (short)value;
                Debug.WriteLine(ConsoleFunctions.SetConsoleWindowInfo(Handle, true, ref csbi.srWindow));
            }
        }
        /// <summary>
        /// バッファー領域におけるカーソルの列位置を取得または設定します。
        /// </summary>
        public int CursorLeft
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.dwCursorPosition.X;
            }
            set
            {
                ConsoleFunctions.SetConsoleCursorPosition(Handle, new ConsoleFunctions.COORD { X = (short)value, Y = (short)CursorTop });
            }
        }
        /// <summary>
        /// バッファー領域におけるカーソルの行位置を取得または設定します。
        /// </summary>
        public int CursorTop
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);
                return csbi.dwCursorPosition.Y;
            }
            set
            {
                ConsoleFunctions.SetConsoleCursorPosition(Handle, new ConsoleFunctions.COORD { X = (short)CursorLeft, Y = (short)value });
            }
        }
        /// <summary>
        /// カーソルを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        public bool CursorVisible
        {
            get
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO();
                ConsoleFunctions.GetConsoleCursorInfo(Handle, out cci);
                return cci.Visible;
            }
            set
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO { Size = CursorSize, Visible = value };
                ConsoleFunctions.SetConsoleCursorInfo(Handle, ref cci);
            }
        }
        /// <summary>
        /// 文字セル内のカーソルの高さを取得または設定します。
        /// </summary>
        public uint CursorSize
        {
            get
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO();
                ConsoleFunctions.GetConsoleCursorInfo(Handle, out cci);
                return cci.Size;
            }
            set
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO { Size = value, Visible = CursorVisible };
                ConsoleFunctions.SetConsoleCursorInfo(Handle, ref cci);
            }
        }
        /// <summary>
        /// コンソール ウィンドウの縦スクロールバーの位置を取得または設定します。
        /// </summary>
        public int ScrollY
        {
            get
            {
                var hWnd = ConsoleFunctions.GetConsoleWindow();
                return user32.GetScrollPos(hWnd, System.Windows.Forms.Orientation.Vertical);
            }
            set
            {
                var hWnd = ConsoleFunctions.GetConsoleWindow();
                user32.SetScrollPos(hWnd, System.Windows.Forms.Orientation.Vertical, value, true);
            }
        }
        /// <summary>
        /// コンソール ウィンドウの横スクロールバーの位置を取得または設定します。
        /// </summary>
        public int ScrollX
        {
            get
            {
                var hWnd = ConsoleFunctions.GetConsoleWindow();
                return user32.GetScrollPos(hWnd, System.Windows.Forms.Orientation.Horizontal);
            }
            set
            {
                var hWnd = ConsoleFunctions.GetConsoleWindow();
                user32.SetScrollPos(hWnd, System.Windows.Forms.Orientation.Horizontal, value, true);
            }
        }
        /// <summary>
        /// 自動的にコンソールバッファを拡張するかどうかを示す値を設定します。
        /// </summary>
        public bool AutoScreenBuff = true;
        /// <summary>
        /// 描画対象のコンソールハンドル
        /// </summary>
        public IntPtr Handle;
        public static IntPtr StdHandle;
        public static IntPtr NULL = IntPtr.Zero;
        /// <summary>
        /// 指定したHtmlNodeCollectionとStateClassを利用してHtmlを描画します。
        /// </summary>
        /// <param name="htmlNodeCollection"></param>
        /// <param name="state"></param>
        internal void RenderHtml(HtmlAgilityPack.HtmlNodeCollection htmlNodeCollection,StateClass state)
        {
            Core.getHtml(htmlNodeCollection, state,true);
        }
        /// <summary>
        /// 指定したHtmlNodeとStateClassを利用してHtmlを描画します。
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="state"></param>
        internal void RenderHtml(HtmlAgilityPack.HtmlNode htmlNode, StateClass state)
        {
            var hc = new HtmlAgilityPack.HtmlNodeCollection(htmlNode);
            hc.Add(htmlNode);
            Core.getHtml(hc, state, true);
        }
        /// <summary>
        /// 適切にAタグを描画できるように設定し、指定したHtmlNodeとStateClassを利用してHtmlを描画します。
        /// </summary>
        /// <param name="htmlNodeCollection"></param>
        /// <param name="state"></param>
        internal void RenderHtmlA(HtmlAgilityPack.HtmlNodeCollection htmlNodeCollection, StateClass state)
        {
            var ss = state.Clone();
            ss.RenderA = true;
            Core.getHtml(htmlNodeCollection, ss, true);
        }

        public void RenderButton(string text)
        {
            this.Write("[" + text + "]");
        }
        public void RenderButtonColor(string text, ConsoleColor Fore, ConsoleColor Back)
        {
            this.Color.SetBackgroundColor(Back);
            this.Color.SetForegroundColor(Fore);
            RenderButton(text);
            this.Color.OldBackgroundColor();
            this.Color.OldForegroundColor();
        }
        public void RenderButtonColor(string text, Color Fore, Color Back)
        {
            this.Color.SetBackgroundColor(Back);
            this.Color.SetForegroundColor(Fore);
            RenderButton(text);
            this.Color.OldBackgroundColor();
            this.Color.OldForegroundColor();
        }

        public void RenderInput(int size = 8)
        {
            this.WritePre("[" + new string('_', size) + "]");
        }
        public void RenderCheckBox(bool cheked)
        {
            this.WritePre("[" + (cheked ? "x" : " ") + "]");
        }
        public void RenderSelect(string item)
        {
            this.WritePre("[" + item + "]");
        }
        public void RenderSelect(string item, int max,int min)
        {
            if (max < min)
                this.WritePre("[" + item.Substring(0, max - 1) + ">]");
            else
                this.WritePre("[" + item + new string(' ', max - min) + "]");
        }
        public void RenderTextBox(string item, int max)
        {
            byte[] ary1 = Encoding.GetEncoding(932).GetBytes(item);
            int len = ary1.Length;
            if (max < len)
            {
                byte[] ary=new byte[max-1];
                Array.Copy(ary1, ary, max - 1);
                int cx = CursorLeft;
                this.WritePre("[" + Encoding.GetEncoding(932).GetString(ary));
                CursorLeft = cx + max;
                this.WritePre(">]");
            }
            else
                this.WritePre("[" + item + new string('_', max - len) + "]");
        }
        public void RenderTextBox()
        {
            this.WritePre("[" + new string('_', LineTextBoxUI.DefaultSize) + "]");
        }
        public void RenderTextBox(int siz)
        {
            this.WritePre("[" + new string('_', siz) + "]");
        }
        public int cursorTop = 0, cursorLeft = 0;
        public string Replace(string arg)
        {
            arg = HttpUtility.HtmlDecode(arg.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("  ", "").Replace("  ", "").Replace("&nbsp;", " ").Replace("&ensp;", " ").Replace("&emsp;", " ").Replace("&thinsp;", " "));
#if TEST
            cursorLeft+=Encoding.GetEncoding(932).GetBytes(arg).Length;

            if (cursorLeft > Console.BufferWidth)
            {
                cursorTop += (int)Math.Round((double)cursorLeft / (double)Console.BufferWidth);
                cursorLeft = cursorLeft % Console.BufferWidth;
            }
#endif
            return arg;
        }
        public string ReplacePre(string arg)
        {
            return HttpUtility.HtmlDecode(arg.Replace("\t", "").Replace("&nbsp;"," "));
        }
        public StringBuilder console=new StringBuilder();
        public void Scroll(string arg)
        {
            if (AutoScreenBuff)
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(Handle, out csbi);

                if (csbi.dwSize.Y < csbi.dwCursorPosition.Y + (Encoding.GetEncoding(932).GetBytes(arg).Length / Core.ScreenBuffSizeY) + 20)
                {
                    csbi.dwSize.Y *= 2;
                    ConsoleFunctions.SetConsoleScreenBufferSize(Handle, csbi.dwSize);
                }
            }
            return;
        }
        public void Write(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg));
            uint cell;
            arg = Replace(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.Write(Replace(arg));
        }
        public void WritePre(string arg)
        {
            Scroll(arg);
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg)); 
            uint cell;
            arg = ReplacePre(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.Write(ReplacePre(arg));
        }
        public void WriteLink(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            var c = this.Color.ForegroundColor;
            this.Color.ForegroundColor = ConsoleColor.DarkBlue;
            this.Color.Under(true);
            //console.Append(Replace(arg)); 
            //Console.Write(Replace(arg));
            uint cell;
            arg = Replace(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            this.Color.Under(false);
            this.Color.ForegroundColor = c;
            
        }
        public void WriteLine(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(Replace(arg)); 
            //Console.WriteLine(arg.Replace("\n", ""));
            uint cell;
            arg = Replace(arg)+"\n";
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        public void WriteLine()
        {
            //CursorTop++; CursorLeft = 0;
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(); 
            uint cell;
            string arg = "\n";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.WriteLine();
        }
        public void WriteTab()
        {
            uint cell;

            string arg = "\t";//new String(' ', CursorLeft % 3);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        public void WriteTab(int margin)
        {
            uint cell;

            string arg = new String(' ', CursorLeft % margin);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        unsafe public bool Copy(IntPtr Handle1, IntPtr Handle, short cursorTop_ = -1, short size = -1)
        {
            if (cursorTop_ == -1) cursorTop_ = 0;
            if (size == -1) size = Convert.ToInt16(Core.ScreenBuffSizeY - 1);
            Copy(Handle1, Handle, cursorTop_, size, 0);//if (Top == -1) Top = 0;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Handle1">コピー元</param>
        /// <param name="Handle">コピー先</param>
        /// <returns></returns>Core.ScreenBuffSizeX
        unsafe public bool Copy(IntPtr Handle1, IntPtr Handle, short cursorTop_, short size, short Top)
        {
            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[Core.ScreenBuffSizeX * size];
            var sm = new ConsoleFunctions.SMALL_RECT { Top = cursorTop_, Left = 0, Bottom = Convert.ToInt16(Convert.ToInt16(size) + cursorTop_), Right = (short)Core.ScreenBuffSizeX };
            ConsoleFunctions.ReadConsoleOutput(Handle1,
                ci,
                new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = size },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm
                );
            sm = new ConsoleFunctions.SMALL_RECT { Top = Top, Left = 0, Bottom = Convert.ToInt16(size), Right = (short)Core.ScreenBuffSizeX };
            //sm = new ConsoleFunctions.SMALL_RECT { Top = cursorTop_, Left = 0, Bottom = Convert.ToInt16(Convert.ToInt16(24) + cursorTop_), Right = 80 };
           
            fixed (ConsoleFunctions.CHAR_INFO* p = ci)

            ConsoleFunctions.WriteConsoleOutput(Handle, 
                new IntPtr(p),
                new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = size },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm);
            ConsoleFunctions.SetConsoleCursorPosition(Handle, new ConsoleFunctions.COORD { X = (short)0, Y = (short)cursorTop_ });
            /*for (int j = 0; j < ci.Length; j++)
            {
                var i = ci[j];
                ConsoleFunctions.SetConsoleTextAttribute(hSrceen, i.Attributes);
                if (i.AsciiChar >= 0x81 && i.AsciiChar <= 0xA0)
                {
                    j++;
                    //if (j < ci.Length) ; else break;
                    byte[] sjis = new byte[] { (byte)i.AsciiChar, (byte)ci[j].AsciiChar };
                    uint cell;
                    ConsoleFunctions.WriteConsole(hSrceen, Encoding.GetEncoding(932).GetString(sjis), Convert.ToUInt32(sjis.Length), out cell, NULL);
                    //Render.Write(Encoding.GetEncoding(932).GetString(sjis));
                }
                else
                {
                    uint cell;
                    ConsoleFunctions.WriteConsole(hSrceen, Encoding.GetEncoding(932).GetString(new byte[1] { i.AsciiChar }), 1, out cell, NULL);
                    //Render.Write(i.UnicodeChar.ToString());
                }//j++;
            }*/
            
            return true;
        }
        [Obsolete("use method CopyToNewRender")]
        public IntPtr CopyToNewBuff()
        {
            var Handle = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(Handle, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
            this.Copy(this.Handle, Handle, (short)this.ScrollY, 25, 0);
            return Handle;
        }
        /// <summary>
        /// このRenderの現在表示されている部分をコピーして、新しいRenderインスタンスを生成します。
        /// </summary>
        /// <returns>このメソッドが作成する新しい Render</returns>
        public Render CopyToNewRender()
        {
            var Handle = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(Handle, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
            this.Copy(this.Handle, Handle, (short)this.ScrollY, 25, 0);
            return new Render(Handle);
        }
        /// <summary>
        /// このRenderの指定部分をコピーして、新しいRenderインスタンスを生成します。
        /// </summary>
        /// <param name="Handle"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        unsafe public bool CopyTo(IntPtr Handle, int x1, int y1, int x2, int y2)
        {
            short x1s = (short)x1, y1s = (short)y1, x2s = (short)x2, y2s = (short)y2;
            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[(x2 - x1) * (y2 - y1)];
            
            var sm = new ConsoleFunctions.SMALL_RECT { Top = y1s, Left = x1s, Bottom = y2s, Right = x2s };
            ConsoleFunctions.ReadConsoleOutput(this.Handle,
                ci,
                new ConsoleFunctions.COORD { X = (short)(x2 - x1), Y = (short)(y2 - y1) },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm
                );
            sm = new ConsoleFunctions.SMALL_RECT { Top = y1s, Left = x1s, Bottom = y2s, Right = x2s };
            //sm = new ConsoleFunctions.SMALL_RECT { Top = cursorTop_, Left = 0, Bottom = Convert.ToInt16(Convert.ToInt16(24) + cursorTop_), Right = 80 };

            fixed (ConsoleFunctions.CHAR_INFO* p = ci)

                ConsoleFunctions.WriteConsoleOutput(Handle,
                    new IntPtr(p),
                    new ConsoleFunctions.COORD { X = (short)(x2 - x1), Y = (short)(y2 - y1) },
                    new ConsoleFunctions.COORD { X = 0, Y = 0 },
                    ref sm);
            return false;
        }
        unsafe public bool CopyTo(IntPtr Handle, Rectangle rect)
        {
            short x1s = (short)rect.X, y1s = (short)rect.Y, x2s = (short)rect.Right, y2s = (short)rect.Bottom;
            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[rect.Width * rect.Height];
            var sm = new ConsoleFunctions.SMALL_RECT { Top = y1s, Left = x1s, Bottom = y2s, Right = x2s };
            ConsoleFunctions.ReadConsoleOutput(this.Handle,
                ci,
                new ConsoleFunctions.COORD { X = (short)rect.Width, Y = (short)rect.Height },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm
                );
            sm = new ConsoleFunctions.SMALL_RECT { Top = y1s, Left = x1s, Bottom = y2s, Right = x2s };
            fixed (ConsoleFunctions.CHAR_INFO* p = ci)

                ConsoleFunctions.WriteConsoleOutput(Handle,
                    new IntPtr(p),
                    new ConsoleFunctions.COORD { X = (short)rect.Width, Y = (short)rect.Height },
                    new ConsoleFunctions.COORD { X = 0, Y = 0 },
                    ref sm);
            return false;
        }
        public static int Scale(string text)
        {
            return Encoding.GetEncoding(932).GetBytes(text).Length;
        }
        unsafe public Render Clone()
        {
            //int Get = this.ScrollY;
            var rend = new Render().CreateHandle(this.BufferWidth, this.BufferHeight);
            //rend.BufferWidth  = this.BufferWidth;
            //rend.BufferHeight = this.BufferHeight;
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbi.cbSize = 96;
            //ConsoleFunctions.GetConsoleScreenBufferInfoEx(this.Handle, ref csbi);
            //ConsoleFunctions.SetConsoleScreenBufferInfoEx(rend.Handle, ref csbi);

            short x2s = (short)this.BufferWidth, y2s = (short)this.BufferHeight;
            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[this.BufferWidth * this.BufferHeight];
            var sm = new ConsoleFunctions.SMALL_RECT { Top = 0, Left = 0, Bottom = y2s, Right = x2s };
            ConsoleFunctions.ReadConsoleOutput(this.Handle,
                ci,
                new ConsoleFunctions.COORD { X = x2s, Y = y2s },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm
                );
            sm = new ConsoleFunctions.SMALL_RECT { Top = 0, Left = 0, Bottom = y2s, Right = x2s };
            fixed (ConsoleFunctions.CHAR_INFO* p = ci)

                ConsoleFunctions.WriteConsoleOutput(rend.Handle,
                    new IntPtr(p),
                    new ConsoleFunctions.COORD { X = x2s, Y = y2s },
                    new ConsoleFunctions.COORD { X = 0, Y = 0 },
                    ref sm);
            rend.CursorLeft = this.CursorLeft;
            rend.CursorTop = this.CursorTop;
            rend.CursorSize = this.CursorSize;
            rend.CursorVisible = this.CursorVisible;
            rend.Color.TextAttribute = rend.Color.TextAttribute;
            //rend.ScrollY = Get;
            return rend;
        }



    }
}
