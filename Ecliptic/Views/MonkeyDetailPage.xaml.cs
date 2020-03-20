using Xamarin.Forms;
using Ecliptic.ViewModels;

namespace Ecliptic.Views
{
    public partial class MonkeyDetailPage : ContentPage
    {
        public MonkeyDetailPage()
        {
            InitializeComponent();
            BindingContext = new MonkeyDetailViewModel();
        }
    }
}
