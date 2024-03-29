use [aww]
go

CREATE CLUSTERED INDEX [_dta_index_UserTable_c_17_1003150619__K1] ON [dbo].[UserTable] 
(
	[UserID] ASC
)
go

CREATE STATISTICS [_dta_stat_1003150619_1_9] ON [dbo].[UserTable]([UserID], [UserRights])
go

CREATE STATISTICS [_dta_stat_1003150619_4_3_1] ON [dbo].[UserTable]([UserLogin], [UserCustomerID], [UserID])
go

use [MCProd]
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K30_K41_K40_K25_K35_K32_K44_K20_K6_K16_K14] ON [dbo].[tblNJAWValves] 
(
	[StName] ASC,
	[ValSuf] ASC,
	[ValNum] ASC,
	[RecID] ASC,
	[TwnSection] ASC,
	[Town] ASC,
	[ValveStatus] ASC,
	[OpCntr] ASC,
	[CrossStreet] ASC,
	[Lon] ASC,
	[Lat] ASC
)
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K20_K40_K14_K16_K37] ON [dbo].[tblNJAWValves] 
(
	[OpCntr] ASC,
	[ValNum] ASC,
	[Lat] ASC,
	[Lon] ASC,
	[ValCtrl] ASC
)
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K32_K20_K41_K40] ON [dbo].[tblNJAWValves] 
(
	[Town] ASC,
	[OpCntr] ASC,
	[ValSuf] ASC,
	[ValNum] ASC
)
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K27_K32] ON [dbo].[tblNJAWValves] 
(
	[Route] ASC,
	[Town] ASC
)
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K40] ON [dbo].[tblNJAWValves] 
(
	[ValNum] ASC
)
go

CREATE STATISTICS [_dta_stat_1031010754_41_32] ON [dbo].[tblNJAWValves]([ValSuf], [Town])
go

CREATE STATISTICS [_dta_stat_1031010754_32_41] ON [dbo].[tblNJAWValves]([Town], [ValSuf])
go

CREATE STATISTICS [_dta_stat_1031010754_30_32_20] ON [dbo].[tblNJAWValves]([StName], [Town], [OpCntr])
go

CREATE STATISTICS [_dta_stat_1031010754_30_32_41] ON [dbo].[tblNJAWValves]([StName], [Town], [ValSuf])
go

CREATE STATISTICS [_dta_stat_1031010754_20_32_41_40] ON [dbo].[tblNJAWValves]([OpCntr], [Town], [ValSuf], [ValNum])
go

CREATE STATISTICS [_dta_stat_1031010754_30_20_40_41] ON [dbo].[tblNJAWValves]([StName], [OpCntr], [ValNum], [ValSuf])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_44_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValveStatus], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_38_41_4_5_25] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValLoc], [ValSuf], [Critical], [CriticalNotes], [RecID])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_38_21_19_34_4_5] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValLoc], [Opens], [NorPos], [Turns], [Critical], [CriticalNotes])
go

CREATE STATISTICS [_dta_stat_1031010754_40_20_32_38_21_19_34_4_5] ON [dbo].[tblNJAWValves]([ValNum], [OpCntr], [Town], [ValLoc], [Opens], [NorPos], [Turns], [Critical], [CriticalNotes])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_43_37_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValveSize], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_37_43_44_3_1_7_12_13_14] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValCtrl], [ValveSize], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [Lat])
go

CREATE STATISTICS [_dta_stat_1031010754_20_40_14_16_37_43_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [Lat], [Lon], [ValCtrl], [ValveSize], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit])
go

CREATE STATISTICS [_dta_stat_1031010754_20_41_32_30_40_6_7_25_29_45_35_18_44_43_33_37] ON [dbo].[tblNJAWValves]([OpCntr], [ValSuf], [Town], [StName], [ValNum], [CrossStreet], [DateInst], [RecID], [StNum], [WONum], [TwnSection], [MapPage], [ValveStatus], [ValveSize], [Traffic], [ValCtrl])
go

CREATE STATISTICS [_dta_stat_1031010754_40_20_42_43_34_33_4_5_25_37_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([ValNum], [OpCntr], [ValType], [ValveSize], [Turns], [Traffic], [Critical], [CriticalNotes], [RecID], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit])
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWStreets_15_1365579903__K8_K6_K9_K7_K5_K4_K3_K2_K1] ON [dbo].[tblNJAWStreets] 
(
	[Town] ASC,
	[StreetName] ASC,
	[St] ASC,
	[StreetSuffix] ASC,
	[StreetPrefix] ASC,
	[InactSt] ASC,
	[FullStName] ASC,
	[County] ASC,
	[RecID] ASC
)
go

