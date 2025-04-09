$(document).ready(function () {
    var ajaxCount = 0;
    $('[data-toggle="popover"]').popover({ trigger: 'hover', html: true, container: 'body' });
    $('[data-toggle=offsidemenu]').click(function () { $('#left').toggle(); $('#main').toggleClass('mainleft-toggle'); });
    //$('[data-toggle="tooltip"]').tooltip({ container: 'body' });

    $("#bBack").click(function () { window.history.go(-1); return false; });
    $(".btnSave").click(function () { $("form").submit(); });
    $(".nav li.disabled a").click(function () {
        if ($(this).parent().hasClass("disabled")) {
            return false;
        }
    });

    $.ajaxSetup({
        cache: false,
        beforeSend: function () { ajaxCount++; if (ajaxCount == 1) { $.blockUI({ message: '<h4>Lütfen işlem tamamlanana kadar bekleyin...</h4>' }); } },
        complete: function () { ajaxCount--; if (ajaxCount < 1) { ajaxCount = 0; $.unblockUI(); } },
        error: function (x, status, error) { showAjaxError(x, status, error); }
    });

    window.onerror = function (message, url, lineNumber) {
        if (message.indexOf('Script error.') > -1) {
            showErrorNotification(message, "Script Hatası");
            return;
        }

        else if (message != 'ResizeObserver loop limit exceeded') {
            showErrorNotification(message, "Hata");
            return;
        }
        /*
         try {
             $.post('/Common/clientLog', {message: message, lineNumber: lineNumber, url: url});
             }
         catch(err) { } 
         finally { }
          */
    };

    ////////////////////////// Reporting Functions //////////////////////////

    // çıktısı alınabilecek form listesini modal içerisinde gösteren fonksiyon
    $("body").on("click", ".btn-getReportList", function (e) {
        var _url = $(this).data("urlreportlist");
        var divID = $(this).data("modaldiv");
        var _urlreportcontent = $(this).data("urlreportcontent");
        //localStorage.setItem('urlreportcontent', $(this).data("urlreportcontent"));

        $.ajax({
            url: _url,
            type: 'GET',
            success: function (data) {
                if (data != null) {
                    var div = $("#" + divID);
                    var str = '<div> <select class="form-control" name="reportList" size=10>';
                    $.each(data, function (index, report) {
                        str += '<option style="border-bottom:1px solid #f1f1f1" value="' + report.Value + '"><a>' + report.Text + '</option>';
                    });
                    str += '</select></div>';
                    $(div).data("urlreportcontent", _urlreportcontent);
                    $(div).find(".modal-body").html(str);
                    $(div).modal('show');
                }
            },
        });
    });

    // seçili olan raporun print edilebileceği ekranı getiren fonksiyon
    $("body").on("click", ".btn-printReport", function () {
        event.preventDefault();
        var modalID = $(this).data("modaldiv");
        var seledtedValue = $("#" + modalID).find("select").val();
        if (seledtedValue != null) {

            var _url = $("#" + modalID).data("urlreportcontent");
            //_url = localStorage.getItem('urlreportcontent');
            //localStorage.removeItem('urlreportcontent');

            var url = "";
            if (_url.indexOf("?") > 0) {
                url = window.location.protocol + '//' + window.location.host + _url + '&id=' + seledtedValue;
            }
            else {
                url = window.location.protocol + '//' + window.location.host + _url + '/' + seledtedValue;
            }
            $("#reportContentDiv .modal-body").html("<iframe id='printFrame' name='printFrame' src='" + url + "' style='width:21cm; min-height:5000px;border:0px !important;box-shadow: 0 0 10px 0 rgba(0,0,0,.4);'></iframe>");
            $("#reportContentDiv").modal('show');
            $("#reportModalDiv").modal('hide');
        }
        else {
            showWarningNotification("Öncelikle kayıtlı rapor listesinden çıktısını almak istediğiniz formu seçmelisiniz.", "Uyarı");
        }
    });



    $("body").on("click", "#btn-frmPrint", function () {
        $("#reportContentDiv").modal('hide');
        window.frames["printFrame"].focus();
        window.frames["printFrame"].print();
        $("#reportContentDiv .modal-body").html('');
    });


    $("body").on("click", "#btn-frmClose", function () {
        $("#reportContentDiv .modal-body").html('');
        $("#reportContentDiv").modal('hide');
    });

    function PrintElement(elem) {
        var width = 1200;
        var height = 650;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;
        var params = 'width=' + width + ', height=' + height;
        params += ', top=' + top + ', left=' + left;
        var mywindow = window.open('', 'PRINT', params);
        mywindow.document.write('<html><head><title></title>');
        mywindow.document.write('</head><body >');
        mywindow.document.write(document.getElementById(elem).innerHTML);
        mywindow.document.write('</body></html>');
        mywindow.document.close();
        mywindow.focus();
        mywindow.print();
        mywindow.close();
        return true;
    }

    ////////////////////////// End of Reporting Functions //////////////////////////

    $('img').on("error", function () { $(this).src = "../content/images/noimage.png"; $(this).onerror = ""; });
    $('body').on('show.bs.modal', function (e) {
        reInitializeDateTimePicker();
    })

    $(".navbar-search-input").keydown(function (e) {
        if (e.which == 13) {
            $(".navbar-search-btn").trigger("click");
            return false;
        }
    });
    $(".navbar-search-btn").click(function () {
        var searchKey = $(this).parent().prev().val();
        if (!wsLib.isNullOrEmpty(searchKey)) {
            $.ajax({
                url: '/Kisi/Ara', type: 'GET', dataType: 'json', data: { searchKey: searchKey },
                success: function (data) {
                    if (data) {
                        if (data.length > 0) {
                            if (data.length == 1) { window.location = "/Kisi/Edit/" + data[0].ID; }
                            else {
                                var tableProps = {
                                    PKcol: 'ID',
                                    cols: [{ colName: 'TCKimlikNo', colTitle: 'TC Kimlik No', colWidth: 'col-xs-3' },
                                    { colName: 'AdiSoyadi', colTitle: 'Adı Soyadı', colWidth: 'col-xs-4' },
                                    { colName: 'DogumTarihi', colTitle: 'Doğum T.', colWidth: 'col-xs-2' },
                                    { colName: 'DogumYeri', colTitle: 'Doğum Y.', colWidth: 'col-xs-3' }],
                                    action: "/Kisi/Edit", actionTitle: "Seç",
                                    actionIcon: "fa-edit",
                                    actinIconCSS: "btn-primary"
                                }
                                showModal(data, tableProps);
                            }
                        }
                        else { showWarningNotification("Aradağınız bilgilere uygun kişi bulunamadı.", "Kişi Arama"); }
                    }
                    else { showWarningNotification("Aradağınız bilgilere uygun kişi bulunamadı.", "Kişi Arama"); }
                }
            });
        }

    });

    $("body").on("change", ".ddl-cascade", function () {
        var tmpValue = $(this).val();
        var targetID = $(this).data("targetid");
        var targetURL = $(this).data("targeturl");
        var targetProperty = $(this).data("targetproperty");
        var targetOption = $(this).data("option");
        var targetValue = $("#" + targetID).data("value");

        if (!wsLib.isNullOrEmpty(targetURL) && !wsLib.isNullOrEmpty(targetID) && !wsLib.isNullOrEmpty(targetProperty) &&
            ((wsLib.isNullOrEmpty(targetOption) && !wsLib.isNullOrEmpty(targetValue)) || (!wsLib.isNullOrEmpty(targetOption)))) {
            if (wsLib.isNullOrEmpty(tmpValue)) {
                $("#" + targetID).val('');
                $("#" + targetID).trigger('change');
            } else {
                getDDLData(targetURL, JSON.parse('{ "' + targetProperty + '":"' + tmpValue + '"}'), targetID, targetOption, targetValue)
            }
        }
    });

    $("body").on("change", ".ddl-multicascade", function () {
        var tmpValue = $(this).val();
        var targetID = $(this).data("targetid");
        var targetURL = $(this).data("targeturl");
        var targetProperty = $(this).data("targetproperty");
        var targets = $(this).data("targets");
        var targetsArr = targets.split(',');
        var targetparameters = $(this).data("targetparameters");
        var targetparametersArr = targetparameters.split(',');
        var actionData = '{ "' + targetProperty + '":"' + tmpValue + '"'
        for (var i = 0; i < targetsArr.length; i++) {
            var property = targetparametersArr[i];
            var tmp = targetsArr[i];
            var value = $("#" + tmp).val();
            actionData += ", " + '"' + property + '":"' + value + '"'
        }
        actionData += " " + '}';
        if (!wsLib.isNullOrEmpty(targetURL) && !wsLib.isNullOrEmpty(targetID) && !wsLib.isNullOrEmpty(targetProperty)) {
            getDDLData(targetURL, JSON.parse(actionData), targetID, true, null)
        }
    });

    initCustomAjax();
    reInitializeDateTimePicker();
    initCustomValidation();

});

