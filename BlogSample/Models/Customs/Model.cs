using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSampleApi.Models
{
    public class Model
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }
    }
}
