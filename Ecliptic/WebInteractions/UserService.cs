using Android.Util;
using Ecliptic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecliptic.WebInteractions
{
    class UserService
    { 
        const string Url = WebData.ADRESS + "/api/user/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем пользователя
        public async Task<User> Get(string login, string pass)
        {
            HttpClient client = GetClient();

            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(new { Login = login, Pass = pass }),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        private class WebUser
        {
            public string Name{get;set; }
            public string Login { get; set; }
            public string Pass { get; set; }
        }

        // добавляем пользоваетля
        public async Task<User> Register(string name, string login, string pass)
        {
            WebUser user = new WebUser { Name = name, Login = login, Pass = pass };

            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;    

            return JsonConvert.DeserializeObject<User>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
