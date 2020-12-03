using System;
using System.Diagnostics;
using ArtScanner.Models.Entities;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public partial class ItemGalleryComponent : Grid
    {
        private int animationLifecycle = 0;

        public enum CardState
        {
            Expanded,
            Collapsed
        }

        public class CardEvent
        {
        }

        private CardState _cardState = CardState.Expanded;

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

        //void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        //{
        //    if (animationLifecycle == 0)
        //    {
        //        AnimateTransitionAsync();

        //        animationLifecycle = 1;
        //    }
        //    else if(animationLifecycle == 1)
        //    {
        //        animationLifecycle = 0;

        //        var parentAnimation = new Animation();

        //        parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.TranslationY = v,
        //              0, 50, Easing.SpringIn));
        //        parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.Opacity = v,
        //            1, 0, Easing.Linear));

        //        _cardState = CardState.Expanded;

        //        parentAnimation.Commit(this, "CardUndoExpand", 16, 1000);
        //    }
        //    else if(animationLifecycle == 2)
        //    {
        //        animationLifecycle = 1;


        //        //Animation last items
        //        var parentAnimation = new Animation();

        //        parentAnimation.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
        //            1, 0, Easing.Linear));


        //        parentAnimation.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
        //            1, 0, Easing.Linear));


        //        parentAnimation.Commit(this, "FrameCardCollapse", 16, 1000);


        //        var firstStepAnimation = new Animation();
        //        //Animation 1-st step
        //        firstStepAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.TranslationY = v,
        //           50, 0, Easing.SpringOut));
        //        firstStepAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.Opacity = v,
        //            0, 1, Easing.Linear));

        //        _cardState = CardState.Collapsed;

        //        firstStepAnimation.Commit(this, "CardExpand", 16, 1000);


        //    }
        //}

        //private void AnimateTransitionAsync()
        //{
        //    var parentAnimation = new Animation();

        //    if (_cardState == CardState.Expanded)
        //    {
        //        parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.TranslationY = v,
        //            50, 0, Easing.SpringOut));
        //        parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.Opacity = v,
        //            0, 1, Easing.Linear));

        //        _cardState = CardState.Collapsed;

        //        parentAnimation.Commit(this, "CardExpand", 16, 1300);
        //    }
        //    else
        //    {
        //        parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.TranslationY = v,
        //                0, 50, Easing.SpringIn));
        //        parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.Opacity = v,
        //            1, 0, Easing.Linear));

        //        _cardState = CardState.Expanded;

        //        parentAnimation.Commit(this, "CardUndoExpand", 16, 1000);
        //    }
        //}



        //void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (animationLifecycle == 2)
        //        {
        //            animationLifecycle = 0;

        //            var parentAnimation = new Animation();

        //            parentAnimation.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
        //                1, 0, Easing.Linear));


        //            parentAnimation.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
        //                1, 0, Easing.Linear));


        //            parentAnimation.Commit(this, "FrameCardCollapse", 16, 1000);

        //            return;
        //        }

        //        if (animationLifecycle == 1)
        //        {
        //            animationLifecycle = 2;


        //            AnimateTransitionAsync();

        //            var parentAnimation1 = new Animation();
        //            var parentAnimation2 = new Animation();

        //            parentAnimation1.Add(0.00, 0.45, new Animation((v) => frameButtons.TranslationY = v,
        //                  50, 0, Easing.SpringOut));
        //            parentAnimation1.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
        //                0, 1, Easing.Linear));

        //            parentAnimation1.Commit(this, "FrameCardExpand1", 16, 1000);

        //            parentAnimation2.Add(0.00, 0.45, new Animation((v) => scrollData.TranslationY = v,
        //                    50, 0, Easing.SpringOut));
        //            parentAnimation2.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
        //                0, 1, Easing.Linear));


        //            parentAnimation2.Commit(this, "FrameCardExpand2", 16, 1000);

        //        }

        //        if (animationLifecycle == 0)
        //        {
        //            AnimateTransitionAsync();

        //            animationLifecycle = 1;
        //        }

              
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}
    }
}
