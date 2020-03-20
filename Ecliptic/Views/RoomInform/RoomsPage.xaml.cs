using System.Linq;
using Xamarin.Forms;
using Ecliptic.Models;
using System.Collections.ObjectModel;
using Ecliptic.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using Ecliptic.ViewModels;
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
            // BindingContext = new BearsViewModel();
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
                //Create new FoodGroups so we do not alter original list
                RoomGroup newGroup = new RoomGroup(group.Title, group.ShortName, group.Expanded);
                //Add the count of food items for Lits Header Titles to use
                newGroup.RoomCount = group.Count;
                if (group.Expanded)
                {
                    foreach (Room food in group)
                    {
                        newGroup.Add(food);
                    }
                }
                expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = expandedGroups;
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // string roomName = " wow";

            string roomName = (e.CurrentSelection as Room).Name;
            //roomName = (e.CurrentSelection.FirstOrDefault()).ToString();
            //bool answer = await DisplayAlert("Question?", "Would you like to play a game"+ roomName, "Yes", "No");

            // This works because route names are unique in this application.
            await Shell.Current.GoToAsync($"roomdetails?name={roomName}");
            //await Shell.Current.GoToAsync($"roomdetails?name={roomName}");
            // The full route is shown below.
            // await Shell.Current.GoToAsync($"//animals/monkeys/monkeydetails?name={monkeyName}");

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

        public bool Expanded
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
            get { return Expanded ? "expanded_blue.png" : "collapsed_blue.png"; }
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
            ObservableCollection<RoomGroup> Groups = new ObservableCollection<RoomGroup>();

            List<RoomGroup> Floors = new List<RoomGroup>();

            var rooms = RoomData.Roooms.GroupBy(room => room.Floor)
                                       .Select(group => group.First())
                                       .OrderBy(order=>order.Floor);

            foreach (var i in rooms)
                Floors.Add(new RoomGroup(i.Floor.ToString() + "Этаж", i.Floor.ToString()));

            // Floors.Add(new RoomGroup("1 Этаж", "1"));
            // Floors.Add(new RoomGroup("2 Этаж", "2"));
            // Floors.Add(new RoomGroup("3 Этаж", "3"));


            foreach (var i in RoomData.Roooms)
            {
                foreach (var r in Floors)
                {
                    if (i.Floor.ToString() == r.ShortName)
                    {
                        r.Add(i);
                    }
                }
            }

            // foreach (var i in RoomData.Roooms)
            // {
            //     if (i.Floor == 1)
            //         Floors[0].Add(i);
            //     if (i.Floor == 2)
            //         Floors[1].Add(i);
            //     if (i.Floor == 3)
            //         Floors[2].Add(i);
            //
            // }
            foreach (var r in Floors)
            {
                Groups.Add(r);
            }

           // Groups.Add(Floors[0]);
           // Groups.Add(Floors[1]);
           // Groups.Add(Floors[2]);

            All = Groups;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}