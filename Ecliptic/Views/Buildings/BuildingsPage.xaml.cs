using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.Repository;
using Plugin.Connectivity;
using Ecliptic.WebInteractions;

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

        async protected override void OnAppearing()
        {
            base.OnAppearing();

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
            BuildingData.Buildings = DbService.LoadAllBuildings();

            DbService.AddBuilding(await new BuildingService().GetBuilding());

            BuildingView.ItemsSource = null;
            BuildingView.ItemsSource = BuildingData.Buildings;

            if (BuildingData.Buildings.Count == 0)
            {
                BuildingTitle.Text = "Нет доступных зданий";
            }
        }
    }
}