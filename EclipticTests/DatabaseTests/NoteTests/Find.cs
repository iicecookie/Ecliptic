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
    public class Find
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
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
        public void FindOne()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            Note test = new Note("заметка", "213", "KSU", false);

            // Act   -----------------------------------------     
            Note result = DbService.FindNote(test);

            // Assert-----------------------------------------
            Assert.AreEqual(1, result.NoteId);
        }

        [TestMethod]
        public void FindOneNonExist()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            Note test = new Note("заметка", "404", "KSU", false);

            // Act   -----------------------------------------     
            Note result = DbService.FindNote(test);

            // Assert-----------------------------------------
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindNull()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            Note test = null;

            // Act   -----------------------------------------     
            Note result = DbService.FindNote(test);

            // Assert-----------------------------------------
            Assert.IsNull(result);
        }

    }
}
