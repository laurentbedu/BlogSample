using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSampleApi.Models
{
    [Table("app_user")]
    [Index("Login", Name = "UQ__app_user__7838F272976609B7", IsUnique = true)]
    [Index("IdAuteur", Name = "UQ__app_user__DEA78B8B69F8332F", IsUnique = true)]
    public partial class AppUser : Model
    {
        //[Key]
        //[Column("id")]
        //public int Id { get; set; }
        [Column("login")]
        [StringLength(255)]
        [Unicode(false)]
        public string Login { get; set; } = null!;
        [Column("password")]
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        //[Column("is_deleted")]
        //public bool? IsDeleted { get; set; }
        [Column("Id_auteur")]
        public int? IdAuteur { get; set; }

        [ForeignKey("IdAuteur")]
        [InverseProperty("AppUser")]
        public virtual Auteur? IdAuteurNavigation { get; set; }
    }
}
