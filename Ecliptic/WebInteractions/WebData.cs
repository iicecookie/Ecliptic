using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecliptic.WebInteractions
{
    public static class WebData
    {
        public const string ADRESS = "http://ecliptic.site/";

        public static bool istest = true;

        public async static Task<bool> CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return false;
            }
            bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
            if (!isRemoteReachable)
            {
                DependencyService.Get<IToast>().Show("Сервер не доступен. Повторите попытку позже");
                return false;
            }
            return true;
        }
    }
}