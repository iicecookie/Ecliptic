﻿using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;
using Ecliptic.Views.WayFounder;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class BuildingDetailPage : ContentPage
    {
        public string Name
        {
            get { return Name; }
            set
            {
                Current = BuildingData.Buildings
                                   .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
                Title = "Здание" + Current.Name;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;
                stackLayout.Spacing = 10;

                Label NameLab = new Label
                {
                    Text = Current.Name,
                    TextColor = Color.Black,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                };
                stackLayout.Children.Add(NameLab);

                Label DescriptionLab = new Label
                {
                    Text = Current.Description,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                };
                stackLayout.Children.Add(DescriptionLab);

                Label AddreesLab = new Label
                {
                    Text = Current.Addrees,
                    TextColor = Color.Black,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                };
                stackLayout.Children.Add(AddreesLab);

                if (Current.TimeTable != null)
                {
                    Label TimeTableLab = new Label
                    {
                        Text = Current.TimeTable,
                        TextColor = Color.Black,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    };
                    stackLayout.Children.Add(TimeTableLab);
                }
                if (Current.Site      != null)
                {
                    Button SiteBut = new Button
                    {
                        Text = Current.Site,
                        FontSize = 13,
                        FontAttributes = FontAttributes.Bold,
                        BorderColor = Color.Gray,
                        TextColor   = Color.Black,
                        BackgroundColor = Color.FromHex("#FFC7C7"),
                        HeightRequest = 40,
                    };
                    SiteBut.Clicked += clickSite;
                    stackLayout.Children.Add(SiteBut);
                }

                Button DownloadBut = new Button
                {
                    Text = "Загрузить это здание",
                    FontSize = 13,
                    FontAttributes = FontAttributes.Bold,
                    BorderColor = Color.Gray,
                    TextColor   = Color.Black,
                    BackgroundColor = Color.FromHex("#FFC7C7"),
                    HeightRequest = 40,
                };
                DownloadBut.Clicked += DownloadBut_Click;
                stackLayout.Children.Add(DownloadBut);

                this.Content = new ScrollView { Content = stackLayout };
            }
        }

        public Building Current { get; set; }

        public BuildingDetailPage()
        {
            InitializeComponent();
        }

        void DownloadBut_Click(Object sender, EventArgs e)
        {

        }

        void clickSite(object sender, EventArgs args)
        {
            try
            {
                new System.Threading.Thread(() =>
                {
                    Launcher.OpenAsync(new Uri(Current.Site));
                }).Start();
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>().Show("Не удалось открыть сайт " + e.Message);
            }
            
        }
    }
}
