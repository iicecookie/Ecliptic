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
    class FavRoomService
    {
        const string Url = "http://192.168.1.18:3000/api/favroom/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все заметки пользователя
        public async Task<List<FavRoom>> Get(int Userid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "/" + Userid);
            return JsonConvert.DeserializeObject<List<FavRoom>>(result);
        }

        // добавляем одну заметку
        public async Task<FavRoom> Add(FavRoom note)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FavRoom>(
                await response.Content.ReadAsStringAsync());
        }

        // удаляем заметку
        public async Task<FavRoom> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FavRoom>(
               await response.Content.ReadAsStringAsync());
        }
    }
}