function setTableRowVisibility(selectedValue, tableID, dataPropertyName, visibility) {
    if (visibility) {
        $("#" + tableID + " tbody tr").removeClass("hidden");
        if (!wsLib.isNullOrEmpty(selectedValue)) {
            $("#" + tableID + " tbody tr[data-" + dataPropertyName + "!=" + selectedValue + "]").addClass("hidden");
        }
    }
    else {
        $("#" + tableID + " tbody tr").addClass("hidden");
        if (!wsLib.isNullOrEmpty(selectedValue)) {
            $("#" + tableID + " tbody tr[data-" + dataPropertyName + "=" + selectedValue + "]").removeClass("hidden");
        }
    }
}

function showModal(html) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(targetElementID)) {
        $.ajax({
            url: actionURL, data: actionData, type: "GET",
            success: function (data) {
                if (data != null && data) {
                    $("#modalLayout .modal-body").html(data);
                    reInitializeDateTimePicker();
                    initCustomValidation();
                    $('#modalLayout').modal('show');
                    if (successCallback && typeof (successCallback) === "function") {
                        successCallback();
                    }
                }
            }
        });
    }
}

function showModal(actionURL, methodType, btns) {
    //btns :  [{ btnTitle: 'OK',btnCSS:'btn-success', callBack: callbackFunctionName }, { btnTitle: 'Custom',btnCSS:'btn-default', callBack: callbackFunctionName }
    if (methodType == undefined || methodType == "" || (methodType.toLowerCase() != "get" && methodType.toLowerCase() != "post")) {
        methodType = "get";
    }

    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: actionData, type: methodType.toUpperCase(),
            success: function (data) {
                if (data != null && data) {
                    $("#modalLayout .modal-body").html(data);
                    reInitializeDateTimePicker();
                    initCustomValidation();
                    var tmpHtml = "";
                    if (btns && typeof btns === 'object') {
                        if (btns.length > 0) {
                            for (var i = 0; i < btns.length; i++) {
                                var tmpTitle = btns[i].btnTitle;
                                var tmpCSS = btns[i].btnCSS;
                                //var tmpCallback=btns[i].callBack;
                                tmpHtml += '<button type="button" class="btn ' + tmpCSS + '" id="btnModalLayoutBtn_' + i + '">' + tmpTitle + '"</button>"';
                            }
                        }
                        tmpHtml += '<button type="button" class="btn btn-default" data-dismiss="modal">Vazgeç</button>';
                    }
                    $("#modalLayout .modal-footer").html(tmpHtml);
                    if (btns && typeof btns === 'object') {
                        if (btns.length > 0) {
                            for (var j = 0; j < btns.length; j++) {
                                if (btns[j].callBack && typeof (btns[j].callBack) === "function") {
                                    $("#btnModalLayoutBtn_' + i + '").click(function (e) { tmpCallback = btns[j].callBack(e); });
                                }
                            }
                        }
                    }

                    $('#modalLayout').modal('show');
                    if (successCallback && typeof (successCallback) === "function") {
                        successCallback();
                    }
                }
            }
        });
    }
}

