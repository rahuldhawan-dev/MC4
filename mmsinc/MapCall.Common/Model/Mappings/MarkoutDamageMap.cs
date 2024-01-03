using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutDamageMap : ClassMap<MarkoutDamage>
    {
        public MarkoutDamageMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.RequestNumber, "RequestNum")
               .Length(MarkoutDamage.StringLengths.REQUEST_NUM)
               .Nullable();
            Map(x => x.CreatedBy)
               .Not.Nullable()
               .Length(MarkoutDamage.StringLengths.CREATED_BY);
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.Street)
               .Not.Nullable()
               .Length(MarkoutDamage.StringLengths.STREET);
            Map(x => x.NearestCrossStreet)
               .Not.Nullable()
               .Length(MarkoutDamage.StringLengths.CROSS_STREET);
            Map(x => x.Excavator)
               .Length(MarkoutDamage.StringLengths.EXCAVATOR);
            Map(x => x.ExcavatorAddress)
               .Length(MarkoutDamage.StringLengths.EXCAVATOR_ADDRESS);
            Map(x => x.ExcavatorPhone)
               .Length(MarkoutDamage.StringLengths.EXCAVATOR_PHONE);
            Map(x => x.DamageOn).Not.Nullable();
            Map(x => x.DamageComments).Not.Nullable();
            Map(x => x.UtilitiesDamaged);
            Map(x => x.EmployeesOnJob);
            Map(x => x.IsMarkedOut).Not.Nullable();
            Map(x => x.IsMismarked).Not.Nullable();
            Map(x => x.MismarkedByInches).Not.Nullable();
            Map(x => x.ExcavatorDiscoveredDamage).Not.Nullable();
            Map(x => x.ExcavatorCausedDamage).Not.Nullable();
            Map(x => x.Was911Called).Not.Nullable();
            Map(x => x.WerePicturesTaken).Not.Nullable();
            Map(x => x.ApprovedOn);
            Map(x => x.SAPWorkOrderId)
               .Nullable()
               .Length(MarkoutDamage.StringLengths.SAP_WORK_ORDER_ID);

            Map(x => x.HasAttachedPictures)
               .ReadOnly()
               .Formula(String.Format(
                    "(CASE WHEN EXISTS (SELECT 1 FROM {0} dlv WHERE dlv.TableName = '{1}' AND dlv.LinkedId = Id) THEN 1 ELSE 0 END)",
                    CreateDocumentLinkView.VIEW_NAME, nameof(MarkoutDamage) + "s"));

            References(x => x.OperatingCenter)
               .Not.Nullable();
            References(x => x.Town)
               .Not.Nullable();
            References(x => x.Coordinate)
               .Not.Nullable();
            References(x => x.MarkoutDamageToType)
               .Not.Nullable();
            References(x => x.SupervisorSignOffEmployee)
               .Nullable();
            References(x => x.WorkOrder).Nullable();

            HasMany(x => x.MarkoutDamageNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.MarkoutDamageDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.UtilityDamages)
               .Table("MarkoutDamagesMarkoutDamageUtilityDamageTypes")
               .ParentKeyColumn("MarkoutDamageId")
               .ChildKeyColumn("MarkoutDamageUtilityDamageTypeId");
        }
    }
}
