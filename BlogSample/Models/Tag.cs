using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSampleApi.Models
{
    [Table("tag")]
    [Index("Mot", Name = "UQ__tag__DF50CE3C97B5E2DE", IsUnique = true)]
    public partial class Tag : Model
    {
        public Tag()
        {
            IdArticles = new HashSet<Article>();
        }

        //[Key]
        //[Column("id")]
        //public int Id { get; set; }
        [Column("mot")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Mot { get; set; }
        //[Column("is_deleted")]
        //public bool? IsDeleted { get; set; }

        [ForeignKey("IdTag")]
        [InverseProperty("IdTags")]
        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
