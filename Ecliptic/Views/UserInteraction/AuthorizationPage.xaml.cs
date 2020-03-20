using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public Authorization()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            GetLoginPage();
        }

    }
}