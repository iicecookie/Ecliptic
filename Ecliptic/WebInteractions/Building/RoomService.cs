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
        const string Url = WebData.ADRESS + "/api/Rooms/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все помещения системы
        public async Task<List<Room>> GetRooms(int buildingid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + buildingid);
            return JsonConvert.DeserializeObject<List<Room>>(result);
        }
    }
}
