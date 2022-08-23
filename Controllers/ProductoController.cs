using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi2.Model;
using MiPrimeraApi2.Repository;
using MiPrimeraApi2.Controllers.DTOS;


namespace MiPrimeraApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductoController : ControllerBase
    {
        [HttpGet(Name = "GetProductos")]
        public List<Producto> GetProductos()
        {
            return ProductoHandler.GetProductos();
        }

        [HttpPut]
        public bool ModificarProducto([FromBody] PutProducto producto)
        {
            return ProductoHandler.ModificarProducto(new Producto
            {
                Id = producto.Id,
                Descripciones = producto.Descripciones,
                Costo = producto.Costo,
                PrecioVenta = producto.PrecioVenta, 
                Stock = producto.Stock, 
                IdUsuario = producto.IdUsuario
            });
        }

        [HttpPost]
        public bool CrearProducto([FromBody] PostProducto producto)
        {
            try
            {
                return ProductoHandler.AgregarProducto(new Producto
                {
                    //Descripciones, Costo, PrecioVenta, Stock, IdUsuario
                    Descripciones = producto.Descripciones,
                    Costo = producto.Costo,
                    PrecioVenta = producto.PrecioVenta,
                    Stock = producto.Stock,
                    IdUsuario = producto.IdUsuario
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        [HttpDelete]
        public bool BorrarProducto([FromBody] int id)
        {

            /*  Eliminar producto: Recibe un id de producto a eliminar y debe eliminarlo de la base
             de datos (eliminar antes sus productos vendidos también, sino no lo podrá hacer).      */

            bool borrar = ProductosVendidosHandler.EliminarProductovendido(id);
            
            if (borrar)
            {
                return ProductoHandler.BorrarUnProducto(id);
            }
            else
            {
                Console.WriteLine("Este id de Producto no se encuentra en la tabla de productos vendidos");
                return false;
            }

        }


    }
}
