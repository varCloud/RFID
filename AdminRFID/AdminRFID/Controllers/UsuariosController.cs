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
    public class UsuariosController : Controller
    {
        // GET: Usuario
        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_usuario)]
        public ActionResult Usuarios()
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
        public ActionResult _Usuario(Usuario usuario)
        {
            try
            {
                Usuario us = new Usuario();
                
                if (usuario.idUsuario > 0)
                {
                    us = new UsuariosDAO().ObtenerUsuarios(usuario).Modelo.First();      
                }
                
                return PartialView(us);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult _ConsultaUsuarios()
        {
            try
            {
                return PartialView(new UsuariosDAO().ObtenerUsuarios(new Usuario()));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult GuardarUsuario(Usuario usuario)
        {
            try
            {
                Notificacion<string> result = new UsuariosDAO().GuardaUsuario(usuario);
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult ActualizaActivoUsuario(Usuario usuario)
        {
            try
            {
                Notificacion<string> result = new UsuariosDAO().ActualizaActivoUsuario(usuario);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



    }
}