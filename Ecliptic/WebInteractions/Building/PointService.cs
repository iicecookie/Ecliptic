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
    class PointService
    {
        const string Url = WebData.ADRESS + "api/PointMs/";

        // получаем все точки выбраного здания
        public async Task<List<PointM>> GetPoints(int buildingid)
        {
            HttpClient client = WebData.GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<PointM>>(result);
        }
    }
}
