using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMCWD.Data.Entities
{
    [Table("users")]
    public class User
    {
        #region constructor
        public User() { }
        #endregion

        #region properties
        [Key]
        public System.Int64 Id { get; set; }

        [Column("Name")]
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Column("Email")]
        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Column("Role")]
        [Required]
        public int Role { get; set; }

        [Column("DateVerified")]
        public DateTime DateVerified { get; set; }

        [Column("Password")]
        [Required, MaxLength(20)]
        public string Password { get; set; }

        [Column("RememberToken")]
        [MaxLength(100)]
        public string RememberToken { get; set; }

        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("DateUpdated")]
        public DateTime DateUpdated { get; set; }

        [Column("IsVerified")]
        public bool IsVerified { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }
        #endregion
    }
}
