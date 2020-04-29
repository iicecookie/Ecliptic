using Ecliptic.Data;
using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EclipticTests
{
    [TestClass]
    public class WorkerDataTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            DbService.RefrashDb(true);
            bool expected;

            // Act   
            expected = RoomData.isThatRoom("213");

            // Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            // Arrange
            DbService.RefrashDb(true);
            bool expected;

            // Act   
            Room v = null;
            expected = RoomData.isThatRoom(v);

            // Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            // Arrange
            DbService.RefrashDb(true);
            bool expected;

            // Act   
            expected = RoomData.isThatRoom("");

            // Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod4()
        {
            // Arrange
            DbService.RefrashDb(true);
            bool expected;

            // Act   
            expected = RoomData.isThatRoom("134");
            // Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod5()
        {
            // Arrange
            DbService.RefrashDb(true);
            bool expected;

            // Act   
            expected = RoomData.isThatRoom("asssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssassssssssssssssssssssssssssssssssssssssssssssssssssasssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");

            // Assert
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }   
    }
}
