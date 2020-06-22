using AdminRFID.Models;
using Dapper;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminRFID.DAO
{
    public class ProductoDAO
    {
        private IDbConnection db = null;

        public Notificacion<List<Producto>> ObtenerProductos(Producto p)
        {

            Notificacion<List<Producto>> notificacion = new Notificacion<List<Producto>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idProducto", p.idProducto == 0 ? (object)null : p.idProducto);
                    parameters.Add("@idEstatusCalidad", p.estatusCalidad.idEstatusCalidad == 0 ? (object)null : p.estatusCalidad.idEstatusCalidad);
                    parameters.Add("@idUnidadMedida", p.unidadMedida.idUnidadMedida == 0 ? (object)null : p.unidadMedida.idUnidadMedida);
                    var result = db.QueryMultiple("SP_OBTENER_PRODUCTOS ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<Producto,EstatusCalidad,UnidadMedida,Producto>(MapProducto, splitOn: "idEstatusCalidad,idUnidadMedida").ToList();
                    }
                    else
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Producto MapProducto(Producto p, EstatusCalidad e,UnidadMedida u)
        {
            p.estatusCalidad = e;
            p.unidadMedida = u;
            return p;
        }

        public Notificacion<string> GuardaProducto(Producto producto)
        {
            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idProducto", producto.idProducto == 0 ? (object)null : producto.idProducto);
                    parameters.Add("@tag", producto.tag);
                    parameters.Add("@descripcion", producto.descripcion);
                    parameters.Add("@codigo", producto.codigo);
                    parameters.Add("@nombre", producto.nombre);
                    parameters.Add("@lote", producto.lote);
                    parameters.Add("@idUnidadMedida", producto.unidadMedida.idUnidadMedida == 0 ? (object)null : producto.unidadMedida.idUnidadMedida);
                    parameters.Add("@idEstatusCalidad", producto.estatusCalidad.idEstatusCalidad == 0 ? (object)null : producto.estatusCalidad.idEstatusCalidad);
                    parameters.Add("@LPN", producto.LPN);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_AGREGA_ACTUALIZA_PRODUCTO ", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Notificacion<string> ActualizaActivoProducto(Producto producto)
        {

            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idProducto", producto.idProducto);
                    parameters.Add("@activo", producto.activo);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_ACTUALIZA_ACITVO_PRODUCTO", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

    }
}