using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ArtScanner.Models;
using ArtScanner.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApologizeLanguagePopupPage : PopupPage
    {
        //private ApologizeLanguagePopupPageViewModel _viewModel;

        public ApologizeLanguagePopupPage()
        {
            InitializeComponent();
        }


        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    _viewModel = (ApologizeLanguagePopupPageViewModel)BindingContext;
        //}

        //void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        //{
        //    var checkedObject = sender as CultureInfoModelExtended;

        //    if (checkedObject != null)
        //    {
        //        _viewModel.AvailableCulturesList.Where(x => x != checkedObject).All(y => y.IsChecked = false);
        //        _viewModel.SelectedCulture = checkedObject;
        //    }
        //}
    }
}