function showModal(data, tableProps) {
    //tableProps : {  PKcol: 'ID',cols: [{ colName: 'TCKimlikNo',  colTitle: 'TC Kimlik No', colWidth: 'col-xs-2' },{ colName: 'AdiSoyadi',   colTitle: 'Adı Soyadı',   colWidth: 'col-xs-5' }],action: "Kisi/Edit", actionTitle: "Seç",actionIcon: "fa-edit",actinIconCSS: "btn-primary"}

    if (data != null && data) {
        var tmpHtml = "";
        var tmpActionHtml = "";
        tmpHtml += '<table class="table">';
        tmpHtml += '<thead><tr>';
        if (tableProps.action != undefined && tableProps.action != "") {
            tmpHtml += '<th></th>';
            var tmpIcon = "fa-edit";
            var tmpActinIconCSS = "btn-primary"
            var tmpActionTitle = "";
            if (tableProps.actionIcon != undefined && tableProps.actionIcon != "") {
                tmpIcon = tableProps.actionIcon;
            }
            if (tableProps.actinIconCSS != undefined && tableProps.actinIconCSS != "") {
                tmpActinIconCSS = tableProps.actionIcon;
            }
            if (tableProps.actionTitle == undefined || tableProps.actionTitle == "") {
                tmpActionTitle = tableProps.actionTitle;
            }
            tmpActionHtml = '<td><a href="' + tableProps.action + '/{0}" class="btn btn-icon-sm btn-primary xx-lmargin"><i class="fa ' + tmpIcon + '"></i>' + tmpActionTitle + '</a></td>';
        }

        for (var i = 0; i < tableProps.cols.length; i++) {
            tmpHtml += '<th class="' + tableProps.cols[i].colWidth + '">' + tableProps.cols[i].colTitle + '</th>';
        }
        tmpHtml += '</tr></thead>';
        tmpHtml += '<tbody>';
        $.each(data, function (i, dataItem) {
            tmpHtml += '<tr>';
            if (tmpActionHtml != "") { tmpHtml += tmpActionHtml.replace("{0}", dataItem[tableProps.PKcol]); }
            for (var j = 0; j < tableProps.cols.length; j++) {
                if (wsLib.isNullOrEmpty(dataItem[tableProps.cols[j].colName])) {
                    tmpHtml += '<td>-</td>';
                }
                else {
                    tmpHtml += '<td>' + dataItem[tableProps.cols[j].colName] + '</td>';
                }
            }
            tmpHtml += '</tr>';
        });
        tmpHtml += '</tbody></table>';
        $("#modalLayout .modal-body").html(tmpHtml);
        $("#modalLayout .modal-footer").html('<button type="button" class="btn btn-default" data-dismiss="modal">Vazgeç</button>');
        $('#modalLayout').modal('show');
    }
}

function showTab(tabID) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
};

function showPill(pillID) {
    $('.nav-pills a[href="#' + pillID + '"]').tab('show');
};

function manualValidation(formID) {
    $("form").validate().settings.ignore = "";
    $.validator.unobtrusive.parse($("#" + formID));

    var result = $("#" + formID).validate();
    return $("#" + formID).valid();
}


function initCustomAjax(targetContainer) {
    var containerID = ".form-ddl";
    if (!wsLib.isNullOrEmpty(targetContainer))
        containerID = targetContainer + " .form-ddl"


    $.each($(containerID), function (i, elm) {
        var targetID = $(this).attr('id');
        var tmpData = $(this).data("actiondata");
        var targetURL = $(this).data("ajaxurl");
        var targetValue = $(this).data("value");
        var optionValue = $(this).data("option");

        if (!wsLib.isNullOrEmpty(targetURL) && !wsLib.isNullOrEmpty(targetID)) {
            if (wsLib.isNullOrEmpty(tmpData)) {
                getDDLData(targetURL, null, targetID, optionValue != "" ? optionValue : null, targetValue)
            } else {
                getDDLData(targetURL, tmpData, targetID, optionValue != "" ? optionValue : null, targetValue)
            }
        }
    });
}

