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
        public Notificacion<string> AfectaInventario(TipoInventario tipoInventario, List<Producto> listProductos,int idUsuario,int noPuerta)
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
                    parameters.Add("@TipoInventario", tipoInventario.idTipoInventario);
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
                    parameters.Add("@idCatTipoInventario", i.tipoInventario.idTipoInventario==-1 ? (object)null : i.tipoInventario.idTipoInventario);
                    parameters.Add("@idEstatusInventario", i.estatusInventario == 0 ? (object)null : i.estatusInventario);
                    parameters.Add("@idUsuario", i.usuario.idUsuario == 0 ? (object)null : i.usuario.idUsuario);
                    parameters.Add("@fechaInicio", i.fechaInicio == DateTime.MinValue ? (object)null : i.fechaInicio);
                    parameters.Add("@fechaFin", i.fechaFin == DateTime.MinValue ? (object)null : i.fechaFin);
                    parameters.Add("@idEstatusCalidad", i.producto.estatusCalidad.idEstatusCalidad == 0 ? (object)null : i.producto.estatusCalidad.idEstatusCalidad);
                    var result = db.QueryMultiple("SP_OBTENER_INVENTARIO", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<InventarioDetalle,TipoInventario, Producto,EstatusCalidad,Usuario, InventarioDetalle>(MapInventario, splitOn: "idTipoInventario,idProducto,idUsuario,idEstatusCalidad").ToList();
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

        public Notificacion<string> CancelarInventario(Int64 idInventarioDetalle,Int64 idUsuario)
        {

            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idInventarioDetalle", idInventarioDetalle);
                    parameters.Add("@idUsuario", idUsuario);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_CANCELA_INVENTARIO", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Notificacion<List<Producto>> ObtenerInventarioGeneral(InventarioDetalle i)
        {

            Notificacion<List<Producto>> notificacion = new Notificacion<List<Producto>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@NombreProducto", string.IsNullOrEmpty(i.producto.nombre) ? (object)null : i.producto.nombre);
                    parameters.Add("@idEstatusCalidad", i.producto.estatusCalidad.idEstatusCalidad==0 ? (object)null : i.producto.estatusCalidad.idEstatusCalidad);
                    parameters.Add("@fechaIni", i.fechaInicio == DateTime.MinValue ? (object)null : i.fechaInicio);
                    parameters.Add("@fechaFin", i.fechaFin == DateTime.MinValue ? (object)null : i.fechaFin);
                    var result = db.QueryMultiple("SP_OBTENER_INVENTARIO_GENERAL ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<Producto,EstatusCalidad,Producto>(MapProducto,splitOn: "idEstatusCalidad").ToList();
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

        public Producto MapProducto(Producto p, EstatusCalidad e)
        {
            p.estatusCalidad = e;   
            return p;
        }



        public InventarioDetalle MapInventario(InventarioDetalle i, TipoInventario t,Producto p,EstatusCalidad e,Usuario u)
        {
            i.producto = p;
            i.producto.estatusCalidad = e;
            i.tipoInventario = t;
            i.usuario = u;

            return i;
        }
    }
}