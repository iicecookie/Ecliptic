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
        const string Url = WebData.ADRESS + "api/Floors/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все этажи здания
        public async Task<List<Floor>> GetFloors(int buildingid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Floor>>(result);
        }
    }
}
