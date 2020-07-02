using Android.InputMethodServices;
using Ecliptic.Data;
using Ecliptic.Database;
using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ecliptic.Repository
{
    public static class DbService
    {
        private static ApplicationContext db = new ApplicationContext();

        // при связывании паттерна раскоментировать
        // и использовать эти интерфейсы для работы с базой данныъ
        // public static Repository<Building> buildings = new Repository<Building>(db);
        // public static Repository<Floor>    floors    = new Repository<Floor>   (db);
        // public static Repository<Room>     rooms     = new Repository<Room>    (db);
        // public static Repository<Worker>   workers   = new Repository<Worker>  (db);
        // public static Repository<PointM>   points    = new Repository<PointM>  (db);
        // public static Repository<EdgeM>    edges     = new Repository<EdgeM>   (db);
        // public static Repository<Client>   clients   = new Repository<Client>  (db);
        // public static Repository<Note>     notes     = new Repository<Note>    (db);
        // public static Repository<FavoriteRoom> favorites = new Repository<FavoriteRoom>(db);

        /// <summary>
        /// Обновление базы данных
        /// </summary>
        /// <param name="delete">если true очитстить текущую базу</param>
        public static void RefrashDb(bool delete = false)
        {
            // Удаляем бд, если она существуеты
            if (delete)
                db.Database.EnsureDeleted();

            // Создаем бд, если она отсутствует
            db.Database.EnsureCreated();
        }

        public static void SaveDb()
        {
            db.SaveChanges();
        }

        /// <summary>
        /// Загрузить все данные из базы в память (при запуске)
        /// </summary>
        public static void LoadAll()
        {
            WorkerData.Workers = RelationsWorkersRoom();
            RoomData  .Rooms   = RelationsRoomsWorker();

            FloorData   .Floors    = db.Floors   .ToList();
            BuildingData.Buildings = db.Buildings.ToList();

            NoteData  .Notes   = LoadAllPublicNotes();
            PointData .Points  = LoadAllPoints();
            EdgeData  .Edges   = LoadAllEdges();

            BuildingData.CurrentBuilding = FloorData.Floors.FirstOrDefault()?.Building;

            if (db.Client.Count() > 0)
            {
                Client.setClient(LoadClientFromDb());
                Client.CurrentClient.Favorites = db.FavoriteRooms.ToList();
                LoadClientNotes(Client.CurrentClient);
            }
        }

        #region building
        // удалить все здания из списка доступных кроме текущего
        public static void RemoveUncurrentBuildings()
        {
            foreach (var building in BuildingData.Buildings)
            {
                if (building != BuildingData.CurrentBuilding)
                    db.Buildings.Remove(building);
            }
            db.SaveChanges();
        }

        // удалить данные текущего здания
        public static void RemoveCurrentBuilding()
        {
            if (db.Floors .Count() > 0) db.Floors .RemoveRange(db.Floors);  db.SaveChanges();
            if (db.Rooms  .Count() > 0) db.Rooms  .RemoveRange(db.Rooms);   db.SaveChanges();
            if (db.Workers.Count() > 0) db.Workers.RemoveRange(db.Workers); db.SaveChanges();
            if (db.Points .Count() > 0) db.Points .RemoveRange(db.Points);  db.SaveChanges();
            if (db.Edges  .Count() > 0) db.Edges  .RemoveRange(db.Edges);   db.SaveChanges();
            BuildingData.CurrentBuilding = null;
            db.Notes.RemoveRange(db.Notes);
            db.SaveChanges();
        }

        #endregion

        #region Building

        public static void AddBuilding(Building building)
        {
            if (building == null) return;
            db.Buildings.Add(building); 
            db.SaveChanges();
        }

        public static void AddBuilding(List<Building> buildings)
        {
            if (buildings == null) return;

            foreach (var building in buildings)
                db.Buildings.Add(building);

            db.SaveChanges();
        }

        public static List<Building> LoadAllBuildings()
        {
            return db.Buildings.ToList();
        }

        #endregion

        #region Floor
        public static void AddFloor(Floor floor)
        {
            if (floor == null) return;
            db.Floors.Add(floor);
            db.SaveChanges();
        }
        public static void AddFloor(List<Floor> floors)
        {
            if (floors == null) return;
            foreach (var floor in floors)
            {
                db.Floors.Add(floor);
                db.SaveChanges();
            }
        }

        public static void RemoveFloor(Floor floor)
        {
            if (floor == null) return;
            db.Floors.Remove(floor);

            db.SaveChanges();
        }
        public static void RemoveFloor(List<Floor> floors)
        {
            foreach (var floor in floors)
            {
                db.Floors.Remove(floor);
            }
            db.SaveChanges();
        }

        public static List<Floor> LoadAllFloors()
        {
            return db.Floors.ToList();
        }
        #endregion

        #region Rooms
        public static void AddRoom(Room room)
        {
            if (room == null) return;
            db.Rooms.Add(room);
            db.SaveChanges();
        }

        public static void AddRoom(List<Room> rooms)
        {
            if (rooms == null) return;

            for (int i = 0, c = 0; i < rooms.Count; i++)
            {
                db.Rooms.Add(rooms[i]);
                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }
            db.SaveChanges();
        }

        public static void RemoveRoom(Room room)
        {
            if (room == null) return;
            db.Rooms.Remove(room);

            db.SaveChanges();
        }

        public static void RemoveRoom(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                db.Rooms.Remove(room);
            }
            db.SaveChanges();
        }

        // явная загрузка связей между объектами
        public static List<Room> RelationsRoomsWorker()
        {
            return db.Rooms.Include(u => u.Workers).ToList();
        }

        public static List<Room> LoadAllRooms()
        {
            return db.Rooms.ToList();
        }

        #endregion

        #region Workers

        public static void AddWorker(Worker worker)
        {
            if (worker == null) return;
            db.Workers.Add(worker);
            db.SaveChanges();
        }

        public static void AddWorker(List<Worker> workers)
        {
            if (workers == null) return; 
            for (int i = 0, c = 0; i < workers.Count; i++)
            {
                db.Workers.Add(workers[i]);
                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }

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

        #region Points
        public static void AddPoing(PointM point)
        {
            if (point == null) return;
            db.Points.Add(point);
            db.SaveChanges();
        }

        // сохраняется после каждой 50й точки
        // объем памяти для хранения не сохраненных данных ограничен, поетому нужно переодически сохранять данные
        public static void AddPoing(List<PointM> points)
        {
            if (points == null) return;
            for (int i = 0, c = 0; i < points.Count; i++)
            {
                db.Points.Add(points[i]);
                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }

            db.SaveChanges();
        }

        public static void RemovePoint(PointM point)
        {
            if (point == null) return;
            db.Points.Remove(point);

            db.SaveChanges();
        }
        public static void RemovePoint(List<PointM> points)
        {
            foreach (var point in points)
            {
                db.Points.Remove(point);
            }
            db.SaveChanges();
        }

        public static List<PointM> LoadAllPoints()
        {
            return db.Points.ToList();
        }
        #endregion

        #region Edges
        public static void AddEdge(EdgeM edge)
        {
            if (edge == null) return;
            db.Edges.Add(edge);
            db.SaveChanges();
        }
        public static void AddEdge(List<EdgeM> edges)
        {
            if (edges == null) return;
            for (int i = 0, c = 0; i < edges.Count; i++)
            {
                db.Edges.Add(edges[i]);
                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }
            db.SaveChanges();
        }

        public static void RemoveEdge(EdgeM edge)
        {
            if (edge == null) return;
            db.Edges.Remove(edge);

            db.SaveChanges();
        }
        public static void RemoveEdge(List<EdgeM> edges)
        {
            foreach (var edge in edges)
            {
                db.Edges.Remove(edge);
            }
            db.SaveChanges();
        }

        public static List<EdgeM> LoadAllEdges()
        {
            return db.Edges.ToList();
        }
        #endregion

        #region Notes

        public static void AddNote(Note note)
        {
            if (note == null) return;
            db.Notes.Add(note);
            db.SaveChanges();
        }

        public static void AddNote(List<Note> notes)
        {
            if (notes == null) return;
            for (int i = 0, c = 0; i < notes.Count; i++)
            {
                db.Notes.Add(notes[i]);

                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }
            db.SaveChanges();
        }

        public static void RemoveNote(Note note)
        {
            if (note == null) return;
            db.Notes.Remove(note); 
            db.SaveChanges();
        }

        public static void RemoveNote(List<Note> notes)
        {
            if (notes == null) return;
            for (int i = 0, c = 0; i < notes.Count; i++)
            {
                db.Notes.Remove(notes[i]);
                if (c == 50)
                {
                    db.SaveChanges();
                    c = 0;
                }
            }
            db.SaveChanges();
        }

        public static void UpdateNote(Note note)
        {
            if (note == null) return;
            db.Notes.Update(note);

            db.SaveChanges();
        }

        public static Note FindNote(Note note)
        {
            if (note == null) return null;

            var notes = db.Notes.Where(s =>
                                       s.Text == note.Text &&
                                       s.Date == note.Date &&
                                       s.Building == note.Building &&
                                       s.Room == note.Room &&
                                       s.isPublic == note.isPublic &&
                                       s.ClientId == note.ClientId
                                       );
            if (notes.Count() > 0)
                return notes.First();
            return null;
        }

        public static List<Note> LoadAllNotes()
        {
            return db.Notes.ToList();
        }

        public static List<Note> LoadAllPublicNotes()
        {
            return db.Notes.Where(s => s.isPublic == true&& s.ClientId == null).ToList();
        }

        #endregion

        #region Favs
        public static void AddFavoriteRoom(FavoriteRoom favRoom)
        {
            if (favRoom == null) return;
            db.FavoriteRooms.Add(favRoom);
            db.SaveChanges();
        }

        public static void AddFavoriteRoom(List<FavoriteRoom> favRooms)
        {
            foreach (var favRoom in favRooms)
            {
                db.FavoriteRooms.Add(favRoom);
            }
            db.SaveChanges();
        }

        public static void RemoveFavoriteRoom(FavoriteRoom room)
        {
            if (room == null) return;
            db.FavoriteRooms.Remove(room);

            db.SaveChanges();
        }

        public static void RemoveFavoriteRoom(List<FavoriteRoom> rooms)
        {
            foreach (var room in rooms)
            {
                db.FavoriteRooms.Remove(room);
            }
            db.SaveChanges();
        }
        #endregion

        #region Client

        public static Client LoadClientFromDb()
        {
            if (db.Client.Count() == 0) return null;
            return db.Client.ToList().First();
        }

        public static void SaveClient(Client client)
        {
            if (client == null) return;
            db.Client.Add(client);
            db.SaveChanges();
        }

        public static void RemoveClient(Client client)
        {
            if (client == null) return;
            db.Client.Remove(client);
            db.SaveChanges();
        }

        // подгрузка 
        public static List<Note> LoadClientNotes(Client client)
        {
            foreach (var i in db.Notes)
                if (i.ClientId == client.ClientId)
                {}
            return Client.CurrentClient.Notes;
        }

        #endregion
    }
}
