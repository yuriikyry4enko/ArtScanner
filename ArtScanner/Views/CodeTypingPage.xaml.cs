using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtScanner.Utils.Helpers;
using ArtScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodeTypingPage : BasePage
    {
        CodeTypingPageViewModel viewModel;

        public CodeTypingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            codeEntry.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            codeEntry.Unfocus();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            viewModel = (CodeTypingPageViewModel)BindingContext;
        }

        private List<Label> digitsLabels;

        private void CodeEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (digitsLabels == null)
            {
                digitsLabels = new List<Label>() { digit1, digit2, digit3 };
            }

            if (codeEntry.Text == null)
            {
                foreach (var digitLabel in digitsLabels)
                {
                    digitLabel.Text = "-";
                }
                return;
            }

            var digits = codeEntry.Text.OnlyDigits();
            if (digits.Length > 3)
            {
                digits = digits.Substring(0, 3);
            }

            if (digits != codeEntry.Text)
            {
                codeEntry.Text = digits;
                return;
            }

            if (digits.Length == 3)
            {
                viewModel.ContinueCommand.Execute(null);
            }


            for (int i = 0; i < 3; i++)
            {
                digitsLabels[i].Text = i < digits.Length ? digits[i].ToString() : "-";
            }
        }

        private void CodeEntryTapped(object sender, EventArgs e)
        {
            codeEntry.Focus();
            if (!string.IsNullOrEmpty(codeEntry.Text))
            {
                codeEntry.CursorPosition = codeEntry.Text.Length;
            }
            else
            {
                codeEntry.CursorPosition = 0;
            }
        }
    }
}
