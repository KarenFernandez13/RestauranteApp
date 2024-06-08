using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Orden
{
    public int Id { get; set; }

    public double? Total { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdReserva { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
