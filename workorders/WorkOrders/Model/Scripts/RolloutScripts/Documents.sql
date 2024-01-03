--- Missing FK Constraint
ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_DocumentType_DocumentTypeID] FOREIGN KEY (
	[DocumentTypeID]
) REFERENCES [DocumentType] (
	[DocumentTypeID]
)
GO

---------------------------DOCUMENTS WORK ORDERS----------------------------
CREATE TABLE [DocumentsWorkOrders] (
	[DocumentWorkOrderID] int unique identity not null,
	[DocumentID] int not null,
	[WorkOrderID] int not null
	CONSTRAINT [PK_DocumentsWorkOrders] PRIMARY KEY CLUSTERED (
		[DocumentWorkOrderID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [DocumentsWorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_DocumentsWorkOrders_Documents_DocumentID] FOREIGN KEY (
	[DocumentID]
) REFERENCES [Document] (
	[DocumentID]
)
GO

ALTER TABLE [DocumentsWorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_DocumentsWorkOrders_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

SET IDENTITY_INSERT [dbo].[DataType] ON;
INSERT INTO Datatype(DataTypeID, Data_Type) values(127, 'Work Order')
SET IDENTITY_INSERT [dbo].[DataType] OFF;

SET IDENTITY_INSERT [dbo].[DocumentType] ON;
insert into documenttype(DocumentTypeID, Document_Type, DataTypeID) values(170,'Traffic Control', 127)
insert into documenttype(DocumentTypeID, Document_Type, DataTypeID) values(171,'Photos/Sketches', 127)
insert into documenttype(DocumentTypeID, Document_Type, DataTypeID) values(172,'Permits', 127)
insert into documenttype(DocumentTypeID, Document_Type, DataTypeID) values(173,'Correspondence', 127)

insert into documenttype(documentTypeID, Document_Type, DataTypeID) values(174, 'Markout Violation',127)
insert into documenttype(documentTypeID, Document_Type, DataTypeID) values(175, 'Misc', 127)
insert into documenttype(documentTypeID, Document_Type, DataTypeID) values(176, 'Maps', 127)
insert into documenttype(documentTypeID, Document_Type, DataTypeID) values(177, 'Easement', 127)
insert into documenttype(documentTypeID, Document_Type, DataTypeID) values(178, 'Specification', 127)
SET IDENTITY_INSERT [dbo].[DocumentType] OFF;


ALTER TABLE [Document] ADD [CreatedByID] int NULL
GO

ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_tblPermissions_CreatedByID] FOREIGN KEY (
[CreatedByID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

ALTER TABLE [Document] ADD [ModifiedByID] int NULL
GO

ALTER TABLE [Document] WITH NOCHECK ADD CONSTRAINT [FK_Document_tblPermissions_ModifiedByID] FOREIGN KEY (
[ModifiedByID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO
