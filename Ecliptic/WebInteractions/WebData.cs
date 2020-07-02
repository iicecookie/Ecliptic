using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecliptic.WebInteractions
{
    public static class WebData
    {
        // адрес серверной части информационной системы
        public const string ADRESS = "http://ecliptic.site/";

        // проверка подключения к веб сервису
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

        // настройка клиента
        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
    }
}