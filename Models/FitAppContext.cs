using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Fit.Models;

public partial class FitAppContext : DbContext
{
    public FitAppContext()
    {
    }

    public FitAppContext(DbContextOptions<FitAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aktivnost> Aktivnosts { get; set; }

    public virtual DbSet<Cilj> Ciljs { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Obrok> Obroks { get; set; }

    public virtual DbSet<Rola> Rolas { get; set; }

    public virtual DbSet<TipAktivnosti> TipAktivnostis { get; set; }

    public virtual DbSet<TipObroka> TipObrokas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=fitapp", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Aktivnost>(entity =>
        {
            entity.HasKey(e => e.IdAktivnost).HasName("PRIMARY");

            entity.ToTable("aktivnost");

            entity.HasIndex(e => e.IdKorisnika, "fk_aktivnost_korisnik_idx");

            entity.HasIndex(e => e.IdTipaAktivnosti, "fk_aktivnost_tip_aktivnosti_idx");

            entity.Property(e => e.IdAktivnost).HasColumnName("id_aktivnost");
            entity.Property(e => e.IdKorisnika).HasColumnName("id_korisnika");
            entity.Property(e => e.IdTipaAktivnosti).HasColumnName("id_tipa_aktivnosti");
            entity.Property(e => e.TrajanjeUMinutama).HasColumnName("trajanje_u_minutama");
            entity.Property(e => e.DatumVrijeme)
            .HasColumnName("datum_vrijeme")
            .HasColumnType("datetime");


            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .HasColumnName("naziv");

            entity.HasOne(d => d.IdKorisnikaNavigation).WithMany(p => p.Aktivnosts)
                .HasForeignKey(d => d.IdKorisnika)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aktivnost_korisnik");

            entity.HasOne(d => d.IdTipaAktivnostiNavigation).WithMany(p => p.Aktivnosts)
                .HasForeignKey(d => d.IdTipaAktivnosti)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aktivnost_tip_aktivnosti");
        });


        modelBuilder.Entity<Cilj>(entity =>
        {
            entity.HasKey(e => e.IdCilj).HasName("PRIMARY");

            entity.ToTable("cilj");

            entity.HasIndex(e => e.IdKorisnik, "fk_cilj_korisnik_idx");

            entity.Property(e => e.IdCilj).HasColumnName("id_cilj");
            entity.Property(e => e.DnevniLimitKalorija).HasColumnName("dnevni_limit_kalorija");
            entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");

            entity.HasOne(d => d.IdKorisnikNavigation).WithMany(p => p.Ciljs)
                .HasForeignKey(d => d.IdKorisnik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cilj_korisnik");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.HasKey(e => e.IdKorisnik).HasName("PRIMARY");

            entity.ToTable("korisnik");

            entity.HasIndex(e => e.IdRole, "fk_korisnik_rola_idx");

            entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .HasColumnName("ime");
            entity.Property(e => e.KorisnickoIme)
                .HasMaxLength(255)
                .HasColumnName("korisnicko_ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(255)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(100)
                .HasColumnName("prezime");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Korisniks)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_korisnik_rola");
        });

        modelBuilder.Entity<Obrok>(entity =>
        {
            entity.HasKey(e => e.IdObrok).HasName("PRIMARY");

            entity.ToTable("obrok");

            entity.HasIndex(e => e.IdKorisnik, "fk_obrok_korisnik_idx");

            entity.HasIndex(e => e.IdTipObroka, "id_tip_obroka_idx");

            entity.Property(e => e.IdObrok).HasColumnName("id_obrok");
            entity.Property(e => e.IdKorisnik).HasColumnName("id_korisnik");
            entity.Property(e => e.IdTipObroka).HasColumnName("id_tip_obroka");
            entity.Property(e => e.Kalorije).HasColumnName("kalorije");
            entity.Property(e => e.Masa).HasColumnName("masa");
            entity.Property(e => e.Masti).HasColumnName("masti");
            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .HasColumnName("naziv");
            entity.Property(e => e.Proteini).HasColumnName("proteini");
            entity.Property(e => e.Ugljenihidrati).HasColumnName("ugljenihidrati");
            entity.Property(e => e.DatumVrijeme)
            .HasColumnName("datum_vrijeme")
            .HasColumnType("datetime");

            entity.HasOne(d => d.IdKorisnikNavigation).WithMany(p => p.Obroks)
                .HasForeignKey(d => d.IdKorisnik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_obrok_korisnik");

            entity.HasOne(d => d.IdTipObrokaNavigation).WithMany(p => p.Obroks)
                .HasForeignKey(d => d.IdTipObroka)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_obrok_tip_obroka");
        });

        modelBuilder.Entity<Rola>(entity =>
        {
            entity.HasKey(e => e.IdRola).HasName("PRIMARY");

            entity.ToTable("rola");

            entity.Property(e => e.IdRola).HasColumnName("id_rola");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
        });

        modelBuilder.Entity<TipAktivnosti>(entity =>
        {
            entity.HasKey(e => e.IdTipAktivnosti).HasName("PRIMARY");

            entity.ToTable("tip_aktivnosti");

            entity.Property(e => e.IdTipAktivnosti).HasColumnName("id_tip_aktivnosti");
            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .HasColumnName("naziv");
            entity.Property(e => e.KalorijePoMinuti).HasColumnName("kalorije_po_minuti");

        });

        modelBuilder.Entity<TipObroka>(entity =>
        {
            entity.HasKey(e => e.IdTipObroka).HasName("PRIMARY");

            entity.ToTable("tip_obroka");

            entity.Property(e => e.IdTipObroka).HasColumnName("id_tip_obroka");
            entity.Property(e => e.Naziv)
                .HasMaxLength(100)
                .HasColumnName("naziv");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
