var key;
function onBeginSubmit() {
    ShowLoader("Cargando...");
    $("#btnIniciarSesion").addClass('btn-progress disabled');
}
function onCompleteSubmit() {

}

function onSuccessResult(data) {
    OcultarLoader();

    if (data.Estatus == 200) {
        location.href = rootUrl("" + data.Controller + "/" + data.Action+"/");
    } else {
        $("#btnIniciarSesion").removeClass('btn-progress disabled');
        InitRecaptcha();
        MuestraToast("error", data.Mensaje);
    }
}

function onFailureResult() {
    OcultarLoader();
    $("#btnIniciarSesion").removeClass('btn-progress disabled');
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