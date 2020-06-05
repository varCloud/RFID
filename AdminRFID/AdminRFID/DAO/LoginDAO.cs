using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using AdminRFID.Models;

namespace AdminRFID.DAO
{
    public class LoginDAO
    {

        private IDbConnection db = null;


        public Notificacion<Sesion> ValidaUsuario(Sesion sesion)
        {
            Notificacion<Sesion> n = null;
            try
            {
              
               n = new Notificacion<Sesion>();
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@usuario", sesion.usuario.usuario);
                    parameters.Add("@contrasena",sesion.usuario.contrasena);
                    var result = db.QueryMultiple("SP_VALIDAR_USUARIO ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        n.Estatus = 200;
                        n.Mensaje = "OK";                        
                        n.Modelo = result.Read<Sesion,Usuario,Rol,Sesion>(MapSesion, splitOn:"idUsuario,idRol").First();  
                        n.Modelo.usuario.rol.permisos = ObtenerPermisosRol(n.Modelo.usuario.rol.idRol);
                    }
                    else {
                        n.Estatus = -1;
                        n.Mensaje = "Datos incorrectos";
                        n.Modelo = sesion;
                    }

                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return n;
        }

        public Sesion MapSesion(Sesion s,Usuario u, Rol rol)
        {
            s.usuario = u;         
            s.usuario.rol = rol;       
            return s;
        }

        public List<Permiso> ObtenerPermisosRol(int idRol)
        {

            List<Permiso> permisos = new List<Permiso>();           

            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idRol", idRol);
                    var result = db.QueryMultiple("SP_OBTENER_PERMISOS_ROL ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        permisos = result.Read<Permiso>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return permisos;
        }
    }
}