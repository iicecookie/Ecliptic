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

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class RoomDetailPage : ContentPage
    {
        public Room Current { get; set; }
        public string Name
        {
            get { return Name; }
            set
            {
                Current = RoomData.Rooms
                                   .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));

                Title = Current.Name + "     " + Current.Floor.Level + " Этаж";

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;
                Color backcolor = Color.FromHex("#FAFAFA");

                if (Current.Name      != null)
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
                if (Current.Phone     != null)
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
                if (Current.Site      != null)
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
                    bool isone = false;
                    foreach (var i in Client.CurrentClient.Notes)
                    {
                        if (i.RoomId == Current.RoomId) 
                        {
                            if (!isone)
                            {
                                Label Notelab = new Label
                                {
                                    Text = "Заметки " + Client.CurrentClient.Name + ":",
                                    Style = Device.Styles.BodyStyle,
                                    HorizontalOptions = LayoutOptions.Center
                                };
                                stackLayout.Children.Add(Notelab);
                                isone = true;
                            }

                            Grid grid = new Grid
                            {
                                RowDefinitions =     { new RowDefinition {   Height = new GridLength(30) },
                                                       new RowDefinition {   Height = new GridLength(1, GridUnitType.Star) } },

                                ColumnDefinitions =  { new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Star) },
                                                       new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Auto) }, },
                                ColumnSpacing = 10,
                                RowSpacing = 10,
                            };

                            // возможно здание будет заменено на пользователя
                            Label Clientlab = new Label
                            {
                                Text = "Добавил: " + i.Client.Name.ToString(),
                                TextColor = Color.Black,
                                FontSize = 14,
                                Style = Device.Styles.TitleStyle,
                            };
                            Label Datalab = new Label
                            {
                                Text = "в: " + string.Join("", i.Date.Take(8).ToArray()),
                                TextColor = Color.Black,
                                FontSize = 14,
                                Style = Device.Styles.TitleStyle,
                            };
                            Label Textlab = new Label
                            {
                                Text = i.Text.ToString() ?? "wot",
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                TextColor = Color.Black,
                                FontSize = 14,
                                Style = Device.Styles.TitleStyle,
                                AutomationId = i.NoteId.ToString(),
                            };

                            grid.Children.Add(Clientlab, 0, 0);
                            grid.Children.Add(Datalab, 1, 0);
                            grid.Children.Add(Textlab, 0, 1);
                            Grid.SetColumnSpan(Textlab, 2);

                            Frame frame = new Frame()
                            {
                                BorderColor = Color.FromHex("#04006A"),
                                BackgroundColor = backcolor,
                                AutomationId = i.NoteId.ToString(),
                                Content = grid,
                            };

                            stackLayout.Children.Add(frame);
                        }
                    }

                    #region toolbarStar
                    ToolbarItem item = new ToolbarItem();
                    item.Text = "fav";
                    item.Clicked += OnfaviriteClicked;
                    item.Order    = ToolbarItemOrder.Default;
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
                    #endregion
                }

                // публичные заметки
                if (Current.Notes.Count != 0)
                {
                    Label PublicNotelab = new Label
                    {
                        Text = "Общие заметки:",
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    stackLayout.Children.Add(PublicNotelab);

                    foreach (var i in Current.Notes)
                    {
                        // if (i.RoomId == Current.RoomId// && i.Client == null
                        //     )
                        // {
                        Grid grid = new Grid
                        {
                            RowDefinitions =     { new RowDefinition {   Height = new GridLength(30) },
                                                       new RowDefinition {   Height = new GridLength(1, GridUnitType.Star) } },

                            ColumnDefinitions =  { new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Star) },
                                                       new ColumnDefinition { Width =  new GridLength(1, GridUnitType.Auto) }, },
                            ColumnSpacing = 10,
                            RowSpacing = 10,
                        };

                        // возможно здание будет заменено на пользователя
                        Label Clientlab = new Label
                        {
                            Text = "Добавил: ",
                            TextColor = Color.Black,
                            FontSize = 14,
                            Style = Device.Styles.TitleStyle,
                        };

                        Clientlab.Text += i.ClientName == "" ? "Администратор" : i.ClientName;

                        Label Datalab = new Label
                        {
                            Text = "в: " + string.Join("", i.Date.Take(8).ToArray()),
                            TextColor = Color.Black,
                            FontSize = 14,
                            Style = Device.Styles.TitleStyle,
                        };
                        Label Textlab = new Label
                        {
                            Text = i.Text.ToString() ?? "wot",
                            TextColor = Color.Black,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            FontSize = 14,
                            Style = Device.Styles.TitleStyle,
                            AutomationId = i.NoteId.ToString(),
                        };

                        grid.Children.Add(Clientlab, 0, 0);
                        grid.Children.Add(Datalab, 1, 0);
                        grid.Children.Add(Textlab, 0, 1);
                        Grid.SetColumnSpan(Textlab, 2);

                        Frame frame = new Frame()
                        {
                            BorderColor = Color.FromHex("#04006A"),
                            BackgroundColor = backcolor,
                            AutomationId = i.NoteId.ToString(),
                            Content = grid,
                        };

                        stackLayout.Children.Add(frame);
                    }
                    //  }
                }

                this.Content = new ScrollView { Content = stackLayout };
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
                foreach(var item in this.ToolbarItems)
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

        // protected override bool OnBackButtonPressed() { Navigation.PopAsync(false); return true; }

        #region Buttons

        void clickphone(object sender, EventArgs args)
        {
            try
            {
                PhoneDialer.Open(Current.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                DependencyService.Get<IToast>().Show("Неверный номер " + anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                DependencyService.Get<IToast>().Show("Не потдерживается на вашем телефоне" + ex.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Неизвесная ошибка "+ ex.Message);
            }
        }

        void clickSite(object sender, EventArgs args)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(Current.Site));
            }).Start();
        }

        async void clickWorker(object sender, EventArgs args)
        {
            Button btn = (Button)sender;

            string[] words = btn.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            await Navigation.PushAsync(new WorkerDetailPage(words));
        }

        #endregion

        async void OnfaviriteClicked(object sender, EventArgs args)
        {
            bool connect = await WebData.CheckConnection();
            if (connect == false) return;

            if (Client.CurrentClient != null)
            {
                FavoriteRoom room = Client.isRoomFavoit(Current);
                foreach (var item in this.ToolbarItems)
                {
                    if (item.Text == "fav")
                    {
                        if (room == null) // add
                        {
                            FavoriteRoom newroom = await new FavRoomService().Add((Current.Clone() as Room).ToFavRoom(Client.CurrentClient.ClientId));

                            if (newroom != null)
                            {
                                DbService.AddFavoriteRoom(newroom);
                                item.IconImageSource = "@drawable/stared.png";
                            }
                        }
                        else // remove
                        {
                            bool deleted = await new FavRoomService().Delete(room.FavoriteRoomId);

                            if (deleted)
                            {
                                DbService.RemoveFavoriteRoom(room);
                                item.IconImageSource = "@drawable/unstared.png";
                            }
                        }
                    }
                }
            }
        }

        #region Toolbar

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

        #endregion

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            // Shell.Current.GoToAsync($"buildingdetails?name={Current.Name}");
            // Navigation.PopAsync();

            Navigation.PopToRootAsync();
            Navigation.PushAsync(new SchemePlanPage(Current));
        }
    }
}
