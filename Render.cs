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

namespace browser
{
    class Render
    {
        static public int CursorLeft
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(hSrceen, out csbi);
                return csbi.dwCursorPosition.X;
            }
            set
            {
                ConsoleFunctions.SetConsoleCursorPosition(hSrceen, new ConsoleFunctions.COORD { X = (short)value, Y = (short)CursorTop });
            }
        }
        static public int CursorTop
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(hSrceen, out csbi);
                return csbi.dwCursorPosition.Y;
            }
            set
            {
                ConsoleFunctions.SetConsoleCursorPosition(hSrceen, new ConsoleFunctions.COORD { X = (short)CursorLeft, Y = (short)value });
            }
        }
        static public bool CursorVisible
        {
            get
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO();
                ConsoleFunctions.GetConsoleCursorInfo(hSrceen, out cci);
                return cci.Visible;
            }
            set
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO { Size = CursorSize, Visible = value };
                ConsoleFunctions.SetConsoleCursorInfo(hSrceen, ref cci);
            }
        }
        public static uint CursorSize
        {
            get
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO();
                ConsoleFunctions.GetConsoleCursorInfo(hSrceen, out cci);
                return cci.Size;
            }
            set
            {
                var cci = new ConsoleFunctions.CONSOLE_CURSOR_INFO { Size = value, Visible = CursorVisible };
                ConsoleFunctions.SetConsoleCursorInfo(hSrceen, ref cci);
            }
        }
        public static int ScrollY
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
        public static bool AutoScreenBuff = true;
        static public IntPtr hSrceen;
        static public IntPtr StdHandle;
        static public IntPtr NULL = IntPtr.Zero;
        internal static void RenderHtml(HtmlAgilityPack.HtmlNodeCollection htmlNodeCollection,StateClass state)
        {
            Core.getHtml(htmlNodeCollection, state,true);
        }
        internal static void RenderHtml(HtmlAgilityPack.HtmlNode htmlNode, StateClass state)
        {
            var hc = new HtmlAgilityPack.HtmlNodeCollection(htmlNode);
            hc.Add(htmlNode);
            Core.getHtml(hc, state, true);
        }
        internal static void RenderHtmlA(HtmlAgilityPack.HtmlNodeCollection htmlNodeCollection, StateClass state)
        {
            var ss = state.Clone();
            ss.RenderA = true;
            Core.getHtml(htmlNodeCollection, ss, true);
        }

        static public void RenderButton(string text)
        {
            Render.Write("[" + text + "]");
        }
        static public void RenderButtonColor(string text, ConsoleColor Fore, ConsoleColor Back)
        {
            RenderColor.SetBackgroundColor(Back);
            RenderColor.SetForegroundColor(Fore);
            RenderButton(text);
            RenderColor.OldBackgroundColor();
            RenderColor.OldForegroundColor();
        }
        static public void RenderButtonColor(string text, Color Fore, Color Back)
        {
            RenderColor.SetBackgroundColor(Back);
            RenderColor.SetForegroundColor(Fore);
            RenderButton(text);
            RenderColor.OldBackgroundColor();
            RenderColor.OldForegroundColor();
        }

        static public void RenderInput(int size = 8)
        {
            Render.WritePre("[" + new string('_', size) + "]");
        }
        static public void RenderCheckBox(bool cheked)
        {
            Render.WritePre("[" + (cheked ? "x" : " ") + "]");
        }
        static public void RenderSelect(string item)
        {
            Render.WritePre("[" + item + "]");
        }
        static public void RenderSelect(string item, int max,int min)
        {
            if (max < min)
                Render.WritePre("[" + item.Substring(0,max-1) + ">]");
            else
                Render.WritePre("[" + item + new string(' ', max - min) + "]");
        }
        static public void RenderTextBox(string item, int max)
        {
            byte[] ary1 = Encoding.GetEncoding(932).GetBytes(item);
            int len = ary1.Length;
            if (max < len)
            {
                byte[] ary=new byte[max-1];
                Array.Copy(ary1, ary, max - 1);
                int cx = CursorLeft;
                Render.WritePre("[" + Encoding.GetEncoding(932).GetString(ary));
                CursorLeft = cx + max;
                Render.WritePre(">]");
            }
            else
                Render.WritePre("[" + item + new string('_', max - len) + "]");
        }
        static public void RenderTextBox()
        {
            Render.WritePre("[" + new string('_', LineTextBoxUI.DefaultSize) + "]");
        }
        static public void RenderTextBox(int siz)
        {
            Render.WritePre("[" + new string('_', siz) + "]");
        }
        static public int cursorTop = 0, cursorLeft = 0;
        static public string Replace(string arg)
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
        static public string ReplacePre(string arg)
        {
            return HttpUtility.HtmlDecode(arg.Replace("\t", "").Replace("&nbsp;"," "));
        }
        static public StringBuilder console=new StringBuilder();
        static public void Scroll(string arg)
        {
            if (AutoScreenBuff)
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(hSrceen, out csbi);

                if (csbi.dwSize.Y < csbi.dwCursorPosition.Y + (Encoding.GetEncoding(932).GetBytes(arg).Length / Core.ScreenBuffSizeY) + 20)
                {
                    csbi.dwSize.Y *= 2;
                    ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, csbi.dwSize);
                }
            }
            return;
        }
        static public void Write(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg));
            uint cell;
            arg = Replace(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.Write(Replace(arg));
        }
        static public void WritePre(string arg)
        {
            Scroll(arg);
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg)); 
            uint cell;
            arg = ReplacePre(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.Write(ReplacePre(arg));
        }
        static public void WriteLink(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            var c = RenderColor.ForegroundColor;
            RenderColor.ForegroundColor = ConsoleColor.DarkBlue;
            RenderColor.Under(true);
            //console.Append(Replace(arg)); 
            //Console.Write(Replace(arg));
            uint cell;
            arg = Replace(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            RenderColor.Under(false);
            RenderColor.ForegroundColor = c;
            
        }
        static public void WriteLine(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(Replace(arg)); 
            //Console.WriteLine(arg.Replace("\n", ""));
            uint cell;
            arg = Replace(arg)+"\n";
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        static public void WriteLine()
        {
            //CursorTop++; CursorLeft = 0;
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(); 
            uint cell;
            string arg = "\n";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.WriteLine();
        }
        static public void WriteTab()
        {
            uint cell;

            string arg = "\t";//new String(' ', CursorLeft % 3);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        static public void WriteTab(int margin)
        {
            uint cell;

            string arg = new String(' ', CursorLeft % margin);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(Render.hSrceen, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        static unsafe public bool Copy(IntPtr hSrceen1, IntPtr hSrceen, short cursorTop_ = -1, short size = -1)
        {
            if (cursorTop_ == -1) cursorTop_ = 0;
            if (size == -1) size = Convert.ToInt16(Core.ScreenBuffSizeY - 1);
            Copy(hSrceen1, hSrceen, cursorTop_, size,0);//if (Top == -1) Top = 0;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hSrceen1">コピー元</param>
        /// <param name="hSrceen">コピー先</param>
        /// <returns></returns>Core.ScreenBuffSizeX
        static unsafe public bool Copy(IntPtr hSrceen1,IntPtr hSrceen,short cursorTop_,short size,short Top)
        {
            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[Core.ScreenBuffSizeX * size];
            var sm = new ConsoleFunctions.SMALL_RECT { Top = cursorTop_, Left = 0, Bottom = Convert.ToInt16(Convert.ToInt16(size) + cursorTop_), Right = (short)Core.ScreenBuffSizeX };
            ConsoleFunctions.ReadConsoleOutput(hSrceen1,
                ci,
                new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = size },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm
                );
            sm = new ConsoleFunctions.SMALL_RECT { Top = Top, Left = 0, Bottom = Convert.ToInt16(size), Right = (short)Core.ScreenBuffSizeX };
            //sm = new ConsoleFunctions.SMALL_RECT { Top = cursorTop_, Left = 0, Bottom = Convert.ToInt16(Convert.ToInt16(24) + cursorTop_), Right = 80 };
           
            fixed (ConsoleFunctions.CHAR_INFO* p = ci)
                
            ConsoleFunctions.WriteConsoleOutput(hSrceen, 
                new IntPtr(p),
                new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = size },
                new ConsoleFunctions.COORD { X = 0, Y = 0 },
                ref sm);
            ConsoleFunctions.SetConsoleCursorPosition(hSrceen, new ConsoleFunctions.COORD { X = (short)0, Y = (short)cursorTop_ });
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
        public static IntPtr CopyToNewBuff()
        {
            var hSrceen = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, NULL, 1, NULL);
            ConsoleFunctions.SetConsoleScreenBufferSize(hSrceen, new ConsoleFunctions.COORD { X = (short)Core.ScreenBuffSizeX, Y = (short)Core.ScreenBuffSizeY });
            Render.Copy(Render.hSrceen, hSrceen, (short)Render.ScrollY,25, 0);
            return hSrceen;
        }





    }
}
