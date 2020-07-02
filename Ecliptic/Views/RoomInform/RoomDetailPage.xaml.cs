using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;
using Ecliptic.Views.WayFounder;
using Ecliptic.Repository;
using Ecliptic.WebInteractions;
using Ecliptic.Views.RoomInform;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class RoomDetailPage : ContentPage
    {
        public Room Current { get; set; } // текущее открытое помещение
        public string Name
        {
            get { return Name; }
            set
            {
                Current = RoomData.Rooms.FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
                Title = Current.Name + "     " + Current.Floor.Level + " Этаж";
                SetComponents(); // отрисовать все компоненты
            }
        }

        public RoomDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Client.CurrentClient != null)
            {
                foreach (var item in this.ToolbarItems)
                {
                    if (item.Text == "fav")
                    {
                        if (Client.isRoomFavoit(Current) != null)
                        {
                            item.IconImageSource = "@drawable/stared.png";
                        }
                        else
                        {
                            item.IconImageSource = "@drawable/unstared.png";
                        }
                        break;
                    }
                }
            }
        }

        private void SetComponents()
        {
            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;
            Color backcolor = Color.FromHex("#FAFAFA");

            if (Current.Name != null)
            {
                Label Namelab = new Label
                {
                    Text = Current.Name,
                    Style = Device.Styles.TitleStyle,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center
                };

                stackLayout.Children.Add(Namelab);
            }
            if (Current.Description != null)
            {
                Label Descrlab = new Label
                {
                    Text = Current.Description,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Italic,
                    HorizontalOptions = LayoutOptions.Center
                };

                stackLayout.Children.Add(Descrlab);
            }
            if (Current.Phone != null)
            {
                Button Phonebut = new Button
                {
                    Text = "Позвонить " + Current.Phone,
                    HeightRequest = 40,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    Style = Device.Styles.BodyStyle,
                    TextColor = Color.Black,
                    BackgroundColor = backcolor,
                };
                Phonebut.Clicked += clickphone;

                stackLayout.Children.Add(Phonebut);
            }
            if (Current.Site != null)
            {
                Button Sitebut = new Button
                {
                    Text = "Открыть сайт",
                    HeightRequest = 40,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    Style = Device.Styles.BodyStyle,
                    TextColor = Color.Black,
                    BackgroundColor = backcolor,
                };
                Sitebut.Clicked += clickSite;

                stackLayout.Children.Add(Sitebut);
            }
            if (Current.Timetable != null)
            {
                Label Timelab = new Label
                {
                    Text = Current.Timetable,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Start
                };

                stackLayout.Children.Add(Timelab);
            }

            // работники помещения
            if (Current.Workers.Count > 0)
            {
                Label Workerslab = new Label
                {
                    Text = "Ответственные лица:",
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Center
                };

                stackLayout.Children.Add(Workerslab);

                foreach (var i in Current.Workers)
                {
                    Button Workerbut = new Button
                    {
                        Text = i.ToString(),
                        TextColor = Color.Black,
                        BackgroundColor = backcolor,
                        VerticalOptions = LayoutOptions.Center,
                        Style = Device.Styles.BodyStyle,
                        FontSize = 12,
                        HeightRequest = 40,
                    };
                    Workerbut.Clicked += clickWorker;

                    stackLayout.Children.Add(Workerbut);
                }
            }

            // заметки пользователя
            if (Client.CurrentClient != null)
            {
                bool isClientNotes = false; // если есть заметки клиента

                foreach (var i in Client.CurrentClient.Notes)
                {
                    if (i.RoomId == Current.RoomId)
                    {
                        if (!isClientNotes)
                        {
                            Label Notelab = new Label
                            {
                                Text = "Заметки " + Client.CurrentClient.Name + ":",
                                Style = Device.Styles.BodyStyle,
                                HorizontalOptions = LayoutOptions.Center
                            };
                            stackLayout.Children.Add(Notelab);
                            isClientNotes = true;
                        }

                        stackLayout.Children.Add(PaintNote(i, false));
                    }
                }

                SetStarBar(); // добавить контроллер избраности
            }

            // публичные заметки
            if (Current.Notes.Count != 0)
            {
                bool isOneNoClients = true; // если есть заметки о помещении. не текущего клиента - публичные

                foreach (var i in Current.Notes)
                {
                    if (i.Client != null) { break; }

                    if (isOneNoClients)
                    {
                        Label PublicNotelab = new Label
                        {
                            Text = "Общие заметки:",
                            Style = Device.Styles.BodyStyle,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        stackLayout.Children.Add(PublicNotelab);
                        isOneNoClients = false;
                    }

                    stackLayout.Children.Add(PaintNote(i, true));
                }
            }

            this.Content = new ScrollView { Content = stackLayout };
        }

        private Frame PaintNote(Note note, bool ispublic)
        {
            Label Author = new Label
            {
                Text = "Добавил: " + note.Client?.Name.ToString(),
                TextColor = Color.Black,
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            
            if (ispublic)
                Author.Text += note.ClientName == "" ? "Администратор" : note.ClientName;
            else
                Author.Text += note.isPublic ? " (Публиная)" : "";

            Label Data = new Label
            {
                Text = "в: " + string.Join("", note.Date.Take(8).ToArray()),
                TextColor = Color.Black,
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label Text = new Label
            {
                Text = note.Text.ToString() ?? "wot",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Black,
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
                AutomationId = note.NoteId.ToString(),
            };

            Grid grid = new Grid
            {
                RowDefinitions =     { new RowDefinition {   Height = new GridLength(30) },
                                       new RowDefinition {   Height = new GridLength(1, GridUnitType.Star) } },

                ColumnDefinitions =  { new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Star) },
                                       new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Auto) }, },
                ColumnSpacing = 10,
                RowSpacing = 10,
            };
            grid.Children.Add (Author, 0, 0);
            grid.Children.Add (Data, 1, 0);
            grid.Children.Add (Text, 0, 1);
            Grid.SetColumnSpan(Text, 2);

            return new Frame()
            {
                BorderColor = Color.FromHex("#04006A"),
                BackgroundColor = Color.FromHex("#FAFAFA"),
                AutomationId = note.NoteId.ToString(),
                Content = grid,
            };
        }

        private void SetStarBar()
        {
            ToolbarItem item = new ToolbarItem();
            item.Text = "fav";
            item.Clicked += OnfaviriteClicked;
            item.Order = ToolbarItemOrder.Default;
            item.Priority = 1;

            if (Client.isRoomFavoit(Current) != null)
            {
                item.IconImageSource = "@drawable/stared.png";
            }
            else
            {
                item.IconImageSource = "@drawable/unstared.png";
            }
            this.ToolbarItems.Add(item);
        }

        #region Buttons

        void clickphone(object sender, EventArgs args)
        {
            Essentials.CallPhone(Current.Phone);
        }
        void clickSite (object sender, EventArgs args)
        {
            Essentials.OpenSite(Current.Site);
        }

        async void clickWorker(object sender, EventArgs args)
        {
            Button btn = (Button)sender;

            string[] words = btn.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            await Navigation.PushAsync(new WorkerDetailPage(words));
        }
        #endregion

        #region Toolbar

        async void OnfaviriteClicked(object sender, EventArgs args)
        {
            bool connect = await WebData.CheckConnection(); // проверка подключения
            if  (connect == false) return;

            ToolbarItem item = (ToolbarItem)sender;

            FavoriteRoom room = Client.isRoomFavoit(Current);
            if (room == null) // добавление
            {
                FavoriteRoom newroom = await new FavRoomService().Add((Current.Clone() as Room).ToFavRoom(Client.CurrentClient.ClientId));
                if (newroom != null)
                {
                    DbService.AddFavoriteRoom(newroom);
                    item.IconImageSource = "@drawable/stared.png";
                }
            }
            else // удаление 
            {
                bool deleted = await new FavRoomService().Delete(room.FavoriteRoomId);
                if  (deleted)
                {
                    DbService.RemoveFavoriteRoom(room);
                    item.IconImageSource = "@drawable/unstared.png";
                }
            }
        }

        void OnButton2Clicked(object sender, EventArgs args)
        {
            Way.Begin = Current;

            DependencyService.Get<IToast>().Show("Начало маршрута установлео");
        }

        void OnButton3Clicked(object sender, EventArgs args)
        {
            Way.End = Current;

            DependencyService.Get<IToast>().Show("Конец маршрута установлен");
        }

        // показать помещение на карте
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
            Navigation.PushAsync(new SchemePlanPage(Current));
        }
        #endregion
    }
}
