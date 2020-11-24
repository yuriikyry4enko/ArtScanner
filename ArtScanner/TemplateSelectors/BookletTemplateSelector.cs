using System;
using ArtScanner.Models.Entities;
using Xamarin.Forms;

namespace ArtScanner.TemplateSelectors
{
    public class BookletTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate { get; set; }

        public DataTemplate ItemTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ItemEntity)item).IsFolder ? FolderTemplate : ItemTemplate;
        }
    }
}