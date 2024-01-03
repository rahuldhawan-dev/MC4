using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.W1V.ImportTasks
{
    [TestClass]
    public class CustomerMaterialTaskTest
        : FileImportTaskTestBase<
            IW1VFileParser,
            IW1VFileDownloadService,
            ShortCycleCustomerMaterial,
            IShortCycleCustomerMaterialRepository,
            CustomerMaterialTask>
    {
        private Mock<IW1VRecordMapper> _mapper;

        protected override void InitializeContainer(ConfigurationExpression e)
        {
            base.InitializeContainer(e);

            _mapper = e.For<IW1VRecordMapper>().Mock();
        }

        [TestMethod]
        public void Test_Run_DownloadsParsesMapsAndSavesEntityRecordsAndThenDeletesFiles()
        {
            var fileName = "fileName";
            var fileContent = "fileContent";
            
            _downloadService.Setup(DownloadMethod).Returns(new List<FileData> {
                new FileData(fileName, fileContent)
            });
            _parser.Setup(x => x.ParseCustomerMaterial(fileContent))
                   .Returns(new List<W1VFileParser.ParsedCustomerMaterial> {
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 1
                        },
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 2
                        },
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 3
                        }
                    });
            
            for (var i = 1; i < 4; ++i)
            {
                var cur = i;
                _repository.Setup(x => x.FindByWorkOrderNumber(cur))
                           .Returns(new ShortCycleCustomerMaterial {
                                ShortCycleWorkOrderNumber = cur
                            });
                _repository.Setup(x =>
                    x.Save(It.Is<ShortCycleCustomerMaterial>(m => m.ShortCycleWorkOrderNumber == cur)));
            }
            _downloadService.Setup(x => x.DeleteFile(fileName));
            
            _target.Run();
        }
        
        [TestMethod]
        public void Test_Run_ParsesMapsAndSavesNewEntities()
        {
            var fileName = "fileName";
            var fileContent = "fileContent";

            _downloadService.Setup(DownloadMethod).Returns(new List<FileData> {
                new FileData(fileName, fileContent)
            });
            _parser.Setup(x => x.ParseCustomerMaterial(fileContent))
                   .Returns(new List<W1VFileParser.ParsedCustomerMaterial> {
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 1,
                            MeterSize = "small"
                        },
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 2,
                            MeterSize = "small"
                        },
                        new W1VFileParser.ParsedCustomerMaterial {
                            WorkOrderNumber = 3,
                            MeterSize = "small"
                        }
                    });
            
            for (var i = 1; i < 4; ++i)
            {
                var cur = i;
                _repository.Setup(x => x.FindByWorkOrderNumber(cur))
                           .Returns((ShortCycleCustomerMaterial)null);
                _repository.Setup(x =>
                    x.Save(It.Is<ShortCycleCustomerMaterial>(m => m != null)));
            }
            _downloadService.Setup(x => x.DeleteFile(fileName));
            
            _target.Run();
        }

        [TestMethod]
        public void Test_Run_ParsesMapsAndSavesNewEntityWithPremise()
        {
            var fileName = "fileName";
            var fileContent = "fileContent";

            // we need a premise with a most recently installed customer service
            var premise = new Premise { Id = 5, PremiseNumber = "12345", Installation = "1234", DeviceLocation = "123" };
            var service = new Service { Premise = premise, DateInstalled = DateTime.Now, ServiceCategory = new ServiceCategory() };
            premise.Services = new[] { service };
            var mostRecentService = new MostRecentlyInstalledService {
                Service = service,
                Premise = premise
            };
            premise.MostRecentService = mostRecentService;
            //sanity check, we got this setup correctly
            Assert.AreEqual(service, premise.MostRecentService.Service);
            var entity = new ShortCycleCustomerMaterial {
                Premise = premise
            };
            _downloadService.Setup(DownloadMethod).Returns(new List<FileData> {
                new FileData(fileName, fileContent)
            });
            var parsedCustomerMaterial = new W1VFileParser.ParsedCustomerMaterial {
                WorkOrderNumber = 1,
                MeterSize = "small",
                PremiseId = premise.PremiseNumber,
                FunctionalLocation = premise.DeviceLocation,
                Installation = premise.Installation
            };
            _parser.Setup(x => x.ParseCustomerMaterial(fileContent))
                   .Returns(new List<W1VFileParser.ParsedCustomerMaterial> { parsedCustomerMaterial });

            _repository.Setup(x => x.FindByWorkOrderNumber(1)).Returns((ShortCycleCustomerMaterial)null);
            // this is where the magic happens.
            _mapper.Setup(x => x.Map(It.IsAny<ShortCycleCustomerMaterial>(), It.IsAny<W1VFileParser.ParsedCustomerMaterial>()))
                   .Callback<ShortCycleCustomerMaterial, W1VFileParser.ParsedCustomerMaterial>((x, y) =>
                        x.Premise = premise);
            _repository.Setup(x => x.Save(It.Is<ShortCycleCustomerMaterial>(m =>
                m != null
                && m.Premise.MostRecentService.Service.NeedsToSync == true
            )));

            _downloadService.Setup(x => x.DeleteFile(fileName));

            _target.Run();
        }
    }
}
