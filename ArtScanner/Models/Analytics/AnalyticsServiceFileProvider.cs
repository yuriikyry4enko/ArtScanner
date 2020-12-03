using System;
namespace ArtScanner.Models.Analytics
{
    public class AnalyticsServiceFileProvider : AnalyticsServiceProvider
    {
        #region Fields

        #endregion

        #region Services

        #endregion

        #region Events

        #endregion

        #region Properties

        public string FilePath { get; }
            
        #endregion

        #region Bindable Properties
            
        #endregion

        #region Commands

        #endregion

        #region Ctor

        public AnalyticsServiceFileProvider(string filePath)
        {
            FilePath = filePath;

            if (IsFileValid())
                return;

            var file = System.IO.File.Create(filePath);
            file.Dispose();
        }

        #endregion

        #region Public Methods

        public override void WriteRecord(string record)
        {
            if (IsFileValid())
                System.IO.File.AppendAllText(FilePath, $"{record}{Environment.NewLine}");
        }

        #endregion

        #region Private Methods

        bool IsFileValid()
            => !string.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath);

        #endregion

        #region Command Execute Handles

        #endregion

        #region Protected Methods

        #endregion

    }
}
