using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.Administrator;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.Administration
{
    public class UserTransaction : TransactionBase
    {

        #region fields
 
        private const string _serviceRoute = "api/users/";
        private const string _saveUrl = $"{_serviceRoute}Save";
        private const string _getByIdUrl = $"{_serviceRoute}Get";
        private const string _getByNameUrl = $"{_serviceRoute}GetByName";
        private const string _getByEmailUrl = $"{_serviceRoute}GetByEmail";
        private const string _getUsersUrl = $"{_serviceRoute}GetUsers";
        private const string _updateUrl = $"{_serviceRoute}Update";
        private const string _changePasswordUrl = $"{_serviceRoute}ChangePassword";

        #endregion

        #region constructors

        public UserTransaction() { }

        #endregion

        #region public methods

        public bool SaveUser(User user)
        {
            bool isSuccess = false;

            try
            {
                if (String.IsNullOrEmpty(user.Name.Trim())) throw new Exception("User name is required");
                if (String.IsNullOrEmpty(user.Email.Trim())) throw new Exception("User email is required");
                if (user.Role == 0) throw new Exception("User role is required");
                if (String.IsNullOrEmpty(user.Password.Trim())) throw new Exception("User password is required");
                user.DateCreated = DateTime.Now;

                isSuccess = Task.Run(() => SaveUserTask(user)).GetAwaiter().GetResult();

            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        public User? GetUserById(int id)
        {
            User? user = new();

            try
            {
                if(id <= 0) throw new Exception("User id is required to get user data");
                user = Task.Run(() => GetUserByIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        public User? GetUserByName(string name)
        {
            User? user = new();

            try
            {
                if (string.IsNullOrEmpty(name)) throw new Exception("User name is required to get user by name");
                user = Task.Run(() => GetUserByNameTask(name)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        public User? GetUserByEmail(string email)
        {
            User? user = new();

            try
            {
                if (string.IsNullOrEmpty(email)) throw new Exception("User email is required to get user data");
                user = Task.Run(() => GetUserByEmailTask(email)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        public List<User>? GetUsers()
        {
            List<User>? users = new();

            try
            {
                users = Task.Run(() => GetUsersTask()).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return users;
        }

        public bool UpdateUser(User user)
        {
            bool isSuccess = false;

            try
            {
                if (user.Id <= 0) throw new Exception("User id is required for update");
                if (string.IsNullOrEmpty(user.Name.Trim())) throw new Exception("User name is required for update");
                if (string.IsNullOrEmpty(user.Email.Trim())) throw new Exception("User email is required for update");
                if (string.IsNullOrEmpty(user.Password.Trim())) throw new Exception("User password is required for update");
                user.DateUpdated = DateTime.Now;

                isSuccess = Task.Run(() => UpdateUserTask(user)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        public bool ChangePassword(int id, string password)
        {
            bool isSuccess = false;

            try
            {
                if (id <= 0) throw new Exception("User id is required to change password");
                if (string.IsNullOrEmpty(password.Trim())) throw new Exception("User password is required to change password");

                isSuccess = Task.Run(() => ChangeUserPasswordTask(id, password)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        #endregion

        #region private methods

        private async Task<bool> SaveUserTask(User user)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(user);
                    using(var response = await client.PostAsync(_saveUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        isSuccess = data.ToLower() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        private async Task<User?> GetUserByIdTask(int id)
        {
            User? user = new();

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        user = JsonSerializer.Deserialize<User>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        private async Task<User?> GetUserByNameTask(string name)
        {
            User? user = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByNameUrl, "name", name.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        user = JsonSerializer.Deserialize<User>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        private async Task<User?> GetUserByEmailTask(string email)
        {
            User? user = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByEmailUrl, "email", email);
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode) throw new Exception(data);
                        user = JsonSerializer.Deserialize<User>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return user;
        }

        private async Task<List<User>?> GetUsersTask()
        {
            List<User>? users = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    using(var response = await client.GetAsync(_getUsersUrl))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if(!response.IsSuccessStatusCode) throw new Exception(data);
                        users = JsonSerializer.Deserialize<List<User>>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return users;
        }

        private async Task<bool> UpdateUserTask(User user)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(user);
                    using(var response = await client.PutAsync(_updateUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        isSuccess = data.ToLower() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        private async Task<bool> ChangeUserPasswordTask(int id, string password)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    Dictionary<string, string?> queryParams = new()
                    {
                        { "id", id.ToString() },
                        { "password", password  }
                    };
                    string url = QueryHelpers.AddQueryString(_changePasswordUrl, queryParams);
                    HttpContent content = JsonContent.Create(new { id = id, password = password });
                    using(var response = await client.PutAsync(url, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        isSuccess = data.Trim().ToLower() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        #endregion

    }
}
