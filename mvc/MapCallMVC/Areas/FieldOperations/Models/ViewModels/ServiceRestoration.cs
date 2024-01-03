using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class ServiceRestorationViewModel : ViewModel<ServiceRestoration>
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(User))]
        public int? ApprovedBy { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? ApprovedOn { get; set; }
        public bool Cancel { get; set; }
        public decimal? EstimatedRestorationAmount { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceRestorationContractor))]
        public int? FinalRestorationCompletionBy { get; set; }
        public decimal? FinalRestorationCost { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? FinalRestorationDate { get; set; }
        public string FinalRestorationInvoiceNumber { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(RestorationMethod))]
        public int? FinalRestorationMethod { get; set; }
        public decimal? FinalRestorationAmount { get; set; }
        public decimal? FinalRestorationTrafficControlHours { get; set; }
        [Multiline, AllowHtml]
        public string Notes { get; set; }
        [DropDown,EntityMap,EntityMustExist(typeof(ServiceRestorationContractor))]
        public int? PartialRestorationCompletionBy { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? PartialRestorationDate { get; set; }
        public string PartialRestorationInvoiceNumber { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(RestorationMethod))]
        public int? PartialRestorationMethod { get; set; }
        public virtual decimal? PartialRestorationAmount { get; set; }
        public virtual decimal? PartialRestorationCost { get; set; }
        public virtual decimal? PartialRestorationTrafficControlHours { get; set; }
        [EntityMap, EntityMustExist(typeof(User))]
        public int? RejectedBy { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? RejectedOn { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public int? RestorationType { get; set; }
        public virtual string PurchaseOrderNumber { get; set; }
        public virtual decimal? EstimatedValue { get; set; }

        #endregion

        #region Constructors

        public ServiceRestorationViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override ServiceRestoration MapToEntity(ServiceRestoration entity)
        {
            if (ApprovedOn.HasValue && entity.ApprovedOn != ApprovedOn.Value)
                ApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Id;

            if (RejectedOn.HasValue && entity.RejectedOn != RejectedOn.Value)
                RejectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Id;

            base.MapToEntity(entity);

            return entity;
        }

        #endregion
	}

    public class CreateServiceRestoration : ServiceRestorationViewModel
    {
        #region Properties

        private Service _service;
        [AutoMap(MapDirections.None)]
        public Service DisplayService 
        {
            get
            {
                if (_service == null)
                    _service = _container.GetInstance<IServiceRepository>().Find(Service);
                return _service;
            } 
        }

        // NOT NULLABLE
        [EntityMap, EntityMustExist(typeof(Service))]
        public int Service { get; set; }
        
        #endregion
        
        #region Constructors

		public CreateServiceRestoration(IContainer container) : base(container) {}

        #endregion

        #region Exposed Method

        public override ServiceRestoration MapToEntity(ServiceRestoration entity)
        {
            base.MapToEntity(entity);

            entity.InitiatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            return entity;
        }

        #endregion
    }

    public class EditServiceRestoration : ServiceRestorationViewModel
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        public Service DisplayService { get; set; }

        #endregion

        #region Constructors

		public EditServiceRestoration(IContainer container) : base(container) {}

        #endregion

        #region Public Methods

        public override void Map(ServiceRestoration entity)
        {
            base.Map(entity);
            DisplayService = entity.Service;
        }

        #endregion
    }

    public class SearchServiceRestoration : SearchSet<ServiceRestoration>
    {
        #region Properties

        [DropDown]
        [SearchAlias("Service", "serv", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Service", "serv", "Town.Id")]
        public int? Town { get; set; }

        [DropDown("", "ServiceRestorationContractor", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("Service", "serv", "WorkIssuedTo.Id")]
        public int? WorkIssuedTo { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public bool? Approved { get; set; }

        #endregion
	}
}