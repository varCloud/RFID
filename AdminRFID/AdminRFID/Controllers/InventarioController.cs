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


        [HttpPost]
        public ActionResult _Inventario(EnumTipoInventario tipoInventario)
        {
            try
            {
                Notificacion<List<Producto>> notificacion = new ProductoDAO().ObtenerProductos(new Producto());
                List<Producto> productos = notificacion.Modelo != null ? notificacion.Modelo : new List<Producto>();
                ViewBag.listProductos = new SelectList(productos, "idProducto", "descipcionExistencias").ToList();
                ViewBag.tipoInventario = Convert.ToInt32(tipoInventario);
                return PartialView();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AfectaInventario(EnumTipoInventario tipoInventario,List<Producto> listProductos)
        {
            try
            {
                Sesion sesion= Session["UsuarioActual"] as Sesion;
                Notificacion<string> notificacion = new InventarioDAO().AfectaInventario(tipoInventario, listProductos, sesion.usuario.idUsuario);
                return Json(notificacion,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




    }
}