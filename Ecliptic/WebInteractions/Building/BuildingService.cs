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

        // получаем список доступных к загрузке зданий
        public async Task<List<Building>> GetBuildings()
        {
            HttpClient client = WebData.GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<List<Building>>(result);
        }
    }
}
