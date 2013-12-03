using ConsoleClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace otyanoko
{
    class Center:Render
    {
        static string Centering(List<HojichaHtmlNode> hhn,string centeringBuff)
        {
           //var next = hhn;
            foreach (var next in hhn)
            {

                if (next.Name != "br" && next.Name != "hr" && next.Name != "th") centeringBuff += next.InnerText;
                    else return centeringBuff;
                    if (next.ChildNodes != null) if (next.ChildNodes.Count != 0) Centering(next.ChildNodes, centeringBuff);
                
                else return centeringBuff;
            }
            return centeringBuff;
        }
        public static void Centering(Render rend, HojichaHtmlNode hhn)
        {
            string centeringBuff = "";
            var next = hhn;
            while(true)//foreach (var next in hhn)
            {
                if (next != null)
                {

                    if (next.Name != "br" && next.Name != "hr" && next.Name != "th") { if (next.Name == "#text")centeringBuff += next.InnerText; }
                    else break;
                    if (next.ChildNodes != null)if(next.ChildNodes.Count!=0) centeringBuff = Centering(next.ChildNodes,centeringBuff);
                }
                else break;
                next = next.NextSibling;
            }
            rend.CursorLeft = rend.BufferWidth / 2 - (Render.Scale(centeringBuff) / 2);
        }
        public Center(Render rend)
        {
            this.Handle = rend.Handle;
            this.centerBuff = new Render().CreateHandle();
        }
        Render centerBuff;
        public void Write(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg));
            uint cell;
            arg = Replace(arg);
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.Write(Replace(arg));
        }
        public void WritePre(string arg)
        {
            Scroll(arg);
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.Append(Replace(arg)); 
            uint cell;
            arg = ReplacePre(arg);
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
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
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            this.Color.Under(false);
            this.Color.ForegroundColor = c;

        }
        public void WriteLine(string arg)
        {
            Scroll(arg);//if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(Replace(arg)); 
            //Console.WriteLine(arg.Replace("\n", ""));
            uint cell;
            arg = Replace(arg) + "\n";
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        public void WriteLine()
        {
            //CursorTop++; CursorLeft = 0;
            //if (Console.BufferHeight < Console.CursorTop + 10) Console.BufferHeight *= 2;
            //console.AppendLine(); 
            uint cell;
            string arg = "\n";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
            //Console.WriteLine();
        }
        public void WriteTab()
        {
            uint cell;

            string arg = "\t";//new String(' ', CursorLeft % 3);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
        public void WriteTab(int margin)
        {
            uint cell;

            string arg = new String(' ', CursorLeft % margin);//"\t";
            Scroll(arg);
            ConsoleFunctions.WriteConsole(this.centerBuff.Handle, arg, Convert.ToUInt32(Encoding.GetEncoding(932).GetBytes(arg).Length), out cell, NULL);
        }
    }
}
