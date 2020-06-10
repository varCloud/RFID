var tblInventario;
$(document).ready(function () {    
    InitRangePicker('rangeInventario', 'fechaIni', 'fechaFin');    
    $('#fechaIni').val($('#rangeInventario').data('daterangepicker').startDate.format('YYYY-MM-DD'));
    $('#fechaFin').val($('#rangeInventario').data('daterangepicker').startDate.format('YYYY-MM-DD'));
    $("#frmInventarioDetalle").submit();
    //InitTableInventario();
    //$('#rangeInventario').val('');

    $("#btnLimpiarForm").click(function (evt) {
        $("#frmInventarioDetalle").trigger("reset");
        $('#fechaIni').val('');
        $('#fechaFin').val('');

    });
});

function InitTableInventario() {
    var NombreTabla = "tblInventarioDetalle";
    tblInventario = initDataTable(NombreTabla);

    new $.fn.dataTable.Buttons(tblInventario, {
        buttons: [
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a PDF',
                title: "Inventario",
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 10;
                    doc.defaultStyle.alignment = 'center';
                    doc.content[1].table.widths = ['20%', '20%', '10%', '10%','10%', '10%','10%','10%'];
                    doc.pageMargins = [30, 85, 20, 30];
                    doc.content.splice(0, 1);
                    doc['header'] = SetHeaderPDF("Inventario");
                    doc['footer'] = (function (page, pages) { return setFooterPDF(page, pages) });
                },
                exportOptions: {
                    columns: [1, 2, 3, 4,5,6,7,8]
                },
            },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a Excel',
                exportOptions: {
                    columns: [1, 2, 3, 4,5,6,7,8]
                },
            },
        ],
    });

    tblInventario.buttons(0, null).container().prependTo(
        tblInventario.table().container()
    );

    $('#' + NombreTabla + '_filter').append($("#DivBtnEntradasSalidas").html());
}

function PintarTabla() {
    $.ajax({
        url: rootUrl("/Inventario/_ObtenerInventario"),
        method: 'post',
        dataType: 'html',
        async: true,
        beforeSend: function (xhr) {
            ShowLoader("Consultando inventario...");
        },
        success: function (data) {
            tblInventario.destroy();
            $('#viewInventario').html(data);
            InitTableInventario();
            OcultarLoader();
        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar el inventario")
        }
    });
}


function onBeginSubmitInventario() {
    ShowLoader("Buscando...");
}

function onSuccessResultInventario(data) {
    $("#viewInventario").html(data);
    if ($("#tblInventarioDetalle").length > 0) {
        if (tblInventario!=null)
            tblInventario.destroy();
        InitTableInventario();
    }

    OcultarLoader();
}
function onFailureResultInventario() {
    OcultarLoader();
}

function CancelarInventario(idInventarioDetalle,tipoInventario,cantidad) {

    swal({
        title: '',
        text: 'Estas seguro que deseas cancelar la ' + tipoInventario + ' de ' + cantidad + ' productos del inventario?',
        icon: '',
        buttons: ["Cancelar", "Aceptar"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Inventario/CancelaInventario",
                    data: { idInventarioDetalle: idInventarioDetalle },
                    method: 'post',
                    dataType: 'json',
                    async: true,
                    beforeSend: function (xhr) {
                        ShowLoader("Cancelando Inventario...");
                    },
                    success: function (data) {
                        OcultarLoader();
                        if (data.Estatus == 200) {
                            MuestraToast("success", data.Mensaje)
                            tblInventario.destroy();
                            $("#frmInventarioDetalle").submit();
                            //PintarTabla();
                        }
                        else {
                            MuestraToast("error", data.Mensaje)
                        }
                    },
                    error: function (xhr, status) {
                        OcultarLoader();
                        MuestraToast("error", "Ocurrio un error al cancelar el inventario")
                    }
                });
            }
        });


}


//************** REGISTRO DE ENTRADAS Y SALIDAS **************************************//

function RegistrarInventario(idTipoInventario) {
    $.ajax({
        url: "/Inventario/_Inventario",
        data: { tipoInventario: idTipoInventario },
        method: 'post',
        dataType: 'html',
        async: false,
        beforeSend: function (xhr) {
            ShowLoader("Cargando...");
        },
        success: function (data) {
            OcultarLoader();
            $('#viewModalInventario').html(data);
            $.validator.unobtrusive.parse("#frmInventario")
            if (idTipoInventario == 1)
                $("#TituloModalInventario").html("Entrada de productos");
            else {
                $("#TituloModalInventario").html("Salida de productos");
                revisarExistenciasCombo();
            }

            $('#modalInventario').modal({ backdrop: 'static', keyboard: false, show: true });

            InitInventario();
        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar el inventario")
        }
    });
}

