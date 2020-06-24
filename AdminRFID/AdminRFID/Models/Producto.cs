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
        public Int64 cantidadTotalEntradas { get; set; }
        public Int64 cantidadTotalSalidas { get; set; }
        public string descipcionExistencias { get; set; }
        public string codigo { get; set; }
        public string ultimoEstatusInventario { get; set; }

        public string nombre { get; set; }
        public string lote { get; set; }
        public UnidadMedida unidadMedida { get; set; }
        public EstatusCalidad estatusCalidad { get; set; }
        public string LPN { get; set; }
        public string producto { get; set; }


        public Producto()
        {
            unidadMedida = new UnidadMedida();
            estatusCalidad = new EstatusCalidad();
        }

    }
}