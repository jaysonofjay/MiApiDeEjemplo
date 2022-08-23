using System.Data.SqlClient;
namespace MiPrimeraApi2
{

    public class DbHandler
    {

        public const string ConnectionToMyServer = "Server=DESKTOP-4HSFFS3; Database=SistemaGestion; Trusted_Connection=True";

        SqlConnection sqlConnection = new SqlConnection(ConnectionToMyServer);

        //public void AbrirConexionYCerrarConexion()
        //{
        //    sqlConnection.Open();

        //    sqlConnection.Close();
        //}





    }
}
