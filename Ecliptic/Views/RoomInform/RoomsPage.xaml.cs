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
    public partial class RoomsPage : ContentPage
    {
        private ObservableCollection<RoomGroup> allGroups;
        private ObservableCollection<RoomGroup> expandedGroups;

        public RoomsPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            allGroups = RoomGroup.All;
            UpdateListContent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RoomGroup.UpdateList();
            allGroups = RoomGroup.All;
            UpdateListContent();
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = expandedGroups.IndexOf(
                ((RoomGroup)((Button)sender).CommandParameter));
            allGroups[selectedIndex].Expanded = !allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }

        private void UpdateListContent()
        {
            expandedGroups = new ObservableCollection<RoomGroup>();
            foreach (RoomGroup group in allGroups)
            {
                RoomGroup newGroup = new RoomGroup(group.Title, group.ShortName, group.Expanded);

                newGroup.RoomCount = group.Count;
                if (group.Expanded)
                {
                    foreach (Room room in group)
                    {
                        newGroup.Add(room);
                    }
                }
                expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = expandedGroups;
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string roomName = (e.CurrentSelection as Room).Name;

            await Shell.Current.GoToAsync($"roomdetails?name={roomName}");
        }
    }

    public class RoomGroup : ObservableCollection<Room>, INotifyPropertyChanged
    {
        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0}    ({1} помещений)", Title, RoomCount); }
        }

        public string ShortName { get; set; }

        public bool   Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "expanded_white.png" : "collapsed_white.png"; }
        }

        public int RoomCount { get; set; }

        public RoomGroup(string title, string shortName, bool expanded = true)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public static ObservableCollection<RoomGroup> All { private set; get; }

        static RoomGroup()
        {
            UpdateList();
        }

        public static void UpdateList()
        {
            ObservableCollection<RoomGroup> Groups = new ObservableCollection<RoomGroup>();

            List<RoomGroup> Floors = new List<RoomGroup>();

            var rooms = RoomData.Rooms.GroupBy(room => room.Floor.Level)
                                      .Select(group => group.First())
                                      .OrderBy(order => order.Floor.Level);

            foreach (var room in rooms)
            {
                Floors.Add(new RoomGroup(room.Floor.Level + " Этаж", room.Floor.Level.ToString()));
            }

            foreach (var room in RoomData.Rooms)
            {
                foreach (var floor in Floors)
                {
                    if (room.Floor.Level.ToString() == floor.ShortName)
                    {
                        floor.Add(room);
                    }
                }
            }

            foreach (var r in Floors)
            {
                Groups.Add(r);
            }

            All = Groups;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}