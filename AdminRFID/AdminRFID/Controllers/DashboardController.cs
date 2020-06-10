using AdminRFID.DAO;
using AdminRFID.Filters;
using AdminRFID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminRFID.Controllers
{
    [SessionTimeout]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [PermisoAttribute(Permiso=EnumRolesPermisos.Puede_visualizar_dashboard)]
        public ActionResult Index()
        {
            return View();
        }

        [PermisoAttribute(Permiso = EnumRolesPermisos.Puede_visualizar_dashboard)]
        public ActionResult _Grafico(EnumTipoGrafico tipoGrafico, EnumTipoReporteGrafico tipoReporteGrafico)
        {
            DashboardDAO dao = new DashboardDAO();
            Notificacion<Grafico> grafico = new Notificacion<Grafico>();

            if ((tipoGrafico == EnumTipoGrafico.EntradasPorFecha) || (tipoGrafico == EnumTipoGrafico.SalidasPorFecha))
            {
                //Notificacion<List<Estacion>> estaciones = dao.ObtenerVentasEstacion();
                Notificacion<List<Categoria>> categorias = dao.ObtenerEntradasSalidasPorFecha(tipoReporteGrafico, tipoGrafico);
                grafico.Estatus = categorias.Estatus;
                grafico.Mensaje = categorias.Mensaje;

                List<Data> dataProductos = new List<Data>();
                List<seriesDrilldown> SeriesDrilldown = new List<seriesDrilldown>();


                if (categorias.Estatus == 200)
                {
                    foreach (Categoria categoria in categorias.Modelo)
                    {
                        Data data = new Data();
                        data.name = categoria.categoria;
                        data.y = categoria.total;

                        Notificacion<List<Categoria>> productos = dao.ObtenerEntradasSalidasPorProducto(categoria.fechaIni, categoria.fechaFin, tipoReporteGrafico, tipoGrafico);
                        if (productos.Estatus == 200)
                        {                           
                            List<List<Object>> DataDrilldown = new List<List<Object>>();
                            foreach (Categoria e in productos.Modelo)
                            {
                                DataDrilldown.Add(new List<Object>() { e.categoria.ToString(), e.total });
                            }


                            SeriesDrilldown.Add(new seriesDrilldown()
                            {
                                id = categoria.id + "_" + categoria.categoria,
                                name = categoria.categoria,
                                data = DataDrilldown
                            });
                        }
                        data.drilldown = categoria.id + "_" + categoria.categoria;


                        dataProductos.Add(data);
                    }

                    grafico.Modelo = new Grafico();
                    if (dataProductos.Count > 0)
                    {
                        grafico.Estatus = 200;
                        grafico.Modelo.data = dataProductos;
                        grafico.Modelo.seriesDrilldowns = SeriesDrilldown;
                    }
                    else
                    {
                        grafico.Estatus = -1;
                        grafico.Mensaje = "No existe información para mostrar";
                    }

                }


            }

            if (tipoGrafico == EnumTipoGrafico.TopTenProductosEntrantes || tipoGrafico == EnumTipoGrafico.TopTenProductosSalientes)
            {
                Notificacion<List<Categoria>> categorias = dao.ObtenerTopTen(tipoReporteGrafico, tipoGrafico);
                grafico.Estatus = categorias.Estatus;
                grafico.Mensaje = categorias.Mensaje;
                if (categorias.Estatus == 200)
                {
                    grafico.Modelo = new Grafico();
                    grafico.Modelo.data = new List<Data>();
                    //grafico.Modelo.categorias = categorias.Modelo;
                    foreach (Categoria categoria in categorias.Modelo)
                    {
                        Data data = new Data();
                        data.name = categoria.categoria;
                        data.y = categoria.total;
                        grafico.Modelo.data.Add(data);                       
                    }                  
                }
            }

            ViewBag.tipoGrafico = tipoGrafico;
            ViewBag.tipoReporteGrafico = tipoReporteGrafico;

            return PartialView(grafico);
        }

    }
}