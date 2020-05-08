using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecliptic.Views
{
    public partial class WorkerDetailPage : ContentPage
    {
        public string FirstName
        {
            set
            {
                BindingContext = WorkerData.Workers.FirstOrDefault(m => m.FirstName == "Celivans");//Uri.UnescapeDataString(value));
            }
        }

        Worker Current = null;
        
        public WorkerDetailPage()
        {
            InitializeComponent();
        }

        public WorkerDetailPage(string[] words) : this()
        {

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            if(words.Length==2)
                Current = WorkerData.GetWorker(words[0], words[1]);
            else
                Current = WorkerData.GetWorker(words[0], words[1], words[2]);

            Button button1 = null;
            Button button2 = null;
            Label label1 = null;
            Label label2 = null;
            Label label3 = null;
            Label label4 = null;

            if (Current != null)
            {
                label1 = new Label
                {
                    Text = Current.FirstName + " " + Current.SecondName + " " + Current.LastName,

                    TextColor = Color.Black,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Add(label1);
            }

            if (Current.Status != null)
            {

                label2 = new Label
                {
                    Text = Current.Status,

                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Add(label2);
            }

            if (Current.Details != null)
            {

                label3 = new Label
                {
                    Text = Current.Details,

                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                stackLayout.Children.Add(label3);
            }

            if (Current.Email != null)
            {
                Button mailBtn = new Button
                {
                    Text = "Написать " + Current.Email,
                    TextColor = Color.Black,
                    BackgroundColor = Color.FromHex("#6866A6"),
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                mailBtn.Clicked += clickmail;
                stackLayout.Children.Add(mailBtn);
            }

            if (Current.Phone != null)
            {
                button1 = new Button
                {
                    Text = "Позвонить " + Current.Phone,
                    TextColor = Color.Black,
                    BackgroundColor = Color.FromHex("#6866A6"),
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                button1.Clicked += clickphone;

                stackLayout.Children.Add(button1);
            }

            if (Current.Site != null)
            {
                button2 = new Button
                {
                    Text = "Открыть сайт",
                    TextColor = Color.Black,
                    BackgroundColor = Color.FromHex("#6866A6"),
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                button2.Clicked += clickSite;

                stackLayout.Children.Add(button2);  
            }


            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;
        }

        protected override void OnAppearing()
        {


        }

        async void clickmail(object sender, System.EventArgs e)
        {
            List<string> toAddress = new List<string>();
            toAddress.Add(Current.Email);

            await SendEmail(toAddress);
        }

        public async Task SendEmail(List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "",
                    Body = "",
                    To = recipients,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                DependencyService.Get<IToast>().Show("не поддерживается на вашем устройстве");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Произошла ошибка");
            }
        }

        // Buttons on page
        void clickphone(object sender, EventArgs args)
        {
            try
            {
                PhoneDialer.Open(Current.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                DependencyService.Get<IToast>().Show("Неверный номер " + anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                DependencyService.Get<IToast>().Show("Не потдерживается на вашем телефоне" + ex.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Неизвесная ошибка " + ex.Message);
            }
        }

        void clickSite (object sender, EventArgs args)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(Current.Site));
            }).Start();
        }
    }
}