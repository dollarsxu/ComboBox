using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Xumeijin
{
    [Serializable]
    public class CCheckItem
    {
        public CCheckItem()
        {
        }

        public CCheckItem(string strText, string strValue)
        {
            _Text = strText;
            _Value = strValue;
        }

        public CCheckItem(string strValue)
        {
            _Text = strValue;
            _Value = strValue;
        }

        public static List<CCheckItem> GetList(string[] arrStr)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();

            foreach (var str in arrStr)
            {
                listItem.Add(new CCheckItem(str));
            }

            return listItem;
        }

        public static List<CCheckItem> GetList(List<string> list)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();

            foreach (var str in list)
            {
                listItem.Add(new CCheckItem(str));
            }

            return listItem;
        }

        public static List<CCheckItem> GetList(string strItems)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();

            if (strItems != "")
            {
                string strSplit = strItems.Substring(0, 1);

                Regex re = new Regex("(?<item>(\\\\.|.)+?)(" + Regex.Escape(strSplit) + "|$)");
                MatchCollection mc = re.Matches(strItems.Trim(strSplit.ToCharArray()));
                if (mc.Count != 0)
                {
                    for (int i = 0; i <= mc.Count - 1; i++)
                    {
                        listItem.Add(new CCheckItem(mc[i].Groups["item"].Value.Replace("\\" + strSplit, strSplit)));
                    }
                }
            }

            return listItem;
        }

        public static List<CCheckItem> GetList<T>(List<T> list, string strTextField, string strValueField)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();
            for (int i = 0; i < list.Count; i++)
            {
                Type objType = list[i].GetType();
                PropertyInfo objTextProperty = objType.GetProperty(strTextField);
                PropertyInfo objValueProperty = objType.GetProperty(strValueField);

                listItem.Add(new CCheckItem(objTextProperty.GetValue(list[i], null).ToString(), objValueProperty.GetValue(list[i], null).ToString()));
            }

            return listItem;
        }

        public static List<CCheckItem> GetList<T>(List<T> list, string strTextField)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();
            for (int i = 0; i < list.Count; i++)
            {
                Type objType = list[i].GetType();
                PropertyInfo objTextProperty = objType.GetProperty(strTextField);

                listItem.Add(new CCheckItem(objTextProperty.GetValue(list[i], null).ToString()));
            }

            return listItem;
        }

        private string _Text = "";
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            set { _Text = value; }
            get { return _Text; }
        }

        private string _Value = "";
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            set { _Value = value; }
            get { return _Value; }
        }

        private ItemType _ItemType = ItemType.项目;
        /// <summary>
        /// 项目属性
        /// </summary>
        public ItemType ItemType
        {
            set { _ItemType = value; }
            get { return _ItemType; }
        }

        private string _FatherItemValue = "";
        /// <summary>
        /// 所属父组值
        /// </summary>
        public string FatherItemValue
        {
            set { _FatherItemValue = value; }
            get { return _FatherItemValue; }
        }

        private string _Color = "";
        /// <summary>
        /// 文字颜色
        /// </summary>
        public string Color
        {
            set { _Color = value; }
            get { return _Color; }
        }

        private string _BackColor = "";
        /// <summary>
        /// 背景色
        /// </summary>
        public string BackColor
        {
            set { _BackColor = value; }
            get { return _BackColor; }
        }
    }

    internal class CheckGroup
    {
        public CheckGroup()
        {
        }

        public CheckGroup(string strText, string strValue)
        {
            _Text = strText;
            _Value = strValue;
        }

        public CheckGroup(string strValue)
        {
            _Text = strValue;
            _Value = strValue;
        }

        private string _Text = "";
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            set { _Text = value; }
            get { return _Text; }
        }

        private string _Value = "";
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            set { _Value = value; }
            get { return _Value; }
        }
    }

    public enum SingledType
    {
        NotSet,
        Group,
        All
    }

    public enum ItemType
    {
        组,
        项目
    }

    public enum ItemAlignType
    {
        NotSet,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}