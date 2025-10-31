using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RegistroAsistenciasDF.Models;

public partial class RegistroDfContext : DbContext
{
    public RegistroDfContext()
    {
    }

    public RegistroDfContext(DbContextOptions<RegistroDfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contrato> Contratos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }
    
    public virtual DbSet<TipoHorario> TipoHorarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Persist Security Info=False;Integrated Security=true; Initial Catalog= RegistroDF; Server=LAPTOP-QVE03SDO\\SQLEXPRESS;Encrypt=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.HasKey(e => e.IdContrato).HasName("PK__CONTRATO__FF5F2A561394729B");

            entity.ToTable("CONTRATO");

            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdTipoHorario).HasColumnName("id_tipo_horario");
            entity.Property(e => e.Sueldo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sueldo");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CONTRATO__id_emp__5070F446");

            entity.HasOne(d => d.IdTipoHorarioNavigation).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.IdTipoHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CONTRATO__id_tip__5165187F");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__EMPLEADO__88B51394733F705C");

            entity.ToTable("EMPLEADOS");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__EMPLEADOS__id_us__4BAC3F29");
        });

        modelBuilder.Entity<TipoHorario>(entity =>
        {
            entity.HasKey(e => e.IdTipoHorario).HasName("PK__TIPO_HOR__316C649F8A7593B5");

            entity.ToTable("TIPO_HORARIO");

            entity.Property(e => e.IdTipoHorario).HasColumnName("id_tipo_horario");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIO__4E3E04ADD3DC8365");

            entity.ToTable("USUARIO");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
