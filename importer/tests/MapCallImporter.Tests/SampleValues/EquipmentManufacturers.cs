using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.SampleValues
{
    public static class EquipmentManufacturers
    {
        public static string GetInsertQuery(string equipmentType)
        {
            switch (equipmentType)
            {
                #region ADJUSTABLE SPEED DRIVE

                case "ADJUSTABLE SPEED DRIVE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1928, 121, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1929, 121, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1930, 121, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1931, 121, 'BARDAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1932, 121, 'BENSHAW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1933, 121, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1934, 121, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1935, 121, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1936, 121, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1937, 121, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1938, 121, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1939, 121, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1940, 121, 'KOLLMORGEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1941, 121, 'LENZE-AC TECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1942, 121, 'MAGNETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1943, 121, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1944, 121, 'RELIANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1945, 121, 'ROBICON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1946, 121, 'SAFETRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1947, 121, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1948, 121, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1949, 121, 'TELEMECANIQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1950, 121, 'TOSHIBA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1951, 121, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1952, 121, 'US DRIVES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1953, 121, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1954, 121, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1955, 121, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1956, 121, 'YASKAWA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3863, 121, 'VACON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3974, 121, 'Eaton', 'Eaton');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4093, 121, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region AERATOR

                case "AERATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3691, 228, 'LOWRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3692, 228, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3963, 228, 'Baldor', 'Baldor');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4198, 228, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region AIR COMPRESSOR

                case "AIR COMPRESSOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2189, 145, 'CAMBELL HAUSFIELD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2190, 145, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2191, 145, 'CHAMPION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2192, 145, 'COPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2193, 145, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2194, 145, 'EMGLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2195, 145, 'FORD MOTOR COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2196, 145, 'GARDNER DENVER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2197, 145, 'GAST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2198, 145, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2199, 145, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2200, 145, 'MAXUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2201, 145, 'MIDWEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2202, 145, 'QUINCY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2203, 145, 'RIETSCHE THOMES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2204, 145, 'SPEEDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2205, 145, 'SULLAIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2206, 145, 'SULLIVAN-PALATEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2207, 145, 'THOMAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2208, 145, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2209, 145, 'WORTHINGTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3899, 145, 'ATLAS CAPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3960, 145, 'KAESER', 'KAESER');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4117, 145, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region AIR/ VACUUM TANK

                case "AIR/ VACUUM TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3604, 222, 'AMTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3605, 222, 'BRUNNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3606, 222, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3607, 222, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3608, 222, 'LOCHINVAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3609, 222, 'MANCHESTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3610, 222, 'MELBEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3611, 222, 'PENWAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3612, 222, 'QUINCY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3613, 222, 'SPEEDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3614, 222, 'STEELFAB-SAMUEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3615, 222, 'STOYSTOWN TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3616, 222, 'TACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3617, 222, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3618, 222, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4192, 222, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region AM WATER NARUC ACCOUNT

                case "AM WATER NARUC ACCOUNT":
                    return @"

";

                #endregion

                #region AMIDATACOLL

                case "AMIDATACOLL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3875, 241, 'ACLARA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3876, 241, 'ITRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3877, 241, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3878, 241, 'SENSUS', NULL);
";

                #endregion

                #region ARC FLASH PROTECTION

                case "ARC FLASH PROTECTION":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3072, 193, 'LITHONIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3073, 193, 'NORTH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3074, 193, 'SALISBURY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3075, 193, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4163, 193, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BATTERY

                case "BATTERY":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1960, 123, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1961, 123, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1962, 123, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1963, 123, 'CD TECHNOLOGIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1964, 123, 'DEKA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1965, 123, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1966, 123, 'DYNACELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1967, 123, 'EXIDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1968, 123, 'FIAMM-MONOLITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1969, 123, 'HALOGEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1970, 123, 'HAZE BATTERY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1971, 123, 'INTERSTATE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1972, 123, 'MAXI POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1973, 123, 'NAPA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1974, 123, 'POWERGUARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1975, 123, 'POWERSONIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1976, 123, 'RAYOVAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1977, 123, 'SAFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1978, 123, 'UNIVERSAL BATTERY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1979, 123, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1980, 123, 'VARTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1981, 123, 'YUASA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3879, 123, 'PHOENIX CONTACT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4095, 123, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BATTERY CHARGER

                case "BATTERY CHARGER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1982, 124, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1983, 124, 'CHARLES INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1984, 124, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1985, 124, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1986, 124, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1987, 124, 'EXIDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1988, 124, 'GENERAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1989, 124, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1990, 124, 'GUEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1991, 124, 'INTERSTATE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1992, 124, 'KATOLIGHT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1993, 124, 'KOHLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1994, 124, 'LAMARCHE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1995, 124, 'MASTER CONTROL SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1996, 124, 'METRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1997, 124, 'ONAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1998, 124, 'POWERGUARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1999, 124, 'POWERSONIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2000, 124, 'SAFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2001, 124, 'SCHUMACHER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2002, 124, 'STORED ENERGY SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2003, 124, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2004, 124, 'VARTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4096, 124, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BLOW OFF VALVE

                case "BLOW OFF VALVE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3554, 219, 'AMERICAN AVK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3555, 219, 'AMERICAN CAST IRON PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3556, 219, 'AMERICAN DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3557, 219, 'AMERICAN FLOW CONTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3558, 219, 'AP SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3559, 219, 'AY MCDONALD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3560, 219, 'CLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3561, 219, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3562, 219, 'DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3563, 219, 'EDDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3564, 219, 'FORD METER BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3565, 219, 'IOWA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3566, 219, 'KENNEDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3567, 219, 'LUDLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3568, 219, 'M-H/MCWANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3569, 219, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3570, 219, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3571, 219, 'RENSSELAER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3572, 219, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3573, 219, 'US PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3574, 219, 'WATEROUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4189, 219, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BLOWER

                case "BLOWER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2005, 125, 'AMERICAN FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2006, 125, 'AMETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2007, 125, 'CINCINNATI FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2008, 125, 'FANTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2009, 125, 'GARDNER DENVER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2010, 125, 'GAST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2011, 125, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2012, 125, 'NEW YORK BLOWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2013, 125, 'ROOTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2014, 125, 'SEMBLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2015, 125, 'SPENCER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2016, 125, 'SULLAIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2017, 125, 'TROY-BILT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2018, 125, 'TWIN CITY FAN-BLOWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2019, 125, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3965, 125, 'zenon', 'zenon');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4097, 125, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BOILER

                case "BOILER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2020, 126, 'BURNHAM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2021, 126, 'CLEAVER-BROOKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2022, 126, 'HARSCO-PATTERSON-KELLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2023, 126, 'HEAT TRANSFER PRODUCTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2024, 126, 'KEWAUNEE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2025, 126, 'LOCHINVAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2026, 126, 'MESTEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2027, 126, 'PEERLESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2028, 126, 'RAYPAK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2029, 126, 'REZNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2030, 126, 'RUUD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2031, 126, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2032, 126, 'WEIL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3942, 126, 'HB SMITH CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4098, 126, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region BURNER

                case "BURNER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2033, 127, 'LENNOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2034, 127, 'MODINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2035, 127, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4099, 127, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CALIBRATION DEVICE

                case "CALIBRATION DEVICE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2036, 128, 'FLUKE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2037, 128, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4100, 128, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CATHODIC PROTECTION

                case "CATHODIC PROTECTION":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2038, 129, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4101, 129, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL DRY FEEDER

                case "CHEMICAL DRY FEEDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2055, 132, 'ALLIED COLLOIDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2056, 132, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2057, 132, 'CHEMCO SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2058, 132, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2059, 132, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2060, 132, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2061, 132, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2062, 132, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2063, 132, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3966, 132, 'WALLACE', 'Wallace');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4104, 132, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL GAS FEEDER

                case "CHEMICAL GAS FEEDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2064, 133, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2065, 133, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2066, 133, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2067, 133, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2068, 133, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2069, 133, 'SUPERIOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2070, 133, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2071, 133, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2072, 133, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4105, 133, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL GENERATORS

                case "CHEMICAL GENERATORS":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2039, 130, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2040, 130, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2041, 130, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2042, 130, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2043, 130, 'WATSON-MARLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3900, 130, 'JOHN DEERE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3901, 130, 'KOHLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3902, 130, 'DETROIT DIESEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3903, 130, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3941, 130, 'Onan', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4102, 130, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL LIQUID FEEDER

                case "CHEMICAL LIQUID FEEDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2073, 134, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2074, 134, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2075, 134, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2076, 134, 'MASTERFLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2077, 134, 'PULSATRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2078, 134, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2079, 134, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2080, 134, 'STENNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2081, 134, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2082, 134, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2083, 134, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2084, 134, 'WATSON-MARLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4106, 134, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL PIPING

                case "CHEMICAL PIPING":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2044, 131, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2045, 131, 'CHEMCO SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2046, 131, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2047, 131, 'MASTERFLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2048, 131, 'PULSATRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2049, 131, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2050, 131, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2051, 131, 'STENNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2052, 131, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2053, 131, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2054, 131, 'WATSON-MARLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4103, 131, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CHEMICAL TANK

                case "CHEMICAL TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3575, 220, 'ADAMSON TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3576, 220, 'ASSMANN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3577, 220, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3578, 220, 'CHEM-TAINER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3579, 220, 'CROWN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3580, 220, 'EDWARDS FIBERGLASS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3581, 220, 'HIGHLAND TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3582, 220, 'JONES HUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3583, 220, 'JUSTIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3584, 220, 'KINETICO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3585, 220, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3586, 220, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3587, 220, 'NALGENE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3588, 220, 'OMEGA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3589, 220, 'PDM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3590, 220, 'PLAS-TANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3591, 220, 'POLYPROCESSING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3592, 220, 'PROMINENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3593, 220, 'RAVEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3594, 220, 'SNYDER IND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3595, 220, 'TERRACON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3596, 220, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3597, 220, 'WARNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3598, 220, 'XERXES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3873, 220, 'AUGUSTA FIBERGLASS CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3958, 220, 'JUSTIN TANKS LLC', 'JUSTIN TANKS LLC');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4063, 220, 'Houston Poly Tanks', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4190, 220, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CLARIFIER

                case "CLARIFIER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3693, 229, 'ENVIREX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3694, 229, 'SEW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3695, 229, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3967, 229, 'Hayward', 'Hayward');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4199, 229, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CLEAN OUT

                case "CLEAN OUT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2144, 137, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4109, 137, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region COLLECTION SYSTEM GENERAL

                case "COLLECTION SYSTEM GENERAL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2145, 138, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4110, 138, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CONTROL PANEL

                case "CONTROL PANEL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2085, 135, 'ACCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2086, 135, 'ACS DESIGN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2087, 135, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2088, 135, 'ALLIED COLLOIDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2089, 135, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2090, 135, 'BENSHAW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2091, 135, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2092, 135, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2093, 135, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2094, 135, 'CARGOCAIRE/MUNTERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2095, 135, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2096, 135, 'CLA-VAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2097, 135, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2098, 135, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2099, 135, 'FIRETROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2100, 135, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2101, 135, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2102, 135, 'FRANKLIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2103, 135, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2104, 135, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2105, 135, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2106, 135, 'GOLDEN ANDERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2107, 135, 'GORMAN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2108, 135, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2109, 135, 'HALOGEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2110, 135, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2111, 135, 'HYDROMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2112, 135, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2113, 135, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2114, 135, 'LAMSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2115, 135, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2116, 135, 'MILTON ROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2117, 135, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2118, 135, 'PACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2119, 135, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2120, 135, 'REVERE CONTROL SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2121, 135, 'RUSS ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2122, 135, 'SAFETRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2123, 135, 'SENTINEL-CALGON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2124, 135, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2125, 135, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2126, 135, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2127, 135, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2128, 135, 'TIME MARK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2129, 135, 'TITAN CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2130, 135, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2131, 135, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2132, 135, 'WARRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2133, 135, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2134, 135, 'WILO-DAVIS-EMU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3872, 135, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4107, 135, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CONTROLLER

                case "CONTROLLER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2135, 136, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2136, 136, 'AQUATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2137, 136, 'AUTOMATION DIRECT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2138, 136, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2139, 136, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2140, 136, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2141, 136, 'QEI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2142, 136, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2143, 136, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3975, 136, 'Allied Control', 'Allied Control');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4108, 136, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region CONVEYOR

                case "CONVEYOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2229, 148, 'OLIVER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2230, 148, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4120, 148, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region COOLING TOWER

                case "COOLING TOWER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2529, 172, 'REZNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2530, 172, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4144, 172, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region DAM

                case "DAM":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2231, 149, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4121, 149, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region DEFIBRILLATOR

                case "DEFIBRILLATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1957, 122, 'ADT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1958, 122, 'CARDIAC SCIENCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (1959, 122, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3943, 122, 'Defibtech', 'Defibtech');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4094, 122, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region DISTRIBUTION SYSTEM

                case "DISTRIBUTION SYSTEM":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2232, 150, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4122, 150, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region DISTRIBUTION TOOL

                case "DISTRIBUTION TOOL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2233, 151, 'FISHER SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2234, 151, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2235, 151, 'VIVAX-METROTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4123, 151, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region ELEVATOR

                case "ELEVATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2236, 152, 'DOVER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2237, 152, 'OTIS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2238, 152, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4124, 152, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region EMERGENCY GENERATOR

                case "EMERGENCY GENERATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2379, 163, 'ALPS TECHNOLOGY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2380, 163, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2381, 163, 'BRIGGS-STRATTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2382, 163, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2383, 163, 'CHEVROLET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2384, 163, 'COPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2385, 163, 'CORNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2386, 163, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2387, 163, 'DELCO-REMY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2388, 163, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2389, 163, 'ENERGY DYNAMICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2390, 163, 'FORD MOTOR COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2391, 163, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2392, 163, 'GENERAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2393, 163, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2394, 163, 'HOMELITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2395, 163, 'HONDA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2396, 163, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2397, 163, 'INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2398, 163, 'JOHN DEERE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2399, 163, 'KATOLIGHT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2400, 163, 'KOHLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2401, 163, 'KUBOTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2402, 163, 'MAGNAPLUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2403, 163, 'MARATHON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2404, 163, 'MAXI POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2405, 163, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2406, 163, 'OLYMPLAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2407, 163, 'ONAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2408, 163, 'PERKINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2409, 163, 'POWERGUARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2410, 163, 'SAMLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2411, 163, 'STAMFORD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2412, 163, 'TECUMSEH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2413, 163, 'TORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2414, 163, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2415, 163, 'WAUKESHA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2416, 163, 'WINCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2417, 163, 'WISCONSIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3855, 163, 'DELCO AC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3856, 163, 'RUDOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3857, 163, 'CONSOLIDATED', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3858, 163, 'MECON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3904, 163, 'CUMMINS POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3905, 163, 'DETROIT DIESEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4135, 163, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region EMERGENCY LIGHT

                case "EMERGENCY LIGHT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2239, 153, 'BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2240, 153, 'DUALITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2241, 153, 'EMERGI-LITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2242, 153, 'EXIDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2243, 153, 'ISOLITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2244, 153, 'LIGHTALARMS ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2245, 153, 'LITHONIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2246, 153, 'PHILIPS-MCPHILBEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2247, 153, 'SURE-LITES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2248, 153, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4125, 153, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region ENGINE

                case "ENGINE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2249, 154, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2250, 154, 'BRIGGS-STRATTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2251, 154, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2252, 154, 'CHEVROLET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2253, 154, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2254, 154, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2255, 154, 'ENERGY DYNAMICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2256, 154, 'FORD MOTOR COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2257, 154, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2258, 154, 'GENERAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2259, 154, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2260, 154, 'HOMELITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2261, 154, 'HONDA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2262, 154, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2263, 154, 'INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2264, 154, 'JOHN DEERE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2265, 154, 'KATOLIGHT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2266, 154, 'KOHLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2267, 154, 'KUBOTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2268, 154, 'MAXI POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2269, 154, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2270, 154, 'OLYMPLAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2271, 154, 'ONAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2272, 154, 'PERKINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2273, 154, 'TECUMSEH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2274, 154, 'TORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2275, 154, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2276, 154, 'WAUKESHA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2277, 154, 'WHITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2278, 154, 'WINCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2279, 154, 'WISCONSIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3859, 154, 'DETROIT DIESEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3860, 154, 'FORD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3861, 154, 'GM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3862, 154, 'VOLVO PENTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3976, 154, 'MTU', 'MTU');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4126, 154, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region EYEWASH

                case "EYEWASH":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2280, 155, 'BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2281, 155, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2282, 155, 'ENCON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2283, 155, 'FENDALL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2284, 155, 'FEUDALL CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2285, 155, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2286, 155, 'HAWS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2287, 155, 'NORTH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2288, 155, 'SCIENCEWARE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2289, 155, 'SPEAKMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2290, 155, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4127, 155, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FACILITY AND GROUNDS

                case "FACILITY AND GROUNDS":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2291, 156, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2292, 156, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2293, 156, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3933, 156, 'CALDWELL TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4128, 156, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FALL PROTECTION

                case "FALL PROTECTION":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3076, 194, 'MILLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3077, 194, 'MSA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3078, 194, 'SALISBURY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3079, 194, 'TRACTEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3080, 194, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4164, 194, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FILTER

                case "FILTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3697, 231, 'ASHBROOK SIMON HARTLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3698, 231, 'LEOPOLD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3699, 231, 'NETZSCH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3700, 231, 'ROBERTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3701, 231, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3944, 231, 'CALGON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4065, 231, 'Kaeser', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4201, 231, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FIRE ALARM

                case "FIRE ALARM":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2294, 157, 'ADT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2295, 157, 'AMEREX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2296, 157, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2297, 157, 'GENTEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2298, 157, 'GENTEX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2299, 157, 'KIDDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2300, 157, 'NAPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2301, 157, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2302, 157, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2303, 157, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2304, 157, 'VIKING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4129, 157, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FIRE EXTINGUISHER

                case "FIRE EXTINGUISHER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2305, 158, 'AMEREX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2306, 158, 'ANSUL-SCNTRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2307, 158, 'BADGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2308, 158, 'BROOKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2309, 158, 'BUCKEYE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2310, 158, 'FLAG FIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2311, 158, 'GENERAL FIRE EX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2312, 158, 'GETZ', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2313, 158, 'KIDDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2314, 158, 'PEM ALL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2315, 158, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2316, 158, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4130, 158, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FIRE SUPPRESSION

                case "FIRE SUPPRESSION":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2317, 159, 'ADT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2318, 159, 'GENERAL FIRE EX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2319, 159, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2320, 159, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2321, 159, 'VIKING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4131, 159, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FIREWALL

                case "FIREWALL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2146, 139, 'FORTINET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2147, 139, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2148, 139, 'MULTITECH INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2149, 139, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4111, 139, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FLOATATION DEVICE

                case "FLOATATION DEVICE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3081, 195, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4165, 195, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FLOW METER (NON PREMISE)

                case "FLOW METER (NON PREMISE)":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2322, 160, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2323, 160, 'BADGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2324, 160, 'BEECO-HERSEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2325, 160, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2326, 160, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2327, 160, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2328, 160, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2329, 160, 'DANFOSS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2330, 160, 'DYNASONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2331, 160, 'EASTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2332, 160, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2333, 160, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2334, 160, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2335, 160, 'FLOWMOTION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2336, 160, 'FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2337, 160, 'GREYLINE INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2338, 160, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2339, 160, 'HOFFER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2340, 160, 'INFILCO-DEGREMONT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2341, 160, 'KING INSTRUMENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2342, 160, 'KROHNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2343, 160, 'MARSH MCBIRNEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2344, 160, 'MCCROMETER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2345, 160, 'MICROMOTION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2346, 160, 'NEPTUNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2347, 160, 'NORGAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2348, 160, 'NUSONICS-MESA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2349, 160, 'PFS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2350, 160, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2351, 160, 'PRECISION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2352, 160, 'ROSEMOUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2353, 160, 'SEAMETRICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2354, 160, 'SENSUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2355, 160, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2356, 160, 'SIGMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2357, 160, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2358, 160, 'SPARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2359, 160, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2360, 160, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2361, 160, 'WATER SPECIALTIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3869, 160, 'SWAGELOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3907, 160, 'FLUID COMPONENTS INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3908, 160, 'SIGNATURE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3909, 160, 'UNIVERSAL FLOW MONITOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3910, 160, 'INVENSYS FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3911, 160, 'GREYLINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3912, 160, 'DIETERICH STANDARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3913, 160, 'INVERSYS SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3970, 160, 'hayward', 'hayward');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4070, 160, 'Badger Magneto', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4132, 160, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FLOW WEIR

                case "FLOW WEIR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2362, 161, 'INVENTRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2363, 161, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2364, 161, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4133, 161, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region FUEL TANK

                case "FUEL TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3599, 221, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3600, 221, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3601, 221, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3602, 221, 'HIGHLAND TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3603, 221, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4191, 221, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region GAS DETECTOR

                case "GAS DETECTOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3433, 210, 'ATI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3434, 210, 'BIOSYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3435, 210, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3436, 210, 'DRAGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3437, 210, 'ENMET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3438, 210, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3439, 210, 'IN-USA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3440, 210, 'INDUSTRIAL SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3441, 210, 'MSA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3442, 210, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3443, 210, 'SCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3444, 210, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3445, 210, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3446, 210, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3447, 210, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3448, 210, 'ZELLWEGER ANALYTICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3914, 210, 'MINIMAX XP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3915, 210, 'AFX IN USA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3916, 210, 'ROSEMONT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3939, 210, 'Honeywell BW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3945, 210, 'RKI INSTRUMENTS', 'RKI INSTRUMENTS');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3946, 210, 'WEDECO', 'WEDECO');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3961, 210, 'GAS ALERT MICRO 5', 'GAS ALERT MICRO 5');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4181, 210, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region GEARBOX

                case "GEARBOX":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2365, 162, 'BONFIGLIOLI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2366, 162, 'DELROYD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2367, 162, 'DODGE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2368, 162, 'ENVIREX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2369, 162, 'FALK REXNORD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2370, 162, 'FMC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2371, 162, 'LIGHTNIN-SPX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2372, 162, 'LINK-BELT REX NORD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2373, 162, 'NORD GEAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2374, 162, 'PHILADELPHIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2375, 162, 'RELIANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2376, 162, 'SEW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2377, 162, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2378, 162, 'WINFIELD H. SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3977, 162, 'SEW Eurodrive', 'SEW Eurodrive');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4071, 162, 'Auger', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4134, 162, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region GRAVITY SEWER MAIN

                case "GRAVITY SEWER MAIN":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2418, 164, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4136, 164, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region GRINDER

                case "GRINDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2419, 165, 'JWC ENVIRONMENTAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2420, 165, 'MOYNO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2421, 165, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2422, 165, 'SEW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2423, 165, 'TPI CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2424, 165, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4137, 165, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HEAT EXCHANGER

                case "HEAT EXCHANGER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2485, 170, 'BELL-GOSSETT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2486, 170, 'TRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2487, 170, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3947, 170, 'TRANTER', 'TRANTER');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4142, 170, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HOIST

                case "HOIST":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2425, 166, 'ACCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2426, 166, 'AUTOCRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2427, 166, 'BUDGIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2428, 166, 'CHESTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2429, 166, 'COFFING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2430, 166, 'COLUMBUS MCKINNON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2431, 166, 'DAVID ROUND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2432, 166, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2433, 166, 'HARRINGTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2434, 166, 'KONECRANES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2435, 166, 'LIFTMOORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2436, 166, 'MANNING-MAXWELL-MOORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2437, 166, 'MILLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2438, 166, 'R-M MATERIAL HANDLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2439, 166, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2440, 166, 'WRIGHT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2441, 166, 'YALE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3948, 166, 'CM MOTORIZED', 'CM MOTORIZED');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3949, 166, 'TROLLEY', 'TROLLEY');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3950, 166, 'WALLACE', 'WALLACE');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3951, 166, 'MOTIVATION', 'MOTIVATION');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4138, 166, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HVAC CHILLER

                case "HVAC CHILLER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2442, 167, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2443, 167, 'BARD MFG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2444, 167, 'CARRIER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2445, 167, 'DOMETIC-DUOTHERM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2446, 167, 'FASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2447, 167, 'FRIGIDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2448, 167, 'LENNOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2449, 167, 'LIEBERT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2450, 167, 'TRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2451, 167, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4139, 167, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HVAC COMBINATION UNIT

                case "HVAC COMBINATION UNIT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2452, 168, 'AMANA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2453, 168, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2454, 168, 'BARD MFG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2455, 168, 'CARRIER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2456, 168, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2457, 168, 'FRIGIDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2458, 168, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2459, 168, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2460, 168, 'LENNOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2461, 168, 'LIEBERT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2462, 168, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2463, 168, 'REZNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2464, 168, 'TRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2465, 168, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2466, 168, 'YORK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4140, 168, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HVAC DEHUMIDIFIER

                case "HVAC DEHUMIDIFIER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2467, 169, 'AIR TECHNOLOGY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2468, 169, 'AMANA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2469, 169, 'BRY-AIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2470, 169, 'CARGOCAIRE/MUNTERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2471, 169, 'CROSLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2472, 169, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2473, 169, 'DESERT-AIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2474, 169, 'DRYOMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2475, 169, 'FANTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2476, 169, 'FRIGIDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2477, 169, 'HANKINSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2478, 169, 'KENMORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2479, 169, 'LIEBERT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2480, 169, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2481, 169, 'OASIS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2482, 169, 'PENN VENTILATION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2483, 169, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2484, 169, 'WHIRLPOOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4141, 169, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HVAC HEATER

                case "HVAC HEATER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2488, 171, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2489, 171, 'BARD MFG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2490, 171, 'BEACON-MORRIS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2491, 171, 'BURNHAM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2492, 171, 'CADET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2493, 171, 'CARRIER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2494, 171, 'CHROMALOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2495, 171, 'COMFORT GLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2496, 171, 'COOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2497, 171, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2498, 171, 'DRAVO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2499, 171, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2500, 171, 'EMPIRE COMFORT SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2501, 171, 'ENERCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2502, 171, 'FEDERAL PACIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2503, 171, 'FOSTORIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2504, 171, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2505, 171, 'HEAT-FLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2506, 171, 'INDEECO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2507, 171, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2508, 171, 'JANITROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2509, 171, 'LENNOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2510, 171, 'MARLEY ELEC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2511, 171, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2512, 171, 'MODINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2513, 171, 'NALCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2514, 171, 'NESBITTAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2515, 171, 'PEERLESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2516, 171, 'PENN VENTILATION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2517, 171, 'POWERHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2518, 171, 'QMARK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2519, 171, 'REZNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2520, 171, 'SINGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2521, 171, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2522, 171, 'STERLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2523, 171, 'TEMPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2524, 171, 'TPI CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2525, 171, 'TRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2526, 171, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2527, 171, 'VANGUARD HEATERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2528, 171, 'XERXES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4143, 171, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HVAC VENTILATOR

                case "HVAC VENTILATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2531, 173, 'ACME', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2532, 173, 'ADVANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2533, 173, 'AEROVENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2534, 173, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2535, 173, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2536, 173, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2537, 173, 'BARBER COLMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2538, 173, 'BARD MFG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2539, 173, 'BELIMO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2540, 173, 'BROAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2541, 173, 'CARRIER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2542, 173, 'CENTRAL FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2543, 173, 'CHELSEA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2544, 173, 'COOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2545, 173, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2546, 173, 'DOMEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2547, 173, 'DURALAB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2548, 173, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2549, 173, 'EMPIRE COMFORT SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2550, 173, 'FANTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2551, 173, 'FUMEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2552, 173, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2553, 173, 'GREENHECK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2554, 173, 'GREENTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2555, 173, 'HARTZELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2556, 173, 'HEMCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2557, 173, 'ILG INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2558, 173, 'KANALFLAKT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2559, 173, 'KEWAUNEE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2560, 173, 'KEWAUNEE SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2561, 173, 'LABCONOCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2562, 173, 'LASKO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2563, 173, 'LEADER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2564, 173, 'OMEGA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2565, 173, 'PENN FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2566, 173, 'PENN VENTILATION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2567, 173, 'TRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2568, 173, 'TSI-ALNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2569, 173, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2570, 173, 'ZEPHYR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4061, 173, 'Gemu', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4145, 173, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region HYDRANT

                case "HYDRANT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2589, 175, 'ABS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2590, 175, 'AMERICAN CAST IRON PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2591, 175, 'AMERICAN DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2592, 175, 'AMERICAN FLOW CONTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2593, 175, 'AMERICAN FOUNDRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2594, 175, 'AP SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2595, 175, 'CHAPMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2596, 175, 'CLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2597, 175, 'COREY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2598, 175, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2599, 175, 'DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2600, 175, 'DRESSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2601, 175, 'EAST JORDAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2602, 175, 'ECLIPSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2603, 175, 'EDDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2604, 175, 'FRANKLIN HYDRANTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2605, 175, 'GREENBERG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2606, 175, 'IOWA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2607, 175, 'JONES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2608, 175, 'KENMORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2609, 175, 'KENNEDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2610, 175, 'KUPFERLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2611, 175, 'LONG BEACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2612, 175, 'LUDLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2613, 175, 'M-H/MCWANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2614, 175, 'MATHEWS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2615, 175, 'MICHIGAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2616, 175, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2617, 175, 'PACIFIC STATES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2618, 175, 'PENN-TROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2619, 175, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2620, 175, 'PRATT-CADY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2621, 175, 'RD WOOD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2622, 175, 'RENSSELAER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2623, 175, 'RICH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2624, 175, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2625, 175, 'TCIW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2626, 175, 'TRAVERSE CITY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2627, 175, 'U.S. FOUNDRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2628, 175, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2629, 175, 'US PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2630, 175, 'WATEROUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2631, 175, 'WESTCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3882, 175, 'WHARF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4147, 175, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region INDICATOR

                case "INDICATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2632, 176, 'ACTION INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2633, 176, 'ASHCROFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2634, 176, 'BANNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2635, 176, 'BINMASTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2636, 176, 'BKZ INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2637, 176, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2638, 176, 'CAPITAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2639, 176, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2640, 176, 'DEVAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2641, 176, 'DRAGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2642, 176, 'DREXELBROOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2643, 176, 'DWYER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2644, 176, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2645, 176, 'FLOW LINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2646, 176, 'FLOWSERVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2647, 176, 'FORCE FLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2648, 176, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2649, 176, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2650, 176, 'INVENTRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2651, 176, 'KESSLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2652, 176, 'MAGNATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2653, 176, 'MERCOID', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2654, 176, 'MILLTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2655, 176, 'MKS INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2656, 176, 'PRECISION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2657, 176, 'RED LION CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2658, 176, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2659, 176, 'ROSEMOUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2660, 176, 'SCHNEIDER ELECTRIC-APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2661, 176, 'SENSUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2662, 176, 'SIGMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2663, 176, 'SIMPSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2664, 176, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2665, 176, 'VORNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2666, 176, 'WIKA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2667, 176, 'WIZARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4148, 176, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region INSTRUMENT SWITCH

                case "INSTRUMENT SWITCH":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2668, 177, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2669, 177, 'ANCHOR SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2670, 177, 'ANDERSON INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2671, 177, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2672, 177, 'ASHCROFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2673, 177, 'BARKSDALE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2674, 177, 'BINDICATOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2675, 177, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2676, 177, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2677, 177, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2678, 177, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2679, 177, 'DREXELBROOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2680, 177, 'DWYER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2681, 177, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2682, 177, 'FLO-TECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2683, 177, 'FLOW LINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2684, 177, 'FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2685, 177, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2686, 177, 'GEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2687, 177, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2688, 177, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2689, 177, 'INTERMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2690, 177, 'JOHNSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2691, 177, 'JOHNSON CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2692, 177, 'KOBOLD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2693, 177, 'LEED-NORTHRUP CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2694, 177, 'MAGNATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2695, 177, 'MAGNETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2696, 177, 'MCCROMETER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2697, 177, 'MCDONNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2698, 177, 'MERCOID', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2699, 177, 'MH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2700, 177, 'MILLTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2701, 177, 'MSA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2702, 177, 'OLIVER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2703, 177, 'OMRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2704, 177, 'PEECO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2705, 177, 'POTTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2706, 177, 'ROSEMOUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2707, 177, 'SEAMETRICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2708, 177, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2709, 177, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2710, 177, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2711, 177, 'VERDER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2712, 177, 'VICTAULIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2713, 177, 'WARRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2714, 177, 'WIKA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3890, 177, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4055, 177, 'RDP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4149, 177, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region KIT (SAFETY, REPAIR, HAZWOPR)

                case "KIT (SAFETY, REPAIR, HAZWOPR)":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2715, 178, 'ACME', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2716, 178, 'CINTAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2717, 178, 'MILLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2718, 178, 'MSA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2719, 178, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2720, 178, 'WATTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2721, 178, 'ZEE MEDICAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3917, 178, 'NEW PIG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4150, 178, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region LAB EQUIPMENT

                case "LAB EQUIPMENT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2722, 179, 'BARNSTEAD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2723, 179, 'BLAK-RAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2724, 179, 'BLUE M', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2725, 179, 'BOEKEL SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2726, 179, 'CORNING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2727, 179, 'FISHER SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2728, 179, 'FRIGIDAIRE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2729, 179, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2730, 179, 'GRIEVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2731, 179, 'H-B INSTRUMENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2732, 179, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2733, 179, 'HUMBOLDT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2734, 179, 'IDEXX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2735, 179, 'KENMORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2736, 179, 'KESSLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2737, 179, 'LABCONOCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2738, 179, 'LEICA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2739, 179, 'MAGIC CHEF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2740, 179, 'MARKET FORGE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2741, 179, 'MARVEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2742, 179, 'METROHM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2743, 179, 'MILLIPORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2744, 179, 'NAPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2745, 179, 'OHAUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2746, 179, 'OLYMPUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2747, 179, 'PHILPS-BIRD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2748, 179, 'PRECISION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2749, 179, 'QUINCY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2750, 179, 'REICHART', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2751, 179, 'SHELDON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2752, 179, 'TELEDYNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2753, 179, 'THERMO-ORION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2754, 179, 'TUTTNAUER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2755, 179, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2756, 179, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2757, 179, 'UVP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2758, 179, 'VWR SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2759, 179, 'WHIRLPOOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2760, 179, 'WINDMERE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2761, 179, 'YAMATO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4151, 179, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region LEAK MONITOR

                case "LEAK MONITOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3874, 180, 'UNKNOWN', NULL);
";

                #endregion

                #region MANHOLE

                case "MANHOLE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2762, 181, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4152, 181, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region MIXER

                case "MIXER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2763, 182, 'ABS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2764, 182, 'CHEMINEER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2765, 182, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2766, 182, 'FMC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2767, 182, 'JMS-JIM MYERS EQUIP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2768, 182, 'KOFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2769, 182, 'KOMAX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2770, 182, 'LANDIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2771, 182, 'LEESON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2772, 182, 'LIGHTNIN-SPX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2773, 182, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2774, 182, 'NEPTUNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2775, 182, 'PHILADELPHIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2776, 182, 'SEW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2777, 182, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2778, 182, 'STRANCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2779, 182, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2780, 182, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2781, 182, 'WINFIELD H. SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4060, 182, 'Cleveland', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4153, 182, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region MODEM

                case "MODEM":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2150, 140, 'B-B ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2151, 140, 'BLACK BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2152, 140, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2153, 140, 'CISCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2154, 140, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2155, 140, 'MOTOROLA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2156, 140, 'RAYLAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2157, 140, 'SIERRA WIRELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2158, 140, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4112, 140, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region MOTOR

                case "MOTOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2782, 183, 'ABS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2783, 183, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2784, 183, 'ALLIS CHAMBERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2785, 183, 'AMERICAN FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2786, 183, 'AMETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2787, 183, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2788, 183, 'ARMSTRONG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2789, 183, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2790, 183, 'BELL-GOSSETT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2791, 183, 'CENTURY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2792, 183, 'CONTINENTAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2793, 183, 'CORNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2794, 183, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2795, 183, 'EBARA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2796, 183, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2797, 183, 'FAIRBANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2798, 183, 'FLEX PRO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2799, 183, 'FLOWSERVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2800, 183, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2801, 183, 'FRANKLIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2802, 183, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2803, 183, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2804, 183, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2805, 183, 'GENERAL DYNAMICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2806, 183, 'GORMAN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2807, 183, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2808, 183, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2809, 183, 'HITACHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2810, 183, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2811, 183, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2812, 183, 'KOLLMORGEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2813, 183, 'LEESON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2814, 183, 'LIBERTY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2815, 183, 'LIGHTNIN-SPX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2816, 183, 'LINCOLN MOTORS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2817, 183, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2818, 183, 'LOUIS ALLIS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2819, 183, 'MAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2820, 183, 'MAGNETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2821, 183, 'MARATHON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2822, 183, 'MIDWEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2823, 183, 'MONOFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2824, 183, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2825, 183, 'NORD GEAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2826, 183, 'ORIENTAL MOTOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2827, 183, 'PACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2828, 183, 'PEABODY ENG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2829, 183, 'PEERLESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2830, 183, 'RELIANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2831, 183, 'REX MANUFACTURING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2832, 183, 'ROBBINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2833, 183, 'SEW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2834, 183, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2835, 183, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2836, 183, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2837, 183, 'SPENCER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2838, 183, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2839, 183, 'STA-RITE-BERKELEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2840, 183, 'STENNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2841, 183, 'STERLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2842, 183, 'TECO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2843, 183, 'TOSHIBA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2844, 183, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2845, 183, 'US ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2846, 183, 'US MOTORS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2847, 183, 'US SELECT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2848, 183, 'WAGNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2849, 183, 'WARNER ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2850, 183, 'WEG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2851, 183, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2852, 183, 'WILO-DAVIS-EMU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2853, 183, 'ZOELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3867, 183, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3868, 183, 'PMSL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4154, 183, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region MOTOR CONTACTOR

                case "MOTOR CONTACTOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2210, 146, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2211, 146, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2212, 146, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2213, 146, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2214, 146, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2215, 146, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2216, 146, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2217, 146, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2218, 146, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2219, 146, 'SPRECHER-SCHUH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2220, 146, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2221, 146, 'TELEMECANIQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2222, 146, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2223, 146, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4118, 146, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region MOTOR STARTER

                case "MOTOR STARTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2854, 184, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2855, 184, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2856, 184, 'ALLIS CHAMBERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2857, 184, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2858, 184, 'BALDOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2859, 184, 'BENSHAW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2860, 184, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2861, 184, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2862, 184, 'DAYNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2863, 184, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2864, 184, 'FIRETROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2865, 184, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2866, 184, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2867, 184, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2868, 184, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2869, 184, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2870, 184, 'LYNN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2871, 184, 'MARATHON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2872, 184, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2873, 184, 'MOELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2874, 184, 'ROBICON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2875, 184, 'RONK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2876, 184, 'SAFETRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2877, 184, 'SCHNEIDER ELECTRIC-APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2878, 184, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2879, 184, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2880, 184, 'TELEMECANIQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2881, 184, 'TOSHIBA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2882, 184, 'TPI CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2883, 184, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2884, 184, 'US DRIVES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2885, 184, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2886, 184, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4056, 184, 'RDP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4058, 184, 'Vacon', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4155, 184, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region NETWORK ROUTER

                case "NETWORK ROUTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2168, 142, 'BLACK BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2169, 142, 'CISCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2170, 142, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2171, 142, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3918, 142, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4114, 142, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region NETWORK SWITCH

                case "NETWORK SWITCH":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2172, 143, 'BLACK BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2173, 143, 'CISCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2174, 143, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2175, 143, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3880, 143, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3881, 143, 'HIRSCHMANN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4115, 143, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region NON POTABLE WATER TANK

                case "NON POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3619, 223, 'BELL-GOSSETT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3620, 223, 'CALDWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3621, 223, 'CHICAGO BRIDGE-IRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3622, 223, 'POLYPROCESSING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3623, 223, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3624, 223, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4193, 223, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region OPERATOR COMPUTER TERMINAL

                case "OPERATOR COMPUTER TERMINAL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2887, 186, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2888, 186, 'DELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2889, 186, 'EZAUTOMATION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2890, 186, 'HP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2891, 186, 'RED LION CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2892, 186, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4156, 186, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PC

                case "PC":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2893, 187, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2894, 187, 'DELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2895, 187, 'DYNICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2896, 187, 'HP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2897, 187, 'IBM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2898, 187, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4157, 187, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PDM TOOL

                case "PDM TOOL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2899, 188, 'BAKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2900, 188, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4158, 188, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PHASE CONVERTER

                case "PHASE CONVERTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2901, 189, 'RONK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2902, 189, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4159, 189, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PLANT VALVE

                case "PLANT VALVE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3094, 199, 'A-T CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3095, 199, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3096, 199, 'ALLIS CHAMBERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3097, 199, 'AMERICAN CONE VALVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3098, 199, 'AMERICAN DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3099, 199, 'AMERICAN FLOW CONTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3100, 199, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3101, 199, 'AMES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3102, 199, 'AP SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3103, 199, 'APCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3104, 199, 'APOLLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3105, 199, 'AQUATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3106, 199, 'ASAHI/AMERICA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3107, 199, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3108, 199, 'ASHCROFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3109, 199, 'AUMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3110, 199, 'AVK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3111, 199, 'BAILEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3112, 199, 'BECK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3113, 199, 'BEECO-HERSEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3114, 199, 'BERMAD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3115, 199, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3116, 199, 'BLACOH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3117, 199, 'BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3118, 199, 'BRAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3119, 199, 'CAMERON-DEMCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3120, 199, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3121, 199, 'CENTERLINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3122, 199, 'CHAPMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3123, 199, 'CLA-VAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3124, 199, 'CLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3125, 199, 'CONBRACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3126, 199, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3127, 199, 'CRISPIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3128, 199, 'DANFOSS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3129, 199, 'DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3130, 199, 'DEZURICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3131, 199, 'DRESSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3132, 199, 'EDDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3133, 199, 'EIM COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3134, 199, 'FAIRBANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3135, 199, 'FEBCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3136, 199, 'FLOMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3137, 199, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3138, 199, 'FMC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3139, 199, 'FNW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3140, 199, 'FONTAINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3141, 199, 'FORD METER BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3142, 199, 'GF PIPING SYSTEMS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3143, 199, 'GOLDEN ANDERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3144, 199, 'GRIFFCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3145, 199, 'GRISWOLD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3146, 199, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3147, 199, 'HALOGEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3148, 199, 'HAMMOND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3149, 199, 'HAYWARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3150, 199, 'HILLS-MCCANNA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3151, 199, 'INLINE INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3152, 199, 'ITT FABRI-VALVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3153, 199, 'KATES-CUSTOM VLV CONCEPT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3154, 199, 'KECKLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3155, 199, 'KENNEDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3156, 199, 'KENSEAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3157, 199, 'KEYSTONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3158, 199, 'KUNKLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3159, 199, 'LEONARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3160, 199, 'LIMITORQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3161, 199, 'LUDLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3162, 199, 'M-H/MCWANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3163, 199, 'MAGNATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3164, 199, 'MATCO NORCA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3165, 199, 'MAX-AIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3166, 199, 'MCDONALD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3167, 199, 'MCDONNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3168, 199, 'METRAFLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3169, 199, 'METSO-JAMESBURY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3170, 199, 'MH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3171, 199, 'MHV-F', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3172, 199, 'MILLIKEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3173, 199, 'MILTON ROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3174, 199, 'MILWAUKEE VALVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3175, 199, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3176, 199, 'NEPTUNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3177, 199, 'NIBCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3178, 199, 'NIL-COR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3179, 199, 'ORBINOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3180, 199, 'PENN-TROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3181, 199, 'PENTAIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3182, 199, 'PLAST-O-MATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3183, 199, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3184, 199, 'RED FLAG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3185, 199, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3186, 199, 'RENSSELAER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3187, 199, 'ROCKWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3188, 199, 'RODNEY HUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3189, 199, 'ROLL SEAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3190, 199, 'ROSS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3191, 199, 'SENSUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3192, 199, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3193, 199, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3194, 199, 'SINGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3195, 199, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3196, 199, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3197, 199, 'SPEARS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3198, 199, 'STOCKHAM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3199, 199, 'SUREFLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3200, 199, 'TCIW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3201, 199, 'TECHNO VALVES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3202, 199, 'TOYO VALVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3203, 199, 'TRIAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3204, 199, 'ULTRAFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3205, 199, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3206, 199, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3207, 199, 'US PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3208, 199, 'VAL-MATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3209, 199, 'VALVE AND PRIMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3210, 199, 'VICTAULIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3211, 199, 'VIKING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3212, 199, 'WACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3213, 199, 'WADSWORTH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3214, 199, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3215, 199, 'WATERMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3216, 199, 'WATEROUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3217, 199, 'WATSON-MCDANIEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3218, 199, 'WATTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3219, 199, 'WESTCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3220, 199, 'WHIPPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3221, 199, 'WILKINS-ZURN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3222, 199, 'WILLAMETTE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3891, 199, 'DEZURIK/EIM CO. INC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3892, 199, 'EIM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3936, 199, 'DEZURIK/EIM CO. INC.', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3938, 199, 'TEAM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3955, 199, 'GUREX', 'GUREX');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4062, 199, 'Gemu', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4064, 199, 'Kaeser', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4169, 199, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POTABLE WATER TANK

                case "POTABLE WATER TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3625, 224, 'ADAMSON TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3626, 224, 'ADVANCE TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3627, 224, 'AMTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3628, 224, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3629, 224, 'AQUASTORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3630, 224, 'BRUNNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3631, 224, 'BUFFALO TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3632, 224, 'CALDWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3633, 224, 'CHAMPION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3634, 224, 'CHICAGO BRIDGE-IRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3635, 224, 'FISHER TANK CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3636, 224, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3637, 224, 'GRAVER TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3638, 224, 'HIGHLAND TANK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3639, 224, 'JOHN WOOD CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3640, 224, 'NATGUN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3641, 224, 'NOOTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3642, 224, 'PDM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3643, 224, 'PEABODY ENG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3644, 224, 'RECO USA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3645, 224, 'RHEEM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3646, 224, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3647, 224, 'WESSELS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4194, 224, 'UNKNOWN', 'OTHER');
";


                #endregion

                #region POWER BREAKER

                case "POWER BREAKER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3223, 200, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3224, 200, 'ACME', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3225, 200, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3226, 200, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3227, 200, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3228, 200, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3229, 200, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3230, 200, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3231, 200, 'ENVIRONMENT ONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3232, 200, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3233, 200, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3234, 200, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3235, 200, 'HAMMOND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3236, 200, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3237, 200, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3238, 200, 'MIDWEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3239, 200, 'MURRAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3240, 200, 'ONAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3241, 200, 'RUSS ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3242, 200, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3243, 200, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3244, 200, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3245, 200, 'TELEMECANIQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3246, 200, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3247, 200, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4170, 200, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER CONDITIONER

                case "POWER CONDITIONER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3248, 201, 'AEROVOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3249, 201, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3250, 201, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3251, 201, 'COMMONWEALTH SPRAGUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3252, 201, 'CORNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3253, 201, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3254, 201, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3255, 201, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3256, 201, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3257, 201, 'LAMARCHE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3258, 201, 'MYRON ZUCKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3259, 201, 'RONK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3260, 201, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3261, 201, 'TCI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3262, 201, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3263, 201, 'VERSATEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3264, 201, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4171, 201, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER DISCONNECT

                case "POWER DISCONNECT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3265, 202, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3266, 202, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3267, 202, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3268, 202, 'AMERICAN ELECTRICAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3269, 202, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3270, 202, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3271, 202, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3272, 202, 'CLIPPER POWER SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3273, 202, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3274, 202, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3275, 202, 'FEDERAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3276, 202, 'FEDERAL PACIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3277, 202, 'FRANKLIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3278, 202, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3279, 202, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3280, 202, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3281, 202, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3282, 202, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3283, 202, 'MET ED/GPU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3284, 202, 'MTL INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3285, 202, 'MURRAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3286, 202, 'ROBICON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3287, 202, 'SCHNEIDER ELECTRIC-APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3288, 202, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3289, 202, 'SPANG-MAGNETICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3290, 202, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3291, 202, 'TRUMBULL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3292, 202, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3293, 202, 'WADSWORTH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3294, 202, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4172, 202, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER FEEDER CABLE

                case "POWER FEEDER CABLE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3295, 203, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3296, 203, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3297, 203, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3298, 203, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3299, 203, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3300, 203, 'FEDERAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3301, 203, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3302, 203, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3303, 203, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3304, 203, 'MET ED', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3305, 203, 'MET ED/GPU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3306, 203, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3307, 203, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3308, 203, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3309, 203, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3919, 203, 'RELIANCE ELECTRIC ', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3920, 203, 'MARATHON ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4173, 203, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER MONITOR

                case "POWER MONITOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3310, 204, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3311, 204, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3312, 204, 'APT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3313, 204, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3314, 204, 'CLIPPER POWER SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3315, 204, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3316, 204, 'DIVERSIFIED ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3317, 204, 'E-MON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3318, 204, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3319, 204, 'FRANKLIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3320, 204, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3321, 204, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3322, 204, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3323, 204, 'INNOVATIVE TECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3324, 204, 'INTERMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3325, 204, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3326, 204, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3327, 204, 'SEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3328, 204, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3329, 204, 'SIMPSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3330, 204, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3331, 204, 'SYMCOM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3332, 204, 'TELEMECANIQUE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3333, 204, 'TIME MARK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3334, 204, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3335, 204, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4174, 204, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER PANEL

                case "POWER PANEL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3336, 205, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3337, 205, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3338, 205, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3339, 205, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3340, 205, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3341, 205, 'FEDERAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3342, 205, 'FEDERAL PACIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3343, 205, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3344, 205, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3345, 205, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3346, 205, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3347, 205, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3348, 205, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3349, 205, 'MET ED', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3350, 205, 'MURRAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3351, 205, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3352, 205, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3353, 205, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3354, 205, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4175, 205, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER RELAY

                case "POWER RELAY":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3355, 206, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3356, 206, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3357, 206, 'BASLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3358, 206, 'BECKWITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3359, 206, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3360, 206, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3361, 206, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3362, 206, 'SEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3363, 206, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3364, 206, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4176, 206, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER SURGE PROTECTION

                case "POWER SURGE PROTECTION":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3365, 207, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3366, 207, 'ALANTIC SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3367, 207, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3368, 207, 'APT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3369, 207, 'CLIPPER POWER SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3370, 207, 'COOPER CROUSE-HINDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3371, 207, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3372, 207, 'DELTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3373, 207, 'DITEK-TVSS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3374, 207, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3375, 207, 'FERRAZ SHAWMUT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3376, 207, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3377, 207, 'HOFFMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3378, 207, 'HUBBELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3379, 207, 'INNOVATIVE TECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3380, 207, 'INTERMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3381, 207, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3382, 207, 'LEA INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3383, 207, 'LEVITON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3384, 207, 'MCG SURGE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3385, 207, 'MTL INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3386, 207, 'SCHNEIDER ELECTRIC-APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3387, 207, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3388, 207, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3389, 207, 'SYMCOM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3390, 207, 'TCI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3391, 207, 'TRIPPLITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3392, 207, 'UNITED POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3393, 207, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3883, 207, 'PHOENIX CONTACT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3884, 207, 'SOLA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4177, 207, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region POWER TRANSFER SWITCH

                case "POWER TRANSFER SWITCH":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3669, 227, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3670, 227, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3671, 227, 'ASCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3672, 227, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3673, 227, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3674, 227, 'CUMMINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3675, 227, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3676, 227, 'DETROIT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3677, 227, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3678, 227, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3679, 227, 'GENERAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3680, 227, 'KATOLIGHT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3681, 227, 'KOHLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3682, 227, 'LAKE SHORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3683, 227, 'MIDWEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3684, 227, 'ONAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3685, 227, 'RONK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3686, 227, 'RUSS ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3687, 227, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3688, 227, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3689, 227, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3690, 227, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3921, 227, 'ENERCON ENGINEERING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4197, 227, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PRESSURE DAMPER

                case "PRESSURE DAMPER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3088, 197, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3089, 197, 'WATTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4167, 197, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PRINTER

                case "PRINTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3090, 198, 'CANON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3091, 198, 'HP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3092, 198, 'KODAK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3093, 198, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4168, 198, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PUMP CENTRIFUGAL

                case "PUMP CENTRIFUGAL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2903, 190, 'A-C PUMP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2904, 190, 'ABS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2905, 190, 'ADVANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2906, 190, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2907, 190, 'ALLIS CHAMBERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2908, 190, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2909, 190, 'ARMSTRONG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2910, 190, 'AURORA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2911, 190, 'AY MCDONALD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2912, 190, 'BAKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2913, 190, 'BELL-GOSSETT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2914, 190, 'BERKLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2915, 190, 'BYRON JACKSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2916, 190, 'CARRIER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2917, 190, 'CATERPILLAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2918, 190, 'CHRISTENSEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2919, 190, 'CLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2920, 190, 'CONTINENTAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2921, 190, 'CORNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2922, 190, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2923, 190, 'CRANE BURKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2924, 190, 'CRANE DEMING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2925, 190, 'CROKER WHEELER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2926, 190, 'CROWN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2927, 190, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2928, 190, 'DELAVAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2929, 190, 'DOBBS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2930, 190, 'DURCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2931, 190, 'DURIRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2932, 190, 'EBARA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2933, 190, 'ENVIROTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2934, 190, 'FAIRBANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2935, 190, 'FEDERAL PACIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2936, 190, 'FINISH THOMPSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2937, 190, 'FLOWAY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2938, 190, 'FLOWSERVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2939, 190, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2940, 190, 'FM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2941, 190, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2942, 190, 'G-L', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2943, 190, 'GAST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2944, 190, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2945, 190, 'GORMAN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2946, 190, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2947, 190, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2948, 190, 'HAZLETON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2949, 190, 'HOMELITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2950, 190, 'HONDA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2951, 190, 'HYDROMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2952, 190, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2953, 190, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2954, 190, 'IWAKI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2955, 190, 'JOHNSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2956, 190, 'KSB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2957, 190, 'LAYNE-BOWLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2958, 190, 'LE COURTNEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2959, 190, 'LENNOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2960, 190, 'LIBERTY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2961, 190, 'LITTLE GIANT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2962, 190, 'LUTZ', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2963, 190, 'MARATHON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2964, 190, 'MARCH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2965, 190, 'MILLIPORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2966, 190, 'MILTON ROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2967, 190, 'MONOFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2968, 190, 'MORSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2969, 190, 'MOYNO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2970, 190, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2971, 190, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2972, 190, 'NASH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2973, 190, 'NATIONAL PUMP CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2974, 190, 'PACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2975, 190, 'PATTERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2976, 190, 'PEERLESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2977, 190, 'PENN VALLEY PUMP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2978, 190, 'PRICE PUMP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2979, 190, 'PUMPEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2980, 190, 'RED JACKET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2981, 190, 'SERFILCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2982, 190, 'SIMMONS PUMP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2983, 190, 'SJE-RHOMBUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2984, 190, 'SMITH AND LOVELESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2985, 190, 'SPENCER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2986, 190, 'STA-RITE-BERKELEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2987, 190, 'SULZER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2988, 190, 'TACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2989, 190, 'TEEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2990, 190, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2991, 190, 'US MOTORS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2992, 190, 'WACKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2993, 190, 'WACKER NEUSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2994, 190, 'WEIL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2995, 190, 'WEIR POWER AND INDUSTRIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2996, 190, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2997, 190, 'WILO-DAVIS-EMU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2998, 190, 'WORTHINGTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2999, 190, 'ZOELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3864, 190, 'HIDROSTAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3865, 190, 'WEMCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3893, 190, 'CINCINNATI FAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3894, 190, 'GOULD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3895, 190, 'INGERSOLL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3896, 190, 'WHEELER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3984, 190, 'Netzsch', 'Netzsch');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4057, 190, 'RDP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4059, 190, 'Vacon', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4067, 190, 'Weir', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4068, 190, 'Graco EP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4069, 190, 'Pulsatron', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4072, 190, 'Verder', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4160, 190, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PUMP GRINDER

                case "PUMP GRINDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3000, 191, 'ABS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3001, 191, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3002, 191, 'CRANE DEMING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3003, 191, 'CROWN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3004, 191, 'ENVIRONMENT ONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3005, 191, 'FAIRBANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3006, 191, 'FLYGT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3007, 191, 'FRANKLIN ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3008, 191, 'GORMAN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3009, 191, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3010, 191, 'HYDROMATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3011, 191, 'LIBERTY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3012, 191, 'MONOFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3013, 191, 'MOYNO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3014, 191, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3015, 191, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3016, 191, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3017, 191, 'ZOELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4161, 191, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region PUMP POSITIVE DISPLACEMENT

                case "PUMP POSITIVE DISPLACEMENT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3018, 192, 'ALYAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3019, 192, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3020, 192, 'BLUE-WHITE INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3021, 192, 'CARTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3022, 192, 'CH AND E PUMPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3023, 192, 'COLE-PARMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3024, 192, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3025, 192, 'DURCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3026, 192, 'ENVIRONMENT ONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3027, 192, 'FLEX PRO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3028, 192, 'FLOWMOTION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3029, 192, 'G-L', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3030, 192, 'GAST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3031, 192, 'GOULDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3032, 192, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3033, 192, 'HONDA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3034, 192, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3035, 192, 'ITT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3036, 192, 'IWAKI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3037, 192, 'JAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3038, 192, 'JESCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3039, 192, 'LAKESIDE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3040, 192, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3041, 192, 'M-D PNEUMATICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3042, 192, 'MARATHON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3043, 192, 'MARCH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3044, 192, 'MASTERFLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3045, 192, 'MILLIPORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3046, 192, 'MILROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3047, 192, 'MILTON ROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3048, 192, 'MONOFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3049, 192, 'MOYNO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3050, 192, 'NASH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3051, 192, 'NETZSCH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3052, 192, 'NGC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3053, 192, 'PROMINENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3054, 192, 'PULSAFEEDER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3055, 192, 'PULSAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3056, 192, 'PULSATRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3057, 192, 'ROBBINS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3058, 192, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3059, 192, 'STENNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3060, 192, 'STRANCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3061, 192, 'TEEL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3062, 192, 'THERMO-ORION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3063, 192, 'TUTHILL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3064, 192, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3065, 192, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3066, 192, 'VERDER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3067, 192, 'WACKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3068, 192, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3069, 192, 'WARREN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3070, 192, 'WATSON-MARLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3071, 192, 'WOERNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3897, 192, 'KECO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3898, 192, 'SIEMANS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4162, 192, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region RECORDER

                case "RECORDER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3394, 208, 'AMETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3395, 208, 'BAILEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3396, 208, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3397, 208, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3398, 208, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3399, 208, 'DICKSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3400, 208, 'DWYER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3401, 208, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3402, 208, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3403, 208, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3404, 208, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3405, 208, 'LEED-NORTHRUP CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3406, 208, 'MISSION COMMUNICATIONS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3407, 208, 'PARTLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3408, 208, 'SIGMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3409, 208, 'TELOG', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3410, 208, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4178, 208, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region RESPIRATOR

                case "RESPIRATOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3082, 196, 'DRAGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3083, 196, 'MSA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3084, 196, 'NORTH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3085, 196, 'SCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3086, 196, 'SURVIVAIR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3087, 196, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3922, 196, 'LAKELAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3923, 196, 'DUPONT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4166, 196, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region RTU - PLC

                case "RTU - PLC":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3411, 209, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3412, 209, 'AUTOMATION DIRECT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3413, 209, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3414, 209, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3415, 209, 'CONTROLWAVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3416, 209, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3417, 209, 'GORMAN RUPP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3418, 209, 'MISSION COMMUNICATIONS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3419, 209, 'MITSUBISHI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3420, 209, 'MODICON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3421, 209, 'QEI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3422, 209, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3423, 209, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3424, 209, 'STARNET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3425, 209, 'TOSHIBA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3426, 209, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3866, 209, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3959, 209, 'CUMMINS', 'CUMMINS');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4179, 209, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SAFETY SHOWER

                case "SAFETY SHOWER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3427, 211, 'BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3428, 211, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3429, 211, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3430, 211, 'HAWS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3431, 211, 'SPEAKMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3432, 211, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4180, 211, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SCADA RADIO

                case "SCADA RADIO":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2159, 141, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2160, 141, 'CONTROLWAVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2161, 141, 'DATA-LINC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2162, 141, 'FREEWAVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2163, 141, 'GE-MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2164, 141, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2165, 141, 'MOTOROLA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2166, 141, 'REPCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2167, 141, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3870, 141, 'CAMBIUM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3871, 141, 'XETAWAVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4113, 141, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SCADA SYSTEM GEN

                case "SCADA SYSTEM GEN":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3449, 212, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4182, 212, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SCALE

                case "SCALE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3450, 213, 'BRECKNELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3451, 213, 'CAPITAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3452, 213, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3453, 213, 'DENVER INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3454, 213, 'DETECTO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3455, 213, 'EAGLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3456, 213, 'FAIRBANKS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3457, 213, 'FLO-TECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3458, 213, 'FORCE FLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3459, 213, 'FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3460, 213, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3461, 213, 'HOWE RICHARDSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3462, 213, 'INSCALE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3463, 213, 'KISTLER-MORSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3464, 213, 'METROHM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3465, 213, 'METTLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3466, 213, 'METTLER TOLEDO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3467, 213, 'OHAUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3468, 213, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3469, 213, 'RICE LAKE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3470, 213, 'SARTORIUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3471, 213, 'SCALETRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3472, 213, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3473, 213, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3474, 213, 'WIZARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3885, 213, 'MERRICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3924, 213, 'CHLOR-SCALE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4066, 213, 'Kistler Morris', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4183, 213, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SCREEN

                case "SCREEN":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3477, 215, 'ENVIREX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3478, 215, 'FMC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3479, 215, 'HAYWARD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3480, 215, 'JOHNSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3481, 215, 'LINK-BELT REX NORD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3482, 215, 'ROCKWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3483, 215, 'SENSUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3484, 215, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3485, 215, 'VICTAULIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4185, 215, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SCRUBBER

                case "SCRUBBER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3475, 214, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3476, 214, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3925, 214, 'POWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4184, 214, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SECONDARY CONTAINMENT

                case "SECONDARY CONTAINMENT":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2224, 147, 'CHEM-TAINER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2225, 147, 'EAGLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2226, 147, 'ENPAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2227, 147, 'POLYPROCESSING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2228, 147, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4119, 147, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SECURITY SYSTEM

                case "SECURITY SYSTEM":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3486, 216, 'ADT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3487, 216, 'ALLSTAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3488, 216, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3489, 216, 'GENTEX CORP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3490, 216, 'GUARDIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3491, 216, 'SIMPLEX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3492, 216, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4186, 216, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SERVER

                case "SERVER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3493, 217, 'DELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3494, 217, 'HP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3495, 217, 'IBM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3496, 217, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4187, 217, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region SOFTENER

                case "SOFTENER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3702, 232, 'KINETICO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3703, 232, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4202, 232, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region STREET VALVE

                case "STREET VALVE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3497, 218, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3498, 218, 'AMERICAN AVK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3499, 218, 'AMERICAN CAST IRON PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3500, 218, 'AMERICAN DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3501, 218, 'AMERICAN FLOW CONTROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3502, 218, 'AMERICAN FOUNDRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3503, 218, 'AP SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3504, 218, 'APCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3505, 218, 'AVK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3506, 218, 'AY MCDONALD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3507, 218, 'CHAPMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3508, 218, 'CLA-VAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3509, 218, 'CLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3510, 218, 'COREY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3511, 218, 'CRANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3512, 218, 'CRISPIN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3513, 218, 'DARLING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3514, 218, 'DEZURICK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3515, 218, 'DRESSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3516, 218, 'EAST JORDAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3517, 218, 'ECLIPSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3518, 218, 'EDDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3519, 218, 'FMC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3520, 218, 'FORD METER BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3521, 218, 'FRANKLIN HYDRANTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3522, 218, 'GOLDEN ANDERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3523, 218, 'IOWA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3524, 218, 'JONES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3525, 218, 'KENNEDY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3526, 218, 'KUNKLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3527, 218, 'KUPFERLE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3528, 218, 'LUDLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3529, 218, 'M-H/MCWANE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3530, 218, 'MATCO NORCA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3531, 218, 'MH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3532, 218, 'MICHIGAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3533, 218, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3534, 218, 'PENN-TROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3535, 218, 'PRATT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3536, 218, 'PRATT-CADY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3537, 218, 'RENSSELAER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3538, 218, 'REZNOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3539, 218, 'SCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3540, 218, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3541, 218, 'TCIW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3542, 218, 'TRUFLO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3543, 218, 'U.S. FOUNDRY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3544, 218, 'UNITED POWER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3545, 218, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3546, 218, 'US PIPE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3547, 218, 'VAL-MATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3548, 218, 'WACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3549, 218, 'WAL-MATIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3550, 218, 'WATEROUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3551, 218, 'WATTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3552, 218, 'WESTCOTT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3553, 218, 'WILLAMETTE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3962, 218, 'TEAM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3973, 218, 'HYDRA-STOP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4188, 218, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region TELEPHONE

                case "TELEPHONE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2176, 144, 'B-B ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2177, 144, 'BLACK BOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2178, 144, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2179, 144, 'CISCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2180, 144, 'ICX-DAQ ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2181, 144, 'MDS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2182, 144, 'MOTOROLA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2183, 144, 'OPTIMUM CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2184, 144, 'QEI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2185, 144, 'RACO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2186, 144, 'RADIONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2187, 144, 'SENSAPHONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2188, 144, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4116, 144, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region TOOL

                case "TOOL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3651, 226, 'CLIPPER POWER SYS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3652, 226, 'CRAFTSMAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3653, 226, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3654, 226, 'FLUKE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3655, 226, 'FORD MOTOR COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3656, 226, 'GENERAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3657, 226, 'INGERSOL RAND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3658, 226, 'JOHN DEERE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3659, 226, 'JOHNSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3660, 226, 'KUBOTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3661, 226, 'LINCOLN MOTORS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3662, 226, 'MUELLER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3663, 226, 'RADIO DETECTION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3664, 226, 'TORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3665, 226, 'TROY-BILT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3666, 226, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3667, 226, 'WACKER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3668, 226, 'WACKER NEUSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4196, 226, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region TRANSFORMER

                case "TRANSFORMER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3775, 239, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3776, 239, 'ACME', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3777, 239, 'ADVANCE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3778, 239, 'AED INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3779, 239, 'ALLEN BRADLEY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3780, 239, 'ALLIS CHALMERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3781, 239, 'ALLIS CHAMBERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3782, 239, 'CHALLENGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3783, 239, 'CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3784, 239, 'DONGAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3785, 239, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3786, 239, 'EGS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3787, 239, 'FEDERAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3788, 239, 'FEDERAL PACIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3789, 239, 'FURNAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3790, 239, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3791, 239, 'GENERAL SIGNAL-SPX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3792, 239, 'HAMMOND', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3793, 239, 'HITRAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3794, 239, 'ITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3795, 239, 'JEFFERSON ELECTRIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3796, 239, 'MAGNETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3797, 239, 'OLSUN ELECTRICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3798, 239, 'REX MANUFACTURING', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3799, 239, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3800, 239, 'SPANG-MAGNETICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3801, 239, 'SQUARE D', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3802, 239, 'SUNBELT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3803, 239, 'TCI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3804, 239, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3805, 239, 'WAGNER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3806, 239, 'WESTINGHOUSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3937, 239, 'OZONIA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3956, 239, 'EATON', 'EATON');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4209, 239, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region TRANSMITTER

                case "TRANSMITTER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3807, 240, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3808, 240, 'AGM ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3809, 240, 'AIRMATE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3810, 240, 'AMETEK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3811, 240, 'ANCHOR SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3812, 240, 'ASHCROFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3813, 240, 'BADGER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3814, 240, 'BIF', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3815, 240, 'BRISTOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3816, 240, 'BRISTOL BABCOCK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3817, 240, 'CONTROLWAVE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3818, 240, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3819, 240, 'DEVAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3820, 240, 'DREXELBROOK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3821, 240, 'DWYER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3822, 240, 'EMERSON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3823, 240, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3824, 240, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3825, 240, 'FLOW LINE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3826, 240, 'FORCE FLOW', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3827, 240, 'FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3828, 240, 'GRUNDFOS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3829, 240, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3830, 240, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3831, 240, 'INVENTRON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3832, 240, 'ITT BARTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3833, 240, 'KISTLER-MORSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3834, 240, 'LESLIE CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3835, 240, 'MAGNATROL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3836, 240, 'MERCOID', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3837, 240, 'MILLTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3838, 240, 'MKS INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3839, 240, 'MOORE INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3840, 240, 'MORSE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3841, 240, 'PANAMETRICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3842, 240, 'PRECISION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3843, 240, 'PULSAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3844, 240, 'REGAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3845, 240, 'RIGHTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3846, 240, 'ROSEMOUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3847, 240, 'SENSUS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3848, 240, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3849, 240, 'SIGMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3850, 240, 'SONIC SOLUTIONS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3851, 240, 'TAYLOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3852, 240, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3853, 240, 'WIKA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3854, 240, 'YOKOGAWA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3926, 240, 'MILTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3927, 240, 'PNEUMECATOR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3928, 240, 'POWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3929, 240, 'DREXELBROOK ENG CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4073, 240, 'IFM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4210, 240, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region UNINTERUPTED POWER SUPPLY

                case "UNINTERUPTED POWER SUPPLY":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3710, 235, 'BEST', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3711, 235, 'BEST-UPS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3712, 235, 'CD TECHNOLOGIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3713, 235, 'EATON/CUTLER HAMMER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3714, 235, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3715, 235, 'INTERNATIONAL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3716, 235, 'LIEBERT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3717, 235, 'MESTA ELECTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3718, 235, 'SCHNEIDER ELECTRIC-APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3719, 235, 'TRIPPLITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3720, 235, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3886, 235, 'PHOENIX CONTACT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3930, 235, 'APC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4205, 235, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region UV SANITIZER

                case "UV SANITIZER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3707, 234, 'SENTINEL-CALGON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3708, 234, 'TROJAN UV', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3709, 234, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4204, 234, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region UV-SOUND

                case "UV-SOUND":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3887, 242, 'LG SONIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3888, 242, 'SONIC SOLUTIONS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3889, 242, 'UNKNOWN', NULL);
";

                #endregion

                #region VEHICLE

                case "VEHICLE":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3721, 236, 'CASE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3722, 236, 'CHEVROLET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3723, 236, 'CLARK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3724, 236, 'CROWN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3725, 236, 'DODGE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3726, 236, 'FORD MOTOR COMPANY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3727, 236, 'HYSTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3728, 236, 'KUBOTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3729, 236, 'SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3730, 236, 'SMOKER CRAFT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3731, 236, 'TOYOTA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3732, 236, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3733, 236, 'YALE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3931, 236, 'KOMATSO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3932, 236, 'SKYJACK', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3957, 236, 'MILLER-TRAILBLAZER', 'MILLER-TRAILBLAZER');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4206, 236, 'UNKNOWN', 'OTHER');
";


                #endregion

                #region VOC STRIPPER

                case "VOC STRIPPER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3704, 233, 'HYDRO GROUP', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3705, 233, 'LAYNE CHRISTENSEN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3706, 233, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3953, 233, 'Liqui-Cell', 'Liqui-Cell');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4203, 233, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region WASTE TANK

                case "WASTE TANK":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3648, 225, 'AQUASTORE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3649, 225, 'MYERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3650, 225, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4195, 225, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region WATER HEATER

                case "WATER HEATER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2571, 174, 'AMERICAN STD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2572, 174, 'AO SMITH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2573, 174, 'ARISTON THERMO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2574, 174, 'BRADFORD WHITE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2575, 174, 'DAYTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2576, 174, 'EEMAX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2577, 174, 'GE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2578, 174, 'HUBBELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2579, 174, 'INDEECO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2580, 174, 'LOCHINVAR', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2581, 174, 'PEERLESS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2582, 174, 'PVI INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2583, 174, 'RHEEM', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2584, 174, 'RUUD', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2585, 174, 'STATE INDUSTRIES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2586, 174, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2587, 174, 'VANGUARD HEATERS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (2588, 174, 'WHIRLPOOL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3940, 174, 'CHROMALOX', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3954, 174, 'NAVIEN', 'NAVIEN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4146, 174, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region WATER QUALITY ANALYZER

                case "WATER QUALITY ANALYZER":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3736, 238, 'ACCUMET', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3737, 238, 'AGILENT-VARIAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3738, 238, 'ATI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3739, 238, 'CAPITAL CONTROLS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3740, 238, 'CHEMTRAC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3741, 238, 'DENVER INSTRUMENTS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3742, 238, 'ENDRESS-HAUSER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3743, 238, 'ENTECH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3744, 238, 'FISCHER PORTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3745, 238, 'FISHER SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3746, 238, 'FOXBORO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3747, 238, 'GREAT LAKES', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3748, 238, 'HACH', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3749, 238, 'HIAC-ROYCO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3750, 238, 'HONEYWELL', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3751, 238, 'IN-USA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3752, 238, 'LEED-NORTHRUP CO', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3753, 238, 'LMI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3754, 238, 'MET ONE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3755, 238, 'MILLTRONICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3756, 238, 'MILTON ROY', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3757, 238, 'OAKTON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3758, 238, 'PROMINENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3759, 238, 'ROSEMOUNT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3760, 238, 'SEVERN TRENT', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3761, 238, 'SHIMADZU', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3762, 238, 'SIEMENS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3763, 238, 'SIGMA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3764, 238, 'TELEDYNE', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3765, 238, 'THERMO-ORION', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3766, 238, 'THOMAS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3767, 238, 'TURNER DESIGNS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3768, 238, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3769, 238, 'US FILTER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3770, 238, 'VWR SCIENTIFIC', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3771, 238, 'WALLACE-TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3772, 238, 'YSI', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3773, 238, 'ZELLWEGER ANALYTICS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3774, 238, 'ZETA-METER', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3934, 238, 'WALLACE & TIERNAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3935, 238, 'EVOQUA', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3979, 238, 'ABB', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3980, 238, 'INFICON', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3981, 238, 'S::CAN', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4208, 238, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region WATER TREATMENT CONTACTOR

                case "WATER TREATMENT CONTACTOR":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3696, 230, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4200, 230, 'UNKNOWN', 'OTHER');
";

                #endregion

                #region WELL

                case "WELL":
                    return @"
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3734, 237, 'UNKNOWN', 'UNKNOWN');
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (3735, 237, 'US MOTORS', NULL);
INSERT INTO EquipmentManufacturers (Id, EquipmentTypeId, Description, MapCallDescription) VALUES (4207, 237, 'UNKNOWN', 'OTHER');
";

                #endregion

                default:
                    throw new InvalidOperationException(
                        $"{typeof(EquipmentManufacturer).Name} for EquipmentType '{equipmentType}' have not been scripted.");
            }
        }
    }
}
