using MapCallScheduler.Library.Common;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    public interface ISapEntityUpdaterService
    {
        #region Abstract Methods

        void Process(FileData sapFile);

        #endregion
    }
}