using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Usuario
    {
        
        public string usuario { get; set; }
       
        public string contrasena { get; set; }
        public int idUsuario { get; set; }
        
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public DateTime fechaAlta { get; set; }
        public bool Activo { get; set; }

        public Rol rol { get; set; }

        public Usuario()
        {
            rol = new Rol();
        }

    }
}