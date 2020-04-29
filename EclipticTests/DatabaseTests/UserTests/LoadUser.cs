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
    public class LoadUser
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
        public void LoadEmptyUser()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            User user = DbService.LoadUserFromDb();

            // Assert-----------------------------------------
            Assert.AreEqual(null, user);
        }

        [TestMethod]
        public void LoadOkeyUser()
        {
            // Arrange   -------------------------------------    
            DbService.SaveUser(User.setInstance("Username", "", ""));

            // Act   -----------------------------------------        
            User user = DbService.LoadUserFromDb();

            // Assert-----------------------------------------
            Assert.AreEqual("Username", user.Name);
        }

        [TestMethod]
        public void LoadFilldFavUser()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            User user = DbService.LoadUserFromDb();

            // Assert-----------------------------------------
            Assert.AreEqual(3, user.Favorites.Count);
        }

        [TestMethod]
        public void LoadFilldNotesUser()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("Username", "");

            // Act   -----------------------------------------        
            User user = DbService.LoadUserFromDb();

            // Assert-----------------------------------------
            Assert.AreEqual(5, user.Notes.Count);
        }
    }
}
