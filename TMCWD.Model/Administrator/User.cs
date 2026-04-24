using System;
using System.Collections.Generic;
using System.Text;
using TMCWD.Model.Administrator.Interface;

namespace TMCWD.Model.Administrator
{
    public class User : IUser
    {
        public decimal Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Role { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateVerified { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RememberToken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateCreated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateUpdated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsVerified { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
