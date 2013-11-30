using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace otyanoko
{
    /// <summary>
    /// System.Net.Mimeだと0;URL=がエラー
    /// </summary>
    class ContentType
    {
        public ContentType(string content)
        {
            this.parameters = new StringDictionary();
            this.mediaType = String.Empty;
            this.charset = String.Empty;
            var param = content.Split(';');
            for (int i = 0; i < param.Length; i++)
            {
                var p = param[i];
                var key = String.Empty;
                var iskey = true;
                for (int j = 0; j < p.Length; j++)
                {
                    var k = p[j];
                    if (k == ' ') continue;
                    if (k == '=') { iskey = false; key = key.ToLower(); continue; }
                    if (iskey) key += k;
                    else
                        this.parameters[key] += k;
                }
                if (i == 0) this.mediaType = key;//MediaTypeの確認は行わない
                if (key == "charset") this.charset = this.parameters[key];
            }
        }
        string mediaType;
        string charset;
        StringDictionary parameters;
        //
        // 概要:
        //     このインスタンスで表される Content-Type ヘッダー内に含まれる文字セット パラメーターの値を取得または設定します。
        //
        // 戻り値:
        //     文字セット パラメーターに関連付けられた値を格納している System.String。
        public string CharSet
        {
            get { return charset; }
            set { charset = value; }
        }
        //
        // 概要:
        //     このインスタンスで表される Content-Type ヘッダー内に含まれるメディア タイプの値を取得または設定します。
        //
        // 戻り値:
        //     メディア タイプおよびサブタイプの値を格納している System.String。 この値には、サブタイプの後にセミコロン (;) の区切り記号は含まれていません。
        //
        // 例外:
        //   System.ArgumentNullException:
        //     設定操作に指定された値が null です。
        //
        //   System.ArgumentException:
        //     設定操作として指定した値が System.String.Empty ("") です。
        //
        //   System.FormatException:
        //     設定操作として指定した値が、解析できない形式です。
        public string MediaType
        {
            get
            {
                return mediaType;
            }
            set
            {
                mediaType = value;
            }
        }
        //
        // 概要:
        //     このインスタンスで表される Content-Type ヘッダー内に含まれる名前パラメーターの値を取得または設定します。
        //
        // 戻り値:
        //     名前パラメーターに関連付けられた値を格納している System.String。
        public string Name { get; set; }
        //
        // 概要:
        //     このインスタンスで表される Content-Type ヘッダー内に含まれるパラメーターを格納しているディクショナリを取得します。
        //
        // 戻り値:
        //     名前と値のペアを格納している書き込み可能な System.Collections.Specialized.StringDictionary。
        public StringDictionary Parameters
        {
            get
            {
                return parameters;
            }
        }
    }
}
