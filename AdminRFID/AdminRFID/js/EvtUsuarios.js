var tblUsuarios;
$(document).ready(function () {
    InitTableUsuarios();
});

function InitTableUsuarios() {
    var NombreTabla = "tblUsuarios";
    tblUsuarios = initDataTable(NombreTabla);

    new $.fn.dataTable.Buttons(tblUsuarios, {
        buttons: [
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a PDF',
                title: "Usuarios",
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 8;
                    doc.styles.tableHeader.fontSize = 10;
                    doc.defaultStyle.alignment = 'center';
                    doc.content[1].table.widths = ['10%', '30%', '15%', '15%', '15%', '15%'];
                    doc.pageMargins = [30, 85, 20, 30];
                    doc.content.splice(0, 1);
                    doc['header'] = SetHeaderPDF("Usuarios");
                    doc['footer'] = (function (page, pages) { return setFooterPDF(page, pages) });
                },
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                },
            },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel" style="font-size:20px;"></i>',
                className: '',
                titleAttr: 'Exportar a Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                },
            },
        ],
    });

    tblUsuarios.buttons(0, null).container().prependTo(
        tblUsuarios.table().container()
    );

    $('#' + NombreTabla + '_filter').append('&nbsp;&nbsp;&nbsp;<a href="#" class="btn btn-icon btn-primary" name="" id="btnAgregarUsuario" data-toggle="tooltip" title="Agregar Usuario"><i class="fas fa-plus"></i></a>');
    InitBtnAgregar();
}

function InitBtnAgregar() {
    $('#btnAgregarUsuario').click(function (e) {
        ConsultaUsuario();

    });

}

function ConsultaUsuario(idUsuario) {
    $.ajax({
        url: "/Usuarios/_Usuario",
        data: { idUsuario: idUsuario },
        method: 'post',
        dataType: 'html',
        async: false,
        beforeSend: function (xhr) {
            ShowLoader("Cargando...");
        },
        success: function (data) {
            OcultarLoader();
            $('#viewUsuario').html(data);
            $.validator.unobtrusive.parse("#frmUsuario")
            if (idUsuario > 0)
                $('#TituloModalUsuario').html("Editar Usuario");
            else
                $('#TituloModalUsuario').html("Agregar Usuario");
            $('#modalUsuario').modal({ backdrop: 'static', keyboard: false, show: true });

            $("#MostrarPassword").click(function (evt) {
                evt.preventDefault();
                if ($('#contrasena').attr('type') =="password")
                    $('#contrasena').attr('type', 'text');
                else
                    $('#contrasena').attr('type', 'password');

            });

        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar el usuario")
        }
    });
}


function onBeginSubmitGuardarUsuario() {
    ShowLoader("Guardando...");
}
function onSuccessResultGuardarUsuario(data) {
    OcultarLoader();
    if (data.Estatus == 200) {
        MuestraToast("success", data.Mensaje)
        PintarTabla();
    } else {
        MuestraToast("error", data.Mensaje)
    }

    $('#modalUsuario').modal('hide');

}
function onFailureResultGuardarUsuario() {
    OcultarLoader();
    MuestraToast("error", "Ocurrio un error al guardar")
}

function PintarTabla() {
    $.ajax({
        url: rootUrl("/Usuarios/_ConsultaUsuarios"),
        method: 'post',
        dataType: 'html',
        async: true,
        beforeSend: function (xhr) {
            ShowLoader("Consultando usuarios...");
        },
        success: function (data) {
            tblUsuarios.destroy();
            $('#viewUsuarios').html(data);
            InitTableUsuarios();
            OcultarLoader();
        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al consultar los usuarios")
        }
    });
}

function ActualizaActivo(element, idUsuario) {

    var status = false;
    status=$(element).attr("activo");

    $.ajax({
        url: "/Usuarios/ActualizaActivoUsuario",
        data: { idUsuario: idUsuario, Activo:  !status },
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
                $(element).attr("activo", !status);              
            }
            else {
                MuestraToast("error", data.Mensaje)
                $(element).attr("activo", status);
            }
      

        },
        error: function (xhr, status) {
            OcultarLoader();
            MuestraToast("error", "Ocurrio un error al actualizar el estatus del usuario")
        }
    });   
}

function EliminarUsuario(idUsuario) {

    swal({
        title: '',
        text: 'Estas seguro que deseas eliminar a este Usuario?',
        icon: '',
        buttons: ["Cancelar", "Aceptar"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Usuarios/ActualizaActivoUsuario",
                    data: { idUsuario: idUsuario},
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
                            tblUsuarios.destroy();
                            PintarTabla();
                        }
                        else {
                            MuestraToast("error", data.Mensaje)                            
                        }
                    },
                    error: function (xhr, status) {
                        OcultarLoader();
                        MuestraToast("error", "Ocurrio un error al actualizar el estatus del usuario")
                    }
                });  
            } 
        });


}