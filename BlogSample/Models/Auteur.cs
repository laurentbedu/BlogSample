using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlogSampleApi.Models
{
    [Table("auteur")]
    [Index("Pseudo", Name = "UQ__auteur__EA0EEA227B42EBF4", IsUnique = true)]
    public partial class Auteur
    {
        public Auteur()
        {
            Articles = new HashSet<Article>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nom")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Nom { get; set; }
        [Column("prenom")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Prenom { get; set; }
        [Column("pseudo")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Pseudo { get; set; }
        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [InverseProperty("IdAuteurNavigation")]
        public virtual AppUser? AppUser { get; set; }
        [InverseProperty("IdAuteurNavigation")]
        public virtual ICollection<Article> Articles { get; set; }
    }
}
