using Ecliptic.Data;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Client
    {
        #region Params
        public static Client CurrentClient { get; private set; } // текущий пользователь системы (singleton)

        public int ClientId { get; set; }
        public string Name  { get; private set; }
        public string Login { get; private set; }   

        public virtual List<Note> Notes             { get; set; } // созданые пользователем заметки
        public virtual List<FavoriteRoom> Favorites { get; set; } // избраные помещения пользвоателя
        #endregion

        #region Constructors
        private Client()
        {
            Notes     = new List<Note>();
            Favorites = new List<FavoriteRoom>();
        }
        private Client(int id, string Name, string login) : this()
        {
            this.ClientId = id;
            this.Name = Name;
            this.Login = login;
        }

        public static Client setClient(Client client)
        {
            CurrentClient = client;
            return CurrentClient;
        }

        public static Client setClient (int id, string name, string login)
        {
            CurrentClient = new Client(id, name, login);
            return CurrentClient;
        }
        #endregion

        /// <summary>
        /// Выполнить очистку данных пользователе при выходе из аккаунта
        /// </summary>
        public static void LoginOut()
        {
            DbService.RemoveFavoriteRoom(CurrentClient.Favorites);
            DbService.RemoveNote(CurrentClient.Notes);

            DbService.RemoveClient(CurrentClient);

            CurrentClient = null;
        }

        /// <summary>
        /// Поиск заметки при нажатии на сохранение/удаление/переключение статуса в профиле
        /// </summary>
        /// <param name="id">Идентификатор заметки</param>
        /// <returns>Заметка</returns>
        public static Note FindNoteById(int id)
        {
            Note note = null;

            for (int i = 0; i < CurrentClient.Notes.Count; i++)
            {
                if (CurrentClient.Notes[i].NoteId == id)
                {
                    note = CurrentClient.Notes[i];
                }
            }
            return note;
        }
 
        /// <summary>
        /// Проверка, является ли помещение избранным пользователем
        /// </summary>
        /// <param name="room">помещение</param>
        /// <returns>избранность</returns>
        public static FavoriteRoom isRoomFavoit(Room room)
        {
            if (CurrentClient == null) return null;
            foreach (var favorite in CurrentClient.Favorites)
            {
                if (favorite.Name == room.Name && 
                    favorite.Details == room.Description)
                {
                    return favorite;
                }
            }
            return null;
        }
    }
}