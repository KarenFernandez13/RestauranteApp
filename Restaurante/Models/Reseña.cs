using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Reseña
{
    public int Id { get; set; }

    public int? CiCliente { get; set; }

    public int? IdSucursal { get; set; }

    public DateOnly Fecha { get; set; }

    public string Comentario { get; set; } = null!;

    public int Puntaje { get; set; }

    public virtual Cliente? CiClienteNavigation { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }
}
