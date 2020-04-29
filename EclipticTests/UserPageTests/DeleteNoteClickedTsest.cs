using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;

namespace EclipticTests.ModelTests
{
    [TestClass]
    public class DeleteNoteClickedTsest
    {
        [TestMethod]
        public void PrivateNoteNormalData()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton DeleteBtn = new ImageButton { AutomationId = "1" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // нажимаю кнопку удалить
            UserPage.OnButtonDeleteClicked(DeleteBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(1);

            Assert.IsNull(note);
            User.LoginOut();
        }

        [TestMethod]
        public void ZeroIdInButton()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton DeleteBtn = new ImageButton { AutomationId = "0" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // нажимаю кнопку удалить
            UserPage.OnButtonDeleteClicked(DeleteBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(1);

            Assert.AreEqual(note.Text, "заметка1");
            User.LoginOut();
        }

        [TestMethod]
        public void UnexistIdInButton()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton DeleteBtn = new ImageButton { AutomationId = "50" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // нажимаю кнопку удалить
            UserPage.OnButtonDeleteClicked(DeleteBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(1);

            Assert.AreEqual(note.Text, "заметка1");
            User.LoginOut();
        }

        [TestMethod]
        public void DeletePublicNote()
        {
            // Arrange   -------------------------------------
            // обновление базы данных
            DbService.RefrashDb(true);
            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");

            // кнопка соответствующей заметки
            ImageButton DeleteBtn = new ImageButton { AutomationId = "2" };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // нажимаю кнопку удалить
            UserPage.OnButtonDeleteClicked(DeleteBtn, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(1);

            Assert.AreEqual(note.Text, "заметка1");
            User.LoginOut();
        }
    }
}
