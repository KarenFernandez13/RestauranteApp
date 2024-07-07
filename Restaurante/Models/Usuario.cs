using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string? Email { get; set; }

    public int? IdSucursal { get; set; }
    
    [Required]
    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();
}
