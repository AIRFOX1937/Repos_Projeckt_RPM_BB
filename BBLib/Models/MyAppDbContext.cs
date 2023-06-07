using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BBLib.Models;

public partial class MyAppDbContext : DbContext
{
    public MyAppDbContext()
    {
    }

    private static MyAppDbContext _context = new MyAppDbContext();

    public static MyAppDbContext GetContext()
    {
        if (_context == null)
            _context = new MyAppDbContext();
        return _context;
    }

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chet> Chets { get; set; }

    public virtual DbSet<Disc> Discs { get; set; }

    public virtual DbSet<Doki> Dokis { get; set; }

    public virtual DbSet<GD> GDs { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Nedeli> Nedelis { get; set; }

    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<UserRoleGroupcs> UserRoleGroupcs { get; set; }

    public virtual DbSet<Otcheti> Otchetis { get; set; }

    public virtual DbSet<Raspi> Raspis { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    public virtual DbSet<UD> UDs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vidi> Vidis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=bbkai;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chet>(entity =>
        {
            entity.HasKey(e => e.IdC).HasName("PK__Chet__9DB7D2F690E0E813");

            entity.ToTable("Chet");

            entity.Property(e => e.IdC).HasColumnName("id_c");
            entity.Property(e => e.NameC)
                .HasMaxLength(10)
                .HasColumnName("name_c");
        });

        modelBuilder.Entity<Disc>(entity =>
        {
            entity.HasKey(e => e.IdD).HasName("PK__Discs__9DB7D2F9A7A706D7");

            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.NameD)
                .HasMaxLength(50)
                .HasColumnName("name_d");
        });

        modelBuilder.Entity<Doki>(entity =>
        {
            entity.HasKey(e => e.IdD).HasName("PK__Doki__9DB7D2F9AA30BF70");

            entity.ToTable("Doki");

            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.FlagD).HasColumnName("flag_d");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.IdV).HasColumnName("id_v");
            entity.Property(e => e.IdDi).HasColumnName("id_di");
            entity.Property(e => e.NameD)
                .HasMaxLength(50)
                .HasColumnName("name_d");
            entity.Property(e => e.SsilkaD).HasColumnName("ssilka_d");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Dokis)
                .HasForeignKey(d => d.IdU)
                .HasConstraintName("FK__Doki__id_u__5EBF139D");

