using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSampleApi.Models
{
    [Table("article")]
    public partial class Article : Model
    {
        public Article()
        {
            IdImages = new HashSet<Image>();
            IdTags = new HashSet<Tag>();
        }

        //[Key]
        //[Column("id")]
        //public int Id { get; set; }
        [Column("titre")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Titre { get; set; }
        [Column("texte")]
        [Unicode(false)]
        public string? Texte { get; set; }
        [Column("published_date")]
        public DateTime? PublishedDate { get; set; }
        //[Column("is_deleted")]
        //public bool? IsDeleted { get; set; }
        [Column("Id_auteur")]
        public int? IdAuteur { get; set; }

        [ForeignKey("IdAuteur")]
        [InverseProperty("Articles")]
        public virtual Auteur? IdAuteurNavigation { get; set; }

        [ForeignKey("IdArticle")]
        [InverseProperty("IdArticles")]
        public virtual ICollection<Image> IdImages { get; set; }
        [ForeignKey("IdArticle")]
        [InverseProperty("IdArticles")]
        public virtual ICollection<Tag> IdTags { get; set; }
    }
}
