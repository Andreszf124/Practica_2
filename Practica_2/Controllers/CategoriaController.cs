using Microsoft.AspNetCore.Mvc;
using Practica_2.Application.Dtos;
using Practica_2.Application.Services;

namespace Practica_2.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaServicio _categoriaServicio;

        public CategoriaController(ICategoriaServicio categoriaServicio)
        {
            _categoriaServicio = categoriaServicio;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaServicio.ObtenerCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> ObtenerCategorias()
        {
            var categorias = await _categoriaServicio.ObtenerCategoriasAsync();
            return Json(categorias);
        }

        public async Task<IActionResult> ObtenerCategoriaPorId(int id)
        {
            var categoria = await _categoriaServicio.ObtenerCategoriaPorIdAsync(id);
            return Json(categoria);
        }

        public async Task<IActionResult> CrearCategoria(CategoriaDto categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _categoriaServicio.CrearCategoriaAsync(categoria);
            return Json(respuesta);
        }

        public async Task<IActionResult> ActualizarCategoria(CategoriaDto categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _categoriaServicio.ActualizarCategoriaAsync(categoria);
            return Json(respuesta);
        }

        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var respuesta = await _categoriaServicio.EliminarCategoriaAsync(id);
            return Json(respuesta);
        }
    }
}
