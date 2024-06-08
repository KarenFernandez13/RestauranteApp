using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Mesa
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public int Capacidad { get; set; }

    public string Estado { get; set; } = null!;

    public int? IdSucursal { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
