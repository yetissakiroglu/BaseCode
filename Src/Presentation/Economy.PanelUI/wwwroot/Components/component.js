
var ajaxCount = 0;
$.ajaxSetup({
    cache: false,
    beforeSend: function () { ajaxCount++; if (ajaxCount == 1) { $.blockUI({ message: '  <h6> ... İşleniyor ... </h6>' }); } },
    complete: function () { ajaxCount--; if (ajaxCount < 1) { ajaxCount = 0; $.unblockUI(); } },
    error: function (x, status, error) {/* showAjaxError(x, status, error);*/ }
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

};
//function showAjaxError(x, status, error) {
//    var statusMsg = "";
//    var warningMsg = "";
//    var redirectURL = "";
//    if (x.status == 0) {
//        statusMsg = "Ağ bağlantısı bulunamıyor yada yoğunluk nedeni ile data alınamıyor. Lütfen ağ bağlantısının sorunsuz çalıştığını kontrol edin, eğer bağlantınızda bir sorun yoksa bir süre bekleyiniz.";
//    }
//    else if (x.status == 200) {
//        statusMsg = "";
//    }
//    else if (x.status == 400) {
//        statusMsg = "Böyle bir servis bulunamıyor.";
//    }
//    else if (x.status == 405) {
//        statusMsg = "Metot Hatası.";
//    }
//    else if (x.status == 403) {
//        statusMsg = "Oturum süresi dolmuş. Lütfen kullanıcı adınız ve parolanızı kullanarak yeniden oturum açın.";
//    }
//    else if (x.status == 406) {
//        if (x.responseText != "" && x.responseText != null && x.responseText != undefined) {
//            warningMsg = x.responseText;
//        }
//        else if (error != "" && error != null && error != undefined) {
//            warningMsg = error;
//        }
//        else {
//            warningMsg = "İşlemi gerçekleştirmek için gereken kriterler sağlanamadı.";
//        }
//    }
//    else {
//        statusMsg = "Ağ bağlantısı bulunamıyor yada yoğunluk nedeni ile data alınamıyor. Lütfen ağ bağlantısının sorunsuz çalıştığını kontrol edin, eğer bağlantınızda bir sorun yoksa bir süre bekleyiniz.";
//    }
//    if (statusMsg != "") {
//        showErrorMessage(statusMsg);
//    }
//    if (warningMsg != "") {
//        showWarningNotification(warningMsg, "Uyarı !");
//    }
//    if (redirectURL != "") {
//        window.location.href = redirectURL;
//    }

//}


function showInfoNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 1); }
function showWarningNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 2); }
function showSuccessNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 0); }
function showErrorNotification(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 3); }
function showInfoMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 1); }
function showWarningMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 2); }
function showSuccessMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 0); }
function showErrorMessage(msgContent, msgHeader) { showToastNotification(msgContent, msgHeader, false, 3); }
function showToastNotification(msgContent, msgHeader, boolNotification, msgType) {
    if (typeof msgContent === 'undefined') msgContent = '';
    if (typeof msgHeader === 'undefined') msgHeader = '';

    let bgColor;
    let duration = 5000;

    switch (msgType) {
        case 0: // Success
            bgColor = "#28a745"; // Yeşil
            if (!msgHeader) msgHeader = "Başarılı";
            break;
        case 1: // Information
            bgColor = "#17a2b8"; // Mavi
            if (!msgHeader) msgHeader = "Bilgi";
            break;
        case 2: // Warning
            bgColor = "#ffc107"; // Sarı
            if (!msgHeader) msgHeader = "Uyarı";
            break;
        case 3: // Danger
            bgColor = "#dc3545"; // Kırmızı
            duration = 0; // Hata mesajları kapanmaz
            if (!msgHeader) msgHeader = "Hata";
            break;
        default:
            bgColor = "#6c757d"; // Gri (secondary)
            break;
    }

    if (boolNotification)
        Toastify({
            text: msgHeader ? `<strong>${msgHeader}</strong>: ${msgContent}` : msgContent,
            duration: duration,
            gravity: "centerToast",
            position: "center",
            close: true,
            escapeMarkup: false, // HTML desteği
            style: {
                background: bgColor
            }
        }).showToast();
    else
        Toastify({
            text: msgHeader ? `<strong>${msgHeader}</strong>: ${msgContent}` : msgContent,
            duration: duration,
            gravity: "top",
            position: "right",
            close: true,
            escapeMarkup: false, // HTML desteği
            style: {
                background: bgColor
            }
        }).showToast();

    
}


