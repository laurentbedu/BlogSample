namespace ApiRestExpress.Models
{
    public partial class Image : ModelBase
    {
        public Image()
        {
            IdArticles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string? Filepath { get; set; }
        public string? Titre { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Article> IdArticles { get; set; }
    }
}
