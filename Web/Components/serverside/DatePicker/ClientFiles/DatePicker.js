var g_elemPicker = null;
var g_elemTarget = null;
var g_elemContainer = null;
var g_popup = null;

function dp_showDatePickerPopup(elemPicker, elemTarget, elemContainer) {
    if (elemTarget.disabled || elemTarget.readOnly) {
        return;
    }
    
    g_elemPicker = elemPicker;
    g_elemTarget = elemTarget;
    g_elemContainer = elemContainer;

    var strContent =
        "<html xmlns:c><body scroll=no style=\"border: none; margin: 0; padding: 0;\"><?import namespace=\"c\" implementation=\"" + elemContainer.dp_htcURL + "\">" +
        "<c:Calendar onDatePicked=\"parent.dp_onPickerClosed(this.selectedDate)\" selectedDate=\"" + elemTarget.value + "\" ";

    var strWidth = elemContainer.dp_width;
    var strHeight = elemContainer.dp_height;
    if ((strWidth != null) || (strHeight != null)) {
        var strStyle = "style=\"";
        if (strWidth != null) {
            strStyle = strStyle + "width:" + strWidth + ";";
        }
        if (strHeight != null) {
            strStyle = strStyle + "height:" + strHeight + ";";
        }
        strStyle = strStyle + "\" ";
    
        strContent = strContent + strStyle;
    }

    if (elemContainer.dp_calendarStyle != null) {
        strContent = strContent + "calendarStyle=\"" + elemContainer.dp_calendarStyle + "\" ";
    }
    if (elemContainer.dp_titleStyle != null) {
        strContent = strContent + "titleStyle=\"" + elemContainer.dp_titleStyle + "\" ";
    }
    if (elemContainer.dp_dayHeaderStyle != null) {
        strContent = strContent + "dayHeaderStyle=\"" + elemContainer.dp_dayHeaderStyle + "\" ";
    }
    if (elemContainer.dp_dayStyle != null) {
        strContent = strContent + "dayStyle=\"" + elemContainer.dp_dayStyle + "\" ";
    }
    if (elemContainer.dp_otherMonthDayStyle != null) {
        strContent = strContent + "otherMonthDayStyle=\"" + elemContainer.dp_otherMonthDayStyle + "\" ";
    }
    if (elemContainer.dp_todayDayStyle != null) {
        strContent = strContent + "todayDayStyle=\"" + elemContainer.dp_todayDayStyle + "\" ";
    }
    if (elemContainer.dp_selectedDayStyle != null) {
        strContent = strContent + "selectedDayStyle=\"" + elemContainer.dp_selectedDayStyle + "\" ";
    }

    strContent = strContent + " />" +
                 "<body></html>"

    var width = 16;
    var height = 16;
    var left = elemPicker.offsetWidth;
    var top = elemPicker.offsetHeight;

    g_popup = window.createPopup();
    g_popup.document.write(strContent);

    var popupBody = g_popup.document.body;
    g_popup.show(0, 0, width, height);
    width = popupBody.scrollWidth;
    height = popupBody.scrollHeight;
    left = left - width;

    g_popup.hide();
    g_popup.show(left, top, width, height, elemPicker);
}

function dp_onPickerClosed(dtSelected) {
    if (dtSelected != null) {
        var nYear = dtSelected.getYear();
        if (nYear < 1900) {
            nYear += 1900;
        }

        g_elemTarget.value = (dtSelected.getMonth() + 1) + '/' + dtSelected.getDate() + '/' + nYear;
        g_elemTarget.focus();
        dp_runValidation(g_elemTarget);

        if (g_elemContainer.dp_autoPostBack != null) {
            dp_doAutoPostBack(g_elemTarget);
        }
    }
    g_elemTarget = null;
    g_elemContainer = null;

    g_popup.hide();
    g_popup = null;
}

function dp_runValidation(elem) {
    var vals = elem.Validators;
    var i;
    for (i = 0; i < vals.length; i++) {
        ValidatorValidate(vals[i]);
    }
    ValidatorUpdateIsValid();
}

function dp_doAutoPostBack(elem) {
    __doPostBack(elem.name, '');
}

function dp_showDatePickerFrame(elemPicker, elemTarget, elemContainer, elemFrame, ownerDocument) {
    if (elemTarget.disabled || elemTarget.readOnly) {
        return;
    }

    var left = 0;
    var top = elemPicker.offsetHeight;

    for (var p = elemPicker; p && (p.tagName != 'BODY'); p = p.offsetParent) {
        left += p.offsetLeft;
        top += p.offsetTop;
    }

    elemFrame.style.left = left;
    elemFrame.style.top = top;
    if (elemContainer.dp_width != null) {
        elemFrame.style.width = elemContainer.dp_width;
    }
    if (elemContainer.dp_height != null) {
        elemFrame.style.height = elemContainer.dp_height;
    }
    window.frames[elemFrame.id].InitializeCalendarFrame(elemTarget, elemFrame, elemContainer, ownerDocument);
    
    elemFrame.style.display = '';
}
