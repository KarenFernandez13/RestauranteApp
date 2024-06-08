using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Clima
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public double Temperatura { get; set; }

    public string? Descripcion { get; set; }

    public int? Descuento { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
