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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // очистка контекста
            DbService.ClearAll();

            // загрузка тестового пользователя  
            DbService.LoadSampleUser("", "");
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            // закрыть контекст пользователья
            User.LoginOut();

            // обновить базу данных
            DbService.RefrashDb(true);
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
