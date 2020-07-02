using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ecliptic.Models;

using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

using Ecliptic.Data;

namespace Ecliptic.Views.RoomInform
{
	public partial class QrScan : ContentPage
	{
		ZXingScannerView    zxing;
		ZXingDefaultOverlay overlay;

		public QrScan()
		{
			InitializeComponent();

			zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions   = LayoutOptions.FillAndExpand
			};

			zxing.AutoFocus();

			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{
					// Остановить анализ пока мы не вернемся на страницу
					zxing.IsAnalyzing = false;

					if (result.Text != null)
						if (RoomData.isThatRoom(result.Text) != null)
						{
							// если помещение с имянем на QR есть в системе - открыть его страницу
							await Shell.Current.GoToAsync($"roomdetails?name={result.Text}"); zxing.IsAnalyzing = true;
						}
						else
						{
							// иначе вывести содержимое кода
							await DisplayAlert("Информация с кода: ", result.Text, "OK"); zxing.IsAnalyzing = true;
						}

					zxing.IsAnalyzing = true;
				});

			overlay = new ZXingDefaultOverlay
			{
				TopText	   = "Поднесите телефон к штрих-коду",
				BottomText = "Сканирование произойдет автоматически",
				ShowFlashButton = zxing.HasTorch,
			};
			overlay.FlashButtonClicked += (sender, e) =>
			{
				zxing.IsTorchOn = !zxing.IsTorchOn;
			};
			var grid = new Grid
			{
				VerticalOptions   = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			grid.Children.Add(zxing);
			grid.Children.Add(overlay);

			Content = grid;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			zxing.IsScanning = true;
			var contentHolder = Content;
			Content = null;
			Content = contentHolder;
		}

		protected override void OnDisappearing()
		{
			zxing.IsScanning = false;
			base.OnDisappearing();
		}
	}
}