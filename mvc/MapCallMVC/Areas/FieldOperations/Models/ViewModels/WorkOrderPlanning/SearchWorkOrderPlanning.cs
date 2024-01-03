using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderPlanning
{
    public class SearchWorkOrderPlanning : SearchWorkOrder
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties
             
        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        [DropDown(
            "",
            "User",
            "GetByOperatingCenterId",
            DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(User))]
        public int? OfficeAssignment { get; set; }

        [Search(CanMap = false)]
        public bool MarkoutToBeCalledToday { get; set; }

        /// <summary>
        /// This isn't offered as a search field directly, but rather is set to the current date when the
        /// <see cref="MarkoutToBeCalledToday"/> checkbox is checked.
        /// </summary>
        public DateRange MarkoutToBeCalled { get; set; }

        #endregion

        #region Constructors

        public SearchWorkOrderPlanning(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        protected override void ModifyValuesWhenIdHasValueIsFalse(ISearchMapper mapper)
        {
            base.ModifyValuesWhenIdHasValueIsFalse(mapper);

            if (MarkoutToBeCalledToday)
            {
                mapper.MappedProperties[nameof(MarkoutToBeCalled)].Value = new DateRange {
                    End = _dateTimeProvider.GetCurrentDate().Date,
                    Operator = RangeOperator.Equal
                };
            }
        }

        #endregion
    }
}
