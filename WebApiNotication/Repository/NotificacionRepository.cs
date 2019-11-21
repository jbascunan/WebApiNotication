using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApiProxy.Models;
using Dapper;
using System.Text;

namespace WebApiProxy.Repository
{
	public static class NotificacionRepository
	{
		private static string _connectionString = ConfigurationManager.ConnectionStrings["ConexionStr"].ConnectionString;		

		public static List<NotificacionEstadoExpedienteRequest> GetEstadoProyectos()
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var spName = "GetEstadoProyectos";				

				return db.Query<NotificacionEstadoExpedienteRequest>(spName, commandType: CommandType.StoredProcedure).ToList();
			}
		}

        /// <summary>
        /// Cambia a procesada la notificación en base de datos
        /// </summary>
        /// <param name="notificacion">Objeto notificacion</param>
        /// <param name="estadoProceso">Identificador de estado del proceso de notificación. 1=Procesado con Exito 2=Error</param>
        /// <returns></returns>
        public static void Update()
		{
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat("UPDATE [dbo].[LicitacionesSugeridas] ");

                query.AppendFormat("SET [Estado] = 1");

                db.Execute(query.ToString());
            }
        }

        public static int Insert(Listado lic)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var fechaProceso = DateTime.Now.ToString();
                StringBuilder query = new StringBuilder();
                query.AppendFormat("INSERT INTO [dbo].[LicitacionesSugeridas] ");
                query.AppendFormat("([CodigoExterno]");
                query.AppendFormat(",[Nombre]");
                query.AppendFormat(",[CodigoEstado]");
                query.AppendFormat(",[FechaCierre]");
                query.AppendFormat(",[Rubro]) ");
                query.AppendFormat("VALUES (@CodigoExterno, @Nombre, @CodigoEstado,@FechaCierre,@Rubro)");

                return db.Execute(query.ToString(), new
                {
                    lic.CodigoExterno,
                    lic.Nombre,
                    lic.CodigoEstado,
                    lic.FechaCierre,
                    lic.Rubro
                });
            }
        }

        public static List<Listado> GetLicitaciones()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [dbo].[LicitacionesSugeridas] order by Estado asc";
                return db.Query<Listado>(query).ToList();
            }
        }
    }
}