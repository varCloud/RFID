using AdminRFID.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdminRFID.DAO
{
    public class DashboardDAO
    {
        private IDbConnection db = null;
        public Notificacion<List<Categoria>> ObtenerEntradasSalidasPorFecha(EnumTipoReporteGrafico idTipoReporte, EnumTipoGrafico enumTipoGrafico)
        {
            Notificacion<List<Categoria>> categoria = new Notificacion<List<Categoria>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idTipoReporte", idTipoReporte);
                    parameters.Add("@idTipoInventario", enumTipoGrafico == EnumTipoGrafico.EntradasPorFecha ? 1 : 2);
                    var rs = db.QueryMultiple("SP_DASHBOARD_CONSULTA_TOTAL_ENTRADAS_SALIDAS_INVENTARIO", parameters, commandType: CommandType.StoredProcedure);
                    var rs1 = rs.ReadFirst();
                    if (rs1.status == 200)
                    {
                        categoria.Estatus = rs1.status;
                        categoria.Mensaje = rs1.mensaje;
                        categoria.Modelo = rs.Read<Categoria>().ToList();
                    }
                    else
                    {
                        categoria.Estatus = rs1.status;
                        categoria.Mensaje = rs1.mensaje;
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return categoria;
        }

        public Notificacion<List<Categoria>> ObtenerEntradasSalidasPorProducto(DateTime? fechaIni, DateTime? fechaFin, EnumTipoReporteGrafico idTipoReporte, EnumTipoGrafico enumTipoGrafico)
        {
            Notificacion<List<Categoria>> notificacion = new Notificacion<List<Categoria>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@fechaIni", fechaIni == DateTime.MinValue ? (object)null : fechaIni);
                    parameters.Add("@fechaFin", fechaFin == DateTime.MinValue ? (object)null : fechaFin);
                    parameters.Add("@idTipoInventario", enumTipoGrafico == EnumTipoGrafico.EntradasPorFecha ? 1 : 2);
                    var rs = db.QueryMultiple("SP_DASHBOARD_CONSULTA_TOTAL_ENTRADAS_SALIDAS_POR_PRODUCTO", parameters, commandType: CommandType.StoredProcedure);
                    var rs1 = rs.ReadFirst();
                    if (rs1.status == 200)
                    {
                        notificacion.Estatus = rs1.status;
                        notificacion.Mensaje = rs1.mensaje;
                        notificacion.Modelo = rs.Read<Categoria>().ToList();
                    }
                    else
                    {
                        notificacion.Estatus = rs1.status;
                        notificacion.Mensaje = rs1.mensaje;
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return notificacion;
        }

        public Notificacion<List<Categoria>> ObtenerTopTen(EnumTipoReporteGrafico idTipoReporte, EnumTipoGrafico idTipoGrafico)
        {
            Notificacion<List<Categoria>> categoria = new Notificacion<List<Categoria>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idTipoReporte", idTipoReporte);
                    parameters.Add("@idTipoGrafico", idTipoGrafico);
                    var rs = db.QueryMultiple("SP_DASHBOARD_CONSULTA_TOP_TEN", parameters, commandType: CommandType.StoredProcedure);
                    var rs1 = rs.ReadFirst();
                    if (rs1.status == 200)
                    {
                        categoria.Estatus = rs1.status;
                        categoria.Mensaje = rs1.mensaje;
                        categoria.Modelo = rs.Read<Categoria>().ToList();
                    }
                    else
                    {
                        categoria.Estatus = rs1.status;
                        categoria.Mensaje = rs1.mensaje;
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return categoria;
        }

    }
}