using System.Linq;
using Xamarin.Forms;
using Ecliptic.Models;
using System.Collections.ObjectModel;
using Ecliptic.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Ecliptic.Views
{
    public partial class FindWayPage : ContentPage
    {
        public FindWayPage()
        {
            InitializeComponent();
            stackBar1.HeightRequest = 1;
            stackBar2.HeightRequest = 1;
        }

        #region TextChanged
        void OnTextFromChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            if (searchBar1.Text == "")
            {
                searchResults1.ItemsSource = new List<Room>();
                stackBar1.HeightRequest = 1;
                return;
            }

            var searchedrooms = RoomData.Rooms
                      .Where(room => room.Name   .ToLower().Contains(searchBar1.Text.ToLower()) ||
                                     room.Details.ToLower().Contains(searchBar1.Text.ToLower()))
                      .ToList<Room>();

            stackBar1.HeightRequest    = searchedrooms.Count() * 50 + 25;
            searchResults1.ItemsSource = searchedrooms;
        }
        void OnTextToChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            if (searchBar2.Text == "")
            {
                searchResults2.ItemsSource = new List<Room>();
                stackBar2.HeightRequest = 1;
                return;
            }

            var searchedrooms = RoomData.Rooms
                      .Where(room => room.Name   .ToLower().Contains(searchBar2.Text.ToLower()) ||
                                     room.Details.ToLower().Contains(searchBar2.Text.ToLower()))
                      .ToList<Room>();

            stackBar2.HeightRequest    = searchedrooms.Count() * 50 + 25;
            searchResults2.ItemsSource = searchedrooms;

        }
        #endregion

        #region ItemTapped
        private void OnRoomFromTapped(object sender, ItemTappedEventArgs e)
        {
            Room room = (e.Item as Room);
            searchBar1.Text = room.Name;
            stackBar1.HeightRequest = 1;
            searchResults1.ItemsSource = new List<Room>();
        }

        private void OnRoomToTapped(object sender, ItemTappedEventArgs e)
        {
            Room room = (e.Item as Room);
            searchBar2.Text = room.Name;
            stackBar2.HeightRequest = 1;
            searchResults2.ItemsSource = new List<Room>();
        }
        #endregion
        
        private void Button_Clicked(object sender, EventArgs e)
        {
            // сначала проверить, есть ли записаные аудитории в базе данных
            // вывести предупреждение если нет

            // если все окей - производить поиск

        }
    }

}