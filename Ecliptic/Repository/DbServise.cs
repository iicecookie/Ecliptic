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
            WorkerData.Workers = RelationsWorkersRoom();
            RoomData.Rooms     = RelationsRoomsWorker();

            NoteData.Notes     = LoadAllPublicNotes();

            FloorData.Floors       = db.Floors.ToList();
            BuildingData.Buildings = db.Buildings.ToList();

            if (db.User.Count() > 0)
            {
                User.setUser(LoadUserFromDb());
                User.CurrentUser.Favorites = db.FavoriteRooms.ToList();
                LoadUserNotes(User.CurrentUser);
            }
        }

        public static void ClearAll()   
        {
            if (db.Rooms.Count() > 0)
                db.Rooms.RemoveRange(db.Rooms);
            if (db.Notes.Count() > 0)
                db.Notes.RemoveRange(db.Notes);
            if (db.Workers.Count() > 0)
                db.Workers.RemoveRange(db.Workers);
            if (db.User.Count() > 0)
                db.User.RemoveRange(db.User);
            db.SaveChanges();
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
                                       s.UserId == note.UserId
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
            return db.Notes.Where(s => s.isPublic == true && s.User == null).ToList();
        }

        #endregion

        #region Building

        public static void AddBuilding(Building building)
        {
            db.Buildings.Add(building);
        }

        public static List<Building> LoadAllBuildings()
        {
            return db.Buildings.ToList();
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

        #region User

        public static bool isSavedUser()
        {
            if (db.User.Count() > 0) return true;
            return false;
        }

        public static User LoadUser()
        {
            if (db.User.Count() == 0) return null;
            
            User.setUser(db.User.First());

            User.CurrentUser.Notes     = db.Notes.Include(note => note.UserId).ToList();
            User.CurrentUser.Favorites = db.FavoriteRooms.Include(note => note.UserId).ToList();

            return db.User.ToList().First();
        }

        public static User LoadUserFromDb()
        {
            if (db.User.Count() == 0) return null;
            return db.User.ToList().First();
        }

        public static void SaveUser(User user)
        {
            if (user == null) return;
            db.User.Add(user);
            db.SaveChanges();   
        }

        public static void RemoveUser(User user)
        {
            if (user == null) return;
            db.User.Remove(user);
            db.SaveChanges();
        }

        public static List<Note> LoadUserNotes(User user)
        {
            foreach (var i in db.Notes)
                if (i.UserId == user.UserId)
                {
                }
            return User.CurrentUser.Notes;
        }

        #endregion

        #region Sample

        public static void LoadSample()
        {
            LoadSampleBuildings();
            LoadSampleFloors();
            LoadSampleRooms();
            LoadSampleWorkers();

            LoadSampleNotes();

            db.SaveChanges();

            LoadAll();
        }

        public static void LoadSampleBuildings()
        {
            db.Buildings.Add(new Building("KSU", buildingid: 1, description: "sadsadasdsadasdsadasdsadasdsadasdsadasdsadasdasd"));
            db.Buildings.Add(new Building("MGU", buildingid: 2, description: "asdasd"));
            db.Buildings.Add(new Building("GPU", buildingid: 3, description: "s21adsadasdsadasdsadasdsadasdsadasdsadasdsadasdasd"));
        }

        public static void LoadSampleFloors()
        {
            db.Floors.Add(new Floor(1,  floorid: 1, buildingid: 1));
            db.Floors.Add(new Floor(2,  floorid: 2, buildingid: 1));
            db.Floors.Add(new Floor(3,  floorid: 3, buildingid: 1));
            db.Floors.Add(new Floor(-1, floorid: 4, buildingid: 1));
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
                             floorid: 1));

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
                              floorid: 1));

            AddRoom(new Room("202",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                              floorid: 2));


            AddRoom(new Room("201",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                              floorid: 2));


            AddRoom(new Room("202",
                             "The giant panda, also known as panda bear or simply panda, is a bear native to south central China. It is easily recognized by the large, distinctive black patches around its eyes, over the ears, and across its round body. The name giant panda is sometimes used to distinguish it from the unrelated red panda. Though it belongs to the order Carnivora, the giant panda's diet is over 99% bamboo. Giant pandas in the wild will occasionally eat other grasses, wild tubers, or even meat in the form of birds, rodents, or carrion. In captivity, they may receive honey, eggs, fish, yams, shrub leaves, oranges, or bananas along with specially prepared food.",
                              floorid: 2));

            AddRoom(new Room("203",
                             "A grizzly–polar bear hybrid is a rare ursid hybrid that has occurred both in captivity and in the wild. In 2006, the occurrence of this hybrid in nature was confirmed by testing the DNA of a unique-looking bear that had been shot near Sachs Harbour, Northwest Territories on Banks Island in the Canadian Arctic. The number of confirmed hybrids has since risen to eight, all of them descending from the same female polar bear.",
                             phone: "8(906)6944309",
                             floorid: 3));

            AddRoom(new Room("204",
                             "The sloth bear is an insectivorous bear species native to the Indian subcontinent. It is listed as Vulnerable on the IUCN Red List, mainly because of habitat loss and degradation. It has also been called labiated bear because of its long lower lip and palate used for sucking insects. Compared to brown and black bears, the sloth bear is lankier, has a long, shaggy fur and a mane around the face, and long, sickle-shaped claws. It evolved from the ancestral brown bear during the Pleistocene and through convergent evolution shares features found in insect-eating mammals.",
                             floorid: 3));


            AddRoom(new Room("205",
                             "The sun bear is a bear species occurring in tropical forest habitats of Southeast Asia. It is listed as Vulnerable on the IUCN Red List. The global population is thought to have declined by more than 30% over the past three bear generations. Suitable habitat has been dramatically reduced due to the large-scale deforestation that has occurred throughout Southeast Asia over the past three decades. The sun bear is also known as the honey bear, which refers to its voracious appetite for honeycombs and honey.",
                             phone: "8(906)6944309",
                             floorid: 3));

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
                             floorid: 4));

            AddRoom(new Room("409",
                             description: "The California grizzly bear is an extinct subspecies of the grizzly bear, the very large North American brown bear. Grizzly could have meant grizzled (that is, with golden and grey tips of the hair) or fear-inspiring. Nonetheless, after careful study, naturalist George Ord formally classified it in 1815 – not for its hair, but for its character – as Ursus horribilis (terrifying bear). Genetically, North American grizzlies are closely related; in size and coloring, the California grizzly bear was much like the grizzly bear of the southern coast of Alaska. In California, it was particularly admired for its beauty, size and strength. The grizzly became a symbol of the Bear Flag Republic, a moniker that was attached to the short-lived attempt by a group of American settlers to break away from Mexico in 1846. Later, this rebel flag became the basis for the state flag of California, and then California was known as the Bear State.",
                             phone: "8(906)6944309",
                             floorid: 4));

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
                             floorid: 4));

            AddRoom(new Room("219",
                             description: "The California grizzly bear is an extinct subspecies of the grizzly bear, the very large North American brown bear. Grizzly could have meant grizzled (that is, with golden and grey tips of the hair) or fear-inspiring. Nonetheless, after careful study, naturalist George Ord formally classified it in 1815 – not for its hair, but for its character – as Ursus horribilis (terrifying bear). Genetically, North American grizzlies are closely related; in size and coloring, the California grizzly bear was much like the grizzly bear of the southern coast of Alaska. In California, it was particularly admired for its beauty, size and strength. The grizzly became a symbol of the Bear Flag Republic, a moniker that was attached to the short-lived attempt by a group of American settlers to break away from Mexico in 1846. Later, this rebel flag became the basis for the state flag of California, and then California was known as the Bear State.",
                             site: "okasdjasdk",
                             floorid: 4));
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

        public static void LoadSampleNotes()
        {
            AddNote(new Note("I'm open note", "KGU", true, roomid: 1));
            AddNote(new Note("I'm open okey", "KGU", true, roomid: 1));
            AddNote(new Note("I'm open yesi", "KGU", true, roomid: 2));
            AddNote(new Note("I'm open noby", "KGU", true, roomid: 2));
            AddNote(new Note("I'm open puko", "KGU", true, roomid: 3));
        }

        public static User LoadSampleUser(string login, string password)
        {
            // записываю в пользователя
            User.setUser(1, password, login);

            // загружаю в базу данных
            DbService.SaveUser(User.CurrentUser);

            DbService.AddNote(new Note("заметка1", "KGU", false, roomid: 1, userid: User.CurrentUser.UserId));
            DbService.AddNote(new Note("заметка2", "KGU", true,  roomid: 1, userid: User.CurrentUser.UserId));
            DbService.AddNote(new Note("заметка3", "KGU", false, roomid: 2, userid: User.CurrentUser.UserId));
            DbService.AddNote(new Note("заметка4", "KGU", false, roomid: 2, userid: User.CurrentUser.UserId));
            DbService.AddNote(new Note("заметка5", "KGU", false, roomid: 3, userid: User.CurrentUser.UserId));

            DbService.AddFavoriteRoom(
                new FavoriteRoom("213",
                            "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                            "", User.CurrentUser.UserId, 1));

            DbService.AddFavoriteRoom(
                 new FavoriteRoom("200",
                             "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                             "", User.CurrentUser.UserId, 2));

            DbService.AddFavoriteRoom(
                 new FavoriteRoom("202",
                             "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
                             "", User.CurrentUser.UserId, 3));
            
            db.SaveChanges();

            return User.CurrentUser;
        }

        #endregion
    }
}
