using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

public partial class MethodDBContext : DbContext
{
    public MethodDBContext()
    {
    }

    public MethodDBContext(DbContextOptions<MethodDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Method> Methods { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=MethodDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Method>(entity =>
        {
            entity.ToTable("method");

            entity.HasIndex(e => e.Id, "IX_method_id").IsUnique();

            entity.HasIndex(e => e.Name, "IX_method_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasColumnType("BOOLEAN")
                .HasColumnName("active");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("task");

            entity.HasIndex(e => e.Id, "IX_task_id").IsUnique();

            entity.HasIndex(e => e.Name, "IX_task_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alpha)
                .HasColumnType("DOUBLE (10, 2)")
                .HasColumnName("alpha");
            entity.Property(e => e.Beta)
                .HasColumnType("DOUBLE (10, 2)")
                .HasColumnName("beta");
            entity.Property(e => e.MassConsumption)
                .HasColumnType("DOUBLE (10, 3)")
                .HasColumnName("massConsumption");
            entity.Property(e => e.Mu)
                .HasColumnType("DOUBLE (10, 2)")
                .HasColumnName("mu");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Precision)
                .HasColumnType("DOUBLE (10, 5)")
                .HasColumnName("precision");
            entity.Property(e => e.Pressure)
                .HasColumnType("DOUBLE (10, 2)")
                .HasColumnName("pressure");
            entity.Property(e => e.Price)
                .HasColumnType("DOUBLE (10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.T1max)
                .HasColumnType("DOUBLE (4, 2)")
                .HasColumnName("t1max");
            entity.Property(e => e.T1min)
                .HasColumnType("DOUBLE (4, 2)")
                .HasColumnName("t1min");
            entity.Property(e => e.T2max)
                .HasColumnType("DOUBLE (4, 2)")
                .HasColumnName("t2max");
            entity.Property(e => e.T2min)
                .HasColumnType("DOUBLE (4, 2)")
                .HasColumnName("t2min");
            entity.Property(e => e.TempCondition)
                .HasColumnType("DOUBLE (4, 2)")
                .HasColumnName("tempCondition");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
