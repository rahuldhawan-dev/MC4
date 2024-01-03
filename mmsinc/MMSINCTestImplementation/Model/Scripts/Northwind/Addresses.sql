USE Northwind

/****** Object:  Table [dbo].[States]    Script Date: 11/12/2008 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Cities_States]') AND type = 'F')
ALTER TABLE [dbo].[Cities] DROP CONSTRAINT [FK_Cities_States]

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Addresses_States]') AND type = 'F')
ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [FK_Addresses_States]

--IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Contacts_Addresses]') AND type = 'F')
--ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [FK_Contacts_Addresses]
--
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Addresses_Contacts]') AND type = 'F')
ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [FK_Addresses_Contacts]

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[States]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[States]
GO

CREATE TABLE dbo.States
	(
	StateID int NOT NULL IDENTITY (1, 1), 
	StateName varchar(50) NULL, 
	Abbreviation varchar(4) NULL
	) ON [PRIMARY]

ALTER TABLE dbo.States ADD CONSTRAINT
	PK_States PRIMARY KEY CLUSTERED 
	(
	StateID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Cities]    Script Date: 11/12/2008 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Addresses_Cities]') AND type = 'F')
ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [FK_Addresses_Cities]

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Cities]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[Cities]
GO

CREATE TABLE dbo.Cities
	(
	CityID int NOT NULL IDENTITY (1, 1), 
	StateID int, 
	CityName varchar(50) NULL
	) ON [PRIMARY]
GO

ALTER TABLE dbo.Cities ADD CONSTRAINT
	PK_Cities PRIMARY KEY CLUSTERED 
	(
	CityID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE dbo.Cities ADD CONSTRAINT
	FK_Cities_States FOREIGN KEY
	(
	StateID
	) REFERENCES dbo.States
	(
	StateID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO


/****** Object:  Table [dbo].[Contacts]    Script Date: 11/12/2008 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[Contacts]
GO
CREATE TABLE dbo.Contacts
	(
	ContactID int NOT NULL IDENTITY (1, 1),
	ContactName varchar(100) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Contacts ADD CONSTRAINT
	PK_Contacts PRIMARY KEY CLUSTERED 
	(
	ContactID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


/****** Object:  Table [dbo].[Addresses]    Script Date: 11/12/2008 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Addresses]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [dbo].[Addresses]
GO

CREATE TABLE dbo.Addresses
	(
	AddressID int NOT NULL IDENTITY (1, 1),
	ContactID int NULL,
	AddressLine1 varchar(100) NULL,
	AddressLine2 varchar(100) NULL,
	CityID int NULL,
	StateID int NULL,
	Zip varchar(5) NULL,
	ZipFour varchar(4) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Addresses ADD CONSTRAINT
	PK_Addresses PRIMARY KEY CLUSTERED 
	(
	AddressID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Addresses ADD CONSTRAINT
	FK_Addresses_States FOREIGN KEY
	(
	StateID
	) REFERENCES dbo.States
	(
	StateID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Addresses ADD CONSTRAINT
	FK_Addresses_Cities FOREIGN KEY
	(
	CityID
	) REFERENCES dbo.Cities
	(
	CityID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
ALTER TABLE dbo.Addresses ADD CONSTRAINT
	FK_Addresses_Contacts FOREIGN KEY
	(
	ContactID
	) REFERENCES dbo.Contacts
	(
	ContactID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO



