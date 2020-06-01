using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public enum EnumTipoInventario
    {
        Entrada = 1,
        Salida=2        
    }

    public enum EnumEstatusInventario
    {
        Realizado = 1,
        Cancelado = 2
    }
}