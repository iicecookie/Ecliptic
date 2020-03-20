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
   // [QueryProperty("FirstName", "firstfame")]
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
                label4 = new Label
                {
                    Text = Current.Email,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };

                stackLayout.Children.Add(label4);
            }
            if (Current.Phone != null)
            {
                button1 = new Button
                {
                    Text = "Call " + Current.Phone,
                };
                button1.Clicked += clickphone;

                stackLayout.Children.Add(button1);
            }
            if (Current.Site != null)
            {
                button2 = new Button
                {
                    Text = "Open web site",
                };
                button2.Clicked += clickSite;

                stackLayout.Children.Add(button2);  
            }


            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;

        }
        
        // DisplayAlert("Alert", "и заново" + Current.Favorite, "OK");

        // On open page
        protected override void OnAppearing()
        {


        }
        // Buttons on page
        async void clickphone(object sender, EventArgs args)
        {
            try
            {
                PhoneDialer.Open(Current.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            //  await DisplayAlert("Alert", "You have 1 been alerted"+Current.Phone , "OK");
        }
        async void clickSite(object sender, EventArgs args)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(Current.Site));
                //Device.OpenUri(new Uri(Current.Site));
            }).Start();
        }
    }
}