﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Configuration;
using System.Net.NetworkInformation;
using AdminRFID.DAO;
using AdminRFID.Models;
using System.Web.Security;

namespace AdminRFID.Controllers
{
    public class LoginController : Controller
    {
        public LoginDAO loginDAO;



        // GET: Login
        public ActionResult Login()
        {
            return View(new Sesion());
        }

        [HttpPost]
        public ActionResult ValidarUsuario(Sesion sesion)
        {
            try
            {
                if (!ReCaptchaPassed(sesion.Token))
                {
                    ModelState.AddModelError(string.Empty, "Por favor comunicate con el Administrador");
                    return Json(new Notificacion<object>() { Estatus = -1, Mensaje = "Error al validar el captcha" }, JsonRequestBehavior.AllowGet);
                }

                Notificacion<Sesion> n = new LoginDAO().ValidaUsuario(sesion);
                if (n.Modelo.usuarioValido)
                {
                    Session["UsuarioActual"] = n.Modelo;

                    if (Sesion.TienePermiso(EnumRolesPermisos.Puede_visualizar_usuario))
                    {
                        n.Controller = "Usuarios";
                        n.Action = "Usuarios";
                    }   
                    else if (Sesion.TienePermiso(EnumRolesPermisos.Puede_visualizar_producto))
                    {
                        n.Controller = "Productos";
                        n.Action = "Productos";
                    }
                    else if (Sesion.TienePermiso(EnumRolesPermisos.Puede_visualizar_inventario_historico))
                    {
                        n.Controller = "Inventario";
                        n.Action = "Inventario";
                    }            
                    else if (Sesion.TienePermiso(EnumRolesPermisos.Puede_visualizar_reporte_inventario))
                    {
                        n.Controller = "Reportes";
                        n.Action = "Inventario";
                    }                
                    else
                    {
                        n.Controller = "Login";
                        n.Action = "SinPermisos";
                    }
             

                }

                return Json(n, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, " Login", "ValidarUsuario"));
            }

        }


        public bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LfandgUAAAAACyyGmdqpahF8xXL1haLavqH6X2i&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
                return false;

            return true;
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult SinPermisos()
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
    }
}