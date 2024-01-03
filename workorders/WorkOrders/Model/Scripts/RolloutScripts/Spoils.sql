-----------------------------------------------------------------------------------------------------
-----------------------------------------------SPOILS------------------------------------------------
-----------------------------------------------------------------------------------------------------

-----------------------------------------------SPOILS------------------------------------------------
CREATE TABLE [Spoils] (
	[SpoilID] int unique identity not null,
	[WorkOrderID] int not null,
	[Quantity] decimal(6, 2) not null,
	[SpoilStorageLocationID] int not null,
	CONSTRAINT [PK_Spoils] PRIMARY KEY CLUSTERED (
		[SpoilID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [Spoils] TO mcuser;

ALTER TABLE [Spoils] WITH NOCHECK ADD CONSTRAINT [FK_Spoils_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

------------------------------------OPERATING CENTER SPOIL REMOVAL COSTS-------------------------------------
CREATE TABLE [OperatingCenterSpoilRemovalCosts] (
	[OperatingCenterSpoilRemovalCostID] int unique identity not null,
	[OperatingCenterID] int not null,
	[Cost] smallint not null,
	CONSTRAINT [PK_OperatingCenterSpoilRemovalCosts] PRIMARY KEY CLUSTERED (
		[OperatingCenterSpoilRemovalCostID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [OperatingCenterSpoilRemovalCosts] TO mcuser;

ALTER TABLE [OperatingCenterSpoilRemovalCosts] WITH NOCHECK ADD CONSTRAINT [FK_OperatingCenterSpoilRemovalCosts_tblOpCntr_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [tblOpCntr] (
	[RecID]
)
GO

---------------------------------------SPOIL STORAGE LOCATIONS---------------------------------------
CREATE TABLE [SpoilStorageLocations] (
	[SpoilStorageLocationID] int unique identity not null,
	[Name] varchar(30) not null,
	[OperatingCenterID] int not null,
	[TownID] int null,
	[StreetID] int null,
	CONSTRAINT [PK_SpoilStorageLocations] PRIMARY KEY CLUSTERED (
		[SpoilStorageLocationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [SpoilStorageLocations] TO mcuser;

ALTER TABLE [SpoilStorageLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilStorageLocations_tblOpCntr_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [tblOpCntr] (
	[RecID]
)
GO
ALTER TABLE [SpoilStorageLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilStorageLocations_tblNJAWTownNames_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [tblNJAWTownNames] (
	[RecID]
)
GO
ALTER TABLE [SpoilStorageLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilStorageLocations_tblNJAWStreets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [tblNJAWStreets] (
	[RecID]
)
GO

----------------------------------SPOIL FINAL PROCESSING LOCATIONS-----------------------------------
CREATE TABLE [SpoilFinalProcessingLocations] (
	[SpoilFinalProcessingLocationID] int unique identity not null,
	[Name] varchar(30) not null,
	[OperatingCenterID] int not null,
	[TownID] int null,
	[StreetID] int null,
	CONSTRAINT [PK_SpoilFinalProcessingLocations] PRIMARY KEY CLUSTERED (
		[SpoilFinalProcessingLocationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [SpoilFinalProcessingLocations] TO mcuser;

ALTER TABLE [SpoilFinalProcessingLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_tblOpCntr_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [tblOpCntr] (
	[RecID]
)
GO
ALTER TABLE [SpoilFinalProcessingLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_tblNJAWTownNames_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [tblNJAWTownNames] (
	[RecID]
)
GO
ALTER TABLE [SpoilFinalProcessingLocations] WITH NOCHECK ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_tblNJAWStreets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [tblNJAWStreets] (
	[RecID]
)
GO

-------------------------------------------SPOIL REMOVALS--------------------------------------------
CREATE TABLE [SpoilRemovals] (
	[SpoilRemovalID] int unique identity not null,
	[DateRemoved] smalldatetime not null,
	[Quantity] decimal(6, 2) not null,
	[RemovedFromID] int not null,
	[FinalDestinationID] int not null,
	CONSTRAINT [PK_SpoilRemovals] PRIMARY KEY CLUSTERED (
		[SpoilRemovalID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

GRANT ALL ON [SpoilRemovals] TO mcuser;

ALTER TABLE [SpoilRemovals] WITH NOCHECK ADD CONSTRAINT [FK_SpoilRemovals_SpoilStorageLocations_RemovedFromID] FOREIGN KEY (
	[RemovedFromID]
) REFERENCES [SpoilStorageLocations] (
	[SpoilStorageLocationID]
)
GO
ALTER TABLE [SpoilRemovals] WITH NOCHECK ADD CONSTRAINT [FK_SpoilRemovals_SpoilFinalProcessingLocations_FinalDestinationID] FOREIGN KEY (
	[FinalDestinationID]
) REFERENCES [SpoilFinalProcessingLocations] (
	[SpoilFinalProcessingLocationID]
)
GO

INSERT INTO [OperatingCenterSpoilRemovalCosts] ([OperatingCenterID], [Cost])
SELECT [RecID], 75 FROM [tblOpCntr] WHERE [WorkOrdersEnabled] = 1
