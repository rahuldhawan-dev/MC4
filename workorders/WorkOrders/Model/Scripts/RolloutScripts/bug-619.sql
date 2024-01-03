
insert into OperatingCenterStockedMaterials(OperatingCenterID, MaterialID)
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W0040202' and cast([Description] as varchar) = 'Corp, 2" Taper X FIP')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W048010H' and cast([Description] as varchar) = 'Coupling,  1" X  3/4" CC X FIP')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7440402' and cast([Description] as varchar) = 'Saddle, Tap  4" X 2" SRV')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7440602' and cast([Description] as varchar) = 'Saddle, Tap  6" X 2" SRV')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7440802' and cast([Description] as varchar) = 'Saddle, Tap  8" X 2" SRV')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7441002' and cast([Description] as varchar) = 'Saddle, Tap 10" X  2" SRV')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7441202' and cast([Description] as varchar) = 'Saddle, Tap 12" X 2" SRV')
	union all
	select (select recID from tblOpCntr where opCntr = 'nj3'), (select MaterialID from Materials where partNumber = 'W7441602' and cast([Description] as varchar) = 'Saddle, Tap 16" X  2" SRV')


/* EW1, EW2, EW3, EW4, LWC */

    union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W4000652' and cast([Description] as varchar) = 'Pipe,  6" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W4000852' and cast([Description] as varchar) = 'Pipe,  8" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W4001252' and cast([Description] as varchar) = 'Pipe, 12" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W4001652' and cast([Description] as varchar) = 'Pipe, 16" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W4031252' and cast([Description] as varchar) = 'Pipe, 12" DI RJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W8451414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OL')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W2851414' and cast([Description] as varchar) = 'Coupling, 14" FLEX Bolted DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W3761414' and cast([Description] as varchar) = 'Gland, 14" Retainer DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW1'), (select materialID from Materials where partNumber ='W8441414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OR')

	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W4000652' and cast([Description] as varchar) = 'Pipe,  6" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W4000852' and cast([Description] as varchar) = 'Pipe,  8" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W4001252' and cast([Description] as varchar) = 'Pipe, 12" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W4001652' and cast([Description] as varchar) = 'Pipe, 16" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W4031252' and cast([Description] as varchar) = 'Pipe, 12" DI RJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W8451414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OL')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W2851414' and cast([Description] as varchar) = 'Coupling, 14" FLEX Bolted DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W3761414' and cast([Description] as varchar) = 'Gland, 14" Retainer DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW2'), (select materialID from Materials where partNumber ='W8441414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OR')

	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W4000652' and cast([Description] as varchar) = 'Pipe,  6" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W4000852' and cast([Description] as varchar) = 'Pipe,  8" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W4001252' and cast([Description] as varchar) = 'Pipe, 12" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W4001652' and cast([Description] as varchar) = 'Pipe, 16" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W4031252' and cast([Description] as varchar) = 'Pipe, 12" DI RJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W8451414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OL')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W2851414' and cast([Description] as varchar) = 'Coupling, 14" FLEX Bolted DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W3761414' and cast([Description] as varchar) = 'Gland, 14" Retainer DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW3'), (select materialID from Materials where partNumber ='W8441414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OR')

	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W4000652' and cast([Description] as varchar) = 'Pipe,  6" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W4000852' and cast([Description] as varchar) = 'Pipe,  8" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W4001252' and cast([Description] as varchar) = 'Pipe, 12" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W4001652' and cast([Description] as varchar) = 'Pipe, 16" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W4031252' and cast([Description] as varchar) = 'Pipe, 12" DI RJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W8451414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OL')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W2851414' and cast([Description] as varchar) = 'Coupling, 14" FLEX Bolted DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W3761414' and cast([Description] as varchar) = 'Gland, 14" Retainer DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'EW4'), (select materialID from Materials where partNumber ='W8441414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OR')

	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W4000652' and cast([Description] as varchar) = 'Pipe,  6" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W4000852' and cast([Description] as varchar) = 'Pipe,  8" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W4001252' and cast([Description] as varchar) = 'Pipe, 12" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W4001652' and cast([Description] as varchar) = 'Pipe, 16" DI SJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W4031252' and cast([Description] as varchar) = 'Pipe, 12" DI RJ CL 52')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W8451414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OL')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W2851414' and cast([Description] as varchar) = 'Coupling, 14" FLEX Bolted DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W3761414' and cast([Description] as varchar) = 'Gland, 14" Retainer DI')
	union all
	select (select recID from tblOpCntr where opCntr = 'LWC'), (select materialID from Materials where partNumber ='W8441414' and cast([Description] as varchar) = 'Valve, 14" Butterfly MJ OR')
