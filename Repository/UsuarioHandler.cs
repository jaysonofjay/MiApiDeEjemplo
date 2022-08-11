using MiPrimeraApi2.Model;
using System.Data.SqlClient;

namespace MiPrimeraApi2.Repository
{
    public static class UsuarioHandler
    {
        public const string ConnectionString = "Server=127.0.0.1,1433;Initial Catalog=SistemaGestion;Persist Security Info=False;User ID=sa;Password=123456a@;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";

        public static List<Usuario> GetUsuarios()
        {
            List<Usuario> resultados = new List<Usuario>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Usuario", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        // Me aseguro que haya filas
                        if(dataReader.HasRows)
                        {
                            while(dataReader.Read())
                            {
                                Usuario usuario = new Usuario();

                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();

                                resultados.Add(usuario);
                            }
                        }
                    }

                    sqlConnection.Close();
                }
            }

            return resultados;
        }
    }
}
