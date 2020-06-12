var tblInventario;
$(document).ready(function () {
    InitRangePicker('rangeInventario', 'fechaIni', 'fechaFin');
    $('#fechaIni').val($('#rangeInventario').data('daterangepicker').startDate.format('YYYY-MM-DD'));
    $('#fechaFin').val($('#rangeInventario').data('daterangepicker').startDate.format('YYYY-MM-DD'));
    $("#frmInventario").submit();
    //InitTableInventario();
    //$('#rangeInventario').val('');

    $("#btnLimpiarForm").click(function (evt) {
        $("#frmInventario").trigger("reset");
        $('#fechaIni').val('');
        $('#fechaFin').val('');      

    });
});

function InitTableInventario() {
    var NombreTabla = "tblInventario";
    tblInventario = initDataTable(NombreTabla);

    new $.fn.dataTable.Buttons(tblInventario, {
        buttons: [
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a PDF',
                title: "Reporte Inventario",
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 10;
                    doc.defaultStyle.alignment = 'center';
                    doc.content[1].table.widths = ['20%', '30%', '10%', '10%', '10%', '20%'];
                    doc.pageMargins = [30, 85, 20, 30];
                    doc.content.splice(0, 1);
                    doc['header'] = SetHeaderPDF("Reporte Inventario");
                    doc['footer'] = (function (page, pages) { return setFooterPDF(page, pages) });
                },
                exportOptions: {
                    columns: [0,1, 2, 3, 4, 5]
                },
            },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a Excel',
                exportOptions: {
                    columns: [0,1, 2, 3, 4, 5]
                },
            },
        ],
    });

    tblInventario.buttons(0, null).container().prependTo(
        tblInventario.table().container()
    );

}

function onBeginSubmitInventario() {
    ShowLoader("Buscando...");
}

function onSuccessResultInventario(data) {
    $("#viewInventario").html(data);
    if ($("#tblInventario").length > 0) {
        if (tblInventario != null)
            tblInventario.destroy();
        InitTableInventario();
    }

    OcultarLoader();
}
function onFailureResultInventario() {
    OcultarLoader();
}
