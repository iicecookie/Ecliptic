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

            Title = Current.FirstName + " " + Current.SecondName;

            Color backcolor = Color.FromHex("#FAFAFA");

            if (Current != null)
            {
                Label FIOlab = new Label
                {
                    Text = Current.FirstName + " " + Current.SecondName + " " + Current.LastName,
                    TextColor = Color.Black,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Add(FIOlab);
            }
            if (Current.Status  != null)
            {
                Label Statuslab = new Label
                {
                    Text = Current.Status,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Add(Statuslab);
            }
            if (Current.Details != null)
            {
                Label Detailslab = new Label
                {
                    Text = Current.Details,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                stackLayout.Children.Add(Detailslab);
            }
            if (Current.Email   != null)
            {
                Button Emailbut = new Button
                {
                    Text = "Написать " + Current.Email,
                    TextColor = Color.Black,
                    BackgroundColor = backcolor,
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                Emailbut.Clicked += clickmail;
                stackLayout.Children.Add(Emailbut);
            }
            if (Current.Phone   != null)
            {
                Button Phonebut = new Button
                {
                    Text = "Позвонить " + Current.Phone,
                    TextColor = Color.Black,
                    BackgroundColor = backcolor,
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                Phonebut.Clicked += clickphone;
                stackLayout.Children.Add(Phonebut);
            }
            if (Current.Site    != null)
            {
                Button Sitebut = new Button
                {
                    Text = "Открыть сайт",
                    TextColor = Color.Black,
                    BackgroundColor = backcolor,
                    VerticalOptions = LayoutOptions.Center,
                    Style = Device.Styles.BodyStyle,
                    FontSize = 12,
                    HeightRequest = 40,
                };
                Sitebut.Clicked += clickSite;
                stackLayout.Children.Add(Sitebut);  
            }

            this.Content = new ScrollView { Content = stackLayout };
        }

        protected override void OnAppearing()
        {


        }

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
        void clickSite (object sender, EventArgs args)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(Current.Site));
            }).Start();
        }

    }
}