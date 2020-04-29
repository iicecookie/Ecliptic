using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;
using System.Linq;

namespace EclipticTests.UserPage
{
    [TestClass]
    public class SaveNoteClickedTsest
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
        public void NormalTest()
        {
            // Arrange   -------------------------------------
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
            Note note = DbService.LoadUserNotes(User.CurrentUser).ElementAt(0);

            Assert.AreEqual(note.Text, Text);
        }
        
        [TestMethod]
        public void ZeroLinkInButton()
        {
            // Arrange   -------------------------------------
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
            Note note = DbService.LoadUserNotes(User.CurrentUser).ElementAt(0);

            Assert.AreEqual(note.Text, Text); 
        }

        [TestMethod]
        public void WrongLinkInButton()
        {
            // Arrange   -------------------------------------
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
            Note note = DbService.LoadUserNotes(User.CurrentUser).ElementAt(0);

            Assert.AreEqual(note.Text, Text);
        }
    }
}
