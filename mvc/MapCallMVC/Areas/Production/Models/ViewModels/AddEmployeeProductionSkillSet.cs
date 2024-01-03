using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddEmployeeProductionSkillSet : ViewModel<Employee>
    {
        [DoesNotAutoMap("Mapped manually")]
        [DropDown, EntityMustExist(typeof(ProductionSkillSet))]
        public int? ProductionSkillSet { get; set; }

        public AddEmployeeProductionSkillSet(IContainer container) : base(container) { }

        public override Employee MapToEntity(Employee entity)
        {
            if (!entity.ProductionSkillSets.Any(x => x.ProductionSkillSet.Id == ProductionSkillSet.Value))
                entity.ProductionSkillSets.Add(new EmployeeProductionSkillSet {
                    Employee = entity, 
                    ProductionSkillSet = _container.GetInstance<IRepository<ProductionSkillSet>>()
                        .Find(ProductionSkillSet.Value)
                });
            return entity;
        }
    }

    public class RemoveEmployeeProductionSkillSet : ViewModel<Employee>
    {
        [DoesNotAutoMap("Mapped manually")]
        [DropDown, EntityMustExist(typeof(EmployeeProductionSkillSet))]
        public int? EmployeeProductionSkillSet { get; set; }

        public RemoveEmployeeProductionSkillSet(IContainer container) : base(container) { }

        public override Employee MapToEntity(Employee entity)
        {
            var productionSkillSet = _container.GetInstance<IRepository<EmployeeProductionSkillSet>>().Find(EmployeeProductionSkillSet.Value);
            entity.ProductionSkillSets.Remove(productionSkillSet);
            return entity;
        }
    }
}
