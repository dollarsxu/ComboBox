var ComboBoxAutocompleteCMD = null;
function TextBoxKeydown(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    if (obj.style.display == "none") {
        obj.style.display = "table";
        SetDropDownPosition(sID);
    }

    if (typeof (ComboBoxAutocompleteCMD) == 'number') {
        window.clearTimeout(ComboBoxAutocompleteCMD);
        ComboBoxAutocompleteCMD = null;
    }

    ComboBoxAutocompleteCMD = window.setTimeout("ExecAutocomplete('" + sID + "')", 500);
}

function ExecAutocomplete(sID) {
    var oText = document.getElementById(sID + "_TextBox");
    var sInitValue = oText.getAttribute("initvalue");

    if (sInitValue != oText.value) {
        var oSelectedText = document.getElementById(sID + "_HiddenSelectedText");
        var oSelectedValue = document.getElementById(sID + "_HiddenSelectedValue");
        var oSelectedIndex = document.getElementById(sID + "_HiddenSelectedIndex");

        oSelectedText.value = "";
        oSelectedValue.value = "";
        oSelectedIndex.value = "-1";

        oText.setAttribute("initvalue", oText.value);
        eval(oText.getAttribute("autocomplete"));
        window.setTimeout("SaveDivItem('" + sID + "')", 100);
    }

    ComboBoxAutocompleteCMD = null;
}

function TextBoxDragenter(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    if (obj.style.display == "none")
        obj.style.display = "table";

    if (typeof (ComboBoxAutocompleteCMD) == 'number') {
        window.clearTimeout(ComboBoxAutocompleteCMD);
        ComboBoxAutocompleteCMD = null;
    }

    ComboBoxAutocompleteCMD = window.setTimeout("ExecAutocomplete(" + sID + ")", 500);
}

function ComboBoxClick(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    if (obj.style.display == "none") {
        obj.style.display = "table";
        SetDropDownPosition(sID);
    }
    else
        obj.style.display = "none";
}

function ComboBoxSelectedIndexChanged(tr, sID) {
    var sSelectedValue = tr.getAttribute("selectedvalue");
    var sSelectedText = tr.getAttribute("selectedtext");
    var sSelectedShowText = tr.getAttribute("selectedshowtext");

    var oTextBox = document.getElementById(sID + "_TextBox");
    var oSelectedText = document.getElementById(sID + "_HiddenSelectedText");
    var oText = document.getElementById(sID + "_HiddenText");
    var oSelectedValue = document.getElementById(sID + "_HiddenSelectedValue");

    oTextBox.value = sSelectedShowText;
    oText.value = sSelectedShowText;
    oSelectedText.value = sSelectedText;
    oSelectedValue.value = sSelectedValue;

    tr.setAttribute("class", "mouseout");

    var iNewIndex = tr.getAttribute("currentindex");
    var bAutopostback = oTextBox.getAttribute("autopostback").toLowerCase();
    var oSelectedIndex = document.getElementById(sID + "_HiddenSelectedIndex");

    if (bAutopostback == "true" && (parseInt(oSelectedIndex.value) != parseInt(iNewIndex))) {
        SaveDivItem(sID);
        oSelectedIndex.value = iNewIndex;

        var strControlName = oTextBox.getAttribute("name");
        strControlName = strControlName.substring(0, strControlName.length - 8);
        __doPostBack(strControlName, '');
    }
    HideComboBoxDiv(sID);
}

function HideComboBoxDiv(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    obj.style.display = "none";
}
function RefreshDropDownList(result, userContent, methodName) {
    userContent.innerHTML = result;
}
function SaveDivItem(sID) {
    var oDivItem = document.getElementById(sID + "_DivItem");
    var oHieldDivItem = document.getElementById(sID + "_HiddenDropDownHtml");
    oHieldDivItem.value = encodeURIComponent(oDivItem.innerHTML);
}
function SetDropDownPosition(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    var iDivShowHideHeight = obj.clientHeight;
    var oDiv = document.getElementById(sID);
    var itemalign = obj.getAttribute("itemalign");

    if (itemalign == "Left") {
        return;
    }

    var bTop = false;
    var bBottom = false;

    if (itemalign == "Top") {
        bTop = true;
    }

    if (itemalign == "Bottom") {
        bBottom = true;
    }

    if ((parseInt(oDiv.offsetLeft + obj.clientWidth - GetScrollLeft() + 5) > GetWindowWidth())) {
        obj.style.left = parseInt(oDiv.clientWidth - obj.clientWidth) + "px";
    }
    else
        obj.style.left = "-1px";

    var bExceedHeight = false;
    var oDivItem = document.getElementById(sID + "_DivItem");
    var sInitHeight = oDivItem.getAttribute("initheight");
    if (sInitHeight == "0" || sInitHeight == "0px") {
        if (oDivItem.clientHeight > 200) {
            oDivItem.style.height = '200px';
            bExceedHeight = true;
        }
    }

    if (bTop) {
        obj.style.top = parseInt(0 - obj.clientHeight - 1) + "px";
    }
    else if (bBottom) {
        obj.style.top = "19px";
    }
    else {
        if ((parseInt(oDiv.offsetTop + oDiv.clientHeight + obj.clientHeight - GetScrollTop()) > GetWindowHeight()) && oDiv.offsetTop > obj.clientHeight) {
            if (bExceedHeight) {
                obj.style.top = parseInt(0 - 199 - 1) + "px";
            }
            else {
                obj.style.top = parseInt(0 - obj.clientHeight - 0) + "px";
            }
        }
        else {
            obj.style.top = "19px";
        }
    }
}

