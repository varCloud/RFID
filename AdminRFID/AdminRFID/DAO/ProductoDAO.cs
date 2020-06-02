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
                    var result = db.QueryMultiple("SP_OBTENER_PRODUCTOS ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<Producto>().ToList();
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