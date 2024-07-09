﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurante.Models;

#nullable disable

namespace Restaurante.Migrations
{
    [DbContext(typeof(RestauranteContext))]
    partial class RestauranteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurante.Models.Cliente", b =>
                {
                    b.Property<int>("Ci")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.Property<string>("TipoCliente")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("tipoCliente");

                    b.HasKey("Ci")
                        .HasName("PK__cliente__3213E83F6FCE4DE4");

                    b.ToTable("cliente", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Clima", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("descripcion");

                    b.Property<int?>("Descuento")
                        .HasColumnType("int")
                        .HasColumnName("descuento");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<double>("Temperatura")
                        .HasColumnType("float")
                        .HasColumnName("temperatura");

                    b.HasKey("Id")
                        .HasName("PK__clima__3213E83FB9DE8206");

                    b.ToTable("clima", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Cotizacion", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<string>("Moneda")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)")
                        .HasColumnName("moneda");

                    b.Property<double?>("Valor")
                        .HasColumnType("float")
                        .HasColumnName("valor");

                    b.HasKey("Id")
                        .HasName("pkIdCoti");

                    b.ToTable("cotizacion", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Mesa", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Capacidad")
                        .HasColumnType("int")
                        .HasColumnName("capacidad");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("estado");

                    b.Property<int?>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<int>("Numero")
                        .HasColumnType("int")
                        .HasColumnName("numero");

                    b.HasKey("Id")
                        .HasName("PK__mesa__3213E83FBDB8096A");

                    b.HasIndex("IdSucursal");

                    b.ToTable("mesa", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Orden", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("estado");

                    b.Property<int?>("IdReserva")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<double?>("Total")
                        .HasColumnType("float")
                        .HasColumnName("total");

                    b.HasKey("Id")
                        .HasName("PK__orden__3213E83F4C82699E");

                    b.HasIndex("IdReserva")
                        .IsUnique()
                        .HasFilter("[IdReserva] IS NOT NULL");

                    b.HasIndex("IdUsuario");

                    b.ToTable("orden", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.OrdenDetalle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int")
                        .HasColumnName("cantidad");

                    b.Property<int?>("CodigoProd")
                        .HasColumnType("int")
                        .HasColumnName("codigoProd");

                    b.Property<int?>("IdOrden")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__ordenDet__3213E83F9921427F");

                    b.HasIndex("CodigoProd");

                    b.HasIndex("IdOrden");

                    b.ToTable("ordenDetalle", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Pago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Descuento")
                        .HasColumnType("int")
                        .HasColumnName("descuento");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<int?>("IdClima")
                        .HasColumnType("int");

                    b.Property<int?>("IdCotizacion")
                        .HasColumnType("int");

                    b.Property<int?>("IdOrden")
                        .HasColumnType("int");

                    b.Property<string>("Metodo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("metodo");

                    b.Property<string>("Moneda")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("moneda");

                    b.Property<double>("Monto")
                        .HasColumnType("float")
                        .HasColumnName("monto");

                    b.Property<double?>("TotalConDescuento")
                        .HasColumnType("float")
                        .HasColumnName("totalConDescuento");

                    b.HasKey("Id")
                        .HasName("PK__pago__3213E83FADA35235");

                    b.HasIndex("IdClima");

                    b.HasIndex("IdCotizacion");

                    b.HasIndex("IdOrden");

                    b.ToTable("pago", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Permiso", b =>
                {
                    b.Property<int>("Numero")
                        .HasColumnType("int")
                        .HasColumnName("numero");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("descripcion");

                    b.HasKey("Numero")
                        .HasName("pkNumeroPermiso");

                    b.ToTable("permiso", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Producto", b =>
                {
                    b.Property<int>("Codigo")
                        .HasColumnType("int")
                        .HasColumnName("codigo");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("descripcion");

                    b.Property<int?>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.Property<double>("Precio")
                        .HasColumnType("float")
                        .HasColumnName("precio");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("tipo");

                    b.HasKey("Codigo")
                        .HasName("PK__producto__40F9A207AF7DCEDA");

                    b.HasIndex("IdSucursal");

                    b.ToTable("producto", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CiCliente")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("estado");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<TimeOnly?>("Hora")
                        .IsRequired()
                        .HasPrecision(0)
                        .HasColumnType("time(0)")
                        .HasColumnName("hora");

                    b.Property<int?>("IdMesa")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__reserva__3213E83F2A3E2033");

                    b.HasIndex("CiCliente");

                    b.HasIndex("IdMesa");

                    b.ToTable("reserva", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Reseña", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("CiCliente")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("comentario");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("date")
                        .HasColumnName("fecha");

                    b.Property<int?>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<int>("Puntaje")
                        .HasColumnType("int")
                        .HasColumnName("puntaje");

                    b.HasKey("Id")
                        .HasName("PK__reseña__3213E83F71804E6E");

                    b.HasIndex("CiCliente");

                    b.HasIndex("IdSucursal");

                    b.ToTable("reseña", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("Id")
                        .HasName("pkRol");

                    b.ToTable("rol", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Sucursal", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("direccion");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.Property<int>("Telefono")
                        .HasColumnType("int")
                        .HasColumnName("telefono");

                    b.HasKey("Id")
                        .HasName("PK__sucursal__3213E83F1A8B30FF");

                    b.ToTable("sucursal", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("contraseña");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<int?>("IdRol")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nombre");

                    b.HasKey("Id")
                        .HasName("PK__usuario__3213E83FB2A3411B");

                    b.HasIndex("IdRol");

                    b.HasIndex("IdSucursal");

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("rolPermiso", b =>
                {
                    b.Property<int>("IdRol")
                        .HasColumnType("int");

                    b.Property<int>("numeroPermiso")
                        .HasColumnType("int");

                    b.HasKey("IdRol", "numeroPermiso")
                        .HasName("pkRolPermiso");

                    b.HasIndex("numeroPermiso");

                    b.ToTable("rolPermiso", (string)null);
                });

            modelBuilder.Entity("Restaurante.Models.Mesa", b =>
                {
                    b.HasOne("Restaurante.Models.Sucursal", "IdSucursalNavigation")
                        .WithMany("Mesas")
                        .HasForeignKey("IdSucursal")
                        .HasConstraintName("fkMesaSucursal");

                    b.Navigation("IdSucursalNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Orden", b =>
                {
                    b.HasOne("Restaurante.Models.Reserva", "IdReservaNavigation")
                        .WithOne("Orden")
                        .HasForeignKey("Restaurante.Models.Orden", "IdReserva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fkOrdenReserva");

                    b.HasOne("Restaurante.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Ordens")
                        .HasForeignKey("IdUsuario")
                        .HasConstraintName("fkOrdenUsuario");

                    b.Navigation("IdReservaNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.OrdenDetalle", b =>
                {
                    b.HasOne("Restaurante.Models.Producto", "CodigoProdNavigation")
                        .WithMany("OrdenDetalles")
                        .HasForeignKey("CodigoProd")
                        .HasConstraintName("fkCodigoProd");

                    b.HasOne("Restaurante.Models.Orden", "IdOrdenNavigation")
                        .WithMany("OrdenDetalles")
                        .HasForeignKey("IdOrden")
                        .HasConstraintName("fkOrdenDetalle");

                    b.Navigation("CodigoProdNavigation");

                    b.Navigation("IdOrdenNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Pago", b =>
                {
                    b.HasOne("Restaurante.Models.Clima", "IdClimaNavigation")
                        .WithMany("Pagos")
                        .HasForeignKey("IdClima")
                        .HasConstraintName("fkPagoClima");

                    b.HasOne("Restaurante.Models.Cotizacion", "IdCotizacionNavigation")
                        .WithMany("Pagos")
                        .HasForeignKey("IdCotizacion")
                        .HasConstraintName("fkPagoCotizacion");

                    b.HasOne("Restaurante.Models.Orden", "IdOrdenNavigation")
                        .WithMany("Pagos")
                        .HasForeignKey("IdOrden")
                        .HasConstraintName("fkPagoOrden");

                    b.Navigation("IdClimaNavigation");

                    b.Navigation("IdCotizacionNavigation");

                    b.Navigation("IdOrdenNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Producto", b =>
                {
                    b.HasOne("Restaurante.Models.Sucursal", "IdSucursalNavigation")
                        .WithMany("Productos")
                        .HasForeignKey("IdSucursal")
                        .HasConstraintName("fkProductoSucursal");

                    b.Navigation("IdSucursalNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Reserva", b =>
                {
                    b.HasOne("Restaurante.Models.Cliente", "CiClienteNavigation")
                        .WithMany("Reservas")
                        .HasForeignKey("CiCliente")
                        .HasConstraintName("fkReservaCliente");

                    b.HasOne("Restaurante.Models.Mesa", "IdMesaNavigation")
                        .WithMany("Reservas")
                        .HasForeignKey("IdMesa")
                        .HasConstraintName("fkReservaMesa");

                    b.Navigation("CiClienteNavigation");

                    b.Navigation("IdMesaNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Reseña", b =>
                {
                    b.HasOne("Restaurante.Models.Cliente", "CiClienteNavigation")
                        .WithMany("Reseñas")
                        .HasForeignKey("CiCliente")
                        .HasConstraintName("fkReseñaCli");

                    b.HasOne("Restaurante.Models.Sucursal", "IdSucursalNavigation")
                        .WithMany("Reseñas")
                        .HasForeignKey("IdSucursal")
                        .HasConstraintName("fkReseñaSucursal");

                    b.Navigation("CiClienteNavigation");

                    b.Navigation("IdSucursalNavigation");
                });

            modelBuilder.Entity("Restaurante.Models.Usuario", b =>
                {
                    b.HasOne("Restaurante.Models.Rol", "IdRolNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fkUsuarioRol");

                    b.HasOne("Restaurante.Models.Sucursal", "IdSucursalNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdSucursal")
                        .HasConstraintName("fkUsuarioSucursal");

                    b.Navigation("IdRolNavigation");

                    b.Navigation("IdSucursalNavigation");
                });

            modelBuilder.Entity("rolPermiso", b =>
                {
                    b.HasOne("Restaurante.Models.Rol", null)
                        .WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fkRolPermiso1");

                    b.HasOne("Restaurante.Models.Permiso", null)
                        .WithMany()
                        .HasForeignKey("numeroPermiso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fkRolPermiso2");
                });

            modelBuilder.Entity("Restaurante.Models.Cliente", b =>
                {
                    b.Navigation("Reservas");

                    b.Navigation("Reseñas");
                });

            modelBuilder.Entity("Restaurante.Models.Clima", b =>
                {
                    b.Navigation("Pagos");
                });

            modelBuilder.Entity("Restaurante.Models.Cotizacion", b =>
                {
                    b.Navigation("Pagos");
                });

            modelBuilder.Entity("Restaurante.Models.Mesa", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("Restaurante.Models.Orden", b =>
                {
                    b.Navigation("OrdenDetalles");

                    b.Navigation("Pagos");
                });

            modelBuilder.Entity("Restaurante.Models.Producto", b =>
                {
                    b.Navigation("OrdenDetalles");
                });

            modelBuilder.Entity("Restaurante.Models.Reserva", b =>
                {
                    b.Navigation("Orden");
                });

            modelBuilder.Entity("Restaurante.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Restaurante.Models.Sucursal", b =>
                {
                    b.Navigation("Mesas");

                    b.Navigation("Productos");

                    b.Navigation("Reseñas");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Restaurante.Models.Usuario", b =>
                {
                    b.Navigation("Ordens");
                });
#pragma warning restore 612, 618
        }
    }
}