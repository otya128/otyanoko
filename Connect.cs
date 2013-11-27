using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mime;
namespace otyanoko
{
    //11/26 System.Net.Mimeに変更
    class Connect
    {
        static public string OSVer = Environment.OSVersion.Version.Major.ToString() + "." + Environment.OSVersion.Version.Minor.ToString();
        static public string CLRVer = Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString();
        //otyanoko 1.0;
        static public string UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT " + OSVer + "; .NET CLR " + CLRVer + "; ) otyanoko/1.0";
        
        static public string connect_get(string uri, string encode, string relativeUri)
        {
            string dmmy;
            return connect_get(uri, encode, relativeUri, out dmmy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">http://google.com/</param>
        /// <param name="encode">shift_jis</param>
        /// <param name="relativeUri">/search?q=search</param>
        /// <returns></returns>
        static public string connect_get(string uri, string encode, string relativeUri, out string url)
        {

            Uri u1, u2;
            if (uri != "")
            {
                u1 = new Uri(uri);
                u2 = new Uri(u1, relativeUri);
            }
            else
                u2 = new Uri(relativeUri);
            uri = u2.ToString();
            //url = uri;
            return connect_get(uri, encode, new NameValueCollection(), out url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">http://google.com/</param>
        /// <param name="encode">shift_jis</param>
        /// <param name="relativeUri">/search?q=search</param>
        /// <returns></returns>
        static public string connect_get(string uri, string encode, string relativeUri, out string url, NameValueCollection query)
        {

            Uri u1, u2;
            if (uri != "")
            {
                u1 = new Uri(uri);
                u2 = new Uri(u1, relativeUri);
            }
            else
                u2 = new Uri(relativeUri);
            uri = u2.ToString();
            //url = uri;
            return connect_get(uri, encode, query, out url);
        }
        static public string connect_get(string uri, string encode)
        {
            string d;
            return connect_get(uri, encode, new NameValueCollection(), out d);
        }
        static public string connect_get(string uri, string encode, NameValueCollection query, out string url_)
        {
            Encoding enc = Encoding.GetEncoding(encode);
            Core.encode = encode;
            string url = uri;
            Uri u = new Uri(url);
            var pqs = query;
            List<byte> lb = new List<byte>();
            int count = 0;
            foreach (string nc in pqs)
            {
                count++;
                lb.AddRange(HttpUtility.UrlEncodeToBytes(enc.GetBytes(nc)));
                lb.AddRange(enc.GetBytes("="));
                if (!string.IsNullOrEmpty(pqs[nc])) lb.AddRange(HttpUtility.UrlEncodeToBytes(enc.GetBytes(pqs[nc])));
                if (count < pqs.Count) lb.AddRange(enc.GetBytes("&"));

            }
            string hoge = enc.GetString(lb.ToArray());
            if (!string.IsNullOrEmpty(u.Query) && !string.IsNullOrEmpty(hoge))
            {
                if (u.Query.Substring(u.Query.Length - 1) == "&")
                {
                    url += hoge;
                }
                else
                {
                    if (u.Query.Substring(u.Query.Length - 1) == "?")
                        url += hoge;
                    else
                        url += "&" + hoge;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(hoge))
                    url += "?" + hoge;
            }
            HttpWebRequest req
              = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = Connect.UserAgent;
            //Debug.WriteLine(url); 
            //Debug.WriteLine(req.RequestUri.ToString());
            WebResponse res = req.GetResponse();
            
            url_ = res.ResponseUri.ToString();
            Core.ContentType = new ContentType(res.ContentType);

            if (Core.ContentType.MediaType.IndexOf("image") != -1)
            {
                //UIProcess.OpenImage(url_, res.ContentType);
                //throw new CancelException();
            }
            //ContentTypeのcharsetを優先させるべき?

            //Debug.WriteLine(res.ResponseUri.ToString());
            System.IO.Stream resStream = res.GetResponseStream();
            MemoryStream ms = new MemoryStream();
            resStream.CopyTo(ms);
            // If you need it...
            Program.htmlb = ms.ToArray();
            ms.Close();
            return enc.GetString(Program.htmlb);
#if false
            var wc = new WebClient();
            wc.QueryString = query;
            wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.19 Safari/537.36");
            //Stream st = wc.OpenRead(uri);
            Core.encode = encode;
            Encoding enc = Encoding.GetEncoding(encode);
            htmlb = wc.DownloadData(uri);
            //StreamReader sr = new StreamReader(st, enc);
            var html = enc.GetString(htmlb);//sr.ReadToEnd();
            //sr.Close();
            //st.Close();
            return html;
#endif
        }

        static public string relativeUri(string uri,string relativeUri,out string url)
        {
            Uri u1, u2;
            if (uri != "")
            {
                u1 = new Uri(uri);
                u2 = new Uri(u1, relativeUri);
            }
            else
                u2 = new Uri(relativeUri);
            uri = u2.ToString();
            url = uri;
           return url;
        }
        static public string connect(string url, string encode, string method, byte[] postDataBytes,out string url_)
        {
            //文字コードを指定する
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding(encode);

            //POST送信する文字列を作成
            //バイト型配列に変換
            //byte[] postDataBytes = System.Text.Encoding.ASCII.GetBytes(postData);

            //WebRequestの作成
            System.Net.HttpWebRequest req =
                (HttpWebRequest)System.Net.WebRequest.Create(url);
            req.UserAgent = Connect.UserAgent;
            //メソッドにPOSTを指定
            req.Method = "POST";
            //ContentTypeを"application/x-www-form-urlencoded"にする
            req.ContentType = "application/x-www-form-urlencoded";
            //POST送信するデータの長さを指定
            req.ContentLength = postDataBytes.Length;

            //データをPOST送信するためのStreamを取得
            System.IO.Stream reqStream = req.GetRequestStream();
            //送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length);
            reqStream.Close();

            //サーバーからの応答を受信するためのWebResponseを取得
            System.Net.WebResponse res = req.GetResponse();
            url_ = res.ResponseUri.ToString();
            Core.ContentType = new ContentType(res.ContentType);
            if (Core.ContentType.MediaType.IndexOf("image") != -1)
            {
                //UIProcess.OpenImage(url_,res.ContentType);
                //throw new CancelException();
            }

            //応答データを受信するためのStreamを取得
            System.IO.Stream resStream = res.GetResponseStream();
            MemoryStream ms = new MemoryStream();
            resStream.CopyTo(ms);
            // If you need it...
            Program.htmlb = ms.ToArray();
            ms.Close();
            //Program.htmlb = new byte[Convert.ToInt32(resStream.Length)];
            //resStream.Read(Program.htmlb, 0, Convert.ToInt32(resStream.Length));
            /*//受信して表示
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, enc);
            var result=(sr.ReadToEnd());
            //閉じる
            sr.Close();*/
            return enc.GetString(Program.htmlb);
        }
        public static void connect_file(string url,string path)
        {

            HttpWebRequest req
              = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent
              = Connect.UserAgent;
            
            WebResponse res = req.GetResponse();
 
            using (Stream st = res.GetResponseStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        st.CopyTo(ms);
                        byte[] m = ms.ToArray();
                        fs.Write(m, 0, m.Length);
                    }
                }
            }
        }
        public static string connect_error(WebException ex, string enc, out string _url)
        {
            try
            {
                var res = ex.Response;
                _url = res.ResponseUri.ToString();
                Core.ContentType=new ContentType(res.ContentType);
                if (Core.ContentType.MediaType.IndexOf("image") != -1)
                {
                    //UIProcess.OpenImage(_url, res.ContentType);
                    //throw new CancelException();
                }
                //Debug.WriteLine(res.ResponseUri.ToString());
                System.IO.Stream resStream = res.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                resStream.CopyTo(ms);
                // If you need it...
                Program.htmlb = ms.ToArray();
                ms.Close();
                return Encoding.GetEncoding(enc).GetString(Program.htmlb);
            }
            catch(Exception eex)//エラーページでもエラー
            {
                Debug.WriteLine(eex.Message);
                throw new CancelException();
            }
        }
    }
}
