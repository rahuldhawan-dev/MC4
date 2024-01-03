using System;
using System.Diagnostics.CodeAnalysis;

namespace MapCallImporter.Library
{
    public class ProgressAndStatusChangedArgs : EventArgs
    {
        #region Properties

        [ExcludeFromCodeCoverage]
        public string Status { get; }
        [ExcludeFromCodeCoverage]
        public decimal ProgressPercentage { get; }

        #endregion

        #region Constructors

        public ProgressAndStatusChangedArgs(decimal progressPercentage, string status)
        {
            AssertRange(ProgressPercentage = progressPercentage);
            Status = status;
        }

        #endregion

        #region Private Methods

        private void AssertRange(decimal progressPercentage)
        {
            if (progressPercentage > 100 || progressPercentage < 0)
            {
                throw new ArgumentException("Percentage can only fall within the range from 0 to 100.", nameof(progressPercentage));
            }
        }

        #endregion
    }

    public delegate void ProgressChangedEventHandler(object sender, ProgressAndStatusChangedArgs e);
}
