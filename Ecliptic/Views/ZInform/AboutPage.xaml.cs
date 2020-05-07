﻿using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecliptic.Views
{
    public partial class AboutPage : ContentPage
    {
        public ICommand TapCommand =>
                        new Command<string>(
                            (url) => Xamarin.Essentials.Launcher.CanOpenAsync(url));

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}