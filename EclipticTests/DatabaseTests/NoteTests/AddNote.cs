using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using static Ecliptic.Views.UserInteraction.Authorization;
using Xunit;
using Ecliptic.Data;

namespace EclipticTests.DatabaseTests
{
    [TestClass]
    public class AddNote
    {       
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // обновление базы данных
            //   
            DbService.ClearAll();
         //   DbService.RefrashDb(true);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
          //  NoteData.Notes = new List<Note>();
          //  RoomData.Rooms = new List<Room>();
          //  WorkerData.Workers = new List<Worker>();
        }   

        [TestMethod]
        public void AddOne()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            
            // Act   -----------------------------------------        
            DbService.AddNote(note);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(count, 1);
        }
        

        [TestMethod]
        public void AddTwoSame()
        {
            // Arrange   -------------------------------------
           //  Note note  = new Note("заметка", "213", "KSU", false);
           //  Note note2 = new Note("заметка1", "213", "KSU", false);
           //  Note note1 = new Note("заметка2", "213", "KSU", false);
            // Act   -----------------------------------------

            // DbService.AddNote(note);
            // DbService.AddNote(note1);
            // DbService.AddNote(note2);
            DbService.AddNote(new Note("I'm open note", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open okey", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open yesi", "522", "KGU", true));
            DbService.AddNote(new Note("I'm open noby", "231", "KGU", true));
            DbService.AddNote(new Note("I'm open puko", "409", "KGU", true));

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(5, count);
        }
        

    }
}
