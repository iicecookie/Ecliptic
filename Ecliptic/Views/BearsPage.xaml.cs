using System.Linq;
using Xamarin.Forms;
using Ecliptic.Models;
using Ecliptic.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using Ecliptic.Data;

namespace Ecliptic.Views
{
    public partial class BearsPage : ContentPage
    {
        public BearsPage()
        {
            // Title = "Code Button Click";
            //
            // Label label = new Label
            // {
            //     Text = "Click the Button below",
            //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            //     VerticalOptions = LayoutOptions.CenterAndExpand,
            //     HorizontalOptions = LayoutOptions.Center
            // };
            //
            // Button button = new Button
            // {
            //     Text = "Click to Rotate Text!",
            //     VerticalOptions = LayoutOptions.CenterAndExpand,
            //     HorizontalOptions = LayoutOptions.Center
            // };
            // button.Clicked += async (sender, args) => await label.RelRotateTo(360, 1000);
            //
            // Content = new StackLayout
            // {
            //     Children =
            // {
            //     label,
            //     button
            // }
            // };
            StackLayout stackLayout = new StackLayout();

            for (int i = 1; i < 20; i++)
            {
                Label label = new Label
                {
                    Text = "Метка " + i,
                    FontSize = 23
                };
                stackLayout.Children.Add(label);
            }

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;
        }




    }

}
