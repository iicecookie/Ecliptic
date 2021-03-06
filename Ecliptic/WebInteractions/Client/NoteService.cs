﻿using Ecliptic.Models;
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
        const string Url = WebData.ADRESS + "api/Notes";

        // получаем все публичные заметки о здании
        public async Task<List<Note>> GetPublic(int buildingid) // переделать
        {
            HttpClient client = WebData.GetClient();
            string result = await client.GetStringAsync(Url + "/" + buildingid);
            return JsonConvert.DeserializeObject<List<Note>>(result);
        }

        // получить все заметки выбраного клиента
        public async Task<List<Note>> GetClient(int id)
        {
            Dictionary<string, int> req = new Dictionary<string, int>();
            req.Add("id", id);

            HttpClient client = WebData.GetClient();
            var response = await client.PostAsync(Url + "/PostClientNote",
                new StringContent(
                    JsonConvert.SerializeObject(req),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<List<Note>>(
                await response.Content.ReadAsStringAsync());
        }

        // сохранить новую заметку
        public async Task<Note> Add(Note note)
        {
            HttpClient client = WebData.GetClient();
            var response = await client.PostAsync(Url + "/PostAddNote",
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
                await response.Content.ReadAsStringAsync());
        }

        // обновить заметку
        public async Task<Note> Update(Note note)
        {
            note.Client = null;
            HttpClient client = WebData.GetClient();
            var response = await client.PutAsync(Url + "/" + note.NoteId,
                new StringContent(
                    JsonConvert.SerializeObject(note),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
                await response.Content.ReadAsStringAsync());
        }

        // удалить заметку
        public async Task<Note> Delete(int id)
        {
            HttpClient client = WebData.GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Note>(
               await response.Content.ReadAsStringAsync());
        }
    }
}
