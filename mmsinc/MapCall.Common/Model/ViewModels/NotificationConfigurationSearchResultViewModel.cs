using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class NotificationConfigurationSearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        public string ContactName { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public Application Application { get; set; }
        public Module Module { get; set; }
        public string Purpose { get; set; }

        #endregion
    }
}
