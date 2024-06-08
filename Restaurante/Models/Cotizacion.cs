using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Cotizacion
{
    public int Id { get; set; }

    public string? Moneda { get; set; }

    public DateOnly? Fecha { get; set; }

    public double? Valor { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