function postResponseModelDelete(actionURL, actionData, successCallback, errorCallback) {
    if (!wsLib.isNullOrEmpty(actionURL)) {
        Swal.fire({
            title: 'Emin misiniz?',
            text: "Bu işlemi geri alamazsınız!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, sil!',
            cancelButtonText: 'Vazgeç'
        }).then((result) => {
            if (result.isConfirmed) {
                var token = $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();
                actionData.__RequestVerificationToken = token;

                $.ajax({
                    url: actionURL,
                    data: actionData,
                    type: "POST",
                    success: function (response) {
                        if (response.isSuccess) {
                            // Başarılı mesajı varsa göster
                            if (response.message) {
                                showSuccessNotification(response.message, "Başarılı");
                            } else {
                                showSuccessNotification("İşlem başarılı.", "Başarılı");
                            }

                            if (typeof successCallback === "function") {
                                successCallback(response);
                            }

                            // Yönlendirme
                            if (response.redirectUrl) {
                                window.location.href = response.redirectUrl;
                            }

                        } else {
                            let errorMessages = [];

                            // ValidationErrors varsa detaylı göster
                            if (response.validationErrors) {
                                for (const field in response.validationErrors) {
                                    const messages = response.validationErrors[field];
                                    if (Array.isArray(messages)) {
                                        errorMessages.push(`<b>${field}</b>: ${messages.join(", ")}`);
                                    }
                                }
                            }

                            // Genel Errors varsa
                            if (response.errors && response.errors.length > 0) {
                                errorMessages = errorMessages.concat(response.errors);
                            }

                            // Hiçbir mesaj yoksa default
                            if (errorMessages.length === 0) {
                                errorMessages.push("İşlem sırasında bir hata oluştu.");
                            }

                            showErrorNotification(errorMessages.join("<br>"), "Hata");

                            if (typeof errorCallback === "function") {
                                errorCallback(response);
                            }
                        }
                    },
                    error: function () {
                        showErrorNotification("Sunucu ile iletişim sırasında bir hata oluştu.", "Hata");
                    }
                });
            }
        });
    }
}




wsLib = function () {
    function isNullOrEmpty(value) { if (value == "" || value == null || value == undefined) { return true; } else { return false; } }
    return {
        isNullOrEmpty: isNullOrEmpty
    }
}();




//$(".sanitizedName").keyup(function () {
//    var titleValue = $(this).val();
//    if (titleValue) {
//        var sanitizedURL = titleValue
//            .replace(/[^a-zA-Z0-9-üÜıİöÖşŞğĞçÇ]/g, "-")
//            .replace(/-{2,}/g, "-")
//            .replace(/ü/g, "u")
//            .replace(/Ü/g, "U")
//            .replace(/ı/g, "i")
//            .replace(/İ/g, "I")
//            .replace(/ö/g, "o")
//            .replace(/Ö/g, "O")
//            .replace(/ş/g, "s")
//            .replace(/Ş/g, "S")
//            .replace(/ğ/g, "g")
//            .replace(/Ğ/g, "G")
//            .replace(/ç/g, "c")
//            .replace(/Ç/g, "C")
//            .toLowerCase();
//        $(".sanitizedName").val(sanitizedURL);
//    }
//});

