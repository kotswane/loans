

function isNumberKey(evt, obj) {


    if (window.event) {
        if (event.keyCode == 46 && obj.value.indexOf(".") >= 0) {
            event.keyCode = 0;
            return false;
        }
        //if (event.keyCode == 45 && obj.value.indexOf("-") >= 0) {
        //    event.keyCode = 0;
        //    return false;
        //}
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 8)) {
            if (event.keyCode != 46 && event.keyCode != 0) {
                event.keyCode = 0;
                return false;
            }
        }
        //get the carat position
        var number = obj.value.split('.');
        var caratPos = getSelectionStart(obj);
        var dotPos = obj.value.indexOf(".");
        if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
            return false;
        }
        //if (obj.value.lastIndexOf(".") >= 0) {
        //    if (event.keyCode >= 48 && event.keyCode <= 57) {
        //        var intDecimalPoints = obj.value.substr(obj.value.indexOf(".")).length;
        //        if (intDecimalPoints > 2) {
        //            event.keyCode = 0;
        //            return false;
        //        }
        //    }
        //}
    }
    else {
        if (event.which == 46 && obj.value.indexOf(".") >= 0) {
            event.which = 0;
            return false;
        }
        //if (event.which == 45 && obj.value.indexOf("-") >= 0) {
        //    event.which = 0;
        //    return false;
        //}
        if (event.which < 48 || event.which > 57) {
            if (event.which != 46 && event.which != 0 && event.which != 8) {
                event.which = 0;
                return false;
            }
        }

        //get the carat position
        var number = obj.value.split('.');
        var caratPos = getSelectionStart(obj);
        var dotPos = obj.value.indexOf(".");
        if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
            return false;
        }
        //if (obj.value.lastIndexOf(".") >= 0) {
        //    if (event.which >= 48 && event.which <= 57) {
        //        var intDecimalPoints = obj.value.substr(obj.value.indexOf(".")).length;
        //        if (intDecimalPoints > 2) {
        //            event.which = 0;
        //            return false;
        //        }
        //    }
        //}
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        if (document.getSelection) {
            var r = document.getSelection(); //IE11
        }
        else {
            var r = document.selection.createRange().duplicate();
            r.moveEnd('character', o.value.length);
        }
        if (r.text == '') return o.value.length;
        return o.value.lastIndexOf(r.text);
    } else return o.selectionStart;
}