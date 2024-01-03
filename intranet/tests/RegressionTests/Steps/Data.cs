using AuthorizeNet.Utility.NotProvided;
using DeleporterCore.Client;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions.StringExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using NHibernate;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using NHibernate.Stat;
using TechTalk.SpecFlow;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace RegressionTests.Steps
{
    [Binding]
    public class Data
    {
        #region Constants

        const string NEAR_MISS_DOCUMENT = "Near Miss Document";

        public static readonly TestTypeDictionary TYPE_DICTIONARY = new TestTypeDictionary {
            {"action item", typeof(ActionItem), CreateActionItem},
            {"admin user", typeof(User), (v, c, s) => CreateUser(v, c, s, true)},
            {"communication type", typeof(CommunicationType), CreateCommunicationType },
            {"contact without address", typeof(Contact), CreateContactWithoutAddress},
            {"customer coordinate", typeof(CustomerCoordinate), CreateCustomerCoordinate},
            {"data table layout", typeof(DataTableLayout), CreateDataTableLayout},
            {"division", typeof(Division), CreateDivision},
            {"document", typeof(Document), CreateDocument},
            {"document type", typeof(DocumentType), CreateDocumentType},
            {"employee status", typeof(EmployeeStatus), CreateEmployeeStatus},
            {"notification purpose", typeof(NotificationPurpose), CreateNotificationPurpose},
            {"operating center", typeof(OperatingCenter), CreateOperatingCenter},
            {"project", typeof(Project), CreateProject},
            {"public water supply", typeof(PublicWaterSupply), CreatePublicWaterSupply},
            {"role", typeof(OperatingCenter), CreateRole},
            {"state", typeof(MapCall.Common.Model.Entities.State), CreateState},
            {"town", typeof(Town), CreateTown},
            {"town document link", typeof(DocumentLink), CreateTownDocumentLink},
            {"near miss type", typeof(NearMissType), CreateNearMissType },
            {"near miss category", typeof(NearMissCategory), CreateNearMissCategory },
            {"public water supply status", typeof(PublicWaterSupplyStatus), CreatePublicWaterSupplyStatus },
            {"user", typeof(User), (v, c, s) => CreateUser(v, c, s, false)}
        };

        // for the love of all that is good and just and fair in this world
        // please keep this list alphabetical.
        private static readonly NameValueCollection PAGE_STRINGS = new NameValueCollection {
            {"near miss", "HealthAndSafety/NearMiss"}
        };
        private static object CreateActionItem(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var ignoreKeys = new[] { "note", "data type" };

            var linkedItemKey = nvc.AllKeys.Where(k => !ignoreKeys.Contains(k)).First();
            var linkedItem = (IThingWithActionItems)cache.GetOrNull(linkedItemKey, nvc);

            var args = new {
                Note = nvc.GetValueOrDefault("note"),
                DataType = cache.GetValueOrDefault<DataTypeFactory>("data type", nvc),
                LinkedId = linkedItem.Id
            };

            return container.GetInstance<ActionItemFactory>().Create(args);
        }

        private static object CreateTownDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var town = (Town)cache.GetValueOrDefault<TownFactory>("town", nvc);
            return container.GetInstance<DocumentLinkFactory>().Create(new {
                document.DocumentType,
                document.DocumentType.DataType,
                Document = document,
                LinkedId = town.Id
            });
        }

        #endregion

        #region Exposed Methods

        public static void Initialize()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Data
                .SetTypeDictionary(TYPE_DICTIONARY);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data
                .SetFactoryAssembly(typeof(UserFactory).Assembly);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data
                .SetModelAssembly(typeof(User).Assembly);
            MMSINC.Testing.SpecFlow.StepDefinitions.Navigation.SetPageStringDictionary(PAGE_STRINGS);
        }

        #endregion

        #region Step Definitions

        [Given("operating center:? \"([^\"]+)\" has asset type:? \"([^\"]+)\"")]
        public static void GivenIAddAssetTypeToOperatingCenter(string operatingCenterId, string assetTypeId)
        {
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterId);
            var assetType = TestObjectCache.Instance.Lookup<AssetType>("asset type", assetTypeId);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterAssetType = session.Load<AssetType>(assetType.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterAssetType.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType
                {
                    AssetType = deleporterAssetType,
                    OperatingCenter = deleporterOperatingCenter
                });
                session.SaveOrUpdate(deleporterAssetType);
                session.Flush();
                session.Clear();
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in service material:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToServiceMaterial(string operatingCenterIdentifier, string serviceIdentifier)
        {
            var serviceMaterial = TestObjectCache.Instance.Lookup<ServiceMaterial>("service material", serviceIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterServiceMaterial = session.Load<ServiceMaterial>(serviceMaterial.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterServiceMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial { OperatingCenter = deleporterOperatingCenter, ServiceMaterial = deleporterServiceMaterial, NewServiceRecord = true });
                session.SaveOrUpdate(deleporterServiceMaterial);
            });
        }

        
        [Given("equipment:? \"([^\"]+)\" exists in facility:? \"([^\"]+)\"")]
        public static void GivenIAddEquipmentToFacility(string equipmentIdentifier, string facilityIdentifier)
        {
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipment = session.Load<Equipment>(equipment.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterFacility.Equipment.Add(deleporterEquipment);
                session.SaveOrUpdate(deleporterFacility);
            });
        }

        [Given("facility system delivery entry type:? \"([^\"]+)\" exists in facility:? \"([^\"]+)\"")]
        public static void GivenIAddFacilitySysDelEntryToFacility(string sysdelIdentenfier, string facilityIdentifier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<FacilitySystemDeliveryEntryType>("facility system delivery entry type", sysdelIdentenfier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<FacilitySystemDeliveryEntryType>(sysdel.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterSysDel.Facility = deleporterFacility;
                deleporterFacility.FacilitySystemDeliveryEntryTypes.Add(deleporterSysDel);
                session.SaveOrUpdate(deleporterFacility);
            });
        }

        [Given("facility:? \"([^\"]+)\" exists in system delivery entry:? \"([^\"]+)\"")]
        public static void GivenIAddFacilityToSysDelEntry(string facilityIdentifier, string sysdelIdentenfier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<SystemDeliveryEntry>("system delivery entry", sysdelIdentenfier);
            var facility = TestObjectCache.Instance.Lookup<Facility>("facility", facilityIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<SystemDeliveryEntry>(sysdel.Id);
                var deleporterFacility = session.Load<Facility>(facility.Id);
                deleporterSysDel.Facilities.Add(deleporterFacility);
                session.SaveOrUpdate(deleporterSysDel);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in system delivery entry:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToSysDelEntry(string ocIdentifier, string sysdelIdentenfier)
        {
            var sysdel = TestObjectCache.Instance.Lookup<SystemDeliveryEntry>("system delivery entry", sysdelIdentenfier);
            var oc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", ocIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSysDel = session.Load<SystemDeliveryEntry>(sysdel.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(oc.Id);
                deleporterSysDel.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterSysDel);
            });
        }

        [Given("equipment purpose:? \"([^\"]+)\" has equipment sub category:? \"([^\"]+)\"")]
        public static void IAddEquipmentSubCategoryToEquipmentPurpose(string equipmentPurpose, string equipmentSubType)
        {
            var equipmentSub = TestObjectCache.Instance.Lookup<EquipmentSubCategory>("equipment subcategory", equipmentSubType);
            var equipmentPurposeactual = TestObjectCache.Instance.Lookup<EquipmentPurpose>("equipment purpose", equipmentPurpose);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipmentSubType = session.Load<EquipmentSubCategory>(equipmentSub.Id);
                var deleporterEquipmentPurpose = session.Load<EquipmentPurpose>(equipmentPurposeactual.Id);
                deleporterEquipmentPurpose.EquipmentSubCategory = deleporterEquipmentSubType;
                session.SaveOrUpdate(deleporterEquipmentPurpose);
            });
        }

        [Given("water system:? \"([^\"]+)\" exists in operating center:? \"([^\"]+)\"")]
        public static void GivenIAddWaterSystemToOperatingCenter(string waterSystemIdentifier, string operatingCenterIdentifier)
        {
            var waterSystem = TestObjectCache.Instance.Lookup<WaterSystem>("water system", waterSystemIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);


            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterWaterSystem = session.Load<WaterSystem>(waterSystem.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterWaterSystem.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterWaterSystem);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in town:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToTown(string operatingCenterIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = deleporterOperatingCenter, Town = deleporterTown, Abbreviation = "XX" });
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in town:? \"([^\"]+)\" with abbreviation:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToTown(string operatingCenterIdentifier, string townIdentifier, string abbreviation)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = deleporterOperatingCenter, Town = deleporterTown, Abbreviation = abbreviation });
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("waste water system: \"([^\"]+)\" exists in town: \"([^\"]+)\"")]
        public static void GivenIAddWasteWaterSystemToTown(string wasteWaterSystemIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var wasteWaterSystem = TestObjectCache.Instance.Lookup<WasteWaterSystem>("waste water system", wasteWaterSystemIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterWasteWaterSystem = session.Load<WasteWaterSystem>(wasteWaterSystem.Id);
                deleporterTown.WasteWaterSystems.Add(deleporterWasteWaterSystem);
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("public water supply: \"([^\"]+)\" exists in town: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyToTown(string publicWaterSupplyIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterTown.PublicWaterSupplies.Add(deleporterPublicWaterSupply);
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("public water supply firm capacity: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyFirmCapacityToPublicWaterSupply(string publicWaterSupplyFirmCapacityIdentifier, string publicWaterSupplyIdentifier)
        {
            var firmCapacity = TestObjectCache.Instance.Lookup<PublicWaterSupplyFirmCapacity>("public water supply firm capacity", publicWaterSupplyFirmCapacityIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterFirmCapacity = session.Load<PublicWaterSupplyFirmCapacity>(firmCapacity.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.FirmCapacities.Add(deleporterFirmCapacity);
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("public water supply pressure zone: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyPressureZoneToPublicWaterSupply(string publicWaterSupplyPressureZoneIdentifier, string publicWaterSupplyIdentifier)
        {
            var pressureZone = TestObjectCache.Instance.Lookup<PublicWaterSupplyPressureZone>("public water supply pressure zone", publicWaterSupplyPressureZoneIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterPressureZone = session.Load<PublicWaterSupplyPressureZone>(pressureZone.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.PressureZones.Add(deleporterPressureZone);
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("operating center: \"([^\"]+)\" exists in public water supply: \"([^\"]+)\"")]
        public static void GivenIAddPublicWaterSupplyToOperatingCenter(string operatingCenterIdentifier, string publicWaterSupplyIdentifier)
        {
            var opc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var publicWaterSupply = TestObjectCache.Instance.Lookup<PublicWaterSupply>("public water supply", publicWaterSupplyIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterOperatingCenter = session.Load<OperatingCenter>(opc.Id);
                var deleporterPublicWaterSupply = session.Load<PublicWaterSupply>(publicWaterSupply.Id);
                deleporterPublicWaterSupply.OperatingCenterPublicWaterSupplies.Add(new OperatingCenterPublicWaterSupply { OperatingCenter = deleporterOperatingCenter, PublicWaterSupply = deleporterPublicWaterSupply });
                session.SaveOrUpdate(deleporterPublicWaterSupply);
            });
        }

        [Given("meter: \"([^\"]+)\" exists in interconnection: \"([^\"]+)\"")]
        public static void GivenIAddMeterToInterconnection(string meterIdentifier, string interconnectionIdentifier)
        {
            var interconnection = TestObjectCache.Instance.Lookup<Interconnection>("interconnection", interconnectionIdentifier);
            var meter = TestObjectCache.Instance.Lookup<Meter>("meter", meterIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterInterconnection = session.Load<Interconnection>(interconnection.Id);
                var deleporterMeter = session.Load<Meter>(meter.Id);
                deleporterInterconnection.Meters.Add(deleporterMeter);
                session.SaveOrUpdate(deleporterInterconnection);
            });
        }

        //And operating center: "nj7" exists in environmental permit: "one"
        [Given("operating center: \"([^\"]+)\" exists in environmental permit: \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToEnviromentalPermit(string operatingCenterIdentifier, string environmentalPermitIdentifier)
        {
            var opc = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var permit = TestObjectCache.Instance.Lookup<EnvironmentalPermit>("environmental permit", environmentalPermitIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterOperatingCenter = session.Load<OperatingCenter>(opc.Id);
                var deleporterPermit = session.Load<EnvironmentalPermit>(permit.Id);
                deleporterPermit.OperatingCenters.Add(deleporterOperatingCenter);
                session.SaveOrUpdate(deleporterPermit);
            });
        }


        [Given("position group common name: \"([^\"]+)\" exists in training requirement: \"([^\"]+)\"")]
        public static void GivenIAddPositionGroupCommonNameToTrainingRequirement(string positionGroupCommonNameName, string trainingRequirementName)
        {
            var trainingRequirementId = TestObjectCache.Instance.Lookup<TrainingRequirement>("training requirement", trainingRequirementName).Id;
            var positionGroupCommonNameId = TestObjectCache.Instance.Lookup<PositionGroupCommonName>("position group common name", positionGroupCommonNameName).Id;

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var trainingRequirement = session.Load<TrainingRequirement>(trainingRequirementId);
                var jobTitleCommonName = session.Load<PositionGroupCommonName>(positionGroupCommonNameId);
                trainingRequirement.PositionGroupCommonNames.Add(jobTitleCommonName);
                session.SaveOrUpdate(trainingRequirement);
            });
        }

        [Given("^employee:? \"([^\"]+)\" has production skill set:? \"([^\"]+)\"$")]
        public static void GivenEmployeeHasProductionSkillSet(string employeeName, string productionSkillSetName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var productionSkillSetId =
                TestObjectCache.Instance.Lookup<ProductionSkillSet>("production skill set", productionSkillSetName).Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var productionSkillSet = session.Load<ProductionSkillSet>(productionSkillSetId);
                session.Save(new EmployeeProductionSkillSet {
                    Employee = employee,
                    ProductionSkillSet = productionSkillSet
                });
            });
        }

        [Given("^employee: \"([^\"]+)\" is scheduled for training record: \"([^\"]+)\"$")]
        public static void GivenIAddEmployeeToScheduleForTrainingRecord(string employeeName, string trainingRecordName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var trainingRecordId = TestObjectCache.Instance.Lookup<TrainingRecord>("training record", trainingRecordName).Id;
            var dataTypeId = TestObjectCache.Instance.Lookup<DataType>("data type", "employees scheduled").Id;

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var dataType = session.Load<DataType>(dataTypeId);
                session.Save(new EmployeeLink
                {
                    LinkedId = trainingRecordId,
                    DataType = dataType,
                    Employee = employee,
                    LinkedBy = "regression test step",
                    LinkedOn = DateTime.Now
                });
            });
        }

        [Given("^employee: \"([^\"]+)\" attended training record: \"([^\"]+)\"$")]
        public static void GivenANamedEmployeeAttendedATrainingRecord(string employeeName, string trainingRecordName)
        {
            var employeeId = TestObjectCache.Instance.Lookup<Employee>("employee", employeeName).Id;
            var trainingRecordId = TestObjectCache.Instance.Lookup<TrainingRecord>("training record", trainingRecordName).Id;
            var dataTypeId = TestObjectCache.Instance.Lookup<DataType>("data type", "employees attended").Id;

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var employee = session.Load<Employee>(employeeId);
                var dataType = session.Load<DataType>(dataTypeId);
                session.Save(new EmployeeLink
                {
                    LinkedId = trainingRecordId,
                    DataType = dataType,
                    Employee = employee,
                    LinkedBy = "regression test step",
                    LinkedOn = DateTime.Now
                });
            });
        }

        [Given("equipment: \"([^\"]+)\" has production prerequisite: \"([^\"]+)\"")]
        public static void GivenIAddAProductionPrerequisiteToEquipment(string equipmentIdentifier, string productionprereqIdentifier)
        {
            var eq = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);
            var prereq = TestObjectCache.Instance.Lookup<ProductionPrerequisite>("production prerequisite", productionprereqIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterEquipment = session.Load<Equipment>(eq.Id);
                var deleporterPrereq = session.Load<ProductionPrerequisite>(prereq.Id);
                deleporterEquipment.ProductionPrerequisites.Add(deleporterPrereq);
                session.SaveOrUpdate(deleporterEquipment);
            });
        }

        [Given("voltage:? \"([^\"]+)\" exists in utility transformer k v a rating:? \"([^\"]+)\"")]
        public static void GivenVoltageContainsUtilityTransformerKRVRating(string voltage, string rating)
        {
            var voltageId = TestObjectCache.Instance.Lookup<Voltage>("voltage", voltage).Id;
            var ratingId = TestObjectCache.Instance.Lookup<UtilityTransformerKVARating>("utility transformer k v a rating", rating).Id;

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterVoltage = session.Load<Voltage>(voltageId);
                var deleporterRating = session.Load<UtilityTransformerKVARating>(ratingId);
                deleporterVoltage.UtilityTransformerKVARatings.Add(deleporterRating);
                deleporterRating.Voltages.Add(deleporterVoltage);
                session.SaveOrUpdate(deleporterVoltage);
            });
        }

        [Given("meter supplemental location:? \"([^\"]+)\" exists in small meter location:? \"([^\"]+)\"")]
        public static void GivenIAddAMeterSupplementalLocationToAServiceInstallationPosition(string meterSupplementalLocationIdentifier, string smallMeterLocationIdentifier)
        {
            var smallMeterLocation = TestObjectCache.Instance.Lookup<SmallMeterLocation>("small meter location", smallMeterLocationIdentifier);
            var meterSupplementalLocation = TestObjectCache.Instance.Lookup<MeterSupplementalLocation>("meter supplemental location", meterSupplementalLocationIdentifier);

            Deleporter.Run(() =>
            {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterSmallMeterLocation = session.Load<SmallMeterLocation>(smallMeterLocation.Id);
                var deleporterMeterSupplementalLocation = session.Load<MeterSupplementalLocation>(meterSupplementalLocation.Id);
                deleporterSmallMeterLocation.MeterSupplementalLocations.Add(deleporterMeterSupplementalLocation);
                session.SaveOrUpdate(deleporterSmallMeterLocation);
            });
        }

        [Given("states of matter exist")]
        public static void GivenStatesofMatterExist()
        {
            Action<string> createChemicalFormType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("state of matter",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createChemicalFormType("Solid");
            createChemicalFormType("Liquid");
            createChemicalFormType("Gas");
        }

        [Given("communication types exist")]
        public static void GivenCommunicationTypesExist()
        {
            Action<string> createCommunicationType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("communication type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createCommunicationType("Letter");
            createCommunicationType("Electronic");
            createCommunicationType("Upload");
            createCommunicationType("Email");
            createCommunicationType("PDF");
            createCommunicationType("Agency Submittal Form");
            createCommunicationType("Other");
        }

        [Given("work order requesters exist")]
        public static void GivenWorkOrderRequestersExist()
        {
            Action<string> createRequester = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("work order requester",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createRequester("Customer");
            createRequester("Employee");
            createRequester("Local Government");
            createRequester("Call Center");
            createRequester("FRCC");
        }

        [Given("work order purposes exist")]
        public static void GivenWorkOrderPurposesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createPurpose = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createPurpose("customer");
            createPurpose("compliance");
            createPurpose("safety");
            createPurpose("leak detection");
            createPurpose("revenue 150 to 500");
            createPurpose("revenue 500 to 1000");
            createPurpose("revenue above 1000");
            createPurpose("damaged billable");
            createPurpose("estimates");
            createPurpose("water quality");
            createPurpose("asset record control");
            createPurpose("seasonal");
            createPurpose("demolition");
            createPurpose("bpu");
            createPurpose("hurricane sandy");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("work order purpose", nameValues);
        }

        [Given("work order priorities exist")]
        public static void GivenWorkOrderPrioritiesExist()
        {
            Action<string> createPriority = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("work order priority",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createPriority("emergency");
            createPriority("high priority");
            createPriority("routine");
        }

        [Given("production work order priorities exist")]
        public static void GivenProductionWorkOrderPrioritiesExist()
        {
            Action<string> createPriority = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("production work order priority",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createPriority("emergency");
            createPriority("high");
            createPriority("routine");
            createPriority("medium");
            createPriority("low");
            createPriority("routine - off scheduled");
        }

        [Given("equipment statuses exist")]
        public static void GivenEquipmentStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createEquipmentStatus = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createEquipmentStatus("in service");
            createEquipmentStatus("out of service");
            createEquipmentStatus("pending");
            createEquipmentStatus("retired");
            createEquipmentStatus("pending retirement");
            createEquipmentStatus("cancelled");
            createEquipmentStatus("field installed");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("equipment status", nameValues);
        }

        [Given("facility statuses exist")]
        public static void GivenFacilityStatusesExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createFacilityStatus = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createFacilityStatus("active");
            createFacilityStatus("inactive");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("facility status", nameValues);

        }

        [Given("work descriptions exist")]
        public static void GivenWorkDescriptionsExist()
        {
            var nameValues = new Dictionary<string, string>();
            Action<string> createDescription = (desc) =>
            {
                nameValues.Add(desc.ToLowerInvariant(), $"description: \"{desc}\"");
            };

            createDescription("water main bleeders");
            createDescription("change burst meter");
            createDescription("check no water");
            createDescription("curb box repair");
            createDescription("ball curb stop repair");
            createDescription("excavate meter box setter");
            createDescription("service line flow test");
            createDescription("hydrant frozen");
            createDescription("frozen meter set");
            createDescription("frozen service line company side");
            createDescription("frozen service line cust side");
            createDescription("ground water service");
            createDescription("hydrant flushing");
            createDescription("hydrant investigation");
            createDescription("hydrant installation");
            createDescription("hydrant leaking");
            createDescription("hydrant no drip");
            createDescription("hydrant repair");
            createDescription("hydrant replacement");
            createDescription("hydrant retirement");
            createDescription("inactive account");
            createDescription("valve blow off installation");
            createDescription("fire service installation");
            createDescription("install line stopper");
            createDescription("install meter");
            createDescription("interior setting repair");
            createDescription("service investigation");
            createDescription("main investigation");
            createDescription("leak in meter box inlet");
            createDescription("leak in meter box outlet");
            createDescription("leak survey");
            createDescription("meter box setter installation");
            createDescription("meter change");
            createDescription("meter box adjustment resetter");
            createDescription("new main flushing");
            createDescription("service line installation");
            createDescription("service line leak cust side");
            createDescription("service line renewal");
            createDescription("service line retire");
            createDescription("sump pump");
            createDescription("test shut down");
            createDescription("valve box repair");
            createDescription("valve box blow off repair");
            createDescription("service line valve box repair");
            createDescription("valve investigation");
            createDescription("valve leaking");
            createDescription("valve repair");
            createDescription("valve blow off repair");
            createDescription("valve replacement");
            createDescription("valve retirement");
            createDescription("water ban restriction violator");
            createDescription("water main break repair");
            createDescription("water main installation");
            createDescription("water main retirement");
            createDescription("flushing service");
            createDescription("water main break replace");
            createDescription("meter box setter replace");
            createDescription("sewer main break repair");
            createDescription("sewer main break replace");
            createDescription("sewer main retirement");
            createDescription("sewer main installation");
            createDescription("sewer main cleaning");
            createDescription("sewer lateral installation");
            createDescription("sewer lateral repair");
            createDescription("sewer lateral replace");
            createDescription("sewer lateral retire");
            createDescription("sewer lateral customer side");
            createDescription("sewer opening repair");
            createDescription("sewer opening replace");
            createDescription("sewer opening installation");
            createDescription("sewer main overflow");
            createDescription("sewer backup company side");
            createDescription("hydraulic flow test");
            createDescription("markout crew");
            createDescription("valve box replacement");
            createDescription("site inspection survey new service");
            createDescription("site inspection survey service renewal");
            createDescription("service line repair");
            createDescription("sewer clean out installation");
            createDescription("sewer clean out repair");
            createDescription("sewer camera service");
            createDescription("sewer camera main");
            createDescription("sewer demolition inspection");
            createDescription("sewer main test holes");
            createDescription("water main test holes");
            createDescription("valve broken");
            createDescription("ground water main");
            createDescription("service turn on");
            createDescription("service turn off");
            createDescription("meter obtain read");
            createDescription("meter final start read");
            createDescription("meter repair touch pad");
            createDescription("valve installation");
            createDescription("valve blow off replacement");
            createDescription("hydrant paint");
            createDescription("ball curb stop replace");
            createDescription("valve blow off retirement");
            createDescription("valve blow off broken");
            createDescription("water main relocation");
            createDescription("hydrant relocation");
            createDescription("service relocation");
            createDescription("sewer investigation main");
            createDescription("sewer service overflow");
            createDescription("sewer investigation lateral");
            createDescription("sewer investigation opening");
            createDescription("sewer lift station repair");
            createDescription("curb box replace");
            createDescription("service line valve box replace");
            createDescription("storm catch repair");
            createDescription("storm catch replace");
            createDescription("storm catch installation");
            createDescription("storm catch investigation");
            createDescription("hydrant landscaping");
            createDescription("hydrant restoration investigation");
            createDescription("hydrant restoration repair");
            createDescription("main landscaping");
            createDescription("main restoration investigation");
            createDescription("main restoration repair");
            createDescription("service landscaping");
            createDescription("service restoration investigation");
            createDescription("service restoration repair");
            createDescription("sewer lateral landscaping");
            createDescription("sewer lateral restoration investigation");
            createDescription("sewer lateral restoration repair");
            createDescription("sewer main landscaping");
            createDescription("sewer main restoration investigation");
            createDescription("sewer main restoration repair");
            createDescription("sewer opening landscaping");
            createDescription("sewer opening restoration investigation");
            createDescription("sewer opening restoration repair");
            createDescription("valve landscaping");
            createDescription("valve restoration investigation");
            createDescription("valve restoration repair");
            createDescription("storm catch landscaping");
            createDescription("storm catch restoration investigation");
            createDescription("storm catch restoration repair");
            createDescription("rstrn restoration inquiry");
            createDescription("service off at main storm restoration");
            createDescription("service off at curb stop storm restoration");
            createDescription("service off at meter pit storm restoration");
            createDescription("valve turned off storm restoration");
            createDescription("main repair storm restoration");
            createDescription("main replace storm restoration");
            createDescription("hydrant turned off storm restoration");
            createDescription("hydrant replace storm restoration");
            createDescription("valve installation storm restoration");
            createDescription("valve replacement storm restoration");
            createDescription("curb box locate storm restoration");
            createDescription("meter pit locate storm restoration");
            createDescription("valve retirement storm restoration");
            createDescription("excavate meter pit  storm restoration");
            createDescription("service line renewal storm restoration");
            createDescription("curb box replacement storm restoration");
            createDescription("water main retirement storm restoration");
            createDescription("service line retirement storm restoration");
            createDescription("frame and cover replace storm restoration");
            createDescription("pump repair");
            createDescription("line stop repair");
            createDescription("saw repair");
            createDescription("vehicle repair");
            createDescription("misc repair");
            createDescription("z lwc ew4 3 consecutive mths of 0 usage zero");
            createDescription("z lwc ew4 check meter non emergency ckmtr");
            createDescription("z lwc ew4 demolition closed account democ");
            createDescription("z lwc ew4 meter change out mtrch");
            createDescription("z lwc ew4 read mr edit local ops only mredt");
            createDescription("z lwc ew4 read to stop estimate est");
            createDescription("z lwc ew4 repair install reading device rem");
            createDescription("z lwc ew4 reread and or inspect for leak hilow");
            createDescription("z lwc ew4 set meter turn on and read onset");
            createDescription("z lwc ew4 turn on water on");
            createDescription("hydrant nozzle replacement");
            createDescription("hydrant nozzle investigation");
            createDescription("crossing investigation");

            MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObjectsInBulk("work description", nameValues);
        }

        [Given("markout requirements exist")]
        public static void GivenMarkoutRequirementsExist()
        {
            Action<string> createRequirement = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("markout requirement",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createRequirement("none");
            createRequirement("routine");
            createRequirement("emergency");
        }

        [Given("meter supplemental locations exist")]
        public static void GivenMeterSupplementalLocationsExist()
        {
            Action<string> createMeterSupplementalLocation = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("meter supplemental location",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createMeterSupplementalLocation("Inside");
            createMeterSupplementalLocation("Outside");
            createMeterSupplementalLocation("Secure Access");
        }

        [Given("meter directions exist")]
        public static void GivenMeterDirectionsExist()
        {
            Action<string> createMeterDirection = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("meter direction",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createMeterDirection("Front");
            createMeterDirection("Left Side");
            createMeterDirection("Rear");
            createMeterDirection("Right");
            createMeterDirection("Unable to Verify");
        }

        [Given("small meter locations exist")]
        public static void GivenSmallMeterLocationsExist()
        {
            Action<string> createSmallMeterLocation = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("small meter location",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createSmallMeterLocation("Cellar / Basement");
            createSmallMeterLocation("Curb");
            createSmallMeterLocation("Utility Room");
        }

        [Given("equipment types exist")]
        public static void GivenEquipmentTypesExist()
        {
            Action<string> createEquipmentType = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);
            createEquipmentType("GENERATOR");
            createEquipmentType("ENGINE");
            createEquipmentType("AERATOR");
            createEquipmentType("RTU");
            createEquipmentType("FLOW METER");
        }

        [Given("asset upload statuses exist")]
        public static void GivenAssetUploadStatusesExist()
        {
            Action<string> createAssetUploadStatus = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("asset upload status",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createAssetUploadStatus("Pending");
            createAssetUploadStatus("Success");
            createAssetUploadStatus("Error");
        }

        [Given("production prerequisites exist")]
        public static void GivenProductionPrerequisitesExist()
        {
            Action<string> createProductionPrerequisites = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("production prerequisite",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createProductionPrerequisites("has lockout requirement");
            createProductionPrerequisites("is confined space");
            createProductionPrerequisites("job safety list");
            createProductionPrerequisites("air permit");
            createProductionPrerequisites("hot work");
        }

        [Given("system delivery entry types exist with system delivery type:? \"([^\"]+)\"")]
        public static void GivenSystemDeliveryEntryTypesExist(string systemDeliveryType)
        {
            Action<string, string> createEntryType = (desc, systype) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("system delivery entry type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\", system delivery type: \"{systype}\"", TestObjectCache.Instance);

            var systemDelType = TestObjectCache.Instance.Lookup<SystemDeliveryType>("system delivery type", systemDeliveryType);
            switch (systemDelType.Id)
            {
                case SystemDeliveryType.Indices.WATER:
                    createEntryType("Purchased Water", "water");
                    createEntryType("Delivered Water", "water"); // Water only for now
                    createEntryType("Transferred To", "water");
                    createEntryType("Transferred From", "water");
                    break;
                case SystemDeliveryType.Indices.WASTE_WATER:
                    createEntryType("WasteWater collected", "waste water");
                    createEntryType("WasteWater treated", "waste water"); // Water only for now
                    createEntryType("Untreated Eff. Discharged", "waste water");
                    createEntryType("Treated Eff. Discharged", "waste water");
                    createEntryType("Treated Eff. Reused", "waste water");
                    createEntryType("Biochemical Oxygen Demand", "waste water");
                    break;
            }
        }

        [Given("equipment subcategories exist")]
        public static void GivenEquipmentSubcategoriesExist()
        {
            Action<string> createCategory = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment subcategory",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            // lets just set all of them up here
            createCategory("Air Pneumatics");
            createCategory("Back Flow Preventer");
            createCategory("Building Facilities");
            createCategory("Buildings & Grounds");
            createCategory("Chemical Feed");
            createCategory("Chemical Scale");
            createCategory("Chemical Storage Tank");
            createCategory("Chlorination");
            createCategory("Comminutor");
            createCategory("Computer Systems");
            createCategory("Electrical");
            createCategory("Emergency Power");
            createCategory("Filtration");
            createCategory("Fire Protection");
            createCategory("Flocculation - Clarification");
            createCategory("HVAC");
            createCategory("Instrumentation");
            createCategory("Laboratory Equipment");
            createCategory("Lift Equipment");
            createCategory("Lifting Equipment");
            createCategory("Mixer");
            createCategory("Personal Protective Equipment");
            createCategory("Piping");
            createCategory("Plumbing");
            createCategory("Pump");
            createCategory("Residual Processing");
            createCategory("Screens");
            createCategory("Structure");
            createCategory("Tools");
            createCategory("Transportation");
            createCategory("Valve");
            createCategory("Water Storage");
            createCategory("Well");
            createCategory("Safety");
            createCategory("Dam");
            createCategory("Delivered Water");
            createCategory("Purchased Water");
            createCategory("Transferred Water");
            createCategory("WasteWater");
        }

        [Given("short cycle work order safety brief location types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefLocationTypesExist()
        {
            Action<string> createLocationTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief location type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createLocationTypes("Meter Reading");
            createLocationTypes("FSR Work (Residence)");
            createLocationTypes("FSR Work (Businesses)");
            createLocationTypes("Vault(s)");
            createLocationTypes("Booster Station(s)");
        }

        [Given("short cycle work order safety brief hazard types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefHazardTypesExist()
        {
            Action<string> createHazardTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief hazard type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createHazardTypes("Slips,Trips,Falls");
            createHazardTypes("Cuts/Abrasions");
            createHazardTypes("Ergonomic/Lifting");
            createHazardTypes("Traffic");
            createHazardTypes("Tools");
            createHazardTypes("Electrical Grounding");
            createHazardTypes("Limited Workspace");
            createHazardTypes("Weather/Lighting");
            createHazardTypes("Heat/Cold Stress");
            createHazardTypes("Poisonous Plants");
            createHazardTypes("Animals/Insects");
            createHazardTypes("Confined Space");
            createHazardTypes("Ladder Safety");
            createHazardTypes("Pandemic Precaution");
            createHazardTypes("Other");
        }

        [Given("short cycle work order safety brief tool types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefToolTypesExist()
        {
            Action<string> createToolTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief tool type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createToolTypes("Hand Tools");
            createToolTypes("Pump/Vaccum");
            createToolTypes("Multimeter");
            createToolTypes("Air Monitor");
            createToolTypes("Tripod");
            createToolTypes("Meter Reading Equipment");
            createToolTypes("Other");
        }

        [Given("short cycle work order safety brief ppe types exist")]
        public static void GivenShortCycleWorkOrderSafetyBriefPPETypesExist()
        {
            Action<string> createPPETypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("short cycle work order safety brief PPE type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createPPETypes("Hardhat");
            createPPETypes("Hearing Protection");
            createPPETypes("Class III Apparel");
            createPPETypes("Harness");
            createPPETypes("Gloves");
            createPPETypes("Safety-Toe Shoes");
            createPPETypes("00 Electrical Gloves");
            createPPETypes("Other");
        }

        [Given("equipment manufacturers exist")]
        public static void GivenEquipmentManufacturersExist()
        {
            Action<string, string> createEquipmentManufacturer = (desc, type) =>
                 MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("equipment manufacturer",
                     desc.ToLowerInvariant(),
                     $"description: \"{desc}\", equipment type: {type}, is active: true", TestObjectCache.Instance);
            createEquipmentManufacturer("generator", "generator");
            createEquipmentManufacturer("engine", "engine");
            createEquipmentManufacturer("lowry", "aerator");
            createEquipmentManufacturer("unknown", "aerator");
            createEquipmentManufacturer("other", "aerator");
            createEquipmentManufacturer("master meter", "flow meter");
        }

        [Given("compliance requirements exist")]
        public static void ComplianceRequirementsExist()
        {
            Action<string> createComplianceRequirement = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("compliance requirement",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"");

            createComplianceRequirement("Company");
            createComplianceRequirement("OSHA");
            createComplianceRequirement("PSM");
            createComplianceRequirement("Regulatory");
            createComplianceRequirement("TCPA");
        }

        [Given("equipment risk characteristics exist")]
        public static void GivenEquipmentRiskCharacteristicsExist()
        {
            Action<string, string> createThing = (thing, desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject(thing,
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("equipment condition", "Poor");
            createThing("equipment condition", "Average");
            createThing("equipment condition", "Good");
            createThing("equipment performance rating", "Good");
            createThing("equipment performance rating", "Average");
            createThing("equipment performance rating", "Poor");
            createThing("equipment static dynamic type", "Static");
            createThing("equipment static dynamic type", "Dynamic");
            createThing("equipment consequences of failure rating", "Low");
            createThing("equipment consequences of failure rating", "Medium");
            createThing("equipment consequences of failure rating", "High");
            createThing("equipment likelyhood of failure rating", "Low");
            createThing("equipment likelyhood of failure rating", "Medium");
            createThing("equipment likelyhood of failure rating", "High");
            createThing("equipment reliability rating", "Low");
            createThing("equipment reliability rating", "Medium");
            createThing("equipment reliability rating", "High");
            createThing("equipment failure risk rating", "Low");
            createThing("equipment failure risk rating", "Medium");
            createThing("equipment failure risk rating", "High");
        }

        [Given("environmental non compliance event statuses exist")]
        public static void GivenEnvironmentalNonComplianceEventStatusesExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event status", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Pending");
            createThing("Confirmed");
            createThing("Rescinded");
        }

        [Given("environmental non compliance event types exist")]
        public static void GivenEnvironmentalNonComplianceEventTypesExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event type", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Drinking Water NOV");
            createThing("Water (Non DW) NOV");
            createThing("Wastewater NOV");
            createThing("Environmental");
        }

        [Given("environmental non compliance event entity levels exist")]
        public static void GivenEnvironmentalNonComplianceEventEntityLevelsExist()
        {
            Action<string> createThing = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event entity level", desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("EPA");
            createThing("State");
            createThing("County");
            createThing("OSHA");
            createThing("Other");
        }

        [Given("environmental non compliance event responsibilities exist")]
        public static void GivenEnvironmentalNonComplianceEventResponsibilitiesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event responsibility",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("American Water");
            createThing("Third Party");
            createThing("New Acquisition");
        }

        [Given("environmental non compliance event action item types exist")]
        public static void GivenEnvironmentalNonComplianceEventActionItemTypesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event action item type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Capital Improvement");
            createThing("Contractor Outreach");
            createThing("Education");
            createThing("Not Listed");
            createThing("PM Plan Creation/Modify");
            createThing("SOP Creation/Modify");
            createThing("Tap Root Analysis");
        }

        [Given("environmental non compliance event primary root causes exist")]
        public static void GivenEnvironmentalNonComplianceEvenPrimaryRootCausesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("environmental non compliance event primary root cause",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Contractor adherence to regulations");
            createThing("Failure of equipment");
            createThing("Failure to follow SOP");
            createThing("Lack of adequate oversight");
            createThing("Lack of adequate tracking system");
            createThing("Lack of Communication");
            createThing("Lack of Education \\ Knowledge");
            createThing("Lack of SOP");
            createThing("TBD");
        }

        [Given("end of pipe exceedance types exist")]
        public static void GivenEndOfPipeExceedanceTypesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("end of pipe exceedance type",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("BOD/CBOD");
            createThing("Fecal/E Coli/Total Coliform");
            createThing("Plant Flow");
            createThing("Nitrate/Total N");
            createThing("Ammonia");
            createThing("Phosphorous");
            createThing("pH");
            createThing("TSS/TDS");
            createThing("Dissolved Oxygen");
            createThing("Other");
        }

        [Given("end of pipe exceedance root causes exist")]
        public static void GivenEndOfPipeExceedanceRootCausesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("end of pipe exceedance root cause",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Power failure");
            createThing("Mechanical failure");
            createThing("Treatment disruption");
            createThing("Treatment limitation");
            createThing("Plant capacity");
            createThing("Weather event");
            createThing("Unknown");
            createThing("Other");
        }

        [Given("task group categories exist")]
        public static void GivenTaskGroupCategoriesExist()
        {
            Action<string> createThing = desc =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("task group category",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            createThing("Chemical");
            createThing("Electrical");
            createThing("Mechanical");
            createThing("Safety");
        }

        [Given("covid answer types exist")]
        public static void GivenCovidAnswerTypesExist()
        {
            Action<string> createCovidAnswerTypes = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("covid answer type",
                    desc.ToLowerInvariant(), $"description: \"{desc}\"", TestObjectCache.Instance);
            createCovidAnswerTypes("Yes");
            createCovidAnswerTypes("No");
            createCovidAnswerTypes("TBD");
            createCovidAnswerTypes("Contact Tracer Must Complete");
        }

        #endregion

        #region Object Creation

        public static NearMissType CreateNearMissType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "safety":
                    return container.GetInstance<SafetyNearMissTypeFactory>().Create();
                case "environmental":
                    return container.GetInstance<EnvironmentalNearMissTypeFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create near miss type from description string '{nvc["description"]}'");
            }
        }

        public static NearMissCategory CreateNearMissCategory(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch ((nvc["description"] ?? string.Empty).ToLowerInvariant())
            {
                case "other":
                    return container.GetInstance<OtherNearMissCategoryFactory>().Create();
                case "stormwater":
                    return container.GetInstance<StormWaterNearMissCategoryFactory>().Create();
                case "ergonomics":
                    return container.GetInstance<ErgonomicsNearMissCategoryFactory>().Create();
                default:
                    throw new InvalidOperationException($"Not sure how to create near miss category from description string '{nvc["description"]}'");
            }
        }

        public static CommunicationType CreateCommunicationType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "letter":
                    return container.GetInstance<LetterCommunicationTypeFactory>().Create();
                case "electronic":
                    return container.GetInstance<ElectronicCommunicationTypeFactory>().Create();
                case "upload":
                    return container.GetInstance<UploadCommunicationTypeFactory>().Create();
                case "email":
                    return container.GetInstance<EmailCommunicationTypeFactory>().Create();
                case "pdf":
                    return container.GetInstance<PdfCommunicationTypeFactory>().Create();
                case "agency submittal form":
                    return container.GetInstance<AgencySubmittalFormCommunicationTypeFactory>().Create();
                case "other":
                    return container.GetInstance<OtherCommunicationTypeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create communication type with description '{nvc["description"]}'");
            }
        }

        public static Contact CreateContactWithoutAddress(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ContactWithoutAddressFactory>().Create(new
            {
                FirstName = nvc["firstname"],
                LastName = nvc["lastname"] ?? "",
                Email = nvc["email"],
                BusinessPhoneNumber = nvc["businessphone"],
                HomePhoneNumber = nvc["homephone"],
                MobilePhoneNumber = nvc["mobile"],
                FaxNumber = nvc["fax"]
            });
        }

        public static CustomerCoordinate CreateCustomerCoordinate(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // need to build and save separately, rather than create, to prevent extra customer locations from being created
            var coordinate = container.GetInstance<CustomerCoordinateFactory>().Build(new
            {
                Latitude = nvc.GetValueAs<float>("latitude"),
                Longitude = nvc.GetValueAs<float>("longitude"),
                CustomerLocation = objectCache.Lookup("customer location", nvc["customer location"]),
                Source = Enum.Parse(typeof(CoordinateSource), nvc["source"])
            });

            container.GetInstance<ISession>().Save(coordinate);

            return coordinate;
        }

        public static DataTableLayout CreateDataTableLayout(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<DataTableLayoutFactory>();

            var model = factory.Create(new
            {
                Area = nvc["area"],
                Controller = nvc["controller"],
                LayoutName = nvc["layout name"]
            });

            var propertyNames = nvc["properties"];
            if (string.IsNullOrWhiteSpace(propertyNames))
            {
                throw new NotSupportedException("You need to supply comma separated property names.");
            }

            var propFactory = container.GetInstance<TestDataFactory<DataTableLayoutProperty>>();
            foreach (var propName in propertyNames.Split(','))
            {
                var dtlProp = propFactory.Create(new {
                    DataTableLayout = model,
                    PropertyName = propName
                });
                model.Properties.Add(dtlProp);
            }

            return model;
        }

        public static Division CreateDivision(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<DivisionFactory>();

            var model = factory.Create(new {
                Description = nvc["description"]
            });

            return model;
        }

        // things fail without this one, not sure why
        public static object CreateDocument(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var fileName = nvc["file name"];
            var documentType = obj.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);

            return container.GetInstance<DocumentFactory>().Create(new
            {
                FileName = fileName,
                DocumentType = documentType
            });
        }

        public static DocumentType CreateDocumentType(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            var dataType = obj.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            if (nvc["name"] == "Near Miss Document")
            {
                return container.GetInstance<DocumentTypeFactory>().EnsureSpecificThing("DocumentType", DocumentType.Indices.NEAR_MISS_DOCUMENT,
                    new Dictionary<string,object> {
                        {"DocumentTypeID", DocumentType.Indices.NEAR_MISS_DOCUMENT},
                        {"Document_Type", NEAR_MISS_DOCUMENT},
                        { "DataTypeId", ((IEntity)dataType).Id }
                    });
            }

            return container.GetInstance<DocumentTypeFactory>().Create(new { DataType = dataType, Name = nvc["name"] });
        }

        public static EmployeeStatus CreateEmployeeStatus(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"].ToLower())
            {
                case "active":
                    return container.GetInstance<ActiveEmployeeStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveEmployeeStatusFactory>().Create();
                default:
                    throw new ArgumentException($"Not sure how to create employee status with description {nvc["description"]}.");
            }
        }
        
        public static FacilityStatus CreateFacilityStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActiveFacilityStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactiveFacilityStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a facility status with description '{nvc["description"]}'");
            }
        }

        public static NotificationPurpose CreateNotificationPurpose(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var moduleEnum = nvc.GetValueAs<RoleModules>("module");
            var appEnum = EnumExtensions.GetValues<RoleApplications>().Single(x => moduleEnum.ToString().StartsWith(x.ToString()));
            var app = container.GetInstance<ApplicationFactory>().Create(new { Id = appEnum });
            var module = container.GetInstance<ModuleFactory>().Create(new { Id = moduleEnum, Application = app });

            return container.GetInstance<NotificationPurposeFactory>().Create(new {
                Purpose = nvc["purpose"],
                //Module = (Module)objectCache.GetValueOrDefault<ModuleFactory>("module", nvc)
                //Module = objectCache.Lookup<Module>("module", nvc["module"])
                Module = module
            });
        }

        public static OperatingCenter CreateOperatingCenter(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // NOTE: If you're here because you're setting values for an operating center and they're not persisting
            //       for the regression test it's due to an operating center already existing with the same opcode. The
            //       OperatingCenterFactory will return the existing record that matches the opcode rather than create a
            //       new one. To fix this in your test you need to change the opcode to something else. -Ross 1/10/2018

            const string recFreqType = "recurring frequency unit";

            // TestObjectCachce.GetValueOrDefault chokes and dies if you try to use it when no type is created, rather than
            // returning the default(since there aren't any matching values). So this is what gets to be done:
            object largeValveUnit, smallValveUnit;
            object hydrantUnit = largeValveUnit = smallValveUnit = typeof(YearlyRecurringFrequencyUnitFactory);

            if (objectCache.ContainsKey(recFreqType))
            {
                hydrantUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "hydrantInspFreqUnit", nvc);
                largeValveUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "largeValveInspFreqUnit", nvc);
                smallValveUnit = objectCache.GetValueOrDefault<YearlyRecurringFrequencyUnitFactory>(recFreqType, "smallValveInspFreqUnit", nvc);
            }
            int? gallons = null;
            if (!String.IsNullOrWhiteSpace(nvc["maximum overflow gallons"]))
                gallons = Int32.Parse(nvc["maximum overflow gallons"]);

            var oc = container.GetInstance<OperatingCenterFactory>().Create(new {
                OperatingCenterCode = nvc["opcode"],
                OperatingCenterName = nvc["name"],
                CompanyInfo = nvc["companyInfo"],
                PhoneNumber = nvc["phone"],
                FaxNumber = nvc["fax"],
                MailingAddressName = nvc["mailingName"],
                MailingAddressStreet = nvc["mailingStreet"],
                MailingAddressCityStateZip = nvc["mailingCSZ"],
                ServiceContactPhoneNumber = nvc["servicePhone"],
                HydrantInspectionFrequency = nvc.GetValueAs<int>("hydrantInspFreq"),
                HydrantInspectionFrequencyUnit = hydrantUnit,
                LargeValveInspectionFrequency = nvc.GetValueAs<int>("largeValveInspFreq"),
                LargeValveInspectionFrequencyUnit = largeValveUnit,
                SmallValveInspectionFrequency = nvc.GetValueAs<int>("smallValveInspFreq"),
                SmallValveInspectionFrequencyUnit = smallValveUnit,
                State = objectCache.GetValueOrDefault<StateFactory>("state", nvc),
                PermitsOMUserName = nvc["permitsOmUserName"],
                PermitsCapitalUserName = nvc["permitsCapitalUserName"],
                WorkOrdersEnabled = nvc.GetValueAs<bool>("workOrdersEnabled").GetValueOrDefault(true),
                DefaultServiceReplacementWBSNumber =
                    objectCache.GetValueOrDefault<WBSNumberFactory>("w b s number", nvc),
                IsContractedOperations = nvc.GetValueAs<bool>("is contracted operations"),
                SAPEnabled = nvc.GetValueAs<bool>("sap enabled"),
                SAPWorkOrdersEnabled = nvc.GetValueAs<bool>("sap work orders enabled"),
                UsesValveInspectionFrequency = nvc.GetValueAs<bool>("uses valve inspection frequency"),
                ZoneStartYear = nvc.GetValueAs<int>("zone start year"),
                IsActive = nvc.GetValueAs<bool>("is active") ?? true,
                MapId = nvc["mapId"]
            });

            Debug.Print("argh: {0}", objectCache.GetValueOrDefault<WBSNumberFactory>("w b s number", nvc));
            if (gallons != null)
            {
                oc.MaximumOverflowGallons = gallons.Value;
            }
            return oc;
        }

        private static TEntity CreateEntityLookup<TEntity, TFactory>(TestObjectCache cache, ISession sesh, string dictionaryName, string description, string name = null)
            where TEntity : class, new()
            where TFactory : TestDataFactory<TEntity>
        {
            object obj;
            name = name ?? description.SanitizeAndDowncase();
            var agencyDictionary = cache.EnsureDictionary(dictionaryName);
            if (!agencyDictionary.TryGetValue(name, out obj))
            {
                var factory = (TFactory)DependencyResolver.Current.GetService(typeof(TFactory));
                obj = factory.Create(new { Description = description });
                agencyDictionary[name] = obj;
            }
            return (TEntity)obj;
        }

        public static Project CreateProject(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ProjectFactory>().Create(new {
                Name = nvc["name"]
            });
        }

        public static PublicWaterSupply CreatePublicWaterSupply(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<PublicWaterSupplyFactory>();

            var model = factory.Create(new {
                Identifier = nvc["identifier"],
                OperatingArea = nvc["operating area"],
                Status = objectCache.GetValueOrDefault<PublicWaterSupplyStatusFactory>("public water supply status", "status", nvc),
                JanuaryRequiredBacterialWaterSamples = nvc.GetValueAs<int>("january required bacterial water samples"),
                MarchRequiredBacterialWaterSamples = nvc.GetValueAs<int>("march required bacterial water samples"),
                DecemberRequiredBacterialWaterSamples = nvc.GetValueAs<int>("december required bacterial water samples"),
                State = objectCache.GetValueOrDefault<StateFactory>("state", nvc),
                AWOwned = nvc.GetValueAs<bool>("aw owned"),
                Ownership = objectCache.GetValueOrDefault<PublicWaterSupplyOwnershipFactory>("public water supply ownership", "ownership", nvc),
                Type = objectCache.GetValueOrDefault<PublicWaterSupplyTypeFactory>("public water supply type", "type", nvc),
            });

            return model;
        }

        [Given("public water supply statuses exist")]
        public static void GivenPublicWaterSupplyStatusesExist()
        {
            Action<string> create = (desc) =>
                MMSINC.Testing.SpecFlow.StepDefinitions.Data.CreateObject("public water supply status",
                    desc.ToLowerInvariant(),
                    $"description: \"{desc}\"", TestObjectCache.Instance);

            create("Active");
            create("Pending");
            create("Pending Merger");
            create("Inactive");
            create("Inactive -see note");
        }

        public static PublicWaterSupplyStatus CreatePublicWaterSupplyStatus(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "active":
                    return container.GetInstance<ActivePublicWaterSupplyStatusFactory>().Create();
                case "pending":
                    return container.GetInstance<PendingPublicWaterSupplyStatusFactory>().Create();
                case "pending merger":
                    return container.GetInstance<PendingMergerPublicWaterSupplyStatusFactory>().Create();
                case "inactive":
                    return container.GetInstance<InactivePublicWaterSupplyStatusFactory>().Create();
                case "inactive -see note":
                    return container.GetInstance<InactiveSeeNotePublicWaterSupplyStatusFactory>().Create();
                default:
                    throw new InvalidOperationException($"Unable to create a public water supply status with description '{nvc["description"]}'");
            }
        }

        public static Role CreateRole(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var actionEnum = nvc.GetValueAs<RoleActions>("action");
            var moduleEnum = nvc.GetValueAs<RoleModules>("module");
            var appEnum = EnumExtensions.GetValues<RoleApplications>().Single(x => moduleEnum.ToString().StartsWith(x.ToString()));
            var app = container.GetInstance<ApplicationFactory>().Create(new { Id = appEnum });
            var module = container.GetInstance<ModuleFactory>().Create(new { Id = moduleEnum, Application = app });
            var action = container.GetInstance<ActionFactory>().Create(new { Id = actionEnum });
            var opCenter = objectCache.GetOrNull("operating center", nvc);

            return container.GetInstance<RoleFactory>().Create(new
            {
                Application = app,
                Module = module,
                Action = action,
                OperatingCenter = opCenter,
                User = objectCache.GetValueOrDefault<UserFactory>("user", nvc)
            });
        }

        public static MapCall.Common.Model.Entities.State CreateState(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var factory = container.GetInstance<StateFactory>();

            if (nvc.ContainsKey("abbreviation"))
            {
                return factory.Create(new {
                    Name = nvc["name"],
                    Abbreviation = nvc["abbreviation"],
                    ScadaTable = nvc["scada table"]
                });
            }

            return factory.Create(new {
                Name = nvc["name"],
                ScadaTable = nvc["scada table"]
            });
        }

        public static Town CreateTown(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            if (nvc.ContainsKey("name"))
            {
                return container.GetInstance<TownFactory>().Create(new {
                    ShortName = nvc["name"],
                    County = objectCache.GetValueOrDefault<CountyFactory>("county", nvc)
                });
            }

            return container.GetInstance<TownFactory>().Create(new {
                County = objectCache.GetValueOrDefault<CountyFactory>("county", nvc)
            });
        }

        public static User CreateUser(NameValueCollection nvc, TestObjectCache objectCache, IContainer container, bool isAdmin)
        {
            var factory = isAdmin ? container.GetInstance<AdminUserFactory>() : container.GetInstance<UserFactory>();
            var roleFactory = container.GetInstance<RoleFactory>();
            var email = nvc.ContainsKey("special_email")
                ? nvc["special_email"].PrependCurrentHostname()
                : nvc["email"];

            var roles =
                (nvc["roles"] ?? String.Empty).Split(";".ToCharArray())
                    .Where(roleName => !String.IsNullOrWhiteSpace(roleName))
                    .Select(roleName => objectCache.Lookup<Role>("role", roleName))
                    .Select(r => roleFactory.Create(new
                    {
                        Application = r.Module.Application,
                        r.Module,
                        r.Action,
                        r.OperatingCenter
                    })).ToList();

            var employee = (Employee)objectCache.GetOrNull("employee", nvc);

            var user = factory.Create(new
            {
                Email = email,
                //EmployeeId = nvc["employee id"],
                Employee = employee,
                UserName = nvc["username"] ?? "some user",
                Roles = roles,
                FullName = nvc["full name"],
                DefaultOperatingCenter = objectCache.GetValueOrDefault<OperatingCenterFactory>("operating center", "default operating center", nvc),
                //CustomerProfileId = nvc.GetValueAs<int>("customer profile id"),
                ProfileLastVerified =
                    nvc["profile last verified"] != null &&
                        nvc["profile last verified"] == "today"
                        ? DateTime.Now
                        : (DateTime?)null,
                CustomerProfileId =
                    nvc.ContainsKey("has profileId") &&
                        nvc["has profileId"] == "true"
                        ? Convert.ToInt32(
                            DependencyResolver.Current.GetService<IExtendedCustomerGateway>()
                                .CreateCustomer(email, "regression test user")
                                .ProfileID)
                        : (int?)null
            });

            //if (employee != null)
            //{
            //    // In order for employees-filtered-by-user-role dropdowns to work, both the User and Employee records
            //    // must be linked together. For some reason, when trying to set employee.User from inside regression tests,
            //    // the Employee instance we receive from the objectCache is not the nhibernate session cached instance.
            //    // Because of that, we have to re-query for the employee, set its User, and then save that.
            //    //
            //    // If you try to save the Employee record from the objectCache, nhibernate will attempt to insert a new
            //    // instance and you'll get a unique constraint error related to the employee number.
            //    //
            //    // If you don't session.Save the Employee, the Employee record in the database will always have a null UserId.
            //    //
            //    // I think this may have to do with MagicalBuilderThingy creating the Employee but I'm really not sure.
            //    employee = session.QueryOver<Employee>().Where(x => x.Id == employee.Id).SingleOrDefault();
            //    employee.User = user;
            //    session.Save(employee);
            //}

            if (nvc.ContainsKey("credit card number"))
            {
                CreatePaymentProfile(user, nvc["credit card number"]);
            }

            return user;
        }

        #region Helper Methods

        private static void CreatePaymentProfile(User user, string cardNumber)
        {
            var expiration = DateTime.Now.AddMonths(1);
            DependencyResolver.Current.GetService<IExtendedCustomerGateway>()
                              .AddCreditCard(user.CustomerProfileId.ToString(), cardNumber, expiration.Month, expiration.Year,
                                   "123");
        }

        #endregion
        
        #endregion
    }

    /// <summary>
    /// Having this function here, lets the regression runner go and load up the other functions we
    /// actually want loaded up for the regressions, that we've defined in
    /// MMSINC.Core\Data\NHibernate\DatabaseConfiguration.cs
    /// If it's removed some regressions will fail due to the missing sqlite functions.
    /// </summary>
    [SQLiteFunction(Name = "wellywellywell", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class WellWellWellFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0];
        }
    }
}
