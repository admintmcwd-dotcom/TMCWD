using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.Administrator.Interface;

namespace TMCWD.Model.Administrator
{
    public class User : IUser
    {
        #region constructor
        /// <summary>
        /// Initializes a new instance of the User class with default property values.
        /// </summary>
        public User()
        {
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Role = 0;
            this.Password = string.Empty;
            this.RememberToken = string.Empty;
        }
        #endregion

        #region properties
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public DateTime DateVerified { get; set; }
        public string Password { get; set; }
        public string RememberToken { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
