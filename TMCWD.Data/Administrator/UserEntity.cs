using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text;
using TMCWD.Data.Utilities;
using TMCWD.Model.Administrator;

namespace TMCWD.Data.Administrator
{
    public class UserEntity
    {

        #region constructors
        public UserEntity() { }
        #endregion

        #region properties
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required, MaxLength(50)]
        public string Remember_Token { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Updated_At { get; set; }

        #endregion

    }
}
