using AutoMapper;
using Practica_2.Application.Dtos;
using Practica_2.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_2.Application.Mapping
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            // Agrega esta línea dentro del constructor de tu clase MapeoClases existente:
            CreateMap<Category, CategoriaDto>().ReverseMap();
        }
    }
}
