using ConsoleClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace otyanoko
{
    class RenderColor
    {
        public static ConsoleColor DefaultBackColor = ConsoleColor.Gray;
        public static ConsoleColor DefaultForeColor = ConsoleColor.Black;
        public ConsoleColor DefaultLinkBackColor = DefaultBackColor;
        public ConsoleColor DefaultLinkForeColor = ConsoleColor.DarkBlue;
        public ConsoleColor DefaultSelectLinkBackColor = ConsoleColor.White;
        public ConsoleColor DefaultSelectLinkForeColor = ConsoleColor.DarkBlue;
        Render render;
        /// <summary>
        /// 指定した既存のRenderを使用して、RenderColor クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="rend">Render</param>
        public RenderColor(Render rend)
        {
            this.render = rend;
        }
        /// <summary>
        /// このRenderの属性を取得または設定します。
        /// </summary>
        public int TextAttribute
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                return csbi.wAttributes;

            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(value));
            }
        }
        /// <summary>
        /// Renderの前景色を取得または設定します。
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                return (ConsoleColor)(csbi.wAttributes & 0x000F);
                
            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                ForegroundColorOld = (ConsoleColor)(csbi.wAttributes & 0x000F);//Console.BackgroundColor;
                //Console.BackgroundColor = cc;
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFFF0) | (int)value));
                //ForegroundColorOld = Console.ForegroundColor;
            }
        }
        /// <summary>
        /// Renderの背景色を取得または設定します。
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                return (ConsoleColor)((csbi.wAttributes & 0x00F0)>>4);

            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                BackgroundColorOld = (ConsoleColor)((csbi.wAttributes & 0xFF0F)>>4);
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFF0F) | (int)value << 4));
            }

        }
        /// <summary>
        /// Renderの前景色を取得または設定します。Oldは変更しない
        /// </summary>
        public ConsoleColor ForegroundColor_
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                return (ConsoleColor)(csbi.wAttributes & 0x000F);

            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFFF0) | (int)value));
            }
        }
        /// <summary>
        /// Oldは変更しない
        /// </summary>
        public ConsoleColor BackgroundColor_
        {
            get
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                return (ConsoleColor)((csbi.wAttributes & 0x00F0)>>4);

            }
            set
            {
                var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
                ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFF0F) | (int)value << 4));
            }
        }
        public ConsoleColor ForegroundColorOld = Console.ForegroundColor;
        /// <summary>
        /// Renderの前景色を設定します。
        /// </summary>
        /// <param name="cc">設定するConsoleColor</param>
        public void SetForegroundColor(ConsoleColor cc)
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            ForegroundColorOld = (ConsoleColor)(csbi.wAttributes & 0x000F);//Console.BackgroundColor;
            //Console.BackgroundColor = cc;
            ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFFF0) | (int)cc));
            //ForegroundColorOld = Console.ForegroundColor;
            Console.ForegroundColor = cc;
        }
        /// <summary>
        /// Renderの前景色を設定します。
        /// </summary>
        /// <param name="cc">設定するColor</param>
        public void SetForegroundColor(Color cc)
        {

            ForegroundColorOld = Console.ForegroundColor;
            ConsoleForeColorRGB(cc);
        }
        public void SetForegroundColor()
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            ForegroundColorOld = (ConsoleColor)(csbi.wAttributes & 0x000F);//Console.BackgroundColor;
            //ForegroundColorOld = Console.ForegroundColor;
        }
        public void OldForegroundColor()
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFFF0) | (int)ForegroundColorOld));
            //Console.ForegroundColor = ForegroundColorOld;
        }
        public ConsoleColor BackgroundColorOld = Console.BackgroundColor;
        public void SetBackgroundColor()
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            BackgroundColorOld = (ConsoleColor)((csbi.wAttributes & 0x00F0) >> 4);
            ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFF0F) >> 4));//Console.BackgroundColor;
        }
        public void SetBackgroundColor(ConsoleColor cc)
        {
            var csbi=new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            BackgroundColorOld = (ConsoleColor)((csbi.wAttributes & 0x00F0) >> 4);//Console.BackgroundColor;
            Console.BackgroundColor = cc;
            ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFF0F) | ((int)cc << 4)));
        }
        public void SetBackgroundColor(Color cc)
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            BackgroundColorOld = (ConsoleColor)((csbi.wAttributes & 0x00F0) >> 4);//BackgroundColorOld = Console.BackgroundColor;
            ConsoleBackColorRGB(cc);
        }
        public void OldBackgroundColor()
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0xFF0F) | ((int)BackgroundColorOld << 4)));
            //Console.BackgroundColor = BackgroundColorOld;
        }
        public void ConsoleBackColorRGB(Color color)
        {
            ConsoleBackColorRGB(color.R, color.G, color.B);
        }
        public void ConsoleBackColorRGB(int r, int g, int b)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            CONSOLE_SCREEN_BUFFER_INFOEX csbe = new CONSOLE_SCREEN_BUFFER_INFOEX();
            csbe.cbSize = 96;// (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            //IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            bool brc = GetConsoleScreenBufferInfoEx(render.Handle, ref csbe);
            int mindex = SearchColor(r, g, b);
            SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbe.wAttributes & 0xFF0F) | (mindex << 4)));
        }
        public void ConsoleForeColorRGB(Color color)
        {
            ConsoleForeColorRGB(color.R, color.G, color.B);
        }
        public void ConsoleForeColorRGB(int r, int g, int b)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            CONSOLE_SCREEN_BUFFER_INFOEX csbe = new CONSOLE_SCREEN_BUFFER_INFOEX();
            csbe.cbSize = 96;// (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            //IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            bool brc = GetConsoleScreenBufferInfoEx(render.Handle, ref csbe);
            int mindex = SearchColor(r, g, b);
            SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbe.wAttributes/* & 0x1110*/) | mindex));
        }
        /// <summary>
        /// RGBからコンソールの16色に変換します。
        /// </summary>
        /// <param name="r">R</param>
        /// <param name="g">G</param>
        /// <param name="b">B</param>
        /// <returns>ConsoleColor</returns>
        public int SearchColor(int r, int g, int b)
        {
            CONSOLE_SCREEN_BUFFER_INFOEX csbe = new CONSOLE_SCREEN_BUFFER_INFOEX();
            csbe.cbSize = 96;// (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            //IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            bool brc = GetConsoleScreenBufferInfoEx(render.Handle, ref csbe);
            double[] s = new double[16];


            int index = 0;
            foreach (var i in csbe.ColorTable)
            {
                int r_ = r - i.GetColor().R, g_ = g - i.GetColor().G, b_ = b - i.GetColor().B;
                if (r_ == 0 && g_ == 0 && b_ == 0) return index;
                s[index] = (Math.Sqrt(r_ * r_ + g_ * g_ + b_ * b_));
                index++;
            }
            index = 0;
            double min = double.MaxValue; int mindex = 0;
            foreach (var i in s)
            {
                if (min > i)
                {
                    min = i;
                    mindex = index;
                }
                index++;
            }
            return mindex;
        }
        /// <summary>
        /// 下線を引きます。
        /// </summary>
        /// <param name="flg"></param>
        public void Under(bool flg)
        {
            var csbi = new ConsoleFunctions.CONSOLE_SCREEN_BUFFER_INFO();
            ConsoleFunctions.GetConsoleScreenBufferInfo(render.Handle, out csbi);
            if (flg)
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0x4FFF) | (0x8 << 12)));
            else
                ConsoleFunctions.SetConsoleTextAttribute(render.Handle, Convert.ToUInt16(((int)csbi.wAttributes & 0x4FFF)));
            
        }
        public void Default()
        {
            this.ForegroundColor = RenderColor.DefaultBackColor;
            this.BackgroundColor = RenderColor.DefaultForeColor;
        }
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
        unsafe internal struct CONSOLE_SCREEN_BUFFER_INFOEX
        {
            internal int cbSize;
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal ushort wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal COLORREF[] ColorTable;// = new COLORREF[16];
        }

        const int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFOEX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFOEX csbe);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);


    }
}
