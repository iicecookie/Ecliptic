using Ecliptic.Data;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Client
    {
        #region Params
        public static Client CurrentClient { get; private set; }

        public int ClientId { get; set; }
        public string Name  { get; private set; }
        public string Login { get; private set; }   

        public virtual List<Note> Notes     { get; set; }

        public virtual List<FavoriteRoom> Favorites { get; set; }
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

        public static void LoginOut()
        {
            DbService.RemoveFavoriteRoom(CurrentClient.Favorites);
            DbService.RemoveNote(CurrentClient.Notes);

            DbService.RemoveClient(CurrentClient);

            CurrentClient = null;
        }

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
 
        public static FavoriteRoom isRoomFavoit(Room room)
        {   
            foreach (var favorite in CurrentClient.Favorites)
            {
                if (favorite.Name == room.Name && 
                    favorite.Details == room.Description || 
                    favorite.FavoriteRoomId == room.RoomId)
                {
                    return favorite;
                }
            }
            return null;
        }
    }
}