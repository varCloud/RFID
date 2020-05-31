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
    public class UsuariosDAO
    {
        private IDbConnection db = null;

        public Notificacion<List<Usuario>> ObtenerUsuarios(Usuario usr)
        {

            Notificacion<List<Usuario>> notificacion = new Notificacion<List<Usuario>>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUsuario", usr.idUsuario == 0 ? (object)null : usr.idUsuario);
                    var result = db.QueryMultiple("SP_OBTENER_USUARIOS ", parameters, commandType: CommandType.StoredProcedure);
                    var r1 = result.ReadFirst();
                    if (r1.Estatus == 200)
                    {
                        notificacion.Estatus = r1.Estatus;
                        notificacion.Mensaje = r1.Mensaje;
                        notificacion.Modelo = result.Read<Usuario, Rol, Usuario>(MapUsuario, splitOn: "idRol").ToList();
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

        public Notificacion<string> GuardaUsuario(Usuario usr)
        {
            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUsuario", usr.idUsuario == 0 ? (object)null : usr.idUsuario);
                    parameters.Add("@nombreCompleto", usr.nombreCompleto);
                    parameters.Add("@correo", usr.correo);
                    parameters.Add("@telefono", usr.telefono);
                    parameters.Add("@usuario", usr.usuario);
                    parameters.Add("@contrasena", usr.contrasena);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_AGREGA_ACTUALIZA_USUARIO ", parameters, commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Notificacion<string> ActualizaActivoUsuario(Usuario usr)
        {

            Notificacion<string> notificacion = new Notificacion<string>();
            try
            {
                using (db = new SqlConnection(ConfigurationManager.AppSettings["conexionString"].ToString()))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idUsuario", usr.idUsuario);
                    parameters.Add("@activo", usr.Activo);
                    notificacion = db.QuerySingle<Notificacion<string>>("SP_ACTUALIZA_ACTIVO_USUARIO ", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificacion;
        }

        public Usuario MapUsuario(Usuario u, Rol rol)
        {
            u.rol = rol;
            return u;
        }

    }
}