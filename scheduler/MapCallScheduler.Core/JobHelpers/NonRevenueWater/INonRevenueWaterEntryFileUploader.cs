namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public interface INonRevenueWaterEntryFileUploader
    {
        #region Abstract Methods

        void UploadNonRevenueWaterEntries(string file);

        #endregion
    }
}
