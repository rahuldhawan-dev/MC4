using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Contractors.Tests;
using DeleporterCore.Client;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.SpecFlow.Library;
using NHibernate;
using NHibernate.Linq;
using StructureMap;
using TechTalk.SpecFlow;
using AdminUserFactory = Contractors.Tests.AdminUserFactory;
using ContractorFactory = Contractors.Tests.ContractorFactory;
using ContractorUserFactory = Contractors.Tests.ContractorUserFactory;
using CountyFactory = Contractors.Tests.CountyFactory;
using CrewFactory = Contractors.Tests.CrewFactory;
using CurbBoxRepairWorkDescriptionFactory = Contractors.Tests.CurbBoxRepairWorkDescriptionFactory;
using DocumentFactory = Contractors.Tests.DocumentFactory;
using DocumentTypeFactory = Contractors.Tests.DocumentTypeFactory;
using EmergencyWorkOrderPriorityFactory = Contractors.Tests.EmergencyWorkOrderPriorityFactory;
using HighPriorityWorkOrderPriorityFactory = Contractors.Tests.HighPriorityWorkOrderPriorityFactory;
using HydrantAssetTypeFactory = Contractors.Tests.HydrantAssetTypeFactory;
using HydrantFactory = Contractors.Tests.HydrantFactory;
using MainAssetTypeFactory = Contractors.Tests.MainAssetTypeFactory;
using MainBreakFactory = Contractors.Tests.MainBreakFactory;
using MarkoutFactory = Contractors.Tests.MarkoutFactory;
using MarkoutTypeFactory = Contractors.Tests.MarkoutTypeFactory;
using MaterialFactory = Contractors.Tests.MaterialFactory;
using RequisitionFactory = Contractors.Tests.RequisitionFactory;
using RestorationFactory = Contractors.Tests.RestorationFactory;
using RestorationTypeFactory = Contractors.Tests.RestorationTypeFactory;
using RoutineWorkOrderPriorityFactory = Contractors.Tests.RoutineWorkOrderPriorityFactory;
using ServiceAssetTypeFactory = Contractors.Tests.ServiceAssetTypeFactory;
using ServiceLineRenewalWorkDescriptionFactory = Contractors.Tests.ServiceLineRenewalWorkDescriptionFactory;
using ServiceLineRetireWorkDescriptionFactory = Contractors.Tests.ServiceLineRetireWorkDescriptionFactory;
using ServiceMaterialFactory = Contractors.Tests.ServiceMaterialFactory;
using ServiceSizeFactory = Contractors.Tests.ServiceSizeFactory;
using SpoilStorageLocationFactory = Contractors.Tests.SpoilStorageLocationFactory;
using StockLocationFactory = Contractors.Tests.StockLocationFactory;
using StreetOpeningPermitFactory = Contractors.Tests.StreetOpeningPermitFactory;
using TapImageFactory = Contractors.Tests.TapImageFactory;
using TownFactory = Contractors.Tests.TownFactory;
using TownSectionFactory = Contractors.Tests.TownSectionFactory;
using ValveAssetTypeFactory = Contractors.Tests.ValveAssetTypeFactory;
using ValveBoxRepairWorkDescriptionFactory = Contractors.Tests.ValveBoxRepairWorkDescriptionFactory;
using ValveFactory = Contractors.Tests.ValveFactory;
using ValveImageFactory = Contractors.Tests.ValveImageFactory;
using WorkOrderPriorityFactory = Contractors.Tests.WorkOrderPriorityFactory;

//using WorkOrderRequesterFactory = Contractors.Tests.WorkOrderRequesterFactory;

namespace RegressionTests.Steps
{
    [Binding]
    public static class Data
    {
        #region Constants

