namespace ApiRestExpress.Models
{
    public partial class Tag : ModelBase
    {
        public Tag()
        {
            IdArticles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string? Mot { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
