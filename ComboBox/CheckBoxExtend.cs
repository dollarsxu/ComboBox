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
    [ToolboxData("<{0}:CheckBoxExtend runat=server></{0}:CheckBoxExtend>")]
    public class CheckBoxExtend : WebControl, INamingContainer
    {
        #region Fields
        private TextBox _textBoxControl;
        private HtmlGenericControl _DivControl;
        private Table _comboTable;
        private TableRow _comboTableRow;
        private TableCell _comboTableTextBoxCell;
        private TableCell _comboTableButtonCell;
        private HiddenField _hiddenSelectedText;
        private HiddenField _hiddenSelectedValue;
        private HiddenField _hiddenSelectedGroupNames;

        #endregion

        #region 属性
        [Description("CheckBox数据源")]
        [Localizable(true)]
        public List<CCheckItem> CheckItems
        {
            set { ViewState["CheckItems"] = value; }
            get
            {
                object o = ViewState["CheckItems"];
                return (o != null) ? (List<CCheckItem>)o : new List<CCheckItem>();
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("CheckBox文本框显示文本")]
        [Localizable(true)]
        public string Text
        {
            set
            {
                TextBoxControl.Text = value;
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
        [DefaultValue("0")]
        [Description("CheckBoxList呈现高度")]
        [Localizable(true)]
        public Unit CheckBoxListHeight
        {
            set { ViewState["CheckBoxListHeight"] = value; }
            get
            {
                object o = ViewState["CheckBoxListHeight"];
                return (o != null) ? (Unit)o : Unit.Parse("0px");
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("100%")]
        [Description("CheckBoxList整体宽度(包含组)")]
        [Localizable(true)]
        public Unit CheckBoxListWidth
        {
            set { ViewState["CheckBoxListWidth"] = value; }
            get
            {
                object o = ViewState["CheckBoxListWidth"];
                return (o != null) ? (Unit)o : Unit.Parse("100%");
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("400px")]
        [Description("CheckBoxList所在单元格宽度(不包含组)")]
        [Localizable(true)]
        public Unit CheckBoxListCellWidth
        {
            set { ViewState["CheckBoxListCellWidth"] = value; }
            get
            {
                object o = ViewState["CheckBoxListCellWidth"];
                return (o != null) ? (Unit)o : Unit.Parse("400px");
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("4")]
        [Description("用于布局项的列数")]
        [Localizable(true)]
        public int RepeatColumns
        {
            set { ViewState["RepeatColumns"] = value; }
            get
            {
                object o = ViewState["RepeatColumns"];
                int iCount = (o != null) ? (int)o : 4;

                if (iCount < 1)
                {
                    iCount = 4;
                }

                return iCount;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("true")]
        [Description("是否启用组选择")]
        [Localizable(true)]
        public bool GroupEnabled
        {
            set { ViewState["GroupEnabled"] = value; }
            get
            {
                object o = ViewState["GroupEnabled"];
                return (o != null) ? (bool)o : true;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Description("单选模式")]
        [Localizable(true)]
        public SingledType SingleMode
        {
            set { ViewState["Singled"] = value; }
            get
            {
                object o = ViewState["Singled"];
                return (o != null) ? (SingledType)o : SingledType.NotSet;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("NotSet")]
        [Description("项目呈现位置")]
        [Localizable(true)]
        public ItemAlignType ItemAlign
        {
            set { ViewState["ItemAlign"] = value; }
            get
            {
                object o = ViewState["ItemAlign"];
                return (o != null) ? (ItemAlignType)o : ItemAlignType.NotSet;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Description("是否显示组")]
        [Localizable(true)]
        public bool ShowGroup
        {
            set { ViewState["ShowGroup"] = value; }
            get
            {
                object o = ViewState["ShowGroup"];
                return (o != null) ? (bool)o : false;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Description("是否显示全选项")]
        [Localizable(true)]
        public bool ShowCheckAll
        {
            set { ViewState["ShowCheckAll"] = value; }
            get
            {
                object o = ViewState["ShowCheckAll"];
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

        protected virtual HiddenField HiddenSelectedGroupNames
        {
            get
            {
                if (_hiddenSelectedGroupNames == null)
                {
                    _hiddenSelectedGroupNames = new HiddenField();
                    _hiddenSelectedGroupNames.ID = "HiddenSelectedGroupNames";
                }
                return _hiddenSelectedGroupNames;
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
            if (Controls.Count < 1)
            {
                Controls.Clear();

                ComboTableTextBoxCell.Controls.Add(TextBoxControl);
                ComboTableButtonCell.Controls.Add(DivControl);
                Controls.Add(ComboTable);

                Controls.Add(HiddenSelectedText);
                Controls.Add(HiddenSelectedValue);
                Controls.Add(HiddenSelectedGroupNames);
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

            this.CssClass = "CheckBoxStyle";
            TextBoxControl.CssClass = "TextBox";
            TextBoxControl.ReadOnly = true;

            if (Enabled)
            {
                TextBoxControl.Attributes.Add("onclick", "ShowCheckBox('" + this.ClientID + "'); " + "CheckBoxReSetChecked('" + this.ClientID + "'); ");
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
            HiddenSelectedValue.RenderControl(writer);
            writer.WriteLine();
            HiddenSelectedGroupNames.RenderControl(writer);
            writer.WriteLine();
            RenderDivShowHide(writer);
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
            TextBoxControl.RenderControl(writer);
            writer.WriteLine();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 17px; padding: 0px;");
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "Imgtd");
            //if (Enabled)
            //    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ShowCheckBox('" + this.ClientID + "'); " + "CheckBoxReSetChecked('" + this.ClientID + "'); ");
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

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: 5px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderCheckBoxDiv(writer);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "divconfirm");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (ShowCheckAll && SingleMode == SingledType.NotSet)
            {
                HtmlInputCheckBox cb = new HtmlInputCheckBox();
                cb.ID = this.ClientID + "_SelectAll";
                cb.Attributes.Add("onclick", "CheckBoxSelectAll('" + this.ClientID + "');");
                cb.RenderControl(writer);

                writer.AddAttribute("for", this.ClientID + "_SelectAll");
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write("全选 ");
                writer.RenderEndTag();
                writer.Write(" ");
            }

            HtmlInputButton confirm = new HtmlInputButton();
            confirm.Value = "确 定";
            confirm.Attributes.Add("class", "btn");
            confirm.Attributes.Add("onclick", "CheckBoxListConfirm('" + this.ClientID + "'); HideComboBoxDiv('" + this.ClientID + "');");
            confirm.RenderControl(writer);

            writer.Write(" ");

            HtmlInputButton cancel = new HtmlInputButton();
            cancel.Value = "取 消";
            cancel.Attributes.Add("class", "btn");
            cancel.Attributes.Add("onclick", "HideComboBoxDiv('" + this.ClientID + "');");
            cancel.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private void RenderCheckBoxDiv(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_DivItem");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "checkboxdiv");
            writer.AddAttribute("initheight", CheckBoxListHeight.ToString());

            string strStyle = "";
            if (CheckBoxListHeight != 0)
                strStyle += "height: " + CheckBoxListHeight + ";";

            if (CheckBoxListWidth != 0)
                strStyle += " width: " + CheckBoxListWidth + ";";

            writer.AddAttribute(HtmlTextWriterAttribute.Style, strStyle);

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (ShowGroup)
            {
                LoadCheckBoxTable();
                tableCheckBoxDiv.RenderControl(writer);
                writer.WriteLine();
            }
            else
            {
                RenderCheckBoxList(writer);
            }

            writer.RenderEndTag();
        }

        private int GroupIndex = 1;
        private int ItemIndex = 1;
        private int MaxCol = 1;

        private void SetMaxCol(string strFatherItemValue, int iCount)
        {
            List<CheckGroup> list = GetSubGroupList(strFatherItemValue);

            for (int i = 0; i < list.Count; i++)
            {
                if (GetSubGroupList(list[i].Value).Count > 0)
                {
                    if (iCount + 1 > MaxCol)
                    {
                        MaxCol = iCount + 1;
                    }

                    SetMaxCol(list[i].Value, iCount + 1);
                }
            }
        }

        HtmlTable tableCheckBoxDiv = new HtmlTable();

        private void LoadCheckBoxTable()
        {
            tableCheckBoxDiv.Attributes.Add("class", "CheckGroupTable");
            SetMaxCol("", 1);
            List<CheckGroup> listGroup = GetSubGroupList("");

            for (int i = 0; i < listGroup.Count; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                tableCheckBoxDiv.Rows.Add(row);
                HtmlTableCell cell = new HtmlTableCell();
                row.Cells.Add(cell);
                cell.Attributes.Add("class", "leftcell");
                if (i == 0)
                {
                    cell.Style.Value = "border-top: 0px solid;";
                }

                string strItemIDs = "";
                string strSubGroupIDs = "";

                if (GetSubGroupList(listGroup[i].Value).Count > 0)
                {
                    int iRowSpan = 0;
                    LoadSubGroup(MaxCol - 1, listGroup[i].Value, row, ref iRowSpan, ref strItemIDs, ref strSubGroupIDs);
                    if (iRowSpan < 1)
                    {
                        iRowSpan = 1;
                    }
                    cell.RowSpan = iRowSpan;
                }
                else
                {
                    cell.ColSpan = MaxCol;
                    HtmlTableCell cellCheckBoxItems = new HtmlTableCell();
                    row.Cells.Add(cellCheckBoxItems);
                    if (i == 0)
                    {
                        cellCheckBoxItems.Style.Value = "border-top: 0px solid;";
                    }
                    LoadGroupItem(cellCheckBoxItems, GetSubList(listGroup[i].Value), ref strItemIDs);
                }

                if (strSubGroupIDs != "")
                {
                    strSubGroupIDs += ",";
                }
                strSubGroupIDs += GroupIndex.ToString();

                if (GroupEnabled && SingleMode == SingledType.NotSet)
                {
                    cell.InnerHtml = "<input type=\"checkbox\" id=\"" +
                        this.ClientID + "_Group_" + GroupIndex.ToString() + "\" value=\"" + listGroup[i].Value + "\" groupitemids=\"" + strItemIDs +
                        "\" subgroupids=\"" + strSubGroupIDs +
                        "\" onclick=\"CheckBoxGroupSelect('" + this.ClientID + "', '" + GroupIndex.ToString() + "')\" name=\"" +
                        this.ClientID + "_CheckBoxGroup" + "\" />";
                }

                cell.InnerHtml += "<label";
                if (GroupEnabled && SingleMode == SingledType.NotSet)
                {
                    cell.InnerHtml += " for=\"" + this.ClientID + "_Group_" + GroupIndex.ToString() + "\"";
                }
                cell.InnerHtml += ">" + listGroup[i].Text + "</label>";

                GroupIndex += 1;
            }

            if (tableCheckBoxDiv.Rows.Count > 0)
            {
                for (int i = 0; i < tableCheckBoxDiv.Rows[0].Cells.Count; i++)
                {
                    tableCheckBoxDiv.Rows[0].Cells[i].Style.Value += " border-top: 0px solid;";
                }
            }
        }

        private void LoadSubGroup(int iMax, string strFatherItemValue, HtmlTableRow row, ref int iRowSpan, ref string ItemIDs, ref string SubGroupIDs)
        {
            List<CheckGroup> listGroup = GetSubGroupList(strFatherItemValue);

            for (int i = 0; i < listGroup.Count; i++)
            {
                HtmlTableCell cell = new HtmlTableCell();
                cell.Attributes.Add("class", "leftcell");

                string strItemIDs = "";
                string strSubGroupIDs = "";

                if (i == 0)
                {
                    row.Cells.Add(cell);
                }
                else
                {
                    row = new HtmlTableRow();
                    tableCheckBoxDiv.Rows.Add(row);
                    row.Cells.Add(cell);
                }

                if (GetSubGroupList(listGroup[i].Value).Count > 0)
                {
                    int iSubRowSpan = 0;
                    LoadSubGroup(iMax - 1, listGroup[i].Value, row, ref iSubRowSpan, ref strItemIDs, ref strSubGroupIDs);
                    if (iSubRowSpan < 1)
                    {
                        iSubRowSpan = 1;
                    }
                    if (SubGroupIDs != "")
                    {
                        SubGroupIDs += ",";
                    }
                    SubGroupIDs += strSubGroupIDs;
                    iRowSpan += iSubRowSpan;
                    cell.RowSpan = iSubRowSpan;
                }
                else
                {
                    iRowSpan += 1;
                    cell.ColSpan = iMax;

                    HtmlTableCell cellCheckBoxItems = new HtmlTableCell();
                    row.Cells.Add(cellCheckBoxItems);
                    LoadGroupItem(cellCheckBoxItems, GetSubList(listGroup[i].Value), ref strItemIDs);
                }

                if (GroupEnabled && SingleMode == SingledType.NotSet)
                {
                    cell.InnerHtml = "<input type=\"checkbox\" id=\"" +
                        this.ClientID + "_Group_" + GroupIndex.ToString() + "\" value=\"" + listGroup[i].Value + "\" groupitemids=\"" + strItemIDs +
                        "\" subgroupids=\"" + strSubGroupIDs +
                        "\" onclick=\"CheckBoxGroupSelect('" + this.ClientID + "', '" + GroupIndex.ToString() + "')\" name=\"" +
                        this.ClientID + "_CheckBoxGroup" + "\" />";
                }

                cell.InnerHtml += "<label";
                if (GroupEnabled && SingleMode == SingledType.NotSet)
                {
                    cell.InnerHtml += " for=\"" + this.ClientID + "_Group_" + GroupIndex.ToString() + "\"";
                }
                cell.InnerHtml += ">" + listGroup[i].Text + "</label>";

                if (ItemIDs != "")
                {
                    ItemIDs += ",";
                }
                ItemIDs += strItemIDs;

                if (SubGroupIDs != "")
                {
                    SubGroupIDs += ",";
                }
                SubGroupIDs += GroupIndex.ToString();

                GroupIndex += 1;
            }
        }

        private List<CheckGroup> GetSubGroupList(string strFatherItemValue)
        {
            List<CheckGroup> list = new List<CheckGroup>();

            foreach (CCheckItem objCheckItem in CheckItems)
            {
                if (objCheckItem.FatherItemValue == strFatherItemValue && objCheckItem.ItemType == ItemType.组)
                {
                    CheckGroup objCheckGroup = new CheckGroup(objCheckItem.Text, objCheckItem.Value);
                    if (!list.Contains(objCheckGroup))
                    {
                        list.Add(objCheckGroup);
                    }
                }
            }

            return list;
        }

        private List<CCheckItem> GetSubList(string strFatherItemValue)
        {
            List<CCheckItem> list = new List<CCheckItem>();

            foreach (CCheckItem objCheckItem in CheckItems)
            {
                if (objCheckItem.ItemType == ItemType.项目 && objCheckItem.FatherItemValue == strFatherItemValue)
                {
                    list.Add(objCheckItem);
                }
            }

            return list;
        }

        private void LoadGroupItem(HtmlTableCell cellCheckBoxItems, List<CCheckItem> listItem, ref string ItemIDs)
        {
            cellCheckBoxItems.Attributes.Add("class", "rightcell");

            if (listItem.Count == 0)
            {
                return;
            }

            string strItemIDs = "";
            for (int i = 0; i < listItem.Count; i++)
            {
                if (strItemIDs != "")
                {
                    strItemIDs += ",";
                }
                strItemIDs += (i + ItemIndex).ToString();
            }

            string strRightCellStyle = "";

            if (CheckBoxListCellWidth != 0)
            {
                strRightCellStyle = "width: " + CheckBoxListCellWidth + "; ";
            }

            HtmlTable tableCheckBox = new HtmlTable();
            tableCheckBox.Style.Value = "border: 0px solid white; padding: 0px; width: 100%;";
            cellCheckBoxItems.Controls.Add(tableCheckBox);
            cellCheckBoxItems.Style.Value = strRightCellStyle;

            int rows = 0;
            rows = (int)Math.Ceiling((decimal)listItem.Count / (decimal)RepeatColumns);

            for (int i = 0; i < rows; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                tableCheckBox.Rows.Add(row);

                for (int j = 0; j < RepeatColumns; j++)
                {
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.Style.Value = "width: " + 100 / RepeatColumns + "%;";
                    row.Cells.Add(cell);

                    if ((i * RepeatColumns + j) < listItem.Count)
                    {
                        if (listItem[i * RepeatColumns + j].BackColor != "")
                        {
                            cell.Style.Value += " background-color: " + listItem[i * RepeatColumns + j].BackColor;
                        }
                        if (listItem[i * RepeatColumns + j].Color != "")
                        {
                            cell.Style.Value += " color: " + listItem[i * RepeatColumns + j].Color;
                        }

                        string strSingleMode = "";

                        if (SingleMode == SingledType.Group)
                        {
                            strSingleMode = " onclick=\"CheckBoxSingleSelect('" + this.ClientID + "', '" + strItemIDs + "', '" + ItemIndex.ToString() + "')\"";
                        }
                        else if (SingleMode == SingledType.All)
                        {
                            strSingleMode = " onclick=\"CheckBoxSingleSelect('" + this.ClientID + "', '', '" + ItemIndex.ToString() + "')\"";
                        }

                        cell.InnerHtml = "<input type=\"checkbox\" id=\"" +
                            this.ClientID + "_CheckBox_" + ItemIndex.ToString() + "\" selectedtext=\"" +
                            listItem[i * RepeatColumns + j].Text + "\" value=\"" + listItem[i * RepeatColumns + j].Value + "\"" +
                            strSingleMode + " name=\"" + this.ClientID + "_CheckBoxItem" + "\"";
                        cell.InnerHtml += " />";

                        cell.InnerHtml += "<label for=\"" + this.ClientID + "_CheckBox_" + ItemIndex.ToString() + "\">" + listItem[i * RepeatColumns + j].Text + "</label>";
                        ItemIndex += 1;
                    }
                }
            }

            if (ItemIDs != "")
            {
                ItemIDs += ",";
            }

            ItemIDs += strItemIDs;
        }

        private void RenderCheckBoxList(HtmlTextWriter writer)
        {
            List<CCheckItem> listItem = new List<CCheckItem>();
            foreach (CCheckItem obj in CheckItems)
            {
                if (obj.ItemType == ItemType.项目)
                {
                    listItem.Add(obj);
                }
            }
            if (listItem.Count == 0)
            {
                writer.Write("");
                return;
            }

            string strItemIDs = "";
            for (int i = 0; i < listItem.Count; i++)
            {
                if (strItemIDs != "")
                {
                    strItemIDs += ",";
                }
                strItemIDs += (i + ItemIndex).ToString();
            }

            int rows = 0;
            rows = (int)Math.Ceiling((decimal)listItem.Count / (decimal)RepeatColumns);

            HtmlTable tableCheckBox = new HtmlTable();
            tableCheckBox.Style.Value = "border: 0px solid white; padding: 0px; width: 100%;";

            for (int i = 0; i < rows; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                tableCheckBox.Rows.Add(row);

                for (int j = 0; j < RepeatColumns; j++)
                {
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.Style.Value = "width: " + 100 / RepeatColumns + "%;";
                    row.Cells.Add(cell);

                    if ((i * RepeatColumns + j) < listItem.Count)
                    {
                        if (listItem[i * RepeatColumns + j].BackColor != "")
                        {
                            cell.Style.Value += " background-color: " + listItem[i * RepeatColumns + j].BackColor;
                        }
                        if (listItem[i * RepeatColumns + j].Color != "")
                        {
                            cell.Style.Value += " color: " + listItem[i * RepeatColumns + j].Color;
                        }

                        string strSingleMode = "";

                        if (SingleMode == SingledType.Group)
                        {
                            strSingleMode = " onclick=\"CheckBoxSingleSelect('" + this.ClientID + "', '" + strItemIDs + "', '" + ItemIndex.ToString() + "')\"";
                        }
                        else if (SingleMode == SingledType.All)
                        {
                            strSingleMode = " onclick=\"CheckBoxSingleSelect('" + this.ClientID + "', '', '" + ItemIndex.ToString() + "')\"";
                        }

                        cell.InnerHtml = "<input type=\"checkbox\" id=\"" +
                             this.ClientID + "_CheckBox_" + ItemIndex.ToString() + "\" selectedtext=\"" +
                             listItem[i * RepeatColumns + j].Text + "\" value=\"" + listItem[i * RepeatColumns + j].Value + "\"" +
                             strSingleMode +
                             " name=\"" + this.ClientID + "_CheckBoxItem" + "\"";
                        cell.InnerHtml += " />";

                        cell.InnerHtml += "<label for=\"" + this.ClientID + "_CheckBox_" + ItemIndex.ToString() + "\">" + listItem[i * RepeatColumns + j].Text + "</label>";
                        ItemIndex += 1;
                    }
                }
            }

            tableCheckBox.RenderControl(writer);
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

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveDivItem" + this.ClientID, "SaveDivItem('" + this.ClientID + "');", true);
            }

            base.OnLoad(e);
        }

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
}