using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class EditSampleSite : SampleSiteViewModel
    {
        #region Properties

        [EntityMap, 
         EntityMustExist(typeof(Street)),
         DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? Street { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Street)),
         DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? CrossStreet { get; set; }

        #endregion

        #region Constructors

        public EditSampleSite(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var sampleSite = _container.GetInstance<IRepository<SampleSite>>()
                                       .Find(Id);
            if (sampleSite == null)
            {
                // break out early. Controller will 404 for this.
                yield break;
            }
            if (CertificationAuthorization && !sampleSite.CanBeCertified)
            {
                yield return new ValidationResult($"It is too soon to certify sample site #{Id}.");
            }

            foreach (var result in base.Validate(validationContext))
            {
                yield return result;
            }
        }

        #endregion
    }
}
