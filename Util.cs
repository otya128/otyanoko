using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    class Util
    {
        [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
        static extern int FindMimeFromData(IntPtr pBC,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer,
            int cbSize,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
            int dwMimeFlags,
            out IntPtr ppwzMimeOut,
            int dwReserved);

        public static string GetMimeTypeFromFile(string file)
        {

            IntPtr mimeout;
            FileInfo fi;

            fi = new FileInfo(file);

            if (fi.Exists == false)
                throw new FileNotFoundException(file);

            int MaxContent = (int)fi.Length;
            if (MaxContent > 4096) MaxContent = 4096;

            FileStream fs = File.OpenRead(file);

            byte[] buf = new byte[MaxContent];

            fs.Read(buf, 0, MaxContent);

            fs.Close();

            int result = FindMimeFromData(IntPtr.Zero, file, buf, MaxContent, null, 0, out mimeout, 0);

            if (result != 0)
                throw Marshal.GetExceptionForHR(result);

            string mime = Marshal.PtrToStringUni(mimeout);

            Marshal.FreeCoTaskMem(mimeout);

            return mime;

        }
        static public string MIMEtoExten(string MIME)
        {
            return "";
        }
    }
}
