using Ecliptic.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public static class RegisrationContrioolers
        {
            static public Label labelMessage { get; set; }
            static public Entry NameBox { get; set; }
            static public Entry LoginBox { get; set; }
            static public Entry PasswBox { get; set; }

            static public Entry PasswCheckBox { get; set; }

            static public Button RegisBtn { get; set; }
            static public Button LoginBtn { get; set; }
        }

        public void GetRegistrationPage()
        {
            Title = "Зарегистрироваться";
            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            RegisrationContrioolers.labelMessage  = new Label
            {
                Text = "Заполните поля",
                Style = Device.Styles.TitleStyle,
                //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                //     VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };
            RegisrationContrioolers.NameBox       = new Entry
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
            RegisrationContrioolers.LoginBox      = new Entry
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
            RegisrationContrioolers.PasswBox      = new Entry
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
            RegisrationContrioolers.PasswCheckBox = new Entry
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

            RegisrationContrioolers.RegisBtn      = new Button
            {
                Text = "Зарегестрироваться",
                BackgroundColor = Color.FromHex("#BFD9B6"),
                TextColor = Color.Black,
                BorderColor = Color.Black,
            };
            RegisrationContrioolers.LoginBtn      = new Button
            {
                Text = "Уже есть учетная запись",
                BackgroundColor = Color.FromHex("#BFD9B6"),
                TextColor = Color.Black,
                BorderColor = Color.Black,
            };
            RegisrationContrioolers.RegisBtn.Clicked += RegistrUser;
            RegisrationContrioolers.LoginBtn.Clicked += ToLoginPage;

            stackLayout.Children.Add(RegisrationContrioolers.labelMessage);
            stackLayout.Children.Add(RegisrationContrioolers.NameBox);
            stackLayout.Children.Add(RegisrationContrioolers.LoginBox);
            stackLayout.Children.Add(RegisrationContrioolers.PasswBox);
            stackLayout.Children.Add(RegisrationContrioolers.PasswCheckBox);
            stackLayout.Children.Add(RegisrationContrioolers.RegisBtn);
            stackLayout.Children.Add(RegisrationContrioolers.LoginBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;
            this.Content = scrollView;
        }

        public async void RegistrUser(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }

            if (RegisrationContrioolers.NameBox.Text  == "" ||
                RegisrationContrioolers.LoginBox.Text == "" ||
                RegisrationContrioolers.PasswBox.Text == "" ||
                RegisrationContrioolers.PasswCheckBox.Text == "")
            {
                await DisplayAlert("Ошипка", "Не все поля заполнены", "OK");
                return;
            }
            if (RegisrationContrioolers.PasswBox.Text !=
                RegisrationContrioolers.PasswCheckBox.Text)
            {
                await DisplayAlert("Ошипка", "Пароли не совпадают", "OK");
                return;
            }
            // зарегестрировать узера
            // перейти на окно усера
            User.LoadUser(RegisrationContrioolers.LoginBox.Text, RegisrationContrioolers.PasswBox.Text);

            GetUserPage();
        }

        private void ToLoginPage(object sender, EventArgs e)
        {
            GetLoginPage();
        }
    }
}
