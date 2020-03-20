using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        static public class UserControls
        {
            static public Label labelMessage { get; set; }
            static public Label Login { get; set; }
            static public Label Pass { get; set; }
            static public Button LoginOutBtn { get; set; }
        }

        List<Editor> Editors = new List<Editor>();

        public void GetUserPage()
        {
            Title = "Данные" + User.CurrentUser.Name;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            UserControls.labelMessage = new Label
            {
                Text = User.CurrentUser.Name,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.Login = new Label
            {
                Text = User.CurrentUser.Login,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.Pass = new Label
            {
                Text = User.CurrentUser.Password,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.LoginOutBtn = new Button
            {
                Text = "Login Out",
            };
            UserControls.LoginOutBtn.Clicked += GoLoginPage;

            stackLayout.Children.Add(UserControls.labelMessage);
            stackLayout.Children.Add(UserControls.Login);
            stackLayout.Children.Add(UserControls.Pass);
            if (User.CurrentUser != null)
            {
                foreach (var i in User.CurrentUser.Notes)
                {
                    Grid grid = new Grid
                    {
                        RowDefinitions = {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                        },
                        ColumnDefinitions = {
                                     new ColumnDefinition { Width = new GridLength(150) },
                                     new ColumnDefinition { Width = new GridLength(100) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) }
                        }
                    };

                    Switch switche = new Switch
                    {
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.CenterAndExpand,


                    };
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
                        AutomationId = i.Id
                    }; Editors.Add(noteEnt);
                    ImageButton SaveBtn = new ImageButton
                    {
                        Source = "icon.png",
                        AutomationId = i.Id
                    };
                    ImageButton DeleBtn = new ImageButton
                    {
                        Source = "icon.png",
                        AutomationId = i.Id
                    };

                    //  DisplayAlert("Alert", SaveBtn.AutomationId?.ToString(), "OK");
                    SaveBtn.Clicked += OnButtonSaveClicked;
                    DeleBtn.Clicked += OnButtonDeleteClicked;

                    grid.Children.Add(noteLab, 0, 0);
                    grid.Children.Add(switche, 1, 0);
                    grid.Children.Add(SaveBtn, 2, 0);
                    grid.Children.Add(DeleBtn, 3, 0);
                    grid.Children.Add(noteEnt, 0, 1);

                    Grid.SetColumnSpan(noteEnt, 4);   // растягиваем на 4 столбца

                    stackLayout.Children.Add(grid);
                }
            }
            stackLayout.Children.Add(UserControls.LoginOutBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;
        }
        void GoLoginPage(object sender, EventArgs args)
        {
            if (User.CurrentUser != null)
                User.CurrentUser = null;

            foreach (var i in RoomData.Roooms)
            {
                if (i.Notes.Count > 0)
                {
                    i.Notes = new List<Note>();
                }
            }

            GetLoginPage();
        }

        void OnButtonSaveClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;

            // сохранить в пользователе 
            Note temp = new Note();
            int i = 0;
            for (; i < User.CurrentUser.Notes.Count; i++)
            {
                if (User.CurrentUser.Notes[i].Id == btn.AutomationId)
                {
                    temp = User.CurrentUser.Notes[i];
                    break;
                }
            }

            foreach (var editor in Editors)
            {

                if (editor.AutomationId == btn.AutomationId)
                {
                    temp.Text = editor.Text;
                    break;
                }
            }
            User.CurrentUser.Notes[i] = temp;
            // отправить на сервер
            SendToDatabase(btn.AutomationId);
        }

        void SendToDatabase(string room)
        {

        }

        void OnButtonDeleteClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;

            User.DeleteNote(btn.AutomationId);

            GetUserPage();
        }
    }
}