using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class RoomDetailPage : ContentPage
    {
        public string Name
        {
            set
            {
                Current = RoomData.Roooms
                                  .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));//.Clone();

                Button button1 = null;
                Button button2 = null;
                Label label1 = null;
                Label label2 = null;
                Label label3 = null;
                Label label4 = null;
                Label label5 = null;
                Label label6 = null;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;

                if (Current.Name        != null)
                {
                    label1 = new Label
                    {
                        Text = Current.Name,
                        Style = Device.Styles.TitleStyle,
                        //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        //     VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label1);
                }
                if (Current.Description != null)
                {
                    label2 = new Label
                    {
                        Text = Current.Description,
                        FontAttributes = FontAttributes.Italic,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label2);
                }
                if (Current.Details     != null)
                {
                    label3 = new Label
                    {
                        Text = Current.Details,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Fill
                    };

                    stackLayout.Children.Add(label3);
                }
                if (Current == null)
                {
                    label4 = new Label
                    {
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
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
                if (Current.Site  != null)
                {
                    button2 = new Button
                    {
                        Text = "Open web site",
                    };
                    button2.Clicked += clickSite;

                    stackLayout.Children.Add(button2);
                }
                if (Current.Timetable != null)
                {
                    label5 = new Label
                    {
                        Text = Current.Timetable,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    stackLayout.Children.Add(label5);
                }
                if (Current.Staff     != null)
                {
                    Label labeq = new Label
                    {
                        Text = "Работники:",
                        TextColor = Color.Black,
                        FontSize=12,
                        Style = Device.Styles.TitleStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    stackLayout.Children.Add(labeq);

                    foreach (var i in Current.Staff)
                    {
                        Button buttpers = new Button
                        {
                            Text = i.ToString() ?? "wot",
                            FontSize=12,
                            Style=Device.Styles.TitleStyle,
                        };
                        buttpers.Clicked += clickPers;

                        stackLayout.Children.Add(buttpers);
                    }
                }


                if (!User.isNull)
                {
                    foreach (var i in User.CurrentUser.Notes)
                    {
                        if (i.Room == Current.Name)
                        {
                            Label notelab = new Label
                            {
                                Text = i.Text.ToString() ?? "wot",
                                FontSize = 12,
                                Style = Device.Styles.TitleStyle,
                            };

                            stackLayout.Children.Add(notelab);
                        }
                    }
                }
                // cюда ещще загрузка публичных заметок отдельно




                ScrollView scrollView = new ScrollView();
                scrollView.Content = stackLayout;

                this.Content = scrollView;

                BindingContext = RoomData.Roooms.FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
            }
        }
        // DisplayAlert("Alert", "и заново" + Current.Favorite, "OK");
        public Room Current { get; set; }

        public RoomDetailPage()
        {
            InitializeComponent();
        }

        // On open page
        protected override void OnAppearing()
        {
            // DisplayAlert("Alert", "You have been oritr", "OK");
            ToolbarItem item = star;
            if (Current.Favorite == true)
            {
                item.IconImageSource = "@drawable/stared.png";
                ToolbarItems.Remove(item);
                ToolbarItems.Add(item);
                //      DisplayAlert("Alert", "You have been favoritr on app " + Current.Favorite, "OK");
            }
            else
            {
                item.IconImageSource = "@drawable/unstared.png";
                ToolbarItems.Remove(item);
                ToolbarItems.Add(item);
                //    DisplayAlert("Alert", "You have been unfavoritr on app" + Current.Favorite, "OK");
            }

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
        async void clickPers(object sender, EventArgs args)
        {
            Button btn = (Button)sender;

            string[] words = btn.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            
            //DisplayAlert("Alert", words[2] ?? "no", "OK");

            await Navigation.PushAsync(new WorkerDetailPage(words));
        }
        // Star click
        async void OnfaviriteClicked(object sender, EventArgs args)
        {
            ToolbarItem item = (ToolbarItem)sender;

            if (Current.Favorite == true)
            {
                //star.IconImageSource = "@drawable/unstared.png";
                item.IconImageSource = "@drawable/unstared.png";
                ToolbarItems.Remove(item);
                ToolbarItems.Add(item);
                Current.Favorite = false;
           //     await DisplayAlert("Alert", "You have been unfavoritr " + Current.Favorite, "OK");
            }
            else
            {
                //star.IconImageSource = "@drawable/stared.png";
                item.IconImageSource = "@drawable/stared.png";
                ToolbarItems.Remove(item);
                ToolbarItems.Add(item);
                Current.Favorite = true;
        //        await DisplayAlert("Alert", "You have been favoritr " + Current.Favorite, "OK");
            }
        }
        // Toolbar
        async void OnButton1Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 1 been alerted", "OK");
        }
        async void OnButton2Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 2 been alerted", "OK");
        }
        async void OnButton3Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 3 been alerted", "OK");
        }
        async void OnButton4Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 4 been alerted", "OK");
        }
    }
}
