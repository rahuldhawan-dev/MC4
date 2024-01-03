using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using WorkOrders.Model;
using WorkOrders.Presenters.Assets;
using WorkOrders.Views.Assets;

namespace _271ObjectTests.Tests.Unit.Presenters.Assets
{
    /// <summary>
    /// Summary description for AssetLatLonPickerPresenterTest
    /// </summary>
    [TestClass]
    public class AssetLatLonPickerPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IAssetLatLonPickerView _view;
        private AssetLatLonPickerPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<IAssetLatLonPickerView>();
            _target = new AssetLatLonPickerPresenter(_view);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestOnViewInitializedDoesNothing()
        {
            _view = _mocks.CreateMock<IAssetLatLonPickerView>();
            _target = new AssetLatLonPickerPresenter(_view);

            using (_mocks.Record())
            {
                // expect nothing
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestRepositoryPropertyNotImplemented()
        {
            var repository = _mocks.DynamicMock<IRepository<Asset>>();

            _mocks.ReplayAll();

            MyAssert.Throws(() => _target.Repository = repository,
                typeof(NotImplementedException));
            MyAssert.Throws(() => repository = _target.Repository,
                typeof(NotImplementedException));
        }
    }

    public class MockAssetTypeRepository : MockRepository<AssetType>
    {
        public override AssetType Get(object objID)
        {
            var id = (int)objID;
            Func<string, AssetType> getAssetType = description => new AssetType
            {
                AssetTypeID = id,
                Description = description
            };
            switch (id)
            {
                case AssetTypeRepository.Indices.VALVE:
                    return getAssetType(AssetTypeRepository.Descriptions.VALVE);
                case AssetTypeRepository.Indices.HYDRANT:
                    return getAssetType(AssetTypeRepository.Descriptions.HYDRANT);
                case AssetTypeRepository.Indices.MAIN:
                    return getAssetType(AssetTypeRepository.Descriptions.MAIN);
                case AssetTypeRepository.Indices.SERVICE:
                    return getAssetType(AssetTypeRepository.Descriptions.SERVICE);
                case AssetTypeRepository.Indices.SEWER_OPENING:
                    return getAssetType(AssetTypeRepository.Descriptions.SEWER_OPENING);
                case AssetTypeRepository.Indices.SEWER_MAIN:
                    return getAssetType(AssetTypeRepository.Descriptions.SEWER_MAIN);
                case AssetTypeRepository.Indices.SEWER_LATERAL:
                    return getAssetType(AssetTypeRepository.Descriptions.SEWER_LATERAL);
                case AssetTypeRepository.Indices.STORM_CATCH:
                    return getAssetType(AssetTypeRepository.Descriptions.STORM_CATCH);
                default:
                    return null;
            }
        }
    }

    public class MockValveRepository : MockRepository<Valve>
    {
        public override Valve Get(object id)
        {
            return new Valve
            {
                ValveID = (int)id
            };
        }
    }

    public class MockHydrantRepository : MockRepository<Hydrant>
    {
        public override Hydrant Get(object id)
        {
            return new Hydrant
            {
                HydrantID = (int)id
            };
        }
    }

    public class MockSewerOpeningRepository : MockRepository<SewerOpening>
    {
        public override SewerOpening Get(object id)
        {
            return new SewerOpening 
            {
                Id = (int)id
            };
        }
    }

    public class MockStormCatchRepository : MockRepository<StormCatch>
    {
        public override StormCatch Get(object id)
        {
            return new StormCatch {
                StormCatchID = (int)id
            };
        }
    }
}
