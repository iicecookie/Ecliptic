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

namespace Ecliptic.Views.UserNote
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

        async void OnButtonSaveClicked(object sender, EventArgs args)
        {
            if (NoteText.Text == "")
            {
                DependencyService.Get<IToast>().Show("Ну и зачем так делать?");
                return;
            }

          // if (SearchBarRoom.Text == "" || SearchBarBuilding.Text == "" || NoteText.Text == "")
          // {
          //     DependencyService.Get<IToast>().Show("Не все поля заполнены");
          //     return;
          // }

            Room room = RoomData.isThatRoom(SearchBarRoom.Text);

            if (WebData.istest)
            {
                DbService.AddNote(new Note(NoteText.Text,
                                           SearchBarBuilding.Text,
                                           SearchBarRoom.Text,
                                           false,
                                           roomid: room?.RoomId,
                                           userid: User.CurrentUser.UserId,
                                           username: User.CurrentUser.Name));

                await Navigation.PopAsync();
                return;
            } // что бы тестить без сервера

            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }

            bool isRemoteReachable = await CrossConnectivity.Current.IsReachable(WebData.ADRESS);
            if (!isRemoteReachable)
            {
                await DisplayAlert("Сервер не доступен", "Повторите попытку позже", "OK");
                return;
            }

            NoteService noteService = new NoteService();
            Note note = await noteService.Add(new Note(NoteText.Text,
                                                       SearchBarBuilding.Text,
                                                       SearchBarRoom.Text,
                                                       false,
                                                       roomid: room?.RoomId,
                                                       userid: User.CurrentUser.UserId));
            
            // если сервер вернул данные по заметке - загрузить в пользователя
            if (note != null)
            {
                DbService.AddNote(note); // сохранили полученую заметку с данными

                await Navigation.PopAsync();
                return;
            }
            else
            {
                await DisplayAlert("Ошибка", "Сервер не вернул данные", "OK");
                return;
            }
        }

        void OnTextRoomChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

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

        private void OnRoomTapped(object sender, ItemTappedEventArgs e)
        {
            Room room = (e.Item as Room);
            SearchBarRoom.Text = room.Name;
            stackBarRoom.HeightRequest = 1;
            searchRoomResults.ItemsSource = new List<Room>();
        }

        private void OnBuildingTapped(object sender, ItemTappedEventArgs e)
        {
            Building room = (e.Item as Building);
            SearchBarBuilding.Text = room.Name;
            stackBarBuilding.HeightRequest = 1;
            searchBuildingResults.ItemsSource = new List<Building>();
        }
    }
}