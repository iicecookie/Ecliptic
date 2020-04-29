using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using static Ecliptic.Views.UserInteraction.Authorization;
using Xunit;
using Ecliptic.Data;
using System.Linq;

namespace EclipticTests.DatabaseTests
{
    [TestClass]
    public class Update
    {
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // обновить базу данных
            DbService.ClearAll();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [TestMethod]
        public void UpdateOne()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            // Act   -----------------------------------------       
            note.Text = "текст";
            DbService.UpdateNote(note);

            // Assert-----------------------------------------
            string upstring = DbService.LoadAllNotes().First().Text;

            Assert.AreEqual("текст", upstring);
        }

        [TestMethod]
        public void UpdateEmpty()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            // Act   -----------------------------------------       
            note = null;
            DbService.UpdateNote(note);

            // Assert-----------------------------------------
            string upstring = DbService.LoadAllNotes().First().Text;

            Assert.AreEqual("заметка", upstring);
        }

        [TestMethod]
        public void NonUpdateWoW()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            // Act   -----------------------------------------       
            note.Text = "текст";

            // Assert-----------------------------------------
            string upstring = DbService.LoadAllNotes().First().Text;

            Assert.AreEqual("текст", upstring);
        }
    }
}
