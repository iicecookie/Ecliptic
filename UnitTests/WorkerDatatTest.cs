using Ecliptic.Data;
using Ecliptic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class WordkerDataTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            RoomData.isThatRoom(null);

            // Arrange
            string firstname = null;
            string secondname = null;
            string thirdname = null;
            Worker worker = null;

            // Act
            worker = WorkerData.GetWorker(firstname, secondname, thirdname);

            // Assert
            Worker actual = null;
            Assert.AreEqual(worker, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            RoomData.isThatRoom(null);

            // Arrange
            string firstname = "Celivans";
            string secondname = "irina";
            string thirdname = null;
            Worker worker = null;

            // Act
            worker = WorkerData.GetWorker(firstname, secondname, thirdname);

            // Assert
            Worker actual = null;
            Assert.AreEqual(worker, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            RoomData.isThatRoom(null);

            // Arrange
            string firstname = "Celivans";
            string secondname = "irina";
            string thirdname = "vasileva";
            Worker worker = null;

            // Act
            worker = WorkerData.GetWorker(firstname, secondname, thirdname);

            // Assert

            Worker actual = new Worker
            {
                FirstName = "Celivans",
                SecondName = "irina",
                LastName = "vasileva",

                Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                Status = "Teacher",

                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                Phone = "8(906)6944309",
                Email = "seliv@mail.ru",
            };

            Assert.AreEqual(worker.Phone, actual.Phone);
        }

        [TestMethod]
        public void TestMethod4()
        {
            RoomData.isThatRoom(null);

            // Arrange
            string firstname = "Uraeva";
            string secondname = "irina";
            string thirdname = "vasileva";
            Worker worker = null;

            // Act
            worker = WorkerData.GetWorker(firstname, secondname, thirdname);

            // Assert
            Worker actual = null;
            Assert.AreEqual(worker, actual);
        }

    }
}