function InitInventario() {
    $('#idProductoInventario').select2({
        width: "100%",
        language: {
            noResults: function () {
                return "No hay resultado";
            },
            searching: function () {
                return "Buscando..";
            }
        }
    });

    $('#btnAgregarProductoInventario').click(function (e) {
        if ($('#idProductoInventario').val() <= 0) {
            MuestraToast('warning', "Debe seleccionar un producto.");
        }
        else if (($('#cantidadProductoInventario').val() == "")) {
            MuestraToast('warning', "Debe escribir la cantidad de productos que va a agregar.");
        }
        else if (($('#cantidadProductoInventario').val() <= 0)) {
            MuestraToast('warning', "La cantidad debe ser mayor que 0.");
        }
        else {

            var idProducto = $('#idProductoInventario').val();
            var cantidad = $('#cantidadProductoInventario').val();
            var existeProducto = false;

            $("#tblInventario tbody tr").each(function (index) {
                if ($(this).attr("id") == idProducto) {
                    var input = $(this).children("td").children("input");
                    var cantAct = input.val();
                    input.val(parseFloat(cantidad) + parseFloat(cantAct));
                    existeProducto = true;
                    return;
                }
            });

            if (existeProducto == false) {
                var row_ = "<tr id=" + idProducto + ">" +
                    "  <td> " + $('#idProductoInventario').val() + "</td>" +
                    "  <td> " + $("#idProductoInventario").find("option:selected").text().substr(0, $("#idProductoInventario").find("option:selected").text().indexOf('- (')) + "</td>" +
                    //"  <td class=\"text-center\">" + cantidad + "</td>" +
                    "  <td class=\"text-center\"><input type='text' onkeypress=\"return esNumero(event)\" style=\"text-align: center; border: none; border-color: transparent;  background: transparent; \" value=\"" + cantidad + "\" ></td>" +
                    "  <td class=\"text-center\">" +
                    "      <a href=\"javascript:eliminaFila(" + parseFloat(idProducto) + ")\"  data-toggle=\"tooltip\" title=\"\" data-original-title=\"Eliminar\"><i class=\"far fa-trash-alt\"></i></a>" +
                    "  </td>" +
                    "</tr>";

                $("#tblInventario tbody").append(row_);
            }

            $('#cantidadProductoInventario').val('');
            $('#idProductoInventario').val('').trigger('change');
        }
    });


    $('#btnGuardarInventario').click(function (e) {

        var productos = [];
        $('#tblInventario tbody tr').each(function (index, fila) {
            var row_ = {
                idProducto: fila.children[0].innerHTML,
                cantidad: $(this).children("td").children("input").val()//fila.children[2].children[0].innerHTML              
            };
            productos.push(row_);
        });

        if (productos.length === 0) {
            MuestraToast('warning', "Debe agregar los productos al inventario.");
            return;
        }

        swal({
            title: '',
            text: 'Estas seguro que deseas afectar el inventario?',
            icon: '',
            buttons: ["Cancelar", "Aceptar"],
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    dataToPost = JSON.stringify({ listProductos: productos, tipoInventario: $("#idTipoInventario").val(), noPuerta:0 });

                    $.ajax({
                        url: rootUrl("/Inventario/AfectaInventario"),
                        data: dataToPost,
                        method: 'POST',
                        dataType: 'JSON',
                        contentType: "application/json; charset=utf-8",
                        async: true,
                        beforeSend: function (xhr) {
                            ShowLoader("Guardando Inventario.");
                        },
                        success: function (data) {
                            OcultarLoader();
                            if (data.Estatus == 200) {
                                MuestraToast("success", data.Mensaje);
                                //PintarTabla();
                                $("#frmInventarioDetalle").submit();
                                $('#modalInventario').modal('hide');
                            }
                            else
                                MuestraToast("error", data.Mensaje);

                        },
                        error: function (xhr, status) {
                            OcultarLoader();
                            MuestraToast("error", 'Hubo un problema al guardar el inventario, contactese con el administrador del sistema');

                        }
                    });
                } else {
                    console.log("cancelar");
                }
            });

    });


}



function eliminaFila(idProducto) {
    $('#tblInventario tbody tr#' + idProducto).remove();
}

function revisarExistenciasCombo() {
    $('select[id*="idProductoInventario"] option').each(function (index, value) {
        if ($(this).text().includes('(SIN EXISTENCIAS)')) {
            $("#idProductoInventario>option[value='" + $(this).val() + "']").prop('disabled', true);
        }
    });
}
