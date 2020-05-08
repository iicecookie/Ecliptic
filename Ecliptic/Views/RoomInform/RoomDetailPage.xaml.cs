﻿using System;
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
        public string Name
        {
            get { return Name; }
            set
            {
                Current = RoomData.Rooms
                                   .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));

                Button button1 = null;
                Button button2 = null;
                Label label1 = null;
                Label label2 = null;
                Label label3 = null;
                Label label4 = null;
                Label label5 = null;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;

                if (Current.Name != null)
                {
                    label1 = new Label
                    {
                        Text = Current.Name,
                        Style = Device.Styles.TitleStyle,
                        //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        //     VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label1);
                }
                if (Current.Description != null)
                {
                    label2 = new Label
                    {
                        Text = Current.Description,
                        FontAttributes = FontAttributes.Italic,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label2);
                }
                if (Current.Details != null)
                {
                    label3 = new Label
                    {
                        Text = Current.Details,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Fill
                    };

                    stackLayout.Children.Add(label3);
                }
                if (Current == null)
                {
                    label4 = new Label
                    {
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label4);
                }
                if (Current.Phone != null)
                {
                    button1 = new Button
                    {
                        Text = "Call " + Current.Phone,
                    };
                    button1.Clicked += clickphone;

                    stackLayout.Children.Add(button1);
                }
                if (Current.Site != null)
                {
                    button2 = new Button
                    {
                        Text = "Open web site",
                    };
                    button2.Clicked += clickSite;

                    stackLayout.Children.Add(button2);
                }
                if (Current.Timetable != null)
                {
                    label5 = new Label
                    {
                        Text = Current.Timetable,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    stackLayout.Children.Add(label5);
                }

                if (Current.Workers.Count > 0)
                {
                    Label labeq = new Label
                    {
                        Text = "Работники:",
                        TextColor = Color.Black,
                        FontSize = 12,
                        Style = Device.Styles.TitleStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    stackLayout.Children.Add(labeq);

                    foreach (var i in Current.Workers)
                    {
                        Button buttpers = new Button
                        {
                            Text = i.ToString() ?? "wot",
                            FontSize = 12,
                            Style = Device.Styles.TitleStyle,
                        };
                        buttpers.Clicked += clickWorker;

                        stackLayout.Children.Add(buttpers);
                    }
                }

                // персональные заметки (только приватные)
                if (User.CurrentUser!=null)
                {
                    bool isNote = false;

                    foreach (var i in User.CurrentUser.Notes)
                    {
                        if (i.RoomId == Current.RoomId)// && i.isPublic == false)
                        {
                            if (isNote == false)
                                if (User.CurrentUser.Notes.Count != 0)
                                {
                                    Label labelN = new Label
                                    {
                                        Text = "Заметки " + User.CurrentUser.Name,
                                        Style = Device.Styles.BodyStyle,
                                        HorizontalOptions = LayoutOptions.Center
                                    };
                                    stackLayout.Children.Add(labelN);
                                    isNote = true;
                                }



                            Grid grid = new Grid
                            {
                                RowDefinitions =  {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                                       },
                                ColumnDefinitions =  {
                                     new ColumnDefinition { Width = new GridLength(160) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) }
                                                         }
                            };
                            grid.ColumnSpacing = 10;
                            grid.RowSpacing = 10;

                            Label noteLab = new Label
                            {
                                Text = i.Room.ToString() + " Аудитория ",
                                FontSize = 14,
                                Style = Device.Styles.TitleStyle,
                            };
                            Editor noteEnt = new Editor
                            {
                                AutoSize = EditorAutoSizeOption.TextChanges,
                                Text = i.Text.ToString() ?? "wot",
                                FontSize = 12,
                                Style = Device.Styles.BodyStyle,
                                AutomationId = i.NoteId.ToString(),
                            };

                            grid.Children.Add(noteLab, 0, 0);
                            grid.Children.Add(noteEnt, 0, 1);

                            Grid.SetColumnSpan(noteEnt, 4);

                            Frame frame = new Frame()
                            {
                                BorderColor = Color.ForestGreen,
                                AutomationId = i.NoteId.ToString(),
                            };

                            frame.Content = grid;

                            stackLayout.Children.Add(frame);
                        }
                    }

                    ToolbarItem item = new ToolbarItem();
                    item.Clicked += OnfaviriteClicked;
                    item.Order = ToolbarItemOrder.Default;
                    item.Priority = 1;
                    if (User.isRoomFavoit(Current)!=null)
                    {
                        item.IconImageSource = "@drawable/stared.png";
                    }
                    else
                    {
                        item.IconImageSource = "@drawable/unstared.png";
                    }
                    this.ToolbarItems.Add(item);
                }

                // публичные заметки
                if (NoteData.Notes.Count != 0)
                {
                    bool flag = true;

                    foreach (var i in NoteData.Notes)
                    {

                        if (i.RoomId == Current.RoomId && i.User == null)
                        {
                            if (flag)
                            {
                                Label labelON = new Label
                                {
                                    Text = "Общие заметки ",
                                    Style = Device.Styles.BodyStyle,
                                    HorizontalOptions = LayoutOptions.Center
                                };
                                stackLayout.Children.Add(labelON);
                                flag = false;
                            }


                            Grid grid = new Grid
                            {
                                RowDefinitions =  {
                                                         new RowDefinition { Height = new GridLength(30) },
                                                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                                                           },
                                ColumnDefinitions =  {
                                                         new ColumnDefinition { Width = new GridLength(160) },
                                                         new ColumnDefinition { Width = new GridLength(50) },
                                                         new ColumnDefinition { Width = new GridLength(30) },
                                                         new ColumnDefinition { Width = new GridLength(30) }
                                                                             }
                            };
                            grid.ColumnSpacing = 10;
                            grid.RowSpacing = 10;

                            Label noteLab = new Label
                            {
                                Text = i.Room.ToString() + " Аудитория ",
                                FontSize = 14,
                                Style = Device.Styles.TitleStyle,
                            };
                            Editor noteEnt = new Editor
                            {
                                AutoSize = EditorAutoSizeOption.TextChanges,
                                Text = i.Text.ToString() ?? "wot",
                                FontSize = 12,
                                Style = Device.Styles.BodyStyle,
                                AutomationId = i.NoteId.ToString(),
                            };

                            grid.Children.Add(noteLab, 0, 0);
                            grid.Children.Add(noteEnt, 0, 1);

                            Grid.SetColumnSpan(noteEnt, 4);

                            Frame frame = new Frame()
                            {
                                BorderColor = Color.ForestGreen,
                                AutomationId = i.NoteId.ToString(),
                            };

                            frame.Content = grid;

                            stackLayout.Children.Add(frame);
                        }
                    }
                }

                ScrollView scrollView = new ScrollView();
                scrollView.Content = stackLayout;

                this.Content = scrollView;
            }
        }

        public Room Current { get; set; }

        public RoomDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (User.CurrentUser != null)
            {
                if (User.isRoomFavoit(Current) != null)
                {
                    ToolbarItems.Last().IconImageSource = "@drawable/stared.png";
                }
                else
                {
                    ToolbarItems.Last().IconImageSource = "@drawable/unstared.png";
                }
            }
        }

        // Buttons on page
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

        // Star click
        async void OnfaviriteClicked(object sender, EventArgs args)
        {
            if (User.CurrentUser != null)
            {
                ToolbarItem item = (ToolbarItem)sender;

                var room = User.isRoomFavoit(Current);

                FavRoomService favRoomService = new FavRoomService();

                if (room != null)
                {
                    FavoriteRoom newroom = null;
                     
                    if (WebData.istest)
                    {
                        newroom = User.isRoomFavoit(Current);
                    }
                    else
                    {
                        newroom = await favRoomService.Add((Current.Clone() as Room)
                                                                   .ToFavRoom(User.CurrentUser.UserId));
                    }

                    if (newroom != null)
                    {
                        ToolbarItems.Last().IconImageSource = "@drawable/unstared.png";
                        User.CurrentUser.Favorites.Remove(newroom);
                    }
                }
                else
                {
                    FavoriteRoom newroom = null;

                    if (WebData.istest)
                    {
                        newroom = (Current.Clone() as Room).ToFavRoom(User.CurrentUser.UserId);
                    }
                    else
                    {
                        newroom = await favRoomService.Add((Current.Clone() as Room)
                                                                   .ToFavRoom(User.CurrentUser.UserId));
                    }

                    if (newroom != null)
                    {
                        User.CurrentUser.Favorites.Add(newroom);
                        ToolbarItems.Last().IconImageSource = "@drawable/stared.png";
                    }
                }

                DbService.SaveDb();
            }
        }

        // Toolbar
        async void OnButton1Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 1 been alerted", "OK");
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

        async void OnButton4Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 4 been alerted", "OK");
        }
    }
}
