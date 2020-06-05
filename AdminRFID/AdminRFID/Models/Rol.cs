using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Rol
    {
        public int idRol { get; set; }
        public string descripcion { get; set; }

        public List<Permiso> permisos { get; set; }

        public Rol()
        {
            permisos = new List<Permiso>();
        }

    }
}