CREATE STATISTICS [_dta_stat_1365579903_4_3] ON [dbo].[tblNJAWStreets]([InactSt], [FullStName])
go

CREATE STATISTICS [_dta_stat_1365579903_2_4] ON [dbo].[tblNJAWStreets]([County], [InactSt])
go

CREATE STATISTICS [_dta_stat_1365579903_8_2] ON [dbo].[tblNJAWStreets]([Town], [County])
go

CREATE STATISTICS [_dta_stat_1365579903_8_4_3] ON [dbo].[tblNJAWStreets]([Town], [InactSt], [FullStName])
go

CREATE STATISTICS [_dta_stat_1365579903_4_8_3_1] ON [dbo].[tblNJAWStreets]([InactSt], [Town], [FullStName], [RecID])
go

CREATE STATISTICS [_dta_stat_1365579903_4_8_2_1_3] ON [dbo].[tblNJAWStreets]([InactSt], [Town], [County], [RecID], [FullStName])
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_15_580913141__K38_K41_K1_K32_K7_K30_K25_K21_K18_K34] ON [dbo].[tblNJAWHydrant] 
(
	[StName] ASC,
	[Town] ASC,
	[ActRet] ASC,
	[OpCntr] ASC,
	[CrossStreet] ASC,
	[Lon] ASC,
	[Lat] ASC,
	[HydSuf] ASC,
	[HydNum] ASC,
	[RecID] ASC
)
go

CREATE STATISTICS [_dta_stat_580913141_38_18_32_21] ON [dbo].[tblNJAWHydrant]([StName], [HydNum], [OpCntr], [HydSuf])
go

CREATE STATISTICS [_dta_stat_580913141_32_18_29_27_5_6] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydNum], [Location], [LatValNum], [Critical], [CriticalNotes])
go

CREATE STATISTICS [_dta_stat_580913141_32_18_25_30_1_2_8_23_24] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydNum], [Lat], [Lon], [ActRet], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit])
go

CREATE STATISTICS [_dta_stat_580913141_32_18_29_21_5_6_34_17_13_48_49] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydNum], [Location], [HydSuf], [Critical], [CriticalNotes], [RecID], [HydMake], [DirOpen], [YearManufactured], [ManufacturedUpdated])
go

CREATE STATISTICS [_dta_stat_580913141_32_21_41_38_18_47_7_8_34_39_45_42_17_15_31_1] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydSuf], [Town], [StName], [HydNum], [FireDistrictID], [CrossStreet], [DateInst], [RecID], [StNum], [WONum], [TwnSection], [HydMake], [FireD], [MapPage], [ActRet])
go

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_15_1888777836__K48_K23_K1_K55_K58] ON [dbo].[WorkOrders] 
(
	[OperatingCenterID] ASC,
	[DateCompleted] ASC,
	[WorkOrderID] ASC,
	[CompletedByID] ASC,
	[OfficeAssignedOn] ASC
)
go

CREATE STATISTICS [_dta_stat_1888777836_1_48] ON [dbo].[WorkOrders]([WorkOrderID], [OperatingCenterID])
go

CREATE STATISTICS [_dta_stat_1888777836_37_1] ON [dbo].[WorkOrders]([TrafficControlRequired], [WorkOrderID])
go

CREATE STATISTICS [_dta_stat_1888777836_48_35] ON [dbo].[WorkOrders]([OperatingCenterID], [WorkDescriptionID])
go

CREATE STATISTICS [_dta_stat_1888777836_1_38] ON [dbo].[WorkOrders]([WorkOrderID], [StreetOpeningPermitRequired])
go

CREATE STATISTICS [_dta_stat_1888777836_23_38_22] ON [dbo].[WorkOrders]([DateCompleted], [StreetOpeningPermitRequired], [PriorityID])
go

CREATE STATISTICS [_dta_stat_1888777836_1_58_55] ON [dbo].[WorkOrders]([WorkOrderID], [OfficeAssignedOn], [CompletedByID])
go

