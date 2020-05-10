using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Ecliptic.Data;
using Ecliptic.Models;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ecliptic.Controls
{
    public class BuildingSearchHandler : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = BuildingData.Buildings
                    .Where(building => building.Name       .ToLower().Contains(newValue.ToLower())||
                                       building.Description.ToLower().Contains(newValue.ToLower()))
                    .ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            await Shell.Current.GoToAsync($"buildingdetails?name={((Building)item).Name}");
        }
    }
}
