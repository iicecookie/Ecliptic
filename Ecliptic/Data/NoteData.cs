using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecliptic.Models;
using Ecliptic.Repository;
using Xamarin.Forms;

namespace Ecliptic.Data
{
    public static class NoteData
    {
        public static List<Note> Notes { get; set; } // публичные заметки о помещениях текущего здания

        static NoteData()
        {
            Notes = new List<Note>();
        }
    }
}
