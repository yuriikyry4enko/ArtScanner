using System;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookletItemDetailsPage : BasePage
    {
		private BookletItemDetailsPageViewModel viewModel { get; set; }
		
		private int indexNumber;
		private bool dataLoading;

        public BookletItemDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
			viewModel = (BookletItemDetailsPageViewModel)BindingContext;
		}

        protected override void OnAppearing()
		{
			base.OnAppearing();
			indexNumber = 0;

			try
			{
				CollectionView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
				{
					var item = e.Item as ItemEntity;
					if (item == viewModel.BookletItems[viewModel.BookletItems.Count - 1])
						await AddNextPageData();
				};
			}
			catch(Exception ex)
            {
				LogService.Log(ex);
            }
		}

		private async Task AddNextPageData()
		{
			if (dataLoading)
				return;

			dataLoading = true;

			indexNumber += 10;

			await viewModel.LoadItems(lastItemIndex: indexNumber);

            dataLoading = false;
		}
	}
}
