using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{

    [Table("customers")]
    public class Customer
    {

        [Key]
        public System.Int64 Id { get; set; }

        [Required, MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Middlename { get; set; }

        [Required, MaxLength(50)]
        public string Lastname { get; set; }

        [Required, MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
