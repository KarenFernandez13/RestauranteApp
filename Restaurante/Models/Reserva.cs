using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models;

public partial class Reserva
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? CiCliente { get; set; }

    public int? IdMesa { get; set; }

    [Required(ErrorMessage = "El campo Fecha es requerido")]
    public DateOnly Fecha { get; set; }

    [Required(ErrorMessage = "El campo Estado es requerido")]
    [RegularExpression(@"^(Pendiente|Confirmada|Cancelada)$", ErrorMessage = "Debe seleccionar un estado")]
    public string Estado { get; set; } = null!;

    [Required(ErrorMessage = "El campo Hora es requerido")]
    public TimeOnly? Hora { get; set; }

    public virtual Cliente? CiClienteNavigation { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }

    public virtual Orden? Orden { get; set; }
}