function initCustomValidation() {
    $.each($('form'), function (i, elm) { $.validator.unobtrusive.parse(elm); });
    $.each($('[data-custom-val-required-source]'), function (i, elm) {
        var targetElmID = $(elm).data("custom-val-required-source");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-custom-val-required-source='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {
                    if (tmpVal == String(valelm.data("custom-val-required-condition"))) {
                        valelm.attr("data-val", "true");
                        valelm.attr("data-val-required", valelm.next().text() + " alanı gereklidir.");
                    }
                    else {
                        valelm.removeData("val-required");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-custom-val-compare-source]'), function (i, elm) {
        var targetElmID = $(elm).data("custom-val-compare-source");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-custom-val-compare-source='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {
                    if (tmpVal == String(valelm.data("custom-val-compare-condition"))) {
                        var tmpOther = String(valelm.data("custom-val-compare-target"));
                        valelm.attr("data-val", "true");
                        valelm.attr("data-val-equalto", valelm.next().text() + " alanının değeri " + $("#" + tmpOther).next().text() + " alanının değeri ile eşit olmalı.");
                        valelm.attr("data-val-equalto-other", tmpOther);
                    }
                    else {
                        valelm.removeAttr("data-val-equalto");
                        valelm.removeAttr("data-val-equalto-other");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-custom-val-range-source]'), function (i, elm) {
        var targetElmID = $(elm).data("custom-val-range-source");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-custom-val-range-source='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {
                    if (tmpVal == String(valelm.data("custom-val-range-condition"))) {
                        var tmpMin = String(valelm.data("custom-val-range-min"));
                        var tmpMax = String(valelm.data("custom-val-range-max"));
                        valelm.attr("data-val", "true");
                        valelm.attr("data-val-range", valelm.next().text() + " alanı için girilen değer " + tmpMin + " ile " + tmpMax + " arasında olmalı.");
                        valelm.attr("data-val-range-min", tmpMin);
                        valelm.attr("data-val-range-max", tmpMax);
                    }
                    else {
                        valelm.removeAttr("data-val-range");
                        valelm.removeAttr("data-val-range-min");
                        valelm.removeAttr("data-val-range-max");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-custom-val-regex-source]'), function (i, elm) {
        var targetElmID = $(elm).data("custom-val-regex-source");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-custom-val-regex-source='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {
                    if (tmpVal == String(valelm.data("custom-val-regex-condition"))) {
                        valelm.attr("data-val", "true");
                        valelm.attr("data-val-regex", valelm.next().text() + " alanı için kayıt deseni uygun değil.");
                        valelm.attr("data-val-regex-pattern", String(valelm.data("custom-val-regex-pattern")));
                    }
                    else {
                        valelm.removeAttr("data-val-regex");
                        valelm.removeAttr("data-val-regex-pattern");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-display-source]'), function (i, elm) {
        var targetElmID = $(elm).data("display-source");
        var targetElm = "#" + targetElmID;
        if (targetElm != "#") {
            //if (targetElm == "#frmHizmetAlan #Cinsiyet") {}
            $(targetElm).change(function () {
                var tmpVal = getFormVal($(this));
                $.each($("[data-display-source='" + targetElmID + "']"), function (j, subElm) {
                    var valelm = $(subElm);
                    if (valelm != undefined) {
                        var tmpValeArr = String(valelm.data("display-condition")).split(',');

                        //if (tmpVal == String(valelm.data("display-condition"))) {
                        if ($.inArray(tmpVal, tmpValeArr) > -1) {
                            valelm.removeClass("hidden");
                        }
                        else {
                            valelm.addClass("hidden");
                        }
                    }
                });
            });
            $(targetElm).trigger('change');
        }
    });
    $.each($('[data-class-source]'), function (i, elm) {
        var targetElmID = $(elm).data("class-source");
        var targetElm = "#" + targetElmID;
        if (targetElm != "#") {
            $(targetElm).change(function () {
                var tmpVal = getFormVal($(this));
                $.each($("[data-class-source='" + targetElmID + "']"), function (j, subElm) {
                    var valelm = $(subElm);
                    if (valelm != undefined) {
                        if (tmpVal == String(valelm.data("class-condition"))) {
                            valelm.addClass(valelm.data("class-content"))
                        }
                        else {
                            valelm.removeClass(valelm.data("class-content"));
                        }
                    }
                });
            });
            $(targetElm).trigger('change');
        }
    });
    $.each($('[data-custom-val-required-source2]'), function (i, elm) {
        var targetElmID = $(elm).data("custom-val-required-source2");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-custom-val-required-source2='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {

                    var condition = valelm.data("custom-val-required-condition");
                    if (!wsLib.isNullOrEmpty(tmpVal)) {
                        condition = condition.replace(/Source/g, tmpVal);
                        if (eval(condition)) {
                            valelm.attr("data-val", "true");
                            valelm.attr("data-val-required", valelm.next().text() + " alanı gereklidir.");
                        }
                        else {
                            valelm.removeData("val-required");
                        }
                    }
                    else {
                        valelm.removeData("val-required");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-display-source2]'), function (i, elm) {
        var targetElmID = $(elm).data("display-source2");
        var targetElm = "#" + targetElmID;
        $(targetElm).change(function () {
            var tmpVal = getFormVal($(this));
            $.each($("[data-display-source2='" + targetElmID + "']"), function (j, subElm) {
                var valelm = $(subElm);
                if (valelm != undefined) {
                    var condition = valelm.data("display-condition");
                    if (!wsLib.isNullOrEmpty(tmpVal)) {
                        condition = condition.replace(/Source/g, tmpVal);
                        if (eval(condition)) {
                            valelm.removeClass("hidden")
                        }
                        else {
                            valelm.addClass("hidden");
                        }
                    }
                    else {
                        valelm.addClass("hidden");
                    }
                }
            });
        });
        $(targetElm).trigger('change');
    });
    $.each($('[data-display-radio-source]'), function (i, elm) {
        var targetElmName = $(elm).data("display-radio-source");
        var targetElm = "input[type=radio][name=" + targetElmName + "]"
        if (targetElm != "") {
            $(targetElm).change(function () {
                var tmpVal = getFormVal($(this));
                $.each($("[data-display-radio-source='" + targetElmName + "']"), function (j, subElm) {
                    var valelm = $(subElm);
                    if (valelm != undefined) {
                        if (tmpVal == String(valelm.data("display-condition"))) {
                            valelm.removeClass("hidden");
                        }
                        else {
                            valelm.addClass("hidden");
                        }
                    }
                });
            });
            $(targetElm).trigger('change');
        }
    });
    $.each($('[data-date-val-min]'), function (i, elm) {
        $(document).on('focusout', elm, function () {
            var tmpVal = getFormVal($(elm));
            var valMin = String($(elm).data("date-val-min"));
            if (new Date(getValidateParseDate(tmpVal)) < new Date(getValidateParseDate(valMin))) {
                showWarningNotification(valMin + "'den geçmiş bir tarih seçilemez!", "Uyarı");
                $(elm).val("");
            }
        });
        $(elm).trigger('change');
    });
    $.each($('[data-date-val-max]'), function (i, elm) {
        $(document).on('focusout', elm, function () {
            var tmpVal = getFormVal($(elm));
            var valMax = String($(elm).data("date-val-max"));
            if (new Date(getValidateParseDate(tmpVal)) > new Date(getValidateParseDate(valMax))) {
                showWarningNotification(valMax + "'den büyük bir tarih seçilemez!", "Uyarı");
                $(elm).val("");
            }
        });
        $(elm).trigger('change');
    });
}

