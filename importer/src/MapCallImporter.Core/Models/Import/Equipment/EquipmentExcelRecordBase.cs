using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings.Users;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.V2;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using EquipmentClass=MapCall.Common.Model.Entities.Equipment;

namespace MapCallImporter.Models.Import.Equipment
{
    public abstract class EquipmentExcelRecordBase<TThis> : ExcelRecordBase<EquipmentClass, MyCreateEquipment, TThis>
        where TThis : EquipmentExcelRecordBase<TThis>
    {
        #region Private Static Members

        private static readonly ConcurrentDictionary<string, int> _facilityCache = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<(int typeId, string manufacturer), int?> _equipmentManufacturerCache = new ConcurrentDictionary<(int typeId, string manufacturer), int?>();
        private static readonly ConcurrentDictionary<string, int?> _abcIndicatorCache = new ConcurrentDictionary<string, int?>();

        private static readonly ConcurrentDictionary<string, int> _operatingCenterCache =
            new ConcurrentDictionary<string, int>();

        private static readonly ConcurrentDictionary<string, User> _createdByCache = new ConcurrentDictionary<string, User>();

        private static readonly IList<EquipmentModel> _equipmentModelCache = new List<EquipmentModel>();

        #endregion

        #region Properties

        [AutoMap(SecondaryPropertyName = "SAPEquipmentId")]
        public int? Equipment { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public string FacilityMC { get; set; }

        [AutoMap(SecondaryPropertyName = "FunctionalLocation")]
        public string FunctionalLoc { get; set; }

        public string Planningplant { get; set; }
        public string Userstatus { get; set; }
        public string Systemstatus { get; set; }
        public string Createdby { get; set; }

        public string ABCindic { get; set; }

        [AutoMap(SecondaryPropertyName =  "SerialNumber")]
        public string ManufSerialNo { get; set; }

        public string Modelnumber { get; set; }

        public string NARUCSpecialMtnNote { get; set; }
        public string NARUCSpecialMtnNoteDet { get; set; }

        public string EquipmentCondition { get; set; }
        public string EquipmentPerformance { get; set; }
        public string EquipmentConsequenceofFailure { get; set; }
        public string EquipmentStaticDynamicType { get; set; }

        public DateTime? DateInstalled { get; set; }

        #endregion

        #region Abstract Properties

        protected abstract int EquipmentTypeId { get; }
        protected abstract string EquipmentType { get; }
        protected abstract int EquipmentPurposeId { get; }

        protected abstract int NARUCSpecialMtnNotesId { get; }
        protected abstract int NARUCSpecialMtnNoteDetailsId { get; }

        #endregion

        #region Private Methods

        protected User MapCreatedBy(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            // a null value for CreatedBy will fail helper.RequredStringValue, but that null value
            // would cause an exception in the cache lookup, so we should skip that
            var tmp = helper.RequiredStringValue(Createdby, index, nameof(Createdby));
            if (tmp == null)
            {
                return null;
            }

            if (_createdByCache.ContainsKey(Createdby))
            {
                return _createdByCache[Createdby];
            }

            var result = uow.SqlQuery($"SELECT RecId FROM {UserMap.TABLE_NAME} WHERE UserName = :userName")
                .SetString("userName", Createdby)
                .SafeUniqueIntResult();

            if (result.HasValue)
            {
                return _createdByCache[Createdby] = new User {Id = result.Value};
            }

            helper.AddFailure($"Could not locate User with {nameof(Createdby)} value '{Createdby}' from row {index}.");
            return null;
        }

        protected int MapEquipmentStatus(int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            if (Systemstatus == "INST")
            {
                switch (Userstatus)
                {
                    case "INSV":
                        return EquipmentStatus.Indices.IN_SERVICE;
                    case "OOS":
                        return EquipmentStatus.Indices.OUT_OF_SERVICE;
                    case "OOS TBIN":
                        return EquipmentStatus.Indices.FIELD_INSTALLED;
                }
            }
            else if (Systemstatus == "INAC INST" && Userstatus == "REMV")
            {
                return EquipmentStatus.Indices.RETIRED;
            }
            else if (Systemstatus == "DLFL INAC INST" && Userstatus == "OOS")
            {
                return EquipmentStatus.Indices.CANCELLED;
            }

            helper.AddFailure($"Unable to map {nameof(Systemstatus)} '{Systemstatus}' and {nameof(Userstatus)} '{Userstatus}' to a MapCall {nameof(EquipmentStatus)}.");
            return -1;
        }

        protected int MapOperatingCenter(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            var planningPlant = helper.RequiredStringValue(Planningplant, index, nameof(PlanningPlant));

            if (string.IsNullOrWhiteSpace(planningPlant))
            {
                throw new InvalidOperationException($"Encountered null or empty value for {nameof(Planningplant)} at row {index}.");
            }

            if (_operatingCenterCache.ContainsKey(planningPlant))
            {
                return _operatingCenterCache[Planningplant];
            }

            var operatingCenterId = uow
                .SqlQuery("SELECT COALESCE(OperatingCenterId, -1) as OperatingCenterId FROM PlanningPlants WHERE Code = :code")
                .SetString("code", Planningplant)
                .AddScalar("OperatingCenterId", typeof(int))
                .UniqueResult<int>();

            if (operatingCenterId < 1)
            {
                helper.AddFailure($"Could not locate {nameof(Planningplant)} with code '{Planningplant}' at row {index}.");
                return operatingCenterId;
            }

            return _operatingCenterCache[Planningplant] = operatingCenterId;
        }

        protected int MapFacility(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            if (string.IsNullOrWhiteSpace(helper.RequiredStringValue(FacilityMC, index, nameof(FacilityMC))))
            {
                return -1;
            }

            if (_facilityCache.ContainsKey(FacilityMC))
            {
                return _facilityCache[FacilityMC];
            }

            var facility = uow.Where<Facility>(f => f.FunctionalLocation == FacilityMC);

            if (facility.Count() > 1)
            {
                helper.AddFailure($"Found more than one facility matching the {nameof(FacilityMC)} value '{FacilityMC}' from row {index}.");
                return -1;
            }
            if (facility.Count() == 1)
            {
                return _facilityCache[FacilityMC] = facility.Single().Id;
            }

            var smaller = string.Join("-", FacilityMC.Split('-').Take(3));
            facility = uow.Where<Facility>(f =>
                f.FunctionalLocation == smaller);

            if (facility == null || !facility.Any())
            {
                helper.AddFailure($"Could not locate a facility with the {nameof(FacilityMC)} value '{FacilityMC}' from row {index}.");
            }
            else if (facility.Count() > 1)
            {
                helper.AddFailure($"Found more than one facility matching the {nameof(FacilityMC)} value '{FacilityMC}' (using '{smaller}') from row {index}.");
            }
            else
            {
                return _facilityCache[FacilityMC] = facility.Single().Id;
            }

            return -1;
        }

        private int? MapABCIndicator(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            if (string.IsNullOrWhiteSpace(ABCindic))
            {
                return null;
            }

            if (_abcIndicatorCache.ContainsKey(ABCindic))
            {
                return _abcIndicatorCache[ABCindic];
            }

            var indicator = uow.Where<ABCIndicator>(i => i.Description.StartsWith(ABCindic)).SingleOrDefault();

            if (indicator == null)
            {
                helper.AddFailure($"Row {index}: Could not find {nameof(ABCindic)} '{ABCindic}'");
                return null;
            }

            return _abcIndicatorCache[ABCindic] = indicator.Id;
        }

        protected override MyCreateEquipment MapExtra(MyCreateEquipment viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.EquipmentPurpose = MapEquipmentPurpose(uow, index, helper);
            viewModel.EquipmentType = MapEquipmentType(uow, index, helper);
            viewModel.EquipmentManufacturer = MapEquipmentManufacturer(uow, index, helper);
            viewModel.EquipmentModel = MapEquipmentModel(uow, index, helper);
            viewModel.OperatingCenter = MapOperatingCenter(uow, index, helper);
            viewModel.Facility = MapFacility(uow, index, helper);
            viewModel.ABCIndicator = MapABCIndicator(uow, index, helper);
            viewModel.EquipmentStatus = MapEquipmentStatus(index, helper);
            viewModel.Condition =
                StringToEntityLookup<EquipmentCondition>(uow, index, helper, nameof(EquipmentCondition),
                    EquipmentCondition);
            viewModel.Performance =
                StringToEntityLookup<EquipmentPerformanceRating>(uow, index, helper, nameof(EquipmentPerformance),
                    EquipmentPerformance);
            viewModel.ConsequenceOfFailure =
                StringToEntityLookup<EquipmentConsequencesOfFailureRating>(uow, index, helper, nameof(EquipmentConsequenceofFailure),
                    EquipmentConsequenceofFailure);
            viewModel.StaticDynamicType =
                StringToEntityLookup<EquipmentStaticDynamicType>(uow, index, helper, nameof(EquipmentStaticDynamicType),
                    EquipmentStaticDynamicType);

            if (viewModel.EquipmentStatus == EquipmentStatus.Indices.RETIRED)
            {
                viewModel.DateRetired = uow.Container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            return viewModel;
        }

        private int? MapEquipmentManufacturer(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            helper.RequiredStringValue(Manufacturer, index, nameof(Manufacturer));

            if (_equipmentManufacturerCache.ContainsKey((EquipmentTypeId, Manufacturer)))
            {
                return _equipmentManufacturerCache[(EquipmentTypeId, Manufacturer)];
            }

            var manufacturers = uow.Where<EquipmentManufacturer>(
                                        m =>
                                            m.EquipmentType.Id == EquipmentTypeId &&
                                            m.Description == Manufacturer &&
                                            m.MapCallDescription == null)
                                   .Select(m => m.Id)
                                   .ToList();

            if (!manufacturers.Any())
            {
                manufacturers = uow.Where<EquipmentManufacturer>(
                                        m =>
                                            m.EquipmentType.Id == EquipmentTypeId &&
                                            m.Description == Manufacturer &&
                                            m.MapCallDescription == Manufacturer)
                                   .Select(m => m.Id)
                                   .ToList();
            }

            if (!manufacturers.Any())
            {
                helper.AddFailure($"Row {index}: Could not find {nameof(Manufacturer)} '{Manufacturer}'");
                return null;
            }

            if (manufacturers.Count > 1)
            {
                helper.AddFailure($"Row {index}: Found more than one {nameof(Manufacturer)} with the description '{Manufacturer}'");
                return null;
            }

            return _equipmentManufacturerCache[(EquipmentTypeId, Manufacturer)] = manufacturers.Single();
        }

        private int? MapEquipmentModel(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            if (string.IsNullOrWhiteSpace(Modelnumber))
            {
                return null;
            }

            var model = uow.Where<EquipmentModel>(m =>
                                m.EquipmentManufacturer.EquipmentType.Id == EquipmentTypeId &&
                                m.EquipmentManufacturer.Description == Manufacturer && m.Description == Modelnumber)
                           .SingleOrDefault();

            return model?.Id;
        }

        protected int MapEquipmentType(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            var type = uow.Find<EquipmentType>(EquipmentTypeId);

            if (type == null)
            {
                helper.AddFailure($"Could not find Equipment Type {EquipmentTypeId}");
                return -1;
            }

            return type.Id;
        }

        protected virtual int? MapEquipmentPurpose(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            EquipmentPurpose purpose = null;

            #if DEBUG
            try
            {
                #endif

                purpose = uow.Find<EquipmentPurpose>(EquipmentPurposeId);

                #if DEBUG
            }
            catch (Exception e)
            {
                throw new NotImplementedException(
                    "EquipmentPurpose for this class has most likely not been set.  The following EquipmentPurposes are available for its SAPEquipemtType:" +
                    Environment.NewLine +
                    string.Join(Environment.NewLine,
                        uow.Find<EquipmentType>(EquipmentTypeId).EquipmentPurposes
                            .Map(t => $"\t{t.Id} - {t.Description}")), e);
            }
            #endif


            if (purpose == null)
            {
                helper.AddFailure($"Cound not find Equipment Purpose {EquipmentPurposeId}");
            }

            return purpose?.Id;
        }

        protected IEnumerable<EquipmentCharacteristic> MapCharacteristics(
            EquipmentClass equipment, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            var mapper = new EquipmentCharacteristicMapper(equipment, uow, index, helper);

            return
                InnerMapCharacteristics(mapper)
                   .MergeWith(new [] {
                        mapper.String(NARUCSpecialMtnNote, nameof(NARUCSpecialMtnNote), NARUCSpecialMtnNotesId),
                        mapper.String(NARUCSpecialMtnNoteDet, nameof(NARUCSpecialMtnNoteDet), NARUCSpecialMtnNoteDetailsId),
                    })
                   .Where(c => c != null);
        }

        protected void ValidateCharacteristics(EquipmentClass entity, IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<EquipmentClass, MyCreateEquipment, TThis> helper)
        {
            foreach (var field in entity.EquipmentType.CharacteristicFields.Where(f => f.Required))
            {
                if (entity.Characteristics.SingleOrDefault(c => c.Field.FieldName == field.FieldName) == null)
                {
                    helper.AddFailure($"Row {index}: Characteristic for requried field '{field.FieldName}' has not been provided.");
                }
            }
        }

        protected EquipmentModel CreateNewEquipmentModel(EquipmentClass ret, IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            var existing = _equipmentModelCache.SingleOrDefault(m =>
                string.Equals(m.Description, Modelnumber, StringComparison.CurrentCultureIgnoreCase) && m.EquipmentManufacturer?.Id == ret.EquipmentManufacturer?.Id);

            if (existing == null)
            {
                existing = new EquipmentModel {
                    Description = Modelnumber,
                    EquipmentManufacturer = ret.EquipmentManufacturer
                };
                _equipmentModelCache.Add(existing);
            }

            return existing;
        }

        #endregion

        #region Abstract Methods

        protected abstract IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper);

        #endregion

        #region Exposed Methods

        public override EquipmentClass MapToEntity(IUnitOfWork uow, int index,
            ExcelRecordItemHelperBase<EquipmentClass> helper)
        {
            var ret = base.MapToEntity(uow, index, helper);

            if (ret != null)
            {
                ret.CreatedBy = MapCreatedBy(uow, index, helper);
                ret.Characteristics = MapCharacteristics(ret, uow, index, helper).ToList();

                if (ret.EquipmentModel == null && !string.IsNullOrWhiteSpace(Modelnumber))
                {
                    ret.EquipmentModel = CreateNewEquipmentModel(ret, uow, index, helper);
                }

                if (!ret.SAPEquipmentId.HasValue)
                {
                    ret.SAPErrorCode = SAP_RETRY_ERROR_CODE;
                }

                ret.Coordinate = ret.Facility.Coordinate;
            }

            return ret;
        }

        public override EquipmentClass MapAndValidate(IUnitOfWork uow, int index,
            ExcelRecordItemValidationHelper<EquipmentClass, MyCreateEquipment, TThis> helper)
        {
            var entity = MapToEntity(uow, index, helper);

            if (entity?.EquipmentPurpose != null)
            {
                ValidateCharacteristics(entity, uow, index, helper);
            }

            return entity;
        }

        public override void PreImport(EquipmentClass[] entities)
        {
            var numberingDict = new Dictionary<string, int>();
            var identifierRegex = new Regex(@"^(.+)-(\d+)$");

            foreach (var entity in entities)
            {
                var match = identifierRegex.Match(entity.Identifier);
                var baseStr = match.Groups[1].Value;
                var number = int.Parse(match.Groups[2].Value);

                if (!numberingDict.ContainsKey(baseStr))
                {
                    numberingDict[baseStr] = number;
                }
                else
                {
                    number = numberingDict[baseStr] + 1;
                    numberingDict[baseStr] = number;
                    entity.Number = number;
                }
            }
        }

        #endregion
    }
}