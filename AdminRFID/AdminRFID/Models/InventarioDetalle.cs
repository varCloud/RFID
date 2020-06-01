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
        public Usuario usuario { get; set; }
        public EnumTipoInventario tipoInventario { get; set; }
        public EnumEstatusInventario estatusInventario { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime fechaAlta { get; set; }

        public InventarioDetalle()
        {
            producto = new Producto();
            usuario = new Usuario();

        }
    }
}