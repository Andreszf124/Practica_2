using System;
using System.Collections.Generic;
using System.Text;
using Practica_2.Application.Dtos;

namespace Practica_2.Application.Services
{
    public interface ICategoriaServicio
    {
        Task<IEnumerable<CategoriaDto>> ObtenerCategoriasAsync();
        Task<CategoriaDto> ObtenerCategoriaPorIdAsync(int id);
        Task<bool> CrearCategoriaAsync(CategoriaDto categoriaDto);
        Task<bool> ActualizarCategoriaAsync(CategoriaDto categoriaDto);
        Task<bool> EliminarCategoriaAsync(int id);
    }
}