CREATE STATISTICS [_dta_stat_1888777836_48_1_50] ON [dbo].[WorkOrders]([OperatingCenterID], [WorkOrderID], [ApprovedOn])
go

CREATE STATISTICS [_dta_stat_1888777836_55_1_48] ON [dbo].[WorkOrders]([CompletedByID], [WorkOrderID], [OperatingCenterID])
go

CREATE STATISTICS [_dta_stat_1888777836_23_1_48_55] ON [dbo].[WorkOrders]([DateCompleted], [WorkOrderID], [OperatingCenterID], [CompletedByID])
go

CREATE STATISTICS [_dta_stat_1888777836_48_1_23_38] ON [dbo].[WorkOrders]([OperatingCenterID], [WorkOrderID], [DateCompleted], [StreetOpeningPermitRequired])
go

CREATE STATISTICS [_dta_stat_1888777836_38_22_1_23_48_36] ON [dbo].[WorkOrders]([StreetOpeningPermitRequired], [PriorityID], [WorkOrderID], [DateCompleted], [OperatingCenterID], [MarkoutRequirementID])
go

CREATE NONCLUSTERED INDEX [_dta_index_CrewAssignments_15_640773390__K2_K9_K7_K6_K5_K4_K8_K3_K1] ON [dbo].[CrewAssignments] 
(
	[CrewID] ASC,
	[EmployeesOnJob] ASC,
	[DateEnded] ASC,
	[DateStarted] ASC,
	[AssignedFor] ASC,
	[AssignedOn] ASC,
	[Priority] ASC,
	[WorkOrderID] ASC,
	[CrewAssignmentID] ASC
)
go

CREATE NONCLUSTERED INDEX [_dta_index_CrewAssignments_15_640773390__K3_K5_K9_K7_K6_K4_K8_K2_K1] ON [dbo].[CrewAssignments] 
(
	[WorkOrderID] ASC,
	[AssignedFor] ASC,
	[EmployeesOnJob] ASC,
	[DateEnded] ASC,
	[DateStarted] ASC,
	[AssignedOn] ASC,
	[Priority] ASC,
	[CrewID] ASC,
	[CrewAssignmentID] ASC
)
go

CREATE STATISTICS [_dta_stat_640773390_5_3] ON [dbo].[CrewAssignments]([AssignedFor], [WorkOrderID])
go

CREATE STATISTICS [_dta_stat_640773390_1_9_7_6_8_2_5] ON [dbo].[CrewAssignments]([CrewAssignmentID], [EmployeesOnJob], [DateEnded], [DateStarted], [Priority], [CrewID], [AssignedFor])
go

CREATE STATISTICS [_dta_stat_640773390_3_9_7_6_5_4_8_2_1] ON [dbo].[CrewAssignments]([WorkOrderID], [EmployeesOnJob], [DateEnded], [DateStarted], [AssignedFor], [AssignedOn], [Priority], [CrewID], [CrewAssignmentID])
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K40_K64] ON [dbo].[tblNJAWService] 
(
	[OpCntr] ASC,
	[ServNum] ASC
)
go

CREATE STATISTICS [_dta_stat_1933965966_10] ON [dbo].[tblNJAWService]([CntRec])
go

CREATE STATISTICS [_dta_stat_1933965966_64_40] ON [dbo].[tblNJAWService]([ServNum], [OpCntr])
go

CREATE STATISTICS [_dta_stat_1933965966_56_64] ON [dbo].[tblNJAWService]([RecID], [ServNum])
go

CREATE NONCLUSTERED INDEX [_dta_index_MaterialsUsed_15_80771395__K2_K4_K1_K3_K6] ON [dbo].[MaterialsUsed] 
(
	[WorkOrderID] ASC,
	[Quantity] ASC,
	[MaterialsUsedID] ASC,
	[MaterialID] ASC,
	[StockLocationID] ASC
)
go

CREATE STATISTICS [_dta_stat_80771395_3_2_4_1_6] ON [dbo].[MaterialsUsed]([MaterialID], [WorkOrderID], [Quantity], [MaterialsUsedID], [StockLocationID])
go

CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWRestore_15_1453248232__K24_K25_K5_K7_K14_K16_K12_K21] ON [dbo].[tblNJAWRestore] 
(
	[ServID] ASC,
	[TypeRestore] ASC,
	[FinalCompBy] ASC,
	[FinalDate] ASC,
	[PartCompBy] ASC,
	[PartDate] ASC,
	[InitiatedBy] ASC,
	[RecID] ASC
)
go

