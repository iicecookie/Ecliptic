using System.Linq;
using Xamarin.Forms;
using Ecliptic.Models;
using System.Collections.ObjectModel;
using Ecliptic.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using Ecliptic.Repository;

namespace Ecliptic.Views
{
    public partial class BuildingDetailPage : ContentPage
    {
        public BuildingDetailPage()
        {
            InitializeComponent();
        }

        private void SaveFriend(object sender, EventArgs e)
        {
            var friend = (Building)BindingContext;
            if (!String.IsNullOrEmpty(friend.Name))
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (friend.Id == 0)
                        db.Buildings.Add(friend);
                    else
                    {
                        db.Buildings.Update(friend);
                    }
                    db.SaveChanges();
                }
            }
            this.Navigation.PopAsync();
        }

        private void DeleteFriend(object sender, EventArgs e)
        {
            var friend = (Building)BindingContext;
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Buildings.Remove(friend);
                db.SaveChanges();
            }
            this.Navigation.PopAsync();
        }
    }
}