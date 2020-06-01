using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class InventarioDetalle
    {
        public Int64 idInventarioDetalle { get; set; }
        public Producto producto { get; set; }
        public Int64 cantidadDespuesOperacion { get; set; }
        public int idUsuario { get; set; }
        public EnumTipoInventario tipoInventario { get; set; }
    }
}