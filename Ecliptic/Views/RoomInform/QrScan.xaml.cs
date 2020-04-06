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
	/*
	public partial class QrScan : ContentPage
	{
		ZXingScannerPage scanPage;

		Button buttonScan;

		public QrScan() : base()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			Label lab1 = new Label
			{
				Text = "dfghjkl;",
				

			};
			buttonScan= new Button
			{
				Text = "Scan with Default Overlay",
				AutomationId = "scanWithDefaultOverlay",
			};

			buttonScan.Clicked += async delegate
			{
				scanPage = new ZXingScannerPage();
				scanPage.OnScanResult += (result) =>
				{
					scanPage.IsScanning = false;

					Device.BeginInvokeOnMainThread(async () =>
					{
						await Navigation.PopAsync();

						
						if (RoomData.isThatRoom(result.Text))
						{
							await Shell.Current.GoToAsync($"roomdetails?name={result.Text}");
						}
						else
						{
							await DisplayAlert("Scanned Barcode ", result.Text, "OK");
						}
					});
				};
				await Navigation.PushAsync(scanPage);
			};

            var stack = new StackLayout();

			stack.Children.Add(lab1);
			stack.Children.Add(buttonScan);

		//	ScrollView scrollView = new ScrollView();
		//	scrollView.Content = stack;

			this.Content = stack;
		}
	}
	*/
	/*
	public partial class QrScan : ContentPage
	{
		ZXingScannerView scanPage;

		public QrScan() : base()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			scanPage = new ZXingScannerView();
			
			scanPage.OnScanResult += (result) =>
			{
				scanPage.IsScanning = false;

				Device.BeginInvokeOnMainThread(async () =>
				{
					if (RoomData.isThatRoom(result.Text))
					{
						await Shell.Current.GoToAsync($"roomdetails?name={result.Text}");
					}
					else
					{
						await DisplayAlert("Scanned Barcode ", result.Text, "OK");
					}
				});
			};

			scanPage.IsScanning = true;

			this.Content = scanPage;
		}
	}

	*/

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
					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;
					zxing.IsScanning = false;

					// Show an alert
					if (result.Text != null)
						if (RoomData.isThatRoom(result.Text))
						{
							await Shell.Current.GoToAsync($"roomdetails?name={result.Text}");
							zxing.IsScanning = true;
							zxing.IsAnalyzing = true;

						}
						else
						{
							await DisplayAlert("Scanned Barcode ", result.Text, "OK");
						}
				});

			overlay = new ZXingDefaultOverlay
			{
				TopText = "Поднесите телефон к штрих-коду",
				BottomText = "Сканирование произойдет автоматически",
				ShowFlashButton = zxing.HasTorch,
			};
			overlay.FlashButtonClicked += (sender, e) =>
			{
				zxing.IsTorchOn = !zxing.IsTorchOn;
			};
			var grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			grid.Children.Add(zxing);
			grid.Children.Add(overlay);

			// The root page of your application
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

		protected override void OnDisappearing	()
		{
			zxing.IsScanning = false;

			base.OnDisappearing();
		}
	}
}