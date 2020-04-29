using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;

namespace EclipticTests.UserPage
{
    [TestClass]
    public class DeleteNoteClickedTsest
    {
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // обновление базы данных
            DbService.RefrashDb(true);

            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            User.LoginOut();
        }

        [TestMethod]
        public void PrivateNoteNormalData()
        {
            // Arrange   -------------------------------------
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
        }

        [TestMethod]
        public void ZeroIdInButton()
        {
            // Arrange   -------------------------------------
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
        }

        [TestMethod]
        public void UnexistIdInButton()
        {
            // Arrange   -------------------------------------
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
        }

        [TestMethod]
        public void DeletePublicNote()
        {
            // Arrange   -------------------------------------
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
        }
    }
}
