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
        public static List<Note> Notes { get; private set; }

        static NoteData()
        {
            Notes = new List<Note>();

        //   using (var db = new ApplicationContext())
        //   {
        //      if (db.Workers.Count() == 0)
        //       {
        //
        //
        //           db.Notes.Add(new Note("I'm open note", "213", "KGU", true));
        //           db.Notes.Add(new Note("I'm open yesi", "522", "KGU", true));
        //           db.Notes.Add(new Note("I'm open noby", "231", "KGU", true));
        //           db.Notes.Add(new Note("I'm open okey", "213", "KGU", true));
        //           db.Notes.Add(new Note("I'm open puko", "409", "KGU", true));
        //
        //           db.SaveChanges();
        //       }
        //       else
        //       {
        //           Notes = db.Notes.ToList();
        //           int i = 5;
        //       }
        //   }
        }

    }
}
