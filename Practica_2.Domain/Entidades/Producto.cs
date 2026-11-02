using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica_2.Domain.Entidades
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public bool ValidarDatos()
        {
            return !string.IsNullOrWhiteSpace(Nombre)
                && Precio > 0
                && Stock >= 0
                && CategoriaId > 0;
        }
    }
}
