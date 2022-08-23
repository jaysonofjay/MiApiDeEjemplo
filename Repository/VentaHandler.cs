using MiPrimeraApi2.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiPrimeraApi2
{
    public class VentaHandler : DbHandler
    {
        public static List<Venta> GetVentas()
        {
            List<Venta> Ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Venta", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();

                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();
                                Ventas.Add(venta);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return Ventas;
        }


        // Recibe una lista de productos y el número de IdUsuario de quien la efectuó, primero cargar una nueva Venta en la base de datos
        public static int CrearVenta() 
        {
            int resultado = 0;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryInsert = "INSERT INTO [SistemaGestion].[dbo].[Venta] (Comentarios) VALUES (@Comentarios) " +
                                     "SELECT max(Id) FROM [SistemaGestion].[dbo].[Venta] ";

                SqlParameter commentsParameter = new SqlParameter("Comentarios", SqlDbType.VarChar) { Value = ""};

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.Add(commentsParameter);

                    //int numberOfRows = sqlCommand.ExecuteNonQuery(); // Se ejecuta la sentencia sql
                    int nuevo_id = Convert.ToInt32(sqlCommand.ExecuteScalar());

                    if (nuevo_id > 0)
                    {
                        resultado = nuevo_id;
                    }
                }

                sqlConnection.Close();
            }

            return resultado;
        }

    }
}
