﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class AcsessNoteSwitchedTest
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
            Note note = DbService.LoadUserNotes(User.CurrentUser).First();

            Assert.AreEqual(true, note.isPublic);
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
           
            Assert.AreEqual(false, note.isPublic);
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

            Assert.AreEqual(true, note.isPublic);
        }

        [TestMethod]
        public void MakeUnexistNotePrivate()
        {
            // проверка, что не появились новые публичные заметки
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
            // проверка, что не появились новые публичные заметки
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
