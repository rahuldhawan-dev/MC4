using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class OneCallMarkoutResponseViewModel : ViewModel<OneCallMarkoutResponse>
    {
        #region Constants

        public const string MARKOUT_PERFORMED = "M/O Performed",
            MARKOUT_PERFORMED_VALIDATION_MESSAGE =
                "A technique and one of 'Paint', 'Flag', or 'Stake' must be selected when markout has been performed.";

        #endregion

        #region Properties

        [Required, DateTimePicker]
        public DateTime? CompletedAt { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(OneCallMarkoutResponseStatus))]
        public int? OneCallMarkoutResponseStatus { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(OneCallMarkoutResponseTechnique))]
        public int? OneCallMarkoutResponseTechnique { get; set; }
        [Multiline]
        public string Comments { get; set; }
        public bool? ReqNotified { get; set; }
        public bool? Paint { get; set; }
        public bool? Flag { get; set; }
        public bool? Stake { get; set; }
        public bool? Over500Feet { get; set; }
        public bool? CrewMarkoutIsNeeded { get; set; }
        public int? NumberOfCsmo { get; set; }
        public int? NumberOfCsmoUnableToLocate { get; set; }
        public int? TotalTimeSpentForCsmo { get; set; }

        #endregion

        #region Constructors

        public OneCallMarkoutResponseViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = base.Validate(validationContext);

            if (OneCallMarkoutResponseStatus.HasValue &&
                _container.GetInstance<IRepository<OneCallMarkoutResponseStatus>>()
                    .Find(OneCallMarkoutResponseStatus.Value)
                    .Description == MARKOUT_PERFORMED &&
                ((Paint.IsNullOrFalse() && Flag.IsNullOrFalse() && Stake.IsNullOrFalse()) ||
                 !OneCallMarkoutResponseTechnique.HasValue))
            {
                return new List<ValidationResult> {
                    new ValidationResult(MARKOUT_PERFORMED_VALIDATION_MESSAGE,
                        new[] {"OneCallMarkoutResponseStatus"})
                }.MergeWith(result);
            }

            return result;
        }

        #endregion
    }

    public class CreateOneCallMarkoutResponse : OneCallMarkoutResponseViewModel
    {
        //TODO: Maybe just get the tickets from the last month?

        #region Properties

        [DoesNotAutoMap("used by the controller action")]
        public string IndexQS { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(OneCallMarkoutTicket))]
        public int? OneCallMarkoutTicket { get; set; }

        [AutoMap(MapDirections.None)]
        public OneCallMarkoutTicket OneCallMarkoutTicketDisplay { get; set; }

        #endregion

        #region Constructors

		public CreateOneCallMarkoutResponse(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override OneCallMarkoutResponse MapToEntity(OneCallMarkoutResponse entity)
        {
            base.MapToEntity(entity);
            
            var curUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.CompletedBy = curUser;

            return entity;
        }

        #endregion
    }

    public class EditOneCallMarkoutResponse : OneCallMarkoutResponseViewModel
    {
        #region Constructors

		public EditOneCallMarkoutResponse(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchOneCallMarkoutResponse : SearchSet<OneCallMarkoutResponse>
    {
        #region Properties

        [MultiSelect, DisplayName("CDC Code"), SearchAlias("OneCallMarkoutTicket", "CDCCode")]
        public List<string> CDCCode { get; set; }

        [DropDown, SearchAlias("OneCallMarkoutTicket", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }

        [SearchAlias("OneCallMarkoutTicket", "RequestNumber")]
        public int? RequestNumber { get; set; }

        public DateRange CompletedAt { get; set; }

        [DropDown]
        public int? OneCallMarkoutResponseTechnique { get; set; }

        [DropDown]
        public int? OneCallMarkoutResponseStatus { get; set; }

        #endregion
    }
}