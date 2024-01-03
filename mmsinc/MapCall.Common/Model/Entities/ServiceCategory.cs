using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceCategory : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int
                FIRE_RETIRE_SERVICE_ONLY = 1,
                FIRE_SERVICE_INSTALLATION = 2,
                FIRE_SERVICE_RENEWAL = 3,
                INSTALL_METER_SET = 4,
                IRRIGATION_NEW = 5,
                IRRIGATION_RENEWAL = 6,
                REPLACE_METER_SET = 7,
                SEWER_MEASUREMENT_ONLY = 8,
                SEWER_RECONNECT = 9,
                SEWER_RETIRE_SERVICE_ONLY = 10,
                SEWER_SERVICE_INCREASE_SIZE = 11,
                SEWER_SERVICE_NEW = 12,
                SEWER_SERVICE_RENEWAL = 13,
                SEWER_SERVICE_SPLIT = 14,
                WATER_MEASUREMENT_ONLY = 15,
                WATER_RECONNECT = 16,
                WATER_RELOCATE_METER_SET = 17,
                WATER_RETIRE_METER_SET_ONLY = 18,
                WATER_RETIRE_SERVICE_ONLY = 19,
                WATER_SERVICE_INCREASE_SIZE = 20,
                WATER_SERVICE_NEW_COMMERCIAL = 21,
                WATER_SERVICE_NEW_DOMESTIC = 22,
                WATER_SERVICE_RENEWAL = 23,
                WATER_SERVICE_SPLIT = 24,
                SEWER_INSTALL_CLEAN_OUT = 25,
                SEWER_REPLACE_CLEAN_OUT = 26,
                WATER_SERVICE_RENEWAL_CUST_SIDE = 27,
                WATER_COMMERICAL_RECORD_IMPORT = 28,
                WATER_DOMESTIC_RECORD_IMPORT = 29,
                FIRE_SERVICE_RECORD_IMPORT = 30,
                IRRIGATION_SERVICE_RECORD_IMPORT = 31,
                FIRE_SERVICE_RECONNECT = 32,
                SEWER_SERVICE_RECORD_IMPORT = 34;
        }

        public static int[] GetLengthOfServiceRequiredValues()
        {
            return new[] {
                Indices.INSTALL_METER_SET,
                Indices.REPLACE_METER_SET,
                Indices.SEWER_MEASUREMENT_ONLY,
                Indices.WATER_MEASUREMENT_ONLY,
                Indices.WATER_RELOCATE_METER_SET,
                Indices.WATER_RETIRE_METER_SET_ONLY
            };
        }

        public static int[] GetSewerCategories()
        {
            return new[] {
                Indices.SEWER_SERVICE_NEW,
                Indices.SEWER_INSTALL_CLEAN_OUT,
                Indices.SEWER_MEASUREMENT_ONLY,
                //Indices.SEWER_RECONNECT,
                Indices.SEWER_REPLACE_CLEAN_OUT,
                Indices.SEWER_RETIRE_SERVICE_ONLY,
                Indices.SEWER_SERVICE_INCREASE_SIZE,
                Indices.SEWER_SERVICE_RENEWAL,
                Indices.SEWER_SERVICE_SPLIT
            };
        }

        public static int[] GetWaterNewServiceCategories()
        {
            return new[] {
                Indices.WATER_SERVICE_NEW_COMMERCIAL,
                Indices.WATER_SERVICE_NEW_DOMESTIC
            };
        }

        public static int[] GetNewServiceCategories()
        {
            return new[] {
                Indices.WATER_SERVICE_NEW_COMMERCIAL,
                Indices.IRRIGATION_NEW,
                Indices.SEWER_SERVICE_NEW,
                Indices.WATER_SERVICE_NEW_DOMESTIC,
                Indices.FIRE_SERVICE_INSTALLATION
            };
        }

        public static int[] GetRenewalServiceCategories()
        {
            return new[] {
                Indices.FIRE_SERVICE_RENEWAL,
                Indices.IRRIGATION_RENEWAL,
                Indices.SEWER_SERVICE_RENEWAL,
                Indices.WATER_SERVICE_RENEWAL
            };
        }

        public static int[] GetRenewalSampleSiteServiceCategories()
        {
            return new[] {
                Indices.WATER_RECONNECT,
                Indices.WATER_RETIRE_SERVICE_ONLY,
                Indices.WATER_SERVICE_INCREASE_SIZE,
                Indices.WATER_SERVICE_RENEWAL,
                Indices.WATER_SERVICE_SPLIT
            };
        }

        public static int[] GetCustomerMaterialSizeServiceCategories()
        {
            return new[] {
                Indices.FIRE_SERVICE_INSTALLATION,
                Indices.FIRE_SERVICE_RENEWAL,
                Indices.IRRIGATION_NEW,
                Indices.IRRIGATION_RENEWAL,
                Indices.SEWER_SERVICE_NEW,
                Indices.SEWER_SERVICE_RENEWAL,
                Indices.WATER_SERVICE_NEW_COMMERCIAL,
                Indices.WATER_SERVICE_NEW_DOMESTIC,
                Indices.WATER_SERVICE_RENEWAL,
                Indices.WATER_SERVICE_RENEWAL_CUST_SIDE,
                Indices.WATER_COMMERICAL_RECORD_IMPORT,
                Indices.WATER_DOMESTIC_RECORD_IMPORT,
                Indices.FIRE_SERVICE_RECORD_IMPORT,
                Indices.IRRIGATION_SERVICE_RECORD_IMPORT
            };
        }

        #endregion

        #region Table Properties

        public virtual ServiceUtilityType ServiceUtilityType { get; set; }

        #endregion

        public virtual bool IsSewerCategory => Description.ToUpper().Contains("SEWER");
    }
}
