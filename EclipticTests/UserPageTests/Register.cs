using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using static Ecliptic.Views.UserInteraction.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace EclipticTests.UserPage
{
    [TestClass]
    public class Register
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            // очистка контекста
            DbService.ClearAll();
            DbService.LoadAll();
            // загрузка тестового пользователя  
            // DbService.LoadSampleUser("", "");
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            // закрыть контекст пользователья
            // User.LoginOut();

            // обновить базу данных
            DbService.RefrashDb(true);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        //-------------------------------------

        [TestMethod]
        public void EmptyRegister()
        {
            // Arrange   -------------------------------------
            // создаю отображение пользователей с заметкаим
            Authorization UserPage = new Authorization();
            UserPage.GetRegistrationPage();

            Button LoginBtn = new Button();
            // Act   -----------------------------------------
            // свитчер переключается на публичный

            UserPage.RegistrUser(LoginBtn, new System.EventArgs());

            // Assert-----------------------------------------

            Assert.AreEqual(false, DbService.isSavedUser());
        }

    }
}
