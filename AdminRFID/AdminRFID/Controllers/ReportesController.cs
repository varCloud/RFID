using AdminRFID.DAO;
using AdminRFID.Filters;
using AdminRFID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminRFID.Controllers
{
    [SessionTimeout]
    public class ReportesController : Controller
    {
        // GET: Reportes

        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_reporte_inventario)]
        public ActionResult Inventario()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_reporte_inventario)]
        public ActionResult _ObtenerInventario(InventarioDetalle i)
        {
            try
            {
                Notificacion<List<Producto>> notificacion = new InventarioDAO().ObtenerInventarioGeneral(i);
                //List<Producto> productos = notificacion.Modelo != null ? notificacion.Modelo : new List<Producto>();
                return PartialView(notificacion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}