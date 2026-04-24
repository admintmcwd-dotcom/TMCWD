using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using TMCWD.Utility.Generic;
using TMCWD.Model.Administrator;

namespace TMCWD.Administration
{
    public class ApplicationLogin
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the ApplicationLogin class.`
        /// </summary>
        public ApplicationLogin() 
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public ApplicationLogin(string email, string password)
        {
            Email = email;
            Password = password;
        }

        #endregion

        #region properties

        public string Email { get; set; }
        public string Password { get; set; }

        #endregion

        #region methods

        public bool Login()
        {
            bool isSuccess = false;

            var user = Task.Run(() => LoginTask()).GetAwaiter().GetResult();

            if (user.Password.Equals(this.Password)) isSuccess = true;

            return isSuccess;
        }

        #endregion

        #region private methods
        private async Task<User> LoginTask()
        {

            try
            {
                if (String.IsNullOrEmpty(this.Email.Trim()) || String.IsNullOrEmpty(this.Password.Trim()))
                {
                    throw new ArgumentException("Email and Password cannot be empty.");
                }

                var loginData = new { Email = this.Email };
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5178/Users");
                var response = await client.PostAsJsonAsync("/GetByEmail", loginData);
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException("Login failed. Please check your credentials and try again.");

                var userData = await response.Content.ReadFromJsonAsync<User>();
                if (userData == null)
                {
                    throw new ArgumentException("Login failed. User not found. Please check your credentials and try again.");
                }

                return userData;

            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Application, ErrorType.Error, ex.Message);
            }

            return new User();
        }

        #endregion
    }
}
