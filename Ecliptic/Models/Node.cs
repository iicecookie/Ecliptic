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

        public bool isPublic { get; set; }// общая ли заметтка или нет

        public Note(string text)
        {
            Id = User.CurrentUser.Login;
            Text = text;
            Room = "";
            Date = new DateTime().ToString();
            isPublic = false;
        }

        public Note(string id, string text, string room)
        {
            Id = User.CurrentUser.Login + id;
            Text = text;
            Room = room;
            Date = new DateTime().ToString();
            isPublic = false;
        }
    }
}