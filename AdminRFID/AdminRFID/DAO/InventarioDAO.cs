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
        public Notificacion<string> AfectaInventario(EnumTipoInventario tipoInventario, List<Producto> listProductos,int idUsuario)
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
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_AFECTA_INVENTARIO_MASIVO", parameters, commandType: CommandType.StoredProcedure);
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