using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Areaprofissional> Areaprofissionals { get; set; }

    public virtual DbSet<Experiencium> Experiencia { get; set; }

    public virtual DbSet<Perfil> Perfils { get; set; }

    public virtual DbSet<Proposta> Propostas { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Skillprof> Skillprofs { get; set; }

    public virtual DbSet<Skillspropostum> Skillsproposta { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=es2;Username=es2;Password=es2;Port=59007");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("topology", "postgis_topology");

        modelBuilder.Entity<Areaprofissional>(entity =>
        {
            entity.HasKey(e => e.IdAreaProfissional).HasName("areaprofissioanl_pk");

            entity.ToTable("areaprofissional");

            entity.Property(e => e.IdAreaProfissional)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_area_profissional");
            entity.Property(e => e.NomeAreaPrfossional)
                .HasMaxLength(100)
                .HasColumnName("nome_area_prfossional");
        });

        modelBuilder.Entity<Experiencium>(entity =>
        {
            entity.HasKey(e => e.IdExperiencia).HasName("experiencia_pkey");

            entity.ToTable("experiencia");

            entity.Property(e => e.IdExperiencia)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_experiencia");
            entity.Property(e => e.Anofim).HasColumnName("anofim");
            entity.Property(e => e.Anoinicio).HasColumnName("anoinicio");
            entity.Property(e => e.Continuo)
                .HasDefaultValueSql("false")
                .HasColumnName("continuo");
            entity.Property(e => e.IdPerfil).HasColumnName("id_perfil");
            entity.Property(e => e.NomeEmpresa)
                .HasMaxLength(100)
                .HasColumnName("nome_empresa");
            entity.Property(e => e.NomeExperiencia)
                .HasMaxLength(100)
                .HasColumnName("nome_experiencia");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Experiencia)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("experiencia_id_perfil_fkey");
        });

        modelBuilder.Entity<Perfil>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("perfil_pk");

            entity.ToTable("perfil");

            entity.Property(e => e.IdPerfil)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_perfil");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.NomePerfil)
                .HasMaxLength(100)
                .HasColumnName("nome_perfil");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .HasColumnName("pais");
            entity.Property(e => e.Precohora).HasColumnName("precohora");
            entity.Property(e => e.Publico)
                .HasDefaultValueSql("false")
                .HasColumnName("publico");
        });

        modelBuilder.Entity<Proposta>(entity =>
        {
            entity.HasKey(e => e.IdPropostas).HasName("propostas_pkey");

            entity.ToTable("propostas");

            entity.Property(e => e.IdPropostas)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_propostas");
            entity.Property(e => e.Descricao)
                .HasMaxLength(1000)
                .HasColumnName("descricao");
            entity.Property(e => e.IdAreaProfissional).HasColumnName("id_area_profissional");
            entity.Property(e => e.NomePropostas)
                .HasMaxLength(100)
                .HasColumnName("nome_propostas");
            entity.Property(e => e.Ntotalhoras).HasColumnName("ntotalhoras");

            entity.HasOne(d => d.IdAreaProfissionalNavigation).WithMany(p => p.Proposta)
                .HasForeignKey(d => d.IdAreaProfissional)
                .HasConstraintName("propostas_id_area_profissional_fkey");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.IdSkills).HasName("skills_pkey");

            entity.ToTable("skills");

            entity.Property(e => e.IdSkills)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_skills");
            entity.Property(e => e.IdAreaProfissional).HasColumnName("id_area_profissional");
            entity.Property(e => e.NomeSkills)
                .HasMaxLength(100)
                .HasColumnName("nome_skills");

            entity.HasOne(d => d.IdAreaProfissionalNavigation).WithMany(p => p.Skills)
                .HasForeignKey(d => d.IdAreaProfissional)
                .HasConstraintName("skills_id_area_profissional_fkey");
        });

        modelBuilder.Entity<Skillprof>(entity =>
        {
            entity.HasKey(e => e.IdSkillsprof).HasName("skillprof_pkey");

            entity.ToTable("skillprof");

            entity.Property(e => e.IdSkillsprof)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_skillsprof");
            entity.Property(e => e.IdPerfil).HasColumnName("id_perfil");
            entity.Property(e => e.IdSkills).HasColumnName("id_skills");
            entity.Property(e => e.Nhoras).HasColumnName("nhoras");

            entity.HasOne(d => d.IdPerfilNavigation).WithMany(p => p.Skillprofs)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("skillprof_id_perfil_fkey");

            entity.HasOne(d => d.IdSkillsNavigation).WithMany(p => p.Skillprofs)
                .HasForeignKey(d => d.IdSkills)
                .HasConstraintName("skillprof_id_skills_fkey");
        });

        modelBuilder.Entity<Skillspropostum>(entity =>
        {
            entity.HasKey(e => e.IdSkillsProposta).HasName("skillsproposta_pkey");

            entity.ToTable("skillsproposta");

            entity.Property(e => e.IdSkillsProposta)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_skills_proposta");
            entity.Property(e => e.IdPropostas).HasColumnName("id_propostas");
            entity.Property(e => e.IdSkills).HasColumnName("id_skills");
            entity.Property(e => e.NMinimoHorasSkill).HasColumnName("n_minimo_horas_skill");

            entity.HasOne(d => d.IdPropostasNavigation).WithMany(p => p.Skillsproposta)
                .HasForeignKey(d => d.IdPropostas)
                .HasConstraintName("skillsproposta_id_propostas_fkey");

            entity.HasOne(d => d.IdSkillsNavigation).WithMany(p => p.Skillsproposta)
                .HasForeignKey(d => d.IdSkills)
                .HasConstraintName("skillsproposta_id_skills_fkey");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.IdUtilizador).HasName("utilizador_pkey");

            entity.ToTable("utilizador");

            entity.Property(e => e.IdUtilizador)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id_utilizador");
            entity.Property(e => e.NomeUtilizador)
                .HasMaxLength(100)
                .HasColumnName("nome_utilizador");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.TipoUtilizador).HasColumnName("tipo_utilizador");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
