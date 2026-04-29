using System;
using System.Net.Http.Json;
using TMCWD.Utility.Generic;
using TMCWD.Utility.Encryption;
using TMCWD.Model.Administrator;
using TMCWD.Model.Interfaces;
using Microsoft.AspNetCore.WebUtilities;


namespace TMCWD.Administration
{
    public class ApplicationLoginTransaction : TransactionBase
    {

        #region constructors

        /// <summary>
        /// Initializes a new instance of the ApplicationLogin class.`
        /// </summary>
        public ApplicationLoginTransaction() 
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public ApplicationLoginTransaction(string email, string password)
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

            if(String.IsNullOrEmpty(this.Email.Trim()) || String.IsNullOrEmpty(this.Password.Trim()))
                return isSuccess;

            var user = Task.Run(() => LoginTask()).GetAwaiter().GetResult();

            if (user.Password.Equals(StringEncyption.Encrypt(this.Password))) isSuccess = true;

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

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);

                    string requestUrl = QueryHelpers.AddQueryString("api/Users/GetByEmail", "email", this.Email.Trim());
                    using (var response = await client.GetAsync(requestUrl))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new ArgumentException("Login failed. Please check your credentials and try again.");

                        var userData = await response.Content.ReadFromJsonAsync<User>();
                        if (userData == null)
                        {
                            throw new ArgumentException("Login failed. User not found. Please check your credentials and try again.");
                        }

                        return userData;
                    }
                }

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
