﻿using System;
using System.Collections.Generic;

namespace Restaurante.Models;

public partial class Permiso
{
    public int Numero { get; set; }

    public string? Descripcion { get; set; } 

    public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
}
