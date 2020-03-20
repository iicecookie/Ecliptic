using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        static class LoginControls
        {
            static public Label labelMessage { get; set; }
            static public Entry LoginBox { get; set; }
            static public Entry PasswBox { get; set; }
            static public Button LoginBtn { get; set; }
            static public Button RegisBtn { get; set; }
        }

        public void GetLoginPage()
        {
            Title = "Войти";

            LoginControls.labelMessage = new Label
            {
                Text = "Вход",
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            LoginControls.LoginBox = new Entry
            {
                Text = "",
                Placeholder = "Login",
                Keyboard = Keyboard.Default,
                TextColor = Color.Black,
                PlaceholderColor = Color.Black,


                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                FontAttributes = FontAttributes.Italic,
                Style = Device.Styles.BodyStyle,
                HorizontalOptions = LayoutOptions.Fill
            };
            LoginControls.PasswBox = new Entry
            {
                Text = "",
                Placeholder = "Password",
                Keyboard = Keyboard.Default,
                TextColor = Color.Black,
                PlaceholderColor = Color.Black,
                IsPassword = true,
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                Style = Device.Styles.BodyStyle,
                HorizontalOptions = LayoutOptions.Fill
            };
            LoginControls.LoginBtn = new Button
            {
                Text = "Login in",
            };
            LoginControls.LoginBtn.Clicked += LoginIn;
            LoginControls.RegisBtn = new Button
            {
                Text = "Registration",
            };
            LoginControls.RegisBtn.Clicked += ToRegistrationPage;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(LoginControls.labelMessage);
            stackLayout.Children.Add(LoginControls.LoginBox);
            stackLayout.Children.Add(LoginControls.PasswBox);
            stackLayout.Children.Add(LoginControls.LoginBtn);
            stackLayout.Children.Add(LoginControls.RegisBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;
            this.Content = scrollView;
        }

        private async void LoginIn(object sender, EventArgs e)
        {
            if (LoginControls.LoginBox.Text == "" || LoginControls.PasswBox.Text == "")
            {
                // WrongData();
                await DisplayAlert("Опупка", "Заполните поля", "OK");
                return;
            }
            // проверить есть ли на сервере пользователь
            // с заданным логином и паролем
            // если все ОКе, 
            if (User.CheckUser(LoginControls.LoginBox.Text, LoginControls.PasswBox.Text))
            {
                // загружаем данные в User
                User.LoadUser(LoginControls.LoginBox.Text, LoginControls.PasswBox.Text);
                // открываем страницу с данными
                GetUserPage();
            }
            // иначе
            else
            {
                await DisplayAlert("Alert", "Такого пользователя не существует", "OK");
                //WrongData();
            }
        }

        private void ToRegistrationPage(object sender, EventArgs e)
        {
            GetRegistrationPage();
        }
    }
}
