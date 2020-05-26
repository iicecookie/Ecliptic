using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using Plugin.Connectivity;
using Ecliptic.WebInteractions;
using System.Collections.Generic;
using System.Linq;

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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BuildingData.CurrentBuilding != null)
            {
                BuildingTitle.Text = "Загруженое здание: " + BuildingData.CurrentBuilding.Name.ToString();
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

            DbService.RemoveBuildings();

            List<Building> buildings = await new BuildingService().GetBuildings();

            DbService.AddBuilding(buildings.Where(b => b.BuildingId != BuildingData.CurrentBuilding?.BuildingId).ToList());
            BuildingData.Buildings = DbService.LoadAllBuildings();

            BuildingView.ItemsSource = null;
            BuildingView.ItemsSource = BuildingData.Buildings;

            if (BuildingData.Buildings.Count == 0)
            {
                BuildingTitle.Text = "Нет доступных зданий";
            }
        }
    }
}