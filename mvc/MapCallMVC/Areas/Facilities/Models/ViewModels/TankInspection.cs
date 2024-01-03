 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public abstract class TankInspectionViewModel : ViewModel<TankInspection>
    {
        #region Private Members

        private Facility _displayFacility;
        private OperatingCenter _displayOperatingCenter;
        private ProductionWorkOrder _displayProductionWorkOrder;
        private Equipment _displayEquipmentObj;
        private PublicWaterSupply _displayPublicWaterSupply;
        private IList<string> _listOfPlans;
        private string inspectionTypes;
        
        #endregion

        #region Properties

        #region General Tab

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public virtual int ProductionWorkOrder { get; set; }

        [Required, EntityMap]
        [EntityMustExist(typeof(Facility))]
        public int Facility { get; set; }

        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        public int TankObservedBy { get; set; }

        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        [View(TankInspection.Display.PUBLIC_WATER_SUPPLY)]
        public virtual int? PublicWaterSupply { get; set; }

        [Required]
        [Range(typeof(decimal), TankInspection.ValueRanges.MIN_RANGE, TankInspection.ValueRanges.MAX_RANGE)]
        public virtual decimal TankCapacity { get; set; }

        [StringLength(TankInspection.StringLengths.GENERAL_TAB_LENGTH)]
        public virtual string TankAddress { get; set; }

        [Required]
        [EntityMustExist(typeof(Town))]
        [EntityMap]
        public virtual int? Town { get; set; }

        [Required, Coordinate]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? Coordinate { get; set; }

        [StringLength(TankInspection.StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }
        [View(TankInspection.Display.OBSERVATION_DATE)]
        [DateOnly]
        public virtual DateTime? ObservationDate { get; set; }
        public virtual DateTime? LastObserved { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(TankInspectionType))]
        [View(TankInspection.Display.TANK_INSPECTION_TYPE)]
        public virtual int? TankInspectionType { get; set; }

        #region DisplayProperties

        [DoesNotAutoMap("Needed to set the private instance of this")]
        private Equipment DisplayEquipmentObj
        {
            get
            {
                if (_displayEquipmentObj == null && Equipment.HasValue)
                {
                    _displayEquipmentObj = _container.GetInstance<IEquipmentRepository>()
                                                     .Find(Equipment.GetValueOrDefault());
                }

                return _displayEquipmentObj;
            }
        }

        [DoesNotAutoMap("Display only")]
        public string DisplayFacility
        {
            get
            {
                if (_displayFacility == null && Facility != null)
                {
                    _displayFacility = _container.GetInstance<IFacilityRepository>().Find(Facility);
                }
                return _displayFacility != null ? _displayFacility.ToString() : String.Empty;
            }
        }

        [DoesNotAutoMap("Display only")]
        [View(TankInspection.Display.OPERATING_CENTER)]
        public string DisplayOperatingCenter
        {
            get
            {
                if (_displayOperatingCenter == null && OperatingCenter != null)
                {
                    _displayOperatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
                }
                return _displayOperatingCenter != null ? _displayOperatingCenter.ToString() : String.Empty;
            }
        }

        [DoesNotAutoMap("Display only")]
        [View(TankInspection.Display.PRODUCTION_WORK_ORDER)]
        public string DisplayProductionWorkOrder
        {
            get
            {
                if (_displayProductionWorkOrder == null)
                {
                    _displayProductionWorkOrder = _container.GetInstance<IProductionWorkOrderRepository>().Find(ProductionWorkOrder);
                }
                return _displayProductionWorkOrder != null ? _displayProductionWorkOrder.ToString() : String.Empty;
            }
        }

        [DoesNotAutoMap("Display only")]
        public string DisplayEquipment => DisplayEquipmentObj != null ? DisplayEquipmentObj.ToString() : String.Empty;

        [DoesNotAutoMap("Display only")]
        [View(TankInspection.Display.PUBLIC_WATER_SUPPLY)]
        public string DisplayPublicWaterSupply
        {
            get
            {
                if (_displayPublicWaterSupply == null && PublicWaterSupply != null)
                {
                    _displayPublicWaterSupply = _container.GetInstance<IPublicWaterSupplyRepository>().Find(PublicWaterSupply.Value);
                }
                return _displayPublicWaterSupply != null ? _displayPublicWaterSupply.ToString() : String.Empty;
            }
        }
        
        [DoesNotAutoMap("Display only")]
        [View(TankInspection.Display.PUBLIC_WATER_SUPPLY)]
        public string DisplayObservationInspectionType
        {
            get
            {
                if (DisplayEquipmentObj != null)
                {
                    foreach (var plan in DisplayEquipmentObj.MaintenancePlans)
                    {
                        _listOfPlans.Add(plan.MaintenancePlan.Description);
                    }
                    inspectionTypes = String.Join(" ", _listOfPlans.ToArray());
                }

                return inspectionTypes;
            }
        }

        [DoesNotAutoMap("Display")]
        [View(TankInspection.Display.TANK_INSPECTION_TYPE)]
        public string DisplayTankInspectionType => TankInspectionType != null ? _container.GetInstance<IRepository<TankInspectionType>>().Find(TankInspectionType.Value).ToString() : null;

        #endregion

        #region TankInspection Questions

        [DoesNotAutoMap("Done manually")]
        public List<TankInspectionQuestionViewModel> TankInspectionQuestions { get; set; }

        #endregion

        #endregion

        #endregion

        #region Constructor

        public TankInspectionViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            var viewModelFactory = _container.GetInstance<IViewModelFactory>();
            // ToList() is needed because trying to call GetAll().Select() with viewModelFactory does not work
            // with sql queriesp.
            var questionTypes = _container.GetInstance<IRepository<TankInspectionQuestionType>>().GetAll().ToList();
            TankInspectionQuestions = questionTypes.Select(x =>
                viewModelFactory
                   .BuildWithOverrides<TankInspectionQuestionViewModel>(new {
                        QuestionTypeDescription = x.Description,
                        QuestionGroupId = x.TankInspectionQuestionGroup.Id,
                        TankInspectionQuestionType = x.Id
                    })).ToList();
        }

        public override void Map(TankInspection entity)
        {
            base.Map(entity);

            foreach (var question in entity.TankInspectionQuestions)
            {
                var questionToShow = TankInspectionQuestions.Single(x =>
                    x.TankInspectionQuestionType == question.TankInspectionQuestionType.Id);
                questionToShow.ObservationAndComments = question.ObservationAndComments;
                questionToShow.TankInspectionAnswer = question.TankInspectionAnswer;
                questionToShow.TankInspectionQuestionType = question.TankInspectionQuestionType.Id;
                questionToShow.CorrectiveWoDateCompleted = question.CorrectiveWoDateCompleted;
                questionToShow.CorrectiveWoDateCreated = question.CorrectiveWoDateCreated;
                questionToShow.RepairsNeeded = question.RepairsNeeded;
            }
        }

        public override TankInspection MapToEntity(TankInspection entity)
        {
            entity = base.MapToEntity(entity);
            entity.Facility.Coordinate = entity.Coordinate;
            if (TankCapacity != null && Equipment.HasValue)
            {
                foreach (var characteristic in entity.Equipment.Characteristics)
                {
                    if (characteristic.Field.DisplayField.Contains("TNK_VOLUME"))
                    {
                        characteristic.Value = TankCapacity.ToString();
                    }
                }
            }
            entity.TankInspectionQuestions.Clear();
            if (TankInspectionQuestions != null)
            {
                foreach (var question in TankInspectionQuestions)
                {
                    var questionType = _container.GetInstance<IRepository<TankInspectionQuestionType>>()
                                                 .Where(x => x.Id == question.TankInspectionQuestionType)
                                                 .FirstOrDefault();
                    var questionEntity = new TankInspectionQuestion {
                        TankInspection = entity,
                        TankInspectionQuestionType = questionType,
                        ObservationAndComments = question.ObservationAndComments,
                        TankInspectionAnswer = question.TankInspectionAnswer,
                        RepairsNeeded = question.RepairsNeeded,
                        CorrectiveWoDateCreated = question.CorrectiveWoDateCreated,
                        CorrectiveWoDateCompleted = question.CorrectiveWoDateCompleted
                    };
                    entity.TankInspectionQuestions.Add(question.MapToEntity(questionEntity));
                }
            }

            return entity;
        }

        #endregion
    }
}