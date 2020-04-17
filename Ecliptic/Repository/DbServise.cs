using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ecliptic.Repository
{
    public static class DbService
    {
        private static ApplicationContext db = new ApplicationContext();

        public static void RefrashDb()
        {
            // Удаляем бд, если она существуеты
            db.Database.EnsureDeleted();
            // Создаем бд, если она отсутствует
            db.Database.EnsureCreated();
        }
        public static void SaveDb()
        {
            db.SaveChanges();
        }

        #region Notes
        public static void AddNote(Note note)
        {
            db.Notes.Add(note);

            db.SaveChanges();
        }

        public static List<Note> LoadAllNotes()
        {
            return db.Notes.ToList();
        }
        #endregion

        #region Building
        public static void AddBuilding(Building building)
        {
            db.Buildings.Add(building);
      
            db.SaveChanges();
        }

        public static List<Building> LoadAllBuildings()
        {
            return db.Buildings.ToList();
        }

        #endregion

        #region Workers
        public static void AddWorker(Worker worker)
        {
            db.Workers.Add(worker);

            db.SaveChanges();
        }

        public static List<Worker> RelationsWorkersRoom()
        {
            return db.Workers.Include(u => u.Room).ToList();
        }

        public static List<Worker> LoadAllWorkers()
        {
            return db.Workers.ToList();
        }
        #endregion

        #region Rooms
        public static void AddRoom(Room room)
        {
            db.Rooms.Add(room);

            db.SaveChanges();
        }

        public static List<Room> RelationsRoomsWorker()
        {
            return db.Rooms.Include(u => u.Workers).ToList();
        }

        public static List<Room> LoadAllRooms()
        {
            return db.Rooms.ToList();
        }
        #endregion
    }
}
