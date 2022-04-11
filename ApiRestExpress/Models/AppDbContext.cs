using Microsoft.EntityFrameworkCore;

namespace ApiRestExpress.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Auteur> Auteurs { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\;Database=BlogDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("app_user");

                entity.HasIndex(e => e.Login, "UQ__app_user__7838F272976609B7")
                    .IsUnique();

                entity.HasIndex(e => e.IdAuteur, "UQ__app_user__DEA78B8B69F8332F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAuteur).HasColumnName("Id_auteur");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Login)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.HasOne(d => d.IdAuteurNavigation)
                    .WithOne(p => p.AppUser)
                    .HasForeignKey<AppUser>(d => d.IdAuteur)
                    .HasConstraintName("FK__app_user__Id_aut__32E0915F");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAuteur).HasColumnName("Id_auteur");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Texte)
                    .IsUnicode(false)
                    .HasColumnName("texte");

                entity.Property(e => e.Titre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titre");

                entity.HasOne(d => d.IdAuteurNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.IdAuteur)
                    .HasConstraintName("FK__article__Id_aute__29572725");

                entity.HasMany(d => d.IdImages)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleImage",
                        l => l.HasOne<Image>().WithMany().HasForeignKey("IdImage").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_im__3D5E1FD2"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_i__Id_ar__3C69FB99"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdImage").HasName("PK__article___F2BDA3B6667060A2");

                            j.ToTable("article_image");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdImage").HasColumnName("Id_image");
                        });

                entity.HasMany(d => d.IdTags)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("IdTag").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ta__398D8EEE"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__article_t__Id_ar__38996AB5"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdTag").HasName("PK__article___9188955C277B3A8A");

                            j.ToTable("article_tag");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("Id_article");

                            j.IndexerProperty<int>("IdTag").HasColumnName("Id_tag");
                        });
            });

            modelBuilder.Entity<Auteur>(entity =>
            {
                entity.ToTable("auteur");

                entity.HasIndex(e => e.Pseudo, "UQ__auteur__EA0EEA227B42EBF4")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Nom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("prenom");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pseudo");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.HasIndex(e => e.Filepath, "UQ__image__DFE356BE398CD2D8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Filepath)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("filepath");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Titre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("titre");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.HasIndex(e => e.Mot, "UQ__tag__DF50CE3C97B5E2DE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Mot)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("mot");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
