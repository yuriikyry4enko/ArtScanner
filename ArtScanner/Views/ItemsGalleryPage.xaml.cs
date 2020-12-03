using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsGalleryPage : BasePage
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

        public ItemsGalleryPage()
        {
            InitializeComponent();
        }


        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (animationLifecycle == 0)
            {
                //if (animationLifecycle == 0)
                //{
                AnimateTransitionAsync();

                //}
            }
            else if(animationLifecycle == 1)
            {
                //animationLifecycle = 2;
            }
            else if(animationLifecycle == 2)
            {
                //animationLifecycle = 1;

                //Animation last items
                var parentAnimation = new Animation();

                parentAnimation.Add(0.00, 0.45, new Animation((v) => gridMultipleInfoBlock.Opacity = v,
                    1, 0, Easing.Linear));


                parentAnimation.Commit(this, "FrameCardCollapse", 16, 1000);

                AnimateTransitionAsync();

                animationLifecycle = 0;
            } 
        }

        private void AnimateTransitionAsync()
        {
            var parentAnimation = new Animation();

            if (_cardState == CardState.Expanded)
            {
                parentAnimation.Add(0.00, 0.45, new Animation((v) => shortItemInfoBlock.TranslationY = v,
                    50, 0, Easing.SpringOut));
                parentAnimation.Add(0.00, 0.45, new Animation((v) => shortItemInfoBlock.Opacity = v,
                    0, 1, Easing.Linear));


                shortItemInfoBlock.HeightRequest = 175;
                gridMultipleInfoBlock.HeightRequest = 0;

                _cardState = CardState.Collapsed;

                parentAnimation.Commit(this, "CardExpand", 16, 1300);
            }
            else
            {
                parentAnimation.Add(0.00, 0.35, new Animation((v) => shortItemInfoBlock.TranslationY = v,
                        0, 50, Easing.SpringIn));
                parentAnimation.Add(0.00, 0.35, new Animation((v) => shortItemInfoBlock.Opacity = v,
                    1, 0, Easing.Linear));

               

                _cardState = CardState.Expanded;

                parentAnimation.Commit(this, "CardUndoExpand", 16, 1000);

                shortItemInfoBlock.HeightRequest = 0;
            }
        }

        
      

        void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
            var parentAnimation = new Animation();

            gridMultipleInfoBlock.HeightRequest = 325;

            parentAnimation.Add(0.00, 0.45, new Animation((v) => gridMultipleInfoBlock.Opacity = v,
                0, 1, Easing.Linear));


            parentAnimation.Commit(this, "FrameCard", 16, 1000);

            AnimateTransitionAsync();

            animationLifecycle = 2;

            return;
        }
    }
}
