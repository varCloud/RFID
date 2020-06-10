using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public enum EnumTipoInventario
    {
        Entrada = 1,
        Salida=2        
    }

    public enum EnumEstatusInventario
    {
        Realizado = 1,
        Cancelado = 2
    }

    public enum EnumTipoGrafico
    {
        EntradasPorFecha = 1,
        SalidasPorFecha = 2,
        TopTenProductosEntrantes = 3,
        TopTenProductosSalientes = 4
    }

    public enum EnumTipoReporteGrafico
    {
        Semanal = 1,
        Mensuales = 2,
        Anuales = 3,
        Dia = 4
    }

    public enum EnumRolesPermisos
    {
        #region Usuarios
        Puede_visualizar_usuario = 1,
        Puede_crear_un_nuevo_usuario=2,
        Puede_editar_un_usuario=3,
        Puede_eliminar_un_usuario=4,
        #endregion

        #region Productos
        Puede_visualizar_producto = 5,
        Puede_crear_un_nuevo_producto=6,
        Puede_editar_un_producto=7,
        Puede_eliminar_un_producto=8,
        #endregion

        #region Inventario
        Puede_visualizar_inventario_historico = 9,
        Puede_registrar_entradas_y_salidas=10,
        Puede_cancelar_entradas_y_salidas=11,
        #endregion

        #region Reportes
        Puede_visualizar_reporte_inventario = 12,
        #endregion

        #region Dashboard
        Puede_visualizar_dashboard=13
        #endregion
    }
}