﻿using AdminRFID.DAO;
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
    public class InventarioController : Controller
    {
        // GET: Inventario
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_inventario_historico)]
        public ActionResult Inventario()
        {
            try
            {
                ViewBag.tipoInventario = new CatalogoDAO().ObtenerTiposInventario();//.Where(x=>(x.Value!="5" && x.Value != "6"));
                ViewBag.EstatusCalidad = new CatalogoDAO().ObtenerEstatusCalidad();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_registrar_entradas_y_salidas)]
        public ActionResult _Inventario(TipoInventario tipoInventario)
        {
            try
            {
                Notificacion<List<Producto>> notificacion = new ProductoDAO().ObtenerProductos(new Producto());
                List<Producto> productos = notificacion.Modelo != null ? notificacion.Modelo : new List<Producto>();
                ViewBag.listProductos = new SelectList(productos, "idProducto", "descipcionExistencias").ToList();
                ViewBag.tipoInventario = Convert.ToInt32(tipoInventario.idTipoInventario);
                return PartialView();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_registrar_entradas_y_salidas)]
        public ActionResult AfectaInventario(TipoInventario tipoInventario,List<Producto> listProductos,int noPuerta)
        {
            try
            {
                Sesion sesion= Session["UsuarioActual"] as Sesion;
                Notificacion<string> notificacion = new InventarioDAO().AfectaInventario(tipoInventario, listProductos, sesion.usuario.idUsuario, noPuerta);
                return Json(notificacion,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_inventario_historico)]
        public ActionResult _ObtenerInventario(InventarioDetalle inventario)
        {
            try
            {
                Notificacion<List<InventarioDetalle>> notificacion = new InventarioDAO().ObtenerInventario(inventario);
                return PartialView(notificacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_cancelar_entradas_y_salidas)]
        public ActionResult CancelaInventario(Int64 idInventarioDetalle)
        {
            try
            {
                Sesion sesion= Session["UsuarioActual"] as Sesion;
                Notificacion<string> notificacion = new InventarioDAO().CancelarInventario(idInventarioDetalle, sesion.usuario.idUsuario);
                return Json(notificacion, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }
}