USE [master]
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'workorderstest')
BEGIN
	ALTER DATABASE [workorderstest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
END

/****** Object:  Database [workorderstest]    Script Date: 02/06/2013 14:22:25 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'workorderstest')
BEGIN
DROP DATABASE [workorderstest]
END
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'workorderstest')
BEGIN
CREATE DATABASE [workorderstest] 
END
