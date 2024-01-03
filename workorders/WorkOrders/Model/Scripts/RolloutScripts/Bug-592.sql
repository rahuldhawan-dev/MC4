/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
GO
--Add ServiceSizeID to MainBreaks
--Update mainbreaks with a valid id from tblNJAWSizeServ
--make recID the primary key on tblNJAWSizeServ
--Create a FK from mainbreaks to tblNJAWSizeServ
--make ServiceSizeID NOT NULL
ALTER TABLE dbo.MainBreaks ADD
	ServiceSizeID int NULL
GO
UPDATE MainBreaks SET ServiceSizeID = 24 --This is Size 0
GO
ALTER TABLE [dbo].[tblNJAWSizeServ] ADD  CONSTRAINT [PK_tblNJAWSizeServ] PRIMARY KEY CLUSTERED 
([RecID] ASC) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MainBreaks]  WITH CHECK ADD  CONSTRAINT [FK_MainBreaks_tblNJAWSizeServ_ServiceSizeID] FOREIGN KEY([ServiceSizeID])
REFERENCES [dbo].[tblNJAWSizeServ] ([RecID])
GO
ALTER TABLE [dbo].[MainBreaks] CHECK CONSTRAINT [FK_MainBreaks_tblNJAWSizeServ_ServiceSizeID]
GO
ALTER TABLE dbo.MainBreaks 
	ALTER COLUMN ServiceSizeID int NOT NULL
GO
ROLLBACK TRANSACTION