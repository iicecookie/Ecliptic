using Ecliptic.Models;
using Ecliptic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecliptic.Views.ClientInteraction
{
    public partial class Authorization : ContentPage
    {
        public Authorization()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Client.CurrentClient == null)
            {
                GetLoginPage();
            }
            else
            {
                GetClientPage();
            }
        }
    }
}