function SeralizeForm(form) {
    return form.serialize();
}

function ShowSuccessNotification(message) {
    toastr["success"](message)
}
function ShowInfoNotification(message) {
    toastr["info"](message)
}
function ShowErrorNotification(message) {
    toastr["error"](message)
}
function ShowWarningNotification(message) {
    toastr["warning"](message)
}

$(document).ready(function () {
    $('.drop-down').select2({
        placeholder: '--Select--'
    });
});