function GetWindowHeight() {
    if (typeof (window.innerWidth) == 'number')
        return window.innerHeight;
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
        return document.documentElement.clientHeight;
    else if (document.body && (document.body.clientWidth || document.body.clientHeight))
        return document.body.clientHeight;
    return 0;
}

function GetWindowWidth() {
    if (typeof (window.innerWidth) == 'number')
        return window.innerWidth;
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
        return document.documentElement.clientWidth;
    else if (document.body && (document.body.clientWidth || document.body.clientHeight))
        return document.body.clientWidth;
    return 0;
}

function GetScrollTop() {
    if (document.documentElement.scrollTop) {
        return document.documentElement.scrollTop;
    }
    else if (document.body.scrollTop) {
        return document.body.scrollTop;
    }
    return 0;
}

function GetScrollLeft() {
    if (document.documentElement.scrollLeft) {
        return document.documentElement.scrollLeft;
    }
    else if (document.body.scrollLeft) {
        return document.body.scrollLeft;
    }
    return 0;
}

//CheckBox
function ShowCheckBox(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    if (!obj)
        return false;

    if (obj.style.display == "none")
        SetCheckBoxPosition(sID);
    else
        obj.style.display = "none";
}

function CheckBoxListConfirm(sID) {
    var oSelectedText = document.getElementById(sID + '_HiddenSelectedText');
    var oSelectedValue = document.getElementById(sID + '_HiddenSelectedValue');
    var oText = document.getElementById(sID + '_TextBox');

    var listCheckBox = document.getElementsByName(sID + '_CheckBoxItem');
    if (!oSelectedValue || !oText || !oSelectedText)
        return;

    var sSelectedText = "";
    var sSelectedValue = "";

    for (var i = 0; i < listCheckBox.length; i++) {
        if (listCheckBox[i].checked) {
            if (sSelectedValue != "")
                sSelectedValue += ",";
            if (sSelectedText != "")
                sSelectedText += ",";

            sSelectedValue += listCheckBox[i].value;
            sSelectedText += listCheckBox[i].getAttribute("selectedtext");
        }
    }

    oSelectedText.value = sSelectedText;
    oSelectedValue.value = sSelectedValue;
    oText.value = sSelectedText;
    oText.title = sSelectedText;

    var oHiddenSelectedGroupNames = document.getElementById(sID + '_HiddenSelectedGroupNames');
    var listGroup = document.getElementsByName(sID + '_CheckBoxGroup');
    var sGroupSelected = "";

    for (var i = 0; i < listGroup.length; i++) {
        if (listGroup[i].checked) {
            if (sGroupSelected != "")
                sGroupSelected += ",";
            sGroupSelected += listGroup[i].value;
        }
    }

    oHiddenSelectedGroupNames.value = sGroupSelected;
}

function CheckBoxReSetChecked(sID) {
    var oSelectedValue = document.getElementById(sID + '_HiddenSelectedValue');
    var oHiddenSelectedGroupNames = document.getElementById(sID + '_HiddenSelectedGroupNames');

    if (!oSelectedValue || !oHiddenSelectedGroupNames)
        return;

    var strSplit = ",";
    var strReplace = "\\\\" + strSplit;
    var reg1 = new RegExp(strReplace, "gm");
    var reg2 = new RegExp("\\\\&", "gm");

    var arrSelected = oSelectedValue.value.replace(reg1, "\\&").split(strSplit);

    var listCheckBox = document.getElementsByName(sID + '_CheckBoxItem');

    for (var i = 0; i < listCheckBox.length; i++) {
        var bChecked = false;

        for (var j = 0; j < arrSelected.length; j++) {
            if (listCheckBox[i].value == arrSelected[j].replace(reg2, strSplit)) {
                bChecked = true;
                break;
            }
        }

        listCheckBox[i].checked = bChecked;
    }

    var listCheckGroup = document.getElementsByName(sID + '_CheckBoxGroup');
    var arrGroupSelected = oHiddenSelectedGroupNames.value.replace(reg1, "\\&").split(strSplit);

    for (var i = 0; i < listCheckGroup.length; i++) {
        var bChecked = false;

        for (var j = 0; j < arrGroupSelected.length; j++) {
            if (listCheckGroup[i].value == arrGroupSelected[j].replace(reg2, strSplit)) {
                bChecked = true;
                break;
            }
        }

        listCheckGroup[i].checked = bChecked;
    }
}

