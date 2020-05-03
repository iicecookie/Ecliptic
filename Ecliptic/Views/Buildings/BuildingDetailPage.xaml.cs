using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;
using Ecliptic.Views.WayFounder;
using Ecliptic.Repository;

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

                Label Name = null;
                Label Details = null;
                Button DownloadBtn = null;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;

                Name = new Label
                {
                    Text = Current.Name,

                    Style = Device.Styles.TitleStyle,
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Center
                };

                Details = new Label
                {
                    Text = Current.Details,
                    Style = Device.Styles.BodyStyle,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };

                DownloadBtn = new Button
                {
                    Text = "Загрузить это здание",
                    BackgroundColor = Color.FromHex("#B98CC1"),
                    BorderColor = Color.Gray,
                    TextColor = Color.Black,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Fill
                };
                DownloadBtn.Clicked += DownloadBtn_Click;

                stackLayout.Children.Add(Name);
                stackLayout.Children.Add(Details);
                stackLayout.Children.Add(DownloadBtn);

                this.Content = new ScrollView { Content = stackLayout };
            }
        }

        public Building Current { get; set; }

        public BuildingDetailPage()
        {
            InitializeComponent();
        }

        void DownloadBtn_Click(Object sender, EventArgs e)
        {

        }
    }
}
