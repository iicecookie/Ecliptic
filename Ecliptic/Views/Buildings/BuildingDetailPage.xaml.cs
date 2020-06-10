using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;
using Ecliptic.Views.WayFounder;
using Ecliptic.Repository;
using System.Collections.Generic;
using Ecliptic.WebInteractions;
using Plugin.Connectivity;
using Android.InputMethodServices;
using Ecliptic.Views.ClientInteraction;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class BuildingDetailPage : ContentPage
    {
        public string Name
        {
            get { return Name; }
            set
            {
                Current = BuildingData.Buildings
                                   .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
                Title = "Здание " + Current.Name;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;
                stackLayout.Spacing = 10;

                Label NameLab = new Label
                {
                    Text = Current.Name,
                    TextColor = Color.Black,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                };
                stackLayout.Children.Add(NameLab);

                Label DescriptionLab = new Label
                {
                    Text = Current.Description,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                };
                stackLayout.Children.Add(DescriptionLab);

                Label AddreesLab = new Label
                {
                    Text = Current.Addrees,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                };
                stackLayout.Children.Add(AddreesLab);

                if (Current.TimeTable != null)
                {
                    Label TimeTableLab = new Label
                    {
                        Text = Current.TimeTable,
                        TextColor = Color.Black,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    };
                    stackLayout.Children.Add(TimeTableLab);
                }
                if (Current.Site      != null)
                {
                    Button SiteBut = new Button
                    {
                        Text = "Открыть сайт",
                        FontSize = 13,
                        FontAttributes = FontAttributes.Bold,
                        BorderColor = Color.Gray,
                        TextColor   = Color.Black,
                        BackgroundColor = Color.FromHex("#FFC7C7"),
                        HeightRequest = 40,
                    };
                    SiteBut.Clicked += clickSite;
                    stackLayout.Children.Add(SiteBut);
                }

                DownloadBut = new Button
                {
                    Text = "Загрузить это здание",
                    FontSize = 13,
                    FontAttributes = FontAttributes.Bold,
                    BorderColor = Color.Gray,
                    TextColor   = Color.Black,
                    BackgroundColor = Color.FromHex("#FFC7C7"),
                    HeightRequest = 40,
                };
                if (BuildingData.CurrentBuilding?.Name == Current.Name)
                    DownloadBut.Text = "Обновить";

                DownloadBut.Clicked += DownloadBut_Click;
                stackLayout.Children.Add(DownloadBut);

                this.Content = new ScrollView { Content = stackLayout };
            }
        }

        public Building Current { get; set; }

        public static bool BuildingLoad = false;
        
        Button DownloadBut;

        public BuildingDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void DownloadBut_Click(Object sender, EventArgs e)
        {
            if (BuildingLoad == false)
            {
                Load();
                BuildingPage.lastRequest = DateTime.Now;
            }
            else
            {
                DependencyService.Get<IToast>().Show("Дождитесь окончания загрузки");
            }
        }

        public async void Load()
        {
            BuildingLoad = true;
            WebData.CheckConnection();
          //  if (CrossConnectivity.Current.IsConnected == false)
          //  {
          //      DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
          //      return;
          //  }
          //  bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
          //  if (!isRemoteReachable)
          //  {
          //      await DisplayAlert("Сервер не доступен", "Повторите попытку позже", "OK"); return;
          //  }

            DependencyService.Get<IToast>().Show("Загрузка здания");

            FloorData.Floors   = new List<Floor>();
            RoomData .Rooms    = new List<Room>();
            WorkerData.Workers = new List<Worker>();
            PointData.Points     = new List<PointM>();
            PointData.RoomPoints = new List<PointM>();
            PointData.CurrentFloorRoomPoints = new List<PointM>();
            EdgeData .Edges = new List<EdgeM>();
            EdgeData .Ways  = new List<EdgeM>();
            EdgeData .CurrentFloorWalls = new List<EdgeM>();
            NoteData .Notes = new List<Note>();

            List<Floor>  floors  = await new FloorService().GetFloors(Current.BuildingId);
            List<Room>   rooms   = await new RoomService().GetRooms(Current.BuildingId);
            List<Worker> workers = await new WorkerService().GetWorkers(Current.BuildingId);
            List<PointM> points  = await new PointService().GetPoints(Current.BuildingId);
            List<EdgeM>  edges   = await new EdgeService().GetEdges(Current.BuildingId);
            List<Note>   notes   = await new NoteService().GetPublic(Current.BuildingId);
            List<Note>   clientnotes = null;

            // получить все публичные заметки не текущего пользователя
            if (Client.CurrentClient?.ClientId != null)
            {
                notes = notes.Where(n => n.ClientId != Client.CurrentClient.ClientId).ToList();
                clientnotes = await new NoteService().GetClient(Client.CurrentClient.ClientId);
            }
            foreach(var n in notes) { n.ClientId = null; }

            Authorization.RemoveNonLoadedRoomId(clientnotes, rooms);
           
            DbService.RemoveCurrentBuilding();

            try
            {
                DbService.AddFloor(floors);
                DbService.AddRoom(rooms);
                DbService.AddWorker(workers);
                DbService.AddPoing(points);
                DbService.AddEdge(edges);
                DbService.SaveDb();
                DbService.AddNote(notes);
                DbService.AddNote(clientnotes);
            }
            catch (System.Exception e)
            {
                if (e is Microsoft.EntityFrameworkCore.DbUpdateException ||
                    e is System.InvalidCastException)

                    // DbService.RemoveCurrentBuilding();
                    DependencyService.Get<IToast>().Show("Загруженые здания некорректны");
                BuildingLoad = false;
                return;
            }

            FloorData .Floors  = DbService.LoadAllFloors();
            RoomData  .Rooms   = DbService.LoadAllRooms();
            WorkerData.Workers = DbService.LoadAllWorkers();
            PointData .Points  = DbService.LoadAllPoints();
            EdgeData  .Edges   = DbService.LoadAllEdges();
            NoteData  .Notes   = DbService.LoadAllNotes();

            BuildingData.CurrentBuilding = Current;

            PointData.RoomPoints = PointData.Points
                                               .Where(p => p.IsWaypoint == true)
                                               .Where(c => c.Room != null).ToList();

            DependencyService.Get<IToast>().Show("Здание загружено");
            DownloadBut.Text = "Обновить";
            BuildingLoad = false;

        }

        void clickSite(object sender, EventArgs args)
        {
            try
            {
                new System.Threading.Thread(() =>
                {
                    Launcher.OpenAsync(new Uri(Current.Site));
                }).Start();
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>().Show("Не удалось открыть сайт " + e.Message);
            }
            
        }
    }
}
