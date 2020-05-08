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
    public class LoadPublicNotes
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
        public void LoadMany()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("12", "21");
            DbService.AddNote(new Note("I'm open note", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open okey", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open yesi", "522", "KGU", true));
            DbService.AddNote(new Note("I'm open noby", "231", "KGU", true));
            DbService.AddNote(new Note("I'm open puko", "409", "KGU", true));
            DbService.AddNote(new Note("I'm user puko", "409", "KGU", true, 1));
            DbService.AddNote(new Note("I'm user yesi", "409", "KGU", true, 1));
            DbService.AddNote(new Note("I'm user okey", "409", "KGU", true, 1));

            // Act   -----------------------------------------     
            var result = DbService.LoadAllPublicNotes();

            // Assert-----------------------------------------
            Assert.AreEqual(5, result.Count);
        }


    }
}
