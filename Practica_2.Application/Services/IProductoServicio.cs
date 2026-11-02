using Practica_2.Application.Dtos;

namespace Practica_2.Application.Services
{
    public interface IProductoServicio
    {
        Task<List<ProductoDto>> ObtenerProductosAsync();

        Task<ProductoDto?> ObtenerProductoPorIdAsync(int id);

        Task<List<CategoriaOpcionDto>> ObtenerCategoriasAsync();

        Task<Respuesta<ProductoDto>> CrearProductoAsync(
            ProductoDto producto);

        Task<Respuesta<ProductoDto>> ActualizarProductoAsync(
            ProductoDto producto);

        Task<Respuesta<ProductoDto>> EliminarProductoAsync(int id);
    }
}