        public static readonly TestTypeDictionary TYPE_DICTIONARY = new TestTypeDictionary {
            {"user", typeof(ContractorUser), (v, c, s) => CreateUser(v, c, s)},
            {"admin user", typeof(ContractorUser), (v, c, s) => CreateUser(v, c, s, true)},
            {"asset type", typeof(AssetType), (v, c, s) => CreateAssetType(v, c, s)},
            {"contractor", typeof(Contractor), (v, c, s) => CreateContractor(v, c, s)},
            {"crew", typeof(Crew), (v, c, s) => CreateCrew(v, c, s)}, 
            {"crew assignment", typeof(CrewAssignment), (v, c, s) => CreateCrewAssignment(v, c, s)},
            {"closed crew assignment", typeof(CrewAssignment), (v, c, s) => CreateClosedCrewAssignment(v, c, s)},
            {"document", typeof(Document), (v, c, s) => CreateDocument(v, c, s)},
            {"main break", typeof(MainBreak), (v, c, s) => CreateMainBreak(v, c, s)},
            {"main break material", typeof(MainBreakMaterial), (v, c, s) => CreateMainBreakMaterial(v, c, s)},
            {"main condition", typeof(MainCondition), (v, c, s) => CreateMainCondition(v, c, s)},
            {"main failure type", typeof(MainFailureType), (v, c, s) => CreateMainFailureType(v, c, s)},
            {"main soil condition", typeof(MainBreakSoilCondition), (v, c, s) => CreateMainBreakSoilCondition(v, c, s)},
            {"main disinfection method", typeof(MainBreakDisinfectionMethod), (v, c, s) => CreateMainBreakDisinfectionMethod(v, c, s)},
            {"main flush method", typeof(MainBreakFlushMethod), (v, c, s) => CreateMainBreakFlushMethod(v, c, s)},
            {"material", typeof(Material), (v, c, s) => CreateMaterial(v, c, s)},
            {"material used", typeof(MaterialUsed), (v, c, s) => CreateMaterialUsed(v, c, s)},
            {"meter set", typeof(ServiceInstallation), (v, c, s) => CreateMeterSet(v, c, s)},
            {"service size", typeof(ServiceSize), (v, c, s) => CreateServiceSize(v, c, s)},
            {"service material", typeof(ServiceMaterial), (v,c,s)=>CreateServiceMaterial(v,c,s) },
            {"markout", typeof(Markout), (v, c, s) => CreateMarkout(v, c, s)},
            {"markout type", typeof(MarkoutType), (v, c, s) => CreateMarkoutType(v, c, s)},
            {"operating center", typeof(OperatingCenter), (v, c, s) => CreateOperatingCenter(v, c, s)},
            {"requisition", typeof(Requisition), CreateRequisition},
            {"requested by", typeof(WorkOrderRequester), (v, c, s) => CreateRequestedBy(v, c, s)},
            {"restoration", typeof(Restoration), (v, c, s) => CreateRestoration(v, c, s)},
            {"restoration type", typeof(RestorationType), (v, c, s) => CreateRestorationType(v, c, s)},
            {"restoration method", typeof(RestorationMethod), (v, c, s) => CreateRestorationMethod(v, c, s)},
            {"spoil", typeof(Spoil), (v, c, s) => CreateSpoil(v, c, s)},
            {"spoil storage location", typeof(SpoilStorageLocation), (v, c, s) => CreateSpoilStorageLocation(v, c, s)},
            {"stock location", typeof(StockLocation), (v, c, s) => CreateStockLocation(v, c, s)},
            {"street", typeof(Street), (v, c, s) => CreateStreet(v, c, s)},
            {"street opening permit", typeof(StreetOpeningPermit), (v, c, s) => CreateStreetOpeningPermit(v, c, s)},
            {"tap image", typeof(TapImage), (v, c, s) => CreateTapImage(v, c, s)},
            {"town", typeof(Town), (v, c, s) => CreateTown(v, c, s)}, 
            {"town section", typeof(TownSection), (v, c, s) => CreateTownSection(v, c, s)}, 
            {"valve image", typeof(ValveImage), (v, c, s) => CreateValveImage(v, c, s)},
            {"valve work description", typeof(WorkDescription), (v, c, s) => CreateValveWorkDescription(v, c, s)},
            {"hydrant work description", typeof(WorkDescription), (v, c, s) => CreateHydrantWorkDescription(v, c, s)},
            {"scheduling work order with valve", typeof(WorkOrder), (v, c, s) => CreateWorkOrderWithValve<SchedulingWorkOrderFactory>(v, c, s)},
            {"scheduling work order with hydrant", typeof(WorkOrder), (v, c, s) => CreateWorkOrderWithHydrant<SchedulingWorkOrderFactory>(v, c, s)},
            {"planning work order", typeof(WorkOrder), (v, c, s) => CreateWorkOrder<PlanningWorkOrderFactory>(v, c, s)},
            {"planning work order with valve", typeof(WorkOrder), (v, c, s) => CreateWorkOrderWithValve<PlanningWorkOrderFactory>(v, c, s)},
            {"planning work order with main", typeof(WorkOrder), (v, c, s) => CreateWorkOrderWithMayne<PlanningWorkOrderFactory>(v, c, s)},
            {"planning work order with service", typeof(WorkOrder), (v, c, s) => CreateWorkOrderWithService<PlanningWorkOrderFactory>(v, c, s)},
            {"finalization work order", typeof(WorkOrder), (v, c, s) => CreateWorkOrder<FinalizationWorkOrderFactory>(v, c, s)},
            {"finalization work order for a main break", typeof(WorkOrder), (v, c, s) => CreateWorkOrderForMainBreak<FinalizationWorkOrderFactory>(v, c, s)},
            {"finalization work order for a service line renewal", typeof(WorkOrder), (v, c, s) => CreateWorkOrderForServiceLineRenewal<FinalizationWorkOrderFactory>(v, c, s)},
            {"finalization work order for a service line retire", typeof(WorkOrder), (v, c, s) => CreateWorkOrderForServiceLineRetire<FinalizationWorkOrderFactory>(v, c, s)},
            {"general work order", typeof(WorkOrder), (v, c, s) => CreateWorkOrder<WorkOrderFactory>(v, c, s)},
            {"finalization work order for a service line renewal company side", typeof(WorkOrder), (v, c, s) => CreateWorkOrderForServiceLineRenewalCompanySide<FinalizationWorkOrderFactory>(v, c, s)},
            {"work order", typeof(WorkOrder), (v, c, s) => CreateWorkOrder<WorkOrderFactory>(v, c, s)},
            {"work order priority", typeof(WorkOrderPriority), (v, c, s) => CreateWorkOrderPriority<WorkOrderPriorityFactory>(v, c, s)},
            {"work order purpose", typeof(WorkOrderPurpose), CreateWorkOrderPurpose},
            {"work description", typeof(WorkDescription), CreateWorkDescription},
            {"read only work order", typeof(WorkOrder), (v, c, s) => CreateWorkOrder<WorkOrderFactory>(v, c, s)},
            {"response priority", typeof(RestorationResponsePriority), (v, c, s) => CreateRestorationResponsePriority(v, c, s)},
            {"work order document link", typeof(DocumentLink), CreateWorkOrderDocumentLink},
            {"planning work order document link", typeof(DocumentLink), CreatePlanningWorkOrderDocumentLink},
            {"finalization work order document link", typeof(DocumentLink), CreateFinalizationWorkOrderDocumentLink},
            {"pitcher filter delivery method", typeof(PitcherFilterCustomerDeliveryMethod), (v,c,s)=> CreatePitcherFilterDeliveryMethod(v,c,s) },
            {"meter location", typeof(MeterLocation), CreateMeterLocation},
        };

        #endregion

        #region Event-Driven Functionality

        public static void Initialize()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.SetTypeDictionary(
               TYPE_DICTIONARY);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data
              .SetFactoryAssembly(typeof(ContractorUserFactory).Assembly);
            MMSINC.Testing.SpecFlow.StepDefinitions.Data
              .SetModelAssembly(typeof(ContractorUser).Assembly);
        }

