using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{
    [Table("recommendations")]
    public class Recommendation
    {
        [Key, Column("Id")]
        public System.Int64 Id { get; set; }
        [Required, Column("RequestId")]
        public System.Int64 RequestId { get; set; }
        [Required, Column("RecommendationUserId")]
        public System.Int64 RecommendationUserId { get; set; }
        [Required, MaxLength(255), Column("Detail")]
        public string Detail { get; set; } = string.Empty;
        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }
        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }
    }
}
