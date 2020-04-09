﻿using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Ecliptic.Data;
using Ecliptic.Models;

namespace Ecliptic.Controls
{
    public class RoomSearchHandler : SearchHandler
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
                ItemsSource = RoomData.Rooms
                    .Where(room => room.Name.ToLower().Contains(newValue.ToLower())|| 
                                room.Details.ToLower().Contains(newValue.ToLower()))
                    .ToList<Room>();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Task.Delay(1000);

            // Note: strings will be URL encoded for navigation (e.g. "Blue Monkey" becomes "Blue%20Monkey"). Therefore, decode at the receiver.
            // This works because route names are unique in this application.
            await Shell.Current.GoToAsync($"roomdetails?name={((Room)item).Name}");
            // The full route is shown below.
            // await Shell.Current.GoToAsync($"//animals/monkeys/monkeydetails?name={((Animal)item).Name}");
        }
    }
}
