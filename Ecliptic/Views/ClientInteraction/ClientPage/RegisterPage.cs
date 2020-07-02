using Ecliptic.Models;
using Ecliptic.Repository;
using Ecliptic.WebInteractions;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace Ecliptic.Views.ClientInteraction
{
    public partial class Authorization : ContentPage
    {
        public class RegisrationContrioolers
        {
            public Label labelMessage  { get; set; }

            public Entry NameBox       { get; set; }
            public Entry LoginBox      { get; set; }
            public Entry PasswBox      { get; set; }
            public Entry PasswCheckBox { get; set; }

            public Button RegisBtn     { get; set; }
            public Button LoginBtn     { get; set; }

            public RegisrationContrioolers()
            {
                labelMessage  = new Label
                {
                    Text = "Заполните поля",
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center
                };

                NameBox       = new Entry
                {
                    Text = "",
                    Placeholder = "Имя пользователя",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                LoginBox      = new Entry
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
                PasswBox      = new Entry
                {
                    Text = "",
                    Placeholder = "Пароль",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                PasswCheckBox = new Entry
                {
                    Text = "",
                    Placeholder = "Подтверждение пароля",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    IsPassword = true,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };

                RegisBtn = new Button
                {
                    Text = "Зарегестрироваться",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
                LoginBtn = new Button
                {
                    Text = "Уже есть учетная запись",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
            }

            public ScrollView SetContent()
            {
                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;

                stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });
                stackLayout.Children.Add(labelMessage);
                stackLayout.Children.Add(NameBox);
                stackLayout.Children.Add(LoginBox);
                stackLayout.Children.Add(PasswBox);
                stackLayout.Children.Add(PasswCheckBox);
                stackLayout.Children.Add(RegisBtn);
                stackLayout.Children.Add(LoginBtn);
                stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });

                ScrollView scroll = new ScrollView { Content = stackLayout };
                return scroll;
            }
        }

        public RegisrationContrioolers RegisrationPage;

        public void GetRegistrationPage()
        {
            Title = "Зарегистрироваться";

            RegisrationPage = new RegisrationContrioolers();
            RegisrationPage.SetContent();
            RegisrationPage.RegisBtn.Clicked += RegistrClient;
            RegisrationPage.LoginBtn.Clicked += ToLoginPage;

            this.Content = new ScrollView { Content = RegisrationPage.SetContent() };
        }

        public async void RegistrClient(object sender, EventArgs e)
        {
            if (RegisrationPage.NameBox.Text  == "" || RegisrationPage.LoginBox.Text == "" ||
                RegisrationPage.PasswBox.Text == "" || RegisrationPage.PasswCheckBox.Text == "")
            {
                DependencyService.Get<IToast>().Show("Не все поля заполнены"); return;
            }

            if (RegisrationPage.PasswBox.Text != RegisrationPage.PasswCheckBox.Text)
            {
                DependencyService.Get<IToast>().Show("Пароли не совпадают");   return;
            }

            bool connect = await WebData.CheckConnection();
            if  (connect == false) return;

            ClientService clientService = new ClientService();

            // отправка данных регистрации на сервер    
            var client = await clientService.
                Register(RegisrationPage.NameBox.Text, RegisrationPage.LoginBox.Text, RegisrationPage.PasswBox.Text);


            // если сервер вернул данные пользователя - загрузить в пользователя
            if (client != null)
            {
                Client.setClient(Int32.Parse(client["Id"]), client["Name"], client["Login"]);
                DbService.SaveClient(Client.CurrentClient); // сохранили пользователя
                                                          // DbService.LoadUser();

                GetClientPage();
                return;
            }
            else
            {
                await DisplayAlert("Ошибка", "Сервер не вернул данные", "OK");
                return;
            }
        }

        private void ToLoginPage(object sender, EventArgs e)
        {
            GetLoginPage();
        }
    }
}
