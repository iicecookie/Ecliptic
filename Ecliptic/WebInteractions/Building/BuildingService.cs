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
    class BuildingService
    {
        const string Url = WebData.ADRESS + "/api/Buildings/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все здания системы
        public async Task<List<Building>> Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<List<Building>>(result);
        }

        // получаем все здания системы
        public async Task<List<Building>> Get(int buildingid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Building>>(result);
        }
    }
}
