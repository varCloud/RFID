using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Producto
    {
        public Int64 idProducto { get; set; }
        public string tag { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaAlta { get; set; }
        public Boolean activo { get; set; }
        public Int64 cantidad { get; set; }
        public string descipcionExistencias { get; set; }


    }
}