//function sweetConfirm(title, text, type, confirmText, cancelText, callback) {
//    Swal.fire({
//        title: title,
//        text: text,
//        type: type,
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        cancelButtonText: cancelText,
//        confirmButtonText: confirmText,
//    }).then((result) => {
//        return callback(result.value);
//    });
//};

//// --- Response Formatında Post ------
//function postDataResponse(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "POST",
//            success: function (data) {
//                if (data.success) {
//                    if (successCallback && typeof (successCallback) === "function") {
//                        showSuccessNotification("İşlem başarı ile gerçekleşti. </br> Mesaj : " + data.message, "Başarılı");
//                        successCallback(data);
//                    }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. </br> Mesaj : " + data.message, "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//            }
//        });
//    }
//}
//function postDataJsonResponse(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "POST",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            processData: false,
//            contentType: false,
//            success: function (data) {
//                if (data.success) {
//                    if (successCallback && typeof (successCallback) === "function") {
//                        showSuccessNotification("İşlem başarı ile gerçekleşti. </br> Mesaj : " + data.message, "Başarılı");
//                        successCallback(data);
//                    }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. </br> Mesaj : " + data.message, "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//            }
//        });
//    }
//}
//function postFormDataResponse(actionURL, formElementID, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID)) {
//        var requestData = $("#" + formElementID).serializeArray();
//        $.ajax({
//            url: actionURL, data: requestData, type: "POST",
//            success: function (data) {
//                if (data.success) {
//                    if (successCallback && typeof (successCallback) === "function") {
//                        showSuccessNotification("İşlem başarı ile gerçekleşti. </br> Mesaj : " + data.message, "Başarılı");
//                        successCallback(data);
//                    }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. </br> Mesaj : " + data.message, "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//            }
//        });

//    }
//}
//function getPartialView(actionURL, actionData, targetElementID, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(targetElementID)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "GET",
//            success: function (data) {
//                if (data != null && data) {
//                    $("#" + targetElementID).html(data);
//                    if (successCallback && typeof (successCallback) === "function") {
//                        successCallback();
//                    }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İçerik yüklenirken bir hata oluştu.", "Hata");
//                    }
//                }
//            }
//        });
//    }
//}

//// -- --------------------- -------------







//var panel = {
//    setFormDataField: (formData, field, fieldData) => {
//        const value = formData.get(field);
//        formData.set(field, fieldData);
//    },
//    convertDateFields: (formData, fields) => {
//        fields.forEach(field => {
//            const value = formData.get(field);
//            if (value) {
//                const [day, month, year] = value.split('.');
//                const date = new Date(year, month - 1, day);
//                const dateString = `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()}`;
//                formData.set(field, dateString);
//            }
//        });
//    },
//    displayErrors: (data) => {
//        const errorElements = document.querySelectorAll('[data-valmsg-for]');
//        errorElements.forEach((errorElement) => {
//            const key = errorElement.getAttribute('data-valmsg-for');
//            if (data.errors[key]) {
//                errorElement.innerText = data.errors[key][0];
//            } else {
//                errorElement.innerText = '';
//            }
//        });
//    },
//    getErrorMessage: (data) => {
//        if (panel.isArrayNotEmpty(data.errors)) {
//            const errorValues = Object.values(data.errors);
//            return errorValues.join(',</br> ');
//        }
//        return null;
//    }
//    ,
//    isArrayNotEmpty: (arr) => {
//        if (Array.isArray(arr)) {
//            return arr.length > 0;
//        }
//        return false;
//    },

