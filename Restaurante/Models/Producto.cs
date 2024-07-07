using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public partial class Producto
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public double Precio { get; set; }

    [RegularExpression(@"^(Entradas|Platos|Ensaladas|Bebidas|Postres)$", ErrorMessage = "Debe seleccionar una categoría")]
    public string Tipo { get; set; } = null!;

    public int? IdSucursal { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