function CheckBoxSelectAll(sID) {
    var listCheckBox = document.getElementsByName(sID + '_CheckBoxItem');
    var oSelectAll = document.getElementById(sID + '_SelectAll');

    if (!listCheckBox || !oSelectAll)
        return;

    var bChecked = false;

    if (oSelectAll.checked) {
        bChecked = true;
    }

    for (var i = 0; i < listCheckBox.length; i++) {
        listCheckBox[i].checked = bChecked;
    }

    var listGroup = document.getElementsByName(sID + '_CheckBoxGroup');

    if (!listGroup)
        return;

    for (var i = 0; i < listGroup.length; i++) {
        listGroup[i].checked = bChecked;
    }
}

function CheckBoxGroupSelect(sID, sGroupID) {
    var oGroup = document.getElementById(sID + "_Group_" + sGroupID);

    if (!oGroup)
        return;

    var bChecked = false;
    var arrItemID = oGroup.getAttribute("groupitemids").split(',');

    if (oGroup.checked) {
        bChecked = true;
    }

    for (var j = 0; j < arrItemID.length; j++) {
        var oItem = document.getElementById(sID + "_CheckBox_" + arrItemID[j]);
        if (oItem)
            oItem.checked = bChecked;
    }

    var arrGroupID = oGroup.getAttribute("subgroupids").split(',');

    for (var j = 0; j < arrGroupID.length; j++) {
        var oItem = document.getElementById(sID + "_Group_" + arrGroupID[j]);
        if (oItem)
            oItem.checked = bChecked;
    }
}

function CheckBoxSingleSelect(sID, strItemIDs, strCurrentItemID) {
    if (strItemIDs != "") {
        var arr = strItemIDs.split(',');
        for (var j = 0; j < arr.length; j++) {
            var oItem = document.getElementById(sID + "_CheckBox_" + arr[j]);
            if (oItem) {
                if (arr[j] != strCurrentItemID) {
                    oItem.checked = false;
                }
            }
        }
    }
    else {
        var listCheckBox = document.getElementsByName(sID + "_CheckBoxItem");
        if (!listCheckBox)
            return;

        for (var i = 0; i < listCheckBox.length; i++) {
            if (listCheckBox[i].id != sID + "_CheckBox_" + strCurrentItemID) {
                listCheckBox[i].checked = false;
            }
        }
    }
}

function SetCheckBoxPosition(sID) {
    var obj = document.getElementById(sID + "_DivShowHide");
    var iDivShowHideHeight = obj.clientHeight;
    var oDiv = document.getElementById(sID);
    var itemalign = obj.getAttribute("itemalign");

    if (itemalign == "BottomLeft") {
        return;
    }

    var bLeft = false;
    var bRight = false;
    var bTop = false;
    var bBottom = false;

    if (itemalign == "TopLeft") {
        bTop = true;
        bLeft = true;
    }

    if (itemalign == "TopRight") {
        bTop = true;
        bRight = true;
    }

    if (itemalign == "BottomRight") {
        bBottom = true;
        bRight = true;
    }

    if (bLeft) {
        obj.style.display = "table";
    }
    else if (bRight) {
        if (obj.style.left == "")
            obj.style.left = "-600px";
        obj.style.display = "table";
        obj.style.left = parseInt(oDiv.clientWidth - obj.clientWidth) + "px";
    }
    else {
        if (obj.style.left == "")
            obj.style.left = "-600px";
        obj.style.display = "table";
        if ((parseInt(oDiv.offsetLeft + obj.clientWidth - GetScrollLeft() + 5) > GetWindowWidth())) {
            obj.style.left = parseInt(oDiv.clientWidth - obj.clientWidth) + "px";
        }
        else
            obj.style.left = "-1px";
    }

    var bExceedHeight = false;
    var oDivItem = document.getElementById(sID + "_DivItem");
    var sInitHeight = oDivItem.getAttribute("initheight");
    if (sInitHeight == "0" || sInitHeight == "0px") {
        if (oDivItem.clientHeight > 200) {
            oDivItem.style.height = '200px';
            bExceedHeight = true;
        }
    }

    if (bTop) {
        obj.style.top = parseInt(0 - obj.clientHeight - 1) + "px";
    }
    else if (bBottom) {
        obj.style.top = "22px";
    }
    else {
        if ((parseInt(oDiv.offsetTop + oDiv.clientHeight + obj.clientHeight - GetScrollTop()) > GetWindowHeight()) && oDiv.offsetTop > obj.clientHeight) {
            if (bExceedHeight) {
                obj.style.top = parseInt(0 - 202 - 1) + "px";
            }
            else {
                obj.style.top = parseInt(0 - obj.clientHeight - 1) + "px";
            }
        }
        else {
            obj.style.top = "22px";
        }
    }
}