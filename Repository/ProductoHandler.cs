using MiPrimeraApi2.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimeraApi2.Repository
{
    public static class ProductoHandler
    {
        public const string ConnectionString = "Server=127.0.0.1,1433;Initial Catalog=SistemaGestion;Persist Security Info=False;User ID=sa;Password=123456a@;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";

        public static List<Producto> GetProductos()
        {
            List<Producto> resultados = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = "SELECT * FROM Producto";

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

                    sqlDataAdapter.SelectCommand = sqlCommand;

                    DataTable table = new DataTable();
                    sqlDataAdapter.Fill(table);
                    sqlCommand.Connection.Close();

                    foreach (DataRow row in table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt32(row["Id"]);
                        producto.Stock = Convert.ToInt32(row["Stock"]);
                        producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);
                        producto.Costo = Convert.ToInt32(row["Costo"]);
                        producto.PrecioVenta = Convert.ToInt32(row["PrecioVenta"]);
                        producto.Descripciones = row["Descripciones"].ToString();

                        resultados.Add(producto);
                    }
                }
            }

            return resultados;
        }
    }
}
