using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace otyanoko
{
    class HojichaHtmlDocument
    {
        HojichaHtmlNode _documentNode;
        public HojichaHtmlNode DocumentNode { get { return _documentNode; } }
        string _html;
        public void LoadHtml(string p)
        {
            this._html = p;
            _documentNode = new HojichaHtmlNode();
            _documentNode.Position = 0;
            ParseHtml(_documentNode);
        }

        public HojichaHtmlNode ParseHtml(HojichaHtmlNode hhn)
        {
            int i = hhn.Position;
            bool nameRead = false;
            string name = "";
            HojichaHtmlNode newChild = new HojichaHtmlNode();
            string Text = "";
            bool isText = true;
            while (i<_html.Length)
            {
                char p = _html[i];
                if (nameRead)
                {
                    if (p == '_' || p == ' ' || p == '>')
                    {
                        nameRead = false;
                        if(!newChild.Closed)newChild = new HojichaHtmlNode(name);
                    }
                    else
                    {
                        name += p;
                    }
                }
                if (isText&&p!='<') Text += p;
                if (p == '<')
                {
                    if (Text != "")
                    {
                        newChild = new HojichaHtmlNode("#text",Text);
                        hhn.ChildNodes.Add(newChild);
                        Text = "";
                    } isText = false;
                    nameRead = true;
                }
                
                if (p == '/')
                {
                    newChild.Closed = true;
                }
                if (p == '>')
                {
                    isText = true;
                    name = "";
                    if (newChild.Closed) return hhn;
                    else
                    {
                        hhn.ChildNodes.Add(newChild);
                        HojichaHtmlNode newNode = new HojichaHtmlNode();
                        //newChild.ChildNodes.Add(newNode);
                        /*newNode*/
                        newChild.Position = i + 1;
                        i = ParseHtml(newChild).Position;
                    }
                }
                i++;
                hhn.Position = i;
            }
            return hhn;
        }

    }
    class HojichaHtmlNode
    {
        public HojichaHtmlNode()
        {
            _childNodes = new List<HojichaHtmlNode>();
        }
        public HojichaHtmlNode(string name)
        {
            _name = name;
            _childNodes = new List<HojichaHtmlNode>();
        }

        public HojichaHtmlNode(string name, string Text)
        {
            // TODO: Complete member initialization
            this._name = name;
            _childNodes = new List<HojichaHtmlNode>();
            this._innerText = Text;
        }
        string _innerText;
        public string InnerText
        {
            get
            {
                return _innerText;
            }
        }
        public int Position;
        public bool _closed;
        public bool Closed
        {
            get
            {
                return _closed;
            }
            set
            {
                _closed = value;
            }
        }
        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        List<HojichaHtmlNode> _childNodes;
        public List<HojichaHtmlNode> ChildNodes
        {
            get
            {
                return _childNodes;
            }
            set
            {
                _childNodes = value;
            }
        }
    }
}
