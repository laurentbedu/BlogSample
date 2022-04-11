namespace ApiRestExpress.Models
{
    public partial class Auteur : ModelBase
    {
        public Auteur()
        {
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Pseudo { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
