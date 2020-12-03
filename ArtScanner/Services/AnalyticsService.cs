using System;
using ArtScanner.Models.Analytics;

namespace ArtScanner.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        #region Fields

        #endregion

        #region Services

        #endregion

        #region Events

        #endregion

        #region Properties

        public AnalyticsServiceProvider Provider { get; private set; }

        #endregion

        #region Bindable Properties

        #endregion

        #region Commands

        #endregion

        #region Ctor
        public AnalyticsService(AnalyticsServiceProvider provider) => (Provider) = (provider);

        public AnalyticsService()
        {

        }


        #endregion

        #region Public Methods

        public void TrackRecord(string message)
        {
            if (Provider == null)
                throw new ArgumentNullException($"Provider can not be null");

            Provider.WriteRecord(message);
        }

        public void SetProvider(AnalyticsServiceProvider provider)
            => Provider = provider;

        #endregion

        #region Private Methods

        #endregion

        #region Command Execute Handles

        #endregion

        #region Protected Methods

        #endregion

    }
}
