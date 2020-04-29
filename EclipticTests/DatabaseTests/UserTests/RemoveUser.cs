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
    public class RemoveUser
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
        public void SimpleDelete()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            DbService.RemoveUser(User.CurrentUser);

            // Assert-----------------------------------------
            Assert.AreEqual(false, DbService.isSavedUser());
        }

        [TestMethod]
        public void UsersNotesStayhere()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            DbService.RemoveUser(User.CurrentUser);

            // Assert-----------------------------------------
            Assert.AreEqual(5, DbService.LoadAllNotes().Count);
        }

        [TestMethod]
        public void UsersFavsStayhere()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            DbService.RemoveUser(User.CurrentUser);

            // Assert-----------------------------------------
            Assert.AreEqual(3, DbService.LoadAllRooms().Count);
        }

        [TestMethod]
        public void RemoveNull()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            DbService.RemoveUser(null);

            // Assert-----------------------------------------
            Assert.AreEqual("Username", DbService.LoadUserFromDb().Login);
        }
    }
}
