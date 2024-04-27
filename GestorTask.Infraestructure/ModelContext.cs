using System;
using System.Collections.Generic;
using GestorTask.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorTask.Infraestructure;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tasks> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseOracle("User Id=jonathan;Password=jonadt;Data Source=localhost:1521/orlc;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JONATHAN")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.ToTable("TASK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DateEnd)
                .HasColumnType("DATE")
                .HasColumnName("DATE_END");
            entity.Property(e => e.DateInit)
                .HasColumnType("DATE")
                .HasColumnName("DATE_INIT");
            entity.Property(e => e.DateRegister)
                .HasColumnType("DATE")
                .HasColumnName("DATE_REGISTER");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.RegisterBy)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("REGISTER_BY");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_USER");

            entity.ToTable("USERS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DateRegister)
                .HasColumnType("DATE")
                .HasColumnName("DATE_REGISTER");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Salt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SALT");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });
        modelBuilder.HasSequence("TASK_SEQ");
        modelBuilder.HasSequence("USER_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
