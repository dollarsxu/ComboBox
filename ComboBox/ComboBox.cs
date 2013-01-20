using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;

[assembly: TagPrefix("Xumeijin", "xmj")]

namespace Xumeijin
{
    [DefaultProperty("Text")]
    [DefaultEvent("SelectedIndexChanged")]
    [ToolboxData("<{0}:ComboBox runat=server></{0}:ComboBox>")]
    public class ComboBox : WebControl, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        #region Fields
        private TextBox _textBoxControl;
        private HtmlGenericControl _DivControl;
        private Table _comboTable;
        private TableRow _comboTableRow;
        private TableCell _comboTableTextBoxCell;
        private TableCell _comboTableButtonCell;
        private HiddenField _hiddenText;
        private HiddenField _hiddenSelectedText;
        private HiddenField _hiddenSelectedValue;
        private HiddenField _hiddenSelectedIndex;
        private HiddenField _hiddenDropDownHtml;

        private List<CItem> _listItem = new List<CItem>();
        List<object> _ListObject = new List<object>();

        List<CShowColumn> _ListShowColumn = new List<CShowColumn>();
        private static readonly object EventSelectedIndexChanged = new object();
        #endregion

        #region 属性
        [Description("ConboBox数据源")]
        [Localizable(true)]
        public List<CItem> ListItem
        {
            set { _listItem = value; }
            get { return _listItem; }
        }

        [Description("ConboBox多列显示数据源")]
        [Localizable(true)]
        public List<object> ListObject
        {
            set { _ListObject = value; }
            get { return _ListObject; }
        }

