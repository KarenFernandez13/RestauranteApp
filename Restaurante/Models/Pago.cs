using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Pago
{
    public int Id { get; set; }

    public double Monto { get; set; }

    public string Moneda { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public string Metodo { get; set; } = null!;

    public int? IdOrden { get; set; }

    public int? IdClima { get; set; }

    public double? TotalConDescuento { get; set; }

    public int? Descuento { get; set; }

    public int? IdCotizacion { get; set; }

    public virtual Clima? IdClimaNavigation { get; set; }

    public virtual Cotizacion? IdCotizacionNavigation { get; set; }

    public virtual Orden? IdOrdenNavigation { get; set; }
}