//    formatDateFormatterString: (ms) => {
//        const options = {
//            year: 'numeric',
//            month: 'long',
//            day: 'numeric',
//            hour: 'numeric',
//            minute: 'numeric'
//        };
//        if (!ms) {
//            throw "Date null or undefined"
//        }
//        const d = new Date(ms)
//            .toLocaleDateString("tr-TR", options)
//            .replace(/ /g, ' ');
//        return d;
//    },
//    formatDateFormatterNotHourString: (ms) => {
//        const options = {
//            year: 'numeric',
//            month: 'long',
//            day: 'numeric'
//        };
//        if (!ms) {
//            throw "Date null or undefined"
//        }
//        const d = new Date(ms)
//            .toLocaleDateString("tr-TR", options)
//            .replace(/ /g, ' ');
//        return d;
//    },
//    sweetConfirm: (title, text, type, confirmText, cancelText, callback) => {
//        Swal.fire({
//            title: title,
//            text: text,
//            type: type,
//            showCancelButton: true,
//            confirmButtonColor: '#3085d6',
//            cancelButtonColor: '#d33',
//            cancelButtonText: cancelText,
//            confirmButtonText: confirmText,
//        }).then((result) => {
//            return callback(result.value);
//        });
//    }

//};


//$('.blockUIModalBtn').click(function () {
//    KTApp.block('.modalcontent', {
//        overlayColor: '#000000',
//        type: 'v2',
//        state: 'success',
//        message: 'Lütfen Bekleyin...'
//    });

//    setTimeout(function () {
//        KTApp.unblock('.modalcontent');
//    }, 3000);
//});

//// Write your Javascript code.

//function sweetConfirm(title, text, type, confirmText, cancelText, callback) {
//    Swal.fire({
//        title: title,
//        text: text,
//        type: type,
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        cancelButtonText: cancelText,
//        confirmButtonText: confirmText,
//    }).then((result) => {
//        return callback(result.value);
//    });
//};
//$("#gBack").click(function () { window.history.go(-1); return false; });


//$(".search-input").keydown(function (e) {
//    if (e.which == 13) {
//        $(".search-btn").trigger("click");
//        return false;
//    }
//});

//// --- Response Formatında Post Json ------
//function postDataResponseJson(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "POST",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                if (data != null && data.isSuccessful) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data.data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. </br> Request : " + actionURL + "</br > Data : " + actionData + "</br>  Uyarı : </br>" + panel.getErrorMessage(data), "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                var data = err.responseJSON;
//                showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. </br> Request : " + actionURL + "</br > Data : " + actionData + "</br>  Uyarı : </br>" + panel.getErrorMessage(data), "Hata");
//            }
//        });
//    }
//}
////-----------------------------------------

//function getDataResponse(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "GET",
//            success: function (data) {
//                if (data != null && data.data) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data.data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. Request : " + actionURL + "</br> Data : " + actionData, "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                showErrorNotification("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//                console.log("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//            }
//        });
//    }
//}
//function postDataResponse(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "POST",
//            success: function (data) {
//                if (data != null && data.data) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data.data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu. Request : " + actionURL + "</br> Data : " + actionData, "Hata");
//                    }
//                }
//            },
//            error: function (err) {
//                showErrorNotification("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//                console.log("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//            }
//        });
//    }
//}



