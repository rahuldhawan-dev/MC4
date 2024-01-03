use [McProd];
GO

CREATE TABLE [ReportViewings] (
[ReportViewingID] int unique identity not null,
[EmployeeID] int not null,
[DateViewed] datetime not null,
[ReportName] varchar(50) not null,
CONSTRAINT [PK_ReportViewings] PRIMARY KEY CLUSTERED (
[ReportViewingID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [ReportViewings]  ADD CONSTRAINT [FK_ReportViewings_tblPermissions_EmployeeID] FOREIGN KEY (
[EmployeeID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

GRANT ALL ON [ReportViewings] TO MCUSER;
GO