function getFormVal(formElement) {
    var result = "";
    if (formElement.is(":checkbox")) {
        if (formElement.prop("checked")) {
            result = "true";
        }
        else {
            result = "false";
        }
    }
    else {
        result = formElement.val();
    }
    return result;
}

function reInitializeDateTimePicker() {
    var toDay = getToDay();
    $(".datetimepicker").each(function () {
        var isFuture = $(this).data("isfuture");
        if (!wsLib.isNullOrEmpty(isFuture) || isFuture == 1) {
            $(this).datepicker({ format: 'dd.mm.yyyy', language: 'tr' });
        }
        else {
            $(this).datepicker({ format: 'dd.mm.yyyy', language: 'tr', endDate: toDay });
        }

        if ($(this).val() == "01.01.0001") {
            $(this).datepicker('setDate', toDay);
        }
    });
    $(".input-daterange").datepicker({ language: 'tr' });
}

function getToDay() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    return dd + '.' + mm + '.' + yyyy;
}

function showAjaxError(x, status, error) {
    var statusMsg = "";
    var warningMsg = "";
    var redirectURL = "";
    if (x.status == 0) {
        statusMsg = "Ağ bağlantısı bulunamıyor. Lütfen ağ bağlantısının sorunsuz çalıştığını kontrol edin.";
    }
    else if (x.status == 200) {
        statusMsg = "";
    }
    else if (x.status == 400) {
        statusMsg = "Böyle bir sayfa bulunamıyor.";
    }
    else if (x.status == 403) {
        $('.modal').modal('hide');
        statusMsg = "Oturum süresi dolmuş. Lütfen kullanıcı adınız ve parolanızı kullanarak yeniden oturum açın.";
        redirectURL = "/Account/Login";
    }
    else if (x.status == 406) {
        if (x.responseText != "" && x.responseText != null && x.responseText != undefined) {
            warningMsg = x.responseText;
        }
        else if (error != "" && error != null && error != undefined) {
            warningMsg = error;
        }
        else {
            warningMsg = "İşlemi gerçekleştirmek için gereken kriterler sağlanamadı.";
        }
    }
    else {
        statusMsg = "Bir hata oluştu ağ bağlantınızın sorunsuz çalıştığını kontrol edip tekrar deneyin.";
    }
    if (statusMsg != "") {
        showErrorMessage(statusMsg);
    }
    if (warningMsg != "") {
        showWarningNotification(warningMsg, "Uyarı !");
    }
    if (redirectURL != "") {
        window.location.href = redirectURL;
    }

}

function getDDLData(actionURL, actionData, elementID, defaultValue, selectedValue, callback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: actionData, dataType: "json", type: "POST",
            success: function (data) {
                var ddlLst = $("#" + elementID);
                var tmpHTML = "";
                if (defaultValue) { tmpHTML += "<option value=''>Seçiniz...</option>"; }
                if (data != null && $.isArray(data) && data.length > 0) {
                    var isGroup = false;
                    if (!wsLib.isNullOrEmpty(data[0].Group)) {
                        isGroup = true;
                        sortJson(data, "Group", "Name", "string", true);
                    }
                    if (isGroup) {
                        var tmpGroup = "";
                        $.each(data, function (i, item) {
                            if (tmpGroup != item.Group.Name && !wsLib.isNullOrEmpty(item.Group.Name)) {
                                if (tmpGroup != "") { tmpHTML += '</optgroup>'; }
                                tmpGroup = item.Group.Name;
                                tmpHTML += '<optgroup label="' + tmpGroup + '">';
                            }
                            tmpHTML += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        });
                    }
                    else {
                        $.each(data, function (i, item) { tmpHTML += "<option value='" + item.Value + "'>" + item.Text + "</option>"; });
                    }
                    ddlLst.html(tmpHTML);
                    if (!wsLib.isNullOrEmpty(selectedValue)) { ddlLst.val(selectedValue); }
                    else {
                        var tmp = ddlLst.data("init");
                        if (tmp != null && tmp != undefined) {
                            ddlLst.val(tmp);
                        }
                        else if (defaultValue) {
                            ddlLst.val('');
                        }
                    }
                    ddlLst.removeAttr("readonly");
                    ddlLst.change(function () {
                        if (wsLib.isNullOrEmpty($(this).val())) {
                            $(this).find('option:first-child').attr("selected", "selected");
                        }
                    });
                    ddlLst.trigger("change");
                }
                else {
                    ddlLst.html(tmpHTML);
                    ddlLst.trigger("change");
                    ddlLst.attr("readonly", "readonly");
                }
                if (callback && typeof (callback) === "function") { callback(); }
            },
            error: function () {
                var ddlLst = $("#" + elementID);
                var tmpHTML = "";
                if (defaultValue) { tmpHTML += "<option value=''>Seçiniz...</option>"; }
                ddlLst.html(tmpHTML);
                ddlLst.change(function () {
                    if (wsLib.isNullOrEmpty($(this).val())) {
                        $(this).find('option:first-child').attr("selected", "selected");
                    }
                });
                ddlLst.trigger("change");
            }
        });
    }
}

