using AdminRFID.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace AdminRFID.DAO
{
    public class InventarioDAO
    {
        private IDbConnection db = null;
        public Notificacion<string> AfectaInventario(EnumTipoInventario tipoInventario, List<Producto> listProductos,int idUsuario,int noPuerta)
        {

            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Producto>));
                var stringBuilder = new StringBuilder();
                using (var xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    xmlSerializer.Serialize(xmlWriter, listProductos);
                }

                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@productos", stringBuilder.ToString());
                    parameters.Add("@idUsuario", idUsuario);
                    parameters.Add("@TipoInventario", tipoInventario);
                    parameters.Add("@noPuerta", noPuerta);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_AFECTA_INVENTARIO_MASIVO", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Notificacion<List<InventarioDetalle>> ObtenerInventario(InventarioDetalle i)
        {

            Notificacion<List<InventarioDetalle>> notificacion = new Notificacion<List<InventarioDetalle>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@tagProducto", string.IsNullOrEmpty(i.producto.tag)? (object)null : i.producto.tag);
                    parameters.Add("@idCatTipoInventario", i.tipoInventario == 0 ? (object)null : i.tipoInventario);
                    parameters.Add("@idEstatusInventario", i.estatusInventario == 0 ? (object)null : i.estatusInventario);
                    parameters.Add("@idUsuario", i.usuario.idUsuario == 0 ? (object)null : i.usuario.idUsuario);
                    parameters.Add("@fechaInicio", i.fechaInicio == DateTime.MinValue ? (object)null : i.fechaInicio);
                    parameters.Add("@fechaFin", i.fechaFin == DateTime.MinValue ? (object)null : i.fechaFin);
                    var result = db.QueryMultiple("SP_OBTENER_INVENTARIO", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<InventarioDetalle, Producto,Usuario, InventarioDetalle>(MapInventario, splitOn: "idProducto,idUsuario").ToList();
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

        public Notificacion<string> CancelarInventario(Int64 idInventarioDetalle)
        {

            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idInventarioDetalle", idInventarioDetalle);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_CANCELA_INVENTARIO", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }


        public InventarioDetalle MapInventario(InventarioDetalle i, Producto p,Usuario u)
        {
            i.producto = p;
            i.usuario = u;
            return i;
        }
    }
}