        [Description("ConboBox数据列列表")]
        [Localizable(true)]
        public List<CShowColumn> ListShowColumn
        {
            set { _ListShowColumn = value; }
            get { return _ListShowColumn; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("ConboBox文本框显示文本")]
        [Localizable(true)]
        public string Text
        {
            set
            {
                TextBoxControl.Text = value;
                HiddenText.Value = value;
            }
            get
            {
                if (ReadOnly)
                {
                    return HiddenText.Value;
                }
                else
                {
                    return TextBoxControl.Text;
                }

            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Description("文本框只读属性")]
        [Localizable(true)]
        public bool ReadOnly
        {
            set { ViewState["ReadOnly"] = value; }
            get
            {
                object o = ViewState["ReadOnly"];
                return (o != null) ? (bool)o : false;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("true")]
        [Description("控件的启用状态")]
        [Localizable(true)]
        public override bool Enabled
        {
            set { ViewState["Enabled"] = value; }
            get
            {
                object o = ViewState["Enabled"];
                return (o != null) ? (bool)o : true;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Description("AutoPostBack")]
        [Localizable(true)]
        public bool AutoPostBack
        {
            set { ViewState["AutoPostBack"] = value; }
            get
            {
                object o = ViewState["AutoPostBack"];
                return (o != null) ? (bool)o : false;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("NotSet")]
        [Description("项目呈现位置")]
        [Localizable(true)]
        public DropDownAlignType ItemAlign
        {
            set { ViewState["ItemAlign"] = value; }
            get
            {
                object o = ViewState["ItemAlign"];
                return (o != null) ? (DropDownAlignType)o : DropDownAlignType.NotSet;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("SelectedText")]
        [Localizable(true)]
        public string SelectedText
        {
            set { HiddenSelectedText.Value = value; }
            get { return HiddenSelectedText.Value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("SelectedText")]
        [Localizable(true)]
        public string SelectedValue
        {
            set { HiddenSelectedValue.Value = value; }
            get { return HiddenSelectedValue.Value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("-1")]
        [Description("SelectedIndex")]
        [Localizable(true)]
        public int SelectedIndex
        {
            set { HiddenSelectedIndex.Value = value.ToString(); }
            get { return Convert.ToInt32(HiddenSelectedIndex.Value); }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("OnClientScript")]
        [Localizable(true)]
        public string OnClientScript
        {
            set { ViewState["OnClientScript"] = value; }
            get
            {
                object o = ViewState["OnClientScript"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("Autocomplete")]
        [Localizable(true)]
        public string Autocomplete
        {
            set { ViewState["Autocomplete"] = value; }
            get
            {
                object o = ViewState["Autocomplete"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("100%")]
        [Description("DropDown整体宽度")]
        [Localizable(true)]
        public Unit DropDownWidth
        {
            set { ViewState["DropDownWidth"] = value; }
            get
            {
                object o = ViewState["DropDownWidth"];
                return (o != null) ? (Unit)o : Unit.Parse("100%");
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("0")]
        [Description("DropDown高度 单位px 0表示10行的高度180px")]
        [Localizable(true)]
        public int DropDownHeight
        {
            set { ViewState["DropDownHeight"] = value; }
            get
            {
                object o = ViewState["DropDownHeight"];
                return (o != null) ? (int)o : 0;
            }
        }

        private bool _ClearItem = false;
        /// <summary>
        /// 是否清空下拉Html
        /// </summary>
        public bool ClearItem
        {
            set { _ClearItem = value; }
            get { return _ClearItem; }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("false")]
        [Description("启用多列显示")]
        [Localizable(true)]
        public bool UseMulti
        {
            set { ViewState["UseMulti"] = value; }
            get
            {
                object o = ViewState["UseMulti"];
                return (o != null) ? (bool)o : false;
            }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("")]
        [Description("多列显示对象名称")]
        [Localizable(true)]
        public string ObjectName
        {
            set { ViewState["ObjectName"] = value; }
            get
            {
                object o = ViewState["ObjectName"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("")]
        [Description("Value 表达式")]
        [Localizable(true)]
        public string ExpressValue
        {
            set { ViewState["ExpressValue"] = value; }
            get
            {
                object o = ViewState["ExpressValue"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("")]
        [Description("Text 表达式")]
        [Localizable(true)]
        public string ExpressText
        {
            set { ViewState["ExpressText"] = value; }
            get
            {
                object o = ViewState["ExpressText"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("")]
        [Description("ShowText 表达式")]
        [Localizable(true)]
        public string ExpressShowText
        {
            set { ViewState["ExpressShowText"] = value; }
            get
            {
                object o = ViewState["ExpressShowText"];
                return (o != null) ? (string)o : "";
            }
        }

        [Bindable(true)]
        [Category("多列显示")]
        [DefaultValue("false")]
        [Description("是否显示列表头")]
        [Localizable(true)]
        public bool ShowHead
        {
            set { ViewState["ShowHead"] = value; }
            get
            {
                object o = ViewState["ShowHead"];
                return (o != null) ? (bool)o : false;
            }
        }
        #endregion

        #region 子控件
        protected virtual TextBox TextBoxControl
        {
            get
            {
                if (_textBoxControl == null)
                {
                    _textBoxControl = new TextBox();
                    _textBoxControl.ID = "TextBox";
                }
                return _textBoxControl;
            }
        }

        protected virtual HiddenField HiddenSelectedText
        {
            get
            {
                if (_hiddenSelectedText == null)
                {
                    _hiddenSelectedText = new HiddenField();
                    _hiddenSelectedText.ID = "HiddenSelectedText";
                }
                return _hiddenSelectedText;
            }
        }

        protected virtual HiddenField HiddenText
        {
            get
            {
                if (_hiddenText == null)
                {
                    _hiddenText = new HiddenField();
                    _hiddenText.ID = "HiddenText";
                }
                return _hiddenText;
            }
        }

        protected virtual HiddenField HiddenSelectedValue
        {
            get
            {
                if (_hiddenSelectedValue == null)
                {
                    _hiddenSelectedValue = new HiddenField();
                    _hiddenSelectedValue.ID = "HiddenSelectedValue";
                }
                return _hiddenSelectedValue;
            }
        }

        protected virtual HiddenField HiddenSelectedIndex
        {
            get
            {
                if (_hiddenSelectedIndex == null)
                {
                    _hiddenSelectedIndex = new HiddenField();
                    _hiddenSelectedIndex.ID = "HiddenSelectedIndex";
                    _hiddenSelectedIndex.Value = "-1";
                }
                return _hiddenSelectedIndex;
            }
        }

        protected virtual HiddenField HiddenDropDownHtml
        {
            get
            {
                if (_hiddenDropDownHtml == null)
                {
                    _hiddenDropDownHtml = new HiddenField();
                    _hiddenDropDownHtml.ID = "HiddenDropDownHtml";
                }
                return _hiddenDropDownHtml;
            }
        }

        protected virtual Table ComboTable
        {
            get
            {
                if (_comboTable == null)
                {
                    _comboTable = new Table();
                    _comboTable.ID = "Table";
                    _comboTable.Rows.Add(ComboTableRow);
                }
                return _comboTable;
            }
        }

        protected virtual TableRow ComboTableRow
        {
            get
            {
                if (_comboTableRow == null)
                {
                    _comboTableRow = new TableRow();
                    _comboTableRow.Cells.Add(ComboTableTextBoxCell);
                    _comboTableRow.Cells.Add(ComboTableButtonCell);
                }
                return _comboTableRow;
            }
        }

        protected virtual TableCell ComboTableTextBoxCell
        {
            get
            {
                if (_comboTableTextBoxCell == null)
                {
                    _comboTableTextBoxCell = new TableCell();
                }
                return _comboTableTextBoxCell;
            }
        }

        protected virtual TableCell ComboTableButtonCell
        {
            get
            {
                if (_comboTableButtonCell == null)
                {
                    _comboTableButtonCell = new TableCell();
                }
                return _comboTableButtonCell;
            }
        }

        protected virtual HtmlGenericControl DivControl
        {
            get
            {
                if (_DivControl == null)
                {
                    _DivControl = new HtmlGenericControl();
                    _DivControl.TagName = "div";
                }
                return _DivControl;
            }
        }
        #endregion

        #region Rendering
        protected override void CreateChildControls()
        {
            if (Controls.Count < 1)// || Controls[0] != ComboTable
            {
                Controls.Clear();

                ComboTableTextBoxCell.Controls.Add(TextBoxControl);
                ComboTableButtonCell.Controls.Add(DivControl);
                Controls.Add(ComboTable);

                Controls.Add(HiddenSelectedText);
                Controls.Add(HiddenSelectedValue);
                Controls.Add(HiddenSelectedIndex);
                Controls.Add(HiddenText);
                Controls.Add(HiddenDropDownHtml);
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            AddContainerAttributesToRender(writer);
        }

        protected virtual void AddContainerAttributesToRender(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                TextBoxControl.Height = 15;
            }

            this.CssClass = "ComboboxStyle";
            TextBoxControl.CssClass = "TextBox";
            TextBoxControl.ReadOnly = ReadOnly;

            if (Enabled)
            {
                TextBoxControl.Attributes.Add("onclick", "ComboBoxClick('" + this.ClientID + "');");
                if (!ReadOnly)
                {
                    TextBoxControl.Attributes.Add("onkeydown", "TextBoxKeydown('" + this.ClientID + "');");
                    TextBoxControl.Attributes.Add("ondragenter", "TextBoxDragenter('" + this.ClientID + "');");
                }
                DivControl.Attributes.Add("onclick", "ComboBoxClick('" + this.ClientID + "');");
            }
            DivControl.Attributes.Add("class", "Imgtd");
            base.AddAttributesToRender(writer);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                CreateChildControls();
                AddAttributesToRender(writer);
                ComboTable.RenderControl(writer);
                DivControl.RenderControl(writer);
            }
            else
            {
                base.RenderControl(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            RenderComboTable(writer);
            writer.WriteLine();
            HiddenSelectedText.RenderControl(writer);
            writer.WriteLine();
            HiddenText.RenderControl(writer);
            writer.WriteLine();
            HiddenSelectedValue.RenderControl(writer);
            writer.WriteLine();
            HiddenSelectedIndex.RenderControl(writer);
            writer.WriteLine();
            RenderDivShowHide(writer);
            writer.WriteLine();
            HiddenDropDownHtml.RenderControl(writer);
            writer.WriteLine();
        }

        private void RenderComboTable(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "border: 0px none; padding: 0px; width: 100%; table-layout: fixed;");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; padding: 0px 5px 0px 0px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.WriteLine();

            TextBoxControl.Attributes.Add("initvalue", Text);
            TextBoxControl.Attributes.Add("autocomplete", Autocomplete);
            TextBoxControl.Attributes.Add("autopostback", AutoPostBack.ToString());
            TextBoxControl.Attributes.Add("onclientscript", OnClientScript);
            TextBoxControl.Attributes.Add("showhead", ShowHead.ToString());

            TextBoxControl.Attributes.Add("expressvalue", ExpressValue);
            TextBoxControl.Attributes.Add("expresstext", ExpressText);
            TextBoxControl.Attributes.Add("expressshowtext", ExpressShowText);

            TextBoxControl.Attributes.Add("expressvalue", ExpressValue);
            TextBoxControl.Attributes.Add("expresstext", ExpressText);
            TextBoxControl.Attributes.Add("expressshowtext", ExpressShowText);
            TextBoxControl.Attributes.Add("objectname", ObjectName);

            TextBoxControl.RenderControl(writer);
            writer.WriteLine();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 17px; padding: 0px;");//
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "Imgtd");
            //if (Enabled)
            //    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ComboBoxClick('" + this.ClientID + "');");
            writer.WriteLine(); 
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.WriteLine();
            DivControl.RenderControl(writer);
            writer.WriteLine();
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private void RenderDivShowHide(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "itemlistcontainer");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_DivShowHide");
            writer.AddAttribute("itemalign", ItemAlign.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display: none; z-index: 500;");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderDivItem(writer);
            writer.RenderEndTag();
        }

        private void RenderDivItem(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_DivItem");
            writer.AddAttribute("initheight", DropDownHeight.ToString());
            string strStyle = "overflow-y: auto; overflow-x: hidden; width: " + DropDownWidth + ";";
            if (DropDownHeight != 0)
                strStyle += " height: " + DropDownHeight.ToString() + "px;";
            writer.AddAttribute(HtmlTextWriterAttribute.Style, strStyle);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderDropDownList(writer);
            writer.RenderEndTag();
        }

        private void RenderDropDownList(HtmlTextWriter writer)
        {
            if (UseMulti)
            {
                if (ListObject.Count == 0)
                {
                    if (ClearItem)
                    {
                        writer.Write("");
                        HiddenDropDownHtml.Value = "";
                    }
                    else
                    {
                        writer.Write(Page.Server.UrlDecode(HiddenDropDownHtml.Value));
                    }
                }
                else
                {
                    string strDropDown = GetDropDownList(ListObject, ListShowColumn, ShowHead, ExpressValue, ExpressText, ExpressShowText, ObjectName, this.ClientID, OnClientScript);

                    HiddenDropDownHtml.Value = Page.Server.UrlEncode(strDropDown);
                    writer.Write(strDropDown);
                }
            }
            else
            {
                if (ListItem.Count == 0)
                {
                    if (ClearItem)
                    {
                        writer.Write("");
                        HiddenDropDownHtml.Value = "";
                    }
                    else
                    {
                        writer.Write(Page.Server.UrlDecode(HiddenDropDownHtml.Value));
                    }
                }
                else
                {
                    string strDropDown = GetDropDownList(ListItem, this.ClientID, OnClientScript);
                    HiddenDropDownHtml.Value = Page.Server.UrlEncode(strDropDown);
                    writer.Write(strDropDown);
                }
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.WriteLine();
        }
        #endregion

        #region IPostBackEventHandler Implementation
        public void RaisePostBackEvent(string args)
        {
            SelectedIndexChangedEventArgs pcArgs = new SelectedIndexChangedEventArgs();
            OnSelectedIndexChanged(pcArgs);
        }

        #endregion

        #region IPostBackDataHandler Implementation
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            //string str = pcol[UniqueID + "_input"];
            Page.RegisterRequiresRaiseEvent(this);
            return false;
        }

        public virtual void RaisePostDataChangedEvent() { }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveDivItem" + this.ClientID, "SaveDivItem('" + this.ClientID + "');", true);
            }
            else if (ReadOnly)
            {
                TextBoxControl.Text = HiddenText.Value;
            }

            base.OnLoad(e);
        }

        #region Events
        public event EventHandler SelectedIndexChanged
        {
            add
            {
                Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        #endregion

        #region Methods
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EventSelectedIndexChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region 外部生成列表
        /// <summary>
        /// 外部生成列表
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="listShowColumn">显示列</param>
        /// <param name="bShowHead">是否显示表头</param>
        /// <param name="strExpressValue">值表达式</param>
        /// <param name="strExpressText">文本表达式</param>
        /// <param name="strExpressShowText">显示表达式</param>
        /// <param name="strDropDownListID">DropDownList ID</param>
        /// <param name="strScript">客户端执行Javascript</param>
        /// <returns></returns>
        public static string GetDropDownList<T>(List<T> list, List<Xumeijin.CShowColumn> listShowColumn, bool bShowHead, string strExpressValue, string strExpressText,
            string strExpressShowText, string strObjectName, string strDropDownListID, string strScript)
        {
            string strDropDown = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"multitable\">";

            if (bShowHead)
            {
                strDropDown += "<tr>";

                for (int i = 0; i < listShowColumn.Count; i++)
                {
                    Xumeijin.CShowColumn objShowColumn = listShowColumn[i];

                    string strStyle = "border-top: 0px solid;";
                    if (i == 0)
                    {
                        strStyle += " border-left: 0px solid;";
                    }
                    if (i == listShowColumn.Count - 1)
                    {
                        strStyle += " border-right: 0px solid;";
                    }

                    if (objShowColumn.Width != "")
                    {
                        strStyle += " width: " + objShowColumn.Width + ";";
                    }
                    if (objShowColumn.Align != "")
                    {
                        strStyle += "text-align: " + objShowColumn.Align + ";";
                    }
                    if (objShowColumn.Style != "")
                    {
                        strStyle += objShowColumn.Style;
                    }

                    strDropDown += "<td style=\"" + strStyle + "\"";
                    strDropDown += " class=\"multith\"";
                    strDropDown += ">" + objShowColumn.Title + "</td>";
                }

                strDropDown += "</tr>";
            }

            for (int i = 0; i < list.Count; i++)
            {
                Xumeijin.CPkg objPkg = new Xumeijin.CPkg();
                objPkg["$" + strObjectName] = list[i];
                objPkg["$序号"] = i + 1;

                object oValue = Xumeijin.CExp.GetValue(objPkg, strExpressValue);
                object oText = Xumeijin.CExp.GetValue(objPkg, strExpressText);
                object oShowText = Xumeijin.CExp.GetValue(objPkg, strExpressShowText);

                strDropDown += "<tr onmouseout=\"this.className='mouseout'; \" onmouseover=\"this.className='mouseonver';\" selectedvalue=\"" +
                oValue.ToString() + "\" selectedtext=\"" +
                oText.ToString() + "\" currentindex=\"" + i.ToString() + "\" selectedshowtext=\"" +
                oShowText.ToString() + "\" onclick=\"ComboBoxSelectedIndexChanged(this, '" + strDropDownListID + "');" + strScript + "\">";

                for (int j = 0; j < listShowColumn.Count; j++)
                {
                    Xumeijin.CShowColumn objShowColumn = listShowColumn[j];

                    string strValue = Xumeijin.CExp.GetValue(objPkg, objShowColumn.ValueExpress).ToString();
                    string strStyle = "";
                    if (strValue == DateTime.Parse("1900-01-01").ToShortDateString())
                    {
                        strValue = "";
                    }
                    if (j == 0)
                    {
                        strStyle += "border-left: 0px solid;";
                    }
                    if (j == listShowColumn.Count - 1)
                    {
                        strStyle += "border-right: 0px solid;";
                    }
                    if (i == list.Count - 1)
                    {
                        strStyle += "border-bottom: 0px solid;";
                    }
                    if (objShowColumn.Width != "")
                    {
                        strStyle += "width: " + objShowColumn.Width + ";";
                    }
                    if (objShowColumn.Align != "")
                    {
                        strStyle += "text-align: " + objShowColumn.Align + ";";
                    }
                    if (objShowColumn.Style != "")
                    {
                        strStyle += objShowColumn.Style;
                    }

                    strDropDown += "<td class=\"multitd\"";
                    if (strStyle != "")
                    {
                        strDropDown += " style=\"" + strStyle + "\"";
                    }
                    strDropDown += ">" + strValue + "</td>";
                }

                strDropDown += "</tr>";
            }

            strDropDown += "</table>";

            return strDropDown;
        }
        #endregion

        #region 外部生成列表
        /// <summary>
        /// 外部生成列表
        /// </summary>
        /// <param name="listItem">数据源</param>
        /// <param name="strDropDownListID">控件ID</param>
        /// <param name="strOnClientScript">客户端执行Javascript</param>
        /// <returns></returns>
        public static string GetDropDownList(List<CItem> listItem, string strDropDownListID, string strOnClientScript)
        {
            string strDropDown = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"listtable\">";

            for (int i = 0; i < listItem.Count; i++)
            {
                strDropDown += "<tr onmouseout=\"this.className='mouseout';\" onmouseover=\"this.className='mouseonver';\" selectedvalue=\"" +
                    listItem[i].Value + "\" selectedtext=\"" +
                    listItem[i].Text + "\" currentindex=\"" + i.ToString() + "\" selectedshowtext=\"" +
                    listItem[i].SelectedShowText + "\" onclick=\"ComboBoxSelectedIndexChanged(this, '" + strDropDownListID + "');" + strOnClientScript + "\">";

                strDropDown += "<td class=\"item\">" + listItem[i].Text + "</td>";
                strDropDown += "</tr>";
            }

            strDropDown += "</table>";

            return strDropDown;
        }
        #endregion

        #region GetObjectList
        public static List<object> GetList<T>(List<T> list)
        {
            List<object> listObject = new List<object>();
            foreach (T obj in list)
            {
                listObject.Add(obj);
            }
            return listObject;
        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
                this.Page.ClientScript.RegisterClientScriptResource(this.GetType(), "ComboBox.ComboBox.js");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Hide" + this.ClientID, "$(document).click(function (event) {if ($(event.target).is('#" + this.ClientID + ", #" + this.ClientID + " *')) return; $('#" + this.ClientID + "_DivShowHide').hide(); });", true);
            }

            base.OnPreRender(e);
        }
    }

    #region SelectedIndexChangedEventArgs Class
    public sealed class SelectedIndexChangedEventArgs : CancelEventArgs
    {
        public SelectedIndexChangedEventArgs()
        {
        }
    }
    #endregion

    public enum DropDownAlignType
    {
        NotSet,
        Top,
        Bottom
    }
}