function SessionDialog() {
    BootstrapDialog.show({
        message: 'Oturum bir dakika içinde otomatik olarak kapanacaktır. Oturuma devam etmek için "Oturama Devam Et" butonuna tıklayın. Devam etmek istemiyorsanız "Çıkış" butonuna tıklayın.',
        buttons: [{
            label: 'Oturuma Devam Et',
            action: function (dialogItself) { dialogItself.close(); }
        }, {
            label: 'Çıkış',
            cssClass: 'btn-primary',
            action: function (dialogItself) { dialogItself.close(); }
        }]
    });

}

function formatDate(date) {
    var month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) { month = '0' + month };
    if (day.length < 2) { day = '0' + day };

    return [day, month, year].join('.');
}

function formatDateString(date) {
    var d = new Date(parseInt(date.substr(6))),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) { month = '0' + month };
    if (day.length < 2) { day = '0' + day };

    return [day, month, year].join('.');
}

function getValidateParseDate(date) {
    var year = parseInt(date.substr(6));
    var month = parseInt(date.substr(3, 2));
    var day = parseInt(date.substr(0, 2));
    if (month.length < 2) { month = '0' + month };
    if (day.length < 2) { day = '0' + day };

    return [month, day, year].join('.');
}

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}

Date.prototype.removeDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() - days);
    return dat;
}

var autoSessionSlide;

function startSessionSliding() {
    autoSessionSlide = setTimeout(function () {
        $.ajax({ url: '/Ortak/SessionSliding', type: "GET", success: function (data) { } });
    }, 10000);
}

function stopSessionSliding() {
    clearTimeout(autoSessionSlide);
}

function CloseWindow() { window.close(); }

function showConfirmDialog(msgContent, callback) { BootstrapDialog.confirm(msgContent, callback); }
function showAlertDialog(msgContent, callback) { BootstrapDialog.alert(msgContent, callback); }

function showInfoNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, true, 0); }
function showWarningNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, true, 1); }
function showSuccessNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, true, 2); }
function showErrorNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, true, 3); }
function showInfoMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 0); }
function showWarningMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 1); }
function showSuccessMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 2); }
function showErrorMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 3); }

function showToastNotification(msgContent, msgHeader, boolNotification, msgType) {
    if (boolNotification)
        toastr.options.positionClass = "toast-top-center";
    else
        toastr.options.positionClass = "toast-top-full-width";

    toastr.options.timeOut = 5000;
    toastr.options.closeButton = true;
    toastr.options.progressBar = true;

    if (typeof msgContent == 'undefined') { msgContent = ''; }

    if (msgType == 0) {
        if (typeof msgHeader == 'undefined') { msgHeader = 'Bilgi'; }
        toastr.info(msgContent, msgHeader);
    }
    else if (msgType == 1) {
        if (typeof msgHeader == 'undefined') { msgHeader = 'Uyarı'; }
        toastr.options.timeOut = 10000;
        toastr.warning(msgContent, msgHeader);
    }
    else if (msgType == 2) {
        if (typeof msgHeader == 'undefined') { msgHeader = ''; }
        toastr.success(msgContent, msgHeader);
    }
    else if (msgType == 3) {
        if (typeof msgHeader == 'undefined') { msgHeader = 'Hata'; }
        toastr.options.timeOut = 0;
        toastr.error(msgContent, msgHeader);
    }
}
function getData(actionURL, actionData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: actionData, type: "GET",
            success: function (data) {
                if (data != null && data) {
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
                    }
                }
            }
        });
    }
}
function postData(actionURL, actionData, showMessage, customMessage, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        var message = "Kayıt işlemi başarı ile gerçekleşti.";
        if (!wsLib.isNullOrEmpty(customMessage)) { message = customMessage; }
        $.ajax({
            url: actionURL, data: actionData, type: "POST",
            success: function (data) {
                if (data != null && data) {
                    if (showMessage) { showSuccessNotification(message, ""); }
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
                        showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata");
                    }
                }
            }
        });
    }
}
function postDataWithConfirmation(options) {
    let confirmOptions = {
        confirmationMessage: options.confirmMessage,
        callback: function (confirmation) {
            if (confirmation) {

                let postOptions = {
                    actionUrl: options.url,
                    actionData: options.request,
                    showMessage: options.showMessage || false,
                    customMessage: options.customMessage,
                    successCallback: options.success,
                    errorCallback: options.error,
                };

                postData(postOptions.actionUrl, postOptions.actionData, postOptions.showMessage,
                    postOptions.customMessage, postOptions.successCallback, postOptions.errorCallback);
            }
        }
    };

    showConfirmDialog(confirmOptions.confirmationMessage, confirmOptions.callback);
}
function postFormData(actionURL, formElementID, showMessage, customMessage, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID)) {
        var requestData = $("#" + formElementID).serializeArray();
        var message = "Kayıt işlemi başarı ile gerçekleşti.";
        if (wsLib.isNullOrEmpty(customMessage)) { message = customMessage; }
        $.ajax({
            url: actionURL, data: requestData, type: "POST",
            success: function (data) {
                if (data != null && data) {
                    if (showMessage) { showSuccessNotification(message, ""); }
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
                } else {
                    showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata");
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); }
                }
            }
        });
    }
}
function getPartialView(actionURL, actionData, targetElementID, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(targetElementID)) {
        $.ajax({
            url: actionURL, data: actionData, type: "GET",
            success: function (data) {
                if (data != null && data) {
                    $("#" + targetElementID).html(data);
                    reInitializeDateTimePicker();
                    initCustomValidation();
                    if (successCallback && typeof (successCallback) === "function") {
                        successCallback();
                    }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
                        showErrorNotification("İçerik yüklenirken bir hata oluştu.", "Hata");
                    }
                }
            }
        });
    }
}

