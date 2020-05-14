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
    class FloorService
    {
        const string Url = WebData.ADRESS + "/api/Floors/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем этажи здания
        public async Task<List<Floor>> GetFloorsbyBuilding(int buildingid)
        {
            HttpClient client = GetClient();
            HttpResponseMessage response = await client.PostAsync(Url,
                                new StringContent(
                                    JsonConvert.SerializeObject(buildingid),
                                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<List<Floor>>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
