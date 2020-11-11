using ArtScanner.Models;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsGalleryPage : BasePage
    {
        public ItemsGalleryPage()
        {
            InitializeComponent();
            //carouselGalleyItems.CurrentItemChanged += CarouselGalleyItems_CurrentItemChanged;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
        }
    }
}
