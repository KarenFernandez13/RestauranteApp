using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public partial class OrdenDetalle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public int? IdOrden { get; set; }

    public int? CodigoProd { get; set; }

    public virtual Producto? CodigoProdNavigation { get; set; }

    public virtual Orden? IdOrdenNavigation { get; set; }
}
