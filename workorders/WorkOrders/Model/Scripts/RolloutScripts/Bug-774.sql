use [McProd]
GO

---------------------------------------CUSTOMER IMPACT RANGES---------------------------------------
CREATE TABLE [CustomerImpactRanges] (
	[CustomerImpactRangeID] int unique identity not null,
	[Description] varchar(10) unique not null,
	CONSTRAINT [PK_CustomerImpactRanges] PRIMARY KEY CLUSTERED (
		[CustomerImpactRangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY];
GO

ALTER TABLE [WorkOrders]
ADD [CustomerImpactRangeID] int null;
GO

ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_CustomerImpactRanges_CustomerImpactRangeID] FOREIGN KEY (
	[CustomerImpactRangeID]
) REFERENCES [CustomerImpactRanges] (
	[CustomerImpactRangeID]
)
GO

INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('0-50');
INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('51-100');
INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('101-200');
INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('>200');

GRANT ALL ON [CustomerImpactRanges] TO MCUSER;

-----------------------------------------REPAIR TIME RANGES-----------------------------------------
CREATE TABLE [RepairTimeRanges] (
	[RepairTimeRangeID] int unique identity not null,
	[Description] varchar(15) unique not null,
	CONSTRAINT [PK_RepairTimeRanges] PRIMARY KEY CLUSTERED (
		[RepairTimeRangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY];
GO

ALTER TABLE [WorkOrders]
ADD [RepairTimeRangeID] int null;
GO

ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_RepairTimeRanges_RepairTimeRangeID] FOREIGN KEY (
	[RepairTimeRangeID]
) REFERENCES [RepairTimeRanges] (
	[RepairTimeRangeID]
)
GO

INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('4-6');
INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('8-10');
INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('10-12');

GRANT ALL ON [RepairTimeRanges] TO MCUSER;
