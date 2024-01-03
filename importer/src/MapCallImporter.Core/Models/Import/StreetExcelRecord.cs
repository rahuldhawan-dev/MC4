using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using IEnumerableExtensions = MMSINC.ClassExtensions.IEnumerableExtensions.IEnumerableExtensions;

namespace MapCallImporter.Models.Import
{
    public class StreetExcelRecord : ExcelRecordBase<Street, MyCreateStreet, StreetExcelRecord>
    {
        #region Properties

        public string Town { get; set; }
        [AutoMap(SecondaryPropertyName = "Prefix")]
        public string StreetPrefix { get; set; }
        [AutoMap(SecondaryPropertyName = "Name")]
        public string Street { get; set; }
        [AutoMap(SecondaryPropertyName = "Suffix")]
        public string StreetSuffix { get; set; }
        [AutoMap(SecondaryPropertyName = "Town")]
        public int TownID { get; set; }
        public int StateID { get; set; }
        public int CountyID { get; set; }

        #endregion

        #region Private Methods

        private int? MapStreetSuffix(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Street> helper)
        {
            if (string.IsNullOrWhiteSpace(StreetSuffix))
            {
                return null;
            }

            var suffix = uow.SqlQuery($"SELECT Id FROM {StreetSuffixMap.TABLE_NAME} WHERE Description = :description")
                            .SetString("description", StreetSuffix)
                            .SafeUniqueIntResult();

            if (suffix == null)
            {
                helper.AddFailure($"{nameof(StreetSuffix)} value '{StreetSuffix}' at row {index} not found in database.");
            }

            return suffix;
        }

        private int? MapStreetPrefix(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Street> helper)
        {
            if (string.IsNullOrWhiteSpace(StreetPrefix))
            {
                return null;
            }

            var prefix = uow.SqlQuery($"SELECT Id FROM {StreetPrefixMap.TABLE_NAME} WHERE Description = :description")
                            .SetString("description", StreetPrefix)
                            .SafeUniqueIntResult();

            if (prefix == null)
            {
                helper.AddFailure($"{nameof(StreetPrefix)} value '{StreetPrefix}' at row {index} not found in database.");
            }

            return prefix;
        }

        protected IEnumerable<Street> GetMatchingStreets(IUnitOfWork uow, Street entity)
        {
            return uow.Where<Street>(x => x.Town.Id == TownID && x.FullStName == entity.FullStName);
        }

        #endregion

        #region Exposed Methods

        protected override MyCreateStreet MapExtra(MyCreateStreet viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Street> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.Prefix = MapStreetPrefix(uow, index, helper);
            viewModel.Suffix = MapStreetSuffix(uow, index, helper);
            viewModel.IsActive = true;

            return viewModel;
        }

        public override Street MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Street, MyCreateStreet, StreetExcelRecord> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            if (entity == null)
            {
                return null;
            }

            var state = helper.RequiredEntityRef<State>(uow, StateID, index, nameof(StateID));
            var county = helper.RequiredEntityRef<County>(uow, CountyID, index, nameof(CountyID));
            var matchingStreets = GetMatchingStreets(uow, entity);

            if (IEnumerableExtensions.Any(matchingStreets))
            {
                helper.AddFailure($"Street at row {index} has the same prefix, suffix, name, and town as a street already in the database.");
            }
            if (entity?.Town == null || county == null)
            {
                return null;
            }
            if (entity.Town.State.Id != StateID)
            {
                helper.AddFailure($"Street at row {index} has Town {TownID} and State {StateID}, but according to the database that town is in State {entity.Town.State.Id}");
            }
            if (entity.Town.County.Id != CountyID)
            {
                helper.AddFailure($"Street at row {index} has Town {TownID} and County {CountyID}, but according to the database that town is in County {entity.Town.County.Id}");
            }
            if (county.State.Id != StateID)
            {
                helper.AddFailure($"Street at row {index} has County {CountyID} and State {StateID}, but according to the database that county is in State {county.State.Id}");
            }

            return entity;
        }

        #endregion
    }
}
