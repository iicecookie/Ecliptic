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
        const string Url = WebData.ADRESS + "/api/FavoriteRooms";

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<List<FavoriteRoom>> Get(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "/" + id);
            return JsonConvert.DeserializeObject<List<FavoriteRoom>>(result);
        }

        public async Task<FavoriteRoom> Add(FavoriteRoom room)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(room),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.Created)
                return null;

            return JsonConvert.DeserializeObject<FavoriteRoom>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);

            if (response.StatusCode == HttpStatusCode.OK)
                return true;

            return false;

            // return JsonConvert.DeserializeObject<FavoriteRoom>(
            //   await response.Content.ReadAsStringAsync());  
        }
    }
}