            entity.HasOne(d => d.IdVNavigation).WithMany(p => p.Dokis)
                .HasForeignKey(d => d.IdV)
                .HasConstraintName("FK__Doki__id_v__5FB337D6");
            entity.HasOne(d => d.IdDiNavigation).WithMany(p => p.Dokis)
                .HasForeignKey(d => d.IdDi)
                .HasConstraintName("FK__Doki__id_di__6E01572D");
        });

        modelBuilder.Entity<GD>(entity =>
        {
            entity.HasKey(e => e.IdGD).HasName("PK__G_D__D795FE42681CEB84");

            entity.ToTable("G_D");

            entity.Property(e => e.IdGD).HasColumnName("id_g_d");
            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.IdG).HasColumnName("id_g");

            entity.HasOne(d => d.IdDNavigation).WithMany(p => p.GDs)
                .HasForeignKey(d => d.IdD)
                .HasConstraintName("FK__G_D__id_d__3B75D760");

            entity.HasOne(d => d.IdGNavigation).WithMany(p => p.GDs)
                .HasForeignKey(d => d.IdG)
                .HasConstraintName("FK__G_D__id_g__3A81B327");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.IdG).HasName("PK__Groups__9DB7D2FA05BA07E1");

            entity.Property(e => e.IdG).HasColumnName("id_g");
            entity.Property(e => e.NumG).HasColumnName("num_g");
        });

        modelBuilder.Entity<Nedeli>(entity =>
        {
            entity.HasKey(e => e.IdN).HasName("PK__Nedeli__9DB7D2E349A1AFF1");

            entity.ToTable("Nedeli");

            entity.Property(e => e.IdN).HasColumnName("id_n");
            entity.Property(e => e.NameN)
                .HasMaxLength(15)
                .HasColumnName("name_n");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3213E83F43B85B28");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateN)
                .HasColumnType("date")
                .HasColumnName("date_n");
            entity.Property(e => e.Img).HasColumnName("img");
            entity.Property(e => e.Txt).HasColumnName("txt");
            entity.Property(e => e.Txt1).HasColumnName("txt1");
            entity.Property(e => e.Zag).HasColumnName("zag");
        });

        modelBuilder.Entity<Otcheti>(entity =>
        {
            entity.HasKey(e => e.IdO).HasName("PK__Otcheti__9DB7D2E20C7AEDA8");

            entity.ToTable("Otcheti");

            entity.Property(e => e.IdO).HasColumnName("id_o");
            entity.Property(e => e.DateO)
                .HasColumnType("datetime")
                .HasColumnName("date_o");
            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.Ssilka).HasColumnName("ssilka");

            entity.HasOne(d => d.IdDNavigation).WithMany(p => p.Otchetis)
                .HasForeignKey(d => d.IdD)
                .HasConstraintName("FK__Otcheti__id_d__60A75C0F");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Otchetis)
                .HasForeignKey(d => d.IdU)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Otcheti__id_u__6383C8BA");
        });

        modelBuilder.Entity<Raspi>(entity =>
        {
            entity.HasKey(e => e.IdR).HasName("PK__Raspis__9DB7D2E74929E5F5");

            entity.Property(e => e.IdR).HasColumnName("id_r");
            entity.Property(e => e.AudR)
                .HasMaxLength(20)
                .HasColumnName("aud_r");
            entity.Property(e => e.DateR)
                .HasMaxLength(100)
                .HasColumnName("date_r");
            entity.Property(e => e.IdC).HasColumnName("id_c");
            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.IdG).HasColumnName("id_g");
            entity.Property(e => e.IdN).HasColumnName("id_n");
            entity.Property(e => e.IdT).HasColumnName("id_t");
            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.IdV).HasColumnName("id_v");
            entity.Property(e => e.ZdR).HasColumnName("zd_r");

            entity.HasOne(d => d.IdCNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdC)
                .HasConstraintName("FK__Raspis__id_c__3F466844");

            entity.HasOne(d => d.IdDNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdD)
                .HasConstraintName("FK__Raspis__id_d__4222D4EF");

            entity.HasOne(d => d.IdGNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdG)
                .HasConstraintName("FK__Raspis__id_g__3E52440B");

            entity.HasOne(d => d.IdNNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdN)
                .HasConstraintName("FK__Raspis__id_n__403A8C7D");

            entity.HasOne(d => d.IdTNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdT)
                .HasConstraintName("FK__Raspis__id_t__412EB0B6");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdU)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Raspis__id_u__440B1D61");

            entity.HasOne(d => d.IdVNavigation).WithMany(p => p.Raspis)
                .HasForeignKey(d => d.IdV)
                .HasConstraintName("FK__Raspis__id_v__4316F928");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdR).HasName("PK__Roles__9DB7D2E72BD7D94A");

            entity.Property(e => e.IdR).HasColumnName("id_r");
            entity.Property(e => e.NameR)
                .HasMaxLength(20)
                .HasColumnName("name_r");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.HasKey(e => e.IdT).HasName("PK__Times__9DB7D2E9B7A6811E");

            entity.Property(e => e.IdT).HasColumnName("id_t");
            entity.Property(e => e.NameT)
                .HasMaxLength(5)
                .HasColumnName("name_t");
        });

        modelBuilder.Entity<UD>(entity =>
        {
            entity.HasKey(e => e.IdUD).HasName("PK__U_D__6AE9E4495D65581D");

            entity.ToTable("U_D");

            entity.Property(e => e.IdUD).HasColumnName("id_u_d");
            entity.Property(e => e.IdD).HasColumnName("id_d");
            entity.Property(e => e.IdU).HasColumnName("id_u");

            entity.HasOne(d => d.IdDNavigation).WithMany(p => p.UDs)
                .HasForeignKey(d => d.IdD)
                .HasConstraintName("FK__U_D__id_d__3D5E1FD2");

            entity.HasOne(d => d.IdUNavigation).WithMany(p => p.UDs)
                .HasForeignKey(d => d.IdU)
                .HasConstraintName("FK__U_D__id_u__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdU).HasName("PK__Users__9DB7D2E8C95825E1");

            entity.HasIndex(e => e.LoginU, "UQ__Users__1F5D42AE2221B50A").IsUnique();

            entity.Property(e => e.IdU).HasColumnName("id_u");
            entity.Property(e => e.FioU)
                .HasMaxLength(300)
                .HasColumnName("fio_u");
            entity.Property(e => e.GroupS).HasColumnName("group_s");
            entity.Property(e => e.LoginU)
                .HasMaxLength(50)
                .HasColumnName("login_u");
            entity.Property(e => e.PassU)
                .HasMaxLength(50)
                .HasColumnName("pass_u");
            entity.Property(e => e.RoleU).HasColumnName("role_u");

            entity.HasOne(d => d.GroupSNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.GroupS)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__group_s__398D8EEE");

            entity.HasOne(d => d.RoleUNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleU)
                .HasConstraintName("FK__Users__role_u__38996AB5");
        });

        modelBuilder.Entity<UserRoleGroupcs>().HasNoKey();

        modelBuilder.Entity<Vidi>(entity =>
        {
            entity.HasKey(e => e.IdV).HasName("PK__Vidi__9DB7D2EB205063F4");

            entity.ToTable("Vidi");

            entity.Property(e => e.IdV).HasColumnName("id_v");
            entity.Property(e => e.NameV)
                .HasMaxLength(10)
                .HasColumnName("name_v");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
