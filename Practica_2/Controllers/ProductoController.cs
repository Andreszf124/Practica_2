using Microsoft.AspNetCore.Mvc;
using Practica_2.Application.Dtos;
using Practica_2.Application.Services;

namespace Practica_2.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoServicio _productoServicio;

        public ProductoController(IProductoServicio productoServicio)
        {
            _productoServicio = productoServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categorias = await _productoServicio
                .ObtenerCategoriasAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            var productos = await _productoServicio.ObtenerProductosAsync();

            return Json(productos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductoPorId(int id)
        {
            var producto = await _productoServicio
                .ObtenerProductoPorIdAsync(id);

            if (producto == null)
            {
                return NotFound(new
                {
                    mensaje = "Producto no encontrado."
                });
            }

            return Json(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProducto([FromForm] ProductoDto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "Revise los datos del formulario."
                });
            }

            var respuesta = await _productoServicio
                .CrearProductoAsync(producto);

            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarProducto(
            [FromForm] ProductoDto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "Revise los datos del formulario."
                });
            }

            var respuesta = await _productoServicio
                .ActualizarProductoAsync(producto);

            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var respuesta = await _productoServicio
                .EliminarProductoAsync(id);

            return Json(respuesta);
        }
    }
}
