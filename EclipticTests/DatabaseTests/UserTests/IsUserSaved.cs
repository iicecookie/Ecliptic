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
    public class IsUserSaved
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
        public void IsUserSavedYep()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("me", "you");

            // Act   -----------------------------------------        
            bool issaved = DbService.isSavedUser();

            // Assert-----------------------------------------

            Assert.AreEqual(true, issaved);
            User.LoginOut();
        }

        [TestMethod]
        public void IsUserSavedNo()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSampleUser("me", "you");
            User.LoginOut();
            // Act   -----------------------------------------        
            bool issaved = DbService.isSavedUser();

            // Assert-----------------------------------------

            Assert.AreEqual(false, issaved);
        }

        [TestMethod]
        public void IsUserSavedNoNo()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            bool issaved = DbService.isSavedUser();

            // Assert-----------------------------------------

            Assert.AreEqual(false, issaved);
        }
    }
}
