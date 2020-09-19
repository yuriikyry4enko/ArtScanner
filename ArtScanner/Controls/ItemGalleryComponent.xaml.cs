
using System;
using ArtScanner.Models.Entities;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public partial class ItemGalleryComponent : Grid
    {

        //double yHalfPosition;
        //double yFullPosition;
        //double yZeroPosition;
        //int currentPsotion;
        //double currentPostionY;
        //bool up;
        //bool down;
        //bool isTurnY;
        //double valueY;
        //double y;

        private int position = 0;

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
            titleItem.IsVisible = false;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (position == 0)
            {
                titleItem.IsVisible = true;

                itemInfoCircleButton.TranslateTo(this.X, this.Y - 150);
                artItemImage.TranslateTo(this.X, this.Y - 150);
                titleItem.TranslateTo(this.X, this.Y - 100);

                itemInfoCircleButton.RotateTo(-90);

                position = 1;
            }
            else
            {
                itemInfoCircleButton.TranslateTo(this.X, this.Y);
                artItemImage.TranslateTo(this.X, this.Y);
                titleItem.TranslateTo(this.X, this.Y);

                titleItem.IsVisible = false;

                itemInfoCircleButton.RotateTo(90);

                position = 0;
            }

        }

        //void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        //{
        //    // Handle the pan
        //    switch (e.StatusType)
        //    {
        //        case GestureStatus.Running:

        //            if (e.TotalY >= 5 || e.TotalY <= -5 && !isTurnY)
        //            {
        //                isTurnY = true;
        //            }
        //            if (isTurnY)
        //            {
        //                if (e.TotalY <= valueY)
        //                {
        //                    up = true;

        //                }
        //                if (e.TotalY >= valueY)
        //                {
        //                    down = true;

        //                }
        //            }
        //            if (up)
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    if (yFullPosition < (currentPostionY + (-1 * e.TotalY)))
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yFullPosition);
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -1 * (currentPostionY + (-1 * e.TotalY)));
        //                    }
        //                }
        //                else
        //                {
        //                    if (yFullPosition < (currentPostionY + (-1 * e.TotalY)))
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yFullPosition, 20);
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -1 * (currentPostionY + (-1 * e.TotalY)), 20);
        //                    }
        //                }
        //            }
        //            else if (down)
        //            {
        //                if (Device.RuntimePlatform == Device.Android)
        //                {
        //                    if (yZeroPosition > currentPostionY - e.TotalY)
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yZeroPosition);
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -(currentPostionY - (e.TotalY)));
        //                    }
        //                }
        //                else
        //                {
        //                    if (yZeroPosition > currentPostionY - e.TotalY)
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yZeroPosition, 20);
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -(currentPostionY - (e.TotalY)), 20);
        //                    }
        //                }
        //            }
        //            break;
        //        case GestureStatus.Completed:
        //            // Store the translation applied during the pan
        //            valueY = e.TotalY;
        //            y = bottomSheet.TranslationY;

        //            //at the end of the event - snap to the closest location
        //            //var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(getClosestLockState(e.TotalY + y)));

        //            //depending on Swipe Up or Down - change the snapping animation
        //            if (up)
        //            {
        //                //swipe up happened
        //                if (currentPsotion == 1)
        //                {
        //                    bottomSheet.TranslateTo(bottomSheet.X, -yFullPosition);
        //                    currentPsotion = 2;
        //                    currentPostionY = yFullPosition;
        //                    bottomsheetListView.HeightRequest = yFullPosition;
        //                    bottomsheetListView.HeightRequest = yFullPosition;
        //                    //bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringIn);
        //                }
        //                else if (currentPsotion == 0)
        //                {
        //                    double currentY = (-1) * y;
        //                    double differentBetweenHalfAndCurrent = Math.Abs(currentY - yHalfPosition);
        //                    double differentBetweenFullAndCurrent = Math.Abs(currentY - yFullPosition);
        //                    //check which is close snap point and move to the closest snap point
        //                    if (differentBetweenHalfAndCurrent < differentBetweenFullAndCurrent)
        //                    {
        //                        //yHalfPosition is the closest one
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yHalfPosition);
        //                        currentPsotion = 1;
        //                        currentPostionY = yHalfPosition;
        //                        bottomsheetListView.HeightRequest = yHalfPosition;
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yFullPosition);
        //                        currentPsotion = 2;
        //                        currentPostionY = yFullPosition;
        //                        bottomsheetListView.HeightRequest = yFullPosition;
        //                    }

        //                }

        //            }
        //            if (down)
        //            {
        //                //swipe down happened
        //                if (currentPsotion == 1)
        //                {
        //                    bottomSheet.TranslateTo(bottomSheet.X, -yZeroPosition);
        //                    currentPsotion = 0;
        //                    currentPostionY = yZeroPosition;
        //                }
        //                else if (currentPsotion == 2)
        //                {
        //                    double currentY = (-1) * y;
        //                    double differentBetweenHalfAndCurrent = Math.Abs(currentY - yHalfPosition);
        //                    double differentBetweenZeroAndCurrent = Math.Abs(currentY - yZeroPosition);
        //                    if (differentBetweenHalfAndCurrent < differentBetweenZeroAndCurrent)
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yHalfPosition);
        //                        currentPsotion = 1;
        //                        currentPostionY = yHalfPosition;
        //                        bottomsheetListView.HeightRequest = yHalfPosition;
        //                    }
        //                    else
        //                    {
        //                        bottomSheet.TranslateTo(bottomSheet.X, -yZeroPosition);
        //                        currentPsotion = 0;
        //                        currentPostionY = yZeroPosition;
        //                    }


        //                }
        //                //bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
        //            }

        //            y = bottomSheet.TranslationY;
        //            up = false;
        //            down = false;
        //            break;

        //    }
    
        //}
    }
}
