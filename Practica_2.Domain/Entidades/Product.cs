using System;
using System.Collections.Generic;

namespace Practica_2.Domain.Entidades;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public int Stock { get; set; }

    public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;
}
