using Microsoft.AspNetCore.Mvc;
using MiPrimeraApi2.Controllers.DTOS;
using MiPrimeraApi2.Model;
using MiPrimeraApi2.Repository;

namespace MiPrimeraApi2.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VentaController : Controller
    {

        [HttpGet(Name = "GetVenta")]
        public List<Venta> GetVenta()
        {
            return VentaHandler.GetVentas();
        }


        [HttpPost]
        public bool CrearVenta([FromBody] PostProductoVendido productovendido)
        {


            /* Cargar Venta: Recibe una lista de productos y el número de IdUsuario de quien la efectuó, 
                problema : se me complico esta parte de ingresar varios productos en una lista para cargar la venta 
                resolucion:  el Post de venta solo carga un articulo, pero registra venta nueva en ventas y productos vendidos asi como descontar el stock en productos.*/

            int nuevoIDVenta = 0;
            bool productoAgregado = false;
            bool stockModificado = false;

            try
            {
                // primero cargar una nueva Venta en la base de datos, 
                nuevoIDVenta = VentaHandler.CrearVenta();

                //  cargar los productos recibidos en la base de ProductosVendidos
                productoAgregado = ProductosVendidosHandler.AgregarProductoVendido(new ProductoVendido
                {
                    Stock = productovendido.Stock,
                    IdProducto = productovendido.IdProducto,
                    IdVenta = nuevoIDVenta

                });

                //  descontar el stock en la base de productos por el otro
                stockModificado = ProductoHandler.ModificarStockdeProducto(new Producto 
                {
                    Stock = productovendido.Stock, 
                    Id = productovendido.IdProducto 
                }  , nuevoIDVenta);


                if (productoAgregado && stockModificado) { return true; } else { return false; }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        

    }
}