CREATE STATISTICS [_dta_stat_1453248232_25_24_5_7_14_16_12_21] ON [dbo].[tblNJAWRestore]([TypeRestore], [ServID], [FinalCompBy], [FinalDate], [PartCompBy], [PartDate], [InitiatedBy], [RecID])
go

CREATE STATISTICS [_dta_stat_4911089_13_8] ON [dbo].[tblNJAWValInspData]([RecID], [OpCntr])
go

CREATE STATISTICS [_dta_stat_4911089_9_19_13] ON [dbo].[tblNJAWValInspData]([Operated], [WOReq1], [RecID])
go

CREATE STATISTICS [_dta_stat_4911089_8_18_9_19_13] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [Operated], [WOReq1], [RecID])
go

CREATE STATISTICS [_dta_stat_4911089_9_2_19_13_8] ON [dbo].[tblNJAWValInspData]([Operated], [DateInspect], [WOReq1], [RecID], [OpCntr])
go

CREATE STATISTICS [_dta_stat_4911089_8_18_9_2_19_13] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [Operated], [DateInspect], [WOReq1], [RecID])
go

CREATE STATISTICS [_dta_stat_4911089_8_18_13_2_16_17_3_4_5_1_9_10_11_19_20_21] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [RecID], [DateInspect], [Turns], [TurnsNotCompleted], [Inaccessible], [InspectBy], [InspectorNum], [DateAdded], [Operated], [PosFound], [PosLeft], [WOReq1], [WOReq2], [WOReq3])
go

CREATE STATISTICS [_dta_stat_1181299318_1_2] ON [dbo].[Materials]([MaterialID], [PartNumber])
go

CREATE STATISTICS [_dta_stat_1975014117_1_3_4_5_6_7_9_10_11_12] ON [dbo].[Document]([documentID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Description], [File_Size], [File_Name], [CreatedByID], [ModifiedByID])
go

CREATE STATISTICS [_dta_stat_1975014117_3_4_5_6_7_9_10_11_12_2_1] ON [dbo].[Document]([CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Description], [File_Size], [File_Name], [CreatedByID], [ModifiedByID], [documentTypeID], [documentID])
go

CREATE STATISTICS [_dta_stat_237959924_3_7] ON [dbo].[tblNJAWHydInspData]([DateInspect], [HydNum])
go

CREATE STATISTICS [_dta_stat_237959924_7_13_23] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [HydrantTagStatusID])
go

CREATE STATISTICS [_dta_stat_237959924_7_13_3_15] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [DateInspect], [RecID])
go

CREATE STATISTICS [_dta_stat_237959924_7_13_17_15] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [WOReq1], [RecID])
go

CREATE STATISTICS [_dta_stat_237959924_7_13_3_17_15] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [DateInspect], [WOReq1], [RecID])
go

CREATE STATISTICS [_dta_stat_1645300971_25_8] ON [dbo].[tblPermissions]([UserName], [EmpNum])
go

CREATE STATISTICS [_dta_stat_1645300971_23_1] ON [dbo].[tblPermissions]([UserInact], [RecID])
go

CREATE STATISTICS [_dta_stat_354152357_5_1] ON [dbo].[tblNJAWTownNames]([County], [RecID])
go

CREATE STATISTICS [_dta_stat_354152357_19_1_63] ON [dbo].[tblNJAWTownNames]([FD1], [RecID], [Town])
go

CREATE STATISTICS [_dta_stat_354152357_63_52_51] ON [dbo].[tblNJAWTownNames]([Town], [OpCntr3], [OpCntr2])
go

CREATE STATISTICS [_dta_stat_354152357_52_51_50_63_1_5] ON [dbo].[tblNJAWTownNames]([OpCntr3], [OpCntr2], [OpCntr1], [Town], [RecID], [County])
go

CREATE STATISTICS [_dta_stat_354152357_1_50_69_68_51_52] ON [dbo].[tblNJAWTownNames]([RecID], [OpCntr1], [Lon], [Lat], [OpCntr2], [OpCntr3])
go

CREATE STATISTICS [_dta_stat_208771851_3_2] ON [dbo].[OperatingCenterStockedMaterials]([MaterialID], [OperatingCenterID])
go

