using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class AllocationPermitWithdrawalNodeViewModel : ViewModel<AllocationPermitWithdrawalNode>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [DisplayName("Allocation Category")]
        [EntityMustExist(typeof(AllocationCategory))]
        [EntityMap]
        [DropDown]
        public virtual int? AllocationCategory { get; set; }

        [DisplayName("Facility")]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        [DropDown]
        public virtual int? Facility { get; set; }

        //public virtual Coordinate Coordinate { get; set; }

        [Coordinate, DisplayName("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        [StringLength(25)]
        public virtual string WellPermitNumber { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        [DisplayName("Allowable GPM")]
        public virtual decimal? AllowableGpm { get; set; }

        [DisplayName("Allowable GPD")]
        public virtual decimal? AllowableGpd { get; set; }

        [DisplayName("Allowable MGM")]
        public virtual decimal? AllowableMgm { get; set; }

        [DisplayName("Capable GPM")]
        public virtual decimal? CapableGpm { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string WithdrawalConstraint { get; set; }

        public virtual bool? HasStandByPower { get; set; }
        public virtual decimal? CapacityUnderStandbyPower { get; set; }

        #endregion

        #region Constructors

        public AllocationPermitWithdrawalNodeViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateAllocationPermitWithdrawalNode : AllocationPermitWithdrawalNodeViewModel
    {
        #region Constructors

        public CreateAllocationPermitWithdrawalNode(IContainer container) : base(container) {}

        #endregion
    }

    public class EditAllocationPermitWithdrawalNode : AllocationPermitWithdrawalNodeViewModel
    {
        #region Constructors

        public EditAllocationPermitWithdrawalNode(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchAllocationPermitWithdrawalNode : SearchSet<AllocationPermitWithdrawalNode>
    {
        #region Properties

        [View("AllocationPermitWithdrawalNodeID")]
        public int? EntityId { get; set; }

        [View("Allocation Category")]
        [DropDown]
        public virtual int? AllocationCategory { get; set; }

        [DropDown]
        [SearchAlias("Facility", "F", "OperatingCenter.Id")]
        public virtual int? OperatingCenter { get; set; }

        [View("Facility")]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter",
             PromptText = "Please select an operating center above")]
        public virtual int? Facility { get; set; }

        #endregion

    }
    
    public class AddAllocationPermitWithdrawalNodeEquipment : ViewModel<AllocationPermitWithdrawalNode>
    {
        #region Properties
        [Required, DropDown, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }
        #endregion

        #region Constructors
        public AddAllocationPermitWithdrawalNodeEquipment(IContainer container) : base(container) { }
        #endregion

        #region Exposed Methods
        public override AllocationPermitWithdrawalNode MapToEntity(AllocationPermitWithdrawalNode entity)
        {
            var equipment = _container.GetInstance<IEquipmentRepository>().Find(Equipment.Value);
            if (!entity.Equipment.Contains(equipment))
                entity.Equipment.Add(equipment);
            return entity;
        }
        #endregion
    }
    public class RemoveAllocationPermitWithdrawalNodeEquipment : ViewModel<AllocationPermitWithdrawalNode>
    {
        [Required, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }
        #region Constructors
        public RemoveAllocationPermitWithdrawalNodeEquipment(IContainer container) : base(container) { }
        #endregion
        #region Exposed Methods
        public override AllocationPermitWithdrawalNode MapToEntity(AllocationPermitWithdrawalNode entity)
        {
            var equipment = _container.GetInstance<RepositoryBase<Equipment>>().Find(Equipment.Value);
            entity.Equipment.Remove(equipment);
            return entity;
        }
        #endregion
    }

    public class AddAllocationPermitAllocationPermitWithdrawalNode : ViewModel<AllocationPermitWithdrawalNode>
    {
        #region Properties
        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(AllocationPermit)), DropDown]
        public int? AllocationPermit { get; set; }
        #endregion
        #region Constructors
        public AddAllocationPermitAllocationPermitWithdrawalNode(IContainer container) : base(container) { }
        #endregion
        #region Exposed Methods
        public override AllocationPermitWithdrawalNode MapToEntity(AllocationPermitWithdrawalNode entity)
        {
            var allocationPermit =
                _container.GetInstance<IRepository<AllocationPermit>>().Find(AllocationPermit.Value);
            if (!entity.AllocationGroupings.Contains(allocationPermit))
                entity.AllocationGroupings.Add(allocationPermit);
            return entity;
        }
        #endregion
    }

    public class RemoveAllocationPermitAllocationPermitWithdrawalNode : ViewModel<AllocationPermitWithdrawalNode>
    {
        #region Properties
        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(AllocationPermit))]
        public int? AllocationPermit { get; set; }
        #endregion

        #region Constructors

        public RemoveAllocationPermitAllocationPermitWithdrawalNode(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods
        public override AllocationPermitWithdrawalNode MapToEntity(AllocationPermitWithdrawalNode entity)
        {
            var allocationPermit = entity.AllocationGroupings.SingleOrDefault(x => x.Id == AllocationPermit.Value);
            entity.AllocationGroupings.Remove(allocationPermit);
            return entity;
        }
        #endregion
    }
}