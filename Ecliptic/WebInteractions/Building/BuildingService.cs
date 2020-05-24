using Ecliptic.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ecliptic.WebInteractions
{
    public class BuildingService
    {
        const string Url = WebData.ADRESS + "api/Buildings/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все здания системы
        public async Task<List<Building>> GetBuildings()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<List<Building>>(result);
        }
    }
}
