using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140910091619124), Tags("Production")]
    public class AddUserTypeToTblPermissions : Migration
    {
        public override void Up()
        {
            Alter.Table("tblPermissions")
                 .AddColumn("UserTypeId")
                 .AsInt32()
                 .Nullable();

            Execute.Sql(@"
    declare @internal int;
    declare @internalContractor int;
    declare @external int;
    set @internal = (select top 1 Id from UserTypes where Description = 'Internal');
    set @internalContractor = (select top 1 Id from UserTypes where Description = 'Internal Contractor');
    set @external = (select top 1 Id from UserTypes where Description = 'External Contractor');
    
    -- Set all to internal by default.
    update [tblPermissions] set [UserTypeid] = @internal;    

    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 54
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 74
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 140
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 163
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 245
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 256
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 364
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 366
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 375
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 385
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 391
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 486
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 542
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 589
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 594
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 603
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 729
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 866
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 867
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 901
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 902
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 909
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 995
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1047
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1052
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1058
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1199
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1202
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1206
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1209
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1260
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1263
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1269
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1292
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1313
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1314
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1316
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1317
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1327
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1328
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1329
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1338
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1377
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1378
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1435
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1461
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1462
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1468
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1471
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1526
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1531
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1534
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1535
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1549
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1553
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1568
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1622
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1623
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1624
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1626
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1627
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1628
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1632
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1635
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1639
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1641
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1650
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1659
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1663
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1687
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1688
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1689
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1690
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1691
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1692
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1693
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1722
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1723
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1742
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1743
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1744
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1747
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1753
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1754
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1755
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1763
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1768
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1772
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1779
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1797
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1802
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1805
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1806
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1807
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1808
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1810
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1815
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1824
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1834
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1835
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1850
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1860
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1876
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1877
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1878
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1879
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1880
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1881
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1882
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1883
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1884
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1885
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1886
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1887
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1889
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1890
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1893
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1894
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1895
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1897
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1898
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1933
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1935
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1937
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1938
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1941
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1942
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1943
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1944
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1945
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1946
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1947
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1948
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1949
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1950
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1951
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1952
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1953
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1954
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1956
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1957
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1958
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1959
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1960
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1961
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1962
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1963
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1964
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1965
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1966
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1967
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1968
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1969
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1970
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1971
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1972
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1973
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1974
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1975
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1976
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1977
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1978
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1979
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1980
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1981
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1982
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1983
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1984
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1985
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1986
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1987
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1988
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1989
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1990
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1991
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1992
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1993
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1994
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1995
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1996
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1997
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1998
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 1999
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2000
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2001
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2002
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2003
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2004
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2005
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2006
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2007
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2008
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2009
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2010
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2011
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2012
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2013
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2014
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2015
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2016
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2017
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2018
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2019
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2020
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2021
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2022
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2023
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2024
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2025
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2026
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2027
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2028
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2029
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2030
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2031
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2032
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2033
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2034
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2035
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2036
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2037
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2038
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2039
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2040
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2041
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2042
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2043
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2044
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2045
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2046
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2047
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2048
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2049
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2050
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2051
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2052
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2053
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2054
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2055
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2056
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2057
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2058
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2059
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2060
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2061
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2062
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2063
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2064
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2065
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2066
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2067
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2068
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2069
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2070
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2071
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2072
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2073
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2074
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2075
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2076
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2077
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2078
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2079
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2080
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2081
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2082
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2083
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2084
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2085
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2086
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2087
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2088
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2089
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2090
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2091
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2092
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2093
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2094
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2095
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2096
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2097
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2098
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2099
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2100
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2101
    update [tblPermissions] set [UserTypeId] = @external where [RecId] = 2102
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 383
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 732
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1339
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1401
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1469
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1470
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1651
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1654
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1661
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1666
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1667
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1685
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1686
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1694
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1700
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1706
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1716
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1730
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1732
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1738
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1739
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1766
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1791
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1795
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1817
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 1820
    update [tblPermissions] set [UserTypeId] = @internalContractor where [RecId] = 2105

");
            Alter.Column("UserTypeId")
                 .OnTable("tblPermissions")
                 .AsInt32()
                 .NotNullable()
                 .ForeignKey("FK_tblPermissions_UserTypes_UserTypeId", "UserTypes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblPermissions_UserTypes_UserTypeId")
                  .OnTable("tblPermissions");

            Delete.Column("UserTypeId")
                  .FromTable("tblPermissions");
        }
    }
}
