using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;

namespace MapCall.Common.Validation
{
    /// <summary>
    /// This class exists to reduce duplicating validation code in three seperate view models.
    /// </summary>
    public class BacterialWaterSampleValidationHelper
    {
        #region Fields

        private readonly ISampleSiteRepository _sampleSiteRepository;

        #endregion

        #region Constructors

        public BacterialWaterSampleValidationHelper(ISampleSiteRepository sampleSiteRepository)
        {
            _sampleSiteRepository = sampleSiteRepository;
        }

        #endregion

        #region Private Methods

        private PublicWaterSupply GetPublicWaterSupplyFromSampleSite(int? sampleSite)
        {
            // A BacterialWaterSample can have a null SampleSite, or a SampleSite with a null PWSID.
            // If either of these cases occur then the Cl2Total/Cl2Free values will not be required.
            if (sampleSite == null)
            {
                return null;
            }

            return _sampleSiteRepository.Find(sampleSite.Value).PublicWaterSupply;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns true if the chlorine value is valid for a sample site's PWSID
        /// </summary>
        /// <param name="cl2Total"></param>
        /// <param name="sampleSite"></param>
        /// <returns></returns>
        public bool ValidateTotalChlorineForSampleSite(decimal? cl2Total, int? sampleSite)
        {
            var pwsid = GetPublicWaterSupplyFromSampleSite(sampleSite);
            var reportsTotalChlorine = (pwsid?.TotalChlorineReported).GetValueOrDefault();
            return !reportsTotalChlorine || cl2Total.HasValue;
        }

        /// <summary>
        /// Returns true if the chlorine value is valid for a sample site's PWSID
        /// </summary>
        /// <param name="cl2Total"></param>
        /// <param name="sampleSite"></param>
        /// <returns></returns>
        public bool ValidateFreeChlorineForSampleSite(decimal? cl2Free, int? sampleSite)
        {
            var pwsid = GetPublicWaterSupplyFromSampleSite(sampleSite);
            var reportsTotalChlorine = (pwsid?.FreeChlorineReported).GetValueOrDefault();
            return !reportsTotalChlorine || cl2Free.HasValue;
        }

        #endregion
    }
}
