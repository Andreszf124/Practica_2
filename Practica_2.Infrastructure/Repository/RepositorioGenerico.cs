using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Practica_2.Infrastructure.Data;

namespace Practica_2.Infrastructure.Repository
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T>
        where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositorioGenerico(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ObtenerTodosAsync(
            params Expression<Func<T, object>>[] relaciones)
        {
            IQueryable<T> consulta = _context.Set<T>()
                .AsNoTracking();

            foreach (var relacion in relaciones)
            {
                consulta = consulta.Include(relacion);
            }

            return await consulta.ToListAsync();
        }

        public async Task<T?> ObtenerPorIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> CrearAsync(T entidad)
        {
            await _context.Set<T>().AddAsync(entidad);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(T entidad)
        {
            _context.Set<T>().Update(entidad);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var entidad = await _context.Set<T>().FindAsync(id);

            if (entidad == null)
            {
                return false;
            }

            _context.Set<T>().Remove(entidad);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