function getPartialViewWithModal(actionURL, actionData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(targetElementID)) {
        $.ajax({
            url: actionURL, data: actionData, type: "GET",
            success: function (data) {
                if (data != null && data) {
                    $("#modalLayout .modal-body").html(data);
                    reInitializeDateTimePicker();
                    initCustomValidation();
                    $('#modalLayout').modal('show');
                    if (successCallback && typeof (successCallback) === "function") {
                        successCallback();
                    }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
                        showErrorNotification("İçerik yüklenirken bir hata oluştu.", "Hata");
                    }
                }
            }
        });
    }
}

function postPartialView(actionURL, formElementID, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID)) {
        if ($("#" + formElementID).valid()) {
            var requestData = $("#" + formElementID).serializeArray();
            $.ajax({
                url: actionURL, data: requestData, type: "POST",
                success: function (data) {
                    if (data != null && data) {
                        if (successCallback && typeof (successCallback) === "function") { successCallback(data); } else { showSuccessNotification("Kayıt işlemi başarı ile gerçekleşti.", ""); }
                    } else {
                        if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else { showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata"); }
                    }
                }
            });
        }
    }
}

function postPartialViewWithGivenData(actionURL, requestData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: requestData, type: "POST",
            success: function (data) {
                if (data != null && data) {
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); } else { showSuccessNotification("İşlem başarı ile gerçekleşti.", ""); }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else { showErrorNotification("İşlem gerçekleştirilemedi.", "Hata"); }
                }
            }
        });
    }
}

function postAndGetPartialView(actionURL, formElementID, targetElementID, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID) && !wsLib.isNullOrEmpty(targetElementID)) {
        if ($("#" + formElementID).valid()) {
            var requestData = $("#" + formElementID).serializeArray();
            $.ajax({
                url: actionURL, data: requestData, type: "POST",
                success: function (data) {
                    if (data != null && data) {
                        $("#" + targetElementID).html(data);
                        reInitializeDateTimePicker();
                        initCustomValidation();
                        showSuccessNotification("Kayıt işlemi başarı ile gerçekleşti.", "");
                        if (successCallback && typeof (successCallback) === "function") { successCallback(); }
                    } else {
                        showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata");
                        if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); }
                    }
                }
            });
        }
    }
}

function postPartialViewWithGivenDataFile(actionURL, requestData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: requestData, type: "POST", contentType: false, processData: false,
            success: function (data) {
                if (data != null && data) {
                    if (data.Success != null) {
                        if (data.Success) {
                            if (data.ResponseMessage != null && data.ResponseMessage != "") { showSuccessNotification(data.ResponseMessage); }
                            else { showSuccessNotification("Kayıt işlemi başarı ile gerçekleşti.", ""); }
                        }
                        else {
                            if (data.ResponseMessage != null && data.ResponseMessage != "") { showWarningNotification(data.ResponseMessage, "Uyarı!"); }
                            else { showWarningNotification("Kayıt işlemi başarı ile gerçekleşti.", "Uyarı!"); }
                        }
                    }
                    else {
                        if (data.ResponseMessage != null && data.ResponseMessage != "") { showSuccessNotification(data.ResponseMessage); }
                        else { showSuccessNotification("Kayıt işlemi başarı ile gerçekleşti.", ""); }
                    }
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
                }
                else {
                    showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata");
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); }
                }
            }
        });
    }
}

function postPartialViewWithGivenDataFileWithoutNotification(actionURL, requestData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        $.ajax({
            url: actionURL, data: requestData, type: "POST", contentType: false, processData: false,
            success: function (data) {
                if (data != null && data) {
                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
                } else {
                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); }
                }
            }
        });
    }
}