//function getDDLData(actionURL, actionData, elementID, defaultValue, selectedValue, callback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, dataType: "json", type: "Get",
//            success: function (data) {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value='-1'> -- Seçiniz -- </option>"; }
//                if (data != null && $.isArray(data) && data.length > 0) {
//                    var isGroup = false;
//                    if (!wsLib.isNullOrEmpty(data[0].group)) {
//                        isGroup = true;
//                        sortJson(data, "group", "name", "string", true);
//                    }
//                    if (isGroup) {
//                        var tmpGroup = "";
//                        $.each(data, function (i, item) {
//                            if (tmpGroup != item.group.name && !wsLib.isNullOrEmpty(item.group.name)) {
//                                if (tmpGroup != "") { tmpHTML += '</optgroup>'; }
//                                tmpGroup = item.group.name;
//                                tmpHTML += '<optgroup label="' + tmpGroup + '">';
//                            }
//                            tmpHTML += "<option value='" + item.value + "'>" + item.text + "</option>";
//                        });
//                    }
//                    else {
//                        $.each(data, function (i, item) { tmpHTML += "<option value='" + item.value + "'>" + item.text + "</option>"; });
//                    }
//                    ddlLst.html(tmpHTML);
//                    if (!wsLib.isNullOrEmpty(selectedValue)) { ddlLst.val(selectedValue); }
//                    else {
//                        var tmp = ddlLst.data("init");
//                        if (tmp != null && tmp != undefined) {
//                            ddlLst.val(tmp);
//                        }
//                        else if (defaultValue) {
//                            ddlLst.val('');
//                        }
//                    }
//                    ddlLst.removeAttr("readonly");
//                    ddlLst.change(function () {
//                        if (wsLib.isNullOrEmpty($(this).val())) {
//                            $(this).find('option:first-child').attr("selected", "selected");
//                        }
//                    });
//                    ddlLst.trigger("change");
//                }
//                else {
//                    ddlLst.html(tmpHTML);
//                    ddlLst.trigger("change");
//                    ddlLst.attr("readonly", "readonly");
//                }
//                if (callback && typeof (callback) === "function") { callback(); }
//            },
//            error: function () {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value=''>Seçiniz...</option>"; }
//                ddlLst.html(tmpHTML);
//                ddlLst.change(function () {
//                    if (wsLib.isNullOrEmpty($(this).val())) {
//                        $(this).find('option:first-child').attr("selected", "selected");
//                    }
//                });
//                ddlLst.trigger("change");
//            }
//        });
//    }
//}
//function getDDLDataUnder(actionURL, actionData, elementID, defaultValue, selectedValue, callback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, dataType: "json", type: "Get",
//            success: function (data) {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value=''> -- Seçiniz... -- </option>"; }
//                if (data != null && $.isArray(data) && data.length > 0) {
//                    var isGroup = false;
//                    if (!wsLib.isNullOrEmpty(data[0].Group)) {
//                        isGroup = true;
//                        sortJson(data, "Group", "Name", "string", true);
//                    }
//                    if (isGroup) {
//                        var tmpGroup = "";
//                        $.each(data, function (i, item) {
//                            if (tmpGroup != item.Group.Name && !wsLib.isNullOrEmpty(item.Group.Name)) {
//                                if (tmpGroup != "") { tmpHTML += '</optgroup>'; }
//                                tmpGroup = item.Group.Name;
//                                tmpHTML += '<optgroup label="' + tmpGroup + '">';
//                            }
//                            tmpHTML += "<option value='" + item.value + "'>" + item.text + "</option>";
//                        });
//                    }
//                    else {
//                        tmpHTML += "<option value='-1'> -- Tümü -- </option>";
//                        $.each(data, function (i, item) {
//                            tmpHTML += "<option value='" + item.value + "'>" + item.text + "</option>";
//                        });
//                    }
//                    ddlLst.html(tmpHTML);
//                    if (!wsLib.isNullOrEmpty(selectedValue)) { ddlLst.val(selectedValue); }
//                    else {
//                        var tmp = ddlLst.data("init");
//                        if (tmp != null && tmp != undefined) {
//                            ddlLst.val(tmp);
//                        }
//                        else if (defaultValue) {
//                            ddlLst.val('');
//                        }
//                    }
//                    ddlLst.removeAttr("readonly");
//                    ddlLst.change(function () {
//                        if (wsLib.isNullOrEmpty($(this).val())) {
//                            $(this).find('option:first-child').attr("selected", "selected");
//                        }
//                    });
//                    ddlLst.trigger("change");
//                }
//                else {
//                    ddlLst.html(tmpHTML);
//                    ddlLst.trigger("change");
//                    ddlLst.attr("readonly", "readonly");
//                }
//                if (callback && typeof (callback) === "function") { callback(); }
//            },
//            error: function () {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value=''>Seçiniz...</option>"; }
//                ddlLst.html(tmpHTML);
//                ddlLst.change(function () {
//                    if (wsLib.isNullOrEmpty($(this).val())) {
//                        $(this).find('option:first-child').attr("selected", "selected");
//                    }
//                });
//                ddlLst.trigger("change");
//            }
//        });
//    }
//}

