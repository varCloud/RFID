using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Usuario
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        public string usuario { get; set; }

        //public int NumeroUsuario { get; set; }
        [Display(Name = "Constraseña")]
        [Required(ErrorMessage = "Este campo no puede estar vacio")]
        public string contrasena { get; set; }
        public int idUsuario { get; set; }        
        public string nombreCompleto { get; set; }        
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