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
    public class ProductosController : Controller
    {
        // GET: Producto
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_producto)]
        public ActionResult Productos()
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

        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_crear_un_nuevo_producto)]
        public ActionResult _Producto(Producto producto)
        {
            try
            {
                Producto p = new Producto();
                CatalogoDAO daoCatalogo = new CatalogoDAO();


                if (producto.idProducto > 0)
                {
                    p = new ProductoDAO().ObtenerProductos(producto).Modelo.First();
                }

                ViewBag.EstatusCalidad = daoCatalogo.ObtenerEstatusCalidad();
                ViewBag.UnidadMedida = daoCatalogo.ObtenerUnidadesMedida();

                return PartialView(p);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_producto)]
        public ActionResult _ConsultaProductos()
        {
            try
            {
                return PartialView(new ProductoDAO().ObtenerProductos(new Producto()));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_crear_un_nuevo_producto)]
        public ActionResult GuardaProducto(Producto producto)
        {
            try
            {
                Notificacion<string> result = new ProductoDAO().GuardaProducto(producto);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_eliminar_un_producto)]
        public ActionResult ActualizaActivoProducto(Producto producto)
        {
            try
            {
                Notificacion<string> result = new ProductoDAO().ActualizaActivoProducto(producto);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult ObtenerCodigos(string cadena)
        {
            try
            {
                Dictionary<string, object> codigos = new Dictionary<string, object>();
                codigos.Add("barra", Convert.ToBase64String(Utilerias.Utils.GenerarCodigoBarras(cadena)));
                codigos.Add("qr", Convert.ToBase64String(Utilerias.Utils.GenerarQR(cadena)));
                return Json(codigos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [PermisoAttribute(Permiso=EnumRolesPermisos.Puede_imprimir_codigos_productos)]
        public ActionResult PdfCodigos(string codigo, string Producto)
        {
            Notificacion<String> notificacion = new Notificacion<string>();
            try
            {                
                string pdfCodigos=Convert.ToBase64String(Utilerias.Utils.GenerarImprimibleCodigos(codigo, Producto));
                ViewBag.pdfBase64 = pdfCodigos;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}