        [BeforeScenario("@no_data_reload")]
        public static void PreNoDataReload()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.NoDataReload = true;
        }

        [AfterScenario("@no_data_reload")]
        public static void PostNoDataReload()
        {
            MMSINC.Testing.SpecFlow.StepDefinitions.Data.NoDataReload = false;
        }

        #endregion

        #region Helper Methods

        public static void ProcessMarkoutDates(WorkOrder workOrder, NameValueCollection nvc, out DateTime? dateOfRequest, out DateTime? readyDate, out DateTime? expirationDate)
        {
            var requirement = workOrder.MarkoutRequirement.MarkoutRequirementEnum;

            dateOfRequest = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["date of request"]);
            readyDate = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["ready date"]);
            expirationDate = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["expiration date"]);

            if (dateOfRequest == null && readyDate != null)
            {
                dateOfRequest = WorkOrdersWorkDayEngine.GetCallDate(readyDate.Value, requirement);
            }

            if (dateOfRequest != null && expirationDate == null)
            {
                expirationDate = WorkOrdersWorkDayEngine.GetExpirationDate(dateOfRequest.Value, requirement);
            }

            if (dateOfRequest != null && readyDate == null)
            {
                readyDate = WorkOrdersWorkDayEngine.GetReadyDate(dateOfRequest.Value, requirement);
            }
        }

        #endregion

        #region Object Creation

        public static AssetType CreateAssetType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            AssetType assetType;
            switch (nvc["description"])
            {
                case "valve":
                    assetType = container.GetInstance<ValveAssetTypeFactory>().Create();
                    break;
                case "service":
                    assetType = container.GetInstance<ServiceAssetTypeFactory>().Create();
                    break;
                default:
                    assetType = container.GetInstance<HydrantAssetTypeFactory>().Create();
                    break;
            }

            if (nvc["operating center"] != null)
            {
                var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"]);
                if (operatingCenter != null)
                {
                    var operatingCenterAssetType = new OperatingCenterAssetType {
                        AssetType = assetType,
                        OperatingCenter = operatingCenter
                    };
                    assetType.OperatingCenterAssetTypes.Add(operatingCenterAssetType);
                        
                    container.GetInstance<ISession>().Save(operatingCenterAssetType);
                    container.GetInstance<ISession>().Merge(assetType);
                }
            }
            return assetType;
        }

        public static Contractor CreateContractor(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var contractor = container.GetInstance<ContractorFactory>().Create();
            if (nvc["contractors access"] != null)

                contractor.ContractorsAccess =
                    nvc.GetValueAs<bool>("contractors access").GetValueOrDefault();

            if (nvc["operating center"] != null)
            {
                var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"]);
                if (operatingCenter != null)
                {
                    contractor.OperatingCenters.Add(operatingCenter);
                    container.GetInstance<ISession>().Merge(contractor);
                }
            }
            return contractor;
        }

        public static ContractorUser CreateUser(NameValueCollection nvc, TestObjectCache objectCache, IContainer container, bool isAdmin = false)
        {
            var factory = isAdmin ? container.GetInstance<AdminUserFactory>() : container.GetInstance<ContractorUserFactory>();
            if (!String.IsNullOrEmpty(nvc["contractor"]))
            {
                return factory.Create(new {
                    Email = nvc["email"],
                    Password = nvc["password"],
                    Contractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                    IsAdmin = isAdmin
                });
            }

            return factory.Create(new {
                Email = nvc["email"],
                Password = nvc["password"],
                IsAdmin = isAdmin
            });
        }

        public static Crew CreateCrew(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var contractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]);
            return container.GetInstance<CrewFactory>().Create(new {
                Availability = nvc.GetValueAs<decimal>("availabilit" +
                    "y").GetValueOrDefault(),
                Contractor = contractor,
                Description = nvc["description"]
            });
        }

        public static CrewAssignment CreateCrewAssignment(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            var workOrder = (!string.IsNullOrEmpty(workOrderKey) ? objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey]) : null);
            return container.GetInstance<CrewAssignmentFactory>().Create(new {
                Crew = objectCache.Lookup<Crew>("crew", nvc["crew"]),
                WorkOrder = workOrder,
                AssignedFor = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["assigned for"]),
                AssignedOn = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["assigned on"]),
                DateEnded = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["date ended"]),
                DateStarted = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["date started"]),
                EmployeesOnJob = nvc.GetValueAs<Single>("employees on job"),
                Priority = nvc.GetValueAs<int>("priority").GetValueOrDefault(1)
            }.CloneWithoutNulls());
        }

        public static CrewAssignment CreateClosedCrewAssignment(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            return container.GetInstance<ClosedCrewAssignmentFactory>().Create(new {
                Crew = objectCache.Lookup<Crew>("crew", nvc["crew"]),
                WorkOrder =
                    objectCache.Lookup<WorkOrder>(workOrderKey,
                        nvc[workOrderKey]),
                AssignedFor = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["assigned for"]),
                AssignedOn = MMSINC.Testing.SpecFlow.StepDefinitions.Data.GetDateTime(nvc["assigned on"])
            });
        }

        public static Document CreateDocument(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            var workOrder = string.IsNullOrWhiteSpace(workOrderKey)
                ? null
                : objectCache.Lookup<WorkOrder>(workOrderKey,
                    nvc[workOrderKey]);

            var document = container.GetInstance<DocumentFactory>().Create(new {
                    FileName = "Foo",
                    DocumentType = objectCache.Lookup<DocumentType>("document type", nvc["document type"])
                });

            if (workOrder != null)
            {
                document.WorkOrders.Add(container.GetInstance<ISession>().Load<WorkOrder>(workOrder.Id));
            }

            container.GetInstance<ISession>().Flush();

            return document;
        }

        public static Markout CreateMarkout(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            DateTime? dateOfRequest, readyDate, expirationDate;

            // this will be a problem if there's no work order specified
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault() ??
                               nvc.FindKeys("general work order").FirstOrDefault();
            var workOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey]);

            ProcessMarkoutDates(workOrder, nvc, out dateOfRequest, out readyDate, out expirationDate);

            return container.GetInstance<MarkoutFactory>().Create(new {
                DateOfRequest = dateOfRequest,
                ExpirationDate = expirationDate,
                MarkoutNumber = nvc["markout number"],
                ReadyDate = readyDate,
                WorkOrder = workOrder
            }.CloneWithoutNulls());
        }

        public static MainBreak CreateMainBreak(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            var workOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey]);

            return container.GetInstance<MainBreakFactory>().Create(new {
                WorkOrder = workOrder
            });
        }

        public static ServiceInstallation CreateMeterSet(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrder = objectCache.Lookup<WorkOrder>("finalization work order", nvc["finalization work order"]);

            return container.GetInstance<Contractors.Tests.ServiceInstallationFactory>().Create(new {
                WorkOrder = workOrder
            });
        }

        public static MainBreakMaterial CreateMainBreakMaterial(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainBreakMaterialFactory>().Create(new {
                Description = nvc["Description"]
            });
        }

        public static MainCondition CreateMainCondition(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainConditionFactory>().Create(new {
                Description = nvc["Description"]
            });
        }

        public static MainFailureType CreateMainFailureType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainFailureTypeFactory>().Create(new
                                                    {
                                                        Description = nvc["Description"]
                                                    });
        }

        public static MainBreakSoilCondition CreateMainBreakSoilCondition(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainBreakSoilConditionFactory>().Create(new
                                                    {
                                                        Description = nvc["Description"]
                                                    });
        }

        public static MainBreakDisinfectionMethod CreateMainBreakDisinfectionMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainBreakDisinfectionMethodFactory>().Create(new
                                                    {
                                                        Description = nvc["Description"]
                                                    });
        }

        public static MainBreakFlushMethod CreateMainBreakFlushMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MainBreakFlushMethodFactory>().Create(new
                                                    {
                                                        Description = nvc["Description"]
                                                    });
        }

        public static ServiceSize CreateServiceSize(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ServiceSizeFactory>().Create(new {
                ServiceSizeDescription = nvc["service size description"]
            });
        }
        
        public static ServiceMaterial CreateServiceMaterial(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<ServiceMaterialFactory>().Create(new {
                Description = nvc["description"]
            });
        }

        public static MarkoutType CreateMarkoutType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<MarkoutTypeFactory>().Create(new {
                Description = nvc["description"]
            });
        }

        public static Material CreateMaterial(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"]);
            // For some reason, we need to re-query for the OperatingCenter instance because the one from the Lookup
            // isn't the same one associated with the Session. Using Session.Merge with the lookup instance causes existing
            // StockedMaterials to be duplicated. Using Session.Save on the lookup instance causes an error because
            // OperatingCenterCode needs to be unique and NHibernate thinks this is a new record instead.
            operatingCenter = container.GetInstance<ISession>().Query<OperatingCenter>().Single(x => x.Id == operatingCenter.Id);
            var material = container.GetInstance<MaterialFactory>().Create(new {
                PartNumber = nvc["part number"],
                Description = nvc["description"]
            });

            if (!operatingCenter.StockedMaterials.Any(x => x.Material == material))
            {
                operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material });
            }

            container.GetInstance<ISession>().Save(operatingCenter);
            container.GetInstance<ISession>().Flush();
           // container.GetInstance<ISession>().Merge(operatingCenter);

            return material;
        }

        public static MaterialUsed CreateMaterialUsed(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrder =
                objectCache.Lookup<WorkOrder>("finalization work order",
                    nvc["finalization work order"]);
            var mu = container.GetInstance<Contractors.Tests.MaterialUsedFactory>().Create(new {
                WorkOrder = workOrder,
                Material =
                    objectCache.Lookup<Material>("material", nvc["material"]),
                StockLocation =
                    objectCache.Lookup<StockLocation>("stock location",
                        nvc["stock location"]),
                Quantity = Int32.Parse(nvc["quantity"])
            });

            workOrder = container.GetInstance<ISession>().Get<WorkOrder>(workOrder.Id);
            if (workOrder == null)
            {
                throw new Exception("work order not found");
            }
            if (workOrder.MaterialsUsed.Count == 0)
            {
                throw new Exception("work order has no materials");
            }

            return mu;
        }

        public static OperatingCenter CreateOperatingCenter(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<UniqueOperatingCenterFactory>().Create(new {
                OperatingCenterCode = nvc["opcntr"],
                OperatingCenterName = nvc["opcntrname"],
                SAPEnabled = nvc.GetValueAs<bool>("s a p enabled").GetValueOrDefault(),
                SAPWorkOrdersEnabled = nvc.GetValueAs<bool>("s a p work orders enabled").GetValueOrDefault(),
                IsContractedOperations = nvc.GetValueAs<bool>("is contracted operations").GetValueOrDefault(),
                MarkoutsEditable = nvc.GetValueAs<bool>("markouts editable").GetValueOrDefault()
            });
        }

        public static WorkOrderRequester CreateRequestedBy(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"])
            {
                case "Local Government":
                    return container.GetInstance<LocalGovernmentWorkOrderRequesterFactory>().Create();

                case "Employee":
                    return container.GetInstance<EmployeeWorkOrderRequesterFactory>().Create();

                default:
                    throw new NotSupportedException("A WorkOrderRequester isn't implemnted in Data.cs for " + nvc["description"]);
            }
        }

        public static Requisition CreateRequisition(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            var workOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey]);
            return container.GetInstance<RequisitionFactory>().Create(new
            {
                WorkOrder = workOrder,
                SAPRequisitionNumber = nvc["sap requisition number"]
            });
        }

        public static Restoration CreateRestoration(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            var workOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey]);

            return container.GetInstance<RestorationFactory>().Create(new {
                PavingSquareFootage = Decimal.Parse(nvc["paving square footage"]),
                WorkOrder = workOrder,
                RestorationType = objectCache.Lookup<RestorationType>("restoration type", nvc["restoration type"]),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["assigned contractor"]),
                InitialPurchaseOrderNumber = nvc["initial purchase order number"]
            });
        }

        public static RestorationType CreateRestorationType(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<RestorationTypeFactory>().Create(new {
                Description = nvc["description"]
            });
        }

        public static RestorationMethod CreateRestorationMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var restorationMethod = container.GetInstance<RestorationMethodFactory>().Create(new {
                Description = nvc["description"]
            });

            if (nvc["restoration type"] != null)
            {
                var restorationType =
                    objectCache.Lookup<RestorationType>("restoration type",
                        nvc["restoration type"]);

                if (restorationType != null)
                {
                    restorationMethod.RestorationTypes.Add(restorationType);
                    container.GetInstance<ISession>().Merge(restorationMethod);
                }
            }
            return restorationMethod;
        }

        public static Spoil CreateSpoil(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            return container.GetInstance<Contractors.Tests.SpoilFactory>().Create(new
            {
                Quantity = nvc.GetValueAs<decimal>("quantity").GetValueOrDefault(),
                SpoilStorageLocation = objectCache.Lookup<SpoilStorageLocation>("spoil storage location", nvc["spoil storage location"]),
                WorkOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey])
            });
        }

        public static SpoilStorageLocation CreateSpoilStorageLocation(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<SpoilStorageLocationFactory>().Create(new
            {
                Name = nvc["name"],
                OperatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"])
            });
        }

        public static StockLocation CreateStockLocation(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<StockLocationFactory>().Create(new {
                Description = nvc["description"],
                IsActive = nvc.GetValueAs<bool>("is active").GetValueOrDefault(true),
                OperatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"])
            });
        }

        public static Street CreateStreet(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<StreetFactory>().Create(new {
                FullStName = nvc["name"],
                Town = objectCache.Lookup<Town>("town", nvc["town"])
            });
        }

        public static StreetOpeningPermit CreateStreetOpeningPermit(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var workOrderKey = nvc.FindKeys("work order").FirstOrDefault();
            return container.GetInstance<StreetOpeningPermitFactory>().Create(new {
                ExpirationDate = nvc["expiration date"]?.ToDateTime(),
                DateIssued = nvc["date issued"].ToDateTime(),
                Notes = nvc["notes"],
                StreetOpeningPermitNumber = nvc["street opening permit number"],
                WorkOrder = objectCache.Lookup<WorkOrder>(workOrderKey, nvc[workOrderKey])
            }.CloneWithoutNulls());
        }

        public static TapImage CreateTapImage(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<TapImageFactory>().Create(new {
                PremiseNumber = nvc["premise number"],
                ServiceNumber = nvc["service number"],
                Folder = nvc["folder"],
                Filename = nvc["filename"],
                Town = objectCache.GetValueOrDefault<TownFactory>("town", nvc),
                OperatingCenter =
                nvc.AllKeys.Contains("operating center")
                    ? (object)
                    objectCache.Lookup<OperatingCenter>(
                        "operating center", nvc["operating center"])
                    : (object)typeof(UniqueOperatingCenterFactory),
            }.CloneWithoutNulls());
        }

        public static Town CreateTown(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            var town = container.GetInstance<TownFactory>().Create(new {
                ShortName = nvc["shortname"],
                County = objectCache.GetValueOrDefault<CountyFactory>("county", nvc)
            }.CloneWithoutNulls());
            
            if (nvc["operating center"] != null)
            {
                var operatingCenter = objectCache.Lookup<OperatingCenter>("operating center", nvc["operating center"]);
                if (operatingCenter != null)
                {
                    town.OperatingCenters.Add(operatingCenter);
                    container.GetInstance<ISession>().Merge(town);
                }
            }

            return town;
        }

        public static TownSection CreateTownSection(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<TownSectionFactory>().Create(new {
                Name = nvc["name"],
                Town = objectCache.Lookup<Town>("town", nvc["town"])
            }.CloneWithoutNulls());
        }

        public static MarkoutRequirement CreateMarkoutRequirement(NameValueCollection nvc, IContainer container)
        {
            switch (nvc["markout requirement"])
            {
                case "none":
                    return container.GetInstance<MarkoutRequirementNoneFactory>().Create();
                case "emergency":
                    return container.GetInstance<MarkoutRequirementEmergencyFactory>().Create();
                default:
                    return container.GetInstance<MarkoutRequirementRoutineFactory>().Create();
            }
        }

        public static PitcherFilterCustomerDeliveryMethod CreatePitcherFilterDeliveryMethod(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"])
            {
                case "handed to customer":
                    return container.GetInstance<PitcherFilterCustomerDeliveryMethodHandedToCustomerFactory>().Create();
                case "left on porch/doorstep":
                    return container.GetInstance<PitcherFilterCustomerDeliveryMethodLeftOnPorchFactory>().Create();
                case "other":
                    return container
                          .GetInstance<
                               PitcherFilterCustomerDeliveryMethodOtherFactory>()
                          .Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create pitcher filter delivery method with description '{nvc["description"]}'");
            }
        }

        //public static WorkOrder CreateWorkOrder<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        //    where TFactory : TestDataFactory<WorkOrder>
        //{
        //    dynamic args = new ExpandoObject();
        //    args.AssetType = objectCache.GetOrNull("asset type", nvc);
        //    args.Valve = GetValve(nvc, objectCache);
        //    args.AssignedContractor =
        //        objectCache.GetOrNull("contractor", nvc);
        //    args.CustomerName = nvc["customer name"];
        //    args.DateCompleted =
        //        nvc.AllKeys.Contains("date completed")
        //            ? nvc.GetValueAs<DateTime>("date completed")
        //            : (DateTime?)null;
        //    args.DateReceived = nvc.GetValueAs<DateTime>("date received");
        //    args.DateStarted = nvc.GetValueAs<DateTime>("date started");
        //    args.ExcavationDate = nvc.GetValueAs<DateTime>("excavation date");
        //    args.LostWater = nvc.GetValueAs<int>("lost water");
        //    // both these casts need to be here
        //    args.OperatingCenter =
        //        nvc.AllKeys.Contains("operating center")
        //        // ReSharper disable RedundantCast
        //            ? (object)objectCache.Lookup<OperatingCenter>(
        //                    "operating center", nvc["operating center"])
        //            : (object)typeof(UniqueOperatingCenterFactory);
        //        // ReSharper restore RedundantCast
        //    args.Notes = nvc["notes"];
        //    args.ORCOMServiceOrderNumber = nvc["orcom"];
        //    args.PhoneNumber = nvc["phone number"];
        //    args.PremiseNumber = nvc["premise number"];
        //    args.Street = objectCache.GetOrNull("street", nvc);
        //    args.StreetNumber = nvc["street number"];
        //    args.Town = objectCache.GetOrNull("town", nvc);
        //    args.WorkDescription =
        //        objectCache.GetOrNull(
        //            nvc.FindKeys("work description").SingleOrDefault(), nvc);
        //    switch (nvc["priority"])
        //    {
        //        case "emergency":
        //            args.Priority = typeof(EmergencyWorkOrderPriorityFactory);
        //            break;
        //    }

        //    return ((TFactory)Activator.CreateInstance(typeof(TFactory), Session)).Create(args);
        //}

        public static WorkOrder CreateWorkOrder<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)container.GetInstance(typeof(TFactory));
            var dateCompleted = (!String.IsNullOrWhiteSpace(nvc["date completed"])) ? nvc["date completed"].ToDateTime() : nvc.GetValueAs<DateTime>("date completed");

            return factory.Create(new {
                AssetType =
                    objectCache.GetOrNull("asset type", nvc),
                Valve = GetValve(nvc, objectCache, container),
                AssignedContractor =
                    objectCache.GetOrNull("contractor", nvc),
                CustomerName = nvc["customer name"],
                DateCompleted = dateCompleted,
                DateReceived = nvc.GetValueAs<DateTime>("date received"),
                DateStarted = nvc.GetValueAs<DateTime>("date started"),
                ExcavationDate = nvc.GetValueAs<DateTime>("excavation date"),
                LostWater = nvc.GetValueAs<int>("lost water").GetValueOrDefault(),
                OperatingCenter =
                    nvc.AllKeys.Contains("operating center")
                        ? (object)
                            objectCache.Lookup<OperatingCenter>(
                                "operating center", nvc["operating center"])
                        : (object)typeof(UniqueOperatingCenterFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container),
                Notes = nvc["notes"],
                ORCOMServiceOrderNumber = nvc["orcom"],
                PhoneNumber = nvc["phone number"],
                PremiseNumber = nvc["premise number"],
                Priority = nvc["priority"] == "emergency" ? typeof(EmergencyWorkOrderPriorityFactory) : typeof(RoutineWorkOrderPriorityFactory),
                RequestedBy = objectCache.GetOrNull("requested by", nvc),
                Service = objectCache.GetOrNull("service", nvc),
                Street = objectCache.GetOrNull("street", nvc),
                StreetOpeningPermitRequired =
                    nvc.GetValueAs<bool>("permit required"),
                StreetNumber = nvc["street number"],
                Town = objectCache.GetOrNull("town", nvc),
                TownSection = objectCache.GetOrNull("town section", nvc),
                WorkDescription =
                    objectCache.GetOrNull(
                        nvc.FindKeys("work description").SingleOrDefault(), nvc),
                Premise = objectCache.GetOrNull("premise", nvc)
            }.CloneWithoutNulls());
        }

        public static WorkOrder CreateWorkOrderWithValve<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var valve = GetValve(nvc, objectCache, container);
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new {
                AssetType = typeof(ValveAssetTypeFactory),
                Valve = valve,
                DateReceived = nvc.GetValueAs<DateTime>("date received").GetValueOrDefault(),
                NearestCrossStreet = (nvc["nearest cross street"] != null)
                    ? objectCache.Lookup<Street>("street",nvc["nearest cross street"])
                    : (object)typeof(StreetFactory),
                OperatingCenter =
                    nvc.AllKeys.Contains("operating center")
                        ? (object)
                            objectCache.Lookup<OperatingCenter>(
                                "operating center", nvc["operating center"])
                        : (object)typeof(UniqueOperatingCenterFactory),
                Service = objectCache.GetOrNull("service", nvc),
                Street = objectCache.GetOrNull("street", nvc),
                StreetNumber = nvc["street number"],
                StreetOpeningPermitRequired =
                    nvc.GetValueAs<bool>("permit required"),
                AssignedContractor =
                    objectCache.Lookup<Contractor>("contractor",
                        nvc["contractor"]),
                TrafficControlRequired =
                    nvc.GetValueAs<bool>("traffic control required"),
                WorkDescription =
                    objectCache.GetOrNull("valve work description", nvc),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container),
                Priority = nvc["priority"] == "emergency" ? typeof(EmergencyWorkOrderPriorityFactory) : typeof(RoutineWorkOrderPriorityFactory),
                Town = objectCache.GetOrNull("town", nvc),
                TownSection = objectCache.GetOrNull("town section", nvc),
                Purpose = objectCache.GetOrNull("work order purpose", nvc),
                RequestedBy = objectCache.GetOrNull("requested by", nvc),
            }.CloneWithoutNulls());
        }

        public static Valve GetValve(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // nvc["valve"] => "one"
            Valve valve;
            //check nvc for the key "valve"
            if (nvc["valve"] != null)
            {
                //if it has that key, use the valve to look up from the object cache
                if (objectCache.ContainsKey("valve"))
                {
                    valve = objectCache.Lookup<Valve>("valve", nvc["valve"]);
                    //if the valve exists in the object cache, use that, that's your valve
                    if (valve != null)
                        return valve;
                }

                //if it doesn't exist in the object cache, make a new one, use that, and add it to the object cache
                valve = container.GetInstance<ValveFactory>().Create();
                objectCache.EnsureDictionary("valve").Add(nvc["valve"] ?? "one", valve);
                return valve;
            }
            //if it doesn't have that key, just create a valve and attach it to the order */
            valve = container.GetInstance<ValveFactory>().Create();
            return valve;
        }

        public static WorkOrder CreateWorkOrderWithMayne<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof (TFactory), container);
            return factory.Create(new {
                AssetType = typeof(MainAssetTypeFactory),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                DateReceived = nvc.GetValueAs<DateTime>("date received").GetValueOrDefault(),
                WorkDescription = typeof(MainBreakRepairWorkDescriptionFactory),
                OperatingCenter =
                    nvc.AllKeys.Contains("operating center")
                        ? (object)
                            objectCache.Lookup<OperatingCenter>(
                                "operating center", nvc["operating center"])
                        : (object)typeof(UniqueOperatingCenterFactory),
                RequestedBy = objectCache.GetOrNull("requested by", nvc),
                StreetNumber = nvc["street number"],
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container),
                Purpose = objectCache.GetOrNull("work order purpose", nvc),
            }.CloneWithoutNulls());
        }
        
        public static WorkOrder CreateWorkOrderWithService<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new {
                AssetType = typeof(ServiceAssetTypeFactory),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                PremiseNumber = nvc["premise number"],
                ServiceNumber = nvc["service number"],
                WorkDescription = typeof(CurbBoxRepairWorkDescriptionFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container)
            }.CloneWithoutNulls());
        }

        public static WorkOrder CreateWorkOrderForServiceLineRenewal<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new
            {
                AssetType = typeof(ServiceAssetTypeFactory),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                PremiseNumber = nvc["premise number"],
                ServiceNumber = nvc["service number"],
                WorkDescription = typeof(ServiceLineRenewalWorkDescriptionFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container)
            });
        }

        public static WorkOrder CreateWorkOrderForServiceLineRenewalCompanySide<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new
            {
                AssetType = typeof(ServiceAssetTypeFactory),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                PremiseNumber = nvc["premise number"],
                ServiceNumber = nvc["service number"],
                WorkDescription = typeof(ServiceLineRenewalCompanySideFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container),
                Premise = objectCache.GetOrNull("premise", nvc),
                Service = objectCache.GetOrNull("service", nvc),
                HasPitcherFilterBeenProvidedToCustomer = nvc.GetValueAs<bool>("has pitcher filter been provided to customer"),
                DatePitcherFilterDeliveredToCustomer = nvc.GetValueAs<DateTime>("date pitcher filter delivered to customer")
            });
        }

        public static WorkOrder CreateWorkOrderForServiceLineRetire<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new {
                AssetType = typeof(ServiceAssetTypeFactory),
                AssignedContractor = objectCache.Lookup<Contractor>("contractor", nvc["contractor"]),
                PremiseNumber = nvc["premise number"],
                ServiceNumber = nvc["service number"],
                WorkDescription = typeof(ServiceLineRetireWorkDescriptionFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container),
                Premise = objectCache.GetOrNull("premise", nvc)
            });
        }

        public static WorkOrder CreateWorkOrderWithHydrant<TFactory>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);

            return
                factory.Create(new {
                        AssetType = typeof(HydrantAssetTypeFactory),
                        Valve = typeof(HydrantFactory),
                        MarkoutRequirement = CreateMarkoutRequirement(nvc, container)
                    });
        }

        public static object CreateWorkOrderForMainBreak<TFactory>(NameValueCollection nvc, TestObjectCache c, IContainer container)
            where TFactory : TestDataFactory<WorkOrder>
        {
            var factory = (TFactory)Activator.CreateInstance(typeof(TFactory), container);
            return factory.Create(new {
                AssetType = typeof(MainAssetTypeFactory),
                AssignedContractor = c.Lookup<Contractor>("contractor", nvc["contractor"]),
                WorkDescription = typeof(MainBreakRepairWorkDescriptionFactory),
                MarkoutRequirement = CreateMarkoutRequirement(nvc, container)
            });
        }

        // TODO: why are these methods generic?
        public static object CreateWorkOrderPriority<T>(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            switch (nvc["description"])
            {
                case "High Priority":
                    return container.GetInstance<HighPriorityWorkOrderPriorityFactory>()
                        .Create();
                case "Emergency":
                    return
                        container.GetInstance<EmergencyWorkOrderPriorityFactory>()
                            .Create();
                default:
                    return
                        container.GetInstance<RoutineWorkOrderPriorityFactory>()
                            .Create();
            }
        }

        public static WorkOrderPurpose CreateWorkOrderPurpose(NameValueCollection nvc, TestObjectCache obj, IContainer container)
        {
            switch (nvc["description"].ToLowerInvariant())
            {
                case "customer":
                    return container.GetInstance<CustomerWorkOrderPurposeFactory>().Create();
                case "compliance":
                    return container.GetInstance<ComplianceWorkOrderPurposeFactory>().Create();
                case "safety":
                    return container.GetInstance<SafetyWorkOrderPurposeFactory>().Create();
                case "leak detection":
                    return container.GetInstance<LeakDetectionWorkOrderPurposeFactory>().Create();
                case "revenue 150 to 500":
                    return container.GetInstance<Revenue150To500WorkOrderPurposeFactory>().Create();
                case "revenue 500 to 1000":
                    return container.GetInstance<Revenue500To1000WorkOrderPurposeFactory>().Create();
                case "revenue above 1000":
                    return container.GetInstance<RevenueAbove1000WorkOrderPurposeFactory>().Create();
                case "damaged billable":
                    return container.GetInstance<DamagedBillableWorkOrderPurposeFactory>().Create();
                case "estimates":
                    return container.GetInstance<EstimatesWorkOrderPurposeFactory>().Create();
                case "water quality":
                    return container.GetInstance<WaterQualityWorkOrderPurposeFactory>().Create();
                case "asset record control":
                    return container.GetInstance<AssetRecordControlWorkOrderPurposeFactory>().Create();
                case "seasonal":
                    return container.GetInstance<SeasonalWorkOrderPurposeFactory>().Create();
                case "demolition":
                    return container.GetInstance<DemolitionWorkOrderPurposeFactory>().Create();
                case "bpu":
                    return container.GetInstance<BPUWorkOrderPurposeFactory>().Create();
                case "hurricane sandy":
                    return container.GetInstance<HurricaneSandyWorkOrderPurposeFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create purpose with description '{nvc["description"]}'");
            }
        }

        /// <summary>
        /// Defaults to Hydrant Asset Type
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="objectCache"></param>
        /// <returns></returns>
        public static WorkDescription CreateHydrantWorkDescription(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<FrozenHydrantWorkDescriptionFactory>().Create(new {
                TimeToComplete = nvc.GetValueAs<decimal>("time to complete")
            });
        }
        
        public static WorkDescription CreateValveWorkDescription(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // Id is being set here because WorkDescription.Id is mapped with GeneratedBy().Assigned
            var desc = nvc.AllKeys.Contains("description")
                ? container.GetInstance<ValveWorkDescriptionFactory>().Create(new {
                    TimeToComplete = nvc.GetValueAs<decimal>("time to complete"),
                    Description = nvc["description"],
                    Id = nvc.GetValueAs<int>("id")
                })
                : container.GetInstance<ValveBoxRepairWorkDescriptionFactory>().Create(new {
                    TimeToComplete = nvc.GetValueAs<decimal>("time to complete"),
                    Id = nvc.GetValueAs<int>("id")
                });
            return desc;
        }

        public static WorkDescription CreateWorkDescription(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(nvc["description"]))
            {
                throw new NullReferenceException(
                    "To create a work description you must provide the required description as an attribute (i.e. a work description \"blah\" exists with description: \"hydrant repair\").");
            }

            switch (nvc["description"].ToLowerInvariant())
            {
                case "service line installation":
                    return container.GetInstance<Contractors.Tests.ServiceLineInstallationWorkDescriptionFactory>().Create();
                case "service line renewal company side":
                    return container.GetInstance<Contractors.Tests.ServiceLineRenewalCompanySideFactory>().Create();
                default:
                    throw new InvalidOperationException(
                        $"Unable to create work description with description '{nvc["description"]}'.");
            }
        }

        public static ValveImage CreateValveImage(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            // There's something weird here when generating a default Valve if one isn't supplied. Some bizarre error shows up when running the ValveImage scenarios
            // then running the WorkOrderReadOnly scenarios. The WorkOrderReadOnly scenarios all fail for some reason
            // I don't have time to investigate at the moment. -Ross 9/12/2017

            Valve valve = null;

            if (nvc.AllKeys.Contains("valve"))
            {
                valve = objectCache.Lookup<Valve>("valve", nvc["valve"]);
            }

            var valveImage = container.GetInstance<ValveImageFactory>().Create(new {
                Town = objectCache.GetValueOrDefault<TownFactory>("town", nvc),
                Valve = valve,
                OperatingCenter = objectCache.GetValueOrDefault<UniqueOperatingCenterFactory>("operating center", nvc)
            });
            return valveImage;
        }

        private static object CreateWorkOrderDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var workOrder = (WorkOrder)cache.GetValueOrDefault<WorkOrderFactory>("work order", nvc);
            var documentType = (DocumentType)cache.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);
            var dataType = (DataType)cache.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            return container.GetInstance<DocumentLinkFactory>().Create(new {
                DocumentType = documentType,
                DataType = dataType,
                Document = document,
                LinkedId = workOrder.Id
            });
        }

        private static object CreatePlanningWorkOrderDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var workOrder = (WorkOrder)cache.GetValueOrDefault<PlanningWorkOrderFactory>("planning work order", nvc);
            var documentType = (DocumentType)cache.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);
            var dataType = (DataType)cache.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            return container.GetInstance<DocumentLinkFactory>().Create(new {
                DocumentType = documentType,
                DataType = dataType,
                Document = document,
                LinkedId = workOrder.Id
            });
        }

        private static object CreateFinalizationWorkOrderDocumentLink(NameValueCollection nvc, TestObjectCache cache, IContainer container)
        {
            var document = (Document)cache.GetValueOrDefault<DocumentFactory>("document", nvc);
            var workOrder = (WorkOrder)cache.GetValueOrDefault<FinalizationWorkOrderFactory>("finalization work order", nvc);
            var documentType = (DocumentType)cache.GetValueOrDefault<DocumentTypeFactory>("document type", nvc);
            var dataType = (DataType)cache.GetValueOrDefault<DataTypeFactory>("data type", nvc);

            return container.GetInstance<DocumentLinkFactory>().Create(new {
                DocumentType = documentType,
                DataType = dataType,
                Document = document,
                LinkedId = workOrder.Id
            });
        }

        public static RestorationResponsePriority CreateRestorationResponsePriority(NameValueCollection nvc, TestObjectCache objectCache, IContainer container)
        {
            return container.GetInstance<RestorationResponsePriorityFactory>().Create();
        }

        public static MeterLocation CreateMeterLocation(NameValueCollection nvc, TestObjectCache objectCache,
            IContainer container)
        {
            return container.GetInstance<MeterLocationFactory>().Create(new {
                Description = nvc["description"],
                SAPCode = nvc["code"],
            });
        }

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
                deleporterAssetType.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType {
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

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterServiceMaterial = session.Load<ServiceMaterial>(serviceMaterial.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterServiceMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial { OperatingCenter = deleporterOperatingCenter, ServiceMaterial = deleporterServiceMaterial, NewServiceRecord = true });
                session.SaveOrUpdate(deleporterServiceMaterial);
            });
        }

        [Given("operating center:? \"([^\"]+)\" exists in town:? \"([^\"]+)\"")]
        public static void GivenIAddOperatingCenterToTown(string operatingCenterIdentifier, string townIdentifier)
        {
            var town = TestObjectCache.Instance.Lookup<Town>("town", townIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var deleporterTown = session.Load<Town>(town.Id);
                var deleporterOperatingCenter = session.Load<OperatingCenter>(operatingCenter.Id);
                deleporterTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = deleporterOperatingCenter, Town = deleporterTown, Abbreviation = "XX" });
                session.SaveOrUpdate(deleporterTown);
            });
        }

        [Given("I have deleted the default markout for ([\\w\\s]*work order):? \"([^\"]+)\"")]
        public static void GivenIDeleteMarkoutForWorkOrder(string workOrderType, string workOrderIdentifier)
        {
            var workOrderId =
                TestObjectCache.Instance.Lookup<WorkOrder>(workOrderType, workOrderIdentifier).Id;

            Deleporter.Run(() => {
                var session = DependencyResolver.Current.GetService<ISession>();
                var workOrder = session.Load<WorkOrder>(workOrderId);
                
                session.Delete(workOrder.Markouts.First());
            });
        }

        #endregion
    }
}
