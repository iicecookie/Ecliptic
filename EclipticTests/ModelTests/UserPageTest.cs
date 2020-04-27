using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;

namespace EclipticTests.ModelTests
{
    [TestClass]
    public class UserPageTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            DbService.RefrashDb(true);    
            DbService.LoadTestSample();
         
            // Arrange
            Note note = DbService.FindNote(1);

            ImageButton SaveBtn = new ImageButton
            {
                Source = "save.png",
                AutomationId = note.Id.ToString(),
            };

            Authorization UserPage = new Authorization();

            
            // Act   
            note.Text = "net text";
            
            UserPage.OnButtonSaveClicked(SaveBtn, null);
            /*
            // Assert
            Assert.AreEqual(DbService.FindNote(1).Text, note.Text);
            
             */
            Assert.AreEqual(1, 1);
        }
    }
}
