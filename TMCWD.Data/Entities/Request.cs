using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("requests")]
    public class Request
    {

        #region constructors

        public Request() { }

        #endregion

        #region properties

        [Key, Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, MaxLength(50), Column("ControlNumber")]
        public string ControlNumber { get; set; } = string.Empty;

        [Required, Column("CustomerId")]
        public int CustomerId { get; set; }

        [Required, Column("AccountId")]
        public int AccountId { get; set; }

        [Required, Column("UserId")]
        public int UserId { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        #endregion

    }
}
