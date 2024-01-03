using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving AssetType objects from persistence.
    /// </summary>
    public class AssetTypeRepository : WorkOrdersRepository<AssetType>
    {
        #region Constants

        public struct Indices
        {
            public const short VALVE = 1,
                               HYDRANT = 2,
                               MAIN = 3,
                               SERVICE = 4,
                               SEWER_OPENING = 5,
                               SEWER_LATERAL = 6,
                               SEWER_MAIN = 7,
                               STORM_CATCH = 8,
                               EQUIPMENT = 9,
                               FACILITY = 11,
                               MAIN_CROSSING = 12;
        }

        public struct Descriptions
        {
            public const string VALVE = "Valve",
                                HYDRANT = "Hydrant",
                                MAIN = "Main",
                                SERVICE = "Service",
                                SEWER_OPENING = "Sewer Opening",
                                SEWER_LATERAL = "Sewer Lateral",
                                SEWER_MAIN = "Sewer Main", 
                                STORM_CATCH = "Storm/Catch", 
                                EQUIPMENT = "Equipment", MAIN_CROSSING = "Main Crossing";
        }

        #endregion

        #region Pirate Static Members
        // 20080919 by Jason Duncan -- Yarr

        private static AssetType _valve,
                                 _hydrant,
                                 _main,
                                 _service, 
                                 _sewerOpening, 
                                 _sewerLateral, 
                                 _sewerMain, 
                                 _stormCatch,
                                 _equipment;

        #endregion

        #region Static Properties

        public static AssetType Valve
        {
            get
            {
                _valve = RetrieveAndAttach(Indices.VALVE);
                return _valve;
            }
        }

        public static AssetType Hydrant
        {
            get
            {
                _hydrant = RetrieveAndAttach(Indices.HYDRANT);
                return _hydrant;
            }
        }

        public static AssetType Main
        {
            get
            {
                _main = RetrieveAndAttach(Indices.MAIN);
                return _main;
            }
        }

        public static AssetType Service
        {
            get
            {
                _service = RetrieveAndAttach(Indices.SERVICE);
                return _service;
            }
        }

        public static AssetType SewerOpening
        {
            get
            {
                _sewerOpening = RetrieveAndAttach(Indices.SEWER_OPENING);
                return _sewerOpening;
            }
        }

        public static AssetType StormCatch
        {
            get
            {
                _stormCatch = RetrieveAndAttach(Indices.STORM_CATCH);
                return _stormCatch;
            }
        }

        public static AssetType SewerLateral
        {
            get
            {
                _sewerLateral = RetrieveAndAttach(Indices.SEWER_LATERAL);
                return _sewerLateral;
            }
        }

        public static AssetType SewerMain
        {
            get
            {
                _sewerMain = RetrieveAndAttach(Indices.SEWER_MAIN);
                return _sewerMain;
            }
        }

        public static AssetType Equipment
        {
            get
            {
                _equipment = RetrieveAndAttach(Indices.EQUIPMENT);
                return _equipment;
            }
        }

        #endregion

        #region Pirate Static Methods

        private static AssetType RetrieveAndAttach(int index)
        {
            var type = GetEntity(index);
            if (!DataTable.Contains(type))
                DataTable.Attach(type);
            return type;
        }

        #endregion

        #region Exposed Static Methods

        public static AssetTypeEnum GetEnumerationValue(AssetType type)
        {
            switch (type.AssetTypeID)
            {
                case Indices.VALVE:
                    return AssetTypeEnum.Valve;
                case Indices.HYDRANT:
                    return AssetTypeEnum.Hydrant;
                case Indices.MAIN:
                    return AssetTypeEnum.Main;
                case Indices.SEWER_OPENING:
                    return AssetTypeEnum.SewerOpening;
                case Indices.STORM_CATCH:
                    return AssetTypeEnum.StormCatch;
                case Indices.SEWER_LATERAL:
                    return AssetTypeEnum.SewerLateral;
                case Indices.SEWER_MAIN:
                    return AssetTypeEnum.SewerMain;
                case Indices.EQUIPMENT:
                    return AssetTypeEnum.Equipment;
                case Indices.MAIN_CROSSING:
                    return AssetTypeEnum.MainCrossing;
                default:
                    return AssetTypeEnum.Service;
            }
        }

        #endregion
    }
}