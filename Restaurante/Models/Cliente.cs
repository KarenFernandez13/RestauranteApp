using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Cliente
{
    public int Ci { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Email { get; set; }

    public string TipoCliente { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
}
