using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.WebInteractions
{
    public static class WebData
    {
        public const string ADRESS = "http://ecliptic.site/";

        public static bool istest = true;

        public async static void CheckConnection()
        {
            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }
            bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(WebData.ADRESS);
            if (!isRemoteReachable)
            {
                DependencyService.Get<IToast>().Show("Сервер не доступен. Повторите попытку позже");
            }
        }
    }
}