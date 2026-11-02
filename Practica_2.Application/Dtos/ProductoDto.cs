using System.ComponentModel.DataAnnotations;

namespace Practica_2.Application.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Range(typeof(decimal), "0.01", "999999999",
            ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaId { get; set; }

        public string? CategoriaNombre { get; set; }
    }
}
