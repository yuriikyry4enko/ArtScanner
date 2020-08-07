using System;
using Xamarin.Forms;

namespace ArtScanner.Views
{
    public partial class BasePage : ContentPage
    {
        public BasePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
