using System.Linq;
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
            await Task.Delay(500);

            await Shell.Current.GoToAsync($"roomdetails?name={((Room)item).Name}");
        }
    }
}
