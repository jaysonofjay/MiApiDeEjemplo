using System.Data;
using System.Data.SqlClient;
using MiPrimeraApi2.Model;

namespace MiPrimeraApi2
{
    public class ProductoHandler : DbHandler
    {

        // Metodos ABM para  clase producto  __________________________________________________// 
        public Producto GetById(int id)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = "select * from Producto where Id = @id;";

                    sqlCommand.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); //Se ejecuta el Select
                    
                    sqlCommand.Connection.Close();
                    foreach (DataRow row in table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt32(row["Id"]);
                        producto.Descripciones = row["Descripciones"]?.ToString();
                        producto.Costo = Convert.ToDouble(row["Costo"]);
                        producto.PrecioVenta = Convert.ToDouble(row["PrecioVenta"]);
                        producto.Stock = Convert.ToInt32(row["Stock"]);
                        producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        productos.Add(producto);
                    }
                }
            }

            return productos?.FirstOrDefault();
        }

        // metodo para capturar en una lista toda la tabla producto 
        public static List<Producto> GetProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Producto", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();
                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                                producto.Costo = Convert.ToInt32(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToInt32(dataReader["PrecioVenta"]);
                                producto.Descripciones = dataReader["Descripciones"].ToString();

                                productos.Add(producto);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return productos;
        }


        /*             | DataAdapter |                                                       *
         *  Recupera los datos para llenar un objeto dataset. A diferencia del DataReader trabaja de forma desconectada. *
         *   Por lo cual recupera los datos sin iterar permitiendo liberar la conexión a la base de datos en el instante */
        public List<string> GetTodasLasDescripcionesConDataAdapter()
        {
            List<string> descripciones = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Producto", sqlConnection);

                sqlConnection.Open();

                DataSet resultado = new DataSet();
                sqlDataAdapter.Fill(resultado);

                sqlConnection.Close();
            }

            return descripciones;
        }


        /*  |       DataReader          |                                                           *
         *  Itera de forma conectada con la base de datos. Mientras este objeto está trabajando     *
         *  la base de datos va a permanecer abierta.                                               */
        public List<string> GetTodasLasDescripcionesConDataReader()
        {
            List<string> descripciones = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "SELECT * FROM Producto", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas que leer
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string descripcionProducto = dataReader.GetString(1);
                                descripciones.Add(descripcionProducto);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return descripciones;
        }


        // Borra un elemento seleccionado en la tabla producto
        public static bool BorrarUnProducto(int idProducto)
        {

            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryDelete = "DELETE FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @idProducto";

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


        // Crear un elemento en la tabla producto utilizando la clase producto
        public static bool AgregarProducto(Producto producto)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryAdd = "INSERT INTO [SistemaGestion].[dbo].[Producto] " +
                    "(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES (@Descripciones, @Costo, @PrecioVenta, @Stock, @IdUsuario);";

                SqlParameter descripcionesParameter = new SqlParameter("Descripciones", SqlDbType.VarChar) { Value = producto.Descripciones };
                SqlParameter costoParameter = new SqlParameter("Costo", SqlDbType.Int) { Value = producto.Costo };
                SqlParameter precioVentaParameter = new SqlParameter("PrecioVenta", SqlDbType.Int) { Value = producto.PrecioVenta };
                SqlParameter stockParameter = new SqlParameter("Stock", SqlDbType.Int) { Value = producto.Stock };
                SqlParameter idUsuarioParameter = new SqlParameter("IdUsuario", SqlDbType.Int) { Value = producto.IdUsuario };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryAdd, sqlConnection))
                {
                    sqlCommand.Parameters.Add(descripcionesParameter);
                    sqlCommand.Parameters.Add(costoParameter);
                    sqlCommand.Parameters.Add(precioVentaParameter);
                    sqlCommand.Parameters.Add(stockParameter);
                    sqlCommand.Parameters.Add(idUsuarioParameter);
                    sqlCommand.ExecuteNonQuery(); // aca se ejecuta el insert
                    resultado = true;
                }

                sqlConnection.Close();
            }
            return resultado;
        }

        // modificar producto ingresando en formato JSON, id del producto y la descripcion nueva. 
        public static bool ModificarProducto(Producto producto)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Producto] " +
                                        "SET [Descripciones]  = @Descripciones, " +
                                           " [Costo]  = @Costo, " +
                                           " [PrecioVenta]  = @PrecioVenta, " +
                                           " [Stock]  = @Stock " + 
                                            " WHERE Id = @id "; 

                SqlParameter nombreParameter = new SqlParameter("Descripciones", SqlDbType.VarChar) { Value = producto.Descripciones};
                SqlParameter CostoParameter = new SqlParameter("Costo", SqlDbType.Money) { Value = producto.Costo };
                SqlParameter PrecioVentaParameter = new SqlParameter("PrecioVenta", SqlDbType.Money) { Value = producto.PrecioVenta };
                SqlParameter StocknombreParameter = new SqlParameter("Stock", SqlDbType.BigInt) { Value = producto.Stock };
                SqlParameter idParameter = new SqlParameter("id", SqlDbType.BigInt) { Value = producto.Id};

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(nombreParameter);
                    sqlCommand.Parameters.Add(CostoParameter);
                    sqlCommand.Parameters.Add(PrecioVentaParameter);
                    sqlCommand.Parameters.Add(StocknombreParameter);
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


        public static bool ModificarStockdeProducto(Producto producto, int ventaStock)
        {
            bool resultado = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer))
            {
                // subquery obteniendo el stock del producto en venta y restandose del producto vendido
                string queryUpdate = " UPDATE [SistemaGestion].[dbo].[Producto] SET [Stock] = " +
                    "                   ( (Select Stock from [SistemaGestion].[dbo].[Producto] WHERE Id = @id) - @ventaStock )" +
                    "                        WHERE Id = @id  ";

                SqlParameter ventastocknombreParameter = new SqlParameter("ventaStock", SqlDbType.BigInt) { Value = ventaStock };
                SqlParameter StocknombreParameter = new SqlParameter("Stock", SqlDbType.BigInt) { Value = producto.Stock };
                SqlParameter idParameter = new SqlParameter("id", SqlDbType.BigInt) { Value = producto.Id };

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(StocknombreParameter);
                    sqlCommand.Parameters.Add(ventastocknombreParameter);
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


    } // end class
}
