using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public struct Note
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string Date { get; private set; }

        public string Room { get; private set; }

        public string Building { get; private set; }

        public bool isPublic { get; set; } // общая ли заметтка или нет

        public string Owner { get; set; }

        // добавление заметок пользователя
        public Note(User user, string text, string room, string building, bool acsess)
        {
            Id = User.CurrentUser.Notes.Count.ToString();
            Text = text;
            Date = new DateTime().ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
            Owner = user.Name;
        }

        // добавление общих заметок в NoteData
        public Note(string text, string room, string building, bool acsess)
        {
            Id = NoteData.Notes.Count.ToString();
            Text = text;
            Date = new DateTime().ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
            Owner = "Вася";
        }
    }
}