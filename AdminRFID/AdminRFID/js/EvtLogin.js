var key;
function onBeginSubmit() {
    ShowLoader("Cargando...");
}
function onCompleteSubmit() {

}

function onSuccessResult(data) {
    OcultarLoader();

    if (data.Estatus == 200) {
        location.href = rootUrl("" + data.Controller + "/" + data.Action+"/");
    } else {
        InitRecaptcha();
        MuestraToast("error", data.Mensaje);
    }
}

function onFailureResult() {
    OcultarLoader();
}
function InitRecaptcha() {
    var key = '6LfandgUAAAAAC71dmEsGltVDobKPEYFvC_ocuP_';
    grecaptcha.ready(function () {
        grecaptcha.execute(key, { action: 'homepage' }).then(function (token) {
            $("#Token").val(token);
        });
        element = document.getElementsByClassName('grecaptcha-badge');

    });
}
$(document).ready(function () {
    InitRecaptcha();



});