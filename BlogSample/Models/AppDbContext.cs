using Microsoft.EntityFrameworkCore;


namespace BlogSampleApi.Models
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
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalDb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdAuteurNavigation)
                    .WithOne(p => p.AppUser)
                    .HasForeignKey<AppUser>(d => d.IdAuteur)
                    .HasConstraintName("FK__app_user__Id_aut__32E0915F");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

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
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
