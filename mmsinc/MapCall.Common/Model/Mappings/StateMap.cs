using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StateMap : ClassMap<State>
    {
        #region Constructors

        public StateMap()
        {
            Id(x => x.Id, "StateID");

            Map(x => x.Name)
               .Length(State.MaxLengths.NAME);
            Map(x => x.Abbreviation)
               .Length(State.MaxLengths.ABBREVIATION);
            Map(x => x.ScadaTable, "ScadaTbl")
               .Length(State.MaxLengths.SCADA_TBL);

            HasMany(x => x.Counties).KeyColumn("StateID");
            HasMany(x => x.Towns).KeyColumn("StateID");
            HasMany(x => x.WorkDescriptionOverrides).KeyColumn("StateId").Cascade.All().Inverse();
        }

        #endregion
    }
}
