using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

public partial class Orden
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public double? Total { get; set; }

    [RegularExpression(@"^(Activa|Cerrada)$", ErrorMessage = "Debe seleccionar un estado")]
    public string? Estado { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdReserva { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
