using System.Linq.Expressions;

namespace Practica_2.Infrastructure.Repository
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task<List<T>> ObtenerTodosAsync(
            params Expression<Func<T, object>>[] relaciones);

        Task<T?> ObtenerPorIdAsync(int id);

        Task<bool> CrearAsync(T entidad);

        Task<bool> ActualizarAsync(T entidad);

        Task<bool> EliminarAsync(int id);
    }
}