function sortJson(element, prop, childProp, propType, asc) {
    if (propType == "int") {
        element = element.sort(function (a, b) {
            if (asc) {
                if (wsLib.isNullOrEmpty(childProp)) {
                    return (parseInt(a[prop]) > parseInt(b[prop])) ? 1 : ((parseInt(a[prop]) < parseInt(b[prop])) ? -1 : 0);
                } else {
                    return (parseInt(a[prop][childProp]) > parseInt(b[prop][childProp])) ? 1 : ((parseInt(a[prop][childProp]) < parseInt(b[prop][childProp])) ? -1 : 0);
                }
            } else {
                if (wsLib.isNullOrEmpty(childProp)) {
                    return (parseInt(b[prop]) > parseInt(a[prop])) ? 1 : ((parseInt(b[prop]) < parseInt(a[prop])) ? -1 : 0);
                } else {
                    return (parseInt(b[prop][childProp]) > parseInt(a[prop][childProp])) ? 1 : ((parseInt(b[prop][childProp]) < parseInt(a[prop][childProp])) ? -1 : 0);
                }
            }
        });
    }
    else if (propType == "string") {
        element = element.sort(function (a, b) {
            if (asc) {
                if (wsLib.isNullOrEmpty(childProp)) {
                    return (a[prop].toLowerCase() > b[prop].toLowerCase()) ? 1 : ((a[prop].toLowerCase() < b[prop].toLowerCase()) ? -1 : 0);
                }
                else {
                    return (a[prop][childProp].toLowerCase() > b[prop][childProp].toLowerCase()) ? 1 : ((a[prop][childProp].toLowerCase() < b[prop][childProp].toLowerCase()) ? -1 : 0);
                }
            } else {
                if (wsLib.isNullOrEmpty(childProp)) {
                    return (b[prop].toLowerCase() > a[prop].toLowerCase()) ? 1 : ((b[prop].toLowerCase() < a[prop].toLowerCase()) ? -1 : 0);
                } else {
                    return (b[prop][childProp].toLowerCase() > a[prop][childProp].toLowerCase()) ? 1 : ((b[prop][childProp].toLowerCase() < a[prop][childProp].toLowerCase()) ? -1 : 0);
                }
            }
        });
    }
}


function bindProp(containerID, data) {
    $.each(data, function (propName, propValue) {
        var tmpElm = $("#" + containerID).find("[data-bindprop='" + propName + "']");
        if (tmpElm.length > 0) {
            $.each(tmpElm, function (index, element) {
                var tag = element.tagName.toString().toLowerCase();
                if (tag == "input" || tag == "select" || tag == "textarea") {
                    $(element).val(propValue);
                }
                else {
                    $(element).text(propValue);
                }
            });

        }
    });
}

function bindData(containerID, data) {
    $.each(data, function (propName, propValue) {
        var tmpElm = $("#" + containerID).find("#" + propName);
        if (tmpElm.length > 0) {
            var tag = tmpElm[0].tagName.toString().toLowerCase();
            if (tag == "input" || tag == "select" || tag == "textarea") {
                tmpElm.val(propValue);
            }
            else {
                tmpElm.text(propValue);
            }
        }
    });
    reInitializeDateTimePicker();
    initCustomValidation();
}

wsLib = function () {
    function isStorageSupport() {
        try {
            return 'localStorage' in window && window['localStorage'] !== null;
        } catch (e) { return false; }
    }
    function setSession(myKey, myValue) { window.sessionStorage.setItem(myKey, myValue); }
    function getSession(myKey) { return window.sessionStorage.getItem(myKey); }
    function clearSession() { window.sessionStorage.clear(); }
    function keySession(myKey) { return window.sessionStorage.key(myKey); }
    function removeSession(myKey) { window.sessionStorage.removeItem(myKey); }
    function lengthSession() { return window.sessionStorage.length; }

    function setLocal(myKey, myValue) { window.localStorage.setItem(myKey, myValue); }
    function getLocal(myKey) { return window.localStorage.getItem(myKey); }
    function clearLocal() { window.localStorage.clear(); }
    function keyLocal(myKey) { return window.localStorage.key(myKey); }
    function removeLocal(myKey) { window.localStorage.removeItem(myKey); }
    function lengthLocal() { return window.localStorage.length; }

    function el(element) { return document.querySelector(element); }
    function trim(str) { return str.replace(/\s/g, ''); }
    function log(myLog) { console.log(myLog); }
    function isNullOrEmpty(value) { if (value == "" || value == null || value == undefined) { return true; } else { return false; } }
    function isGuidEmpty(value) { if (value == "00000000-0000-0000-0000-000000000000") { return true; } else { return false; } }
    function getWeekOfDay(value) {
        var dayOfWeek = value.getDay(); var weekday = new Array(7);
        weekday[0] = "Pazartesi"; weekday[1] = "Salı"; weekday[2] = "Çarşamba"; weekday[3] = "Perşembe"; weekday[4] = "Cuma"; weekday[5] = "Cumartesi"; weekday[6] = "Pazar";
        return weekday[dayOfWeek - 1];
    }
    function getYas(dogumTarihi) {
        var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
        var dt = new Date(dogumTarihi.replace(pattern, '$3-$2-$1'));
        var diff_ms = Date.now() - dt.getTime();
        var age_dt = new Date(diff_ms);
        return Math.abs(age_dt.getUTCFullYear() - 1970);
    }
    function getRequestVerificationToken(form) {
        form = form || $('form');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
        return token;
    }
    function setRequestVerificationToken(request, form) {
        request.__RequestVerificationToken = getRequestVerificationToken(form);
        return request;
    }


    return {
        sp: isStorageSupport,
        setSession: setSession,
        getSession: getSession,
        clearSession: clearSession,
        keySession: keySession,
        removeSession: removeSession,
        lengthSession: lengthSession,
        setLocal: setLocal,
        getLocal: getLocal,
        clearLocal: clearLocal,
        keyLocal: keyLocal,
        removeLocal: removeLocal,
        lengthLocal: lengthLocal,
        el: el,
        trim: trim,
        log: log,
        isNullOrEmpty: isNullOrEmpty,
        isGuidEmpty: isGuidEmpty,
        getWeekOfDay: getWeekOfDay,
        getYas: getYas,
        getRequestVerificationToken: getRequestVerificationToken,
        setRequestVerificationToken: setRequestVerificationToken
    }
}();
