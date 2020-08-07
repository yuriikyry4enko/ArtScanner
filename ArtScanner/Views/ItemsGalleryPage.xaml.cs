using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsGalleryPage : BasePage
    {
        public ItemsGalleryPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}
