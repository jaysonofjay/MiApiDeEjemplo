using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi2.Controllers.DTOS;
using MiPrimeraApi2.Model;


namespace MiPrimeraApi2.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class ProductoVendidoController : ControllerBase

    {

        [HttpGet(Name = "GetProductosVendidos")]
        public List<ProductoVendido> GetProductosVendidos()
        {
            return ProductosVendidosHandler.GetProductosVendidos();
        }


        [HttpPut]
        public bool ModificarCantidadProducto([FromBody] PutProductoVendido productovendido)
        {
            return ProductosVendidosHandler.ModificarCantidadProducto(new ProductoVendido
            {
                Id = productovendido.Id,
                Stock = productovendido.Stock
            });
        }

        [HttpPost]


        public bool CrearProductovendido([FromBody] PostProductoVendido productovendido)
        {
            try
            {
                return ProductosVendidosHandler.AgregarProductoVendido(new ProductoVendido
                {
                    //Descripciones, Costo, PrecioVenta, Stock, IdUsuario
                    Stock = productovendido.Stock,
                    IdProducto = productovendido.IdProducto,
                    IdVenta = productovendido.IdVenta

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        
        [HttpDelete]

        public bool EliminarProductovendido([FromBody] int id)
        {
            try
            {
                return ProductosVendidosHandler.EliminarProductovendido(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
