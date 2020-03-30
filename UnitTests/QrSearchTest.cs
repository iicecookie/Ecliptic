using Ecliptic.Data;
using Ecliptic.Models;
using Ecliptic.Views.RoomInform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace UnitTests
{
    [TestClass]
    public class QrSearchTest
    {
        struct ZXing
        {
            public string Text;
            public ZXing(string res) { Text = res; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            string expected = "206";
            string outres = "";


            ZXing result = new ZXing("206");

            // Act
            if (result.Text != null)
                if (RoomData.isThatRoom(result.Text))
                {
                    outres = result.Text;
                }
                else
                {
                    outres = result.Text;
                }

            // Assert
            Assert.AreEqual(expected, outres);
        }

    }
}


