using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class RemoveSampleSiteBracketSite : ViewModel<SampleSite>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        public int? SampleSiteBracketSiteId { get; set; }

        #endregion

        #region Constructors

        public RemoveSampleSiteBracketSite(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override SampleSite MapToEntity(SampleSite entity)
        {
            // NOTE: Don't call base method.

            var removable = entity.BracketSites.SingleOrDefault(x => x.Id == SampleSiteBracketSiteId.Value);
            if (removable != null)
            {
                entity.BracketSites.Remove(removable);
            }

            return entity;
        }

        #endregion
    }
}