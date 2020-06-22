using AdminRFID.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminRFID.DAO
{
    public class CatalogoDAO
    {
        private IDbConnection db = null;
        public List<SelectListItem> ObtenerEstatusCalidad()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    var result = db.QueryMultiple("SP_OBTENER_ESTATUS_CALIDAD ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        listItems = result.Read<SelectListItem>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listItems;
        }

        public List<SelectListItem> ObtenerTiposInventario()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    var result = db.QueryMultiple("SP_OBTENER_TIPOS_DE_INVENTARIO ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        listItems = result.Read<SelectListItem>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listItems;
        }

        public List<SelectListItem> ObtenerUnidadesMedida()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    var result = db.QueryMultiple("SP_OBTENER_UNIDADES_MEDIDA ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        listItems = result.Read<SelectListItem>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listItems;
        }

    }
}