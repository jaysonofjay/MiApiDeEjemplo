namespace MiPrimeraApi2.Controllers.DTOS
{
    public class PutProductoVendido
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int IdProducto { get; set; }
        public int IdVenta { get; set; }
        
    }
}
