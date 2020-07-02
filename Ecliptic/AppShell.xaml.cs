using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Ecliptic.Data;
using Ecliptic.Views;
using Ecliptic.Views.ClientInteraction;
using Ecliptic.Models;

namespace Ecliptic
{
    public partial class AppShell : Shell
    {
        private Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public  Dictionary<string, Type> Routes { get { return routes; } }

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;
        }

        void RegisterRoutes()
        {
            routes.Add("scheme",          typeof(SchemePlanPage));
            routes.Add("roomdetails",     typeof(RoomDetailPage));
            routes.Add("buildings",       typeof(BuildingPage));
            routes.Add("buildingdetails", typeof(BuildingDetailPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}
