using AdminRFID.Filters;
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
        public ActionResult Usuarios()
        {
            return View();
        }
    }
}