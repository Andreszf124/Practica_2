using System;
using System.Collections.Generic;
using System.Text;

namespace Practica_2.Application.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
