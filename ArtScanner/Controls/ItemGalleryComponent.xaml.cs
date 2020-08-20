
using ArtScanner.Models.Entities;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public partial class ItemGalleryComponent : Grid
    {
        //public static readonly BindableProperty ArtProperty =
        //           BindableProperty.Create(nameof(ArtModel), typeof(ItemEntity), typeof(ItemGalleryComponent), new ItemEntity(), BindingMode.TwoWay);

        //public ItemEntity ArtModel
        //{
        //    get { return (ItemEntity)GetValue(ArtProperty); }
        //    set { SetValue(ArtProperty, value); }
        //}

        public static readonly BindableProperty ToggleFavoriteCommandProperty =
                   BindableProperty.Create(nameof(ToggleFavoriteCommand), typeof(DelegateCommand<ItemEntity>), typeof(ItemGalleryComponent));


        public DelegateCommand<ItemEntity> ToggleFavoriteCommand
        {
            get { return (DelegateCommand<ItemEntity>)GetValue(ToggleFavoriteCommandProperty); }
            set { SetValue(ToggleFavoriteCommandProperty, value); }
        }

        public ItemGalleryComponent()
        {
            InitializeComponent();
        }
    }
}
