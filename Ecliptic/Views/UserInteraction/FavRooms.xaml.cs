using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class FavRoomsPage : ContentPage
    {
        ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        public ObservableCollection<Room> Rooms { get { return rooms; } }

        public FavRoomsPage()
        {
            InitializeComponent();

            Title = "Избоанные аудитории";

            RoomView.ItemsSource = rooms;
        }

        private async void RoomView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string roomName = (e.Item as Room).Name;

            await Shell.Current.GoToAsync($"roomdetails?name={roomName}");
        }

        protected override void OnAppearing()
        {
            rooms.Clear();

            foreach (var i in User.CurrentUser.Favorites)
            {
                rooms.Add(i);
            }
            base.OnAppearing();
        }
    }
}