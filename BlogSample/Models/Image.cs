using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogSampleApi.Models
{
    [Table("image")]
    [Index("Filepath", Name = "UQ__image__DFE356BE398CD2D8", IsUnique = true)]
    public partial class Image
    {
        public Image()
        {
            IdArticles = new HashSet<Article>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("filepath")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Filepath { get; set; }
        [Column("titre")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Titre { get; set; }
        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [ForeignKey("IdImage")]
        [InverseProperty("IdImages")]
        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
