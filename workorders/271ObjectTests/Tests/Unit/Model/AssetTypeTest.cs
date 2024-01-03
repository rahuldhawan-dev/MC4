using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for AssetTypeTestTest
    /// </summary>
    [TestClass]
    public class AssetTypeTest : WorkOrdersTestClass<AssetType>
    {
        #region Exposed Static Methods

        public static AssetType GetValidAssetType()
        {
            return AssetTypeRepository.Valve;
        }

        public static void DeleteAssetType(AssetType entity)
        {
            AssetTypeRepository.Delete(entity);
        }

        #endregion

        #region Private Methods

        protected override AssetType GetValidObject()
        {
            return GetValidAssetType();
        }

        protected override AssetType GetValidObjectFromDatabase()
        {
            var obj = GetValidObject();
            AssetTypeRepository.Insert(obj);
            return obj;
        }

        protected override void DeleteObject(AssetType entity)
        {
            DeleteAssetType(entity);
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var description = "Test Asset";
            var target = new AssetType {
                Description = description
            };

            Assert.AreEqual(description, target.ToString());
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }
    }

    public class TestAssetTypeBuilder : TestDataBuilder<AssetType>
    {
        #region Constants

        // something may come back to bite us here. 
        // ClassNames and Values in the table do not match.
        public struct Descriptions
        {
            public const string VALVE = "Valve",
                                HYDRANT = "Hydrant",
                                MAIN = "Main",
                                SERVICE = "Service",
                                SEWER_OPENING = "SewerOpening",
                                SEWER_LATERAL = "SewerLateral",
                                SEWER_MAIN = "SewerMain",
                                STORM_CATCH = "StormCatch",
                                EQUIPMENT = "Equipment";
        }

        #endregion

        #region Private Static Members

        private static AssetType _valve, _hydrant, _main, _service, _sewerOpening, _sewerLateral, _sewerMain, _stormCatch, _equipment;

        #endregion

        #region Private Members

        private string _typeName;

        #endregion

        #region Private Methods

        protected static AssetType GetAssetType(string name)
        {
            switch (name)
            {
                case Descriptions.VALVE:
                    return GetValveAssetType();
                case Descriptions.HYDRANT:
                    return GetHydrantAssetType();
                case Descriptions.MAIN:
                    return GetMainAssetType();
                case Descriptions.SERVICE:
                    return GetServiceAssetType();
                case Descriptions.SEWER_OPENING:
                    return GetSewerOpeningAssetType();
                case Descriptions.SEWER_LATERAL:
                    return GetSewerLateralAssetType();
                case Descriptions.SEWER_MAIN:
                    return GetSewerMainAssetType();
                case Descriptions.STORM_CATCH:
                    return GetStormCatchType();
                case Descriptions.EQUIPMENT:
                    return GetEquipmentPurpose();
                default:
                    return null;
            }
        }

        private static AssetType GetValveAssetType()
        {
            if (_valve == null)
                _valve = new AssetType
                {
                    AssetTypeID = AssetTypeRepository.Indices.VALVE,
                    Description = AssetTypeRepository.Descriptions.VALVE
                };
            return _valve;
        }

        private static AssetType GetHydrantAssetType()
        {
            if (_hydrant == null)
                _hydrant = new AssetType
                {
                    AssetTypeID = AssetTypeRepository.Indices.HYDRANT,
                    Description = AssetTypeRepository.Descriptions.HYDRANT
                };
            return _hydrant;
        }

        private static AssetType GetMainAssetType()
        {
            if (_main == null)
                _main = new AssetType
                {
                    AssetTypeID = AssetTypeRepository.Indices.MAIN,
                    Description = AssetTypeRepository.Descriptions.MAIN
                };
            return _main;
        }

        private static AssetType GetServiceAssetType()
        {
            if (_service == null)
                _service = new AssetType
                {
                    AssetTypeID = AssetTypeRepository.Indices.SERVICE,
                    Description = AssetTypeRepository.Descriptions.SERVICE
                };
            return _service;
        }

        private static AssetType GetSewerOpeningAssetType()
        {
            if (_sewerOpening == null)
                _sewerOpening = new AssetType {
                    AssetTypeID = AssetTypeRepository.Indices.SEWER_OPENING,
                    Description = AssetTypeRepository.Descriptions.SEWER_OPENING
                };
            return _sewerOpening;
        }

        private static AssetType GetSewerLateralAssetType()
        {
            if (_sewerLateral == null)
                _sewerLateral = new AssetType {
                    AssetTypeID = AssetTypeRepository.Indices.SEWER_LATERAL,
                    Description = AssetTypeRepository.Descriptions.SEWER_LATERAL
                };
            return _sewerLateral;
        }

        private static AssetType GetSewerMainAssetType()
        {
            if (_sewerMain == null)
                _sewerMain = new AssetType {
                    AssetTypeID = AssetTypeRepository.Indices.SEWER_MAIN,
                    Description = AssetTypeRepository.Descriptions.SEWER_MAIN
                };
            return _sewerMain;
        }

        private static AssetType GetStormCatchType()
        {
            if (_stormCatch == null)
                _stormCatch = new AssetType {
                    AssetTypeID = AssetTypeRepository.Indices.STORM_CATCH,
                    Description = AssetTypeRepository.Descriptions.STORM_CATCH
                };
            return _stormCatch;
        }

        private static AssetType GetEquipmentPurpose()
        {
            if (_equipment == null)
                _equipment = new AssetType {
                    AssetTypeID = AssetTypeRepository.Indices.EQUIPMENT,
                    Description = AssetTypeRepository.Descriptions.EQUIPMENT
                };
            return _equipment;
        }

        #endregion

        #region Exposed Methods
        
        public override AssetType Build()
        {
            return GetAssetType(_typeName);
        }

        public TestAssetTypeBuilder WithTypeName(string name)
        {
            _typeName = name;
            return this;
        }

        #endregion
    }

    // TODO: Should this be internal?
    public class TestAssetTypeBuilder<TAsset> : TestAssetTypeBuilder
        where TAsset : IAsset
    {
        #region Private Static Properties

        private static Type typeOfAsset
        {
            get { return typeof (TAsset); }
        }

        #endregion

        #region Exposed Methods
        
        public override AssetType Build()
        {
            return GetAssetType(typeOfAsset.Name);
        }

        #endregion
    }
}
