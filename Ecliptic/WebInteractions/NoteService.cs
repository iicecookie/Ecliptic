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
    class NoteService
    {
        const string Url = WebData.ADRESS + "/api/note/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все заметки пользователя
        public async Task<List<Note>> Get(int Userid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + "/" + Userid);
            return JsonConvert.DeserializeObject<List<Note>>(result);
        }

        // добавляем одну заметку
        public async Task<Note> Add(Note note)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
                await response.Content.ReadAsStringAsync());
        }

        // обновляем заметку
        public async Task<Note> Update(Note note)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "/" + note.Id,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
                await response.Content.ReadAsStringAsync());
        }

        // удаляем заметку
        public async Task<Note> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
               await response.Content.ReadAsStringAsync());
        }
    }
}
