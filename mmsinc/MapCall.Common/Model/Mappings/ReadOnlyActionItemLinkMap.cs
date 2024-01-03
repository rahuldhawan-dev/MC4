using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ReadOnlyActionItemLinkMap : ClassMap<ReadOnlyActionItemLink>
    {
        public ReadOnlyActionItemLinkMap()
        {
            Table("ActionItemLinkView");
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.DataType);
            References(x => x.ActionItem, "Id").ReadOnly();
            Map(x => x.LinkedId);

            DiscriminateSubClassesOnColumn("TableName").AlwaysSelectWithValue();

            SchemaAction.None();
        }
    }

    public abstract class ActionItemSubClassMap<T> : SubclassMap<ActionItem<T>>
        where T : IThingWithActionItems
    {
        protected ActionItemSubClassMap(string tableName)
        {
            DiscriminatorValue(tableName);
            References(x => x.Entity, "LinkedId").NotFound.Ignore().ReadOnly();
        }
    }

    public class RiskRegisterActionItemMap : ActionItemSubClassMap<RiskRegisterAsset> 
    { 
        public RiskRegisterActionItemMap() : base(nameof(RiskRegisterAsset) + "s") { }
    }

    public class IncidentActionItemMap : ActionItemSubClassMap<Incident>
    {
        public IncidentActionItemMap() : base(nameof(Incident) + "s") { }
    }

    public class NearMissActionItemMap : ActionItemSubClassMap<NearMiss>
    {
        public NearMissActionItemMap() : base(NearMissMap.TABLE_NAME) { }
    }

    public class GeneralLiabilityClaimActionItemMap : ActionItemSubClassMap<GeneralLiabilityClaim>
    {
        public GeneralLiabilityClaimActionItemMap() : base(nameof(GeneralLiabilityClaim) + "s") { }
    }

    public class EndOfPipeExceedanceActionItemMap : ActionItemSubClassMap<EndOfPipeExceedance>
    {
        public EndOfPipeExceedanceActionItemMap() : base(nameof(EndOfPipeExceedance) + "s"){ }
    }

    public class ActionItemLinkViewMap : NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(NHibernate.Dialect.Dialect dialect, NHibernate.Engine.IMapping p,
            string defaultCatalog, string defaultSchema)
        {
            return Migrations._2020.MC2047AddActionItemsTableActionItemTypeTableActionItemsView.CREATE_VIEW;
        }

        public override string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog,
            string defaultSchema)
        {
            return Migrations._2020.MC2047AddActionItemsTableActionItemTypeTableActionItemsView.DROP_VIEW;
        }

        #endregion
    }
}
