
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

        private CardState _cardState = CardState.Collapsed;


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
            //titleItem.IsVisible = false;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            //if (position == 0)
            //{
            //    titleItem.IsVisible = true;

            //    artItemImage.TranslateTo(this.X, this.Y - 120);
            //    titleItem.TranslateTo(this.X, this.Y - 35);

            //    position = 1;
            //}
            //else
            //{
            //    artItemImage.TranslateTo(this.X, this.Y);
            //    titleItem.TranslateTo(this.X, this.Y);

            //    titleItem.IsVisible = false;


            //    position = 0;
            //}
            GoToState(CardState.Expanded);

        }


        public void GoToState(CardState cardState)
        {
            // chec we are actually changing state
            if (_cardState == cardState)
                return;

            MessagingCenter.Send<CardEvent>(new CardEvent(), cardState.ToString());
            AnimateTransition(cardState);
            _cardState = cardState;
            //HeroDetails.InputTransparent = _cardState == CardState.Collapsed;

        }

        private void AnimateTransition(CardState cardState)
        {
            var parentAnimation = new Animation();

            if (cardState == CardState.Expanded)
            {
                parentAnimation.Add(0.00, 0.25, CreateLearnMoreAnimation(cardState));
                parentAnimation.Add(0.00, 0.50, CreateImageAnimation(cardState));

                // animate in the details scroller
                parentAnimation.Add(0.60, 0.85, new Animation((v) => titleItem.TranslationY = v,
                    200, 0, Easing.SpringOut));
                parentAnimation.Add(0.60, 0.85, new Animation((v) => titleItem.Opacity = v,
                    0, 1, Easing.Linear));

                // animate in the top bar
                parentAnimation.Add(0.75, 0.85, new Animation((v) => titleItem.TranslationY = v,
                    -20, 0, Easing.Linear));
                parentAnimation.Add(0.75, 0.85, new Animation((v) => titleItem.Opacity = v,
                    0, 1, Easing.Linear));

            }
            else
            {
                parentAnimation.Add(0.25, 0.45, CreateImageAnimation(cardState));
                parentAnimation.Add(0.35, 0.45, CreateLearnMoreAnimation(cardState));

                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.TranslationY = v,
                        0, 200, Easing.SpringIn));
                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.Opacity = v,
                    1, 0, Easing.Linear));

                // animate out the top bar
                parentAnimation.Add(0.00, 0.10, new Animation((v) => titleItem.TranslationY = v,
                    0, -20, Easing.Linear));
                parentAnimation.Add(0.0, 0.10, new Animation((v) => titleItem.Opacity = v,
                    1, 0, Easing.Linear));
            }

            parentAnimation.Commit(this, "CardExpand", 16, 2000);
        }

        private Animation CreateLearnMoreAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var learnMoreAnimationStart = cardState == CardState.Expanded ? 0 : 100;
            var learnMoreAnimationEnd = cardState == CardState.Expanded ? 100 : 0;

            var learnMoreAnim = new Animation(
                v =>
                {
                    //LearnMoreLabel.TranslationX = v;
                    //LearnMoreLabel.Opacity = 1 - (v / 100);
                },
                learnMoreAnimationStart,
                learnMoreAnimationEnd,
                Easing.SinInOut
                );
            return learnMoreAnim;

        }

        private Animation CreateImageAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var imageAnimationStart = cardState == CardState.Expanded ? 50 : 0;
            var imageAnimationEnd = cardState == CardState.Expanded ? 0 : 50;

            var imageAnim = new Animation(
                v =>
                {
                    //HeroImage.TranslationY = v;
                },
                imageAnimationStart,
                imageAnimationEnd,
                Easing.SpringOut
                );
            return imageAnim;

        }
    }
}
