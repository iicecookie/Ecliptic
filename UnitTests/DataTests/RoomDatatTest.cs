using Ecliptic.Data;
using Ecliptic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class RoomDataTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            RoomData.isThatRoom(null);

            // Arrange
            string room = null;
            bool isRomm;

            // Act
            isRomm = RoomData.isThatRoom(room);

            // Assert
            bool actual = false;
            Assert.AreEqual(isRomm, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            RoomData.isThatRoom(null);

            // Arrange
            bool isRomm;
            string room = "206";

            // Act
            isRomm = RoomData.isThatRoom(room);

            // Assert
            bool actual = true;
            Assert.AreEqual(isRomm, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            RoomData.isThatRoom(null);

            // Arrange
            bool isRomm;
            string room = "55";

            // Act
            isRomm = RoomData.isThatRoom(room);

            // Assert
            bool actual = false;
            Assert.AreEqual(isRomm, actual);
        }
    }

}


