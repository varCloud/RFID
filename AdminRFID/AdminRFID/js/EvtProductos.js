var tblProductos;
$(document).ready(function () {
    InitTableProductos();
});

function InitTableProductos() {
    var NombreTabla = "tblProductos";
    tblProductos = initDataTable(NombreTabla);

    new $.fn.dataTable.Buttons(tblProductos, {
        buttons: [
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a PDF',
                title: "Productos",
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 10;
                    doc.defaultStyle.alignment = 'center';
                    //doc.content[1].table.widths = ['10%', '20%', '20%', '15%',  '20%', '15%'];
                    doc.pageMargins = [30, 85, 20, 30];
                    doc.content.splice(0, 1);
                    doc['header'] = SetHeaderPDF("Productos");
                    doc['footer'] = (function (page, pages) { return setFooterPDF(page, pages) });
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4,5,6,7,8,9,10,11]
                },
            },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4,5,6,7,8,9,10,11]
                },
            },
        ],
    });

    tblProductos.buttons(0, null).container().prependTo(
        tblProductos.table().container()
    );

    $('#' + NombreTabla + '_filter').append('&nbsp;&nbsp;&nbsp;' + $("#DivBtnNuevoProducto").html());

}


function ConsultaProducto(idProducto) {
    $.ajax({
        url: "/Productos/_Producto",
        data: { idProducto: idProducto },
        method: 'post',
        dataType: 'html',
        async: false,
        beforeSend: function (xhr) {
            ShowLoader("Cargando...");
        },
        success: function (data) {
            OcultarLoader();
            $('#viewProducto').html(data);
            $.validator.unobtrusive.parse("#frmProducto")
            if (idProducto > 0) {
                $('#TituloModalProducto').html("Editar Producto");
                obtenerCodigos();
            }
            else
                $('#TituloModalProducto').html("Agregar Producto");
            $('#modalProducto').modal({ backdrop: 'static', keyboard: false, show: true });

            $('#codigoBarras').keyup(function () {
                obtenerCodigos();
            });

            $('#codigoBarras').focusout(function () {
                obtenerCodigos();
            });
        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar el usuario")
        }
    });
}
function onBeginSubmitGuardarProducto() {
    ShowLoader("Guardando...");
}
function onSuccessResultGuardarProducto(data) {
    OcultarLoader();
    if (data.Estatus == 200) {
        MuestraToast("success", data.Mensaje)
        PintarTabla();
    } else {
        MuestraToast("error", data.Mensaje)
    }

    $('#modalProducto').modal('hide');

}
function onFailureResultGuardarProducto() {
    OcultarLoader();
    MuestraToast("error", "Ocurrio un error al guardar")
}
function PintarTabla() {
    $.ajax({
        url: rootUrl("/Productos/_ConsultaProductos"),
        method: 'post',
        dataType: 'html',
        async: true,
        beforeSend: function (xhr) {
            ShowLoader("Consultando productos...");
        },
        success: function (data) {
            tblProductos.destroy();
            $('#viewProductos').html(data);
            InitTableProductos();
            OcultarLoader();
        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar los productos")
        }
    });
}
function EliminarProducto(idProducto) {

    swal({
        title: '',
        text: 'Estas seguro que deseas eliminar a este producto?',
        icon: '',
        buttons: ["Cancelar", "Aceptar"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Productos/ActualizaActivoProducto",
                    data: { idProducto: idProducto },
                    method: 'post',
                    dataType: 'json',
                    async: true,
                    beforeSend: function (xhr) {
                        ShowLoader("Actualizando...");
                    },
                    success: function (data) {
                        OcultarLoader();
                        if (data.Estatus == 200) {
                            MuestraToast("success", data.Mensaje)
                            tblProductos.destroy();
                            PintarTabla();
                        }
                        else {
                            MuestraToast("error", data.Mensaje)
                        }
                    },
                    error: function (xhr, status) {
                        OcultarLoader();
                        MuestraToast("error", "Ocurrio un error al actualizar eliminar el producto")
                    }
                });
            }
        });


}
function obtenerCodigos() {
    if ($('#codigoBarras').val() !== '') {
        $.ajax({
            url: rootUrl("/Productos/ObtenerCodigos"),
            data: { cadena: $('#codigoBarras').val() },
            method: 'post',
            dataType: 'json',
            async: false,
            beforeSend: function (xhr) {
                //ShowLoader("Generando codigo...");
            },
            success: function (data) {
                //OcultarLoader();
                $("#barra").attr('src', 'data:image/png;base64,' + data.barra);
                $("#qr").attr('src', 'data:image/png;base64,' + data.qr);
            },
            error: function (xhr, status) {
                //OcultarLoader();
                MuestraToast("error", "Hubo un problema al generar el codigo, contactese con el administrador del sistema")
            }
        });
    }
    else {
        $("#barra").attr('src', '');
        $("#qr").attr('src', '');
    }
}

