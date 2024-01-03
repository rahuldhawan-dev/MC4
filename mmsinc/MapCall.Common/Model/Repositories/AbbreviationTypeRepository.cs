using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IAbbreviationTypeRepository : IRepository<AbbreviationType>
    {
        #region Abstract Methods

        AbbreviationType GetTownAbbreviationType();
        AbbreviationType GetTownSectionAbbreviationType();
        AbbreviationType GetFireDistrictAbbreviationType();

        #endregion
    }

    public class AbbreviationTypeRepository : RepositoryBase<AbbreviationType>, IAbbreviationTypeRepository
    {
        #region Constructors

        #region Constructor

        public AbbreviationTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #endregion

        #region Public Methods

        public AbbreviationType GetTownAbbreviationType()
        {
            return Find(AbbreviationType.Indices.TOWN);
        }

        public AbbreviationType GetTownSectionAbbreviationType()
        {
            return Find(AbbreviationType.Indices.TOWN_SECTION);
        }

        public AbbreviationType GetFireDistrictAbbreviationType()
        {
            return Find(AbbreviationType.Indices.FIRE_DISTRICT);
        }

        #endregion
    }
}
