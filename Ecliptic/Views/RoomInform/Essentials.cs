
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ecliptic.Views.RoomInform
{
  static  class Essentials
    {
        public static void CallPhone(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                DependencyService.Get<IToast>().Show("Неверный номер " + anEx.Message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                DependencyService.Get<IToast>().Show("Не потдерживается на вашем телефоне" + ex.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Неизвесная ошибка " + ex.Message);
            }
        }

        public static void OpenSite(string siteAdress)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(siteAdress));
            }).Start();
        }

        public static async Task SendEmail(string toAdress)
        {
            List<string> toAddress = new List<string>();
            toAddress.Add(toAdress);

            try
            {
                var message = new EmailMessage
                {
                    Subject = "",
                    Body = "",
                    To = toAddress,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                DependencyService.Get<IToast>().Show("не поддерживается на вашем устройстве");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Произошла ошибка");
            }
        }
    }
}
