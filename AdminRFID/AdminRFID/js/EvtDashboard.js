$(document).ready(function () {
    $(".btnGraficoSalidasProductos").click(function (evt) {
        evt.preventDefault();
        $(".btnGraficoSalidasProductos").removeClass("active");
        $(this).addClass("active");
        CargaGrafico(2, $(this).attr("tipoReporteGrafico"), "GraficoSalidasProductos");

    });

    $(".btnGraficoEntradasProductos").click(function (evt) {
        evt.preventDefault();
        $(".btnGraficoEntradasProductos").removeClass("active");
        $(this).addClass("active");
        CargaGrafico(1, $(this).attr("tipoReporteGrafico"), "GraficoEntradasProductos");

    });


    $(".btnGraficoTopTenEntradas").click(function (evt) {
        evt.preventDefault();
        $(".btnGraficoTopTenEntradas").removeClass("active");
        $(this).addClass("active");
        CargaGrafico(3, $(this).attr("tipoReporteGrafico"), "GraficoTopTenEntradas");

    });


    $(".btnGraficoTopTenSalidas").click(function (evt) {
        evt.preventDefault();
        $(".btnGraficoTopTenSalidas").removeClass("active");
        $(this).addClass("active");
        CargaGrafico(4, $(this).attr("tipoReporteGrafico"), "GraficoTopTenSalidas");

    });

    Highcharts.setOptions({
        lang: {
            loading: 'Cargando...',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            exportButtonTitle: "Exportar",
            printButtonTitle: "Importar",
            rangeSelectorFrom: "Desde",
            rangeSelectorTo: "Hasta",
            rangeSelectorZoom: "Período",
            downloadPNG: 'Descargar imagen PNG',
            downloadJPEG: 'Descargar imagen JPEG',
            downloadPDF: 'Descargar imagen PDF',
            downloadSVG: 'Descargar imagen SVG',
            printChart: 'Imprimir',
            resetZoom: 'Reiniciar zoom',
            resetZoomTitle: 'Reiniciar zoom',
            thousandsSep: ",",
            decimalPoint: '.'
        }
    });

});

function CargaGrafico(tipoGrafico, tipoReporteGrafico, nameDiv) {
    $.ajax({
        url: rootUrl("/DashBoard/_Grafico"),
        data: { tipoGrafico: tipoGrafico, tipoReporteGrafico: tipoReporteGrafico },
        method: 'post',
        dataType: 'html',
        async: true,
        beforeSend: function (xhr) {
            ShowLoader();
        },
        success: function (data) {
            $("#" + nameDiv).html(data);
            OcultarLoader();

        },
        error: function (xhr, status) {
            OcultarLoader();
        }
    });

}