//function getData(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "GET",
//            success: function (data) {
//                if (data != null && data) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//                    }
//                }
//            }
//        });
//    }
//}
//function postData(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, type: "POST",
//            success: function (data) {
//                if (data != null && data) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//                    }
//                }
//            }
//        });
//    }
//}
//function postDataJson(actionURL, actionData, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            contentType: "application/json", url: actionURL, data: actionData, type: "POST", dataType: 'json',
//            success: function (data) {
//                if (data != null && data) {
//                    if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
//                } else {
//                    if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else {
//                        showErrorNotification("İşlem gerçekleştirilirken bir hata oluştu.", "Hata");
//                    }
//                }
//            }
//        });
//    }
//}

////getDropListData("/api/Orders/GetDefVendor", null, "vendorId", true, "Tümü");
//function getDropListData(actionURL, actionData, elementID, defaultValue, defaultName, callback) {
//    if (!wsLib.isNullOrEmpty(actionURL)) {
//        $.ajax({
//            url: actionURL, data: actionData, dataType: "json", type: "POST",
//            success: function (data) {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value=''>" + defaultName + "</option>"; }
//                if (data != null && $.isArray(data) && data.length > 0) {
//                    $.each(data, function (i, item) { tmpHTML += "<option value='" + item.value + "'>" + item.text + "</option>"; });
//                    ddlLst.html(tmpHTML);

//                    ddlLst.removeAttr("readonly");
//                    ddlLst.change(function () {
//                        if (wsLib.isNullOrEmpty($(this).val())) {
//                            $(this).find('option:first-child').attr("selected", "selected");
//                        }
//                    });
//                    ddlLst.trigger("change");
//                }
//                else {
//                    ddlLst.html(tmpHTML);
//                    ddlLst.trigger("change");
//                    ddlLst.attr("readonly", "readonly");
//                }
//                if (callback && typeof (callback) === "function") { callback(); }
//            },
//            error: function () {
//                var ddlLst = $("#" + elementID);
//                var tmpHTML = "";
//                if (defaultValue) { tmpHTML += "<option value=''>" + defaultName + "</option>"; }
//                ddlLst.html(tmpHTML);
//                ddlLst.change(function () {
//                    if (wsLib.isNullOrEmpty($(this).val())) {
//                        $(this).find('option:first-child').attr("selected", "selected");
//                    }
//                });
//                ddlLst.trigger("change");
//            }
//        });
//    }
//}
//function manualValidation(formID) {
//    var currentForm = document.getElementById(formID);
//    $.validator.unobtrusive.parse(currentForm);
//    var result = $("#" + formID).validate();
//    return $("#" + formID).valid();
//}
//function postFormData(actionURL, formElementID, showMessage, customMessage, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID)) {
//        var requestData = $("#" + formElementID).serializeArray();
//        if (manualValidation(formElementID)) {
//            var message = "Kayıt işlemi başarı ile gerçekleşti.";
//            if (wsLib.isNullOrEmpty(customMessage)) { message = customMessage; }
//            $.ajax({
//                url: actionURL, data: requestData, type: "POST",
//                success: function (data) {
//                    if (data != null && data) {
//                        if (showMessage) { showSuccessNotification(message, ""); }
//                        if (successCallback && typeof (successCallback) === "function") { successCallback(data); }
//                    } else {
//                        showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata");
//                        if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); }
//                    }
//                }
//                , error: function (err) {
//                    showErrorNotification("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//                    console.log("Hata : " + JSON.stringify(err, null, 2) + " Request : " + actionURL + "</br> Data : " + actionData);
//                }

