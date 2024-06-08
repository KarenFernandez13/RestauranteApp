using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Reserva
{
    public int Id { get; set; }

    public int? CiCliente { get; set; }

    public int? IdMesa { get; set; }

    public DateOnly Fecha { get; set; }

    public string Estado { get; set; } = null!;

    public TimeOnly? Hora { get; set; }

    public virtual Cliente? CiClienteNavigation { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }

    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();
}
