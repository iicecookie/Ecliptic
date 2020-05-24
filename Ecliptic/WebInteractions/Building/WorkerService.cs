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
    class WorkerService
    {
        const string Url = WebData.ADRESS + "api/Workers/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем всех работников здания
        public async Task<List<Worker>> GetWorkers(int buildingid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Worker>>(result);
        }
    }
}
