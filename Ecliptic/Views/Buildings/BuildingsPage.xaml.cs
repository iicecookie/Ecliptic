using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;
using Ecliptic.Repository;

namespace Ecliptic.Views
{
    public partial class BuildingPage : ContentPage
    {
        public BuildingPage()
        {
            InitializeComponent();

            Title = "Доступные зданиия";
        }

        private async void BuildingView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Building building = e.Item as Building;

            await Shell.Current.GoToAsync($"buildingdetails?name={building.Name}");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BuildingView.ItemsSource = null;
            BuildingView.ItemsSource = BuildingData.Buildings;
        }
    }
}
