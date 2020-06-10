using Xamarin.Forms;
using System;
using System.Net.Http;
using Ecliptic.Models;
using Plugin.Connectivity;
using Newtonsoft.Json;
using Ecliptic.WebInteractions;
using Newtonsoft.Json.Linq;
using Ecliptic.Repository;
using System.Collections.Generic;
using System.Text;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using System.Net;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Ecliptic.Data;

namespace Ecliptic.Views.ClientInteraction
{
    public partial class Authorization : ContentPage
    {
        public class LoginControls
        {
            public Label TitleLab { get; set; }
            public Entry LoginBox { get; set; }
            public Entry PasswBox { get; set; }
            public Button LoginBtn { get; set; }
            public Button RegisBtn { get; set; }

            public LoginControls()
            {
                TitleLab = new Label
                {
                    Text = "Вход",
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center,
                };

                LoginBox = new Entry
                {
                    Text = "",
                    Placeholder = "Логин",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                PasswBox = new Entry
                {
                    Text = "",
                    Placeholder = "Пароль",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    IsPassword = true,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };

                LoginBtn = new Button
                {
                    Text = "Войти",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
                RegisBtn = new Button
                {
                    Text = "Зарегестрироваться",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
            }
        }

        private LoginControls LoginPage;

        public void GetLoginPage()
        {
            Title = "Войти";

            LoginPage = new LoginControls();

            LoginPage.LoginBtn.Clicked += LoginIn;
            LoginPage.RegisBtn.Clicked += ToRegistrationPage;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });
            stackLayout.Children.Add(LoginPage.TitleLab);
            stackLayout.Children.Add(LoginPage.LoginBox);
            stackLayout.Children.Add(LoginPage.PasswBox);
            stackLayout.Children.Add(LoginPage.LoginBtn);
            stackLayout.Children.Add(LoginPage.RegisBtn);
            stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });

            this.Content = stackLayout;
        }

        private bool isLoading = false;

        public async void LoginIn(object sender, EventArgs e)
        {
            if (LoginPage.LoginBox.Text == "" || LoginPage.PasswBox.Text == "")
            {
                DependencyService.Get<IToast>().Show("Введены не все поля");
                return;
            }
            if (isLoading)
            {
                DependencyService.Get<IToast>().Show("Пользователь уже загружается");
                return;
            }
            WebData.CheckConnection();
            // if (CrossConnectivity.Current.IsConnected == false)
            //  {
            //      DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
            //      return;
            //  }
            // bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
            // if (!isRemoteReachable)
            //  {
            //      await DisplayAlert("Сервер не доступен", "Повторите попытку позже", "OK"); return;
            //  }
            isLoading = true;

            var client = await new ClientService().Authrization(LoginPage.LoginBox.Text, LoginPage.PasswBox.Text);
            if (client != null) // если сервер вернул данные пользователя - загрузить в пользователя
            {
                Client.setClient(Int32.Parse(client["Id"]), client["Name"], client["Login"]);
                DbService.SaveClient(Client.CurrentClient); // сохранили пользователя

                List<Note> clientnotes = await new NoteService().GetClient(Client.CurrentClient.ClientId);
                RemoveDuplicateNote  (clientnotes);
                RemoveNonLoadedRoomId(clientnotes);

                DbService.AddNote(clientnotes);

                DbService.AddFavoriteRoom(await new FavRoomService().Get(Client.CurrentClient.ClientId));

                isLoading = false;
                GetClientPage();
                return;
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
                isLoading = false;
                return;
            }
        }

        public static void RemoveDuplicateNote  (List<Note> notes)
        {
            if (notes == null) return;
            if (DbService.LoadAllNotes().Count == 0 || notes.Count == 0) return;

            foreach (var note in DbService.LoadAllNotes())
            {
                foreach (var clientnote in notes)
                {
                    if (note.NoteId == clientnote.NoteId)
                    {
                        DbService.RemoveNote(note);
                    }
                }
            }
        }
        public static void RemoveNonLoadedRoomId(List<Note> notes, List<Room> rooms= null)
        {
            if (notes == null) return;
            if (notes.Count == 0) return;
            if (rooms == null) { rooms = DbService.LoadAllRooms(); }

            foreach (var note in notes)
            {
                // получаю комнату, о которой эта заметка
                var room = rooms.FirstOrDefault(n => n.RoomId == note.RoomId);
                // если такой нет - зануляю инедкс
                if (room == null)
                    note.RoomId = null;
            }
        }

        private void ToRegistrationPage(object sender, EventArgs e)
        {
            if (isLoading)
            {
                DependencyService.Get<IToast>().Show("Дождитесь окончания загрузки");
                return;
            }
            GetRegistrationPage();
        }
    }
}
