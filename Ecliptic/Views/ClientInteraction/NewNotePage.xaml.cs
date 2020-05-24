using Ecliptic.Data;
using Ecliptic.Models;
using Ecliptic.Repository;
using Ecliptic.WebInteractions;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecliptic.Views.ClientNote
{
    public partial class NewNotePage : ContentPage
    {
        public NewNotePage()
        {
            InitializeComponent();

            stackBarRoom.HeightRequest = 1;
            stackBarBuilding.HeightRequest = 1;
            SearchBarRoom.Text = "";
            SearchBarBuilding.Text = "";
            NoteText.Text = "";
        }

        Room roomnote = null;

        async void OnButtonSaveClicked(object sender, EventArgs args)
        {
            if (NoteText.Text == "")
            {
                DependencyService.Get<IToast>().Show("Ну и зачем так делать?");
                return;
            }

            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }

            bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
            if (!isRemoteReachable)
            {
                await DisplayAlert("Сервер не доступен", "Повторите попытку позже", "OK"); return;
            }

            NoteService noteService = new NoteService();
            Note note = await noteService.Add(new Note(NoteText.Text,
                                                       SearchBarRoom.Text,
                                                       roomid:    roomnote?.RoomId,
                                                       building:  SearchBarBuilding.Text,
                                                       clientid:  Client.CurrentClient.ClientId,
                                                       clientname:Client.CurrentClient.Name));
            
            if (note != null) // если сервер вернул данные по заметке - загрузить в пользователя
            {
                DbService.AddNote(note); 

                await Navigation.PopAsync();
                return;
            }
            else
            {
                await DisplayAlert("Ошибка", "Сервер не вернул данные", "OK");
                return;
            }
        }

        #region textchange
        void OnTextRoomChanged    (object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (tapped == false)
            {
                roomnote = null;
            }

            if (SearchBarRoom.Text == "")
            {
                searchRoomResults.ItemsSource = new List<Room>();
                stackBarRoom.HeightRequest = 1;
                return;
            }

            var searchedrooms = RoomData.Rooms
                      .Where(room => room.Name       .ToLower().Contains(SearchBarRoom.Text.ToLower()) ||
                                     room.Description.ToLower().Contains(SearchBarRoom.Text.ToLower()))
                      .ToList<Room>();

            stackBarRoom.HeightRequest    = searchedrooms.Count() > 5 ? 250 : searchedrooms.Count() * 50;
            searchRoomResults.ItemsSource = searchedrooms;
            tapped = false;
        }

        void OnTextBuildingChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            if (SearchBarBuilding.Text == "")
            {
                searchBuildingResults.ItemsSource = new List<Room>();
                stackBarBuilding.HeightRequest = 1;
                return;
            }

            var searchedbuildings = BuildingData.Buildings
                      .Where(building => building.Name       .ToLower().Contains(SearchBarBuilding.Text.ToLower()) ||
                                         building.Description.ToLower().Contains(SearchBarBuilding.Text.ToLower()))
                      .ToList<Building>();

            stackBarBuilding.HeightRequest    = searchedbuildings.Count() > 5 ? 250 : searchedbuildings.Count() * 50;
            searchBuildingResults.ItemsSource = searchedbuildings;
        }
        #endregion

        #region tappedivents
        public bool tapped = false;
        private void OnRoomTapped    (object sender, ItemTappedEventArgs e)
        {
            tapped = true;
            roomnote = (Room)e.Item;
            SearchBarRoom.Text = roomnote.Name;
            stackBarRoom.HeightRequest = 1;
            searchRoomResults.ItemsSource = new List<Room>();
        }
        private void OnBuildingTapped(object sender, ItemTappedEventArgs e)
        {
            tapped = true;
            Building room = (e.Item as Building);
            SearchBarBuilding.Text = room.Name;
            stackBarBuilding.HeightRequest = 1;
            searchBuildingResults.ItemsSource = new List<Building>();
        }
        #endregion
    }
}