
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraApi2.Model;


namespace MiPrimeraApi2
{
    public class ProductosVendidosHandler : DbHandler
    {
        public static List<ProductoVendido> GetProductosVendidos()
        {
            List<ProductoVendido> ProductosVendidos = new List<ProductoVendido>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM ProductoVendido", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();

                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);
                                ProductosVendidos.Add(productoVendido);

                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return ProductosVendidos;
        }

        // modificar el stock del producto vendido ingresando en formato JSON, id del producto y la nueva cantidad. 
        public static bool ModificarCantidadProducto(ProductoVendido productovendido)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[ProductoVendido] " +
                    "SET Stock  = @Stock " +
                    "WHERE Id = @id ";

                SqlParameter StockParameter = new SqlParameter("Stock", SqlDbType.BigInt) { Value = productovendido.Stock };
                SqlParameter idParameter = new SqlParameter("id", SqlDbType.BigInt) { Value = productovendido.Id };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(StockParameter);
                    sqlCommand.Parameters.Add(idParameter);

                    int numberOfRows = sqlCommand.ExecuteNonQuery(); // Se ejecuta la sentencia sql

                    if (numberOfRows > 0)
                    {
                        resultado = true;
                    }
                }

                sqlConnection.Close();
            }

            return resultado;
        }

        // Create un producto vendido nuevo 
        public static bool AgregarProductoVendido(ProductoVendido productovendido)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryAdd = "INSERT INTO [SistemaGestion].[dbo].[ProductoVendido]  " +
                    "(Stock, idProducto, idVenta) " +
                    "VALUES (@Stock, @idProducto, @idVenta);";

                SqlParameter stockParameter = new SqlParameter("Stock", SqlDbType.Int) { Value = productovendido.Stock };
                SqlParameter idProductoParameter = new SqlParameter("idProducto", SqlDbType.Int) { Value = productovendido.IdProducto };
                SqlParameter idVentaParameter = new SqlParameter("idVenta", SqlDbType.Int) { Value = productovendido.IdVenta };
                
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryAdd, sqlConnection))
                {
                    sqlCommand.Parameters.Add(stockParameter);
                    sqlCommand.Parameters.Add(idProductoParameter);
                    sqlCommand.Parameters.Add(idVentaParameter);
                    sqlCommand.ExecuteNonQuery(); // aca se ejecuta el insert
                    resultado = true;
                }

                sqlConnection.Close();
            }
            return resultado;
        }

        public static bool EliminarProductovendido(int idProducto)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[ProductoVendido] WHERE IdProducto = @idProducto";

                SqlParameter sqlParameter = new SqlParameter("idProducto", SqlDbType.BigInt);
                sqlParameter.Value = idProducto;

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    try
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                        sqlCommand.ExecuteScalar(); // aca se ejecuta el delete
                        resultado = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                sqlConnection.Close();
            }
            return resultado;
        }



    }
}
