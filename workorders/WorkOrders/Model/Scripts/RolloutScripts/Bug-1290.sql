CREATE TABLE [RequisitionTypes] (
[RequisitionTypeID] int unique identity not null,
[Description] varchar(50) not null,
CONSTRAINT [PK_RequisitionTypes] PRIMARY KEY CLUSTERED (
[RequisitionTypeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [Requisitions] (
[RequisitionID] int unique identity not null,
[RequisitionTypeID] int not null,
[SAPRequisitionNumber] varchar(50) null,
[WorkOrderID] int not null,
[CreatorID] int not null,
[CreatedOn] smalldatetime not null default getdate()
CONSTRAINT [PK_Requisitions] PRIMARY KEY CLUSTERED (
[RequisitionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_RequisitionType_RequisitionTypeID] FOREIGN KEY (
[RequisitionTypeID]
) REFERENCES [RequisitionTypes] (
[RequisitionTypeID]
)
GO

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_WorkOrders_WorkOrderID] FOREIGN KEY (
[WorkOrderID]
) REFERENCES [WorkOrders] (
[WorkOrderID]
)
GO

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_tblPermissions_CreatorID] FOREIGN KEY (
[CreatorID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

INSERT INTO RequisitionTypes Values('Paving')
INSERT INTO RequisitionTypes Values('Contracted Service')
INSERT INTO RequisitionTypes Values('Traffic Control')
INSERT INTO RequisitionTypes Values('Spoils')

GRANT ALL ON Requisitions TO MCUser
GRANT ALL ON RequisitionTypes TO MCUser