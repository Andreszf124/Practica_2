using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Practica_2.Application.Dtos;
using Practica_2.Domain.Entidades;
using Practica_2.Infrastructure.Repositories;

namespace Practica_2.Application.Services
{
    public class CategoriaServicio : ICategoriaServicio
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaServicio(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerCategoriasAsync()
        {
            var categorias = await _categoriaRepository.ListarCategoriasAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto> ObtenerCategoriaPorIdAsync(int id)
        {
            var categoria = await _categoriaRepository.ObtenerCategoriaPorIdAsync(id);
            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<bool> CrearCategoriaAsync(CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Category>(categoriaDto);
            return await _categoriaRepository.AgregarCategoriaAsync(categoria);
        }

        public async Task<bool> ActualizarCategoriaAsync(CategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Category>(categoriaDto);
            return await _categoriaRepository.ActualizarCategoriaAsync(categoria);
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            return await _categoriaRepository.EliminarCategoriaAsync(id);
        }
    }
}
