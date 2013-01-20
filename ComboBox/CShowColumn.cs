using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace Xumeijin
{
    public class CShowColumn
    {
        public CShowColumn(string strTitle)
        {
            _Title = strTitle;
        }

        private string _Title = "";
        /// <summary>
        /// 显示标题
        /// </summary>
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }

        private string _SortField = "";
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField
        {
            set { _SortField = value; }
            get { return _SortField; }
        }

        private bool _Link = false;
        /// <summary>
        /// 连接
        /// </summary>
        public bool Link
        {
            set { _Link = value; }
            get { return _Link; }
        }

        private string _GroupField = "";
        /// <summary>
        /// 小计字段
        /// </summary>
        public string GroupField
        {
            set { _GroupField = value; }
            get { return _GroupField; }
        }
      
        private bool _Sum = false;
        /// <summary>
        /// 小计
        /// </summary>
        public bool Sum
        {
            set { _Sum = value; }
            get { return _Sum; }
        }

        private string _LinkType = "";
        /// <summary>
        /// 连接类别
        /// </summary>
        public string LinkType
        {
            set { _LinkType = value; }
            get { return _LinkType; }
        }

        private string _ValueExpress = "";
        /// <summary>
        /// 取值表达式
        /// </summary>
        public string ValueExpress
        {
            set { _ValueExpress = value; }
            get { return HttpUtility.HtmlDecode(_ValueExpress); }
        }

        private string _Width = "";
        /// <summary>
        /// 宽度
        /// </summary>
        public string Width
        {
            set { _Width = value; }
            get { return _Width; }
        }
        
        private string _WidthExpress = "";
        /// <summary>
        /// 宽度表达式
        /// </summary>
        public string WidthExpress
        {
            set { _WidthExpress = value; }
            get { return _WidthExpress; }
        }
      
        private string _Align = "";
        /// <summary>
        /// 文本对齐格式
        /// </summary>
        public string Align
        {
            set { _Align = value; }
            get { return _Align; }
        }

        private string _Style = "";
        /// <summary>
        /// 样式
        /// </summary>
        public string Style
        {
            set { _Style = value; }
            get { return _Style; }
        }

        private string _Params = "";
        /// <summary>
        /// 参数
        /// </summary>
        public string Params
        {
            set { _Params = value; }
            get { return _Params; }
        }

        private bool _Hidden = false;
        /// <summary>
        /// 浏览器显示宽度小于1024时隐藏
        /// </summary>
        public bool Hidden
        {
            set { _Hidden = value; }
            get { return _Hidden; }
        }
      
        public static List<CShowColumn> GetList(string strXml)
        {
            List<CShowColumn> list = new List<CShowColumn>();
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(strXml);
            }
            catch
            {
                return list;
            }

            XmlNode xn = doc.LastChild;

            foreach (XmlNode nodeStatus in xn.ChildNodes)
            {
                try
                {
                    CShowColumn obj = new CShowColumn(nodeStatus.Attributes["title"].Value.Trim());

                    obj.ValueExpress = nodeStatus.Attributes["valueexpress"].Value;

                    if (nodeStatus.Attributes["width"] != null)
                    {
                        obj.Width = nodeStatus.Attributes["width"].Value;
                    }
                    if (nodeStatus.Attributes["widthexpress"] != null)
                    {
                        obj.WidthExpress = nodeStatus.Attributes["widthexpress"].Value;
                    }
                    if (nodeStatus.Attributes["align"] != null)
                    {
                        obj.Align = nodeStatus.Attributes["align"].Value;
                    }
                    if (nodeStatus.Attributes["link"] != null)
                    {
                        obj.Link = bool.Parse(nodeStatus.Attributes["link"].Value);
                    }
                    if (nodeStatus.Attributes["sortfield"] != null)
                    {
                        obj.SortField = nodeStatus.Attributes["sortfield"].Value;
                    }
                    if (nodeStatus.Attributes["linktype"] != null)
                    {
                        obj.LinkType = nodeStatus.Attributes["linktype"].Value;
                    }
                    if (nodeStatus.Attributes["sum"] != null)
                    {
                        obj.Sum = bool.Parse(nodeStatus.Attributes["sum"].Value);
                    }
                    if (nodeStatus.Attributes["groupfield"] != null)
                    {
                        obj.GroupField = nodeStatus.Attributes["groupfield"].Value;
                    }
                    if (nodeStatus.Attributes["style"] != null)
                    {
                        obj.Style = nodeStatus.Attributes["style"].Value;
                    }
                    if (nodeStatus.Attributes["params"] != null)
                    {
                        obj.Params = nodeStatus.Attributes["params"].Value;
                    }
                    if (nodeStatus.Attributes["hidden"] != null)
                    {
                        obj.Hidden = bool.Parse(nodeStatus.Attributes["hidden"].Value);
                    }

                    list.Add(obj);
                }
                catch
                {
                }
            }

            return list;
        }
    }
}
