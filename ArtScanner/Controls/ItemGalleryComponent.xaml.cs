using System;
using System.Diagnostics;
using ArtScanner.Models.Entities;
using Prism.Commands;
using Xamarin.Forms;

namespace ArtScanner.Controls
{
    public interface ISwipeCallBack
    {
        void onLeftSwipe(View view);
        void onRightSwipe(View view);
        void onTopSwipe(View view);
        void onBottomSwipe(View view);
        void onNothingSwiped(View view);
    }

    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack mISwipeCallback;
        private double translatedX = 0, translatedY = 0;

        private View _pageTransition;
        private View _currentView; 

        public SwipeListener(View view, View pagetransition, ISwipeCallBack iSwipeCallBack)
        {
            _currentView = view;
            _pageTransition = pagetransition;
            mISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {

            View Content = (View)sender;

            switch (e.StatusType)
            {

                case GestureStatus.Running:

                    try
                    {
                        translatedX = e.TotalX;
                        //translatedY = e.TotalY;

                        //_pageTransition.TranslationX += translatedX;
                        System.Diagnostics.Debug.WriteLine("translatedX : " + translatedX);
                        //System.Diagnostics.Debug.WriteLine("translatedY : " + translatedY);

                        if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                        {
                            mISwipeCallback.onLeftSwipe(Content);

                            _pageTransition.TranslationX += translatedX;
                        }
                        else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                        {
                            mISwipeCallback.onRightSwipe(Content);

                            _pageTransition.TranslationX += translatedX;
                        }
                        else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX))
                        {
                            mISwipeCallback.onTopSwipe(Content);
                        }
                        else if (translatedY > 0 && translatedY > Math.Abs(translatedX))
                        {
                            mISwipeCallback.onBottomSwipe(Content);
                        }
                        else
                        {
                            mISwipeCallback.onNothingSwiped(Content);
                        }


                    }
                    catch (Exception err)
                    {
                        System.Diagnostics.Debug.WriteLine("" + err.Message);
                    }
                    break;

                case GestureStatus.Completed:

                    break;

            }
        }

    }

    public partial class ItemGalleryComponent : Grid, ISwipeCallBack
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

            //SwipeListener swipeListener = new SwipeListener(artItemImage, artItem, this);

        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (animationLifecycle == 0)
            {
                AnimateTransitionAsync();

                animationLifecycle = 1;
            }
        }

        private void AnimateTransitionAsync()
        {
            var parentAnimation = new Animation();

            if (_cardState == CardState.Expanded)
            {
                parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.TranslationY = v,
                    50, 0, Easing.SpringOut));
                parentAnimation.Add(0.00, 0.45, new Animation((v) => titleItem.Opacity = v,
                    0, 1, Easing.Linear));

                _cardState = CardState.Collapsed;

                parentAnimation.Commit(this, "CardExpand", 16, 1300);
            }
            else
            {
                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.TranslationY = v,
                        0, 50, Easing.SpringIn));
                parentAnimation.Add(0.00, 0.35, new Animation((v) => titleItem.Opacity = v,
                    1, 0, Easing.Linear));

                _cardState = CardState.Expanded;

                parentAnimation.Commit(this, "CardUndoExpand", 16, 1000);
            }
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle the swipe
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    break;
            }
        }


        void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (animationLifecycle == 2)
                {
                    animationLifecycle = 0;

                    var parentAnimation = new Animation();

                    parentAnimation.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
                        1, 0, Easing.Linear));


                    parentAnimation.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
                        1, 0, Easing.Linear));


                    parentAnimation.Commit(this, "FrameCardCollapse", 16, 1000);

                    return;
                }

                if (animationLifecycle == 1)
                {
                    animationLifecycle = 2;


                    AnimateTransitionAsync();

                    var parentAnimation1 = new Animation();
                    var parentAnimation2 = new Animation();

                    parentAnimation1.Add(0.00, 0.45, new Animation((v) => frameButtons.TranslationY = v,
                          50, 0, Easing.SpringOut));
                    parentAnimation1.Add(0.00, 0.45, new Animation((v) => frameButtons.Opacity = v,
                        0, 1, Easing.Linear));

                    parentAnimation1.Commit(this, "FrameCardExpand1", 16, 1000);

                    parentAnimation2.Add(0.00, 0.45, new Animation((v) => scrollData.TranslationY = v,
                            50, 0, Easing.SpringOut));
                    parentAnimation2.Add(0.00, 0.45, new Animation((v) => scrollData.Opacity = v,
                        0, 1, Easing.Linear));


                    parentAnimation2.Commit(this, "FrameCardExpand2", 16, 1000);

                }

                if (animationLifecycle == 0)
                {
                    AnimateTransitionAsync();

                    animationLifecycle = 1;
                }

              
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void onLeftSwipe(View view)
        {
            
        }

        public void onRightSwipe(View view)
        {
           
        }

        public void onTopSwipe(View view)
        {
            
        }

        public void onBottomSwipe(View view)
        {
            
        }

        public void onNothingSwiped(View view)
        {
            
        }
    }
}
