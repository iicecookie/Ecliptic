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
    class RoomService
    {
        const string Url = WebData.ADRESS + "api/Rooms/";

        // получаем все помещения выбраного здания
        public async Task<List<Room>> GetRooms(int buildingid)
        {
            HttpClient client = WebData.GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Room>>(result);
        }
    }
}
