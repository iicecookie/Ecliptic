using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class BearDetailPage : ContentPage
    {
        public string Name
        {
            set
            {
                BindingContext = BearData.Bears.FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
            }
        }

        public BearDetailPage()
        {
            InitializeComponent();
        }
    }
}
