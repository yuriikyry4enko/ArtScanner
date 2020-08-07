using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Utils.Constants;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ItemsGalleryPageViewModel : BaseViewModel
    {
        public ObservableCollection<ArtModel> Items { get; set; }


        private string _selectedId;
        public string SelectedId
        {
            get => _selectedId;
            set => SetProperty(ref _selectedId, value);
        }

        #region Ctor

        public ItemsGalleryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Items = new ObservableCollection<ArtModel>()
            {
                new ArtModel
                {
                    Id = "1",
                    Title = "Sistine Madonna",
                    Author = "Raffaello Santi",
                    MusicUrl = "https://docs.google.com/uc?export=open&id=1v5fEDgNbA6DU-RM_dt1N7vakpRMVITGg",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7a/RAFAEL_-_Madonna_Sixtina_%28Gem%C3%A4ldegalerie_Alter_Meister%2C_Dresden%2C_1513-14._%C3%93leo_sobre_lienzo%2C_265_x_196_cm%29.jpg/437px-RAFAEL_-_Madonna_Sixtina_%28Gem%C3%A4ldegalerie_Alter_Meister%2C_Dresden%2C_1513-14._%C3%93leo_sobre_lienzo%2C_265_x_196_cm%29.jpg",
                    WikiUrl= "https://en.wikipedia.org/wiki/Sistine_Madonna",
                    Description = "The Sistine Madonna, also called the Madonna di San Sisto, is an oil painting by the Italian artist Raphael. The painting was commissioned in 1512 by Pope Julius II for the church of San Sisto, Piacenza. The canvas was one of the last Madonnas painted by Raphael. Giorgio Vasari called it ''a truly rare and extraordinary work'' The painting was moved to Dresden from 1754 and is well known for its influence in the German and Russian art scene. After World War II, it was relocated to Moscow for a decade before being returned to Germany.",
                },

                new ArtModel
                {
                    Id = "2",
                    Title = "Mona Lisa",
                    Author = "Leonardo da Vinci",
                    MusicUrl = "https://docs.google.com/uc?export=open&id=1nXXW25XzDU2csyl4DBuxNZeQw8knjDgl",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/Mona_Lisa%2C_by_Leonardo_da_Vinci%2C_from_C2RMF_retouched.jpg/600px-Mona_Lisa%2C_by_Leonardo_da_Vinci%2C_from_C2RMF_retouched.jpg",
                    WikiUrl= "https://en.wikipedia.org/wiki/Mona_Lisa",
                    Description = "The Mona Lisa is a half-length portrait painting by the Italian artist Leonardo da Vinci. It is considered an archetypal masterpiece of the Italian Renaissance, and has been described as ''the best known, the most visited, the most written about, the most sung about, the most parodied work of art in the world''.",
                }

            };
        }

        #endregion

        public ICommand NavigateToItemDetail => new DelegateCommand<ArtModel>(async(artModel) =>
        {
            SelectedId = artModel.Id;

            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(artModel));
        });

    }
}
