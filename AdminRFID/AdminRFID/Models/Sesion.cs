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

    }

}