using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePagoIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    Ci = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    tipoCliente = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cliente__3213E83F6FCE4DE4", x => x.Ci);
                });

            migrationBuilder.CreateTable(
                name: "clima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    temperatura = table.Column<double>(type: "float", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    descuento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__clima__3213E83FB9DE8206", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cotizacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    moneda = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    valor = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkIdCoti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "permiso",
                columns: table => new
                {
                    numero = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkNumeroPermiso", x => x.numero);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sucursal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    direccion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    telefono = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__sucursal__3213E83F1A8B30FF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    numeroPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRolPermiso", x => new { x.IdRol, x.numeroPermiso });
                    table.ForeignKey(
                        name: "fkRolPermiso1",
                        column: x => x.IdRol,
                        principalTable: "rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fkRolPermiso2",
                        column: x => x.numeroPermiso,
                        principalTable: "permiso",
                        principalColumn: "numero",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<int>(type: "int", nullable: false),
                    capacidad = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    IdSucursal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mesa__3213E83FBDB8096A", x => x.Id);
                    table.ForeignKey(
                        name: "fkMesaSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "sucursal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    precio = table.Column<double>(type: "float", nullable: false),
                    tipo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    IdSucursal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__40F9A207AF7DCEDA", x => x.codigo);
                    table.ForeignKey(
                        name: "fkProductoSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "sucursal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "reseña",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CiCliente = table.Column<int>(type: "int", nullable: true),
                    IdSucursal = table.Column<int>(type: "int", nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    comentario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    puntaje = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reseña__3213E83F71804E6E", x => x.Id);
                    table.ForeignKey(
                        name: "fkReseñaCli",
                        column: x => x.CiCliente,
                        principalTable: "cliente",
                        principalColumn: "Ci");
                    table.ForeignKey(
                        name: "fkReseñaSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "sucursal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    contraseña = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IdSucursal = table.Column<int>(type: "int", nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__3213E83FB2A3411B", x => x.Id);
                    table.ForeignKey(
                        name: "fkUsuarioRol",
                        column: x => x.IdRol,
                        principalTable: "rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fkUsuarioSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "sucursal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "reserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CiCliente = table.Column<int>(type: "int", nullable: true),
                    IdMesa = table.Column<int>(type: "int", nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    estado = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    hora = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reserva__3213E83F2A3E2033", x => x.Id);
                    table.ForeignKey(
                        name: "fkReservaCliente",
                        column: x => x.CiCliente,
                        principalTable: "cliente",
                        principalColumn: "Ci");
                    table.ForeignKey(
                        name: "fkReservaMesa",
                        column: x => x.IdMesa,
                        principalTable: "mesa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "orden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total = table.Column<double>(type: "float", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdReserva = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orden__3213E83F4C82699E", x => x.Id);
                    table.ForeignKey(
                        name: "fkOrdenReserva",
                        column: x => x.IdReserva,
                        principalTable: "reserva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fkOrdenUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ordenDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    IdOrden = table.Column<int>(type: "int", nullable: true),
                    codigoProd = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ordenDet__3213E83F9921427F", x => x.Id);
                    table.ForeignKey(
                        name: "fkCodigoProd",
                        column: x => x.codigoProd,
                        principalTable: "producto",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fkOrdenDetalle",
                        column: x => x.IdOrden,
                        principalTable: "orden",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    monto = table.Column<double>(type: "float", nullable: false),
                    moneda = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    metodo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    IdOrden = table.Column<int>(type: "int", nullable: true),
                    IdClima = table.Column<int>(type: "int", nullable: true),
                    totalConDescuento = table.Column<double>(type: "float", nullable: true),
                    descuento = table.Column<int>(type: "int", nullable: true),
                    IdCotizacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pago__3213E83FADA35235", x => x.Id);
                    table.ForeignKey(
                        name: "fkPagoClima",
                        column: x => x.IdClima,
                        principalTable: "clima",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fkPagoCotizacion",
                        column: x => x.IdCotizacion,
                        principalTable: "cotizacion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fkPagoOrden",
                        column: x => x.IdOrden,
                        principalTable: "orden",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mesa_IdSucursal",
                table: "mesa",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_orden_IdReserva",
                table: "orden",
                column: "IdReserva",
                unique: true,
                filter: "[IdReserva] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_orden_IdUsuario",
                table: "orden",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ordenDetalle_codigoProd",
                table: "ordenDetalle",
                column: "codigoProd");

            migrationBuilder.CreateIndex(
                name: "IX_ordenDetalle_IdOrden",
                table: "ordenDetalle",
                column: "IdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_pago_IdClima",
                table: "pago",
                column: "IdClima");

            migrationBuilder.CreateIndex(
                name: "IX_pago_IdCotizacion",
                table: "pago",
                column: "IdCotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_pago_IdOrden",
                table: "pago",
                column: "IdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_producto_IdSucursal",
                table: "producto",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_reseña_CiCliente",
                table: "reseña",
                column: "CiCliente");

            migrationBuilder.CreateIndex(
                name: "IX_reseña_IdSucursal",
                table: "reseña",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_CiCliente",
                table: "reserva",
                column: "CiCliente");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_IdMesa",
                table: "reserva",
                column: "IdMesa");

            migrationBuilder.CreateIndex(
                name: "IX_rolPermiso_numeroPermiso",
                table: "rolPermiso",
                column: "numeroPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdRol",
                table: "usuario",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdSucursal",
                table: "usuario",
                column: "IdSucursal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ordenDetalle");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "reseña");

            migrationBuilder.DropTable(
                name: "rolPermiso");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "clima");

            migrationBuilder.DropTable(
                name: "cotizacion");

            migrationBuilder.DropTable(
                name: "orden");

            migrationBuilder.DropTable(
                name: "permiso");

            migrationBuilder.DropTable(
                name: "reserva");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "mesa");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "sucursal");
        }
    }
}
