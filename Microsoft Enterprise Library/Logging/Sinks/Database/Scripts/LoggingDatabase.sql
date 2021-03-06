/****** Object:  Database Logging    Script Date: 10/1/2004 3:16:33 PM ******/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Logging')
	DROP DATABASE [Logging]
GO

CREATE DATABASE [Logging]  ON (NAME = N'Logging', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\Logging.mdf' , SIZE = 1, FILEGROWTH = 10%) LOG ON (NAME = N'Logging_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\Logging_log.LDF' , FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

exec sp_dboption N'Logging', N'autoclose', N'false'
GO

exec sp_dboption N'Logging', N'bulkcopy', N'false'
GO

exec sp_dboption N'Logging', N'trunc. log', N'false'
GO

exec sp_dboption N'Logging', N'torn page detection', N'true'
GO

exec sp_dboption N'Logging', N'read only', N'false'
GO

exec sp_dboption N'Logging', N'dbo use', N'false'
GO

exec sp_dboption N'Logging', N'single', N'false'
GO

exec sp_dboption N'Logging', N'autoshrink', N'false'
GO

exec sp_dboption N'Logging', N'ANSI null default', N'false'
GO

exec sp_dboption N'Logging', N'recursive triggers', N'false'
GO

exec sp_dboption N'Logging', N'ANSI nulls', N'false'
GO

exec sp_dboption N'Logging', N'concat null yields null', N'false'
GO

exec sp_dboption N'Logging', N'cursor close on commit', N'false'
GO

exec sp_dboption N'Logging', N'default to local cursor', N'false'
GO

exec sp_dboption N'Logging', N'quoted identifier', N'false'
GO

exec sp_dboption N'Logging', N'ANSI warnings', N'false'
GO

exec sp_dboption N'Logging', N'auto create statistics', N'true'
GO

exec sp_dboption N'Logging', N'auto update statistics', N'true'
GO

use [Logging]
GO

/****** Object:  Stored Procedure dbo.WriteLog    Script Date: 10/1/2004 3:16:34 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WriteLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WriteLog]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 10/1/2004 3:16:34 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Log]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Log]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 10/1/2004 3:16:35 PM ******/
CREATE TABLE [dbo].[Log] (
	[LogID] [int] IDENTITY (1, 1) NOT NULL ,
	[EventID] [int] NULL ,
	[Category] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Priority] [int] NOT NULL ,
	[Severity] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Title] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Timestamp] [datetime] NOT NULL ,
	[MachineName] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[AppDomainName] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ProcessID] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ProcessName] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ThreadName] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Win32ThreadId] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Message] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[FormattedMessage] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.WriteLog    Script Date: 10/1/2004 3:16:36 PM ******/

CREATE PROCEDURE WriteLog
(
	@EventID int, 
	@Category nvarchar(64),
	@Priority int, 
	@Severity nvarchar(32), 
	@Title nvarchar(256), 
	@Timestamp datetime,
	@MachineName nvarchar(32), 
	@AppDomainName nvarchar(2048),
	@ProcessID nvarchar(256),
	@ProcessName nvarchar(2048),
	@ThreadName nvarchar(2048),
	@Win32ThreadId nvarchar(128),
	@Message nvarchar(2048),
	@FormattedMessage ntext
)
AS 

	INSERT INTO [Log] (
		EventID,
		Category,
		Priority,
		Severity,
		Title,
		[Timestamp],
		MachineName,
		AppDomainName,
		ProcessID,
		ProcessName,
		ThreadName,
		Win32ThreadId,
		Message,
		FormattedMessage
	)
	VALUES (
		@EventID, 
		@Category, 
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Message,
		@FormattedMessage)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