//            });
//        }
//    }
//}
//function postPartialView(actionURL, formElementID, successCallback, errorCallback) {
//    if (!wsLib.isNullOrEmpty(actionURL) && !wsLib.isNullOrEmpty(formElementID)) {
//        if ($("#" + formElementID).valid()) {
//            var requestData = $("#" + formElementID).serializeArray();
//            $.ajax({
//                url: actionURL, data: requestData, type: "POST",
//                success: function (data) {
//                    if (data != null && data) {
//                        if (successCallback && typeof (successCallback) === "function") { successCallback(data); } else { showSuccessNotification("Kayıt işlemi başarı ile gerçekleşti.", ""); }
//                    } else {
//                        if (errorCallback && typeof (errorCallback) === "function") { errorCallback(); } else { showErrorNotification("Kayıt işlemi gerçekleştirilemedi.", "Hata"); }
//                    }
//                }
//            });
//        }
//    }
//}
//function sortJson(element, prop, childProp, propType, asc) {
//    if (propType == "int") {
//        element = element.sort(function (a, b) {
//            if (asc) {
//                if (wsLib.isNullOrEmpty(childProp)) {
//                    return (parseInt(a[prop]) > parseInt(b[prop])) ? 1 : ((parseInt(a[prop]) < parseInt(b[prop])) ? -1 : 0);
//                } else {
//                    return (parseInt(a[prop][childProp]) > parseInt(b[prop][childProp])) ? 1 : ((parseInt(a[prop][childProp]) < parseInt(b[prop][childProp])) ? -1 : 0);
//                }
//            } else {
//                if (wsLib.isNullOrEmpty(childProp)) {
//                    return (parseInt(b[prop]) > parseInt(a[prop])) ? 1 : ((parseInt(b[prop]) < parseInt(a[prop])) ? -1 : 0);
//                } else {
//                    return (parseInt(b[prop][childProp]) > parseInt(a[prop][childProp])) ? 1 : ((parseInt(b[prop][childProp]) < parseInt(a[prop][childProp])) ? -1 : 0);
//                }
//            }
//        });
//    }
//    else if (propType == "string") {
//        element = element.sort(function (a, b) {
//            if (asc) {
//                if (wsLib.isNullOrEmpty(childProp)) {
//                    return (a[prop].toLowerCase() > b[prop].toLowerCase()) ? 1 : ((a[prop].toLowerCase() < b[prop].toLowerCase()) ? -1 : 0);
//                }
//                else {
//                    return (a[prop][childProp].toLowerCase() > b[prop][childProp].toLowerCase()) ? 1 : ((a[prop][childProp].toLowerCase() < b[prop][childProp].toLowerCase()) ? -1 : 0);
//                }
//            } else {
//                if (wsLib.isNullOrEmpty(childProp)) {
//                    return (b[prop].toLowerCase() > a[prop].toLowerCase()) ? 1 : ((b[prop].toLowerCase() < a[prop].toLowerCase()) ? -1 : 0);
//                } else {
//                    return (b[prop][childProp].toLowerCase() > a[prop][childProp].toLowerCase()) ? 1 : ((b[prop][childProp].toLowerCase() < a[prop][childProp].toLowerCase()) ? -1 : 0);
//                }
//            }
//        });
//    }
//}

//Tarihleri TR Formatına Çevirir.
//function formatDateFormatterString(ms) {
//    const options = {
//        year: 'numeric',
//        month: 'long',
//        day: 'numeric',
//        hour: "2-digit",
//        minute: "2-digit"
//    };
//    if (!ms) {
//        throw "Date null or undefined"
//    }
//    const d = new Date(ms)
//        .toLocaleDateString("tr-TR", options)
//        .replace(/ /g, ' ');

//    return d;
//}


