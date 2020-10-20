using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookletPage : BasePage
    {
        public BookletPage()
        {
            InitializeComponent();
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
        }
    }
}
