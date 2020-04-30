using Ecliptic.Data;
using Ecliptic.Models;
using Ecliptic.Repository;
using Ecliptic.Views.UserNote;
using Ecliptic.Views.FavoriteRoomList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;


namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        static public class UserControls
        {
            static public Label labelMessage { get; set; }
            static public Label Login { get; set; }
            // static public Label Pass { get; set; }
            static public Label NoteCount { get; set; }
            static public Button LoginOutBtn { get; set; }

            static public List<Editor> Editors { get; set; }
            static public List<Switch> Switches { get; set; }
            static public List<Label>  Dates { get; set; }
        }

        public void GetUserPage()
        {
            Title = "Профиль " + User.CurrentUser.Name;

            #region CreateControls
            this.ToolbarItems.Clear();
            UserControls.labelMessage = new Label
            {
                Text = User.CurrentUser.Name,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Start
            };
            UserControls.Login = new Label
            {
                Text = User.CurrentUser.Login,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Start
            };

            UserControls.NoteCount = new Label
            {
                Text = "У вас " + User.CurrentUser.Notes.Count + " заметок и " + User.CurrentUser.Favorites.Count + " избраных помещений",
                Style = Device.Styles.ListItemTextStyle,
                HorizontalOptions = LayoutOptions.Start
            };
            UserControls.LoginOutBtn  = new Button
            {
                Text = "Login Out",
                BackgroundColor = Color.FromHex("#BFD9B6"),
                TextColor = Color.Black,
                BorderColor = Color.Black,
            };
            UserControls.Editors      = new List<Editor>();
            UserControls.Switches     = new List<Switch>();
            UserControls.Dates        = new List<Label>();
            UserControls.LoginOutBtn.Clicked += GoLoginPage;
            #endregion

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(UserControls.labelMessage);
            stackLayout.Children.Add(UserControls.Login);
            stackLayout.Children.Add(UserControls.NoteCount);

            foreach (var i in User.CurrentUser.Notes)
            {
                Grid grid = new Grid
                {
                    RowDefinitions = {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(20) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                        },
                    ColumnDefinitions = {
                                     new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) }
                        }

                };

                grid.ColumnSpacing = 10;
                grid.RowSpacing    = 10;

                Switch switche = new Switch
                {
                    IsToggled = i.isPublic,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    AutomationId = i.Id.ToString(),
                };
                Label  noteLab = new Label
                {
                    Text = i.Room.ToString(),
                    // hide in test
                    // FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Label  noteBui = new Label
                {
                    Text = "Здание "  + i.Building.ToString(),
                    // hide in test
                    // FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Label  date = new Label
                {
                    Text = " " + i.Date.ToString(),
                    // hide in test
                    // FontSize = 14,
                    AutomationId = i.Id.ToString(),
                    Style = Device.Styles.TitleStyle,
                };
                Editor noteEnt = new Editor
                {
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    Text = i.Text.ToString() ?? "wot",
                    // hide in test
                    // FontSize = 12,
                    Style = Device.Styles.BodyStyle,
                    AutomationId = i.Id.ToString(),
                };
                ImageButton SaveBtn = new ImageButton
                {
                    Source = "save.png",
                    AutomationId = i.Id.ToString(),
                }; 
                ImageButton DeleBtn = new ImageButton
                {
                    Source = "delete.png",
                    AutomationId = i.Id.ToString(),
                }; 

                UserControls.Dates.Add(date);
                UserControls.Editors.Add(noteEnt);
                UserControls.Switches.Add(switche);
                SaveBtn.Clicked += OnButtonSaveClicked;
                DeleBtn.Clicked += OnButtonDeleteClicked;
                switche.Toggled += OnSwitched;

                grid.Children.Add(noteLab, 0, 0);
                grid.Children.Add(switche, 1, 0);
                grid.Children.Add(SaveBtn, 2, 0);
                grid.Children.Add(DeleBtn, 3, 0);
                grid.Children.Add(date, 1, 1);
                Grid.SetColumnSpan(date, 2);
                grid.Children.Add(noteBui, 0, 1);
                grid.Children.Add(noteEnt, 0, 2);
                Grid.SetColumnSpan(noteEnt, 4);   // растягиваем на 4 столбца

                Frame frame = new Frame()
                {
                    BorderColor = Color.ForestGreen,
                    AutomationId = i.Id.ToString(),
                };

                frame.Content = grid;

                stackLayout.Children.Add(frame);
            }

            stackLayout.Children.Add(UserControls.LoginOutBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;
            this.Content = scrollView;

            #region ToolBarItems
            ToolbarItem NewNote = new ToolbarItem
            {
                Text = "Новая заметка",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0,
            };
            ToolbarItem FavRoom = new ToolbarItem
            {
                Text = "Избранное",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1,
            };
            ToolbarItem LogOut  = new ToolbarItem
            {
                Text = "Выйти",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1,
            };
            NewNote.Clicked += OnNewNoteClicked;
            FavRoom.Clicked += OnFavRoomsClicked;
            LogOut.Clicked += GoLoginPage;
            this.ToolbarItems.Add(NewNote);
            this.ToolbarItems.Add(FavRoom);
            this.ToolbarItems.Add(LogOut);
            #endregion
        }

        public void GoLoginPage(object sender, EventArgs args)
        {
            this.ToolbarItems.Clear();

            if (User.CurrentUser != null)
            {
                User.LoginOut();
            }

            GetLoginPage();
        }

        public void OnButtonSaveClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn.AutomationId == "0") { return; }

            // сохранить в пользователе 
            Note note = User.FindNoteById(Int32.Parse(btn.AutomationId));

            if (note == null) return;

            for (int i = 0; i < User.CurrentUser.Notes.Count; i++)
            {
                if (User.CurrentUser.Notes[i].Id == note.Id)
                {
                    note = User.CurrentUser.Notes[i];
                    break;
                }
            }

            foreach (var editor in UserControls.Editors)
            {
                if (editor.AutomationId == btn.AutomationId)
                {
                    note.Text = editor.Text;
                    break;
                }
            }

            foreach (var date   in UserControls.Dates)
            {
                if (date.AutomationId == btn.AutomationId)
                {
                    date.Text = DateTime.Today.ToString();
                    break;
                }
            }

            note.Date = DateTime.Today.ToString();

            DbService.UpdateNote(note);

            // hide in tests
            //  DependencyService.Get<IToast>().Show("Заметка о " + note.Room + " изменена");

            // отправить на сервер
         
        }

        public void OnButtonDeleteClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn.AutomationId == "0") { return; }

            // сохранить в пользователе 
            Note note = User.FindNoteById(Int32.Parse(btn.AutomationId));
            if  (note == null) return;

            // hide in tests
            // DependencyService.Get<IToast>().Show("Заметка о " + note?.Room + " удалена");


            if (note.isPublic)
            {
                //  если публичная, то убрать и с сервера

                // если заметка есть в загруженом здании
                Note buildnote = NoteData.FindNote((Note)note.Clone());

                if (buildnote != null)
                {
                    DbService.RemoveNote(buildnote);
                    NoteData.Notes = DbService.LoadAllPublicNotes();
                }
            }

            DbService.RemoveNote(note);

            GetUserPage();
        }

        public void OnSwitched(object sender, EventArgs args)
        {
            Switch switcher = (Switch)sender;
            if (switcher.AutomationId == "0") { return; }

            Note note = User.FindNoteById(Int32.Parse(switcher.AutomationId));
            if (note == null) return;

            if (switcher.IsToggled) // сделал публичной
            {
                note.isPublic = true;

                // отпрвыить на сервер

                // добавить в общий если добавилось на сервер

                Note buildnote = (Note)note.Clone();
                if (RoomData.isThatRoom(buildnote.Room))
                {
                    DbService.AddNote(buildnote);
                }

                NoteData.Notes = DbService.LoadAllPublicNotes();

                // hide in test
                // DependencyService.Get<IToast>().Show("Заметка о " + note.Room + " стала публичной");
            }
            else
            {
                // удалить с сервера

                // удалить из общиз если удалилось с сервера
                Note buildnote = NoteData.FindNote(note);
                if (buildnote != null)
                {
                    DbService.RemoveNote(NoteData.FindNote(note));
                }

                note.isPublic = false;

                DbService.SaveDb();

                NoteData.Notes = DbService.LoadAllPublicNotes();
                
                // hide in test
                // DependencyService.Get<IToast>().Show("Заметка о " + note.Room + " стала приватной");
            }
        }

        // Toolbar
        public async void OnNewNoteClicked (object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewNotePage("",""));
        }
        public async void OnFavRoomsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new FavRoomsPage());
        }
    }
}