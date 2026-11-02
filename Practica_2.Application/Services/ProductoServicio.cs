using Practica_2.Application.Dtos;
using Practica_2.Domain.Entidades;
using Practica_2.Infrastructure.Repository;

namespace Practica_2.Application.Services
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly IRepositorioGenerico<Producto> _productoRepository;
        private readonly IRepositorioGenerico<Categoria> _categoriaRepository;

        public ProductoServicio(
            IRepositorioGenerico<Producto> productoRepository,
            IRepositorioGenerico<Categoria> categoriaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<List<ProductoDto>> ObtenerProductosAsync()
        {
            var productos = await _productoRepository
                .ObtenerTodosAsync(p => p.Categoria!);

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
                Nombre = c.Nombre
            }).ToList();
        }

        public async Task<Respuesta<ProductoDto>> CrearProductoAsync(
            ProductoDto producto)
        {
            var entidad = new Producto
            {
                Nombre = producto.Nombre.Trim(),
                Descripcion = producto.Descripcion?.Trim(),
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId
            };

            if (!entidad.ValidarDatos())
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

            entidad.Categoria = categoria;

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

            existente.Nombre = producto.Nombre.Trim();
            existente.Descripcion = producto.Descripcion?.Trim();
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;
            existente.CategoriaId = producto.CategoriaId;

            if (!existente.ValidarDatos())
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

            existente.Categoria = categoria;

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

        private static ProductoDto ConvertirDto(Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria?.Nombre
            };
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
