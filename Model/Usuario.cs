namespace MiPrimeraApi2.Model;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Contraseña { get; set; }
    public string NombreUsuario { get; set; }
    public string Mail { get; set; }
}

public class Producto
{
    public int Id { get; set; }
    public string Descripciones { get; set; }
    public double Costo { get; set; }
    public double PrecioVenta { get; set; }
    public int Stock { get; set; }

    public int IdUsuario { get; set; }
}


public class Venta
{
    public int Id { get; set; }
    public string Comentarios { get; set; }
        
}


public class ProductoVendido
{
    public int Id { get; set; }
    public int Stock { get; set; }
    public int IdProducto { get; set; }
    public int IdVenta { get; set; }

}




