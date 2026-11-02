using Practica_2.Infrastructure.Data;
using Practica_2.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Practica_2.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> ListarCategoriasAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> ObtenerCategoriaPorIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> AgregarCategoriaAsync(Category categoria)
        {
            try
            {
                _context.Categories.Add(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarCategoriaAsync(Category categoria)
        {
            try
            {
                var categoriaExistente = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == categoria.Id);

                if (categoriaExistente is null)
                {
                    return false;
                }

                categoriaExistente.Name = categoria.Name;
                categoriaExistente.Description = categoria.Description;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            try
            {
                var categoria = await _context.Categories.FindAsync(id);
                if (categoria != null)
                {
                    _context.Categories.Remove(categoria);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
