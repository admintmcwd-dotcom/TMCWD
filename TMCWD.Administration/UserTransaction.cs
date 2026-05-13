using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using TMCWD.Model.Administrator;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;
using TMCWD.Utility.Encryption;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TMCWD.Administration
{
    public class UserTransaction : TransactionBase
    {

        #region fields
 
        //private const string _serviceRoute = "api/users/";
        //private const string _saveUrl = $"{_serviceRoute}SaveUser";
        //private const string _getByIdUrl = $"{_serviceRoute}GetById";
        //private const string _getByNameUrl = $"{_serviceRoute}GetByName";
        //private const string _getByEmailUrl = $"{_serviceRoute}GetByEmail";
        //private const string _getUsersUrl = $"{_serviceRoute}GetUsers";
        //private const string _updateUrl = $"{_serviceRoute}UpdateUser";
        //private const string _changePasswordUrl = $"{_serviceRoute}ChangePassword";
        //private const string _searchUsersUrl = $"{_serviceRoute}SearchUser";

        private readonly HttpClient _client;

        #endregion

        #region constructors

        public UserTransaction(HttpClient client) 
        {
            _client = client;
        }

        #endregion

        #region public methods

        public User ConvertJsonStringToUser(string jsonString)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<User>(jsonString, serializerOptions) ?? new User();
        }

        public List<User> ConvertJsonStringToUsers(string jsonString)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<User>>(jsonString, serializerOptions) ?? new List<User>();
        }

        public async Task<User?> SaveUpdate(int userId, User user)
        {
            var content = JsonContent.Create(user);
            var response = await _client.PostAsync($"api/Users/SaveUpdate/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonStringToUser(data);
        }

        public async Task<User?> Get(int id)
        {
            var response = await _client.GetAsync($"api/Users/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonStringToUser(data);
        }

        public async Task<User?> GetByName(string name)
        {
            var response = await _client.GetAsync($"api/Users/GetByName/{name}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonStringToUser(data);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var response = await _client.GetAsync($"api/Users/GetByEmail/{email}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonStringToUser(data);
        }

        public async Task<List<User>?> GetUsers()
        {

            var response = await _client.GetAsync("api/Users/GetUsers");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            List<User>? users = new();

            users = this.ConvertJsonStringToUsers(data);

            return users;
        }

        public async Task<bool> ChangePassword(int userId, int id, string currentPassword, string newPassword, string confirmPassword)
        {
            var user = await this.Get(id);

            currentPassword = StringEncyption.Encrypt(currentPassword);
            newPassword = StringEncyption.Encrypt(newPassword);
            confirmPassword = StringEncyption.Encrypt(confirmPassword);

            if (user == null) throw new Exception("Change password failed, user not found");

            if (user.Password != currentPassword) throw new Exception("Current password mismatch");
            if (newPassword != confirmPassword) throw new Exception("New password confirmation failed");

            user.Password = newPassword;

            user = await this.SaveUpdate(userId, user);


            return user != null && user.Id > 0;
        }

        public List<SelectListItem> GetRoles()
        {
            return new List<SelectListItem>()
            {
                { new SelectListItem(nameof(UserRole.Engineer), ((int)UserRole.Engineer).ToString()) },
                { new SelectListItem(nameof(UserRole.SuperAdmin), ((int)UserRole.SuperAdmin).ToString()) },
                { new SelectListItem(nameof(UserRole.Guest), ((int)UserRole.Guest).ToString()) },
                { new SelectListItem(nameof(UserRole.CustomerRepresentative), ((int)UserRole.CustomerRepresentative).ToString()) }
            };
        }

        public async Task<List<User>?> SearchUser(string searchString)
        {

            var response = await _client.GetAsync($"api/Users/SearchUser/{searchString}");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            List<User>? users = new();
            users = this.ConvertJsonStringToUsers(data);

            return users;
        }

        #endregion

        //#region private methods

        //private async Task<bool> SaveUserTask(User user)
        //{
        //    bool isSuccess = false;

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            //string jsonPayload = JsonSerializer.Serialize(user);
        //            HttpContent content = JsonContent.Create(user);
        //            //var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        //            using(var response = await client.PostAsync(_saveUrl, content))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                isSuccess = data.ToLower() == "true";
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return isSuccess;
        //}

        //private async Task<User?> GetUserByIdTask(int id)
        //{
        //    User? user = new();

        //    try
        //    {
        //        using (HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        //                user = JsonSerializer.Deserialize<User>(data, serializerOptions);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return user;
        //}

        //private async Task<User?> GetUserByNameTask(string name)
        //{
        //    User? user = new();

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_getByNameUrl, "name", name.ToString());
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                user = JsonSerializer.Deserialize<User>(data);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return user;
        //}

        //private async Task<User?> GetUserByEmailTask(string email)
        //{
        //    User? user = new();

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_getByEmailUrl, "email", email);
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (response.IsSuccessStatusCode) throw new Exception(data);
        //                user = JsonSerializer.Deserialize<User>(data);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return user;
        //}

        //private async Task<List<User>?> GetUsersTask()
        //{
        //    List<User>? users = new();

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            using(var response = await client.GetAsync(_getUsersUrl))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if(!response.IsSuccessStatusCode) throw new Exception(data);
        //                var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        //                users = JsonSerializer.Deserialize<List<User>>(data, serializerOptions);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return users;
        //}

        //private async Task<bool> UpdateUserTask(User user)
        //{
        //    bool isSuccess = false;

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            HttpContent content = JsonContent.Create(user);
        //            using(var response = await client.PutAsync(_updateUrl, content))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                isSuccess = data.ToLower() == "true";
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return isSuccess;
        //}

        //private async Task<bool> ChangeUserPasswordTask(int id, string password)
        //{
        //    bool isSuccess = false;

        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            Dictionary<string, string?> queryParams = new()
        //            {
        //                { "id", id.ToString() },
        //                { "password", password  }
        //            };
        //            string url = QueryHelpers.AddQueryString(_changePasswordUrl, queryParams);
        //            HttpContent content = JsonContent.Create(new { id = id, password = password });
        //            using(var response = await client.PutAsync(url, content))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                isSuccess = data.Trim().ToLower() == "true";
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }

        //    return isSuccess;
        //}

        //private async Task<List<User>?> SearchUserTask(string searchString)
        //{
        //    try
        //    {
        //        using(HttpClient client = new())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_searchUsersUrl, "searchString", searchString);
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        //                var serialized = JsonSerializer.Deserialize<List<User>>(data, serializerOptions);
        //                if (serialized != null) return serialized;
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
        //    }
        //    return null;
        //}

        //#endregion

    }
}
