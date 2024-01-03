using System.ComponentModel.DataAnnotations;
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
    public class AddSampleSiteBracketSite : ViewModel<SampleSite>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped")]
        [Required, DropDown, EntityMustExist(typeof(SampleSite))]
        public int? BracketSite { get; set; }

        [DoesNotAutoMap("Manually mapped")]
        [Required, DropDown, EntityMustExist(typeof(SampleSiteBracketSiteLocationType))]
        public int? LocationType { get; set; }

        #endregion

        #region Constructors

        public AddSampleSiteBracketSite(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override SampleSite MapToEntity(SampleSite entity)
        {
            // NOTE: No base call for this.

            var bracketSite = new SampleSiteBracketSite {
                SampleSite = entity,
                
                BracketSampleSite = _container.GetInstance<ISampleSiteRepository>().Find(BracketSite.Value),
                BracketSiteLocationType =
                    _container.GetInstance<IRepository<SampleSiteBracketSiteLocationType>>()
                              .Find(LocationType.Value)
            };

            entity.BracketSites.Add(bracketSite);

            return entity;
        }

        #endregion
    }
}