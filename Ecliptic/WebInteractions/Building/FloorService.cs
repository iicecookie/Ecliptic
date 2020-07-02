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

        // получаем все этажи выбраного здания
        public async Task<List<Floor>> GetFloors(int buildingid)
        {
            HttpClient client = WebData.GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Floor>>(result);
        }
    }
}
