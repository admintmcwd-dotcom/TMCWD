using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace TMCWD.Data.Entities
{
    public class Users
    {
        #region constructor
        public Users() { }
        #endregion

        #region properties
        public decimal Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public int Role{ get; set; }

        public DateTime DateVerified { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string RememberToken { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
