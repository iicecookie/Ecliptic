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
using System.Linq;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public class UserControls
        {
            public Label NameLab  { get; set; }
            public Label LoginLab { get; set; }

            public Label NoteCount { get; set; }

            public List<Editor> Editors  { get; set; }
            public List<Switch> Switches { get; set; }
            public List<Label>  Dates    { get; set; }

            public Button LoginOutBtn { get; set; }

            public UserControls()
            {
                NameLab   = new Label
                {
                    Text = User.CurrentUser.Name,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                LoginLab  = new Label
                {
                    Text = User.CurrentUser.Login,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                NoteCount = new Label
                {
                    Text = "У вас " + User.CurrentUser.Notes.Count + " заметок и " + User.CurrentUser.Favorites.Count + " избраных помещений",
                    Style = Device.Styles.ListItemTextStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                LoginOutBtn = new Button
                {
                    Text = "Login Out",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };

                Editors  = new List<Editor>();
                Switches = new List<Switch>();
                Dates    = new List<Label> ();
            }
        }

        public UserControls UserPage;

        public void GetUserPage()
        {
            Title = "Профиль " + User.CurrentUser.Name;

            LoginPage       = null;
            RegisrationPage = null;

            UserPage = new UserControls();
            UserPage.LoginOutBtn.Clicked += GoLoginPage;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(UserPage.NameLab);
            stackLayout.Children.Add(UserPage.LoginLab);
            stackLayout.Children.Add(UserPage.NoteCount);

            // заметки пользователя
            foreach (var note in User.CurrentUser.Notes)
            {
                Grid grid = new Grid
                {
                    RowDefinitions    =  {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(20) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } },
                    ColumnDefinitions =  {
                                     new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) } },
                    ColumnSpacing = 10,
                    RowSpacing = 10,
                };

                #region onenote
                Switch switchaccses = new Switch
                {
                    IsToggled = note.isPublic,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    AutomationId = note.NoteId.ToString(),
                };

                Label roomname     = new Label
                {
                    Text = note.RoomName.ToString(),
                    FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Label buildingname = new Label
                {
                    Text = note.Building != "" ? "Здание " + note.Building.ToString() : "",
                    FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Label datechanges  = new Label
                {
                    Text = " " + note.Date.ToString(),
                    FontSize = 14,
                    AutomationId = note.NoteId.ToString(),
                    Style = Device.Styles.TitleStyle,
                };

                Editor text = new Editor
                {
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    Text = note.Text.ToString() ?? "wot",
                    FontSize = 12,
                    Style = Device.Styles.BodyStyle,
                    AutomationId = note.NoteId.ToString(),
                };

                ImageButton SaveBut = new ImageButton
                {
                    Source = "save.png",
                    AutomationId = note.NoteId.ToString(),
                };
                ImageButton DeleBut = new ImageButton
                {
                    Source = "delete.png",
                    AutomationId = note.NoteId.ToString(),
                };

                UserPage.Switches.Add(switchaccses);
                UserPage.Dates   .Add(datechanges);
                UserPage.Editors .Add(text);

                switchaccses.Toggled += OnSwitched;
                SaveBut.Clicked      += OnButtonSaveClicked;
                DeleBut.Clicked      += OnButtonDeleteClicked;
                #endregion

                #region addingrid
                grid.Children.Add (roomname,     0, 0);
                grid.Children.Add (switchaccses, 1, 0);
                grid.Children.Add (SaveBut,      2, 0);
                grid.Children.Add (DeleBut,      3, 0);
                grid.Children.Add (datechanges,  1, 1);
                Grid.SetColumnSpan(datechanges,  2);
                grid.Children.Add (buildingname, 0, 1);
                grid.Children.Add (text, 0, 2);
                Grid.SetColumnSpan(text, 4);
                #endregion

                Frame frame = new Frame()
                {
                    BorderColor = Color.ForestGreen,
                    AutomationId = note.NoteId.ToString(),
                    Content = grid,
                };

                stackLayout.Children.Add(frame);
            }

            stackLayout.Children.Add(UserPage.LoginOutBtn);

            this.Content = new ScrollView { Content = stackLayout };

            #region ToolBarItems

            this.ToolbarItems.Clear();

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
            LogOut.Clicked  += GoLoginPage;

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
            UserPage = null;
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
                if (User.CurrentUser.Notes[i].NoteId == note.NoteId)
                {
                    note = User.CurrentUser.Notes[i];
                    break;
                }
            }

            foreach (var editor in UserPage.Editors)
            {
                if (editor.AutomationId == btn.AutomationId)
                {
                    note.Text = editor.Text;
                    break;
                }
            }

            foreach (var date   in UserPage.Dates)
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
            DependencyService.Get<IToast>().Show("Заметка о " + note?.RoomName + " изменена");

            // отправить на сервер
         
        }

        public void OnButtonDeleteClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn.AutomationId == "0") { return; }

            // сохранить в пользователе 
            Note note = User.FindNoteById(Int32.Parse(btn.AutomationId));
            if  (note == null) return;

            DependencyService.Get<IToast>().Show("Заметка о " + note?.RoomName + " удалена");


            List<Frame> v = ((StackLayout)((ScrollView)Content).Content).Children
                 .Where(x => x is Frame)
                 .Select(view => view as Frame)
                 .Where(f => f.AutomationId != btn.AutomationId)
                 .ToList();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;
            stackLayout.Children.Add(UserPage.NameLab);
            stackLayout.Children.Add(UserPage.LoginLab);
            stackLayout.Children.Add(UserPage.NoteCount);
            foreach (var frame in v)
            {
                stackLayout.Children.Add(frame);
            }
            stackLayout.Children.Add(UserPage.LoginOutBtn);

            ((ScrollView)Content).Content = stackLayout;

            if (note.isPublic)
            {
                //  если публичная, то убрать и с сервера

                // если заметка есть в загруженом здании
                // Note buildnote = NoteData.FindNote((Note)note.Clone());
                // 
                // if (buildnote != null)
                // {
                //     DbService.RemoveNote(buildnote);
                //     NoteData.Notes = DbService.LoadAllPublicNotes();
                // }
            }

            DbService.RemoveNote(note);

            // GetUserPage();
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

               // Note buildnote = (Note)note.Clone();
               // if (RoomData.isThatRoom(buildnote.Room))
               // {
               //     DbService.AddNote(buildnote);
               // }

                NoteData.Notes = DbService.LoadAllPublicNotes();

                // hide in test
                DependencyService.Get<IToast>().Show("Заметка о " + note?.RoomName + " стала публичной");
            }
            else
            {
                // удалить с сервера

                // удалить из общиз если удалилось с сервера
                Note buildnote = NoteData.FindNote(note);
                if  (buildnote != null)
                {
                    DbService.RemoveNote(NoteData.FindNote(note));
                }

                note.isPublic = false;

                DbService.SaveDb();

                NoteData.Notes = DbService.LoadAllPublicNotes();
                
                // hide in test
                DependencyService.Get<IToast>().Show("Заметка о " + note.Room + " стала приватной");
            }
        }

        // Toolbar
        public async void OnNewNoteClicked (object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewNotePage());
        }
        public async void OnFavRoomsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new FavoriteRoomsPage());
        }
    }
}