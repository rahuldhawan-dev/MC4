using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    public class CreateSewerMainCleaning : SewerMainCleaningViewModel
    {
        #region Properties

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? Street { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? CrossStreet { get; set; }

        [DropDown("FieldOperations", "Hydrant", "ActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public override int? HydrantUsed { get; set; }

        #endregion

        #region Constructors

        public CreateSewerMainCleaning(IContainer container) : base(container)
        {
            OpeningTwoIsATerminus = false;
        }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            Date = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        public void SetValuesFromNewSewerOpening(SewerOpening sewerOpening)
        {
            OperatingCenter = sewerOpening.OperatingCenter?.Id;
            Town = sewerOpening.Town?.Id;
            Street = sewerOpening.Street?.Id;
            CrossStreet = sewerOpening.IntersectingStreet?.Id;
            Opening1 = sewerOpening.Id;
        }

        #endregion
    }
}