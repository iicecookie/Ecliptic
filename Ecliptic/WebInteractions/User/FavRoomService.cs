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
        const string Url = WebData.ADRESS + "/api/FavRooms";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все избраные помещения
        public async Task<List<FavoriteRoom>> Get(int userid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "/" + userid);
            return JsonConvert.DeserializeObject<List<FavoriteRoom>>(result);
        }

        // добавляем одного друга
        public async Task<FavoriteRoom> Add(FavoriteRoom note)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FavoriteRoom>(
                await response.Content.ReadAsStringAsync());
        }

        // обновляем заметку
        public async Task<FavoriteRoom> Update(FavoriteRoom note)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "/" + note.FavoriteRoomId,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FavoriteRoom>(
                await response.Content.ReadAsStringAsync());
        }

        // удаляем заметку
        public async Task<FavoriteRoom> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FavoriteRoom>(
               await response.Content.ReadAsStringAsync());
        }
    }
}
