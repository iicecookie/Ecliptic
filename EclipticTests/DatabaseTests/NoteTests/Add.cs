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
    public class Add
    {

        [TestInitialize()]
        public void MyTestInitialize()
        {
            // очистка контекста
           // DbService.ClearAll();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            // обновить базу данных
            DbService.ClearAll();
           // DbService.RefrashDb(true);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
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
        public void AddFiveSame()
        {
            // Arrange   -------------------------------------

            // Act   -----------------------------------------
            DbService.AddNote(new Note("I'm open note", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open okey", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open yesi", "522", "KGU", true));
            DbService.AddNote(new Note("I'm open noby", "231", "KGU", true));
            DbService.AddNote(new Note("I'm open puko", "409", "KGU", true));

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(5, count);
        }
        
        [TestMethod]
        public void AddListLinks()
        {
            // Arrange   -------------------------------------
             Note note  = new Note("заметка", "213", "KSU", false);
             Note note2 = new Note("заметка1", "213", "KSU", false);
             Note note1 = new Note("заметка2", "213", "KSU", false);

            // Act   -----------------------------------------
            DbService.AddNote(note);
            DbService.AddNote(note1);
            DbService.AddNote(note2);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(3, count);
        }
           
        [TestMethod]
        public void AddList()
        {
            // Arrange   -------------------------------------
            List<Note> notes = new List<Note>();
            notes.Add(new Note("заметка",  "213", "KSU", false));
            notes.Add(new Note("заметка1", "213", "KSU", false));
            notes.Add(new Note("заметка2", "213", "KSU", false));

            // Act   -----------------------------------------
            DbService.AddNote(notes);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(3, count);
        }
        
        [TestMethod]
        public void AddEmpty()
        {
            // Arrange   -------------------------------------
            Note note = new Note();

            // Act   -----------------------------------------
            DbService.AddNote(note);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void AddNull()
        {
            DbService.ClearAll();
            // Arrange   -------------------------------------
            Note note = null;

            // Act   -----------------------------------------
            DbService.AddNote(note);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(0, count);
        }     
    }
}
