using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class CreateSampleSite : SampleSiteViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = nameof(State))]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; set => base.OperatingCenter = value;
        }

        [EntityMap,
         EntityMustExist(typeof(TownSection)),
         DropDown("", "TownSection", "ActiveByTownId", DependsOn = nameof(Town))]
        public override int? TownSection { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Street)),
         DropDown("Street", "GetActiveByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? Street { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Street)),
         DropDown("Street", "GetActiveByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? CrossStreet { get; set; }

        #endregion

        #region Constructors

        public CreateSampleSite(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            Status = SampleSiteStatus.Indices.PENDING;
        }

        #endregion
    }
}
