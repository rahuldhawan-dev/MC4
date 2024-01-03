--Drop Table ContractorUsers
CREATE TABLE [ContractorUsers] (
		[ContractorUserID] int unique identity not null,
		[ContractorID] int not null,
		[Email] varchar(40) unique not null,
		[Password] varchar(128) not null,
		[PasswordSalt] uniqueidentifier unique not null,
		[PasswordQuestion] varchar(256) not null, 
		[PasswordAnswer] varchar(128) not null,
		[IsAdmin] bit not null,
		CONSTRAINT [PK_ContractorUsers] PRIMARY KEY CLUSTERED (
				[ContractorUserID] ASC
		) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [ContractorUsers]  WITH NOCHECK ADD CONSTRAINT [FK_ContractorUsers_Contractors_ContractorID] FOREIGN KEY (
		[ContractorID]
) REFERENCES [Contractors] (
		[ContractorID]
)
GO

ALTER TABLE [dbo].[ContractorUsers] ADD  CONSTRAINT [DF_ContractorUsers_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO

ALTER TABLE [dbo].[Crews] ADD [ContractorID] int null;

ALTER TABLE [dbo].[Crews] WITH NOCHECK ADD CONSTRAINT [FK_Crews_Contractors_ContractorID] FOREIGN KEY (
		[ContractorID]
) REFERENCES [Contractors] (
		[ContractorID]
)
GO

-- make Crews.OperatingCenterID nullable:
ALTER TABLE dbo.Crews ADD
	OpCntrID int NULL
GO

Update dbo.Crews Set OpCntrID = OperatingCenterID
GO

ALTER TABLE dbo.Crews
-- Doesn't appear to be a contraint
--	DROP CONSTRAINT FK_Crews_OperatingCenters_OperatingCenterID
	DROP CONSTRAINT FK_Crews_tblOpCntr_OperatingCenterID
GO

ALTER TABLE dbo.Crews
	DROP COLUMN OperatingCenterID
GO

EXECUTE sp_rename N'dbo.Crews.OpCntrID', N'OperatingCenterID', 'COLUMN' 
GO

ALTER TABLE [Crews]  WITH NOCHECK ADD CONSTRAINT [FK_Crews_OperatingCenters_OperatingCenterID] FOREIGN KEY (
[OperatingCenterID]
) REFERENCES [OperatingCenters] (
[OperatingCenterID]
)
GO
