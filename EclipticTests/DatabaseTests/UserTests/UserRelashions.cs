using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using static Ecliptic.Views.UserInteraction.Authorization;
using Xunit;
using Ecliptic.Data;

namespace EclipticTests.DatabaseTests.UserTests
{
    [TestClass]
    public class UserRelashions
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
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
        }

        [TestMethod]
        public void SimpleNoteLoad()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            var notes = DbService.LoadAllNotes();

            // Assert-----------------------------------------
            Assert.AreEqual("Username", notes[0].User.Login);
        }

        [TestMethod]
        public void LoadNoteOfEmptyUser()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            var notes = DbService.LoadAllNotes();

            // Assert-----------------------------------------
            Assert.AreEqual(0, notes.Count);
        }

        [TestMethod]
        public void RemoveUserNotes()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            DbService.RemoveNote(User.CurrentUser.Notes);

            // Assert-----------------------------------------
            Assert.AreEqual(0, DbService.LoadAllNotes().Count);
        }

        [TestMethod]
        public void LoadFavOfEmptyUser()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            var rooms = DbService.LoadAllRooms();

            // Assert-----------------------------------------
            Assert.AreEqual(0, rooms.Count);
        }

        [TestMethod]
        public void RemoveUserFav()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------    

            // Assert-----------------------------------------
            Assert.AreEqual(0, DbService.LoadAllRooms().Count);
        }

    }
}
