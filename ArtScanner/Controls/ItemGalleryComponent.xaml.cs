
using System;
using ArtScanner.Models.Entities;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public partial class ItemGalleryComponent : Grid
    {
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

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            AnimateTransitionAsync();
        }

        private void AnimateTransitionAsync()
        {
            var parentAnimation = new Animation();

            if (_cardState == CardState.Expanded)
            {
                //parentAnimation.Add(0.00, 0.25, CreateLearnMoreAnimation(_cardState));
                //parentAnimation.Add(0.00, 0.50, CreateImageAnimation(_cardState));

                // animate in the details scroller

                parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.TranslationY = v,
                    50, 0, Easing.SpringOut));
                parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.Opacity = v,
                    0, 1, Easing.Linear));

                // animate in the top bar
                //parentAnimation.Add(0.75, 0.85, new Animation((v) => titleItem.TranslationY = v,
                //    -20, 0, Easing.Linear));
                //parentAnimation.Add(0.75, 0.85, new Animation((v) => titleItem.Opacity = v,
                //    0, 1, Easing.Linear));
                _cardState = CardState.Collapsed;

                parentAnimation.Commit(this, "CardExpand", 16, 1300);
            }
            else
            {
                //parentAnimation.Add(0.25, 0.45, CreateImageAnimation(_cardState));
                //parentAnimation.Add(0.35, 0.45, CreateLearnMoreAnimation(_cardState));

                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.TranslationY = v,
                        0, 50, Easing.SpringIn));
                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.Opacity = v,
                    1, 0, Easing.Linear));

                // animate out the top bar
                //parentAnimation.Add(0.00, 0.10, new Animation((v) => titleItem.TranslationY = v,
                //    0, -20, Easing.Linear));
                //parentAnimation.Add(0.0, 0.10, new Animation((v) => titleItem.Opacity = v,
                //    1, 0, Easing.Linear));
                _cardState = CardState.Expanded;

                parentAnimation.Commit(this, "CardExpand", 16, 1000);
            }

            
        }

        async void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
            await artItemImage.LayoutTo(new Rectangle(artItemImage.Bounds.X, artItemImage.Bounds.Y, artItemImage.Bounds.Width, artItemImage.Bounds.Width), 500, Easing.SpringIn);

            AnimateTransitionAsync();

            var parentAnimation = new Animation();

            parentAnimation.Add(0.00, 0.45, new Animation((v) => frameButtons.TranslationY = v,
                  50, 0, Easing.SpringOut));
            parentAnimation.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
                0, 1, Easing.Linear));


            parentAnimation.Add(0.00, 0.45, new Animation((v) => scrollData.TranslationY = v,
                    50, 0, Easing.SpringOut));
            parentAnimation.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
                0, 1, Easing.Linear));


            parentAnimation.Commit(this, "FrameCardExpand", 16, 1000);
        }
        //private Animation CreateLearnMoreAnimation(CardState cardState)
        //{
        //    // work out where the top of the card should be
        //    var learnMoreAnimationStart = cardState == CardState.Expanded ? 0 : 100;
        //    var learnMoreAnimationEnd = cardState == CardState.Expanded ? 100 : 0;

        //    var learnMoreAnim = new Animation(
        //        v =>
        //        {
        //            //LearnMoreLabel.TranslationX = v;
        //            //LearnMoreLabel.Opacity = 1 - (v / 100);
        //        },
        //        learnMoreAnimationStart,
        //        learnMoreAnimationEnd,
        //        Easing.SinInOut
        //        );
        //    return learnMoreAnim;

        //}

        //private Animation CreateImageAnimation(CardState cardState)
        //{
        //    // work out where the top of the card should be
        //    var imageAnimationStart = cardState == CardState.Expanded ? 50 : 0;
        //    var imageAnimationEnd = cardState == CardState.Expanded ? 0 : 50;

        //    var imageAnim = new Animation(
        //        v =>
        //        {
        //            //artItemImage.TranslationY = v;
        //        },
        //        imageAnimationStart,
        //        imageAnimationEnd,
        //        Easing.SpringOut
        //        );
        //    return imageAnim;

        //}
    }
}
