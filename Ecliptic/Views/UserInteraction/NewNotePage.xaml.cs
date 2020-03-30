using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class NewNotePage : ContentPage
    {
        static public class NoteControls
        {
            static public Editor Text { get; set; }
            static public Entry Room { get; set; }
            static public Entry Building { get; set; }
            static public Switch isPublic { get; set; }
        }

        public NewNotePage()
        {
            InitializeComponent();
        }

        public NewNotePage(string room, string building) : this()
        {
            Title = "Новая заметка";

            #region CreateControls
            NoteControls.Text = new Editor
            {
                AutoSize = EditorAutoSizeOption.TextChanges,
                FontSize = 12,
                Style = Device.Styles.BodyStyle,
            };
            NoteControls.Room = new Entry
            {
                Text = room,
                Style = Device.Styles.TitleStyle,
            };
            NoteControls.Building = new Entry
            {
                Text = building,
                Style = Device.Styles.TitleStyle,
            };
            NoteControls.isPublic = new Switch
            {
                Style = Device.Styles.TitleStyle,
            };

            Label Ltext     = new Label
            {
                Text = "Текст заметки ",
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label Lroom     = new Label
            {
                Text = "О каком помещении",
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label Lbuilding = new Label
            {
                Text = "Для какого здания",
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label Lpublic   = new Label
            {
                Text = "Общедоступная?",
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };

            Button SaveBtn = new Button
            {
                Text = "Сохранить заметку",
            };
            SaveBtn.Clicked += OnButtonSaveClicked;
            #endregion

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(Ltext);
            stackLayout.Children.Add(NoteControls.Text);
            stackLayout.Children.Add(Lroom);
            stackLayout.Children.Add(NoteControls.Room);
            stackLayout.Children.Add(Lbuilding);
            stackLayout.Children.Add(NoteControls.Building);
            stackLayout.Children.Add(Lpublic);
            stackLayout.Children.Add(NoteControls.isPublic);
            stackLayout.Children.Add(SaveBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;
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

           
            User.CurrentUser.Notes[i] = temp;
            // отправить на сервер
        }
    }



}