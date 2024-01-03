using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSiteProfile : IEntity
    {
        #region Private Fields

        private SampleSiteProfileDisplayItem _display; 

        #endregion  

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Number { get; set; }
        public virtual SampleSiteProfileAnalysisType SampleSiteProfileAnalysisType { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        public virtual string Display
        {
            get
            {
                if (_display == null)
                {
                    _display = new SampleSiteProfileDisplayItem {
                        Number = Number,
                        Name = Name,
                        SampleSiteProfileAnalysisType = SampleSiteProfileAnalysisType
                    };
                }

                return _display.Display;
            }
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }
}
