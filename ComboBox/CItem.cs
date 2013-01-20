using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Xumeijin
{
    public class CItem 
    {
        public CItem()
        {
        }

        public CItem(string strText, string strValue, string strSelectedShowText)
        {
            _Text = strText;
            _Value = strValue;
            _SelectedShowText = strSelectedShowText;
        }

        public CItem(string strText, string strValue)
        {
            _Text = strText;
            _Value = strValue;
            _SelectedShowText = strText;
        }

        public CItem(string strText)
        {
            _Text = strText;
            _Value = strText;
            _SelectedShowText = strText;
        }

        public static List<CItem> GetList(string[] arrStr)
        {
            List<CItem> listItem = new List<CItem>();

            foreach (var str in arrStr)
            {
                listItem.Add(new CItem(str, str, str));
            }

            return listItem;
        }

        public static List<CItem> GetList(List<string> list)
        {
            List<CItem> listItem = new List<CItem>();

            foreach (var str in list)
            {
                listItem.Add(new CItem(str, str, str));
            }

            return listItem;
        }

        public static List<CItem> GetList(string strItems)
        {
            List<CItem> listItem = new List<CItem>();

            if (strItems != "")
            {
                string strSplit = strItems.Substring(0, 1);

                Regex re = new Regex("(?<item>(\\\\.|.)+?)(" + Regex.Escape(strSplit) + "|$)");
                MatchCollection mc = re.Matches(strItems.Trim(strSplit.ToCharArray()));
                if (mc.Count != 0)
                {
                    for (int i = 0; i <= mc.Count - 1; i++)
                    {
                        listItem.Add(new CItem(mc[i].Groups["item"].Value.Replace("\\" + strSplit, strSplit),
                            mc[i].Groups["item"].Value.Replace("\\" + strSplit, strSplit),
                            mc[i].Groups["item"].Value.Replace("\\" + strSplit, strSplit)));
                    }
                }
            }

            return listItem;
        }

        public static List<CItem> GetList<T>(List<T> list, string strTextField, string strValueField)
        {
            List<CItem> listItem = new List<CItem>();
            for (int i = 0; i < list.Count; i++)
            {
                Type objType = list[i].GetType();
                PropertyInfo objTextProperty = objType.GetProperty(strTextField);
                PropertyInfo objValueProperty = objType.GetProperty(strValueField);

                listItem.Add(new CItem(objTextProperty.GetValue(list[i], null).ToString(), objValueProperty.GetValue(list[i], null).ToString()));
            }

            return listItem;
        }

        public static List<CItem> GetList<T>(List<T> list, string strTextField)
        {
            List<CItem> listItem = new List<CItem>();
            for (int i = 0; i < list.Count; i++)
            {
                Type objType = list[i].GetType();
                PropertyInfo objTextProperty = objType.GetProperty(strTextField);
                PropertyInfo objValueProperty = objType.GetProperty(strTextField);

                listItem.Add(new CItem(objTextProperty.GetValue(list[i], null).ToString(), objValueProperty.GetValue(list[i], null).ToString()));
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

        private string _SelectedShowText = "";
        /// <summary>
        /// 选中后显示文本
        /// </summary>
        public string SelectedShowText
        {
            set { _SelectedShowText = value; }
            get { return _SelectedShowText; }
        }      
    }
}
