using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Sesion
    {
        public Boolean usuarioValido { get; set; }
        public Usuario usuario { get; set; }
        public string Token { get; set; }

        public Sesion()
        {
            usuario = new Usuario();
        }

        public static bool TienePermiso(EnumRolesPermisos valor)
        {
            HttpContext context = HttpContext.Current;
            Sesion sesion = (Sesion)context.Session["UsuarioActual"];           
                      
            return sesion.usuario.rol.permisos.Where(x =>(EnumRolesPermisos)x.idPermiso == valor).Any();
        }

    }

}