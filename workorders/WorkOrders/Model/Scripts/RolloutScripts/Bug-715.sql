ALTER TABLE [Markouts] ADD [MarkoutTypeID] int null
GO

ALTER TABLE [Markouts] Drop Column [Radius]
GO

ALTER TABLE [Markouts] ADD [Note] text null
--------------------------------------------MARKOUT TYPES-------------------------------------------
CREATE TABLE [MarkoutTypes] (
[MarkoutTypeID] int unique identity not null,
[Description] varchar(120) not null, 
[Order] int not null,
CONSTRAINT [PK_MarkoutTypes] PRIMARY KEY CLUSTERED (
[MarkoutTypeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Markouts] WITH NOCHECK ADD CONSTRAINT [FK_Markouts_MarkoutTypes_MarkoutTypeID] FOREIGN KEY (
[MarkoutTypeID]
) REFERENCES [MarkoutTypes] (
[MarkoutTypeID]
)
GO

INSERT INTO [MarkoutTypes] Values('C TO 10FT BHD C',1)
INSERT INTO [MarkoutTypes] Values('C TO 15FT BHD C',2)
INSERT INTO [MarkoutTypes] Values('C TO 20FT BHD C',3)
INSERT INTO [MarkoutTypes] Values('C TO 5FT BHD C',4)
INSERT INTO [MarkoutTypes] Values('C TO C',5)
INSERT INTO [MarkoutTypes] Values('C TO C, 25FT RADIUS OF HYDRANT',6)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD ALL, C''S M/O BEG AT C/L OF INT & EXT 75FT IN ALL DIR',7)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD BOTH C''S',8)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD OPP C',9)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 15FT BHD C',10)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD ALL, C''S M/O BEG AT C/L OF INT & EXT 50FT IN ALL DIR',11)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD C, C TO 20FT BHD OPP C',12)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD C.',13)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD OPP C',14)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 25FT BHD C',15)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 30FT BHD C OPP C',16)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 5FT BHD C',17)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 5FT BHD OPP C',18)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO ENT PROP',19)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO ENT PROP, C TO 10FT BHD OPP C',20)
INSERT INTO [MarkoutTypes] Values('C TO C, ENT PROP, C/L OF S C, C TO 10FT BHD C, C TO 10FT BHD OPP C',21)
INSERT INTO [MarkoutTypes] Values('C TO C. C TO 10FT BHD C',22)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP',23)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP, C TO C',24)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP, C TO C, C TO 10FT BHD OPP C',25)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR',26)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',27)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',28)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',29)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR',30)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',31)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',32)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',33)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR',34)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',35)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',36)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',37)
INSERT INTO [MarkoutTypes] Values('NOT LISTED',38)

GRANT ALL ON [MarkoutTypes] TO MCUser
GO