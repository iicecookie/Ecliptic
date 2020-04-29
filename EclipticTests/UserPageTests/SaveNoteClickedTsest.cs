using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;

namespace EclipticTests.ModelTests
{
    [TestClass]
    public class SaveNoteClickedTsest
    {
        [TestMethod]
        public void NormalTest()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true); 
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", ""); 

            // кнопка соответствующей заметки
            ImageButton SaveBtn = new ImageButton { AutomationId = "1" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // изменяю текст заметки
            UserControls.Editors[0].Text = "net text";
            // нажимаю кнопку сохранить
            UserPage.OnButtonSaveClicked(SaveBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            string Text = "net text";
            Assert.AreEqual(DbService.FindNote(1).Text, Text);
            User.LoginOut();
        }
        
        [TestMethod]
        public void ZeroLinkInButton()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton SaveBtn = new ImageButton { AutomationId = "0" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // изменяю текст заметки
            UserControls.Editors[0].Text = "net text";
            // нажимаю кнопку сохранить
            UserPage.OnButtonSaveClicked(SaveBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            string Text = "заметка1";
            Assert.AreEqual(DbService.FindNote(1).Text, Text); 
            User.LoginOut();
        }

        [TestMethod]
        public void WrongLinkInButton()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton SaveBtn = new ImageButton { AutomationId = "50" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // изменяю текст заметки
            UserControls.Editors[0].Text = "net text";
            // нажимаю кнопку сохранить
            UserPage.OnButtonSaveClicked(SaveBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            string Text = "заметка1";
            Assert.AreEqual(DbService.FindNote(1).Text, Text);
            User.LoginOut();
        }
    }
}
