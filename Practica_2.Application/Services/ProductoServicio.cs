using Practica_2.Application.Dtos;
using Practica_2.Domain.Entidades;
using Practica_2.Infrastructure.Repository;

namespace Practica_2.Application.Services
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly IRepositorioGenerico<Product> _productoRepository;
        private readonly IRepositorioGenerico<Category> _categoriaRepository;

        public ProductoServicio(
            IRepositorioGenerico<Product> productoRepository,
            IRepositorioGenerico<Category> categoriaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<List<ProductoDto>> ObtenerProductosAsync()
        {
            var productos = await _productoRepository
                .ObtenerTodosAsync(p => p.Category!);

            return productos.Select(ConvertirDto).ToList();
        }

        public async Task<ProductoDto?> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return null;
            }

            return ConvertirDto(producto);
        }

        public async Task<List<CategoriaOpcionDto>> ObtenerCategoriasAsync()
        {
            var categorias = await _categoriaRepository.ObtenerTodosAsync();

            return categorias.Select(c => new CategoriaOpcionDto
            {
                Id = c.Id,
                Nombre = c.Name
            }).ToList();
        }

        public async Task<Respuesta<ProductoDto>> CrearProductoAsync(
            ProductoDto producto)
        {
            var entidad = new Product
            {
                Name = producto.Nombre.Trim(),
                Description = producto.Descripcion?.Trim(),
                Price = (double)producto.Precio,
                Stock = producto.Stock,
                CategoryId = producto.CategoriaId
            };

            if (!ValidarDatos(entidad))
            {
                return Error("Los datos del producto no son válidos.");
            }

            var categoria = await _categoriaRepository
                .ObtenerPorIdAsync(producto.CategoriaId);

            if (categoria == null)
            {
                return Error("La categoría seleccionada no existe.");
            }

            if (!await _productoRepository.CrearAsync(entidad))
            {
                return Error("No se pudo crear el producto.", 500);
            }

            entidad.Category = categoria;

            return new Respuesta<ProductoDto>
            {
                Mensaje = "Producto creado correctamente.",
                Dato = ConvertirDto(entidad)
            };
        }

        public async Task<Respuesta<ProductoDto>> ActualizarProductoAsync(
            ProductoDto producto)
        {
            if (producto.Id <= 0)
            {
                return Error("El ID del producto no es válido.");
            }

            var existente = await _productoRepository
                .ObtenerPorIdAsync(producto.Id);

            if (existente == null)
            {
                return Error("El producto no existe.", 404);
            }

            existente.Name = producto.Nombre.Trim();
            existente.Description = producto.Descripcion?.Trim();
            existente.Price = (double)producto.Precio;
            existente.Stock = producto.Stock;
            existente.CategoryId = producto.CategoriaId;

            if (!ValidarDatos(existente))
            {
                return Error("Los datos del producto no son válidos.");
            }

            var categoria = await _categoriaRepository
                .ObtenerPorIdAsync(producto.CategoriaId);

            if (categoria == null)
            {
                return Error("La categoría seleccionada no existe.");
            }

            if (!await _productoRepository.ActualizarAsync(existente))
            {
                return Error("No se pudo actualizar el producto.", 500);
            }

            existente.Category = categoria;

            return new Respuesta<ProductoDto>
            {
                Mensaje = "Producto actualizado correctamente.",
                Dato = ConvertirDto(existente)
            };
        }

        public async Task<Respuesta<ProductoDto>> EliminarProductoAsync(int id)
        {
            if (id <= 0)
            {
                return Error("El ID del producto no es válido.");
            }

            if (!await _productoRepository.EliminarAsync(id))
            {
                return Error("El producto no existe.", 404);
            }

            return new Respuesta<ProductoDto>
            {
                Mensaje = "Producto eliminado correctamente."
            };
        }

        private static ProductoDto ConvertirDto(Product producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Name,
                Descripcion = producto.Description,
                Precio = (decimal)producto.Price,
                Stock = producto.Stock,
                CategoriaId = producto.CategoryId,
                CategoriaNombre = producto.Category?.Name
            };
        }

        private static bool ValidarDatos(Product producto)
        {
            return !string.IsNullOrWhiteSpace(producto.Name)
                && producto.Price > 0
                && producto.Stock >= 0
                && producto.CategoryId > 0;
        }

        private static Respuesta<ProductoDto> Error(
            string mensaje, int codigo = 400)
        {
            return new Respuesta<ProductoDto>
            {
                EsCorrecto = false,
                Mensaje = mensaje,
                Codigo = codigo
            };
        }
    }
}
