using MapCall.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.ViewModels;
using NHibernate.Criterion;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using NHibernate.Transform;
using NHibernate;
using StructureMap;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.Model.Repositories
{
    // TODO: Rename this. It should be "Order" not "Orders".

    public class ProductionWorkOrdersEquipmentRepository : MapCallSecuredRepositoryBase<ProductionWorkOrderEquipment>,
        IProductionWorkOrdersEquipmentRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        public override RoleModules Role => ROLE;

        public ProductionWorkOrdersEquipmentRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        public IEnumerable<RegulatoryCompliance> GetRegulatoryComplianceReport(ISearchRegulatoryCompliance search)
        {
            var query = Session.QueryOver<ProductionWorkOrderEquipment>();
            var completedSubQuery = QueryOver.Of<ProductionWorkOrderEquipment>();
            var cancelledSubQuery = QueryOver.Of<ProductionWorkOrderEquipment>();
            var incompleteSubQuery = QueryOver.Of<ProductionWorkOrderEquipment>();
            var dateCompletedSubQuery = QueryOver.Of<ProductionWorkOrderEquipment>();
            var lastCompletedSubQuery = QueryOver.Of<ProductionWorkOrderEquipment>();

            search.EnablePaging = false;
            RegulatoryCompliance result = null;

            Equipment equipment = null, compEquipment = null;
            ProductionWorkOrder productionWorkOrder = null,
                                compProductionWorkOrder = null,
                                incompProductionWorkOrder = null,
                                cancProductionWorkOrder = null,
                                dateCompProductionWorkOrder = null,
                                lastCompProductionWorkOrder = null;
            ProductionWorkDescription productionWorkDescription = null, compProductionWorkDescription = null;
            OrderType orderType = null;
            OperatingCenter opc = null, compOpc = null;
            PlanningPlant planningPlant = null, compPlanningPlant = null;
            Facility facility = null, compFacility = null;
            State state = null;

            query.JoinAlias(x => x.Equipment, () => equipment)
                 .JoinAlias(x => x.ProductionWorkOrder, () => productionWorkOrder)
                 .JoinAlias(x => productionWorkOrder.ProductionWorkDescription, () => productionWorkDescription)
                 .JoinAlias(x => productionWorkDescription.OrderType, () => orderType)
                 .JoinAlias(x => productionWorkOrder.OperatingCenter, () => opc)
                 .JoinAlias(x => opc.State, () => state)
                 .JoinAlias(x => productionWorkOrder.PlanningPlant, () => planningPlant)
                 .JoinAlias(x => productionWorkOrder.Facility, () => facility);

            query.WhereRestrictionOn(x => orderType.Id).IsIn(OrderType.COMPLIANCE_ORDER_TYPES);

            if ((search.HasProcessSafetyManagement.HasValue && search.HasProcessSafetyManagement.Value) ||
                (search.HasCompanyRequirement.HasValue && search.HasCompanyRequirement.Value) ||
                (search.HasRegulatoryRequirement.HasValue && search.HasRegulatoryRequirement.Value) ||
                (search.HasOshaRequirement.HasValue && search.HasOshaRequirement.Value) ||
                (search.OtherCompliance.HasValue && search.OtherCompliance.Value))
            {
                if (search.HasProcessSafetyManagement.HasValue && search.HasProcessSafetyManagement.Value)
                {
                    query.Where(x => equipment.HasProcessSafetyManagement);
                }
                if (search.HasCompanyRequirement.HasValue && search.HasCompanyRequirement.Value)
                {
                    query.Where(x => equipment.HasCompanyRequirement);
                }
                if (search.HasRegulatoryRequirement.HasValue && search.HasRegulatoryRequirement.Value)
                {
                    query.Where(x => equipment.HasRegulatoryRequirement);
                }
                if (search.HasOshaRequirement.HasValue && search.HasOshaRequirement.Value)
                {
                    query.Where(x => equipment.HasOshaRequirement);
                }
                if (search.OtherCompliance.HasValue && search.OtherCompliance.Value)
                {
                    query.Where(x => equipment.OtherCompliance);
                }
            }
            else
            {
                query.Where(x => equipment.HasOshaRequirement || equipment.HasRegulatoryRequirement || equipment.HasProcessSafetyManagement || equipment.OtherCompliance || equipment.HasCompanyRequirement);
            }

            // NOTE: Some of the logic below is duplicated for each query to keep the queries simple for each count by avoiding nested loops
            // in a single query
            switch (search.DateReceived.Operator)
            {
                case RangeOperator.Between:
                    query.Where(wo =>
                        productionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        productionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    completedSubQuery.Where(wo =>
                        compProductionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        compProductionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    incompleteSubQuery.Where(wo =>
                        incompProductionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        incompProductionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    cancelledSubQuery.Where(wo =>
                        cancProductionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        cancProductionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    dateCompletedSubQuery.Where(wo =>
                        dateCompProductionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        dateCompProductionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    lastCompletedSubQuery.Where(wo =>
                        lastCompProductionWorkOrder.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        lastCompProductionWorkOrder.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    break;
                case RangeOperator.Equal:
                    query.Where(wo => productionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    completedSubQuery.Where(wo => compProductionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    incompleteSubQuery.Where(wo => incompProductionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    cancelledSubQuery.Where(wo => cancProductionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    dateCompletedSubQuery.Where(wo => dateCompProductionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    lastCompletedSubQuery.Where(wo => lastCompProductionWorkOrder.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    break;
                case RangeOperator.GreaterThan:
                    query.Where(wo => productionWorkOrder.DateReceived > search.DateReceived.End);
                    completedSubQuery.Where(wo => compProductionWorkOrder.DateReceived > search.DateReceived.End);
                    incompleteSubQuery.Where(wo => incompProductionWorkOrder.DateReceived > search.DateReceived.End);
                    cancelledSubQuery.Where(wo => cancProductionWorkOrder.DateReceived > search.DateReceived.End);
                    dateCompletedSubQuery.Where(wo => dateCompProductionWorkOrder.DateReceived > search.DateReceived.End);
                    lastCompletedSubQuery.Where(wo => lastCompProductionWorkOrder.DateReceived > search.DateReceived.End);
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    query.Where(wo => productionWorkOrder.DateReceived >= search.DateReceived.End);
                    completedSubQuery.Where(wo => compProductionWorkOrder.DateReceived >= search.DateReceived.End);
                    incompleteSubQuery.Where(wo => incompProductionWorkOrder.DateReceived >= search.DateReceived.End);
                    cancelledSubQuery.Where(wo => cancProductionWorkOrder.DateReceived >= search.DateReceived.End);
                    dateCompletedSubQuery.Where(wo => dateCompProductionWorkOrder.DateReceived >= search.DateReceived.End);
                    lastCompletedSubQuery.Where(wo => lastCompProductionWorkOrder.DateReceived >= search.DateReceived.End);
                    break;
                case RangeOperator.LessThan:
                    query.Where(wo => productionWorkOrder.DateReceived < search.DateReceived.End);
                    completedSubQuery.Where(wo => compProductionWorkOrder.DateReceived < search.DateReceived.End);
                    incompleteSubQuery.Where(wo => incompProductionWorkOrder.DateReceived < search.DateReceived.End);
                    cancelledSubQuery.Where(wo => cancProductionWorkOrder.DateReceived < search.DateReceived.End);
                    dateCompletedSubQuery.Where(wo => dateCompProductionWorkOrder.DateReceived < search.DateReceived.End);
                    lastCompletedSubQuery.Where(wo => lastCompProductionWorkOrder.DateReceived < search.DateReceived.End);
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    query.Where(wo => productionWorkOrder.DateReceived <= search.DateReceived.End);
                    completedSubQuery.Where(wo => compProductionWorkOrder.DateReceived <= search.DateReceived.End);
                    incompleteSubQuery.Where(wo => incompProductionWorkOrder.DateReceived <= search.DateReceived.End);
                    cancelledSubQuery.Where(wo => cancProductionWorkOrder.DateReceived <= search.DateReceived.End);
                    dateCompletedSubQuery.Where(wo => dateCompProductionWorkOrder.DateReceived <= search.DateReceived.End);
                    lastCompletedSubQuery.Where(wo => lastCompProductionWorkOrder.DateReceived <= search.DateReceived.End);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (search.Description != null)
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => equipment.Description).IsLike($"%{search.Description}%"));
            }
            if (search.Facility != null && search.Facility.Any())
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => facility.Id).IsIn(search.Facility));
            }
            if (search.PlanningPlant != null && search.PlanningPlant.Any())
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => planningPlant.Id).IsIn(search.PlanningPlant));
            }
            if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => opc.Id).IsIn(search.OperatingCenter));
            }
            if (search.State != null && search.State.Any())
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => state.Id).IsIn(search.State));
            }

            query.SelectList(x => x.SelectGroup(y => state.Id).WithAlias(() => result.StateId)
                                   .SelectGroup(y => state.Abbreviation).WithAlias(() => result.State)
                                   .SelectGroup(y => opc.Id).WithAlias(() => result.OperatingCenterId)
                                   .SelectGroup(y => opc.OperatingCenterName)
                                   .WithAlias(() => result.OperatingCenterName)
                                   .SelectGroup(y => opc.OperatingCenterCode)
                                   .WithAlias(() => result.OperatingCenterCode)
                                   .SelectGroup(y => planningPlant.Description)
                                   .WithAlias(() => result.PlanningPlantDescription)
                                   .SelectGroup(y => planningPlant.Code).WithAlias(() => result.PlanningPlantCode)
                                   .SelectGroup(y => planningPlant.Id).WithAlias(() => result.PlanningPlantId)
                                   .SelectGroup(y => facility.Id).WithAlias(() => result.FacilityId)
                                   .SelectGroup(y => facility.FacilityName).WithAlias(() => result.Facility)
                                   .SelectGroup(y => facility.PublicWaterSupply).WithAlias(() => result.PublicWaterSupply)
                                   .SelectGroup(y => equipment.Id).WithAlias(() => result.EquipmentId)
                                   .SelectGroup(y => equipment.Description).WithAlias(() => result.Description)
                                   .SelectGroup(y => equipment.EquipmentPurpose).WithAlias(() => result.EquipmentPurpose)
                                   .SelectGroup(y => equipment.EquipmentType)
                                   .WithAlias(() => result.EquipmentType)
                                   .SelectGroup(y => equipment.HasCompanyRequirement)
                                   .WithAlias(() => result.HasCompanyRequirement)
                                   .SelectGroup(y => equipment.HasOshaRequirement)
                                   .WithAlias(() => result.HasOshaRequirement)
                                   .SelectGroup(y => equipment.HasProcessSafetyManagement)
                                   .WithAlias(() => result.HasProcessSafetyManagement)
                                   .SelectGroup(y => equipment.HasRegulatoryRequirement)
                                   .WithAlias(() => result.HasRegulatoryRequirement)
                                   .SelectGroup(y => equipment.OtherCompliance).WithAlias(() => result.OtherCompliance)
                                   .SelectGroup(y => equipment.OtherComplianceReason)
                                   .WithAlias(() => result.OtherComplianceReason)
                                   .SelectSubQuery(completedSubQuery.JoinAlias(y => y.Equipment, () => compEquipment)
                                                                    .JoinAlias(y => y.ProductionWorkOrder, () => compProductionWorkOrder)
                                                                    .JoinAlias(y => compProductionWorkOrder.OperatingCenter, () => compOpc)
                                                                    .JoinAlias(y => compProductionWorkOrder.PlanningPlant, () => compPlanningPlant)
                                                                    .JoinAlias(y => compProductionWorkOrder.Facility, () => compFacility)
                                                                    .JoinAlias(y => compProductionWorkOrder.ProductionWorkDescription, () => compProductionWorkDescription)
                                                                    .Where(_ => compOpc.Id == opc.Id)
                                                                    .Where(_ => compPlanningPlant.Id == planningPlant.Id)
                                                                    .Where(_ => compFacility.Id == facility.Id)
                                                                    .Where(_ => compEquipment.Id == equipment.Id)
                                                                    .Where(_ => compProductionWorkDescription.OrderType.Id.IsIn(OrderType.COMPLIANCE_ORDER_TYPES))
                                                                    .Where(_ => compEquipment.HasOshaRequirement ||
                                                                         compEquipment.HasProcessSafetyManagement ||
                                                                         compEquipment.HasRegulatoryRequirement ||
                                                                         compEquipment.HasCompanyRequirement ||
                                                                         compEquipment.OtherCompliance)
                                                                    .Where(_ => compProductionWorkOrder.DateCompleted != null)
                                                                    .Select(Projections.Count<ProductionWorkOrder>(y => y.Id)))
                                   .WithAlias(() => result.NumberCompleted)
                                   .SelectSubQuery(dateCompletedSubQuery.JoinAlias(y => y.Equipment, () => compEquipment)
                                                                        .JoinAlias(y => y.ProductionWorkOrder, () => dateCompProductionWorkOrder)
                                                                        .JoinAlias(y => dateCompProductionWorkOrder.OperatingCenter, () => compOpc)
                                                                        .JoinAlias(y => dateCompProductionWorkOrder.PlanningPlant, () => compPlanningPlant)
                                                                        .JoinAlias(y => dateCompProductionWorkOrder.Facility, () => compFacility)
                                                                        .JoinAlias(y => dateCompProductionWorkOrder.ProductionWorkDescription, () => compProductionWorkDescription)
                                                                        .Where(_ => compOpc.Id == opc.Id)
                                                                        .Where(_ => compPlanningPlant.Id == planningPlant.Id)
                                                                        .Where(_ => compFacility.Id == facility.Id)
                                                                        .Where(_ => compEquipment.Id == equipment.Id)
                                                                        .Where(_ => compProductionWorkDescription.OrderType.Id.IsIn(OrderType.COMPLIANCE_ORDER_TYPES))
                                                                        .Where(_ => compEquipment.HasOshaRequirement ||
                                                                             compEquipment.HasProcessSafetyManagement ||
                                                                             compEquipment.HasRegulatoryRequirement ||
                                                                             compEquipment.HasCompanyRequirement ||
                                                                             compEquipment.OtherCompliance)
                                                                        .Where(_ => dateCompProductionWorkOrder.DateCompleted != null)
                                                                        .Select(y => dateCompProductionWorkOrder.DateCompleted).OrderBy(_ => dateCompProductionWorkOrder.DateCompleted).Desc().Take(1))
                                   .WithAlias(() => result.DateCompleted)
                                   .SelectSubQuery(lastCompletedSubQuery.JoinAlias(y => y.Equipment, () => compEquipment)
                                                                        .JoinAlias(y => y.ProductionWorkOrder, () => lastCompProductionWorkOrder)
                                                                        .JoinAlias(y => lastCompProductionWorkOrder.OperatingCenter, () => compOpc)
                                                                        .JoinAlias(y => lastCompProductionWorkOrder.PlanningPlant, () => compPlanningPlant)
                                                                        .JoinAlias(y => lastCompProductionWorkOrder.Facility, () => compFacility)
                                                                        .JoinAlias(y => lastCompProductionWorkOrder.ProductionWorkDescription, () => compProductionWorkDescription)
                                                                        .Where(_ => compOpc.Id == opc.Id)
                                                                        .Where(_ => compPlanningPlant.Id == planningPlant.Id)
                                                                        .Where(_ => compFacility.Id == facility.Id)
                                                                        .Where(_ => compEquipment.Id == equipment.Id)
                                                                        .Where(_ => compProductionWorkDescription.OrderType.Id.IsIn(OrderType.COMPLIANCE_ORDER_TYPES))
                                                                        .Where(_ => compEquipment.HasOshaRequirement ||
                                                                             compEquipment.HasProcessSafetyManagement ||
                                                                             compEquipment.HasRegulatoryRequirement ||
                                                                             compEquipment.HasCompanyRequirement ||
                                                                             compEquipment.OtherCompliance)
                                                                        .Where(_ => lastCompProductionWorkOrder.DateCompleted != null)
                                                                        .Select(y => lastCompProductionWorkOrder.Id).OrderBy(_ => lastCompProductionWorkOrder.DateCompleted).Desc().Take(1))
                                   .WithAlias(() => result.LastCompletedWorkOrderId)
                                   .SelectSubQuery(cancelledSubQuery.JoinAlias(y => y.Equipment, () => compEquipment)
                                                                    .JoinAlias(y => y.ProductionWorkOrder, () => cancProductionWorkOrder)
                                                                    .JoinAlias(y => cancProductionWorkOrder.OperatingCenter, () => compOpc)
                                                                    .JoinAlias(y => cancProductionWorkOrder.PlanningPlant, () => compPlanningPlant)
                                                                    .JoinAlias(y => cancProductionWorkOrder.Facility, () => compFacility)
                                                                    .JoinAlias(y => cancProductionWorkOrder.ProductionWorkDescription, () => compProductionWorkDescription)
                                                                    .Where(_ => compOpc.Id == opc.Id)
                                                                    .Where(_ => compPlanningPlant.Id == planningPlant.Id)
                                                                    .Where(_ => compFacility.Id == facility.Id)
                                                                    .Where(_ => compEquipment.Id == equipment.Id)
                                                                    .Where(_ => compProductionWorkDescription.OrderType.Id.IsIn(OrderType.COMPLIANCE_ORDER_TYPES))
                                                                    .Where(_ => compEquipment.HasOshaRequirement ||
                                                                         compEquipment.HasProcessSafetyManagement ||
                                                                         compEquipment.HasRegulatoryRequirement ||
                                                                         compEquipment.HasCompanyRequirement ||
                                                                         compEquipment.OtherCompliance)
                                                                    .Where(_ => cancProductionWorkOrder.DateCancelled != null)
                                                                    .Select(Projections.Count<ProductionWorkOrder>(y => y.Id)))
                                   .WithAlias(() => result.NumberCancelled)
                                   .SelectSubQuery(incompleteSubQuery.JoinAlias(y => y.Equipment, () => compEquipment)
                                                                     .JoinAlias(y => y.ProductionWorkOrder, () => incompProductionWorkOrder)
                                                                     .JoinAlias(y => incompProductionWorkOrder.OperatingCenter, () => compOpc)
                                                                     .JoinAlias(y => incompProductionWorkOrder.PlanningPlant, () => compPlanningPlant)
                                                                     .JoinAlias(y => incompProductionWorkOrder.Facility, () => compFacility)
                                                                     .JoinAlias(y => incompProductionWorkOrder.ProductionWorkDescription, () => compProductionWorkDescription)
                                                                     .Where(_ => compOpc.Id == opc.Id)
                                                                     .Where(_ => compPlanningPlant.Id == planningPlant.Id)
                                                                     .Where(_ => compFacility.Id == facility.Id)
                                                                     .Where(_ => compEquipment.Id == equipment.Id)
                                                                     .Where(_ => compProductionWorkDescription.OrderType.Id.IsIn(OrderType.COMPLIANCE_ORDER_TYPES))
                                                                     .Where(_ => compEquipment.HasOshaRequirement ||
                                                                          compEquipment.HasProcessSafetyManagement ||
                                                                          compEquipment.HasRegulatoryRequirement ||
                                                                          compEquipment.HasCompanyRequirement ||
                                                                          compEquipment.OtherCompliance)
                                                                     .Where(_ => incompProductionWorkOrder.DateCompleted == null && incompProductionWorkOrder.DateCancelled == null)
                                                                     .Select(Projections.Count<ProductionWorkOrder>(y => y.Id)))
                                   .WithAlias(() => result.NumberIncomplete));

            query.OrderBy(() => state.Abbreviation).Asc()
                 .ThenBy(() => opc.OperatingCenterCode).Asc()
                 .ThenBy(() => opc.OperatingCenterName).Asc()
                 .ThenBy(() => facility.PublicWaterSupply).Asc()
                 .ThenBy(() => facility.FacilityName).Asc();

            query.TransformUsing(Transformers.AliasToBean<RegulatoryCompliance>());

            var ret = Search(search, query);

            if (search.EquipmentPurpose != null && search.EquipmentPurpose.Any())
            {
                search.SelectedEquipmentTypes = ret.Select(x => x.EquipmentType.Description).Distinct().ToArray();
                search.SelectedEquipmentPurposes = ret.Select(x => x.EquipmentPurpose.Description).Distinct().ToArray();
            }
            else if (search.EquipmentType != null && search.EquipmentType.Any())
            {
                search.SelectedEquipmentTypes = ret.Select(x => x.EquipmentType.Description).Distinct().ToArray();
            }

            if (search.Facility != null && search.Facility.Any())
            {
                search.SelectedStates = ret.Select(x => x.State).Distinct().ToArray();
                search.SelectedOperatingCenters = ret.Select(x => $"{x.OperatingCenterCode} - {x.OperatingCenterName}").Distinct().ToArray();
                search.SelectedPlanningPlants = ret.Select(x => $"{x.PlanningPlantCode} - {x.PlanningPlantDescription}").Distinct().ToArray();
                search.SelectedFacilities = ret.Select(x => x.Facility).Distinct().ToArray();
            }
            else if (search.PlanningPlant != null && search.PlanningPlant.Any())
            {
                search.SelectedStates = ret.Select(x => x.State).Distinct().ToArray();
                search.SelectedOperatingCenters = ret.Select(x => $"{x.OperatingCenterCode} - {x.OperatingCenterName}").Distinct().ToArray();
                search.SelectedPlanningPlants = ret.Select(x => $"{x.PlanningPlantCode} - {x.PlanningPlantDescription}").Distinct().ToArray();
            }
            else if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                search.SelectedStates = ret.Select(x => x.State).Distinct().ToArray();
                search.SelectedOperatingCenters = ret.Select(x => $"{x.OperatingCenterCode} - {x.OperatingCenterName}").Distinct().ToArray();
            }
            else if (search.State != null && search.State.Any())
            {
                search.SelectedStates = ret.Select(x => x.State).Distinct().ToArray();
            }
            return ret;
        }
    }

    public interface IProductionWorkOrdersEquipmentRepository : IRepository<ProductionWorkOrderEquipment>
    {
        IEnumerable<RegulatoryCompliance> GetRegulatoryComplianceReport(ISearchRegulatoryCompliance search);
    }
}