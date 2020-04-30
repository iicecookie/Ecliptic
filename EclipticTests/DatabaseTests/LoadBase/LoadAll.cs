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

namespace EclipticTests.DatabaseTests.UserTests
{
    [TestClass]
    public class LoadAll
    {

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DbService.RefrashDb(true);
            DbService.ClearAll();
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
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [TestMethod]
        public void AddEmtyNotes()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = NoteData.Notes.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddEmtyRooms()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = RoomData.Rooms.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddEmtyWorkers()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = WorkerData.Workers.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddEmtyUser()
        {
            // Arrange   -------------------------------------    

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            Assert.IsNull(User.CurrentUser);
        }

        [TestMethod]
        public void AddListNotes()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSample();

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = NoteData.Notes.Count();

            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void AddListRooms()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSample();

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = RoomData.Rooms.Count();

            Assert.AreEqual(14, count);
        }

        [TestMethod]
        public void AddListWorkers()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSample();

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var count = WorkerData.Workers.Count();

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void AddOneUser()
        {
            // Arrange   -------------------------------------    
            DbService.SaveUser(User.setInstance("", "", ""));

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            Assert.IsNotNull(User.CurrentUser);
        }

        [TestMethod]
        public void CheckRoomsRelashions()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSample();

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var room = RoomData.Rooms.First();

            Assert.AreEqual(1, room.Workers.Count);
        }

        [TestMethod]
        public void CheckWorkersRelashions()
        {
            // Arrange   -------------------------------------    
            DbService.LoadSample();

            // Act   -----------------------------------------        
            DbService.LoadAll();

            // Assert-----------------------------------------
            var worker = WorkerData.GetWorker("Celivans", "irina", "vasileva");

            Assert.AreEqual("202", worker.Room.Name);
        }
    }
}
