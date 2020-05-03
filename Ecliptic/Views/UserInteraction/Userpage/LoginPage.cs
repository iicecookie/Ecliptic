using Ecliptic.Models;
using Ecliptic.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Net;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public class LoginControls
        {
            public Label labelMessage { get; set; }
            public Entry LoginBox { get; set; }
            public Entry PasswBox { get; set; }
            public Button LoginBtn { get; set; }
            public Button RegisBtn { get; set; }

            public LoginControls()
            {
                labelMessage = new Label
                {
                    Text = "Вход",
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center
                };

                LoginBox = new Entry
                {
                    Text = "",
                    Placeholder = "Имя",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    // FontAttributes = FontAttributes.Italic,
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

            stackLayout.Children.Add(LoginPage.labelMessage);
            stackLayout.Children.Add(LoginPage.LoginBox);
            stackLayout.Children.Add(LoginPage.PasswBox);
            stackLayout.Children.Add(LoginPage.LoginBtn);
            stackLayout.Children.Add(LoginPage.RegisBtn);

            this.Content = new ScrollView { Content = stackLayout };
        }

        public async void LoginIn(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return; 
            }

            if (LoginPage.LoginBox.Text == "" || LoginPage.PasswBox.Text == "")
            {
                DependencyService.Get<IToast>().Show("Введены не все поля");
                return;
            }

            /*
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri("http://somesite.com");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync();
            }
            */

            // проверить есть ли на сервере пользователь
            // с заданным логином и паролем
            // если все ОКе - загрузить его 
            if (User.CheckUser(LoginPage.LoginBox.Text, LoginPage.PasswBox.Text))
            {
                // загружаем данные в User
                User.LoadUser(LoginPage.LoginBox.Text, LoginPage.PasswBox.Text);

                // открываем страницу с данными
                GetUserPage();
            }
            else
            {
                await DisplayAlert("Alert", "Такого пользователя не существует", "OK");
            }
        }

        private void ToRegistrationPage(object sender, EventArgs e)
        {
            GetRegistrationPage();
        }
    }
}
