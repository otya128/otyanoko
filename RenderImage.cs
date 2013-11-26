using ConsoleClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIN32;

namespace browser
{
    class RenderImage:IDisposable
    {
        Render render = Core.render;
        public IntPtr handle;
        public IntPtr handle2;
        public Bitmap bmp;
        public RenderImage(Bitmap bmp,IntPtr handle)
        {
            this.bmp = bmp;
            this.handle2 = handle;
        }
        public void Dispose()
        {
            ConsoleFunctions.SetConsoleActiveScreenBuffer(handle2);
            kernel32.CloseHandle(handle);
            handle = IntPtr.Zero;
        }

        public IntPtr Render()
        {

            Bitmap bitmap = bmp.Clone(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                PixelFormat.Format4bppIndexed
            );

            ConsoleFunctions.CHAR_INFO[] ci = new ConsoleFunctions.CHAR_INFO[bitmap.Width * bitmap.Height];
            var c = ConsoleFunctions.CreateConsoleScreenBuffer(0x80000000U | 0x40000000U, 0x00000001, Program.NULL, 1, Program.NULL);


            ConsoleFunctions.SetConsoleScreenBufferSize(c, new ConsoleFunctions.COORD { X = (short)bitmap.Width, Y = (short)bitmap.Height });

            var color = new Dictionary<Color, int>();
            for (int i = 0; i < bitmap.Palette.Entries.Length; i++)
            {
                color.Add(bitmap.Palette.Entries[i], render.Color.SearchColor(bitmap.Palette.Entries[i].R, bitmap.Palette.Entries[i].G, bitmap.Palette.Entries[i].B) << 4);
                //SetScreenColorsApp.SetColor((ConsoleColor)i, bitmap.Palette.Entries[i]);
            }
            int K = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    ci[K].Attributes = (ushort)(color[bitmap.GetPixel(j, i)]);
                    K++;
                }
            }
            var sm = new ConsoleFunctions.SMALL_RECT
            {
                Top = 0,
                Left = 0,
                Bottom = Convert.ToInt16(bitmap.Height),
                Right = (short)bitmap.Width
            };
            unsafe
            {
                fixed (ConsoleFunctions.CHAR_INFO* p = ci)
                    ConsoleFunctions.WriteConsoleOutput(c,
                    new IntPtr(p),
                    new ConsoleFunctions.COORD { X = (short)bitmap.Width, Y = Convert.ToInt16(bitmap.Height) },
                    new ConsoleFunctions.COORD { X = 0, Y = 0 },
                    ref sm);
            }
            ConsoleFunctions.SetConsoleActiveScreenBuffer(c);
            handle = c;
            return c;
        }
    }
}
