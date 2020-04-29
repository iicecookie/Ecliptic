using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using static Ecliptic.Views.UserInteraction.Authorization;
using Xunit;

namespace EclipticTests.DatabaseTests
{
    [TestClass]
    public class AddNote
    {/*
        [Fact]
        [TestMethod]
        public void AddOne()
        {
            // Arrange   -------------------------------------    
            DbService.RefrashDb(true);

            DbService.LoadAll();
            Note note = new Note("заметка", "213", "KSU", false);
            
            // Act   -----------------------------------------        
            DbService.AddNote(note); 

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(count, 1);
        }
        
        [Fact]
        [TestMethod]
        public void AddTwoSame()
        {
            // Arrange   -------------------------------------
            DbService.RefrashDb(true);

            DbService.LoadAll();
            Note note  = new Note("заметка", "213", "KSU", false);
            Note note2 = new Note("заметка1", "213", "KSU", false);
            Note note1 = new Note("заметка2", "213", "KSU", false);
            // Act   -----------------------------------------

            DbService.AddNote(note);
            DbService.AddNote(note1);
            DbService.AddNote(note2);
            DbService.AddNote(new Note("I'm open note", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open okey", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open yesi", "522", "KGU", true));
            DbService.AddNote(new Note("I'm open noby", "231", "KGU", true));
            DbService.AddNote(new Note("I'm open puko", "409", "KGU", true));

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(count, 8);
        }
        */

    }
}
