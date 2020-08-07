using System;
using System.Collections.Generic;
using ArtScanner.Models;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public partial class ItemGalleryComponent : Grid
    {

        public static readonly BindableProperty ArtProperty =
                   BindableProperty.Create(nameof(ArtModel), typeof(ArtModel), typeof(ItemGalleryComponent), new ArtModel(), BindingMode.TwoWay);

        public ArtModel ArtModel
        {
            get { return (ArtModel)GetValue(ArtProperty); }
            set { SetValue(ArtProperty, value); }
        }

        public static readonly BindableProperty ToggleFavoriteCommandProperty =
                   BindableProperty.Create(nameof(ToggleFavoriteCommand), typeof(DelegateCommand<ArtModel>), typeof(ItemGalleryComponent));


        public DelegateCommand<ArtModel> ToggleFavoriteCommand
        {
            get { return (DelegateCommand<ArtModel>)GetValue(ToggleFavoriteCommandProperty); }
            set { SetValue(ToggleFavoriteCommandProperty, value); }
        }

        public ItemGalleryComponent()
        {
            InitializeComponent();
        }
    }
}
