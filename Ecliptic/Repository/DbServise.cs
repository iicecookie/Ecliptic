using Ecliptic.Data;
using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Ecliptic.Repository
{
    public static class DbService
    {
        private static ApplicationContext db = new ApplicationContext();

        #region main

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

        public static void LoadAll()
        {
            BuildingData.Buildings = db.Buildings.ToList();
            FloorData.Floors = db.Floors.ToList();

            WorkerData.Workers = RelationsWorkersRoom();
            RoomData.Rooms    = RelationsRoomsWorker();

            NoteData.Notes = LoadAllPublicNotes();

            PointData.Points = LoadAllPoints();
            EdgeData.Edges   = LoadAllEdges();

            if (db.Client.Count() > 0)
            {
                Client.setClient(LoadClientFromDb());
                Client.CurrentClient.Favorites = db.FavoriteRooms.ToList();
                LoadClientNotes(Client.CurrentClient);
            }
        }
        public static void RemoveBuildings()
        {
            if (db.Buildings.Count() > 0) db.Buildings.RemoveRange(db.Buildings);

            db.SaveChanges();
        }

        public static void RemoveCurrentBuilding()
        {
            if (db.Floors .Count() > 0) db.Floors .RemoveRange(db.Floors);
            if (db.Rooms  .Count() > 0) db.Rooms  .RemoveRange(db.Rooms);
            if (db.Workers.Count() > 0) db.Workers.RemoveRange(db.Workers);
            if (db.Points .Count() > 0) db.Points .RemoveRange(db.Points);
            if (db.Edges  .Count() > 0) db.Edges  .RemoveRange(db.Edges);

            if (Client.CurrentClient == null) db.Notes.RemoveRange(db.Notes);
            else db.Notes.RemoveRange(db.Notes.Where(n => n.ClientId != Client.CurrentClient.ClientId));

            db.SaveChanges();
        }

        #endregion

        #region Building

        public static void AddBuilding(Building building)
        {
            db.Buildings.Add(building); 
            db.SaveChanges();
        }

        public static void AddBuilding(List<Building> buildings)
        {
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
                db.Floors.Add(floor);
            db.SaveChanges();
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
            foreach (var room in rooms)
                db.Rooms.Add(room);
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
            foreach (var worker in workers)
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

        #region Points
        public static void AddPoing(PointM point)
        {
            if (point == null) return;
            db.Points.Add(point);
            db.SaveChanges();
        }
        public static void AddPoing(List<PointM> points)
        {
            if (points == null) return;
            foreach (var point in points)
                db.Points.Add(point);
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
            foreach (var edge in edges)
                db.Edges.Add(edge);
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
            foreach (var note in notes)
            {
                db.Notes.Add(note);
            }
            db.SaveChanges();
        }

        public static void RemoveNote(Note note)
        {
            if (note == null) return;
            //   if (db.Notes.Find(note) == null) return;
            db.Notes.Remove(note);

            db.SaveChanges();
        }

        public static void RemoveNote(List<Note> notes)
        {
            foreach (var note in notes)
            {
                db.Notes.Remove(note);
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
            return db.Notes.Where(s => s.isPublic == true && s.Client == null).ToList();
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

        public static bool isSavedClient()
        {
            if (db.Client.Count() > 0) return true;
            return false;
        }

        public static Client LoadClient()
        {
            if (db.Client.Count() == 0) return null;

            Client.setClient(db.Client.First());

            Client.CurrentClient.Notes = db.Notes.Include(note => note.ClientId).ToList();
            Client.CurrentClient.Favorites = db.FavoriteRooms.Include(note => note.ClientId).ToList();

            return db.Client.ToList().First();
        }

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

        public static List<Note> LoadClientNotes(Client client)
        {
            foreach (var i in db.Notes)
                if (i.ClientId == client.ClientId)
                {
                }
            return Client.CurrentClient.Notes;
        }

        #endregion

        #region Sample

        public static void LoadSample()
        {
            LoadSampleBuildings(); db.SaveChanges();
            LoadSampleFloors();    db.SaveChanges();
            LoadSampleRooms();     db.SaveChanges();
            LoadSampleWorkers();   db.SaveChanges();
             
         //   LoadSampleNotes();     db.SaveChanges();

          //  LoadSamplePoints();    db.SaveChanges();
          //  LoadSampleEdges();     db.SaveChanges();
          //
          //  LoadSampleWays();      db.SaveChanges();
          
            LoadAll();
        }

        public static void LoadSampleBuildings()
        {
            db.Buildings.Add(new Building("KSU", buildingid: 1,
                                          site: "http://hui.com/", timetable: "открыто всегда",
                                          addrees: "Курская ул блалала",
                                          description: "sadsadasdsadasdsadasdsadasdsadasdsadasdsadasdasd"));
            db.Buildings.Add(new Building("MGU", buildingid: 2, description: "asdasd"));
            db.Buildings.Add(new Building("GPU", buildingid: 3, description: "s21adsadasdsadasdsadasdsadasdsadasdsadasdsadasdasd"));
        }

        public static void LoadSampleFloors()
        {
            // db.Floors.Add(new Floor(1,  floorid: 1, buildingid: 1));
            // db.Floors.Add(new Floor(2,  floorid: 2, buildingid: 1));
            db.Floors.Add(new Floor(3, floorid: 3, buildingid: 1));
            db.Floors.Add(new Floor(4, floorid: 4, buildingid: 1));
            // db.Floors.Add(new Floor(-1, floorid: 5, buildingid: 1));
        }

        public static void LoadSampleRooms()
        {
            AddRoom(new Room("213",
                             "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                             "Время работы:вторник	  10:00–22:00" +
                                            "             среда       10:00–22:00" +
                                            "             четверг     10:00–22:00" +
                                            "             пятница     10:00–22:00" +
                                            "             суббота     10:00–22:00" +
                                            "             воскресенье 10:00–22:00" +
                                            "             понедельник 10:00–22:00",
                             "8(906)6944309",
                             "xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                             floorid: 4));

            AddRoom(new Room("200",
                              "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                              "Время работы:вторник	  10:00–22:00" +
                                            "             среда       10:00–22:00" +
                                            "             четверг     10:00–22:00" +
                                            "             пятница     10:00–22:00" +
                                            "             суббота     10:00–22:00" +
                                            "             воскресенье 10:00–22:00" +
                                            "             понедельник 10:00–22:00",
                             "8(906)6944309",
                             "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                              floorid: 4));

            AddRoom(new Room("202",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                              floorid: 4));


            AddRoom(new Room("201",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                              floorid: 4));


            AddRoom(new Room("199",
                             "The giant panda, also known as panda bear or simply panda, is a bear native to south central China. It is easily recognized by the large, distinctive black patches around its eyes, over the ears, and across its round body. The name giant panda is sometimes used to distinguish it from the unrelated red panda. Though it belongs to the order Carnivora, the giant panda's diet is over 99% bamboo. Giant pandas in the wild will occasionally eat other grasses, wild tubers, or even meat in the form of birds, rodents, or carrion. In captivity, they may receive honey, eggs, fish, yams, shrub leaves, oranges, or bananas along with specially prepared food.",
                              floorid: 4));

            AddRoom(new Room("203",
                             "A grizzly–polar bear hybrid is a rare ursid hybrid that has occurred both in captivity and in the wild. In 2006, the occurrence of this hybrid in nature was confirmed by testing the DNA of a unique-looking bear that had been shot near Sachs Harbour, Northwest Territories on Banks Island in the Canadian Arctic. The number of confirmed hybrids has since risen to eight, all of them descending from the same female polar bear.",
                             phone: "8(906)6944309",
                             floorid: 4));

            AddRoom(new Room("204",
                             "The sloth bear is an insectivorous bear species native to the Indian subcontinent. It is listed as Vulnerable on the IUCN Red List, mainly because of habitat loss and degradation. It has also been called labiated bear because of its long lower lip and palate used for sucking insects. Compared to brown and black bears, the sloth bear is lankier, has a long, shaggy fur and a mane around the face, and long, sickle-shaped claws. It evolved from the ancestral brown bear during the Pleistocene and through convergent evolution shares features found in insect-eating mammals.",
                             floorid: 4));


            AddRoom(new Room("205",
                             "The sun bear is a bear species occurring in tropical forest habitats of Southeast Asia. It is listed as Vulnerable on the IUCN Red List. The global population is thought to have declined by more than 30% over the past three bear generations. Suitable habitat has been dramatically reduced due to the large-scale deforestation that has occurred throughout Southeast Asia over the past three decades. The sun bear is also known as the honey bear, which refers to its voracious appetite for honeycombs and honey.",
                             phone: "8(906)6944309",
                             floorid: 4));

            AddRoom(new Room("206",
                             "The polar bear is a hypercarnivorous bear whose native range lies largely within the Arctic Circle, encompassing the Arctic Ocean, its surrounding seas and surrounding land masses. It is a large bear, approximately the same size as the omnivorous Kodiak bear. A boar (adult male) weighs around 350–700 kg (772–1,543 lb), while a sow (adult female) is about half that size. Although it is the sister species of the brown bear, it has evolved to occupy a narrower ecological niche, with many body characteristics adapted for cold temperatures, for moving across snow, ice and open water, and for hunting seals, which make up most of its diet. Although most polar bears are born on land, they spend most of their time on the sea ice. Their scientific name means maritime bear and derives from this fact. Polar bears hunt their preferred food of seals from the edge of sea ice, often living off fat reserves when no sea ice is present. Because of their dependence on the sea ice, polar bears are classified as marine mammals.",
                             site: "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                             floorid: 4));

            AddRoom(new Room("207",
                             "The spectacled bear, also known as the Andean bear or Andean short-faced bear and locally as jukumari (Aymara), ukumari (Quechua) or ukuku, is the last remaining short-faced bear. Its closest relatives are the extinct Florida spectacled bear, and the giant short-faced bears of the Middle to Late Pleistocene age. Spectacled bears are the only surviving species of bear native to South America, and the only surviving member of the subfamily Tremarctinae. The species is classified as Vulnerable by the IUCN because of habitat loss.",
                             phone: "8(906)6944309",
                             floorid: 4));

            AddRoom(new Room("208",
                             "The cave bear was a species of bear that lived in Europe and Asia during the Pleistocene and became extinct about 24,000 years ago during the Last Glacial Maximum. Both the word cave and the scientific name spelaeus are used because fossils of this species were mostly found in caves. This reflects the views of experts that cave bears may have spent more time in caves than the brown bear, which uses caves only for hibernation.",
                             floorid: 4));

            AddRoom(new Room("209",
                             "The short-faced bears is an extinct bear genus that inhabited North America during the Pleistocene epoch from about 1.8 Mya until 11,000 years ago. It was the most common early North American bear and was most abundant in California. There are two recognized species: Arctodus pristinus and Arctodus simus, with the latter considered to be one of the largest known terrestrial mammalian carnivores that has ever existed. It has been hypothesized that their extinction coincides with the Younger Dryas period of global cooling commencing around 10,900 BC.",
                             phone: "8(906)6944309",
                             floorid: 3));

            AddRoom(new Room("409",
                             description: "The California grizzly bear is an extinct subspecies of the grizzly bear, the very large North American brown bear. Grizzly could have meant grizzled (that is, with golden and grey tips of the hair) or fear-inspiring. Nonetheless, after careful study, naturalist George Ord formally classified it in 1815 – not for its hair, but for its character – as Ursus horribilis (terrifying bear). Genetically, North American grizzlies are closely related; in size and coloring, the California grizzly bear was much like the grizzly bear of the southern coast of Alaska. In California, it was particularly admired for its beauty, size and strength. The grizzly became a symbol of the Bear Flag Republic, a moniker that was attached to the short-lived attempt by a group of American settlers to break away from Mexico in 1846. Later, this rebel flag became the basis for the state flag of California, and then California was known as the Bear State.",
                             phone: "8(906)6944309",
                             floorid: 3));

            AddRoom(new Room("309",
                             description: "The California grizzly bear is an extinct subspecies of the grizzly bear, the very large North American brown bear. Grizzly could have meant grizzled (that is, with golden and grey tips of the hair) or fear-inspiring. Nonetheless, after careful study, naturalist George Ord formally classified it in 1815 – not for its hair, but for its character – as Ursus horribilis (terrifying bear). Genetically, North American grizzlies are closely related; in size and coloring, the California grizzly bear was much like the grizzly bear of the southern coast of Alaska. In California, it was particularly admired for its beauty, size and strength. The grizzly became a symbol of the Bear Flag Republic, a moniker that was attached to the short-lived attempt by a group of American settlers to break away from Mexico in 1846. Later, this rebel flag became the basis for the state flag of California, and then California was known as the Bear State.",
                             phone: "21232938923",
                             site: "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                             timetable: "Время работы: \n" +
                                        "вторник	 10:00–22:00 \n" +
                                        "среда       10:00–22:00 \n" +
                                        "четверг     10:00–22:00 \n" +
                                        "пятница     10:00–22:00 \n" +
                                        "суббота     10:00–22:00 \n" +
                                        "воскресенье 10:00–22:00 \n" +
                                        "понедельник 10:00–22:00 \n",
                             floorid: 3));

            AddRoom(new Room("219",
                             description: "The California grizzly bear is an extinct subspecies of the grizzly bear, the very large North American brown bear. Grizzly could have meant grizzled (that is, with golden and grey tips of the hair) or fear-inspiring. Nonetheless, after careful study, naturalist George Ord formally classified it in 1815 – not for its hair, but for its character – as Ursus horribilis (terrifying bear). Genetically, North American grizzlies are closely related; in size and coloring, the California grizzly bear was much like the grizzly bear of the southern coast of Alaska. In California, it was particularly admired for its beauty, size and strength. The grizzly became a symbol of the Bear Flag Republic, a moniker that was attached to the short-lived attempt by a group of American settlers to break away from Mexico in 1846. Later, this rebel flag became the basis for the state flag of California, and then California was known as the Bear State.",
                             site: "okasdjasdk",
                             floorid: 3));
        }

        public static void LoadSampleWorkers()
        {
            AddWorker(new Worker
            {
                FirstName = "Celivans",
                SecondName = "irina",
                LastName = "vasileva",

                Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                Status = "Teacher",

                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                Phone = "8(906)6944309",
                Email = "seliv@mail.ru",
                RoomId = 3,
            });

            AddWorker(new Worker
            {
                FirstName = "Uraeva",
                SecondName = "Elena",

                Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                Status = "Teacher",

                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                Phone = "8(906)6944309",
                Email = "seliv@mail.ru",
                RoomId = 1,
            });

            AddWorker(new Worker
            {
                FirstName = "Makarov",
                SecondName = "Kirya",

                Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                Status = "Teacher",

                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                Phone = "8(906)6944309",
                Email = "seliv@mail.ru",
                RoomId = 2,
            });
        }

        public static void LoadSamplePoints()
        {
            #region 4thFloor
            db.Points.Add(new PointM(0, 0,       floorId: 4, pointId: 1));
            db.Points.Add(new PointM(0, 500,     floorId: 4, pointId: 2));
            db.Points.Add(new PointM(250, 0,     floorId: 4, pointId: 3));
            db.Points.Add(new PointM(250, 210,   floorId: 4, pointId: 4));
            db.Points.Add(new PointM(250, 290,   floorId: 4, pointId: 5));
            db.Points.Add(new PointM(250, 500,   floorId: 4, pointId: 6));
            db.Points.Add(new PointM(400, 0,     floorId: 4, pointId: 7));
            db.Points.Add(new PointM(400, 210,   floorId: 4, pointId: 8));
            db.Points.Add(new PointM(400, 290,   floorId: 4, pointId: 9));
            db.Points.Add(new PointM(400, 500,   floorId: 4, pointId: 10));
            db.Points.Add(new PointM(820, 0,     floorId: 4, pointId: 11));
            db.Points.Add(new PointM(820, 210,   floorId: 4, pointId: 12));
            db.Points.Add(new PointM(820, 290,   floorId: 4, pointId: 13));
            db.Points.Add(new PointM(820, 500,   floorId: 4, pointId: 14));
            db.Points.Add(new PointM(1270, 0,    floorId: 4, pointId: 15));
            db.Points.Add(new PointM(1270, 210,  floorId: 4, pointId: 16));
            db.Points.Add(new PointM(1270, 290,  floorId: 4, pointId: 17));
            db.Points.Add(new PointM(1270, 500,  floorId: 4, pointId: 18));
            db.Points.Add(new PointM(1270, -730, floorId: 4, pointId: 19));
            db.Points.Add(new PointM(1270, -900, floorId: 4, pointId: 20));
            db.Points.Add(new PointM(1350, 0,    floorId: 4, pointId: 21));
            db.Points.Add(new PointM(1350, 210,  floorId: 4, pointId: 22));
            db.Points.Add(new PointM(1520, 290,  floorId: 4, pointId: 23));
            db.Points.Add(new PointM(1520, 500,  floorId: 4, pointId: 24));
            db.Points.Add(new PointM(1350, -200, floorId: 4, pointId: 25));
            db.Points.Add(new PointM(1350, -400, floorId: 4, pointId: 26));
            db.Points.Add(new PointM(1350, -730, floorId: 4, pointId: 27));
            db.Points.Add(new PointM(1700, 0,    floorId: 4, pointId: 28));
            db.Points.Add(new PointM(1700, 210,  floorId: 4, pointId: 29));
            db.Points.Add(new PointM(1700, 290,  floorId: 4, pointId: 30));
            db.Points.Add(new PointM(1700, 500,  floorId: 4, pointId: 31));
            db.Points.Add(new PointM(1700, -200, floorId: 4, pointId: 32));
            db.Points.Add(new PointM(1700, -400, floorId: 4, pointId: 33));
            db.Points.Add(new PointM(1700, -730, floorId: 4, pointId: 34));
            db.Points.Add(new PointM(1700, -900, floorId: 4, pointId: 35));
            db.Points.Add(new PointM(1900, 0,    floorId: 4, pointId: 36));
            db.Points.Add(new PointM(1900, 500,  floorId: 4, pointId: 37));

            #endregion
            db.SaveChanges();

            #region 3thFloor
            db.Points.Add(new PointM(0, 0,      floorId: 3, pointId: 51));
            db.Points.Add(new PointM(0, 500,    floorId: 3, pointId: 52));
            db.Points.Add(new PointM(250, 0,    floorId: 3, pointId: 53));
            db.Points.Add(new PointM(250, 210,  floorId: 3, pointId: 54));
            db.Points.Add(new PointM(250, 290,  floorId: 3, pointId: 55));
            db.Points.Add(new PointM(250, 500,  floorId: 3, pointId: 56));
            db.Points.Add(new PointM(400, 0,    floorId: 3, pointId: 57));
            db.Points.Add(new PointM(400, 210,  floorId: 3, pointId: 58));
            db.Points.Add(new PointM(400, 290,  floorId: 3, pointId: 59));
            db.Points.Add(new PointM(400, 500,  floorId: 3, pointId: 60));
            db.Points.Add(new PointM(820, 0,    floorId: 3, pointId: 61));
            db.Points.Add(new PointM(820, 210,  floorId: 3, pointId: 62));
            db.Points.Add(new PointM(820, 290,  floorId: 3, pointId: 63));
            db.Points.Add(new PointM(820, 500,  floorId: 3, pointId: 64));
            db.Points.Add(new PointM(1270, 0,   floorId: 3, pointId: 65));
            db.Points.Add(new PointM(1270, 210, floorId: 3, pointId: 66));
            db.Points.Add(new PointM(1270, 290, floorId: 3, pointId: 67));
            db.Points.Add(new PointM(1270, 500, floorId: 3, pointId: 68));
            db.Points.Add(new PointM(1350, 0,   floorId: 3, pointId: 69));
            db.Points.Add(new PointM(1350, 210, floorId: 3, pointId: 70));
            db.Points.Add(new PointM(1520, 290, floorId: 3, pointId: 71));
            db.Points.Add(new PointM(1520, 500, floorId: 3, pointId: 72));
            db.Points.Add(new PointM(1700, 0,   floorId: 3, pointId: 73));
            db.Points.Add(new PointM(1700, 210, floorId: 3, pointId: 74));
            db.Points.Add(new PointM(1700, 290, floorId: 3, pointId: 75));
            db.Points.Add(new PointM(1700, 500, floorId: 3, pointId: 76));
            db.Points.Add(new PointM(1900, 0,   floorId: 3, pointId: 77));
            db.Points.Add(new PointM(1900, 500, floorId: 3, pointId: 78));
            #endregion
            db.SaveChanges();
        }

        public static void LoadSampleEdges()
        {
            #region 4thFloor
            db.Edges.Add(new EdgeM(0, pointFirId: 1, pointSecId: 3, edgeId: 1));
            db.Edges.Add(new EdgeM(0, pointFirId: 1, pointSecId: 2, edgeId: 2));
            db.Edges.Add(new EdgeM(0, pointFirId: 2, pointSecId: 6, edgeId: 3));
            db.Edges.Add(new EdgeM(0, pointFirId: 3, pointSecId: 4, edgeId: 4));
            db.Edges.Add(new EdgeM(0, pointFirId: 4, pointSecId: 5, edgeId: 5));
            db.Edges.Add(new EdgeM(0, pointFirId: 5, pointSecId: 6, edgeId: 6));
            db.Edges.Add(new EdgeM(0, pointFirId: 6,  pointSecId: 10, edgeId: 7));
            db.Edges.Add(new EdgeM(0, pointFirId: 10, pointSecId: 9,  edgeId: 8));
            db.Edges.Add(new EdgeM(0, pointFirId: 9,  pointSecId: 5,  edgeId: 9));
            db.Edges.Add(new EdgeM(0, pointFirId: 4,  pointSecId: 8,  edgeId: 11));
            db.Edges.Add(new EdgeM(0, pointFirId: 8,  pointSecId: 7,  edgeId: 12));
            db.Edges.Add(new EdgeM(0, pointFirId: 7,  pointSecId: 3,  edgeId: 13));
            db.Edges.Add(new EdgeM(0, pointFirId: 7,  pointSecId: 11, edgeId: 14));
            db.Edges.Add(new EdgeM(0, pointFirId: 11, pointSecId: 12, edgeId: 15));
            db.Edges.Add(new EdgeM(0, pointFirId: 12, pointSecId: 8,  edgeId: 16));
            db.Edges.Add(new EdgeM(0, pointFirId: 11, pointSecId: 15, edgeId: 17));
            db.Edges.Add(new EdgeM(0, pointFirId: 15, pointSecId: 16, edgeId: 18));
            db.Edges.Add(new EdgeM(0, pointFirId: 16, pointSecId: 12, edgeId: 19));
            db.Edges.Add(new EdgeM(0, pointFirId: 16, pointSecId: 19, edgeId: 20));
            db.Edges.Add(new EdgeM(0, pointFirId: 19, pointSecId: 20, edgeId: 21));
            db.Edges.Add(new EdgeM(0, pointFirId: 20, pointSecId: 35, edgeId: 22));
            db.Edges.Add(new EdgeM(0, pointFirId: 35, pointSecId: 34, edgeId: 23));
            db.Edges.Add(new EdgeM(0, pointFirId: 34, pointSecId: 27, edgeId: 24));
            db.Edges.Add(new EdgeM(0, pointFirId: 27, pointSecId: 19, edgeId: 25));
            db.Edges.Add(new EdgeM(0, pointFirId: 34, pointSecId: 33, edgeId: 26));
            db.Edges.Add(new EdgeM(0, pointFirId: 33, pointSecId: 32, edgeId: 27));
            db.Edges.Add(new EdgeM(0, pointFirId: 32, pointSecId: 28, edgeId: 28));
            db.Edges.Add(new EdgeM(0, pointFirId: 28, pointSecId: 36, edgeId: 29));
            db.Edges.Add(new EdgeM(0, pointFirId: 37, pointSecId: 36, edgeId: 30));
            db.Edges.Add(new EdgeM(0, pointFirId: 37, pointSecId: 31, edgeId: 31));
            db.Edges.Add(new EdgeM(0, pointFirId: 31, pointSecId: 24, edgeId: 32));
            db.Edges.Add(new EdgeM(0, pointFirId: 24, pointSecId: 18, edgeId: 33));
            db.Edges.Add(new EdgeM(0, pointFirId: 18, pointSecId: 14, edgeId: 34));
            db.Edges.Add(new EdgeM(0, pointFirId: 14, pointSecId: 10, edgeId: 35));
            db.Edges.Add(new EdgeM(0, pointFirId: 9,  pointSecId: 13, edgeId: 36));
            db.Edges.Add(new EdgeM(0, pointFirId: 13, pointSecId: 17, edgeId: 37));
            db.Edges.Add(new EdgeM(0, pointFirId: 17, pointSecId: 23, edgeId: 38));
            db.Edges.Add(new EdgeM(0, pointFirId: 23, pointSecId: 30, edgeId: 39));
            db.Edges.Add(new EdgeM(0, pointFirId: 13, pointSecId: 14, edgeId: 41));
            db.Edges.Add(new EdgeM(0, pointFirId: 17, pointSecId: 18, edgeId: 42));
            db.Edges.Add(new EdgeM(0, pointFirId: 23, pointSecId: 24, edgeId: 43));
            db.Edges.Add(new EdgeM(0, pointFirId: 31, pointSecId: 30, edgeId: 44));
            db.Edges.Add(new EdgeM(0, pointFirId: 30, pointSecId: 29, edgeId: 45));
            db.Edges.Add(new EdgeM(0, pointFirId: 29, pointSecId: 28, edgeId: 46));
            db.Edges.Add(new EdgeM(0, pointFirId: 22, pointSecId: 29, edgeId: 47));
            db.Edges.Add(new EdgeM(0, pointFirId: 21, pointSecId: 28, edgeId: 48));
            db.Edges.Add(new EdgeM(0, pointFirId: 25, pointSecId: 32, edgeId: 49));
            db.Edges.Add(new EdgeM(0, pointFirId: 26, pointSecId: 33, edgeId: 50));
            db.Edges.Add(new EdgeM(0, pointFirId: 27, pointSecId: 26, edgeId: 51));
            db.Edges.Add(new EdgeM(0, pointFirId: 26, pointSecId: 25, edgeId: 52));
            db.Edges.Add(new EdgeM(0, pointFirId: 25, pointSecId: 21, edgeId: 53));
            db.Edges.Add(new EdgeM(0, pointFirId: 21, pointSecId: 22, edgeId: 54));
            #endregion
            db.SaveChanges();

            #region 3thFloor
            db.Edges.Add(new EdgeM(0, pointFirId: 51, pointSecId: 53, edgeId: 61));
            db.Edges.Add(new EdgeM(0, pointFirId: 51, pointSecId: 52, edgeId: 62));
            db.Edges.Add(new EdgeM(0, pointFirId: 52, pointSecId: 56, edgeId: 63));
            db.Edges.Add(new EdgeM(0, pointFirId: 53, pointSecId: 54, edgeId: 64));
            db.Edges.Add(new EdgeM(0, pointFirId: 54, pointSecId: 55, edgeId: 65));
            db.Edges.Add(new EdgeM(0, pointFirId: 55, pointSecId: 56, edgeId: 66));
            db.Edges.Add(new EdgeM(0, pointFirId: 56, pointSecId: 60, edgeId: 67));
            db.Edges.Add(new EdgeM(0, pointFirId: 60, pointSecId: 59, edgeId: 68));
            db.Edges.Add(new EdgeM(0, pointFirId: 59, pointSecId: 55, edgeId: 69));
            db.Edges.Add(new EdgeM(0, pointFirId: 54, pointSecId: 58, edgeId: 70));
            db.Edges.Add(new EdgeM(0, pointFirId: 58, pointSecId: 57, edgeId: 71));
            db.Edges.Add(new EdgeM(0, pointFirId: 57, pointSecId: 53, edgeId: 72));
            db.Edges.Add(new EdgeM(0, pointFirId: 57, pointSecId: 51, edgeId: 73));
            db.Edges.Add(new EdgeM(0, pointFirId: 51, pointSecId: 77, edgeId: 74));
            db.Edges.Add(new EdgeM(0, pointFirId: 77, pointSecId: 78, edgeId: 75));
            db.Edges.Add(new EdgeM(0, pointFirId: 78, pointSecId: 52, edgeId: 76));
            db.Edges.Add(new EdgeM(0, pointFirId: 61, pointSecId: 62, edgeId: 80)); 
            db.Edges.Add(new EdgeM(0, pointFirId: 63, pointSecId: 64, edgeId: 81));
            db.Edges.Add(new EdgeM(0, pointFirId: 65, pointSecId: 66, edgeId: 82));
            db.Edges.Add(new EdgeM(0, pointFirId: 67, pointSecId: 68, edgeId: 83));
            db.Edges.Add(new EdgeM(0, pointFirId: 73, pointSecId: 76, edgeId: 84));
            db.Edges.Add(new EdgeM(0, pointFirId: 69, pointSecId: 70, edgeId: 85));
            db.Edges.Add(new EdgeM(0, pointFirId: 71, pointSecId: 72, edgeId: 86));
            db.Edges.Add(new EdgeM(0, pointFirId: 54, pointSecId: 66, edgeId: 87));//
            db.Edges.Add(new EdgeM(0, pointFirId: 55, pointSecId: 71, edgeId: 88));
            db.Edges.Add(new EdgeM(0, pointFirId: 75, pointSecId: 71, edgeId: 89));
            db.Edges.Add(new EdgeM(0, pointFirId: 70, pointSecId: 74, edgeId: 90));

            #endregion
            db.SaveChanges();
        }

        public static void LoadSampleWays()
        {
            #region 4thFloor
            // 4 этаж точки комнат
            db.Points.Add(new PointM(125, 250,   pointId:101, isWaypoint: true, floorId: 4, roomId: 1));
            db.Points.Add(new PointM(600, 120,   pointId:102, isWaypoint: true, floorId: 4, roomId: 2));
            db.Points.Add(new PointM(600, 400,   pointId:103, isWaypoint: true, floorId: 4, roomId: 3));
            db.Points.Add(new PointM(1000, 120,  pointId:104, isWaypoint: true, floorId: 4, roomId: 4));
            db.Points.Add(new PointM(1000, 400,  pointId:105, isWaypoint: true, floorId: 4, roomId: 5));
            db.Points.Add(new PointM(1500, -800, pointId:106, isWaypoint: true, floorId: 4, roomId: 6));
            db.Points.Add(new PointM(1520, -500, pointId:107, isWaypoint: true, floorId: 4, roomId: 7));
            db.Points.Add(new PointM(1520, -300, pointId:108, isWaypoint: true, floorId: 4, roomId: 8));
            db.Points.Add(new PointM(1520, -100, pointId:109, isWaypoint: true, floorId: 4, roomId: 9));
            db.Points.Add(new PointM(1800, 300,  pointId:110, isWaypoint: true, floorId: 4, roomId: 10));
            db.Points.Add(new PointM(1350, 400,  pointId:111, isWaypoint: true, floorId: 4, roomId: 11));
            // 4 этаж точки связи                         
            db.Points.Add(new PointM(600, 250,   pointId:112, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(900, 250,   pointId:113, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1300, 250,  pointId:114, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1600, 250,  pointId:115, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1300, 120,  pointId:116, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1300, -300, pointId:117, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1300, -500, pointId:118, isWaypoint: true, floorId: 4));
            db.Points.Add(new PointM(1520, 100,  pointId:119, isWaypoint: true, floorId: 4));

            db.SaveChanges();
            #endregion

            #region 3thFloor
            // 3 этаж токи комнат
            db.Points.Add(new PointM(125, 250,  pointId: 120,  isWaypoint: true, floorId: 3, roomId: 12));
            db.Points.Add(new PointM(600, 120,  pointId: 121,  isWaypoint: true, floorId: 3, roomId: 13));
            db.Points.Add(new PointM(600, 400,  pointId: 122,  isWaypoint: true, floorId: 3, roomId: 14));
            db.Points.Add(new PointM(1000, 120, pointId: 123,  isWaypoint: true, floorId: 3, roomId: 15));
            // 3 этаж точки связи             
            db.Points.Add(new PointM(600, 250,  pointId: 124, isWaypoint: true, floorId: 3));
            db.Points.Add(new PointM(900, 250,  pointId: 125, isWaypoint: true, floorId: 3));
            db.Points.Add(new PointM(1300, 250, pointId: 126, isWaypoint: true, floorId: 3));
            db.Points.Add(new PointM(1600, 250, pointId: 127, isWaypoint: true, floorId: 3));
            db.Points.Add(new PointM(1300, 120, pointId: 128, isWaypoint: true, floorId: 3));
            db.Points.Add(new PointM(1520, 100, pointId: 131, isWaypoint: true, floorId: 3));
            db.SaveChanges();
            #endregion

            #region 4thFloorEdge
            // 4 этаж связи комнат
            db.Edges.Add(new EdgeM(10, pointFirId: 101, pointSecId: 112, edgeId: 101));
            db.Edges.Add(new EdgeM(10, pointFirId: 112, pointSecId: 102, edgeId: 102));
            db.Edges.Add(new EdgeM(10, pointFirId: 112, pointSecId: 103, edgeId: 103));
            db.Edges.Add(new EdgeM(10, pointFirId: 112, pointSecId: 113, edgeId: 104));
            db.Edges.Add(new EdgeM(10, pointFirId: 113, pointSecId: 104, edgeId: 105));
            db.Edges.Add(new EdgeM(10, pointFirId: 113, pointSecId: 105, edgeId: 106));
            db.Edges.Add(new EdgeM(10, pointFirId: 113, pointSecId: 114, edgeId: 107));
            db.Edges.Add(new EdgeM(10, pointFirId: 114, pointSecId: 111, edgeId: 108));
            db.Edges.Add(new EdgeM(10, pointFirId: 114, pointSecId: 115, edgeId: 109));
            db.Edges.Add(new EdgeM(10, pointFirId: 115, pointSecId: 110, edgeId: 110));
            db.Edges.Add(new EdgeM(10, pointFirId: 114, pointSecId: 116, edgeId: 111));
            db.Edges.Add(new EdgeM(10, pointFirId: 116, pointSecId: 119, edgeId: 112));
            db.Edges.Add(new EdgeM(10, pointFirId: 116, pointSecId: 117, edgeId: 113));
            db.Edges.Add(new EdgeM(10, pointFirId: 117, pointSecId: 108, edgeId: 114));
            db.Edges.Add(new EdgeM(10, pointFirId: 117, pointSecId: 109, edgeId: 115));
            db.Edges.Add(new EdgeM(10, pointFirId: 117, pointSecId: 118, edgeId: 116));
            db.Edges.Add(new EdgeM(10, pointFirId: 118, pointSecId: 107, edgeId: 117));
            db.Edges.Add(new EdgeM(10, pointFirId: 118, pointSecId: 106, edgeId: 118));

            db.SaveChanges();
            #endregion

            #region 3thFloorEdge
            // 3 этаж связи комнат 
            db.Edges.Add(new EdgeM(10, pointFirId: 120, pointSecId: 124, edgeId: 120));
            db.Edges.Add(new EdgeM(10, pointFirId: 124, pointSecId: 121, edgeId: 121));
            db.Edges.Add(new EdgeM(10, pointFirId: 124, pointSecId: 122, edgeId: 122));
            db.Edges.Add(new EdgeM(10, pointFirId: 124, pointSecId: 125, edgeId: 123));
            db.Edges.Add(new EdgeM(10, pointFirId: 125, pointSecId: 123, edgeId: 124));
            db.Edges.Add(new EdgeM(10, pointFirId: 125, pointSecId: 126, edgeId: 125));
            db.Edges.Add(new EdgeM(10, pointFirId: 126, pointSecId: 128, edgeId: 126));
            db.Edges.Add(new EdgeM(10, pointFirId: 128, pointSecId: 131, edgeId: 127));
            db.Edges.Add(new EdgeM(10, pointFirId: 126, pointSecId: 127, edgeId: 128));

            db.Edges.Add(new EdgeM(10, pointFirId: 131, pointSecId: 119, edgeId: 130)); // elevator

            db.SaveChanges();
            #endregion

            // cвязь этажей
            db.SaveChanges();
        }

        public static void LoadSampleNotes()
        {
            AddNote(new Note("I'm open note", "KGU", "213", true, roomid: 1));
            AddNote(new Note("I'm open okey", "KGU", "213", true, roomid: 1));
            AddNote(new Note("I'm open yesi", "KGU", "200", true, roomid: 2));
            AddNote(new Note("I'm open noby", "KGU", "200", true, roomid: 2));
            AddNote(new Note("I'm open puko", "KGU", "202", true, roomid: 3));
        }

        public static Client LoadSampleClient(string login, string password)
        {
            // записываю в пользователя
            Client.setClient(1, password, login);

            // загружаю в базу данных
            DbService.SaveClient(Client.CurrentClient);

            DbService.AddNote(new Note("заметка1", "KGU", "213", false, roomid: 1, clientid: Client.CurrentClient.ClientId));
            DbService.AddNote(new Note("заметка2", "KGU", "213", true,  roomid: 1, clientid: Client.CurrentClient.ClientId));
            DbService.AddNote(new Note("заметка3", "KGU", "200", false, roomid: 2, clientid: Client.CurrentClient.ClientId));
            DbService.AddNote(new Note("заметка4", "KGU", "200", false, roomid: 2, clientid: Client.CurrentClient.ClientId));
            DbService.AddNote(new Note("заметка5", "KGU", "202", false, roomid: 3, clientid: Client.CurrentClient.ClientId));

            DbService.AddFavoriteRoom(
                new FavoriteRoom("213",
                            "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                            "", Client.CurrentClient.ClientId, 1));

            DbService.AddFavoriteRoom(
                 new FavoriteRoom("200",
                             "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                             "", Client.CurrentClient.ClientId, 2));

            DbService.AddFavoriteRoom(
                 new FavoriteRoom("202",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                             "", Client.CurrentClient.ClientId, 3));
            
            db.SaveChanges();

            return Client.CurrentClient;
        }

        #endregion
    }
}
