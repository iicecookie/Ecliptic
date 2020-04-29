using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;

namespace EclipticTests.UserPage
{
    [TestClass]
    public class AcsessNoteSwitchedTest
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
        public void MakeExistPrivateNotePublic()
        {
            // Arrange   -------------------------------------
            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // свитчер переключается на публичный
            UserControls.Switches[0].IsToggled = true;
            UserPage.OnSwitched(UserControls.Switches[0], new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(1);

            Assert.AreEqual(note.isPublic, true);
        }

        [TestMethod]
        public void MakeExistPublicNotePrivate()
        {
            // Arrange   -------------------------------------
            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // свитчер переключается на публичный
            UserControls.Switches[1].IsToggled = false;
            UserPage.OnSwitched(UserControls.Switches[1], new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(2);

            Assert.AreEqual(note.isPublic, false);
        }

        [TestMethod]
        public void MakeExistPublicNotePublic()
        {
            // Arrange   -------------------------------------
            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();

            // Act   -----------------------------------------
            // свитчер переключается на публичный
            UserControls.Switches[1].IsToggled = true;
            UserPage.OnSwitched(UserControls.Switches[1], new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            Note note = DbService.FindNote(2);

            Assert.AreEqual(note.isPublic, true);
        }

        [TestMethod]
        public void MakeUnexistNotePrivate()
        {
            // Arrange   -------------------------------------
            // свитчер соответствующей заметки
            Switch switcher = new Switch
            {
                AutomationId = "0",
                IsToggled = false
            };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();
            int before = DbService.LoadAllNotes().Count;

            // Act   -----------------------------------------
            // свитчер переключается на приватный
            UserPage.OnSwitched(switcher, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            int after = DbService.LoadAllNotes().Count;

            Assert.AreEqual(before, after);
        }

        [TestMethod]
        public void MakeUnexistNotePublic()
        {
            // Arrange   -------------------------------------
            // свитчер соответствующей заметки
            Switch switcher = new Switch
            {
                AutomationId = "0",
                IsToggled = true
            };

            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetUserPage();
            int before = DbService.LoadAllNotes().Count;

            // Act   -----------------------------------------
            // свитчер переключается на публичный
            UserPage.OnSwitched(switcher, new System.EventArgs());

            // Assert-----------------------------------------
            // проверяю, сохранилась ли информация по заметке
            int after = DbService.LoadAllNotes().Count;

            Assert.AreEqual(before, after);
        }

    }
}
