using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecliptic.Models;
using Ecliptic.Repository;
using Xamarin.Forms;

namespace Ecliptic.Data
{
    // публичные заметки по текущему открытому зданию будут храниться тут
    public static class NoteData
    {
        public static List<Note> Notes { get; set; }

        static NoteData()
        {
            Notes = new List<Note>();
        }

        public static Note FindNote(Note note)
        {
            foreach(var i in Notes)
            {
                if (i.Equals(note))
                {
                    return i;
                }
            }
            return null;
        }
    }
}
