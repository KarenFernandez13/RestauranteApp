using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public partial class Rol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Nombre es requerido")]
    [RegularExpression(@"^(Admin|Cliente|Mozo|Cajero)$", ErrorMessage = "Debe seleccionar una opción")]
    public string? Nombre { get; set; }

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
