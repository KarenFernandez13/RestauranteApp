using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public partial class Pago
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public double Monto { get; set; }

    [RegularExpression(@"^(UYU|USD|EUR)$", ErrorMessage = "Debe seleccionar una moneda")]
    public string Moneda { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    [RegularExpression(@"^(Tarjeta|Efectivo|)$", ErrorMessage = "Debe seleccionar una forma de pago")]
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
