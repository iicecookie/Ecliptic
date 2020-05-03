﻿using Ecliptic.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public  class RegisrationContrioolers
        {
            public Label labelMessage { get; set; }
            public Entry NameBox { get; set; }
            public Entry LoginBox { get; set; }
            public Entry PasswBox { get; set; }

            public Entry PasswCheckBox { get; set; }

            public Button RegisBtn { get; set; }
            public Button LoginBtn { get; set; }

            public RegisrationContrioolers()
            {
                labelMessage = new Label
                {
                    Text = "Заполните поля",
                    Style = Device.Styles.TitleStyle,
                    //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    //     VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };
                NameBox = new Entry
                {
                    Text = "",
                    Placeholder = "Имя пользователя",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    //   FontAttributes = FontAttributes.Italic,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
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
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    // FontAttributes = FontAttributes.Italic,
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

                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    // FontAttributes = FontAttributes.Italic,
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
        }

        public RegisrationContrioolers RegisrationPage;

        public void GetRegistrationPage()
        {
            Title = "Зарегистрироваться";

            RegisrationPage = new RegisrationContrioolers();

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;


            RegisrationPage.RegisBtn.Clicked += RegistrUser;
            RegisrationPage.LoginBtn.Clicked += ToLoginPage;

            stackLayout.Children.Add(RegisrationPage.labelMessage);
            stackLayout.Children.Add(RegisrationPage.NameBox);
            stackLayout.Children.Add(RegisrationPage.LoginBox);
            stackLayout.Children.Add(RegisrationPage.PasswBox);
            stackLayout.Children.Add(RegisrationPage.PasswCheckBox);
            stackLayout.Children.Add(RegisrationPage.RegisBtn);
            stackLayout.Children.Add(RegisrationPage.LoginBtn);

            this.Content = new ScrollView { Content = stackLayout };
        }

        public async void RegistrUser(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }

            if (RegisrationPage.NameBox.Text  == "" ||
                RegisrationPage.LoginBox.Text == "" ||
                RegisrationPage.PasswBox.Text == "" ||
                RegisrationPage.PasswCheckBox.Text == "")
            {
                await DisplayAlert("Ошипка", "Не все поля заполнены", "OK");
                return;
            }
            if (RegisrationPage.PasswBox.Text !=
                RegisrationPage.PasswCheckBox.Text)
            {
                await DisplayAlert("Ошипка", "Пароли не совпадают", "OK");
                return;
            }
            // зарегестрировать узера
            // перейти на окно усера
            User.LoadUser(RegisrationPage.LoginBox.Text, RegisrationPage.PasswBox.Text);

            GetUserPage();
        }

        private void ToLoginPage(object sender, EventArgs e)
        {
            GetLoginPage();
        }
    }
}
