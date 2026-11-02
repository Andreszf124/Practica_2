using System;
using System.Collections.Generic;
using System.Text;
using Practica_2.Domain.Entidades;

namespace Practica_2.Infrastructure.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Category>> ListarCategoriasAsync();
        Task<Category> ObtenerCategoriaPorIdAsync(int id);
        Task<bool> AgregarCategoriaAsync(Category categoria);
        Task<bool> ActualizarCategoriaAsync(Category categoria);
        Task<bool> EliminarCategoriaAsync(int id);
    }
}
