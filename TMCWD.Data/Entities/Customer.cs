using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("customers")]
    public class Customer
    {

        [Key]
        [Column("Id")]
        public System.Int64 Id { get; set; }

        [Required, MaxLength(50), Column("Firstname")]
        public string Firstname { get; set; }

        [MaxLength(50), Column("Middlename")]
        public string Middlename { get; set; }

        [Required, MaxLength(50), Column("Lastname")]
        public string Lastname { get; set; }

        [Required, MaxLength(11), Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100), Column("Email")]
        public string Email { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }
    }
}
