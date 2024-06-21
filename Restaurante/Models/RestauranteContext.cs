using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Restaurante.Models;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext()
    {
    }

    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Clima> Climas { get; set; }

    public virtual DbSet<Cotizacion> Cotizacions { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Orden> Orden { get; set; }

    public virtual DbSet<OrdenDetalle> OrdenDetalles { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Reseña> Reseñas { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

        var connectionString = configuration.GetConnectionString("CadenaSQL");
        optionsBuilder.UseSqlServer(connectionString);

    }
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    ////=> optionsBuilder.UseSqlServer("CadenaSQL");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Ci).HasName("PK__cliente__3213E83F6FCE4DE4");

            entity.ToTable("cliente");

            entity.Property(e => e.Ci).ValueGeneratedNever();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tipoCliente");
        });

        modelBuilder.Entity<Clima>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__clima__3213E83FB9DE8206");

            entity.ToTable("clima");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Temperatura).HasColumnName("temperatura");
        });

        modelBuilder.Entity<Cotizacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkIdCoti");

            entity.ToTable("cotizacion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Moneda)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("moneda");
            entity.Property(e => e.Valor).HasColumnName("valor");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mesa__3213E83FBDB8096A");

            entity.ToTable("mesa");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Numero).HasColumnName("numero");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("fkMesaSucursal");
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orden__3213E83F4C82699E");

            entity.ToTable("orden");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Estado).HasColumnName("estado");

            entity.HasOne(d => d.IdReservaNavigation).WithOne(r => r.Orden)
                .HasForeignKey<Orden>(o => o.IdReserva)
                .OnDelete(DeleteBehavior.Cascade)            
                .HasConstraintName("fkOrdenReserva");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ordens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fkOrdenUsuario");

            base.OnModelCreating(modelBuilder);               
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ordenDet__3213E83F9921427F");

            entity.ToTable("ordenDetalle");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CodigoProd).HasColumnName("codigoProd");

            entity.HasOne(d => d.CodigoProdNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.CodigoProd)
                .HasConstraintName("fkCodigoProd");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fkOrdenDetalle");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pago__3213E83FADA35235");

            entity.ToTable("pago");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Descuento).HasColumnName("descuento");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Metodo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("metodo");
            entity.Property(e => e.Moneda)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("moneda");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.TotalConDescuento).HasColumnName("totalConDescuento");

            entity.HasOne(d => d.IdClimaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdClima)
                .HasConstraintName("fkPagoClima");

            entity.HasOne(d => d.IdCotizacionNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdCotizacion)
                .HasConstraintName("fkPagoCotizacion");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fkPagoOrden");
        });

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Numero).HasName("pkNumeroPermiso");

            entity.ToTable("permiso");

            entity.Property(e => e.Numero)
                .ValueGeneratedNever()
                .HasColumnName("numero");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pkIdRol");

            entity.ToTable("rol");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Permiso>()
            .HasMany(p => p.Roles)
            .WithMany(r => r.Permisos)
            .UsingEntity<Dictionary<string, object>>(
                "PermisoRol",
                j => j
                    .HasOne<Rol>()
                    .WithMany()
                    .HasForeignKey("IdRol")
                    .HasConstraintName("fkPermisoRolRol"),
                j => j
                    .HasOne<Permiso>()
                    .WithMany()
                    .HasForeignKey("NumeroPermiso")
                    .HasConstraintName("fkPermisoRolPermiso"));

        //modelBuilder.Entity<Permiso>(entity =>
        //{
        //    entity.HasKey(e => e.Numero).HasName("pkNumeroPermiso");

        //    entity.ToTable("permiso");

        //    entity.Property(e => e.Numero)
        //        .ValueGeneratedNever()
        //        .HasColumnName("numero");
        //    entity.Property(e => e.Descripcion)
        //        .HasMaxLength(50)
        //        .IsUnicode(false)
        //        .HasColumnName("descripcion");

        //    entity.HasOne(d => d.Rol).WithMany(p => p.Permisos)
        //        .HasForeignKey(d => d.IdRol)
        //        .HasConstraintName("fkPermisoRol");
        //});

        modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Codigo).HasName("PK__producto__40F9A207AF7DCEDA");

                entity.ToTable("producto");

                entity.Property(e => e.Codigo)
                    .ValueGeneratedNever()
                    .HasColumnName("codigo");
                entity.Property(e => e.Descripcion)
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnName("descripcion");
                entity.Property(e => e.Nombre)
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnName("nombre");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Tipo)
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnName("tipo");

                entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Productos)
                        .HasForeignKey(d => d.IdSucursal)
                        .HasConstraintName("fkProductoSucursal");
            });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reserva__3213E83F2A3E2033");

            entity.ToTable("reserva");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Estado)
                            .HasMaxLength(15)
                            .IsUnicode(false)
                            .HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Hora)
                            .HasPrecision(0)
                            .HasColumnName("hora");

            entity.HasOne(d => d.CiClienteNavigation).WithMany(p => p.Reservas)
                            .HasForeignKey(d => d.CiCliente)
                            .HasConstraintName("fkReservaCliente");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Reservas)
                            .HasForeignKey(d => d.IdMesa)
                            .HasConstraintName("fkReservaMesa");
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reseña__3213E83F71804E6E");

            entity.ToTable("reseña");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comentario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Puntaje).HasColumnName("puntaje");

            entity.HasOne(d => d.CiClienteNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.CiCliente)
                .HasConstraintName("fkReseñaCli");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("fkReseñaSucursal");
        });

        //modelBuilder.Entity<Rol>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("pkRol");

        //    entity.ToTable("rol");

        //    entity.Property(e => e.Id).ValueGeneratedNever();
        //    entity.Property(e => e.Nombre)
        //        .HasMaxLength(50)
        //        .IsUnicode(false)
        //        .HasColumnName("nombre");
        //});

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sucursal__3213E83F1A8B30FF");

            entity.ToTable("sucursal");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuario__3213E83FB2A3411B");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Contraseña)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("fkUsuarioRol");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("fkUsuarioSucursal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
