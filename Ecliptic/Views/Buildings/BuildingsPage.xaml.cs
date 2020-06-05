using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using Plugin.Connectivity;
using Ecliptic.WebInteractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Ecliptic.Views
{
    public partial class BuildingPage : ContentPage
    {
        public BuildingPage()
        {
            InitializeComponent();
            Title = "Доступные зданиия";
            lastRequest = DateTime.Now.AddMinutes(-1);
        }

        private async void BuildingView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Building building = e.Item as Building;

            await Shell.Current.GoToAsync($"buildingdetails?name={building.Name}");
        }

        public static DateTime lastRequest;

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

            if ((DateTime.Now - lastRequest).TotalSeconds > 20)
            {
                lastRequest = DateTime.Now;
                DependencyService.Get<IToast>().Show("Обновление списка зданий");
                BuildingDetailPage.BuildingLoad = true;
                BuildingView.ItemsSource = null;

                await LoadBuildingsAsync();

                lastRequest = DateTime.Now;
                BuildingDetailPage.BuildingLoad = false;
                DependencyService.Get<IToast>().Show("Список обновлен");
            }

            BuildingView.ItemsSource = BuildingData.Buildings;

            if (BuildingData.CurrentBuilding != null)
            {
                BuildingTitle.Text = "Загруженое здание: " + BuildingData.CurrentBuilding.Name.ToString();
            }
            if (BuildingData.Buildings.Count == 0)
            {
                BuildingTitle.Text = "Нет доступных зданий";
            }
        }

        public static async Task<List<Building>> LoadBuildingsAsync()
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                return null;
            }
            bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
            if (!isRemoteReachable)
            {
                return null;
            }

            DbService.RemoveUncurrentBuildings();
            BuildingData.Buildings = new List<Building>();

            List<Building> buildings = await new BuildingService().GetBuildings();
            DbService.AddBuilding(buildings.Where(b => b.BuildingId != BuildingData.CurrentBuilding?.BuildingId).ToList());

            var currentloadedbuilding = buildings.Where(b => b.BuildingId == BuildingData.CurrentBuilding?.BuildingId).FirstOrDefault();
            if (currentloadedbuilding != null)
            {
                BuildingData.CurrentBuilding.Name = currentloadedbuilding.Name;
                BuildingData.CurrentBuilding.Site = currentloadedbuilding.Site;
                BuildingData.CurrentBuilding.Addrees = currentloadedbuilding.Addrees;
                BuildingData.CurrentBuilding.TimeTable = currentloadedbuilding.TimeTable;
                BuildingData.CurrentBuilding.Description = currentloadedbuilding.Description;
                DbService.SaveDb();
            }

            BuildingData.Buildings = DbService.LoadAllBuildings();

            BuildingData.CurrentBuilding = BuildingData.Buildings
                                             .Where(b => b.BuildingId == currentloadedbuilding?.BuildingId)
                                             .FirstOrDefault();

            return BuildingData.Buildings;
        }
    }
}