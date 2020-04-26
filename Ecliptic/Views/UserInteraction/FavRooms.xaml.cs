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
        public FavRoomsPage()
        {
            InitializeComponent();

            Title = "Избоанные аудитории";
        }

        private async void RoomView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Room room = e.Item as Room;

            if (RoomData.isThatRoom(room))
            {
                await Shell.Current.GoToAsync($"roomdetails?name={room.Name}");
            }
            else
            {
                DependencyService.Get<IToast>().Show("Ваша комната в другом замке");
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RoomView.ItemsSource = null;
            RoomView.ItemsSource = User.CurrentUser.Favorites;
        }
    }
}