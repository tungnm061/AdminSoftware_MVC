USE [master]
GO
/****** Object:  Database [DongLucDb]    Script Date: 01/09/2020 15:04:41 ******/
CREATE DATABASE [DongLucDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DongLucDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLTUNGNM\MSSQL\DATA\DongLucDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DongLucDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLTUNGNM\MSSQL\DATA\DongLucDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DongLucDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DongLucDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DongLucDb] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [DongLucDb] SET ANSI_NULLS ON 
GO
ALTER DATABASE [DongLucDb] SET ANSI_PADDING ON 
GO
ALTER DATABASE [DongLucDb] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [DongLucDb] SET ARITHABORT ON 
GO
ALTER DATABASE [DongLucDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DongLucDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DongLucDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DongLucDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DongLucDb] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [DongLucDb] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [DongLucDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DongLucDb] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [DongLucDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DongLucDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DongLucDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DongLucDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DongLucDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DongLucDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DongLucDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DongLucDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DongLucDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DongLucDb] SET RECOVERY FULL 
GO
ALTER DATABASE [DongLucDb] SET  MULTI_USER 
GO
ALTER DATABASE [DongLucDb] SET PAGE_VERIFY NONE  
GO
ALTER DATABASE [DongLucDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DongLucDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DongLucDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DongLucDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DongLucDb] SET QUERY_STORE = OFF
GO
USE [DongLucDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [DongLucDb]
GO
/****** Object:  Schema [hrm]    Script Date: 01/09/2020 15:04:41 ******/
CREATE SCHEMA [hrm]
GO
/****** Object:  Schema [kpi]    Script Date: 01/09/2020 15:04:41 ******/
CREATE SCHEMA [kpi]
GO
/****** Object:  Schema [sale]    Script Date: 01/09/2020 15:04:41 ******/
CREATE SCHEMA [sale]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_Get_Root_Element_Name]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_Get_Root_Element_Name]
(
	-- Add the parameters for the function here
	@XML NVARCHAR(MAX)
)
RETURNS VARCHAR(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(100),
			@FirstIndex INT,
			@Length INT;

	-- Add the T-SQL statements to compute the return value here
	
	SET @FirstIndex = CHARINDEX('<', @XML) + 1;
	SET @Length  = CHARINDEX('>', @XML) - @FirstIndex;

	SET @Result = REPLACE(SUBSTRING(@XML, @FirstIndex, @Length), ' ', '');

	-- Return the result of the function
	RETURN @Result;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_Replace_XmlChars]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_Replace_XmlChars]

	(@XML							NVARCHAR(MAX))

RETURNS								NVARCHAR(MAX)

AS

BEGIN	

	IF ISNULL(@XML,'') = ''
		RETURN ''

	SET	@XML						= REPLACE( REPLACE( REPLACE( @XML, '<?xml version="1.0" encoding="utf-16"?>', ''), 'xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"', ''), 'xmlns:xsd="http://www.w3.org/2001/XMLSchema"', '')
	SET @XML						= REPLACE(@XML, '<?xml version="1.0" encoding="utf-8" ?>','')
	SET @XML						= REPLACE(@XML, 'xsi:nil="true"','')
	SET	@XML						= REPLACE( @XML, '–N','-N')

	RETURN @XML
	
END
GO
/****** Object:  Table [dbo].[User]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserGroupId] [int] NULL,
	[ModuleGroupId] [int] NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[FullName] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[EmployeeId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Employee](
	[EmployeeId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeCode] [varchar](50) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Gender] [tinyint] NOT NULL,
	[SpecialName] [nvarchar](255) NULL,
	[Avatar] [nvarchar](255) NULL,
	[DepartmentId] [bigint] NOT NULL,
	[CountryId] [int] NOT NULL,
	[NationId] [int] NOT NULL,
	[ReligionId] [int] NOT NULL,
	[MaritalStatus] [tinyint] NULL,
	[CityBirthPlace] [int] NULL,
	[CityNativeLand] [int] NULL,
	[IdentityCardNumber] [varchar](50) NULL,
	[IdentityCardDate] [date] NULL,
	[CityIdentityCard] [int] NULL,
	[PermanentAddress] [nvarchar](255) NULL,
	[PermanentCity] [int] NULL,
	[PermanentDistrict] [int] NULL,
	[TemperaryAddress] [nvarchar](255) NULL,
	[TemperaryCity] [int] NULL,
	[TemperaryDistrict] [int] NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[PositionId] [int] NULL,
	[TrainingLevelId] [int] NULL,
	[HealthStatus] [nvarchar](500) NULL,
	[DateOfYouthUnionAdmission] [date] NULL,
	[PlaceOfYouthUnionAdmission] [nvarchar](500) NULL,
	[DateOfPartyAdmission] [date] NULL,
	[PlaceOfPartyAdmission] [nvarchar](500) NULL,
	[Skill] [nvarchar](1000) NULL,
	[Experience] [nvarchar](1000) NULL,
	[Description] [nvarchar](1000) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ShiftWorkId] [int] NULL,
	[WorkedDate] [date] NULL,
	[EducationLevelId] [int] NULL,
	[SchoolId] [int] NULL,
	[CareerId] [int] NULL,
	[TimeSheetCode] [varchar](50) NULL,
	[DepartmentCompany] [int] NULL,
	[CategoryKpiId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Department]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Department](
	[DepartmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentCode] [varchar](50) NOT NULL,
	[DepartmentName] [nvarchar](255) NOT NULL,
	[ParentId] [bigint] NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[Path] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[UserEmployeeDepartment]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[UserEmployeeDepartment] AS 
SELECT
		u.[UserId],
		u.[UserName],
		e.[EmployeeCode],
		e.[EmployeeId],
		e.[FullName],
		d.[DepartmentId],
		d.[DepartmentCode],
		d.[DepartmentName],
		d.[Path],
		e.[DepartmentCompany],
		e.CategoryKpiId
	FROM [hrm].[Employee] e
	INNER JOIN [dbo].[User] u ON u.[EmployeeId] = e.[EmployeeId]
	INNER JOIN [hrm].[Department] d ON e.[DepartmentId] = d.[DepartmentId]
GO
/****** Object:  Table [kpi].[WorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[WorkPlan](
	[WorkPlanId] [uniqueidentifier] NOT NULL,
	[WorkPlanCode] [varchar](50) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ConfirmedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[ConfirmedDate] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[WorkPlanDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[WorkPlanDetail](
	[WorkPlanDetailId] [uniqueidentifier] NOT NULL,
	[TaskId] [uniqueidentifier] NOT NULL,
	[WorkPlanId] [uniqueidentifier] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Explanation] [nvarchar](max) NULL,
	[UsefulHours] [decimal](18, 1) NULL,
	[WorkingNote] [nvarchar](max) NULL,
	[ApprovedFisnishBy] [int] NULL,
	[ApprovedFisnishDate] [datetime] NULL,
	[FisnishDate] [datetime] NULL,
	[WorkPointType] [varchar](1) NULL,
	[WorkPoint] [decimal](18, 0) NULL,
	[DepartmentFisnishBy] [int] NULL,
	[DepartmentFisnishDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[FileConfirm] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkPlanDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [kpi].[SuggesWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[SuggesWork](
	[SuggesWorkId] [uniqueidentifier] NOT NULL,
	[TaskId] [uniqueidentifier] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[VerifiedBy] [int] NULL,
	[VerifiedDate] [datetime] NULL,
	[UsefulHours] [decimal](18, 1) NULL,
	[WorkingNote] [nvarchar](max) NULL,
	[Explanation] [nvarchar](max) NULL,
	[ApprovedFisnishBy] [int] NULL,
	[WorkPointType] [varchar](1) NULL,
	[ApprovedFisnishDate] [datetime] NULL,
	[FisnishDate] [datetime] NULL,
	[WorkPoint] [decimal](18, 2) NULL,
	[DepartmentFisnishBy] [int] NULL,
	[DepartmentFisnishDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[FileConfirm] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[SuggesWorkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [kpi].[AssignWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[AssignWork](
	[AssignWorkId] [uniqueidentifier] NOT NULL,
	[TaskId] [uniqueidentifier] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[AssignBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[UsefulHours] [decimal](18, 1) NULL,
	[WorkingNote] [nvarchar](max) NULL,
	[Explanation] [nvarchar](max) NULL,
	[ApprovedFisnishBy] [int] NULL,
	[ApprovedFisnishDate] [datetime] NULL,
	[FisnishDate] [datetime] NULL,
	[WorkPointType] [varchar](1) NULL,
	[WorkPoint] [decimal](18, 2) NULL,
	[DepartmentFisnishBy] [int] NULL,
	[DepartmentFisnishDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[FileConfirm] [nvarchar](255) NULL,
	[DepartmentFollowBy] [int] NULL,
	[DirectorFollowBy] [int] NULL,
	[TypeAssignWork] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AssignWorkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [kpi].[Task]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[Task](
	[TaskId] [uniqueidentifier] NOT NULL,
	[TaskCode] [varchar](50) NOT NULL,
	[TaskName] [nvarchar](255) NOT NULL,
	[CalcType] [tinyint] NOT NULL,
	[WorkPointConfigId] [int] NOT NULL,
	[UsefulHours] [decimal](18, 1) NOT NULL,
	[Frequent] [bit] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsSystem] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[GroupName] [nvarchar](500) NULL,
	[CategoryKpiId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[WorkStreamDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[WorkStreamDetail](
	[WorkStreamDetailId] [uniqueidentifier] NOT NULL,
	[TaskId] [uniqueidentifier] NOT NULL,
	[WorkStreamId] [uniqueidentifier] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsDefault] [bit] NOT NULL,
	[UsefulHours] [decimal](18, 1) NULL,
	[VerifiedBy] [int] NULL,
	[VerifiedDate] [datetime] NULL,
	[WorkingNote] [nvarchar](max) NULL,
	[Explanation] [nvarchar](max) NULL,
	[ApprovedFisnishBy] [int] NULL,
	[ApprovedFisnishDate] [datetime] NULL,
	[FisnishDate] [datetime] NULL,
	[WorkPointType] [varchar](1) NULL,
	[WorkPoint] [decimal](18, 2) NULL,
	[DepartmentFisnishBy] [int] NULL,
	[DepartmentFisnishDate] [datetime] NULL,
	[Quantity] [int] NULL,
	[FileConfirm] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkStreamDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [kpi].[WorkDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [kpi].[WorkDetail] AS 
SELECT 
		[TaskId]				= CAST(wp.[TaskId] AS VARCHAR(50)),
		[FromDate]				= wp.[FromDate],
		[ToDate]				= wp.[ToDate],
		[UsefulHours]			= wp.[UsefulHours],
		[Status]				= wp.[Status],
		[WorkDetailId]			= CAST([WorkPlanDetailId] AS VARCHAR(50)),
		[WorkType]				= 1,
		[Description]			= wp.[Description],
		[CreateBy]				= w.[CreateBy],
		[WorkingNote]			= [WorkingNote], 
		[Explanation]			= [Explanation],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateByName]			= u.[FullName],
		[FisnishDate]			= [FisnishDate],
		[ApprovedFisnishBy]		= [ApprovedFisnishBy],
		[ApprovedFisnishDate]	= [ApprovedFisnishDate],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[CreateDate]			= w.[CreateDate],
		[VerifiedBy]			= w.[ApprovedBy],
		[EmployeeId]			= u.[EmployeeId],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointType]			= wp.[WorkPointType],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPoint]				= [WorkPoint],
		[CalcType]				= t.[CalcType],
		[DescriptionTask]		= t.[Description],
		[EmployeeCode]			= u.[EmployeeCode],
		[DepartmentId]			= u.[DepartmentId],
		[Quantity]				= wp.[Quantity],
		[FileConfirm]			= wp.[FileConfirm],
		[DepartmentCompany]		= u.DepartmentCompany
	FROM [kpi].[WorkPlanDetail] wp
	INNER JOIN [kpi].[WorkPlan]  w ON w.[WorkPlanId] = wp.[WorkPlanId]
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = wp.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] =  w.[CreateBy]
	UNION ALL
	SELECT 
		[TaskId]				= CAST(ws.[TaskId] AS VARCHAR(50)),
		[FromDate]				= ws.[FromDate],
		[ToDate]				= ws.[ToDate],
		[UsefulHours]			= ws.[UsefulHours],
		[Status]				= ws.[Status],
		[WorkDetailId]			= CAST([WorkStreamDetailId] AS VARCHAR(50)),
		[WorkType]				= 2,
		[Description]			= ws.[Description],
		[CreateBy]				= ws.[CreateBy],
		[WorkingNote]			= ws.[WorkingNote],
		[Explanation]			= ws.[Explanation],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateByName]			= u.[FullName],
		[FisnishDate]			= [FisnishDate],
		[ApprovedFisnishBy]		= [ApprovedFisnishBy],
		[ApprovedFisnishDate]	= [ApprovedFisnishDate],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[CreateDate]			= ws.[CreateDate],
		[VerifiedBy]			= ws.[VerifiedBy],
		[EmployeeId]			= u.[EmployeeId],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointType]			= ws.[WorkPointType],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPoint]				= [WorkPoint],
		[CalcType]				= t.[CalcType],
		[DescriptionTask]		= t.[Description],
		[EmployeeCode]			= u.[EmployeeCode],
		[DepartmentId]			= u.[DepartmentId],
		[Quantity]				= ws.[Quantity],
		[FileConfirm]			= ws.[FileConfirm],
		[DepartmentCompany]		= u.DepartmentCompany
	 FROM [kpi].[WorkStreamDetail] ws
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = ws.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] =  ws.[CreateBy]
	WHERE ws.[IsDefault] = 0
	UNION ALL
	SELECT 
		[TaskId]				= CAST(sg.[TaskId] AS VARCHAR(50)),
		[FromDate]				= sg.[FromDate],
		[ToDate]				= sg.[ToDate],
		[UsefulHours]			= sg.[UsefulHours],
		[Status]				= sg.[Status],
		[WorkDetailId]			= CAST([SuggesWorkId] AS VARCHAR(50)),
		[WorkType]				= 3,
		[Description]			= sg.[Description],
		[CreateBy]				= sg.[CreateBy],
		[WorkingNote]			= sg.[WorkingNote],
		[Explanation]			= sg.[Explanation],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateByName]			= u.[FullName],
		[FisnishDate]			= [FisnishDate],
		[ApprovedFisnishBy]		= [ApprovedFisnishBy],
		[ApprovedFisnishDate]	= [ApprovedFisnishDate],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[CreateDate]			= sg.[CreateDate],
		[VerifiedBy]			= sg.[VerifiedBy],
		[EmployeeId]			= u.[EmployeeId],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointType]			= sg.[WorkPointType],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPoint]				= [WorkPoint],
		[CalcType]				= t.[CalcType],
		[DescriptionTask]		= t.[Description],
		[EmployeeCode]			= u.[EmployeeCode],
		[DepartmentId]			= u.[DepartmentId],
		[Quantity]				= sg.[Quantity],
		[FileConfirm]			= sg.[FileConfirm],
		[DepartmentCompany]		= u.DepartmentCompany
	FROM [kpi].[SuggesWork] sg
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = sg.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] =  sg.[CreateBy]
	UNION ALL
	SELECT 
		[TaskId]				= CAST(aw.[TaskId] AS VARCHAR(50)),
		[FromDate]				= aw.[FromDate],
		[ToDate]				= aw.[ToDate],
		[UsefulHours]			= aw.[UsefulHours],
		[Status]				= aw.[Status],
		[WorkDetailId]			= CAST([AssignWorkId] AS VARCHAR(50)),
		[WorkType]				= 4,
		[Description]			= aw.[Description],
		[CreateBy]				= aw.[AssignBy],
		[WorkingNote]			= aw.[WorkingNote],
		[Explanation]			= aw.[Explanation],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateByName]			= u.[FullName],
		[FisnishDate]			= [FisnishDate],
		[ApprovedFisnishBy]		= [ApprovedFisnishBy],
		[ApprovedFisnishDate]	= [ApprovedFisnishDate],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[CreateDate]			= aw.[CreateDate],
		[VerifiedBy]			= aw.[CreateBy],
		[EmployeeId]			= u.[EmployeeId],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointType]			= aw.[WorkPointType],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPoint]				= [WorkPoint],
		[CalcType]				= t.[CalcType],
		[DescriptionTask]		= t.[Description],
		[EmployeeCode]			= u.[EmployeeCode],
		[DepartmentId]			= u.[DepartmentId],
		[Quantity]				= aw.[Quantity],
		[FileConfirm]			= aw.[FileConfirm],
		[DepartmentCompany]		= u.DepartmentCompany
	 FROM [kpi].[AssignWork] aw
	 INNER JOIN [kpi].[Task] t ON t.[TaskId] = aw.[TaskId]
	 INNER JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] =  aw.[AssignBy]
GO
/****** Object:  Table [dbo].[AutoNumber]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoNumber](
	[AutoNumberId] [int] IDENTITY(1,1) NOT NULL,
	[Prefix] [varchar](15) NOT NULL,
	[Number] [bigint] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Lenght] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AutoNumberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryKpi](
	[CategoryKpiId] [int] IDENTITY(1,1) NOT NULL,
	[KpiCode] [varchar](50) NOT NULL,
	[KpiName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryKpiId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chat]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chat](
	[ChatId] [varchar](50) NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiptId] [int] NULL,
	[ChatGroupId] [bigint] NULL,
	[SendDate] [datetime] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Chat] PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[City]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityId] [int] NOT NULL,
	[CityName] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [varchar](50) NOT NULL,
	[CountryName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[DistrictId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[DistrictName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[DistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Function]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Function](
	[FunctionId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[FunctionName] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[Controller] [varchar](100) NOT NULL,
	[Area] [varchar](50) NOT NULL,
	[Action] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FunctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[ModuleId] [int] NOT NULL,
	[ModuleGroupId] [int] NOT NULL,
	[ModuleName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleGroup](
	[ModuleGroupId] [int] NOT NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[Icon] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[ActionUrl] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [uniqueidentifier] NOT NULL,
	[FromUser] [nvarchar](100) NOT NULL,
	[ToUserId] [int] NOT NULL,
	[NotificationDate] [datetime] NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[PublishDate] [date] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[PostContent] [ntext] NOT NULL,
	[IsFeature] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rights]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rights](
	[UserId] [int] NOT NULL,
	[FunctionId] [int] NOT NULL,
	[IsView] [bit] NOT NULL,
	[IsCreate] [bit] NOT NULL,
	[IsEdit] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FunctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroup](
	[UserGroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupCode] [varchar](50) NOT NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroupRights]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroupRights](
	[UserGroupId] [int] NOT NULL,
	[FunctionId] [int] NOT NULL,
	[IsView] [bit] NOT NULL,
	[IsCreate] [bit] NOT NULL,
	[IsEdit] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ward]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ward](
	[WardId] [int] NOT NULL,
	[WardName] [nvarchar](255) NOT NULL,
	[DistrictId] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[WardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Applicant]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Applicant](
	[ApplicantId] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Sex] [tinyint] NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[CountryId] [int] NULL,
	[NationId] [int] NULL,
	[ReligionId] [int] NULL,
	[CityBirthPlace] [int] NOT NULL,
	[PermanentAddress] [nvarchar](255) NOT NULL,
	[IdentityCardNumber] [varchar](50) NULL,
	[PhoneNumber] [varchar](50) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[ChanelId] [int] NOT NULL,
	[TrainingLevelId] [int] NOT NULL,
	[RecruitPlanId] [bigint] NOT NULL,
	[CvDate] [date] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Career]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Career](
	[CareerId] [int] IDENTITY(1,1) NOT NULL,
	[CareerName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[CareerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Contract](
	[ContractId] [uniqueidentifier] NOT NULL,
	[ContractCode] [varchar](50) NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[ContractTypeId] [int] NOT NULL,
	[ContractFile] [nvarchar](255) NULL,
	[ContractOthorFile] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[ContractType]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[ContractType](
	[ContractTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[EducationLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[EducationLevel](
	[EducationLevelId] [int] IDENTITY(1,1) NOT NULL,
	[LevelCode] [varchar](50) NOT NULL,
	[LevelName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[EducationLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[EmployeeHoliday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[EmployeeHoliday](
	[EmployeeHolidayId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[HolidayReasonId] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeHolidayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Holiday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Holiday](
	[HolidayId] [uniqueidentifier] NOT NULL,
	[HolidayDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HolidayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[HolidayConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[HolidayConfig](
	[EmployeeId] [bigint] NOT NULL,
	[Year] [int] NOT NULL,
	[HolidayNumber] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[HolidayDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[HolidayDetail](
	[HolidayDetailId] [uniqueidentifier] NOT NULL,
	[DateDay] [datetime] NOT NULL,
	[NumberDays] [decimal](18, 1) NOT NULL,
	[Permission] [decimal](18, 1) NOT NULL,
	[PercentSalary] [decimal](18, 2) NOT NULL,
	[ToTalDays] [decimal](18, 2) NOT NULL,
	[EmployeeHolidayId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HolidayDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[HolidayReason]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[HolidayReason](
	[HolidayReasonId] [int] IDENTITY(1,1) NOT NULL,
	[ReasonCode] [varchar](50) NOT NULL,
	[ReasonName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[PercentSalary] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[HolidayReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[IncurredSalary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[IncurredSalary](
	[IncurredSalaryId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[Amount] [money] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[SubmitDate] [date] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[IncurredSalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Insurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Insurance](
	[InsuranceId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[InsuranceNumber] [varchar](50) NOT NULL,
	[SubscriptionDate] [date] NOT NULL,
	[CityId] [int] NOT NULL,
	[MonthBefore] [int] NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InsuranceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[InsuranceMedical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[InsuranceMedical](
	[InsuranceMedicalId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[InsuranceMedicalNumber] [varchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[ExpiredDate] [date] NOT NULL,
	[CityId] [int] NOT NULL,
	[MedicalId] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InsuranceMedicalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[InsuranceProcess]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[InsuranceProcess](
	[InsuranceProcessId] [uniqueidentifier] NOT NULL,
	[InsuranceId] [bigint] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NULL,
	[Amount] [money] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InsuranceProcessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[JobChange]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[JobChange](
	[JobChangeId] [uniqueidentifier] NOT NULL,
	[JobChangeCode] [varchar](50) NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[FromDepartmentId] [bigint] NOT NULL,
	[ToDepartmentId] [bigint] NOT NULL,
	[FromPositionId] [int] NULL,
	[ToPositionId] [int] NULL,
	[Reason] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[JobChangeFile] [nvarchar](255) NULL,
	[JobChangeNumber] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[JobChangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[LocationEmployee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[LocationEmployee](
	[LocationEmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[DepartmentId] [bigint] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LocationEmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Maternity]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Maternity](
	[MaternityId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NOT NULL,
	[StartTime] [varchar](50) NOT NULL,
	[EndTime] [varchar](50) NOT NULL,
	[RelaxStartTime] [varchar](50) NOT NULL,
	[RelaxEndTime] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaternityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Medical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Medical](
	[MedicalId] [int] IDENTITY(1,1) NOT NULL,
	[MedicalName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MedicalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Mission]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Mission](
	[MissionId] [int] IDENTITY(1,1) NOT NULL,
	[MissionName] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[LocationEmployeeId] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[MissionDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[MissionDetail](
	[MissionDetailId] [uniqueidentifier] NOT NULL,
	[MissionId] [int] NOT NULL,
	[TaskId] [uniqueidentifier] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Description] [nvarchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Nation]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Nation](
	[NationId] [int] IDENTITY(1,1) NOT NULL,
	[NationName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[NationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Position]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Position](
	[PositionId] [int] IDENTITY(1,1) NOT NULL,
	[PositionCode] [varchar](50) NOT NULL,
	[PositionName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[PositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[PraiseDiscipline]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[PraiseDiscipline](
	[PraiseDisciplineId] [uniqueidentifier] NOT NULL,
	[PraiseDisciplineCode] [varchar](50) NOT NULL,
	[PraiseDisciplineType] [tinyint] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[DecisionNumber] [varchar](50) NULL,
	[PraiseDisciplineDate] [date] NOT NULL,
	[Formality] [nvarchar](255) NOT NULL,
	[Reason] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PraiseDisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[PraiseDisciplineDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[PraiseDisciplineDetail](
	[PraiseDisciplineId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[RecruitChanel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[RecruitChanel](
	[RecruitChanelId] [int] IDENTITY(1,1) NOT NULL,
	[ChanelName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecruitChanelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[RecruitPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[RecruitPlan](
	[RecruitPlanId] [bigint] IDENTITY(1,1) NOT NULL,
	[RecruitPlanCode] [varchar](50) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[DepartmentId] [bigint] NOT NULL,
	[PositionId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NOT NULL,
	[Requirements] [nvarchar](max) NULL,
	[ChanelIds] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[RecruitPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [hrm].[RecruitResult]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[RecruitResult](
	[RecruitResultId] [uniqueidentifier] NOT NULL,
	[ApplicantId] [uniqueidentifier] NOT NULL,
	[RecruitPlanId] [bigint] NOT NULL,
	[Result] [tinyint] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[EmployeeId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[RecruitResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[RecruitResultDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[RecruitResultDetail](
	[RecruitResultDetailId] [uniqueidentifier] NOT NULL,
	[RecruitResultId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[Result] [tinyint] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[InterviewDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecruitResultDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Religion]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Religion](
	[ReligionId] [int] IDENTITY(1,1) NOT NULL,
	[ReligionName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReligionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[Salary](
	[SalaryId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[BasicSalary] [money] NOT NULL,
	[BasicCoefficient] [decimal](18, 2) NOT NULL,
	[ProfessionalCoefficient] [decimal](18, 2) NOT NULL,
	[ResponsibilityCoefficient] [decimal](18, 2) NOT NULL,
	[PercentProfessional] [decimal](18, 1) NOT NULL,
	[ApplyDate] [date] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[School]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[School](
	[SchoolId] [int] IDENTITY(1,1) NOT NULL,
	[SchoolName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[SchoolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[ShiftWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[ShiftWork](
	[ShiftWorkId] [int] IDENTITY(1,1) NOT NULL,
	[ShiftWorkCode] [varchar](50) NOT NULL,
	[StartTime] [varchar](50) NOT NULL,
	[EndTime] [varchar](50) NOT NULL,
	[RelaxStartTime] [varchar](50) NULL,
	[RelaxEndTime] [varchar](50) NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ShiftWorkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[TimeSheet]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[TimeSheet](
	[TimeSheetId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[TimeSheetDate] [date] NOT NULL,
	[Checkin] [datetime] NULL,
	[Checkout] [datetime] NULL,
	[ShiftWorkId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TimeSheetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [hrm].[TimeSheetOt]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[TimeSheetOt](
	[TimeSheetOtId] [uniqueidentifier] NOT NULL,
	[DayDate] [datetime] NOT NULL,
	[EmployeeId] [bigint] NOT NULL,
	[Hours] [decimal](18, 2) NOT NULL,
	[CoefficientPoint] [decimal](18, 2) NOT NULL,
	[DayPoints] [decimal](18, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[TimeSheetOtId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [hrm].[TrainingLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [hrm].[TrainingLevel](
	[TrainingLevelId] [int] IDENTITY(1,1) NOT NULL,
	[LevelCode] [varchar](50) NOT NULL,
	[LevelName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[TrainingLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[AcceptConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[AcceptConfig](
	[AcceptConfigId] [int] IDENTITY(1,1) NOT NULL,
	[AcceptType] [varchar](50) NOT NULL,
	[AcceptPointMin] [decimal](18, 2) NOT NULL,
	[AcceptPointMax] [decimal](18, 2) NOT NULL,
	[AcceptConditionMin] [decimal](18, 1) NULL,
	[AcceptConditionMax] [decimal](18, 1) NULL,
PRIMARY KEY CLUSTERED 
(
	[AcceptConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[Complain]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[Complain](
	[ComplainId] [uniqueidentifier] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[AccusedBy] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ConfirmedBy] [int] NULL,
	[ConfirmedDate] [datetime] NULL,
	[Status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ComplainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [kpi].[FactorConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[FactorConfig](
	[FactorConfigId] [int] IDENTITY(1,1) NOT NULL,
	[FactorType] [varchar](50) NOT NULL,
	[FactorPointMin] [decimal](18, 2) NOT NULL,
	[FactorPointMax] [decimal](18, 2) NOT NULL,
	[FactorConditionMin] [decimal](18, 2) NULL,
	[FactorConditionMax] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[FactorConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[FinishJobConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[FinishJobConfig](
	[FinishConfigId] [int] IDENTITY(1,1) NOT NULL,
	[FinishType] [varchar](50) NOT NULL,
	[FinishPointMin] [decimal](18, 2) NOT NULL,
	[FinishPointMax] [decimal](18, 2) NOT NULL,
	[FinishConditionMin] [decimal](18, 2) NULL,
	[FinishConditionMax] [decimal](18, 2) NULL,
	[IsRequired] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[FinishConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[KpiConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[KpiConfig](
	[KpiConfigId] [int] IDENTITY(1,1) NOT NULL,
	[MinHours] [decimal](18, 1) NOT NULL,
	[MaxHours] [decimal](18, 1) NOT NULL,
	[PlanningDay] [tinyint] NOT NULL,
	[PlanningHourMax] [varchar](50) NOT NULL,
	[PlanningHourMin] [varchar](50) NOT NULL,
	[HourConfirmMax] [varchar](50) NOT NULL,
	[HourConfirmMin] [varchar](50) NOT NULL,
	[Notification] [decimal](18, 3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[KpiConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[Performer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[Performer](
	[PerformerBy] [int] NOT NULL,
	[WorkStreamId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PerformerBy] ASC,
	[WorkStreamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[SuggestJobConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[SuggestJobConfig](
	[JobConfigId] [int] IDENTITY(1,1) NOT NULL,
	[JobType] [varchar](50) NOT NULL,
	[JobPointMin] [decimal](18, 2) NOT NULL,
	[JobPointMax] [decimal](18, 2) NOT NULL,
	[JobConditionMin] [decimal](18, 1) NULL,
	[JobConditionMax] [decimal](18, 1) NULL,
PRIMARY KEY CLUSTERED 
(
	[JobConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [kpi].[WorkPointConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[WorkPointConfig](
	[WorkPointConfigId] [int] IDENTITY(1,1) NOT NULL,
	[WorkPointName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[WorkPointA] [decimal](18, 2) NOT NULL,
	[WorkPointB] [decimal](18, 2) NOT NULL,
	[WorkPointC] [decimal](18, 2) NOT NULL,
	[WorkPointD] [decimal](18, 2) NOT NULL,
	[WorkPointE] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkPointConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [kpi].[WorkStream]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [kpi].[WorkStream](
	[WorkStreamId] [uniqueidentifier] NOT NULL,
	[WorkStreamCode] [nvarchar](50) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[AssignWorkId] [uniqueidentifier] NULL,
	[TaskId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [tinyint] NOT NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkStreamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[Contract](
	[ContractId] [bigint] IDENTITY(1,1) NOT NULL,
	[ContractCode] [varchar](50) NOT NULL,
	[CustomerId] [bigint] NOT NULL,
	[EmployeeId] [int] NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[TotalPrice] [decimal](18, 0) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [int] NULL,
	[ContractNumber] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[ContractDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[ContractDetail](
	[ContractDetailId] [uniqueidentifier] NOT NULL,
	[ContractId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ContractDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[Customer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [varchar](50) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[IdentityCardDate] [date] NULL,
	[CityIdentityCard] [int] NULL,
	[IdentityCard] [varchar](50) NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [varchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Address] [nvarchar](255) NULL,
	[CityId] [int] NULL,
	[DistrictId] [int] NULL,
	[TaxCode] [varchar](50) NULL,
	[CompanyName] [varchar](255) NULL,
	[BankAccountNumber] [varchar](50) NULL,
	[Status] [tinyint] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[EmployeeInvestor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[EmployeeInvestor](
	[EmployeeInvestorId] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[SpecialName] [nvarchar](255) NULL,
	[TrainingLevelId] [int] NULL,
	[MaritalStatus] [tinyint] NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gender] [tinyint] NULL,
	[EducationLevelId] [int] NULL,
	[Position] [nvarchar](255) NULL,
	[Phone] [varchar](50) NULL,
	[IdentityCardNumber] [int] NULL,
	[Status] [int] NULL,
	[Description] [nvarchar](1000) NULL,
	[Address] [nvarchar](255) NULL,
	[CityId] [int] NULL,
	[DistrictId] [int] NULL,
	[Skill] [nvarchar](1000) NULL,
	[Experience] [nvarchar](1000) NULL,
	[Email] [nvarchar](255) NULL,
	[InvestorId] [uniqueidentifier] NOT NULL,
	[CreateBy] [int] NULL,
	[CreateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeInvestorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[Investor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[Investor](
	[InvestorId] [uniqueidentifier] NOT NULL,
	[InvestorCode] [varchar](50) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Company] [nvarchar](255) NULL,
	[CompanyAddress] [nvarchar](500) NULL,
	[Address] [nvarchar](500) NOT NULL,
	[CityId] [int] NOT NULL,
	[DistrictId] [int] NOT NULL,
	[Position] [varchar](255) NOT NULL,
	[MsEnterprise] [varchar](50) NOT NULL,
	[FoundedYear] [int] NOT NULL,
	[CharterCapital] [decimal](18, 2) NULL,
	[Status] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[InvestorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[Product]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[Product](
	[ProductId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](50) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [sale].[Project]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sale].[Project](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[ProjectCode] [varchar](50) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Status] [int] NOT NULL,
	[InvestorId] [uniqueidentifier] NOT NULL,
	[FromDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[ToDate] [datetime] NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [hrm].[ContractType] ADD  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  StoredProcedure [dbo].[Delete_CategoryKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Delete_CategoryKpi]
	@CategoryKpiId		INT
AS

BEGIN
	DELETE [dbo].[CategoryKpi]	WHERE [CategoryKpiId]	= @CategoryKpiId
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Country]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CReate PROCEDURE [dbo].[Delete_Country]
	@CountryId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [dbo].[Country]	WHERE CountryId = @CountryId
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Post]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Delete_Post]
	@PostId		INT
AS

BEGIN
	DELETE [dbo].[Post]	WHERE [PostId]	= @PostId
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_UserGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Delete_UserGroup]
	-- Add the parameters for the stored procedure here
	@UserGroupId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	DELETE FROM  [dbo].[UserGroup] WHERE [UserGroupId] = @UserGroupId;
END
GO
/****** Object:  StoredProcedure [dbo].[Generate_AutoNumber]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generate_AutoNumber]
	@Prefix			VARCHAR(50),
	@GenerateCode	VARCHAR(50) OUTPUT
AS
BEGIN
	SET @GenerateCode = @Prefix;
	IF NOT EXISTS (SELECT AutoNumberId FROM dbo.AutoNumber WHERE Prefix = @Prefix)
		BEGIN
			INSERT INTO dbo.AutoNumber
			(
				[Prefix],
				[Number],
				[Description],
				[Lenght]
			)
			VALUES
			(
				@Prefix,
				1,
				'',
				6
			)
			SET @GenerateCode = @GenerateCode + '000001';
		END
	ELSE
		BEGIN
			DECLARE @Index INT = 0
			DECLARE @MaxLenght	INT = ISNULL((SELECT Lenght FROM dbo.AutoNumber WHERE Prefix = @Prefix),6)
			DECLARE @Number		BIGINT = ISNULL((SELECT Number FROM dbo.AutoNumber WHERE Prefix = @Prefix),0)
			SET @Number = @Number + 1
			DECLARE @Lenght		INT = @MaxLenght - LEN(CAST(@Number AS VARCHAR(50)))
			IF(@Lenght < @MaxLenght)
				BEGIN
					WHILE @Index < @Lenght
						BEGIN
							SET @Index = @Index + 1;
							SET @GenerateCode = @GenerateCode + '0'
						END
					SET @GenerateCode = @GenerateCode + CAST(@Number AS VARCHAR)
				END
			ELSE
				BEGIN
					SET @GenerateCode = @GenerateCode + CAST(@Number AS VARCHAR)
				END
		END
	UPDATE dbo.AutoNumber SET Number = Number + 1 WHERE Prefix = @Prefix

	SELECT @GenerateCode
END
GO
/****** Object:  StoredProcedure [dbo].[Get_CategoryKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Get_CategoryKpi]
	@CategoryKpiId INT
AS
BEGIN
	SELECT
		[CategoryKpiId],
		[KpiCode],
		[KpiName],
		[Description],
		[CreateDate],
		[CreateBy]
	FROM [dbo].[CategoryKpi]
	WHERE [CategoryKpiId] = @CategoryKpiId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_CategoryKpis]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [dbo].[Get_CategoryKpis]
AS
BEGIN
	SELECT
		[CategoryKpiId],
		[KpiCode],
		[KpiName],
		[Description],
		[CreateDate],
		[CreateBy]
	FROM [dbo].[CategoryKpi]
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Chats]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Get_Chats]
	@CurrentPage	INT,
	@PageSize		INT,
	@RecordDisplay	INT,
	@TotalRecord	BIGINT OUTPUT,
	@SenderId		INT,
	@ReceiptId		INT,
	@ChatGroupId	BIGINT
AS
BEGIN
	SELECT
		*
	FROM (
		SELECT
			[RowNumber] = ROW_NUMBER() OVER(ORDER BY [SendDate] DESC),
			[ChatId],
			[SenderId],
			[ReceiptId],
			[ChatGroupId],
			[SendDate],
			[Message],
			[UserName]
		FROM [dbo].[Chat] c
		INNER JOIN [dbo].[User] u ON u.[UserId] = c.[SenderId]
		WHERE 
				[SendDate] > DATEADD(MONTH,-1,GETDATE())
		--		[SenderId] = ISNULL(@SenderId,[SenderId])
		--	AND
		--		[ReceiptId]	= ISNULL(@ReceiptId,[ReceiptId])
		--	AND
		--		[ChatGroupId] = ISNULL(@ChatGroupId,[ChatGroupId])
	) chat WHERE chat.RowNumber BETWEEN ((@CurrentPage - 1)*@RecordDisplay + 1) AND @CurrentPage*@RecordDisplay
	ORDER BY chat.[SendDate] ASC

	SELECT 
		@TotalRecord = COUNT([ChatId]) 
	FROM [dbo].[Chat]
	WHERE 
			[SendDate] > DATEADD(MONTH,-1,GETDATE())
	--		[SenderId] = ISNULL(@SenderId,[SenderId])
	--	AND
	--		[ReceiptId]	= ISNULL(@ReceiptId,[ReceiptId])
	--	AND
	--		[ChatGroupId] = ISNULL(@ChatGroupId,[ChatGroupId])
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Cities]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Cities]
	-- Add the parameters for the stored procedure here  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[CityId],
		[CityName],
		[Description]
	FROM [dbo].[City]  
	ORDER BY [CityName] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Countries]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Countries]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[CountryId],
		[CountryCode],
		[CountryName],
		[Description]
	FROM [dbo].[Country]
	ORDER BY [CountryCode] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Country]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Country]
	@CountryId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[CountryId],
		[CountryCode],
		[CountryName],
		[Description]
	FROM [dbo].[Country]
	WHERE [CountryId] = @CountryId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Country_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Country_ByCode]
	@CountryCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[CountryId],
		[CountryCode],
		[CountryName],
		[Description]
	FROM [dbo].[Country]
	WHERE CountryCode = @CountryCode
END
GO
/****** Object:  StoredProcedure [dbo].[Get_District_ByCityId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_District_ByCityId]
	-- Add the parameters for the stored procedure here 
	@CityId	INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[DistrictId] ,
		[CityId],
		[DistrictName],
		[Description]
	FROM [dbo].[District]
	WHERE [CityId] = @CityId
	ORDER BY [DistrictName] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Districts]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Districts]
	-- Add the parameters for the stored procedure here 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT 
		[DistrictId],
		[CityId],
		[DistrictName],
		[Description]
	FROM [dbo].[District]
	ORDER BY [DistrictName] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Generate_AutoNumber]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_Generate_AutoNumber]
	@Prefix			VARCHAR(50),
	@GenerateCode	VARCHAR(50) OUTPUT
AS
BEGIN
	SET @GenerateCode = @Prefix;
	IF NOT EXISTS (SELECT AutoNumberId FROM dbo.AutoNumber WHERE Prefix = @Prefix)
		BEGIN
			INSERT INTO dbo.AutoNumber
			(
				[Prefix],
				[Number],
				[Description],
				[Lenght]
			)
			VALUES
			(
				@Prefix,
				0,
				'',
				6
			)
			SET @GenerateCode = @GenerateCode + '000001';
		END
	ELSE
		BEGIN
			DECLARE @Index INT = 0
			DECLARE @MaxLenght	INT = ISNULL((SELECT Lenght FROM dbo.AutoNumber WHERE Prefix = @Prefix),6)
			DECLARE @Number		BIGINT = ISNULL((SELECT Number FROM dbo.AutoNumber WHERE Prefix = @Prefix),0)
			SET @Number = @Number + 1
			DECLARE @Lenght		INT = @MaxLenght - LEN(CAST(@Number AS VARCHAR(50)))
			IF(@Lenght < @MaxLenght)
				BEGIN
					WHILE @Index < @Lenght
						BEGIN
							SET @Index = @Index + 1;
							SET @GenerateCode = @GenerateCode + '0'
						END
					SET @GenerateCode = @GenerateCode + CAST(@Number AS VARCHAR)
				END
			ELSE
				BEGIN
					SET @GenerateCode = @GenerateCode + CAST(@Number AS VARCHAR)
				END
		END

	SELECT @GenerateCode
END
GO
/****** Object:  StoredProcedure [dbo].[Get_ModuleGroup_ByUserId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_ModuleGroup_ByUserId]
	@UserId		INT,
	@RoleId		INT
AS
BEGIN
	SET NOCOUNT ON;
	IF @RoleId = 1
		BEGIN
			SELECT
				[ModuleGroupId],
				[GroupName],
				[Icon],
				[SortOrder],
				[ActionUrl]
			FROM [dbo].[ModuleGroup]
			ORDER BY [SortOrder] ASC
		END
	ELSE
		BEGIN
			SELECT
				[ModuleGroupId],
				[GroupName],
				[Icon],
				[SortOrder],
				[ActionUrl]
			FROM [dbo].[ModuleGroup]
			WHERE [ModuleGroupId] IN (
				SELECT DISTINCT [ModuleGroupId] FROM [dbo].[Module] WHERE [ModuleId] IN (
					SELECT DISTINCT [ModuleId] FROM [dbo].[Function] WHERE [FunctionId] IN (
						SELECT DISTINCT [FunctionId] FROM [dbo].[Rights] WHERE [UserId] = @UserId AND [IsView] = 1
					)
				)
			)
			ORDER BY [SortOrder] ASC
		END
END
GO
/****** Object:  StoredProcedure [dbo].[Get_ModuleGroups]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_ModuleGroups]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ModuleGroupId],
		[GroupName],
		[Icon],
		[SortOrder],
		[ActionUrl]
	FROM [dbo].[ModuleGroup]
	ORDER BY [SortOrder] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Notifications]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Notifications]
	@UserId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT TOP 100
		[NotificationId]	= CAST([NotificationId] AS VARCHAR(50)),
		[FromUser],
		[ToUserId],
		[NotificationDate],
		[Message]
	FROM [dbo].[Notification]
	WHERE [ToUserId] = @UserId
	ORDER BY [NotificationDate] DESC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Post]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Get_Post]
	@PostId INT
AS
BEGIN
	SELECT
		[PostId],
		[Title],
		[PublishDate],
		[CreateDate],
		[CreateBy],
		[PostContent],
		[IsFeature]
	FROM [dbo].[Post]
	WHERE [PostId] = @PostId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Posts]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [dbo].[Get_Posts]
AS
BEGIN
	SELECT
		[PostId],
		[Title],
		[PublishDate],
		[CreateDate],
		[CreateBy],
		[PostContent],
		[IsFeature]
	FROM [dbo].[Post]
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Rights_FunctionName]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [dbo].[Get_Rights_FunctionName]
	@UserId			INT,
	@Controller		VARCHAR(100),
	@Area			VARCHAR(50)
AS
BEGIN
	SELECT
		[UserId],
		[FunctionId]		= r.[FunctionId],
		[IsView],
		[IsCreate],
		[IsEdit],
		[IsDelete]
	FROM [dbo].[Rights] r
	INNER JOIN [dbo].[Function] f ON ( f.Controller = @Controller AND f.Area = @Area)
	WHERE r.[UserId] = @UserId AND r.FunctionId = f.FunctionId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_User]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_User]
	-- Add the parameters for the stored procedure here
	@UserId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		u.[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		u.[UserName],
		[Password],
		u.[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		ued.[EmployeeId],
		ued.[DepartmentId],
		[CreateByName] = ued.[FullName],
		[CreateByCode] = ued.[EmployeeCode],
		ued.[FullName],
		ued.[DepartmentCompany],
		ued.CategoryKpiId

	FROM dbo.[User] u
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = u.[UserId]
	WHERE u.[UserId] = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_User_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_User_ByEmployeeId]
	-- Add the parameters for the stored procedure here
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		[UserName],
		[Password],
		[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		[EmployeeId]
	FROM [dbo].[User]
	WHERE [EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_User_ByUserGroupId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_User_ByUserGroupId]
	-- Add the parameters for the stored procedure here
	@UserGroupId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		[UserName],
		[Password],
		[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		[EmployeeId]
	FROM [dbo].[User] 
	WHERE [UserGroupId]	= @UserGroupId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_User_ByUserName]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_User_ByUserName]
	-- Add the parameters for the stored procedure here
	@UserName	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		[UserName],
		[Password],
		[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		[EmployeeId]
	FROM [dbo].[User]
	WHERE [UserName] = @UserName
END
GO
/****** Object:  StoredProcedure [dbo].[Get_User_For_CheckWorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_User_For_CheckWorkPlan]
	-- Add the parameters for the stored procedure here
	@Date		DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		u.[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		u.[UserName],
		[Password],
		u.[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		ued.[EmployeeId],
		ued.[DepartmentId]
	FROM dbo.[User] u
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = u.[UserId]
	WHERE u.[UserId] NOT IN (SELECT DISTINCT [CreateBy] FROM [kpi].[WorkPlan] WHERE [FromDate] >= @Date AND [ToDate] > @Date)
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserByDepartmentId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_UserByDepartmentId]
	-- Add the parameters for the stored procedure here
	@Path	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserId]			= u.[UserId],
		[UserName]			= u.[UserName],
		[FullName]			= u.[FullName],
		[EmployeeId]		= u.[EmployeeId],
		[EmployeeCode]		= ued.[EmployeeCode]
	FROM [dbo].[User] u
	LEFT JOIN [dbo].[UserEmployeeDepartment]  ued ON ued.UserId = u.[UserId]
	WHERE ued.[Path] LIKE ''+@Path+'%'
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_UserGroup]
	-- Add the parameters for the stored procedure here
	@UserGroupId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserGroupId],
		[GroupName],
		[Description],
		[GroupCode]
	FROM [dbo].[UserGroup]
	WHERE [UserGroupId] = @UserGroupId
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserGroup_ByGroupCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_UserGroup_ByGroupCode]
	-- Add the parameters for the stored procedure here
	@GroupCode	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserGroupId],
		[GroupName],
		[Description],
		[GroupCode]
	FROM [dbo].[UserGroup]
	WHERE [GroupCode] = @GroupCode
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserGroup_Rights_Authority]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_UserGroup_Rights_Authority]
	@UserGroupId			INT
AS
BEGIN
	SELECT
		[UserGroupId]	= @UserGroupId,
		[FunctionId]	= f.[FunctionId],
		[IsView]		= ISNULL((SELECT [IsView] FROM [dbo].[UserGroupRights] WHERE [FunctionId] = f.[FunctionId] AND [UserGroupId] = @UserGroupId),CAST(0 AS BIT)),
		[IsCreate]		= ISNULL((SELECT [IsCreate] FROM [dbo].[UserGroupRights] WHERE [FunctionId] = f.[FunctionId] AND [UserGroupId] = @UserGroupId),CAST(0 AS BIT)),
		[IsEdit]		= ISNULL((SELECT [IsEdit] FROM [dbo].[UserGroupRights] WHERE [FunctionId] = f.[FunctionId] AND [UserGroupId] = @UserGroupId),CAST(0 AS BIT)),
		[IsDelete]		= ISNULL((SELECT [IsDelete] FROM [dbo].[UserGroupRights] WHERE [FunctionId] = f.[FunctionId] AND [UserGroupId] = @UserGroupId),CAST(0 AS BIT)),
		[FunctionName]	= f.[FunctionName],
		[ModuleId]		= f.[ModuleId],
		[FSortOrder]	= f.[SortOrder],
		[Controller]	= f.[Controller],
		[Area]			= f.[Area],
		[Action]		= f.[Action],
		[ModuleName]	= m.[ModuleName],
		[MSortOrder]	= m.[SortOrder],
		[GroupName]		= mg.[GroupName]
	FROM [dbo].[Function] f
	INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
	INNER JOIN [dbo].[ModuleGroup] mg ON mg.[ModuleGroupId] = m.[ModuleGroupId]
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserGroups]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_UserGroups]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		[UserGroupId],
		[GroupName],
		[Description],
		[GroupCode]
	FROM [dbo].[UserGroup]
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRights]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_UserRights]
	@UserId			INT,
	@RoleId			INT,
	@ModuleGroupId	INT
AS
BEGIN
	IF @RoleId = 1 -- admin
		BEGIN
			SELECT
				[UserId]		= @UserId,
				[FunctionId]	= f.[FunctionId],
				[IsView]		= CAST(1 AS BIT),
				[IsCreate]		= CAST(1 AS BIT),
				[IsEdit]		= CAST(1 AS BIT),
				[IsDelete]		= CAST(1 AS BIT),
				[FunctionName]	= f.[FunctionName],
				[ModuleId]		= f.[ModuleId],
				[FSortOrder]	= f.[SortOrder],
				[Controller]	= f.[Controller],
				[Area]			= f.[Area],
				[Action]		= f.[Action],
				[ModuleName]	= m.[ModuleName],
				[MSortOrder]	= m.[SortOrder]
			FROM [dbo].[Function] f
			INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
			WHERE m.[ModuleGroupId] = ISNULL(@ModuleGroupId,m.[ModuleGroupId])
		END
	ELSE -- user
		BEGIN
			SELECT
				[UserId]		= r.[UserId],
				[FunctionId]	= r.[FunctionId],
				[IsView]		= r.[IsView],
				[IsCreate]		= r.[IsCreate],
				[IsEdit]		= r.[IsEdit],
				[IsDelete]		= r.[IsDelete],
				[FunctionName]	= f.[FunctionName],
				[ModuleId]		= f.[ModuleId],
				[FSortOrder]	= f.[SortOrder],
				[Controller]	= f.[Controller],
				[Area]			= f.[Area],
				[Action]		= f.[Action],
				[ModuleName]	= m.[ModuleName],
				[MSortOrder]	= m.[SortOrder]
			FROM [dbo].[Rights] r
			INNER JOIN [dbo].[Function] f ON f.[FunctionId] = r.[FunctionId]
			INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
			WHERE 
					r.[UserId] = @UserId
				AND
					m.[ModuleGroupId] = ISNULL(@ModuleGroupId,m.[ModuleGroupId])
				AND
					r.[IsView] = 1
		END
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRights_Authority]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_UserRights_Authority]
	@UserId			INT,
	@ModuleGroupId	INT
AS
BEGIN
	IF EXISTS (SELECT [UserId] FROM [dbo].[Rights] WHERE [UserId] = @UserId)
		BEGIN
			SELECT
				[UserId]		= @UserId,
				[FunctionId]	= f.[FunctionId],
				[IsView]		= ISNULL((SELECT [IsView] FROM [dbo].[Rights] WHERE [FunctionId] = f.[FunctionId] AND [UserId] = @UserId),CAST(0 AS BIT)),
				[IsCreate]		= ISNULL((SELECT [IsCreate] FROM [dbo].[Rights] WHERE [FunctionId] = f.[FunctionId] AND [UserId] = @UserId),CAST(0 AS BIT)),
				[IsEdit]		= ISNULL((SELECT [IsEdit] FROM [dbo].[Rights] WHERE [FunctionId] = f.[FunctionId] AND [UserId] = @UserId),CAST(0 AS BIT)),
				[IsDelete]		= ISNULL((SELECT [IsDelete] FROM [dbo].[Rights] WHERE [FunctionId] = f.[FunctionId] AND [UserId] = @UserId),CAST(0 AS BIT)),
				[FunctionName]	= f.[FunctionName],
				[ModuleId]		= f.[ModuleId],
				[FSortOrder]	= f.[SortOrder],
				[Controller]	= f.[Controller],
				[Area]			= f.[Area],
				[Action]		= f.[Action],
				[ModuleName]	= m.[ModuleName],
				[MSortOrder]	= m.[SortOrder],
				[GroupName]		= mg.[GroupName]
			FROM [dbo].[Function] f
			INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
			INNER JOIN [dbo].[ModuleGroup] mg ON mg.[ModuleGroupId] = m.[ModuleGroupId]
		END 
	ELSE
		BEGIN
			SELECT
				[UserId]		= @UserId,
				[FunctionId]	= f.[FunctionId],
				[IsView]		= CAST(1 AS BIT),
				[IsCreate]		= CAST(1 AS BIT),
				[IsEdit]		= CAST(1 AS BIT),
				[IsDelete]		= CAST(1 AS BIT),
				[FunctionName]	= f.[FunctionName],
				[ModuleId]		= f.[ModuleId],
				[FSortOrder]	= f.[SortOrder],
				[Controller]	= f.[Controller],
				[Area]			= f.[Area],
				[Action]		= f.[Action],
				[ModuleName]	= m.[ModuleName],
				[MSortOrder]	= m.[SortOrder],
				[GroupName]		= mg.[GroupName]
			FROM [dbo].[Function] f
			INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
			INNER JOIN [dbo].[ModuleGroup] mg ON mg.[ModuleGroupId] = m.[ModuleGroupId]
			WHERE m.[ModuleGroupId] = @ModuleGroupId
			UNION
			SELECT
				[UserId]		= @UserId,
				[FunctionId]	= f.[FunctionId],
				[IsView]		= CAST(0 AS BIT),
				[IsCreate]		= CAST(0 AS BIT),
				[IsEdit]		= CAST(0 AS BIT),
				[IsDelete]		= CAST(0 AS BIT),
				[FunctionName]	= f.[FunctionName],
				[ModuleId]		= f.[ModuleId],
				[FSortOrder]	= f.[SortOrder],
				[Controller]	= f.[Controller],
				[Area]			= f.[Area],
				[Action]		= f.[Action],
				[ModuleName]	= m.[ModuleName],
				[MSortOrder]	= m.[SortOrder],
				[GroupName]		= mg.[GroupName]
			FROM [dbo].[Function] f
			INNER JOIN [dbo].[Module] m ON m.[ModuleId] = f.[ModuleId]
			INNER JOIN [dbo].[ModuleGroup] mg ON mg.[ModuleGroupId] = m.[ModuleGroupId]
			WHERE m.[ModuleGroupId] <> @ModuleGroupId
		END
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Users]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Users]
	-- Add the parameters for the stored procedure here
	 @IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		[UserName],
		[Password],
		[FullName]			= u.[FullName],
		[Email]				= u.[Email],
		[PhoneNumber]		= u.[PhoneNumber],
		[IsActive]			= u.[IsActive],
		[CreateDate]		= u.[CreateDate],
		[EmployeeId]		= u.[EmployeeId],
		[EmployeeCode]		= e.[EmployeeCode],
		[DepartmentName]	= d.[DepartmentName],
		d.[DepartmentId],
		[DepartmentCompany]	= e.DepartmentCompany
	FROM [dbo].[User] u
	LEFT JOIN [hrm].[Employee] e ON u.EmployeeId = e.EmployeeId
	LEFT JOIN [hrm].[Department] d ON d.DepartmentId = e.DepartmentId
	WHERE u.[IsActive] = ISNULL(@IsActive,u.[IsActive])
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Users_Of_Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Users_Of_Employee]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[UserId],
		[UserName],
		[EmployeeCode],
		[EmployeeId],
		[FullName],
		[DepartmentId],
		[DepartmentName]
	FROM [dbo].[UserEmployeeDepartment]
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Ward_ByDistrictId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Ward_ByDistrictId]
	-- Add the parameters for the stored procedure here 
	@DistrictId		INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[WardId],
		[WardName],
		[DistrictId],
		[Description]
	FROM [dbo].[Ward] 
	WHERE  [DistrictId] = @DistrictId
	ORDER BY [WardName] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Wards]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Wards]
	-- Add the parameters for the stored procedure here 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[WardId],
		[WardName],
		[DistrictId],
		[Description]
	FROM [dbo].[Ward]
	ORDER BY [WardName] ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_CategoryKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Insert_CategoryKpi]
	@CategoryKpiId		INT,
	@KpiCode			VARCHAR(50),
	@KpiName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@CreateDate			DATETIME,
	@CreateBy			INT
AS

BEGIN
	INSERT INTO [dbo].[CategoryKpi]
	(
		[KpiCode],
		[KpiName],
		[Description],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@KpiCode,
		@KpiName,
		@Description,
		@CreateDate,
		@CreateBy
	)
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Chat]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Insert_Chat]
	@ChatId			VARCHAR(50),
	@SenderId		INT,
	@ReceiptId		INT,
	@ChatGroupId	BIGINT,
	@SendDate		DATETIME,
	@Message		NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO [dbo].[Chat]
	(
		[ChatId],
		[SenderId],
		[ReceiptId],
		[ChatGroupId],
		[SendDate],
		[Message]
	)
	VALUES
	(
		@ChatId,
		@SenderId,
		@ReceiptId,
		@ChatGroupId,
		@SendDate,
		@Message
	)
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Country]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_Country]
	@CountryId			INT OUTPUT,
	@CountryCode		VARCHAR(50),
	@CountryName		NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Country]
	(
		[CountryCode],
		[CountryName],
		[Description]
	)
	VALUES
	(
		@CountryCode,
		@CountryName,
		@Description
	)
	SET @CountryId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Notification]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_Notification]
	@NotificationId		VARCHAR(50),
	@FromUser			NVARCHAR(100),
	@ToUserId			INT,
	@NotificationDate	DATETIME,
	@Message			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Notification]
	(
		[NotificationId],
		[FromUser],
		[ToUserId],
		[NotificationDate],
		[Message]
	)
	VALUES
	(
		@NotificationId,
		@FromUser,
		@ToUserId,
		@NotificationDate,
		@Message
	)
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Post]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Insert_Post]
	@PostId				INT,
	@Title				NVARCHAR(255),
	@PublishDate		DATE,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@PostContent		NTEXT,
	@IsFeature			BIT
AS
BEGIN
	INSERT INTO [dbo].[Post]
	(
		[Title],
		[PublishDate],
		[CreateDate],
		[CreateBy],
		[PostContent],
		[IsFeature]
	)
	VALUES
	(
		@Title,
		@PublishDate,
		@CreateDate,
		@CreateBy,
		@PostContent,
		@IsFeature
	)
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_User]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_User] 
	-- Add the parameters for the stored procedure here
	@UserId			INT OUTPUT,
	@RoleId			INT,
	@UserGroupId	INT,
	@ModuleGroupId	INT,
	@UserName		VARCHAR(50),
	@Password		VARCHAR(100),
	@FullName		NVARCHAR(255),
	@Email			NVARCHAR(255),
	@PhoneNumber	VARCHAR(50),	
	@IsActive		BIT,
	@CreateDate		DATETIME,
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[User] 
	( 
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		[UserName],
		[Password],
		[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate],
		[EmployeeId]
	 )
	 VALUES
	 (  
		@RoleId,
		@UserGroupId,
		@ModuleGroupId,
		@UserName,
		@Password,
		@FullName,
		@Email,
		@PhoneNumber,
		@IsActive,
		@CreateDate,
		@EmployeeId
	 )
	 SET @UserId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_UserGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_UserGroup]
	-- Add the parameters for the stored procedure here
	@GroupName		NVARCHAR(255),
	@Description	NVARCHAR(500),
	@UserGroupId	INT OUTPUT,
	@GroupCode		VARCHAR(50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here 
	INSERT INTO [dbo].[UserGroup] 
	(
		[GroupName],
		[Description],
		[GroupCode]
	 )
	 VALUES
	 ( 
		@GroupName,	
		@Description,	
		@GroupCode	
	 )
	 SET @UserGroupId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_UserGroupRights]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		DuyNV
-- Create date:  
-- Description:	<Insert_UserRight>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_UserGroupRights]
	@XML				NVARCHAR(MAX), 
	@UserGroupId		INT
AS
BEGIN
	DECLARE	@XMLID				INT,
			@XMLRootName		VARCHAR(100)
	SET		@XML				= dbo.ufn_Replace_XmlChars(@XML)
	SET		@XMLRootName		= dbo.ufn_Get_Root_Element_Name(@XML) +'/UserGroupRights'
	print @XMLRootName
	EXEC	sp_xml_preparedocument	@XMLID OUT, @XML
	SET NOCOUNT ON;
	DELETE  [dbo].[UserGroupRights] WHERE UserGroupId = @UserGroupId
	INSERT INTO  [dbo].[UserGroupRights]
	(
		[UserGroupId],
		[FunctionId],
		[IsView],
		[IsCreate],
		[IsEdit],
		[IsDelete]
	)
	SELECT
		[UserGroupId]	= @UserGroupId,
		[FunctionId]	= x.FunctionId ,
		[IsView]		= x.[IsView],
		[IsCreate]		= x.[IsCreate],
		[IsEdit]		= x.[IsEdit],
		[IsDelete]		= x.[IsDelete]
	FROM OPENXML(@XMLID, @XMLRootName,2) 	
	WITH (
		[UserGroupId]	INT,
		[FunctionId]	INT,
		[IsView]		BIT,
		[IsCreate]		BIT,
		[IsEdit]		BIT,
		[IsDelete]		BIT
	)x 
	EXEC sp_xml_removedocument @XMLID
END
GO
/****** Object:  StoredProcedure [dbo].[Insert_UserRights]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		DuyNV
-- Create date:  
-- Description:	<Insert_UserRight>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_UserRights]
	@XML		NVARCHAR(MAX), 
	@UserId		INT
AS
BEGIN
	DECLARE	@XMLID				INT,
			@XMLRootName		VARCHAR(100)
	SET		@XML				= dbo.ufn_Replace_XmlChars(@XML)
	SET		@XMLRootName		= dbo.ufn_Get_Root_Element_Name(@XML) +'/Rights'
	print @XMLRootName
	EXEC	sp_xml_preparedocument	@XMLID OUT, @XML
	SET NOCOUNT ON;
	DELETE  [dbo].[Rights] WHERE UserId = @UserId
	INSERT INTO  [dbo].[Rights]
	(
		[UserId],
		[FunctionId],
		[IsView],
		[IsCreate],
		[IsEdit],
		[IsDelete]
	)
	SELECT
		[UserId]		= @UserId,
		[FunctionId]	= x.FunctionId ,
		[IsView]		= x.[IsView],
		[IsCreate]		= x.[IsCreate],
		[IsEdit]		= x.[IsEdit],
		[IsDelete]		= x.[IsDelete]
	FROM OPENXML(@XMLID, @XMLRootName,2) 	
	WITH (
		[UserId]		INT,
		[FunctionId]	INT,
		[IsView]		BIT,
		[IsCreate]		BIT,
		[IsEdit]		BIT,
		[IsDelete]		BIT
	)x 
	EXEC sp_xml_removedocument @XMLID
END
GO
/****** Object:  StoredProcedure [dbo].[Update_CategoryKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Update_CategoryKpi]
	@CategoryKpiId		INT,
	@KpiCode			VARCHAR(50),
	@KpiName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@CreateDate			DATETIME,
	@CreateBy			INT
AS

BEGIN
	UPDATE [dbo].[CategoryKpi]
	SET
		[KpiCode]		= @KpiCode,
		[KpiName]		= @KpiName,
		[Description]	= @Description
	WHERE [CategoryKpiId]	= @CategoryKpiId
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Country]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_Country]
	@CountryId		INT,
	@CountryCode	VARCHAR(50),
	@CountryName	NVARCHAR(255),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [dbo].[Country]
	SET
		[CountryCode]	= @CountryCode,
		[CountryName]	= @CountryName,
		[Description]	= @Description
	WHERE CountryId		= @CountryId
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Password]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_Password] 
	-- Add the parameters for the stored procedure here
	@UserId			INT,
	@Password		VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 UPDATE [dbo].[User]
	 SET
		[Password] = @Password
	 WHERE [UserId]	= @UserId
 
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Post]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Update_Post]
	@PostId				INT,
	@Title				NVARCHAR(255),
	@PublishDate		DATE,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@PostContent		NTEXT,
	@IsFeature			BIT
AS

BEGIN
	UPDATE [dbo].[Post]
	SET
		[Title]			= @Title,
		[PublishDate]	= @PublishDate,
		[PostContent]	= @PostContent,
		[IsFeature]		= @IsFeature
	WHERE [PostId] = @PostId
END
GO
/****** Object:  StoredProcedure [dbo].[Update_User]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_User] 
	-- Add the parameters for the stored procedure here
	@UserId			INT,
	@RoleId			INT,
	@UserGroupId	INT,
	@ModuleGroupId	INT,
	@UserName		VARCHAR(50), 
	@FullName		NVARCHAR(255),
	@Email			NVARCHAR(255),
	@PhoneNumber	VARCHAR(50),	
	@IsActive		BIT,
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 UPDATE [dbo].[User]
	 SET
		[RoleId]		= @RoleId,
		[UserGroupId]	= @UserGroupId,
		[ModuleGroupId]	= @ModuleGroupId,
		[UserName]		= @UserName, 
		[FullName]		= @FullName,
		[Email]			= @Email,
		[PhoneNumber]	= @PhoneNumber,
		[IsActive]		= @IsActive,
		[EmployeeId]	= @EmployeeId
	 WHERE [UserId]		= @UserId
 
END
GO
/****** Object:  StoredProcedure [dbo].[Update_UserGroup]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<DNV>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Update_UserGroup]
	-- Add the parameters for the stored procedure here
	@GroupName		NVARCHAR(255),
	@Description	NVARCHAR(500), 
	@UserGroupId	INT,
	@GroupCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE  [dbo].[UserGroup]
	 SET
		[GroupName]		= @GroupName,
		[Description]	= @Description, 
		[GroupCode]		= @GroupCode
	 WHERE [UserGroupId]	= @UserGroupId;
END
GO
/****** Object:  StoredProcedure [dbo].[User_Login]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<ThiNQ>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_Login]
	@UserName	VARCHAR(50),
	@Password	VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		u.[UserId],
		[RoleId],
		[UserGroupId],
		[ModuleGroupId],
		u.[UserName],
		[Password],
		u.[FullName],
		[Email],
		[PhoneNumber],
		[IsActive],
		[CreateDate]
	FROM dbo.[User] u
	WHERE 
			u.UserName = @UserName
		AND
			Password = @Password
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Applicant]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Applicant]
	@ApplicantId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Applicant] WHERE [ApplicantId] = @ApplicantId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Career]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Career]
	@CareerId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Career] WHERE CareerId = @CareerId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Contract]
	@ContractId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	DELETE [hrm].[Contract] WHERE ContractId = @ContractId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_ContractType]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_ContractType]
	@ContractTypeId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[ContractType]	WHERE ContractTypeId = @ContractTypeId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Department]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Department]
	@DepartmentId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Department] WHERE DepartmentId = @DepartmentId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_EducationLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_EducationLevel]
	@EducationLevelId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[EducationLevel] WHERE EducationLevelId = @EducationLevelId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Employee]
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	DELETE [hrm].[Employee] WHERE EmployeeId = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_EmployeeHoliday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_EmployeeHoliday]
	@EmployeeHolidayId		VARCHAR(50)
AS

BEGIN
	DELETE [hrm].[EmployeeHoliday]	WHERE [EmployeeHolidayId]	= @EmployeeHolidayId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Holiday_ByYear]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Holiday_ByYear]
	@Year	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Holiday]
	WHERE YEAR([HolidayDate]) = @Year
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_HolidayReason]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_HolidayReason]
	@HolidayReasonId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[HolidayReason] WHERE [HolidayReasonId] = @HolidayReasonId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_IncurredSalary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_IncurredSalary]
	@IncurredSalaryId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN
		DELETE [hrm].[IncurredSalary] WHERE [IncurredSalaryId] = @IncurredSalaryId
	END
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Insurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_Insurance]
	@InsuranceId		BIGINT
AS
BEGIN
	DELETE [hrm].[Insurance] WHERE [InsuranceId] = @InsuranceId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_InsuranceMedical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_InsuranceMedical]
	@InsuranceMedicalId		VARCHAR(50)
AS
BEGIN
	DELETE [hrm].[InsuranceMedical]	WHERE [InsuranceMedicalId]	= @InsuranceMedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_InsuranceProcess]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_InsuranceProcess]
	@InsuranceProcessId		VARCHAR(50)
AS
BEGIN
	DELETE [hrm].[InsuranceProcess]	WHERE [InsuranceProcessId] = @InsuranceProcessId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_JobChange]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_JobChange]
	@JobChangeId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[JobChange] WHERE [JobChangeId] = @JobChangeId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Maternity]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Delete_Maternity]
	@MaternityId		VARCHAR(50)
AS
BEGIN
	DELETE [hrm].[Maternity] WHERE [MaternityId]	= @MaternityId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Medical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Medical]
	@MedicalId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Medical] WHERE [MedicalId] = @MedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Nation]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Nation]
	@NationId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Nation] WHERE NationId = @NationId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Position]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Position]
	@PositionId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Position]	WHERE PositionId = @PositionId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_PraiseDiscipline]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_PraiseDiscipline]
	@PraiseDisciplineId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[PraiseDisciplineDetail]	WHERE PraiseDisciplineId	= @PraiseDisciplineId
	DELETE [hrm].[PraiseDiscipline]	WHERE PraiseDisciplineId	= @PraiseDisciplineId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_RecruitChanel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_RecruitChanel]
	@RecruitChanelId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[RecruitChanel] WHERE [RecruitChanelId] = @RecruitChanelId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_RecruitPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_RecruitPlan]
	@RecruitPlanId			BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[RecruitPlan] WHERE [RecruitPlanId] = @RecruitPlanId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_RecruitResult]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_RecruitResult]
	@RecruitResultId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[RecruitResultDetail] WHERE [RecruitResultId] = @RecruitResultId

	DELETE [hrm].[RecruitResult] WHERE [RecruitResultId]	= @RecruitResultId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Religion]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Religion]
	@ReligionId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Religion]	WHERE ReligionId = @ReligionId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_Salary]
	@SalaryId					VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[Salary] WHERE [SalaryId] = @SalaryId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_School]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_School]
	@SchoolId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[School] WHERE SchoolId = @SchoolId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_TimeSheet]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_TimeSheet]
	@TimeSheetId		VARCHAR(50)
AS

BEGIN
	DELETE [hrm].[TimeSheet]	WHERE [TimeSheetId]	= @TimeSheetId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_TimeSheetOt]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Delete_TimeSheetOt]
	@TimeSheetOtId		VARCHAR(50)
AS

BEGIN
	DELETE [hrm].[TimeSheetOt]	WHERE [TimeSheetOtId]	= @TimeSheetOtId
END
GO
/****** Object:  StoredProcedure [hrm].[Delete_TrainingLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Delete_TrainingLevel]
	@TrainingLevelId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [hrm].[TrainingLevel] WHERE TrainingLevelId = @TrainingLevelId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Applicant]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Applicant]
	@ApplicantId	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ApplicantId]			= CAST([ApplicantId] AS VARCHAR(50)),
		[FullName],
		[Sex],
		[DateOfBirth],
		[CountryId],
		[NationId],
		[ReligionId],
		[CityBirthPlace],
		[PermanentAddress],
		[IdentityCardNumber],
		[PhoneNumber],
		[Email],
		[ChanelId],
		[TrainingLevelId],
		[RecruitPlanId],
		[CvDate],
		[CreateDate],
		[CreateBy],
		[Description]
	FROM [hrm].[Applicant]
	WHERE [ApplicantId] = @ApplicantId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Applicants]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Applicants]
	@RecruitPlanId	BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ApplicantId]			= CAST([ApplicantId] AS VARCHAR(50)),
		[FullName],
		[Sex],
		[DateOfBirth],
		[CountryId],
		[NationId],
		[ReligionId],
		[CityBirthPlace],
		[PermanentAddress],
		[IdentityCardNumber],
		[PhoneNumber],
		[Email],
		[ChanelId],
		[TrainingLevelId],
		[RecruitPlanId],
		[CvDate],
		[CreateDate],
		[CreateBy],
		[Description]
	FROM [hrm].[Applicant]
	WHERE [RecruitPlanId] = @RecruitPlanId
	ORDER BY [CreateDate] DESC
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Career]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Career]
	@CareerId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[CareerId],
		[CareerName],
		[Description]
	FROM [hrm].[Career]
	WHERE [CareerId] = @CareerId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Careers]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Careers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[CareerId],
		[CareerName],
		[Description]
	FROM [hrm].[Career]
	ORDER BY [CareerName] ASC
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Contract]
	@ContractId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	SELECT
		[ContractId]			= CAST([ContractId] AS VARCHAR(50)),
		[ContractCode],
		[EmployeeId],
		[StartDate],
		[EndDate],
		[ContractTypeId],
		[ContractFile],
		[ContractOthorFile],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [hrm].[Contract]
	WHERE [ContractId] = @ContractId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Contract_ByContractCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Contract_ByContractCode]
	@ContractCode	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	SELECT
		[ContractId]			= CAST([ContractId] AS VARCHAR(50)),
		[ContractCode],
		[EmployeeId],
		[StartDate],
		[EndDate],
		[ContractTypeId],
		[ContractFile],
		[ContractOthorFile],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [hrm].[Contract]
	WHERE [ContractCode] = @ContractCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Contract_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Contract_ByEmployeeId]
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	SELECT
		[ContractId]			= CAST([ContractId] AS VARCHAR(50)),
		[ContractCode],
		[EmployeeId],
		[StartDate],
		[EndDate],
		[ContractTypeId],
		[ContractFile],
		[ContractOthorFile],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [hrm].[Contract]
	WHERE [EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Contracts]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Contracts]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	SELECT
		[ContractId]			= CAST([ContractId] AS VARCHAR(50)),
		[ContractCode],
		[EmployeeId],
		[StartDate],
		[EndDate],
		[ContractTypeId],
		[ContractFile],
		[ContractOthorFile],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [hrm].[Contract]
	ORDER BY [CreateDate] DESC
END
GO
/****** Object:  StoredProcedure [hrm].[Get_ContractType]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_ContractType]
	@ContractTypeId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ContractTypeId],
		[TypeName],
		[Description],
		[IsActive]
	FROM [hrm].[ContractType]
	WHERE [ContractTypeId] = @ContractTypeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_ContractTypes]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_ContractTypes]
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ContractTypeId],
		[TypeName],
		[Description],
		[IsActive]
	FROM [hrm].[ContractType]
	WHERE IsActive = ISNULL(@IsActive,IsActive)
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Department]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Department]
	@DepartmentId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[DepartmentId],
		[DepartmentCode],
		[DepartmentName],
		[ParentId],
		[Description],
		[Path],
		[IsActive]
	FROM [hrm].[Department]
	WHERE [DepartmentId] = @DepartmentId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Department_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Department_ByCode]
	@DepartmentCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[DepartmentId],
		[DepartmentCode],
		[DepartmentName],
		[ParentId],
		[Description],
		[IsActive],
		[Path]
	FROM [hrm].[Department]
	WHERE DepartmentCode = @DepartmentCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Department_ByParentId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Department_ByParentId]
	@ParentId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[DepartmentId],
		[DepartmentCode],
		[DepartmentName],
		[ParentId],
		[Description],
		[IsActive],
		[Path]
	FROM [hrm].[Department]
	WHERE ParentId = @ParentId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Departments]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Departments]
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[DepartmentId],
		[DepartmentCode],
		[DepartmentName],
		[ParentId],
		[Description],
		[IsActive],
		[Path],
		[EmployeeOns] = ISNULL((SELECT COUNT(e.EmployeeId)  FROM [hrm].[Employee] e INNER JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId] WHERE ( u.[Path] LIKE ''+d.[Path]+'%') AND e.[IsActive]=1),0),
		[EmployeeOffs] = ISNULL((SELECT COUNT(e.EmployeeId)  FROM [hrm].[Employee] e INNER JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId] WHERE ( u.[Path] LIKE ''+d.[Path]+'%') AND e.[IsActive]=0),0)
	FROM [hrm].[Department] d
	WHERE IsActive = ISNULL(@IsActive,IsActive)
END
GO
/****** Object:  StoredProcedure [hrm].[Get_EducationLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_EducationLevel]
	@EducationLevelId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EducationLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[EducationLevel]
	WHERE [EducationLevelId] = @EducationLevelId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_EducationLevel_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_EducationLevel_ByCode]
	@LevelCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EducationLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[EducationLevel]
	WHERE LevelCode = @LevelCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_EducationLevels]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_EducationLevels]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EducationLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[EducationLevel]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employee]
	-- Add the parameters for the stored procedure here
	@EmployeeId	BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId]			= e.[DepartmentId],
		e.[CountryId],
		[NationId]				= e.[NationId],
		[ReligionId]			= e.[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		e.[PositionId],
		e.[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description]					= e.[Description],
		[CreateBy],
		[CreateDate],
		[IsActive]						= e.[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		e.[EducationLevelId],
		e.[SchoolId],
		e.[CareerId],
		[DepartmentName]		= d.[DepartmentName],
		[NationName]			= n.NationName,
		[ReligionName]			= r.ReligionName,
		[PermanentCityName]		= c.CityName,
		[PermanentDistrictName] = dt.DistrictName,
		[TimeSheetCode],
		[DepartmentCompany],
		[CountryName]			= ct.CountryName,
		[PositionName]			= p.PositionName,
		[TrainingLevelName]		= tl.LevelName,
		[EducationLevelName]	= el.LevelName,
		[SchoolName]			= s.SchoolName,
		[CarrerName]			= cr.CareerName,
		[CategoryKpiId]
		
	FROM [hrm].[Employee] e
	INNER JOIN hrm.[Department] d ON d.DepartmentId = e.DepartmentId
	LEFT JOIN hrm.Nation n ON n.NationId = e.NationId
	LEFT JOIN dbo.Country ct ON ct.CountryId = e.CountryId
	LEFT JOIN hrm.Position p ON p.PositionId = e.PositionId
	LEFT JOIN hrm.School s ON s.SchoolId = e.SchoolId
	LEFT JOIN hrm.Career cr ON cr.CareerId = e.CareerId
	LEFT JOIN hrm.TrainingLevel tl ON tl.TrainingLevelId = e.TrainingLevelId
	LEFT JOIN hrm.EducationLevel el ON el.EducationLevelId = e.EducationLevelId
	LEFT JOIN [hrm].[Religion] r ON r.ReligionId = e.ReligionId
	LEFT JOIN [dbo].[City] c ON c.CityId = e.PermanentCity
	LEFT JOIN [dbo].[District] dt ON dt.DistrictId = e.PermanentDistrict 
	WHERE EmployeeId = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employee_ByDepartmentIdAndPosition]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employee_ByDepartmentIdAndPosition]
	-- Add the parameters for the stored procedure here
	@DepartmentId		INT,
	@Position			TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Path VARCHAR(500)
	SET @Path = ISNULL((SELECT [Path] FROM [hrm].[Department] WHERE [DepartmentId] = @DepartmentId),'')
	SET NOCOUNT ON;
	SELECT
		e.[EmployeeId],
		e.[EmployeeCode],
		e.[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		e.[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		e.[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		e.[Description],
		[CreateBy],
		[CreateDate],
		e.[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode],
		e.[DepartmentCompany],
		e.[CategoryKpiId],
		ued.[UserName],
		ued.UserId
	FROM [hrm].[Employee] e
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[EmployeeId] = e.[EmployeeId]
	WHERE 
			ued.Path LIKE ''+@Path+'%'
		AND
			e.[DepartmentCompany] = ISNULL(@Position,e.[DepartmentCompany])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employee_ByEmployeeCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employee_ByEmployeeCode]
	-- Add the parameters for the stored procedure here
	@EmployeeCode	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description],
		[CreateBy],
		[CreateDate],
		[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode]
	FROM [hrm].[Employee]
	WHERE EmployeeCode = @EmployeeCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employee_ByPath]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employee_ByPath]
	-- Add the parameters for the stored procedure here
	@Path	VARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		e.[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		e.[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		e.[Description],
		[CreateBy],
		[CreateDate],
		e.[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode],
		[DepartmentCompany],
		[PositionName]			= p.PositionName,
		[DepartmentName]		= d.[DepartmentName],
		[CategoryKpiId]

	FROM [hrm].[Employee] e
	INNER JOIN [hrm].Department d On d.DepartmentId = e.DepartmentId 
	LEFT JOIN hrm.Position p ON p.PositionId = e.PositionId
	WHERE d.Path LIKE ''+@Path+'%'
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employee_ByTimeSheetCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employee_ByTimeSheetCode]
	-- Add the parameters for the stored procedure here
	@TimeSheetCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description],
		[CreateBy],
		[CreateDate],
		[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode]
	FROM [hrm].[Employee] 
	WHERE [TimeSheetCode] = @TimeSheetCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_EmployeeHoliday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_EmployeeHoliday]
	@EmployeeHolidayId	VARCHAR(50)			
AS
BEGIN
	SELECT
		[EmployeeHolidayId]		= CAST([EmployeeHolidayId] AS VARCHAR(50)),
		e.[HolidayReasonId],
		[EmployeeId]			= e.[EmployeeId],
		[FromDate],
		[ToDate],
		e.[Description],
		[CreateDate]			= e.[CreateDate],
		[FullName]				= ued.[FullName],
		[EmployeeCode]			= ued.[EmployeeCode],
		[ReasonName]			= hr.ReasonName
	FROM [hrm].[EmployeeHoliday] e
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[EmployeeId] =  e.[EmployeeId]
	INNER JOIN [hrm].HolidayReason hr On hr.HolidayReasonId = e.HolidayReasonId
	WHERE	[EmployeeHolidayId] =  @EmployeeHolidayId

	SELECT
		[HolidayDetailId]		= CAST([HolidayDetailId] AS VARCHAR(50)),
		[DateDay],
		[NumberDays],
		[Permission],
		[PercentSalary],
		[ToTalDays],
		[EmployeeHolidayId]		= CAST([EmployeeHolidayId] AS VARCHAR(50))
	FROM [hrm].[HolidayDetail] h
	WHERE	h.[EmployeeHolidayId] = @EmployeeHolidayId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_EmployeeHolidays]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_EmployeeHolidays]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@EmployeeId		BIGINT			
AS
BEGIN
	SELECT
		[EmployeeHolidayId]		= CAST([EmployeeHolidayId] AS VARCHAR(50)),
		[HolidayReasonId]		= eh.[HolidayReasonId],
		[FromDate],
		[EmployeeId]			= eh.[EmployeeId],
		[ToDate],
		[Description]			= eh.[Description],
		[CreateDate]			= eh.[CreateDate],
		[FullName]				= ued.[FullName],
		[EmployeeCode]			= ued.[EmployeeCode],
		[DepartmentName]		= ued.[DepartmentName]
	FROM [hrm].[EmployeeHoliday] eh
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[EmployeeId] =  eh.[EmployeeId]
	INNER JOIN [hrm].[Employee] e  ON e.[EmployeeId] =  eh.[EmployeeId]
	WHERE 
			CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE))
		AND
			e.[EmployeeId] = ISNULL(@EmployeeId,e.[EmployeeId])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employees]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employees]
	-- Add the parameters for the stored procedure here
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId]			= e.[DepartmentId],
		e.[CountryId],
		[NationId]				= e.[NationId],
		[ReligionId]			= e.[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		e.[PositionId],
		e.[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description]					= e.[Description],
		[CreateBy],
		[CreateDate],
		[IsActive]						= e.[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		e.[EducationLevelId],
		e.[SchoolId],
		e.[CareerId],
		[DepartmentName]		= d.[DepartmentName],
		[NationName]			= n.NationName,
		[ReligionName]			= r.ReligionName,
		[PermanentCityName]		= c.CityName,
		[PermanentDistrictName] = dt.DistrictName,
		[TimeSheetCode],
		[DepartmentCompany],
		[CountryName]			= ct.CountryName,
		[PositionName]			= p.PositionName,
		[TrainingLevelName]		= tl.LevelName,
		[EducationLevelName]	= el.LevelName,
		[SchoolName]			= s.SchoolName,
		[CarrerName]			= cr.CareerName,
		[CategoryKpiId]
		
	FROM [hrm].[Employee] e
	INNER JOIN hrm.[Department] d ON d.DepartmentId = e.DepartmentId
	LEFT JOIN hrm.Nation n ON n.NationId = e.NationId
	LEFT JOIN dbo.Country ct ON ct.CountryId = e.CountryId
	LEFT JOIN hrm.Position p ON p.PositionId = e.PositionId
	LEFT JOIN hrm.School s ON s.SchoolId = e.SchoolId
	LEFT JOIN hrm.Career cr ON cr.CareerId = e.CareerId
	LEFT JOIN hrm.TrainingLevel tl ON tl.TrainingLevelId = e.TrainingLevelId
	LEFT JOIN hrm.EducationLevel el ON el.EducationLevelId = e.EducationLevelId
	LEFT JOIN [hrm].[Religion] r ON r.ReligionId = e.ReligionId
	LEFT JOIN [dbo].[City] c ON c.CityId = e.PermanentCity
	LEFT JOIN [dbo].[District] dt ON dt.DistrictId = e.PermanentDistrict
	WHERE e.[IsActive] = ISNULL(@IsActive,e.[IsActive])
	ORDER BY [FullName] ASC
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employees_ByKeyword]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employees_ByKeyword]
	-- Add the parameters for the stored procedure here
	@Keyword	NVARCHAR(255)
AS
BEGIN
	SET @Keyword = ISNULL(@Keyword,'')
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description],
		[CreateBy],
		[CreateDate],
		[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode],
		[DepartmentCompany]
	FROM [hrm].[Employee]
	WHERE 
			[EmployeeCode] LIKE '%' + @Keyword + '%'
		OR
			[FullName] LIKE '%' + @Keyword + '%'
		OR
			[SpecialName] LIKE '%' + @Keyword + '%'
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employees_ForInsurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employees_ForInsurance]
	-- Add the parameters for the stored procedure here
	@Join	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @Join = 1
		BEGIN
			SELECT
				[EmployeeId],
				[EmployeeCode],
				[FullName],
				[DateOfBirth],
				[Gender],
				[SpecialName],
				[Avatar],
				[DepartmentId],
				[CountryId],
				[NationId],
				[ReligionId],
				[MaritalStatus],
				[CityBirthPlace],
				[CityNativeLand],
				[IdentityCardNumber],
				[IdentityCardDate],
				[CityIdentityCard],
				[PermanentAddress],
				[PermanentCity],
				[PermanentDistrict],
				[TemperaryAddress],
				[TemperaryCity],
				[TemperaryDistrict],
				[Email],
				[PhoneNumber],
				[PositionId],
				[TrainingLevelId],
				[HealthStatus],
				[DateOfYouthUnionAdmission],
				[PlaceOfYouthUnionAdmission],
				[DateOfPartyAdmission],
				[PlaceOfPartyAdmission],
				[Skill],
				[Experience],
				[Description],
				[CreateBy],
				[CreateDate],
				[IsActive],
				[Status],
				[ShiftWorkId],
				[WorkedDate],
				[EducationLevelId],
				[SchoolId],
				[CareerId],
				[TimeSheetCode]
			FROM [hrm].[Employee]
			WHERE [EmployeeId] IN (SELECT [EmployeeId] FROM [hrm].[Insurance])
		END
	ELSE
		BEGIN
			SELECT
				[EmployeeId],
				[EmployeeCode],
				[FullName],
				[DateOfBirth],
				[Gender],
				[SpecialName],
				[Avatar],
				[DepartmentId],
				[CountryId],
				[NationId],
				[ReligionId],
				[MaritalStatus],
				[CityBirthPlace],
				[CityNativeLand],
				[IdentityCardNumber],
				[IdentityCardDate],
				[CityIdentityCard],
				[PermanentAddress],
				[PermanentCity],
				[PermanentDistrict],
				[TemperaryAddress],
				[TemperaryCity],
				[TemperaryDistrict],
				[Email],
				[PhoneNumber],
				[PositionId],
				[TrainingLevelId],
				[HealthStatus],
				[DateOfYouthUnionAdmission],
				[PlaceOfYouthUnionAdmission],
				[DateOfPartyAdmission],
				[PlaceOfPartyAdmission],
				[Skill],
				[Experience],
				[Description],
				[CreateBy],
				[CreateDate],
				[IsActive],
				[Status],
				[ShiftWorkId],
				[WorkedDate],
				[EducationLevelId],
				[SchoolId],
				[CareerId],
				[TimeSheetCode]
			FROM [hrm].[Employee]
			WHERE [EmployeeId] NOT IN (SELECT [EmployeeId] FROM [hrm].[Insurance])
		END
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employees_Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employees_Salary]
	-- Add the parameters for the stored procedure here
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId]			= e.[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId]			= e.[DepartmentId],
		[CountryId],
		[NationId]				= e.[NationId],
		[ReligionId]			= e.[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description]					= e.[Description],
		[CreateBy]						= e.[CreateBy],
		[CreateDate]					= e.[CreateDate],
		[IsActive]						= e.[IsActive],
		[Status],
		[ShiftWorkId]					= 0,
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		e.[TimeSheetCode],
		[DepartmentCompany]
	FROM [hrm].[Employee] e
	INNER JOIN hrm.Salary sa ON sa.[EmployeeId] = e.[EmployeeId]
	WHERE e.[IsActive] = ISNULL(@IsActive,e.[IsActive])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Employees_TimeSheet]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Employees_TimeSheet]
	-- Add the parameters for the stored procedure here
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId]			= e.[DepartmentId],
		[CountryId],
		[NationId]				= e.[NationId],
		[ReligionId]			= e.[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description]					= e.[Description],
		[CreateBy],
		[CreateDate],
		[IsActive]						= e.[IsActive],
		[Status],
		[ShiftWorkId]					= 0,
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		e.[TimeSheetCode],
		[DepartmentCompany]
	FROM [hrm].[Employee] e
	WHERE e.[IsActive] = ISNULL(@IsActive,e.[IsActive])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Holiday_ByFromToDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Holiday_ByFromToDate]
	@FromDate	DATETIME,
	@ToDate		DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[HolidayId]		= CAST([HolidayId] AS VARCHAR(50)),
		[HolidayDate]
	FROM [hrm].[Holiday]
	WHERE
			CAST([HolidayDate] AS DATE) BETWEEN @FromDate AND @ToDate
		
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Holiday_ByYearAndMonth]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Holiday_ByYearAndMonth]
	@Year	INT,
	@Month	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[HolidayId]		= CAST([HolidayId] AS VARCHAR(50)),
		[HolidayDate]
	FROM [hrm].[Holiday]
	WHERE
			MONTH([HolidayDate])	= @Month
		AND
			YEAR([HolidayDate])		= @Year
END
GO
/****** Object:  StoredProcedure [hrm].[Get_HolidayConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_HolidayConfigs]
	@Year			INT,
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[EmployeeId],
		[Year],
		[HolidayNumber]
	FROM [hrm].[HolidayConfig]
	WHERE @Year = [Year] AND @EmployeeId = [EmployeeId]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_HolidayDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_HolidayDetails]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[HolidayDetailId]			= CAST([HolidayDetailId] AS VARCHAR(50)),
		[DateDay],
		[NumberDays],
		[Permission],
		[PercentSalary],
		[ToTalDays],
		[EmployeeHolidayId]			= CAST(hd.[EmployeeHolidayId] AS VARCHAR(50))
	FROM [hrm].[HolidayDetail] hd
	INNER JOIN [hrm].[EmployeeHoliday] eh ON eh.[EmployeeHolidayId] = hd.[EmployeeHolidayId]
	WHERE 
			eh.[EmployeeId]	= @EmployeeId
		AND
			CAST([DateDay] AS DATE) BETWEEN @FromDate AND @ToDate
END
GO
/****** Object:  StoredProcedure [hrm].[Get_HolidayReason]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_HolidayReason]
	@HolidayReasonId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[HolidayReasonId],
		[ReasonCode],
		[ReasonName],
		[Description],
		[IsActive],
		[PercentSalary]
	FROM [hrm].[HolidayReason]
	WHERE [HolidayReasonId] = @HolidayReasonId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_HolidayReason_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_HolidayReason_ByCode]
	@ReasonCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[HolidayReasonId],
		[ReasonCode],
		[ReasonName],
		[Description],
		[IsActive],
		[PercentSalary]
	FROM [hrm].[HolidayReason]
	WHERE [ReasonCode] = @ReasonCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_HolidayReasons]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_HolidayReasons]
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[HolidayReasonId],
		[ReasonCode],
		[ReasonName],
		[Description],
		[IsActive],
		[PercentSalary]
	FROM [hrm].[HolidayReason]
	WHERE [IsActive] = ISNULL(@IsActive,[IsActive])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_IncurredSalaries]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_IncurredSalaries]
	@EmployeeId		BIGINT,
	@FromDate		DATE,
	@ToDate			DATE,
	@CalcSalary		BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @CalcSalary = 1 -- dành cho tính lương nhân viên
		BEGIN
			SELECT
				[IncurredSalaryId]	= CAST([IncurredSalaryId] AS VARCHAR(50)),
				[EmployeeId]		= i.[EmployeeId],
				[Amount],
				[Title],
				[CreateDate]		= i.[CreateDate],
				[CreateBy]			= i.[CreateBy],
				[SubmitDate],
				[Description]		= i.[Description],
				[EmployeeCode]		= e.[EmployeeCode],
				[FullName]			= e.[FullName],
				[DepartmentId]		= e.[DepartmentId]
			FROM [hrm].[IncurredSalary] i
			INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = i.[EmployeeId]
			WHERE 
					i.[EmployeeId] = ISNULL(@EmployeeId,i.[EmployeeId])
				AND
					CAST([SubmitDate] AS DATE) BETWEEN @FromDate AND @ToDate
		END
	ELSE --  dành cho quản lý
		BEGIN
			SELECT
				[IncurredSalaryId]	= CAST([IncurredSalaryId] AS VARCHAR(50)),
				[EmployeeId]		= i.[EmployeeId],
				[Amount],
				[Title],
				[CreateDate]		= i.[CreateDate],
				[CreateBy]			= i.[CreateBy],
				[SubmitDate],
				[Description]		= i.[Description],
				[EmployeeCode]		= e.[EmployeeCode],
				[FullName]			= e.[FullName],
				[DepartmentId]		= e.[DepartmentId]
			FROM [hrm].[IncurredSalary] i
			INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = i.[EmployeeId]
			WHERE 
					i.[EmployeeId] = ISNULL(@EmployeeId,i.[EmployeeId])
				AND
					CAST(i.[CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate
		END
END
GO
/****** Object:  StoredProcedure [hrm].[Get_IncurredSalary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_IncurredSalary]
	@IncurredSalaryId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN
		SELECT
			[IncurredSalaryId]	= CAST([IncurredSalaryId] AS VARCHAR(50)),
			[EmployeeId]		= i.[EmployeeId],
			[Amount],
			[Title],
			[CreateDate]		= i.[CreateDate],
			[CreateBy]			= i.[CreateBy],
			[SubmitDate],
			[Description]		= i.[Description],
			[EmployeeCode]		= e.[EmployeeCode],
			[FullName]			= e.[FullName],
			[DepartmentId]		= e.[DepartmentId]
		FROM [hrm].[IncurredSalary] i
		INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = i.[EmployeeId]
		WHERE [IncurredSalaryId] = @IncurredSalaryId
	END
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Insurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_Insurance]
	@InsuranceId	BIGINT
AS
BEGIN
	SELECT
		[InsuranceId],
		i.[EmployeeId],
		[InsuranceNumber],
		[SubscriptionDate],
		[CityId],
		[MonthBefore],
		i.[Description],
		i.[IsActive],
		i.[CreateDate],
		i.[CreateBy],
		[EmployeeCode],
		[FullName],
		[DepartmentId]
	FROM [hrm].[Insurance] i
	INNER JOIN [hrm].[Employee] e ON i.[EmployeeId] = e.[EmployeeId]
	WHERE [InsuranceId] = @InsuranceId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Insurance_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_Insurance_ByEmployeeId]
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[InsuranceId],
		i.[EmployeeId],
		[InsuranceNumber],
		[SubscriptionDate],
		[CityId],
		[MonthBefore],
		i.[Description],
		i.[IsActive],
		i.[CreateDate],
		i.[CreateBy],
		[EmployeeCode],
		[FullName],
		[DepartmentId]
	FROM [hrm].[Insurance] i
	INNER JOIN [hrm].[Employee] e ON i.[EmployeeId] = e.[EmployeeId]
	WHERE i.[EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_InsuranceMedical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_InsuranceMedical]
	@InsuranceMedicalId		VARCHAR(50)
AS
BEGIN
	SELECT
		[InsuranceMedicalId]		= CAST([InsuranceMedicalId] AS VARCHAR(50)),
		ie.[EmployeeId],
		[InsuranceMedicalNumber],
		[StartDate],
		[ExpiredDate],
		[CityId],
		[MedicalId],
		[Amount],
		ie.[Description],
		ie.[CreateDate],
		ie.[CreateBy],
		[EmployeeCode],
		[FullName],
		[DepartmentId]
	FROM [hrm].[InsuranceMedical] ie
	INNER JOIN	[hrm].[Employee] e ON e.[EmployeeId] = ie.[EmployeeId]
	WHERE [InsuranceMedicalId] = @InsuranceMedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_InsuranceMedicals]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_InsuranceMedicals]
	@Year		INT,
	@EmployeeId	BIGINT
AS
BEGIN
	SELECT
		[InsuranceMedicalId]		= CAST([InsuranceMedicalId] AS VARCHAR(50)),
		ie.[EmployeeId],
		[InsuranceMedicalNumber],
		[StartDate],
		[ExpiredDate],
		[CityId],
		[MedicalId],
		[Amount],
		ie.[Description],
		ie.[CreateDate],
		ie.[CreateBy],
		[EmployeeCode],
		[FullName],
		[DepartmentId]
	FROM [hrm].[InsuranceMedical] ie
	INNER JOIN	[hrm].[Employee] e ON e.[EmployeeId] = ie.[EmployeeId]
	WHERE YEAR(ie.CreateDate) = @Year AND ie.[EmployeeId] = ISNULL(@EmployeeId,ie.[EmployeeId])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_InsuranceProcess]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_InsuranceProcess]
	@InsuranceProcessId		VARCHAR(50)
AS
BEGIN
	SELECT
		[InsuranceProcessId]	= CAST([InsuranceProcessId] AS VARCHAR(50)),
		ip.[InsuranceId],
		[FromDate],
		[ToDate],
		[Amount],
		ip.[Description],
		ip.[CreateDate],
		ip.[CreateBy],
		i.[InsuranceNumber],
		e.[EmployeeCode],
		e.[FullName]
	FROM [hrm].[InsuranceProcess] ip
	INNER JOIN [hrm].[Insurance] i ON i.[InsuranceId] = ip.[InsuranceId]
	INNER JOIN [hrm].[Employee] e ON i.[EmployeeId] = e.[EmployeeId]
	WHERE [InsuranceProcessId] = @InsuranceProcessId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_InsuranceProcesses]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_InsuranceProcesses]
	@EmployeeId BIGINT
AS
BEGIN
	SELECT
		[InsuranceProcessId]	= CAST([InsuranceProcessId] AS VARCHAR(50)),
		ip.[InsuranceId],
		[FromDate],
		[ToDate],
		[Amount],
		ip.[Description],
		ip.[CreateDate],
		ip.[CreateBy],
		i.[InsuranceNumber],
		e.[EmployeeCode],
		e.[FullName]
	FROM [hrm].[InsuranceProcess] ip
	INNER JOIN [hrm].[Insurance] i ON i.[InsuranceId] = ip.[InsuranceId]
	INNER JOIN [hrm].[Employee] e ON i.[EmployeeId] = e.[EmployeeId]
	WHERE i.[EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Insurances]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_Insurances]
	@IsActive	BIT
AS
BEGIN
	SELECT
		[InsuranceId],
		i.[EmployeeId],
		[InsuranceNumber],
		[SubscriptionDate],
		[CityId],
		[MonthBefore],
		i.[Description],
		i.[IsActive],
		i.[CreateDate],
		i.[CreateBy],
		[EmployeeCode],
		[FullName],
		[DepartmentId]
	FROM [hrm].[Insurance] i
	INNER JOIN [hrm].[Employee] e ON i.[EmployeeId] = e.[EmployeeId]
	WHERE i.[IsActive] = ISNULL(@IsActive,i.[IsActive])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_JobChange]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_JobChange]
	@JobChangeId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[JobChangeId]			= CAST([JobChangeId] AS VARCHAR(50)),
		[JobChangeCode],
		[EmployeeId],
		[FromDepartmentId],
		[ToDepartmentId],
		[FromPositionId],
		[ToPositionId],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate],
		[JobChangeFile],
		[JobChangeNumber]
	FROM [hrm].[JobChange]
	WHERE [JobChangeId] = @JobChangeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_JobChange_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_JobChange_ByEmployeeId]
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[JobChangeId]			= CAST([JobChangeId] AS VARCHAR(50)),
		[JobChangeCode],
		[EmployeeId],
		[FromDepartmentId],
		[ToDepartmentId],
		[FromPositionId],
		[ToPositionId],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate],
		[JobChangeFile],
		[JobChangeNumber]
	FROM [hrm].[JobChange]
	WHERE [EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_JobChanges]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_JobChanges]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[JobChangeId]			= CAST([JobChangeId] AS VARCHAR(50)),
		[JobChangeCode],
		[EmployeeId],
		[FromDepartmentId],
		[ToDepartmentId],
		[FromPositionId],
		[ToPositionId],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate],
		[JobChangeFile],
		[JobChangeNumber]
	FROM [hrm].[JobChange]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Maternities]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_Maternities]
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[MaternityId]			= CAST(m.[MaternityId] AS VARCHAR(50)),
		m.[EmployeeId],
		m.[FromDate],
		m.[ToDate],
		m.[StartTime],
		m.[EndTime],
		m.[RelaxStartTime],
		m.[RelaxEndTime],
		m.[CreateDate],
		m.[CreateBy],
		m.[Description],
		[EmployeeCode]			= e.[EmployeeCode],
		[FullName]				= e.[FullName]
	FROM [hrm].[Maternity] m
	INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = m.[EmployeeId]
	WHERE m.[EmployeeId] = ISNULL(@EmployeeId,m.[EmployeeId])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Maternity]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_Maternity]
	@MaternityId		VARCHAR(50)
AS
BEGIN
	SELECT
		[MaternityId]			= CAST(m.[MaternityId] AS VARCHAR(50)),
		m.[EmployeeId],
		m.[FromDate],
		m.[ToDate],
		m.[StartTime],
		m.[EndTime],
		m.[RelaxStartTime],
		m.[RelaxEndTime],
		m.[CreateDate],
		m.[CreateBy],
		m.[Description],
		[EmployeeCode]			= e.[EmployeeCode],
		[FullName]				= e.[FullName]
	FROM [hrm].[Maternity] m
	INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = m.[EmployeeId]
	WHERE m.[MaternityId] = @MaternityId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Maternitys]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_Maternitys]
AS
BEGIN
	SELECT
		[MaternityId]			= CAST(m.[MaternityId] AS VARCHAR(50)),
		m.[EmployeeId],
		m.[FromDate],
		m.[ToDate],
		m.[StartTime],
		m.[EndTime],
		m.[RelaxStartTime],
		m.[RelaxEndTime],
		m.[CreateDate],
		m.[CreateBy],
		m.[Description],
		[EmployeeCode]			= e.[EmployeeCode],
		[FullName]				= e.[FullName]
	FROM [hrm].[Maternity] m
	INNER JOIN [hrm].[Employee] e ON e.[EmployeeId] = m.[EmployeeId]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Medical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Medical]
	@MedicalId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[MedicalId],
		[MedicalName],
		[Description]
	FROM [hrm].[Medical]
	WHERE [MedicalId] = @MedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Medicals]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Medicals]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[MedicalId],
		[MedicalName],
		[Description]
	FROM [hrm].[Medical]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Nation]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Nation]
	@NationId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[NationId],
		[NationName],
		[Description]
	FROM [hrm].[Nation]
	WHERE [NationId] = @NationId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Nations]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Nations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[NationId],
		[NationName],
		[Description]
	FROM [hrm].[Nation]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Position]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Position]
	@PositionId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PositionId],
		[PositionCode],
		[PositionName],
		[Description]
	FROM [hrm].[Position]
	WHERE [PositionId] = @PositionId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Position_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Position_ByCode]
	@PositionCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PositionId],
		[PositionCode],
		[PositionName],
		[Description]
	FROM [hrm].[Position]
	WHERE PositionCode = @PositionCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Positions]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Positions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PositionId],
		[PositionCode],
		[PositionName],
		[Description]
	FROM [hrm].[Position]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_PraiseDiscipline]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_PraiseDiscipline]
	@PraiseDisciplineId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PraiseDisciplineId]	= CAST([PraiseDisciplineId] AS VARCHAR(50)),
		[PraiseDisciplineCode],
		[PraiseDisciplineType],
		[Title],
		[DecisionNumber],
		[PraiseDisciplineDate],
		[Formality],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate]
	FROM [hrm].[PraiseDiscipline]
	WHERE [PraiseDisciplineId] = @PraiseDisciplineId

	SELECT
		[PraiseDisciplineId]	= CAST([PraiseDisciplineId] AS VARCHAR(50)),
		[EmployeeId]			= pdd.EmployeeId,
		[EmployeeCode]			= e.EmployeeCode,
		[FullName]				= e.FullName,
		[DepartmentId]			= e.DepartmentId
	FROM [hrm].[PraiseDisciplineDetail] pdd
	INNER JOIN [hrm].[Employee] e ON pdd.EmployeeId = e.EmployeeId
	WHERE [PraiseDisciplineId] = @PraiseDisciplineId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_PraiseDisciplines]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_PraiseDisciplines]
	@PraiseDisciplineType	TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PraiseDisciplineId]	= CAST([PraiseDisciplineId] AS VARCHAR(50)),
		[PraiseDisciplineCode],
		[PraiseDisciplineType],
		[Title],
		[DecisionNumber],
		[PraiseDisciplineDate],
		[Formality],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate]
	FROM [hrm].[PraiseDiscipline]
	WHERE [PraiseDisciplineType] = @PraiseDisciplineType
END
GO
/****** Object:  StoredProcedure [hrm].[Get_PraiseDisciplinesByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Get_PraiseDisciplinesByEmployeeId]
	@PraiseDisciplineType	TINYINT,
	@EmployeeId				BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[PraiseDisciplineId]	= CAST(p.[PraiseDisciplineId] AS VARCHAR(50)),
		[PraiseDisciplineCode],
		[PraiseDisciplineType],
		[Title],
		[DecisionNumber],
		[PraiseDisciplineDate],
		[Formality],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate]
	FROM [hrm].[PraiseDiscipline] p
	INNER JOIN hrm.PraiseDisciplineDetail pd On pd.PraiseDisciplineId = p.PraiseDisciplineId
	WHERE [PraiseDisciplineType] = @PraiseDisciplineType AND pd.EmployeeId = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitChanel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitChanel]
	@RecruitChanelId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[RecruitChanelId],
		[ChanelName],
		[Description],
		[IsActive],
		[CreateBy],
		[CreateDate]
	FROM [hrm].[RecruitChanel]
	WHERE [RecruitChanelId] = @RecruitChanelId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitChanels]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitChanels]
	@IsActive	BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[RecruitChanelId],
		[ChanelName],
		[Description],
		[IsActive],
		[CreateBy],
		[CreateDate]
	FROM [hrm].[RecruitChanel]
	WHERE [IsActive] = ISNULL(@IsActive,[IsActive])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitPlan]
	@RecruitPlanId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[RecruitPlanId],
		[RecruitPlanCode],
		[Title],
		[DepartmentId],
		[PositionId],
		[Quantity],
		[FromDate],
		[ToDate],
		[Requirements],
		[ChanelIds],
		[CreateDate],
		[CreateBy],
		[IsActive],
		[Description]
	FROM [hrm].[RecruitPlan]
	WHERE [RecruitPlanId] = @RecruitPlanId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitPlans]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitPlans]
	@IsActive		BIT,
	@FromDate		DATE,
	@ToDate			DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[RecruitPlanId],
		[RecruitPlanCode],
		[Title],
		[DepartmentId],
		[PositionId],
		[Quantity],
		[FromDate],
		[ToDate],
		[Requirements],
		[ChanelIds],
		[CreateDate],
		[CreateBy],
		[IsActive],
		[Description],
		[ApplicantNumber]	= ISNULL((SELECT COUNT(ApplicantId) FROM [hrm].[Applicant] WHERE [RecruitPlanId] = r.[RecruitPlanId]),0),
		[ApplicantPass]		= ISNULL((SELECT COUNT(RecruitResultId) FROM [hrm].[RecruitResult] WHERE [RecruitPlanId] = r.[RecruitPlanId] AND [Result] = 4),0)
	FROM [hrm].[RecruitPlan] r
	WHERE 
			[IsActive] = ISNULL(@IsActive,[IsActive])
		AND
			(
				[FromDate] BETWEEN @FromDate AND @ToDate
			OR
				[ToDate] BETWEEN @FromDate AND @ToDate
			)
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitResult]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitResult]
	@RecruitResultId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[RecruitResultId]	= CAST(rr.[RecruitResultId] AS VARCHAR(50)),
		[ApplicantId]		= CAST(rr.[ApplicantId] AS VARCHAR(50)),
		[RecruitPlanId]		= rr.[RecruitPlanId],
		[Result]			= rr.[Result],
		[Description]		= rr.[Description],
		[CreateDate]		= rr.[CreateDate],
		[CreateBy]			= rr.[CreateBy],
		[EmployeeId]		= rr.[EmployeeId],
		[FullName]			= a.[FullName],
		[Sex]				= a.[Sex],
		[RecruitPlanCode]	= rp.[RecruitPlanCode],
		[DepartmentId]		= rp.[DepartmentId],
		[PositionId]		= rp.[PositionId]
	FROM [hrm].[RecruitResult] rr
	INNER JOIN [hrm].[Applicant] a ON a.[ApplicantId] = rr.[ApplicantId]
	INNER JOIN [hrm].[RecruitPlan] rp ON rp.[RecruitPlanId] = rr.[RecruitPlanId]
	LEFT JOIN [hrm].[Employee] e ON e.[EmployeeId] = rr.[EmployeeId]
	WHERE rr.[RecruitResultId] = @RecruitResultId

	SELECT
		[RecruitResultDetailId]	= CAST(rr.[RecruitResultDetailId] AS VARCHAR(50)),
		[RecruitResultId]		= CAST(rr.[RecruitResultId] AS VARCHAR(50)),
		[EmployeeId],
		[Result],
		[Description],
		[InterviewDate]
	FROM [hrm].[RecruitResultDetail] rr
	WHERE rr.[RecruitResultId] = @RecruitResultId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_RecruitResults]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_RecruitResults]
	@RecruitPlanId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[RecruitResult]
	(
		[RecruitResultId],
		[ApplicantId],
		[RecruitPlanId],
		[Result],
		[Description],
		[CreateDate],
		[CreateBy]
	)
	SELECT
		[RecruitResultId]	= NEWID(),
		[ApplicantId]		= [ApplicantId],
		[RecruitPlanId]		= [RecruitPlanId],
		[Result]			= 1,
		[Description]		= '',
		[CreateDate]		= GETDATE(),
		[CreateBy]			= [CreateBy]	
	FROM [hrm].[Applicant]
	WHERE 
			[RecruitPlanId] = @RecruitPlanId
		AND
			[ApplicantId] NOT IN (SELECT [ApplicantId] FROM [hrm].[RecruitResult] WHERE [RecruitPlanId] = @RecruitPlanId)

	SELECT
		[RecruitResultId]	= CAST(rr.[RecruitResultId] AS VARCHAR(50)),
		[ApplicantId]		= CAST(rr.[ApplicantId] AS VARCHAR(50)),
		[RecruitPlanId]		= rr.[RecruitPlanId],
		[Result]			= rr.[Result],
		[Description]		= rr.[Description],
		[CreateDate]		= rr.[CreateDate],
		[CreateBy]			= rr.[CreateBy],
		[EmployeeId]		= rr.[EmployeeId],
		[FullName]			= a.[FullName],
		[Sex]				= a.[Sex],
		[RecruitPlanCode]	= rp.[RecruitPlanCode],
		[DepartmentId]		= rp.[DepartmentId],
		[PositionId]		= rp.[PositionId]
	FROM [hrm].[RecruitResult] rr
	INNER JOIN [hrm].[Applicant] a ON a.[ApplicantId] = rr.[ApplicantId]
	INNER JOIN [hrm].[RecruitPlan] rp ON rp.[RecruitPlanId] = rr.[RecruitPlanId]
	LEFT JOIN [hrm].[Employee] e ON e.[EmployeeId] = rr.[EmployeeId]
	WHERE rr.[RecruitPlanId] = @RecruitPlanId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Religion]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Religion]
	@ReligionId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ReligionId],
		[ReligionName],
		[Description]
	FROM [hrm].[Religion]
	WHERE [ReligionId] = @ReligionId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Religions]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Religions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ReligionId],
		[ReligionName],
		[Description]
	FROM [hrm].[Religion]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Salaries]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Salaries]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[EmployeeId],
		[EmployeeCode],
		[FullName],
		[DepartmentId],
		[BasicSalary]				= ISNULL((SELECT TOP 1 [BasicSalary] FROM [hrm].[Salary] WHERE [EmployeeId] = e.[EmployeeId] AND [ApplyDate] <= GETDATE() ORDER BY [ApplyDate] DESC),0),
		[BasicCoefficient]			= ISNULL((SELECT TOP 1 [BasicCoefficient] FROM [hrm].[Salary] WHERE [EmployeeId] = e.[EmployeeId] AND [ApplyDate] <= GETDATE() ORDER BY [ApplyDate] DESC),0),
		[ProfessionalCoefficient]	= ISNULL((SELECT TOP 1 [ProfessionalCoefficient] FROM [hrm].[Salary] WHERE [EmployeeId] = e.[EmployeeId] AND [ApplyDate] <= GETDATE() ORDER BY [ApplyDate] DESC),0),
		[ResponsibilityCoefficient]	= ISNULL((SELECT TOP 1 [ResponsibilityCoefficient] FROM [hrm].[Salary] WHERE [EmployeeId] = e.[EmployeeId] AND [ApplyDate] <= GETDATE() ORDER BY [ApplyDate] DESC),0),
		[PercentProfessional]		= ISNULL((SELECT TOP 1 [PercentProfessional] FROM [hrm].[Salary] WHERE [EmployeeId] = e.[EmployeeId] AND [ApplyDate] <= GETDATE() ORDER BY [ApplyDate] DESC),0)
	FROM [hrm].[Employee] e
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Salaries_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Salaries_ByEmployeeId]
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[SalaryId]					= CAST([SalaryId] AS VARCHAR(50)),
		[EmployeeId],
		[BasicSalary],
		[BasicCoefficient],
		[ProfessionalCoefficient],
		[ResponsibilityCoefficient],
		[PercentProfessional],
		[ApplyDate],
		[CreateDate],
		[CreateBy]
	FROM [hrm].[Salary]
	WHERE [EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Salary]
	@SalaryId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[SalaryId]					= CAST([SalaryId] AS VARCHAR(50)),
		[EmployeeId],
		[BasicSalary],
		[BasicCoefficient],
		[ProfessionalCoefficient],
		[ResponsibilityCoefficient],
		[PercentProfessional],
		[ApplyDate],
		[CreateDate],
		[CreateBy]
	FROM [hrm].[Salary]
	WHERE [SalaryId] = @SalaryId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Salary_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Salary_ByEmployeeId]
	@EmployeeId		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT TOP 1
		[SalaryId]					= CAST([SalaryId] AS VARCHAR(50)),
		[EmployeeId],
		[BasicSalary],
		[BasicCoefficient],
		[ProfessionalCoefficient],
		[ResponsibilityCoefficient],
		[PercentProfessional],
		[ApplyDate],
		[CreateDate],
		[CreateBy]
	FROM [hrm].[Salary]
	WHERE		[EmployeeId] = @EmployeeId
	ORDER BY	 [ApplyDate]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_School]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_School]
	@SchoolId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[SchoolId],
		[SchoolName],
		[Description]
	FROM [hrm].[School]
	WHERE [SchoolId] = @SchoolId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_Schools]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_Schools]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[SchoolId],
		[SchoolName],
		[Description]
	FROM [hrm].[School]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_ShiftWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_ShiftWork]
	@ShiftWorkId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ShiftWorkId],
		[ShiftWorkCode],
		[StartTime],
		[EndTime],
		[RelaxStartTime],
		[RelaxEndTime],
		[Description]
	FROM [hrm].[ShiftWork]
	WHERE [ShiftWorkId] = @ShiftWorkId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_ShiftWork_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_ShiftWork_ByCode]
	@ShiftWorkCode	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ShiftWorkId],
		[ShiftWorkCode],
		[StartTime],
		[EndTime],
		[RelaxStartTime],
		[RelaxEndTime],
		[Description]
	FROM [hrm].[ShiftWork]
	WHERE [ShiftWorkCode] = @ShiftWorkCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_ShiftWorks]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_ShiftWorks]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[ShiftWorkId],
		[ShiftWorkCode],
		[StartTime],
		[EndTime],
		[RelaxStartTime],
		[RelaxEndTime],
		[Description]
	FROM [hrm].[ShiftWork]
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TimeSheet_ByEmployeeId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_TimeSheet_ByEmployeeId]
	@TimeSheetDate	DATE,
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[TimeSheetId]		= CAST([TimeSheetId] AS VARCHAR(50)),
		[EmployeeId]		= ts.[EmployeeId],
		[TimeSheetDate],
		[Checkin],
		[Checkout],
		[EmployeeCode]		= e.[EmployeeCode],
		[FullName]			= e.[FullName],
		[EndTime]			= CASE WHEN mt.EndTime IS NULL THEN s.EndTime ELSE mt.EndTime END,
		[StartTime]			= CASE WHEN mt.StartTime IS NULL THEN s.StartTime ELSE mt.StartTime END,
		[RelaxStartTime]	= CASE WHEN mt.RelaxStartTime IS NULL THEN s.RelaxStartTime ELSE mt.RelaxStartTime END,
		[RelaxEndTime]		= CASE WHEN mt.RelaxEndTime IS NULL THEN s.RelaxEndTime ELSE mt.RelaxEndTime END,
		[DepartmentName]	= d.[DepartmentName],
		[ShiftWorkId]		= ts.ShiftWorkId
	FROM [hrm].[TimeSheet] ts
	INNER JOIN [hrm].[Employee]  e ON e.[EmployeeId] = ts.[EmployeeId]
	INNER JOIN [hrm].[Department] d ON d.[DepartmentId] = e.[DepartmentId]
	INNER JOIN [hrm].[ShiftWork] s ON s.[ShiftWorkId] = ts.[ShiftWorkId]
	LEFT JOIN [hrm].[Maternity] mt ON mt.[EmployeeId] = ts.[EmployeeId]
	WHERE CAST([TimeSheetDate] AS DATE) = CAST(@TimeSheetDate AS DATE) AND ts.[EmployeeId] = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TimeSheet_CheckDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_TimeSheet_CheckDate]
	@TimeSheetDate DATETIME
AS
BEGIN
	SELECT
		TOP 1 *
	FROM [hrm].[TimeSheet] ts
	WHERE [TimeSheetDate] = @TimeSheetDate 
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TimeSheetOt]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Get_TimeSheetOt]
	@TimeSheetOtId		VARCHAR(50)
AS
BEGIN
	SELECT
		[TimeSheetOtId]				= CAST([TimeSheetOtId] AS VARCHAR(50)),
		[DayDate],
		[EmployeeId]				= ts.[EmployeeId],
		[Hours],
		[CoefficientPoint],
		[DayPoints],
		[CreateDate],
		[Description]				= ts.[Description],
		[EmployeeCode]				= ued.[EmployeeCode],
		[FullName]					= ued.[FullName],
		[DepartmentName]			= ued.[DepartmentName]
	FROM [hrm].[TimeSheetOt] ts
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[EmployeeId] =  ts.[EmployeeId]
	WHERE [TimeSheetOtId] = @TimeSheetOtId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TimeSheetOts]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_TimeSheetOts]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@EmployeeId		BIGINT
AS
BEGIN
	SELECT
		[TimeSheetOtId]				= CAST([TimeSheetOtId] AS VARCHAR(50)),
		[DayDate],
		[EmployeeId]				= ts.[EmployeeId],
		[Hours],
		[CoefficientPoint],
		[DayPoints],
		[CreateDate],
		[Description]				= ts.[Description],
		[EmployeeCode]				= ued.[EmployeeCode],
		[FullName]					= ued.[FullName],
		[DepartmentName]			= ued.[DepartmentName]
	FROM [hrm].[TimeSheetOt] ts
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[EmployeeId] =  ts.[EmployeeId]
	WHERE 
			CAST([DayDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([DayDate] AS DATE)) AND ISNULL(@ToDate,CAST([DayDate] AS DATE))
		AND
			ts.[EmployeeId] = ISNULL(@EmployeeId,ts.[EmployeeId])
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TimeSheets]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Get_TimeSheets]
	@TimeSheetDate DATETIME
AS
BEGIN
	SELECT
		[TimeSheetId]		= CAST([TimeSheetId] AS VARCHAR(50)),
		[EmployeeId]		= ts.[EmployeeId],
		[TimeSheetDate],
		[Checkin],
		[Checkout],
		[EmployeeCode]		= e.[EmployeeCode],
		[FullName]			= e.[FullName],
		[EndTime]			= CASE WHEN mt.EndTime IS NULL THEN s.EndTime ELSE mt.EndTime END,
		[StartTime]			= CASE WHEN mt.StartTime IS NULL THEN s.StartTime ELSE mt.StartTime END,
		[RelaxStartTime]	= CASE WHEN mt.RelaxStartTime IS NULL THEN s.RelaxStartTime ELSE mt.RelaxStartTime END,
		[RelaxEndTime]		= CASE WHEN mt.RelaxEndTime IS NULL THEN s.RelaxEndTime ELSE mt.RelaxEndTime END,
		[DepartmentName]	= d.[DepartmentName]
	FROM [hrm].[TimeSheet] ts
	INNER JOIN [hrm].[Employee]  e ON e.[EmployeeId] = ts.[EmployeeId]
	INNER JOIN [hrm].[Department] d ON d.[DepartmentId] = e.[DepartmentId]
	LEFT JOIN [hrm].[ShiftWork] s ON s.[ShiftWorkId] = e.[ShiftWorkId]
	LEFT JOIN [hrm].[Maternity] mt ON mt.[EmployeeId] = ts.[EmployeeId]
	WHERE [TimeSheetDate] = @TimeSheetDate 
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TrainingLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_TrainingLevel]
	@TrainingLevelId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[TrainingLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[TrainingLevel]
	WHERE [TrainingLevelId] = @TrainingLevelId
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TrainingLevel_ByCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_TrainingLevel_ByCode]
	@LevelCode		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[TrainingLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[TrainingLevel]
	WHERE LevelCode = @LevelCode
END
GO
/****** Object:  StoredProcedure [hrm].[Get_TrainingLevels]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Get_TrainingLevels]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[TrainingLevelId],
		[LevelCode],
		[LevelName],
		[Description]
	FROM [hrm].[TrainingLevel]
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Applicant]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Applicant]
	@ApplicantId		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Sex				TINYINT,
	@DateOfBirth		DATE,
	@CountryId			INT,
	@NationId			INT,
	@ReligionId			INT,
	@CityBirthPlace		INT,
	@PermanentAddress	NVARCHAR(255),
	@IdentityCardNumber	VARCHAR(50),
	@PhoneNumber		VARCHAR(50),
	@Email				NVARCHAR(255),
	@ChanelId			INT,
	@TrainingLevelId	INT,
	@RecruitPlanId		BIGINT,
	@CvDate				DATE,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Applicant]
	(
		[ApplicantId],
		[FullName],
		[Sex],
		[DateOfBirth],
		[CountryId],
		[NationId],
		[ReligionId],
		[CityBirthPlace],
		[PermanentAddress],
		[IdentityCardNumber],
		[PhoneNumber],
		[Email],
		[ChanelId],
		[TrainingLevelId],
		[RecruitPlanId],
		[CvDate],
		[CreateDate],
		[CreateBy],
		[Description]
	)
	VALUES
	(
		@ApplicantId,
		@FullName,
		@Sex,
		@DateOfBirth,
		@CountryId,
		@NationId,
		@ReligionId,
		@CityBirthPlace,
		@PermanentAddress,
		@IdentityCardNumber,
		@PhoneNumber,
		@Email,
		@ChanelId,
		@TrainingLevelId,
		@RecruitPlanId,
		@CvDate,
		@CreateDate,
		@CreateBy,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Career]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Career]
	@CareerId			INT OUTPUT,
	@CareerName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Career]
	(
		[CareerName],
		[Description]
	)
	VALUES
	(
		@CareerName,
		@Description
	)
	SET @CareerId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Contract]
	@ContractId			VARCHAR(50),
	@ContractCode		VARCHAR(50),
	@EmployeeId			BIGINT,
	@StartDate			DATE,
	@EndDate			DATE,
	@ContractTypeId		INT,
	@ContractFile		NVARCHAR(255),
	@ContractOthorFile	NVARCHAR(255),
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	INSERT INTO [hrm].[Contract]
	(
		[ContractId],
		[ContractCode],
		[EmployeeId],
		[StartDate],
		[EndDate],
		[ContractTypeId],
		[ContractFile],
		[ContractOthorFile],
		[CreateBy],
		[CreateDate],
		[Description]
	)
	VALUES
	(
		@ContractId,
		@ContractCode,
		@EmployeeId,
		@StartDate,
		@EndDate,
		@ContractTypeId,
		@ContractFile,
		@ContractOthorFile,
		@CreateBy,
		@CreateDate,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_ContractType]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_ContractType]
	@ContractTypeId		INT OUTPUT,
	@TypeName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@IsActive			BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[ContractType]
	(
		[TypeName],
		[Description],
		[IsActive]
	)
	VALUES
	(
		@TypeName,
		@Description,
		@IsActive
	)
	SET @ContractTypeId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Department]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Department]
	@DepartmentId		BIGINT OUTPUT,
	@DepartmentCode		VARCHAR(50),
	@DepartmentName		NVARCHAR(255),
	@ParentId			BIGINT,
	@Description		NVARCHAR(500),
	@IsActive			BIT,
	@Path				VARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Department]
	(
		[DepartmentCode],
		[DepartmentName],
		[ParentId],
		[Description],
		[IsActive],
		[Path]
	)
	VALUES
	(
		@DepartmentCode,
		@DepartmentName,
		@ParentId,
		@Description,
		@IsActive,
		@Path
	)
	SET @DepartmentId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_EducationLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_EducationLevel]
	@EducationLevelId	INT OUTPUT,
	@LevelCode			VARCHAR(50),
	@LevelName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[EducationLevel]
	(
		[LevelCode],
		[LevelName],
		[Description]
	)
	VALUES
	(
		@LevelCode,
		@LevelName,
		@Description
	)
	SET @EducationLevelId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Employee]
	@EmployeeId					BIGINT OUTPUT,
	@EmployeeCode				VARCHAR(50) OUTPUT,
	@FullName					NVARCHAR(255),
	@DateOfBirth				DATE,
	@Gender						TINYINT,
	@SpecialName				NVARCHAR(255),
	@Avatar						NVARCHAR(255),
	@DepartmentId				BIGINT,
	@CountryId					INT,
	@NationId					INT,
	@ReligionId					INT,
	@MaritalStatus				TINYINT,
	@CityBirthPlace				INT,
	@CityNativeLand				INT,
	@IdentityCardNumber			VARCHAR(50),
	@IdentityCardDate			DATE,
	@CityIdentityCard			INT,
	@PermanentAddress			NVARCHAR(255),
	@PermanentCity				INT,
	@PermanentDistrict			INT,
	@TemperaryAddress			NVARCHAR(255),
	@TemperaryCity				INT,
	@TemperaryDistrict			INT,
	@Email						NVARCHAR(255),
	@PhoneNumber				VARCHAR(50),
	@PositionId					INT,
	@TrainingLevelId			INT,
	@HealthStatus				NVARCHAR(500),
	@DateOfYouthUnionAdmission	DATE,
	@PlaceOfYouthUnionAdmission	NVARCHAR(500),
	@DateOfPartyAdmission		DATE,
	@PlaceOfPartyAdmission		NVARCHAR(500),
	@Skill						NVARCHAR(1000),
	@Experience					NVARCHAR(1000),
	@Description				NVARCHAR(1000),
	@CreateBy					INT,
	@CreateDate					DATETIME,
	@IsActive					BIT,
	@Status						TINYINT,
	@ShiftWorkId				INT,
	@WorkedDate					DATE,
	@EducationLevelId			INT,
	@SchoolId					INT,
	@CareerId					INT,
	@TimeSheetCode				VARCHAR(50),
	@DepartmentCompany			INT,
	@CategoryKpiId				INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @EmployeeCode,@EmployeeCode  OUTPUT 
	SELECT @EmployeeCode 
	INSERT INTO [hrm].[Employee]
	(
		[EmployeeCode],
		[FullName],
		[DateOfBirth],
		[Gender],
		[SpecialName],
		[Avatar],
		[DepartmentId],
		[CountryId],
		[NationId],
		[ReligionId],
		[MaritalStatus],
		[CityBirthPlace],
		[CityNativeLand],
		[IdentityCardNumber],
		[IdentityCardDate],
		[CityIdentityCard],
		[PermanentAddress],
		[PermanentCity],
		[PermanentDistrict],
		[TemperaryAddress],
		[TemperaryCity],
		[TemperaryDistrict],
		[Email],
		[PhoneNumber],
		[PositionId],
		[TrainingLevelId],
		[HealthStatus],
		[DateOfYouthUnionAdmission],
		[PlaceOfYouthUnionAdmission],
		[DateOfPartyAdmission],
		[PlaceOfPartyAdmission],
		[Skill],
		[Experience],
		[Description],
		[CreateBy],
		[CreateDate],
		[IsActive],
		[Status],
		[ShiftWorkId],
		[WorkedDate],
		[EducationLevelId],
		[SchoolId],
		[CareerId],
		[TimeSheetCode],
		[DepartmentCompany],
		[CategoryKpiId]

	)
	VALUES
	(
		@EmployeeCode,
		@FullName,
		@DateOfBirth,
		@Gender,
		@SpecialName,
		@Avatar,
		@DepartmentId,
		@CountryId,
		@NationId,
		@ReligionId,
		@MaritalStatus,
		@CityBirthPlace,
		@CityNativeLand,
		@IdentityCardNumber,
		@IdentityCardDate,
		@CityIdentityCard,
		@PermanentAddress,
		@PermanentCity,
		@PermanentDistrict,
		@TemperaryAddress,
		@TemperaryCity,
		@TemperaryDistrict,
		@Email,
		@PhoneNumber,
		@PositionId,
		@TrainingLevelId,
		@HealthStatus,
		@DateOfYouthUnionAdmission,
		@PlaceOfYouthUnionAdmission,
		@DateOfPartyAdmission,
		@PlaceOfPartyAdmission,
		@Skill,
		@Experience,
		@Description,
		@CreateBy,
		@CreateDate,
		@IsActive,
		@Status,
		@ShiftWorkId,
		@WorkedDate,
		@EducationLevelId,
		@SchoolId,
		@CareerId,
		@TimeSheetCode,
		@DepartmentCompany,
		@CategoryKpiId

	)
	SET @EmployeeId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_EmployeeHoliday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_EmployeeHoliday]
	@EmployeeHolidayId		VARCHAR(50),
	@HolidayReasonId		INT,
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Description			NVARCHAR(MAX),
	@EmployeeId				BIGINT,
	@CreateDate				DATETIME
AS

BEGIN
	INSERT INTO [hrm].[EmployeeHoliday]
	(
		[EmployeeHolidayId],
		[HolidayReasonId],
		[FromDate],
		[ToDate],
		[Description],
		[EmployeeId],
		[CreateDate]
	)
	VALUES
	(
		@EmployeeHolidayId,
		@HolidayReasonId,
		@FromDate,
		@ToDate,
		@Description,
		@EmployeeId,
		@CreateDate
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Holiday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Holiday]
	@XML	NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/Holiday'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml
	INSERT INTO [hrm].[Holiday]
	(
		[HolidayId],
		[HolidayDate]
	)
	SELECT
		[HolidayId],
		[HolidayDate]
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH(
		[HolidayId]		VARCHAR(50),
		[HolidayDate]	DATE
	) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_HolidayConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_HolidayConfig]
	@EmployeeId			BIGINT,
	@Year				INT,
	@HolidayNumber		INT
AS

BEGIN
	INSERT INTO [hrm].[HolidayConfig]
	(
		[EmployeeId],
		[Year],
		[HolidayNumber]
		)
	VALUES
	(
		@EmployeeId,
		@Year,
		@HolidayNumber
	)	
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_HolidayDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [hrm].[Insert_HolidayDetails]
	@EmployeeHolidayId				VARCHAR(50),
	@XML							NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/HolidayDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [hrm].[HolidayDetail] WHERE  [EmployeeHolidayId] = @EmployeeHolidayId

	INSERT INTO [hrm].[HolidayDetail]
	(
		[HolidayDetailId],
		[DateDay],
		[NumberDays],
		[Permission],
		[PercentSalary],
		[ToTalDays],
		[EmployeeHolidayId]
	)
	SELECT 
		[HolidayDetailId],
		[DateDay],
		[NumberDays],
		[Permission],
		[PercentSalary],
		[ToTalDays],
		[EmployeeHolidayId]		= @EmployeeHolidayId

	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
			HolidayDetailId				VARCHAR(50),
			DateDay						DATETIME,
	        NumberDays					DECIMAL(18,1) ,
			Permission					DECIMAL(18,1),
			PercentSalary				DECIMAL(18,2),
			ToTalDays					DECIMAL(18,2),
			EmployeeHolidayId			VARCHAR(50)

	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_HolidayReason]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_HolidayReason]
	@HolidayReasonId		INT OUTPUT,
	@ReasonCode				VARCHAR(50),
	@ReasonName				NVARCHAR(255),
	@Description			NVARCHAR(500),
	@IsActive				BIT,
	@PercentSalary			DECIMAL(18,2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[HolidayReason]
	(
		[ReasonCode],
		[ReasonName],
		[Description],
		[IsActive],
		[PercentSalary]
	)
	VALUES
	(
		@ReasonCode,
		@ReasonName,
		@Description,
		@IsActive,
		@PercentSalary
	)
	SET @HolidayReasonId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_IncurredSalary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_IncurredSalary]
	@IncurredSalaryId		VARCHAR(50),
	@EmployeeId				BIGINT,
	@Amount					MONEY,
	@Title					NVARCHAR(255),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@SubmitDate				DATE,
	@Description			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN
		INSERT INTO [hrm].[IncurredSalary]
		(
			[IncurredSalaryId],
			[EmployeeId],
			[Amount],
			[Title],
			[CreateDate],
			[CreateBy],
			[SubmitDate],
			[Description]
		)
		VALUES
		(
			@IncurredSalaryId,
			@EmployeeId,
			@Amount,
			@Title,
			@CreateDate,
			@CreateBy,
			@SubmitDate,
			@Description
		)
	END
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Insurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_Insurance]
	@InsuranceId		BIGINT OUTPUT,
	@EmployeeId			BIGINT,
	@InsuranceNumber	VARCHAR(50),
	@SubscriptionDate	DATE,
	@CityId				INT,
	@MonthBefore		INT,
	@Description		NVARCHAR(500),
	@IsActive			BIT,
	@CreateDate			DATETIME,
	@CreateBy			INT
AS
BEGIN
	INSERT INTO [hrm].[Insurance]
	(
		[EmployeeId],
		[InsuranceNumber],
		[SubscriptionDate],
		[CityId],
		[MonthBefore],
		[Description],
		[IsActive],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@EmployeeId,
		@InsuranceNumber,
		@SubscriptionDate,
		@CityId,
		@MonthBefore,
		@Description,
		@IsActive,
		@CreateDate,
		@CreateBy
	)
	SET @InsuranceId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_InsuranceMedical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_InsuranceMedical]
	@InsuranceMedicalId			VARCHAR(50),
	@EmployeeId					BIGINT,
	@InsuranceMedicalNumber		VARCHAR(50),
	@StartDate					DATE,
	@ExpiredDate				DATE,
	@CityId						INT,
	@MedicalId					INT,
	@Amount						MONEY,
	@Description				NVARCHAR(500),
	@CreateDate					DATETIME,
	@CreateBy					INT
AS
BEGIN
	INSERT INTO [hrm].[InsuranceMedical]
	(
		[InsuranceMedicalId],
		[EmployeeId],
		[InsuranceMedicalNumber],
		[StartDate],
		[ExpiredDate],
		[CityId],
		[MedicalId],
		[Amount],
		[Description],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@InsuranceMedicalId,
		@EmployeeId,
		@InsuranceMedicalNumber,
		@StartDate,
		@ExpiredDate,
		@CityId,
		@MedicalId,
		@Amount,
		@Description,
		@CreateDate,
		@CreateBy
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_InsuranceProcess]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_InsuranceProcess]
	@InsuranceProcessId		VARCHAR(50),
	@InsuranceId			BIGINT,
	@FromDate				DATE,
	@ToDate					DATE,
	@Amount					MONEY,
	@Description			NVARCHAR(500),
	@CreateDate				DATETIME,
	@CreateBy				INT
AS
BEGIN
	INSERT INTO [hrm].[InsuranceProcess]
	(
		[InsuranceProcessId],
		[InsuranceId],
		[FromDate],
		[ToDate],
		[Amount],
		[Description],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@InsuranceProcessId,
		@InsuranceId,
		@FromDate,
		@ToDate,
		@Amount,
		@Description,
		@CreateDate,
		@CreateBy
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_JobChange]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_JobChange]
	@JobChangeId		VARCHAR(50),
	@JobChangeCode		VARCHAR(50) OUTPUT,
	@EmployeeId			BIGINT,
	@FromDepartmentId	BIGINT,
	@ToDepartmentId		BIGINT,
	@FromPositionId		INT,
	@ToPositionId		INT,
	@Reason				NVARCHAR(500),
	@Description		NVARCHAR(50),
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@JobChangeFile		NVARCHAR(255),
	@JobChangeNumber	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC [dbo].[Generate_AutoNumber] @JobChangeCode,@JobChangeCode OUTPUT
	SELECT @JobChangeCode
	INSERT INTO [hrm].[JobChange]
	(
		[JobChangeId],
		[JobChangeCode],
		[EmployeeId],
		[FromDepartmentId],
		[ToDepartmentId],
		[FromPositionId],
		[ToPositionId],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate],
		[JobChangeFile],
		[JobChangeNumber]
	)
	VALUES
	(
		@JobChangeId,
		@JobChangeCode,
		@EmployeeId,
		@FromDepartmentId,
		@ToDepartmentId,
		@FromPositionId,
		@ToPositionId,
		@Reason,
		@Description,
		@CreateBy,
		@CreateDate,
		@JobChangeFile,
		@JobChangeNumber
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Maternity]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Insert_Maternity]
	@MaternityId		VARCHAR(50),
	@EmployeeId			BIGINT,
	@FromDate			DATE,
	@ToDate				DATE,
	@StartTime			VARCHAR(50),
	@EndTime			VARCHAR(50),
	@RelaxStartTime		VARCHAR(50),
	@RelaxEndTime		VARCHAR(50),
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@Description		NVARCHAR(500)
AS

BEGIN
	INSERT INTO [hrm].[Maternity]
	(
		[MaternityId],
		[EmployeeId],
		[FromDate],
		[ToDate],
		[StartTime],
		[EndTime],
		[RelaxStartTime],
		[RelaxEndTime],
		[CreateDate],
		[CreateBy],
		[Description]
	)
	VALUES
	(
		@MaternityId,
		@EmployeeId,
		@FromDate,
		@ToDate,
		@StartTime,
		@EndTime,
		@RelaxStartTime,
		@RelaxEndTime,
		@CreateDate,
		@CreateBy,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Medical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Medical]
	@MedicalId		INT OUTPUT,
	@MedicalName	NVARCHAR(255),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Medical]
	(
		[MedicalName],
		[Description]
	)
	VALUES
	(
		@MedicalName,
		@Description
	)
	SET @MedicalId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Nation]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Nation]
	@NationId			INT OUTPUT,
	@NationName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Nation]
	(
		[NationName],
		[Description]
	)
	VALUES
	(
		@NationName,
		@Description
	)
	SET @NationId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Position]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Position]
	@PositionId			INT OUTPUT,
	@PositionCode		VARCHAR(50),
	@PositionName		NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Position]
	(
		[PositionCode],
		[PositionName],
		[Description]
	)
	VALUES
	(
		@PositionCode,
		@PositionName,
		@Description
	)
	SET @PositionId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_PraiseDiscipline]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_PraiseDiscipline]
	@PraiseDisciplineId		VARCHAR(50),
	@PraiseDisciplineCode	VARCHAR(50)	OUTPUT,
	@PraiseDisciplineType	TINYINT,
	@Title					NVARCHAR(255),
	@DecisionNumber			VARCHAR(50),
	@PraiseDisciplineDate	DATE,
	@Formality				NVARCHAR(255),
	@Reason					NVARCHAR(500),
	@Description			NVARCHAR(500),
	@CreateBy				INT,
	@CreateDate				DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @PraiseDisciplineCode,@PraiseDisciplineCode OUTPUT
	SELECT @PraiseDisciplineCode
	INSERT INTO [hrm].[PraiseDiscipline]
	(
		[PraiseDisciplineId],
		[PraiseDisciplineCode],
		[PraiseDisciplineType],
		[Title],
		[DecisionNumber],
		[PraiseDisciplineDate],
		[Formality],
		[Reason],
		[Description],
		[CreateBy],
		[CreateDate]
	)
	VALUES
	(
		@PraiseDisciplineId,
		@PraiseDisciplineCode,
		@PraiseDisciplineType,
		@Title,
		@DecisionNumber,
		@PraiseDisciplineDate,
		@Formality,
		@Reason,
		@Description,
		@CreateBy,
		@CreateDate
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_PraiseDisciplineDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_PraiseDisciplineDetail]
	@PraiseDisciplineId		VARCHAR(50),
	@XML					NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/PraiseDisciplineDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [hrm].[PraiseDisciplineDetail] WHERE  [PraiseDisciplineId] = @PraiseDisciplineId

	INSERT INTO [hrm].[PraiseDisciplineDetail]
	(
		[PraiseDisciplineId],
		[EmployeeId]
	)
	SELECT 
		[PraiseDisciplineId]	= @PraiseDisciplineId,
		[EmployeeId]			= x.EmployeeId
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
	        EmployeeId		BIGINT 
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_RecruitChanel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_RecruitChanel]
	@RecruitChanelId	INT OUTPUT,
	@ChanelName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@IsActive			BIT,
	@CreateBy			INT,
	@CreateDate			DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[RecruitChanel]
	(
		[ChanelName],
		[Description],
		[IsActive],
		[CreateBy],
		[CreateDate]
	)
	VALUES
	(
		@ChanelName,
		@Description,
		@IsActive,
		@CreateBy,
		@CreateDate
	)
	SET @RecruitChanelId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_RecruitPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_RecruitPlan]
	@RecruitPlanId			BIGINT OUTPUT,
	@RecruitPlanCode		VARCHAR(50) OUTPUT,
	@Title					NVARCHAR(255),
	@DepartmentId			BIGINT,
	@PositionId				INT,
	@Quantity				INT,
	@FromDate				DATE,
	@ToDate					DATE,
	@Requirements			NVARCHAR(MAX),
	@ChanelIds				VARCHAR(50),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@IsActive				BIT,
	@Description			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @RecruitPlanCode,@RecruitPlanCode OUTPUT
	SELECT @RecruitPlanCode
	INSERT INTO [hrm].[RecruitPlan]
	(
		[RecruitPlanCode],
		[Title],
		[DepartmentId],
		[PositionId],
		[Quantity],
		[FromDate],
		[ToDate],
		[Requirements],
		[ChanelIds],
		[CreateDate],
		[CreateBy],
		[IsActive],
		[Description]
	)
	VALUES
	(
		@RecruitPlanCode,
		@Title,
		@DepartmentId,
		@PositionId,
		@Quantity,
		@FromDate,
		@ToDate,
		@Requirements,
		@ChanelIds,
		@CreateDate,
		@CreateBy,
		@IsActive,
		@Description
	)
	SET @RecruitPlanId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_RecruitResult]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_RecruitResult]
	@RecruitResultId		VARCHAR(50),
	@ApplicantId			VARCHAR(50),
	@RecruitPlanId			BIGINT,
	@Result					TINYINT,
	@Description			NVARCHAR(1000),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@EmployeeId				BIGINT,
	@XML					NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/RecruitResultDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [hrm].[RecruitResultDetail] WHERE [RecruitResultId] = @RecruitResultId

	INSERT INTO [hrm].[RecruitResult]
	(
		[RecruitResultId],
		[ApplicantId],
		[RecruitPlanId],
		[Result],
		[Description],
		[CreateDate],
		[CreateBy],
		[EmployeeId]
	)
	VALUES
	(
		@RecruitResultId,
		@ApplicantId,
		@RecruitPlanId,
		@Result,
		@Description,
		@CreateDate,
		@CreateBy,
		@EmployeeId
	)

	INSERT INTO [hrm].[RecruitResultDetail]
	(
		[RecruitResultDetailId],
		[RecruitResultId],
		[EmployeeId],
		[Result],
		[Description],
		[InterviewDate]
	)
	SELECT
		[RecruitResultDetailId]	= NEWID(),
		[RecruitResultId]		= @RecruitResultId,
		[EmployeeId]			= x.[EmployeeId],
		[Result]				= x.[Result],
		[Description]			= x.[Description],
		[InterviewDate]			= x.[InterviewDate]
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH (
			[EmployeeId]			BIGINT,
			[Result]				TINYINT,
			[Description]			NVARCHAR(1000),
			[InterviewDate]			DATETIME
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Religion]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Religion]
	@ReligionId		INT OUTPUT,
	@ReligionName	NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Religion]
	(
		[ReligionName],
		[Description]
	)
	VALUES
	(
		@ReligionName,
		@Description
	)
	SET @ReligionId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_Salary]
	@SalaryId					VARCHAR(50),
	@EmployeeId					BIGINT,
	@BasicSalary				MONEY,
	@BasicCoefficient			DECIMAL(18,2),
	@ProfessionalCoefficient	DECIMAL(18,2),
	@ResponsibilityCoefficient	DECIMAL(18,2),
	@PercentProfessional		DECIMAL(18,1),
	@ApplyDate					DATE,
	@CreateDate					DATETIME,
	@CreateBy					INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[Salary]
	(
		[SalaryId],
		[EmployeeId],
		[BasicSalary],
		[BasicCoefficient],
		[ProfessionalCoefficient],
		[ResponsibilityCoefficient],
		[PercentProfessional],
		[ApplyDate],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@SalaryId,
		@EmployeeId,
		@BasicSalary,
		@BasicCoefficient,
		@ProfessionalCoefficient,
		@ResponsibilityCoefficient,
		@PercentProfessional,
		@ApplyDate,
		@CreateDate,
		@CreateBy
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_School]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_School]
	@SchoolId			INT OUTPUT,
	@SchoolName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[School]
	(
		[SchoolName],
		[Description]
	)
	VALUES
	(
		@SchoolName,
		@Description
	)
	SET @SchoolId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_ShiftWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_ShiftWork]
	@ShiftWorkId	INT OUTPUT,
	@ShiftWorkCode	VARCHAR(50),
	@StartTime		VARCHAR(50),
	@EndTime		VARCHAR(50),
	@RelaxStartTime	VARCHAR(50),
	@RelaxEndTime	VARCHAR(50),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[ShiftWork]
	(
		[ShiftWorkCode],
		[StartTime],
		[EndTime],
		[RelaxStartTime],
		[RelaxEndTime],
		[Description]
	)
	VALUES
	(
		@ShiftWorkCode,
		@StartTime,
		@EndTime,
		@RelaxStartTime,
		@RelaxEndTime,
		@Description
	)
	SET @ShiftWorkId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_TimeSheetOt]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Insert_TimeSheetOt]
	@TimeSheetOtId			VARCHAR(50),
	@DayDate				DATETIME,
	@EmployeeId				BIGINT,
	@Hours					DECIMAL(18,2),
	@CoefficientPoint		DECIMAL(18,2),
	@DayPoints				DECIMAL(18,2),
	@CreateDate				DATETIME,
	@Description			NVARCHAR(MAX)
AS

BEGIN
	INSERT INTO [hrm].[TimeSheetOt]
	(
		[TimeSheetOtId],
		[DayDate],
		[EmployeeId],
		[Hours],
		[CoefficientPoint],
		[DayPoints],
		[CreateDate],
		[Description]
	)
	VALUES
	(
		@TimeSheetOtId,
		@DayDate,
		@EmployeeId,
		@Hours,
		@CoefficientPoint,
		@DayPoints,
		@CreateDate,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_TimeSheets_ByDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [hrm].[Insert_TimeSheets_ByDate]
	@TimeSheetDate			DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [hrm].[TimeSheet]
	(
		[TimeSheetId],
		[EmployeeId],
		[TimeSheetDate],
		[Checkin],
		[Checkout],
		[ShiftWorkId]
	)
	SELECT
		[TimeSheetId]		= NEWID(),
		[EmployeeId]		= e.[EmployeeId],
		[TimeSheetDate]		= @TimeSheetDate,
		[Checkin]			= NULL,
		[Checkout]			= NULL,
		[ShiftWorkId]		= NULL
	FROM [hrm].[Employee] e
END
GO
/****** Object:  StoredProcedure [hrm].[Insert_TrainingLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Insert_TrainingLevel]
	@TrainingLevelId	INT OUTPUT,
	@LevelCode	VARCHAR(50),
	@LevelName	NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [hrm].[TrainingLevel]
	(
		[LevelCode],
		[LevelName],
		[Description]
	)
	VALUES
	(
		@LevelCode,
		@LevelName,
		@Description
	)
	SET @TrainingLevelId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Applicant]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Applicant]
	@ApplicantId		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Sex				TINYINT,
	@DateOfBirth		DATE,
	@CountryId			INT,
	@NationId			INT,
	@ReligionId			INT,
	@CityBirthPlace		INT,
	@PermanentAddress	NVARCHAR(255),
	@IdentityCardNumber	VARCHAR(50),
	@PhoneNumber		VARCHAR(50),
	@Email				NVARCHAR(255),
	@ChanelId			INT,
	@TrainingLevelId	INT,
	@RecruitPlanId		BIGINT,
	@CvDate				DATE,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Applicant]
	SET
		[FullName]				= @FullName,
		[Sex]					= @Sex,
		[DateOfBirth]			= @DateOfBirth,
		[CountryId]				= @CountryId,
		[NationId]				= @NationId,
		[ReligionId]			= @ReligionId,
		[CityBirthPlace]		= @CityBirthPlace,
		[PermanentAddress]		= @PermanentAddress,
		[IdentityCardNumber]	= @IdentityCardNumber,
		[PhoneNumber]			= @PhoneNumber,
		[Email]					= @Email,
		[ChanelId]				= @ChanelId,
		[TrainingLevelId]		= @TrainingLevelId,
		[RecruitPlanId]			= @RecruitPlanId,
		[CvDate]				= @CvDate,
		[Description]			= @Description
	WHERE [ApplicantId] = @ApplicantId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Career]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Career]
	@CareerId			INT,
	@CareerName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Career]
	SET
		[CareerName]		= @CareerName,
		[Description]		= @Description
	WHERE CareerId			= @CareerId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Contract]
	@ContractId			VARCHAR(50),
	@ContractCode		VARCHAR(50),
	@EmployeeId			BIGINT,
	@StartDate			DATE,
	@EndDate			DATE,
	@ContractTypeId		INT,
	@ContractFile		NVARCHAR(255),
	@ContractOthorFile	NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	UPDATE [hrm].[Contract]
	SET
		[ContractCode]			= @ContractCode,
		[EmployeeId]			= @EmployeeId,
		[StartDate]				= @StartDate,
		[EndDate]				= @EndDate,
		[ContractTypeId]		= @ContractTypeId,
		[ContractFile]			= @ContractFile,
		[ContractOthorFile]		= @ContractOthorFile,
		[Description]			= @Description
	WHERE [ContractId] = @ContractId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_ContractType]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_ContractType]
	@ContractTypeId		INT,
	@TypeName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@IsActive			BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[ContractType]
	SET
		[TypeName]			= @TypeName,
		[Description]		= @Description,
		[IsActive]			= @IsActive
	WHERE ContractTypeId	= @ContractTypeId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Department]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Department]
	@DepartmentId		BIGINT,
	@DepartmentCode		VARCHAR(50),
	@DepartmentName		NVARCHAR(255),
	@Description		NVARCHAR(500),
	@ParentId			BIGINT,
	@Path				VARCHAR(255),
	@IsActive			BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Department]
	SET
		[DepartmentCode]	= @DepartmentCode,
		[DepartmentName]	= @DepartmentName,
		[Description]		= @Description,
		[ParentId]			= @ParentId,
		[Path]				= @Path,
		[IsActive]			= @IsActive
	WHERE DepartmentId		= @DepartmentId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_EducationLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_EducationLevel]
	@EducationLevelId	INT,
	@LevelCode			VARCHAR(50),
	@LevelName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[EducationLevel]
	SET
		[LevelCode]		= @LevelCode,
		[LevelName]		= @LevelName,
		[Description]	= @Description
	WHERE EducationLevelId	= @EducationLevelId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Employee]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Employee]
	@EmployeeId					BIGINT,
	@FullName					NVARCHAR(255),
	@DateOfBirth				DATE,
	@Gender						TINYINT,
	@SpecialName				NVARCHAR(255),
	@Avatar						NVARCHAR(255),
	@DepartmentId				BIGINT,
	@CountryId					INT,
	@NationId					INT,
	@ReligionId					INT,
	@MaritalStatus				TINYINT,
	@CityBirthPlace				INT,
	@CityNativeLand				INT,
	@IdentityCardNumber			VARCHAR(50),
	@IdentityCardDate			DATE,
	@CityIdentityCard			INT,
	@PermanentAddress			NVARCHAR(255),
	@PermanentCity				INT,
	@PermanentDistrict			INT,
	@TemperaryAddress			NVARCHAR(255),
	@TemperaryCity				INT,
	@TemperaryDistrict			INT,
	@Email						NVARCHAR(255),
	@PhoneNumber				VARCHAR(50),
	@PositionId					INT,
	@TrainingLevelId			INT,
	@HealthStatus				NVARCHAR(500),
	@DateOfYouthUnionAdmission	DATE,
	@PlaceOfYouthUnionAdmission	NVARCHAR(500),
	@DateOfPartyAdmission		DATE,
	@PlaceOfPartyAdmission		NVARCHAR(500),
	@Skill						NVARCHAR(1000),
	@Experience					NVARCHAR(1000),
	@Description				NVARCHAR(1000),
	@IsActive					BIT,
	@Status						TINYINT,
	@ShiftWorkId				INT,
	@WorkedDate					DATE,
	@EducationLevelId			INT,
	@SchoolId					INT,
	@CareerId					INT,
	@TimeSheetCode				VARCHAR(50),
	@DepartmentCompany			INT,
	@CategoryKpiId				INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Employee]
	SET
		[FullName]							= @FullName,
		[DateOfBirth]						= @DateOfBirth,
		[Gender]							= @Gender,
		[SpecialName]						= @SpecialName,
		[Avatar]							= @Avatar,
		[DepartmentId]						= @DepartmentId,
		[CountryId]							= @CountryId,
		[NationId]							= @NationId,
		[ReligionId]						= @ReligionId,
		[MaritalStatus]						= @MaritalStatus,
		[CityBirthPlace]					= @CityBirthPlace,
		[CityNativeLand]					= @CityNativeLand,
		[IdentityCardNumber]				= @IdentityCardNumber,
		[IdentityCardDate]					= @IdentityCardDate,
		[CityIdentityCard]					= @CityIdentityCard,
		[PermanentAddress]					= @PermanentAddress,
		[PermanentCity]						= @PermanentCity,
		[PermanentDistrict]					= @PermanentDistrict,
		[TemperaryAddress]					= @TemperaryAddress,
		[TemperaryCity]						= @TemperaryCity,
		[TemperaryDistrict]					= @TemperaryDistrict,
		[Email]								= @Email,
		[PhoneNumber]						= @PhoneNumber,
		[PositionId]						= @PositionId,
		[TrainingLevelId]					= @TrainingLevelId,
		[HealthStatus]						= @HealthStatus,
		[DateOfYouthUnionAdmission]			= @DateOfYouthUnionAdmission,
		[PlaceOfYouthUnionAdmission]		= @PlaceOfYouthUnionAdmission,
		[DateOfPartyAdmission]				= @DateOfPartyAdmission,
		[PlaceOfPartyAdmission]				= @PlaceOfPartyAdmission,
		[Skill]								= @Skill,
		[Experience]						= @Experience,
		[Description]						= @Description,
		[IsActive]							= @IsActive,
		[Status]							= @Status,
		[ShiftWorkId]						= @ShiftWorkId,
		[WorkedDate]						= @WorkedDate,
		[EducationLevelId]					= @EducationLevelId,
		[SchoolId]							= @SchoolId,
		[CareerId]							= @CareerId,
		[TimeSheetCode]						= @TimeSheetCode,
		[DepartmentCompany]					= @DepartmentCompany,
		[CategoryKpiId]						= @CategoryKpiId

	WHERE EmployeeId = @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_EmployeeHoliday]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_EmployeeHoliday]
	@EmployeeHolidayId		VARCHAR(50),
	@HolidayReasonId		INT,
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Description			NVARCHAR(MAX),
	@EmployeeId				BIGINT,
	@CreateDate				DATETIME
AS

BEGIN
	UPDATE [hrm].[EmployeeHoliday]
	SET
		[HolidayReasonId]	= @HolidayReasonId,
		[FromDate]			= @FromDate,
		[ToDate]			= @ToDate,
		[Description]		= @Description,
		[EmployeeId]		= @EmployeeId,
		[CreateDate]		= @CreateDate
	WHERE [EmployeeHolidayId]	= @EmployeeHolidayId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_HolidayConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_HolidayConfig]
	@EmployeeId			BIGINT,
	@Year				INT,
	@HolidayNumber		INT
AS

BEGIN
	UPDATE [hrm].[HolidayConfig]
	SET
		[Year]			= @Year,
		[HolidayNumber]	= @HolidayNumber
	WHERE [EmployeeId]	= @EmployeeId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_HolidayDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_HolidayDetail]
	@HolidayDetailId		VARCHAR(50),
	@DateDay				DATETIME,
	@NumberDays				DECIMAL(18,1),
	@Permission				DECIMAL(18,1),
	@PercentSalary			DECIMAL(18,2),
	@ToTalDays				DECIMAL(18,2),
	@EmployeeHolidayId		VARCHAR(50)
AS

BEGIN
	UPDATE [hrm].[HolidayDetail]
	SET
		[DateDay]			= @DateDay,
		[NumberDays]		= @NumberDays,
		[Permission]		= @Permission,
		[PercentSalary]		= @PercentSalary,
		[ToTalDays]			= @ToTalDays
	WHERE [HolidayDetailId]	= @HolidayDetailId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_HolidayReason]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_HolidayReason]
	@HolidayReasonId		INT,
	@ReasonCode				VARCHAR(50),
	@ReasonName				NVARCHAR(255),
	@Description			NVARCHAR(500),
	@IsActive				BIT,
	@PercentSalary			DECIMAL(18,2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[HolidayReason]
	SET
		[ReasonCode]		= @ReasonCode,
		[ReasonName]		= @ReasonName,
		[Description]		= @Description,
		[IsActive]			= @IsActive,
		[PercentSalary]		= @PercentSalary
	WHERE [HolidayReasonId] = @HolidayReasonId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_IncurredSalary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_IncurredSalary]
	@IncurredSalaryId		VARCHAR(50),
	@EmployeeId				BIGINT,
	@Amount					MONEY,
	@Title					NVARCHAR(255),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@SubmitDate				DATE,
	@Description			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN
		UPDATE [hrm].[IncurredSalary]
		SET
			[EmployeeId]	= @EmployeeId,
			[Amount]		= @Amount,
			[Title]			= @Title,
			[SubmitDate]	= @SubmitDate,
			[Description]	= @Description
		WHERE [IncurredSalaryId] = @IncurredSalaryId
	END
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Insurance]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_Insurance]
	@InsuranceId		BIGINT,
	@EmployeeId			BIGINT,
	@InsuranceNumber	VARCHAR(50),
	@SubscriptionDate	DATE,
	@CityId				INT,
	@MonthBefore		INT,
	@Description		NVARCHAR(500),
	@IsActive			BIT,
	@CreateDate			DATETIME,
	@CreateBy			INT
AS
BEGIN
	UPDATE [hrm].[Insurance]
	SET
		[EmployeeId]		= @EmployeeId,
		[InsuranceNumber]	= @InsuranceNumber,
		[SubscriptionDate]	= @SubscriptionDate,
		[CityId]			= @CityId,
		[MonthBefore]		= @MonthBefore,
		[Description]		= @Description,
		[IsActive]			= @IsActive
	WHERE [InsuranceId]		= @InsuranceId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_InsuranceMedical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_InsuranceMedical]
	@InsuranceMedicalId			VARCHAR(50),
	@EmployeeId					BIGINT,
	@InsuranceMedicalNumber		VARCHAR(50),
	@StartDate					DATE,
	@ExpiredDate				DATE,
	@CityId						INT,
	@MedicalId					INT,
	@Amount						MONEY,
	@Description				NVARCHAR(500),
	@CreateDate					DATETIME,
	@CreateBy					INT
AS
BEGIN
	UPDATE [hrm].[InsuranceMedical]
	SET
		[EmployeeId]				= @EmployeeId,
		[InsuranceMedicalNumber]	= @InsuranceMedicalNumber,
		[StartDate]					= @StartDate,
		[ExpiredDate]				= @ExpiredDate,
		[CityId]					= @CityId,
		[MedicalId]					= @MedicalId,
		[Amount]					= @Amount,
		[Description]				= @Description
	WHERE [InsuranceMedicalId]		= @InsuranceMedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_InsuranceProcess]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_InsuranceProcess]
	@InsuranceProcessId		VARCHAR(50),
	@InsuranceId			BIGINT,
	@FromDate				DATE,
	@ToDate					DATE,
	@Amount					MONEY,
	@Description			NVARCHAR(500),
	@CreateDate				DATETIME,
	@CreateBy				INT
AS
BEGIN
	UPDATE [hrm].[InsuranceProcess]
	SET
		[InsuranceId]			= @InsuranceId,
		[FromDate]				= @FromDate,
		[ToDate]				= @ToDate,
		[Amount]				= @Amount,
		[Description]			= @Description,
		[CreateDate]			= @CreateDate,
		[CreateBy]				= @CreateBy
	WHERE [InsuranceProcessId]	= @InsuranceProcessId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_JobChange]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_JobChange]
	@JobChangeId		VARCHAR(50),
	@EmployeeId			BIGINT,
	@FromDepartmentId	BIGINT,
	@ToDepartmentId		BIGINT,
	@FromPositionId		INT,
	@ToPositionId		INT,
	@Reason				NVARCHAR(500),
	@Description		NVARCHAR(50),
	@JobChangeFile		NVARCHAR(255),
	@JobChangeNumber	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[JobChange]
	SET
		[EmployeeId]			= @EmployeeId,
		[FromDepartmentId]		= @FromDepartmentId,	
		[ToDepartmentId]		= @ToDepartmentId,
		[FromPositionId]		= @FromPositionId,
		[ToPositionId]			= @ToPositionId,
		[Reason]				= @Reason,
		[Description]			= @Description,
		[JobChangeFile]			= @JobChangeFile,
		[JobChangeNumber]		= @JobChangeNumber
	WHERE [JobChangeId]			= @JobChangeId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Maternity]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [hrm].[Update_Maternity]
	@MaternityId		VARCHAR(50),
	@EmployeeId			BIGINT,
	@FromDate			DATE,
	@ToDate				DATE,
	@StartTime			VARCHAR(50),
	@EndTime			VARCHAR(50),
	@RelaxStartTime		VARCHAR(50),
	@RelaxEndTime		VARCHAR(50),
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@Description		NVARCHAR(500)
AS
BEGIN
	UPDATE [hrm].[Maternity]
	SET
		[EmployeeId]		= @EmployeeId,
		[FromDate]			= @FromDate,
		[ToDate]			= @ToDate,
		[StartTime]			= @StartTime,
		[EndTime]			= @EndTime,
		[RelaxStartTime]	= @RelaxStartTime,
		[RelaxEndTime]		= @RelaxEndTime,
		[Description]		= @Description
	WHERE [MaternityId]		= @MaternityId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Medical]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Medical]
	@MedicalId		INT,
	@MedicalName	NVARCHAR(255),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Medical]
	SET
		[MedicalName]	= @MedicalName,
		[Description]	= @Description
	WHERE [MedicalId]	= @MedicalId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Nation]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Nation]
	@NationId			INT,
	@NationName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Nation]
	SET
		[NationName]		= @NationName,
		[Description]		= @Description
	WHERE NationId			= @NationId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Position]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Position]
	@PositionId		INT,
	@PositionCode	VARCHAR(50),
	@PositionName	NVARCHAR(255),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Position]
	SET
		[PositionCode]	= @PositionCode,
		[PositionName]	= @PositionName,
		[Description]	= @Description
	WHERE PositionId	= @PositionId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_PraiseDiscipline]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [hrm].[Update_PraiseDiscipline]
	@PraiseDisciplineId		VARCHAR(50),
	@Title					NVARCHAR(255),
	@DecisionNumber			VARCHAR(50),
	@PraiseDisciplineDate	DATE,
	@Formality				NVARCHAR(255),
	@Reason					NVARCHAR(500),
	@Description			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[PraiseDiscipline]
	SET
		[Title]					= @Title,
		[DecisionNumber]		= @DecisionNumber,
		[PraiseDisciplineDate]	= @PraiseDisciplineDate,
		[Formality]				= @Formality,
		[Reason]				= @Reason,
		[Description]			= @Description
	WHERE PraiseDisciplineId	= @PraiseDisciplineId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_RecruitChanel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_RecruitChanel]
	@RecruitChanelId	INT,
	@ChanelName			NVARCHAR(255),
	@Description		NVARCHAR(500),
	@IsActive			BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[RecruitChanel]
	SET
		[ChanelName]	= @ChanelName,
		[Description]	= @Description,
		[IsActive]		= @IsActive
	WHERE [RecruitChanelId] = @RecruitChanelId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_RecruitPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_RecruitPlan]
	@RecruitPlanId			BIGINT,
	@RecruitPlanCode		VARCHAR(50),
	@Title					NVARCHAR(255),
	@DepartmentId			BIGINT,
	@PositionId				INT,
	@Quantity				INT,
	@FromDate				DATE,
	@ToDate					DATE,
	@Requirements			NVARCHAR(MAX),
	@ChanelIds				VARCHAR(50),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@IsActive				BIT,
	@Description			NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[RecruitPlan]
	SET
		[Title]			= @Title,
		[DepartmentId]	= @DepartmentId,
		[PositionId]	= @PositionId,
		[Quantity]		= @Quantity,
		[FromDate]		= @FromDate,
		[ToDate]		= @ToDate,
		[Requirements]	= @Requirements,
		[ChanelIds]		= @ChanelIds,
		[IsActive]		= @IsActive,
		[Description]	= @Description
	WHERE [RecruitPlanId] = @RecruitPlanId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_RecruitResult]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_RecruitResult]
	@RecruitResultId		VARCHAR(50),
	@ApplicantId			VARCHAR(50),
	@RecruitPlanId			BIGINT,
	@Result					TINYINT,
	@Description			NVARCHAR(1000),
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@EmployeeId				BIGINT,
	@XML					NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/RecruitResultDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [hrm].[RecruitResultDetail] WHERE [RecruitResultId] = @RecruitResultId

	UPDATE [hrm].[RecruitResult]
	SET
		[ApplicantId]		= @ApplicantId,
		[RecruitPlanId]		= @RecruitPlanId,
		[Result]			= @Result,
		[Description]		= @Description,
		[EmployeeId]		= @EmployeeId
	WHERE [RecruitResultId]	= @RecruitResultId

	INSERT INTO [hrm].[RecruitResultDetail]
	(
		[RecruitResultDetailId],
		[RecruitResultId],
		[EmployeeId],
		[Result],
		[Description],
		[InterviewDate]
	)
	SELECT
		[RecruitResultDetailId]	= NEWID(),
		[RecruitResultId]		= @RecruitResultId,
		[EmployeeId]			= x.[EmployeeId],
		[Result]				= x.[Result],
		[Description]			= x.[Description],
		[InterviewDate]			= x.[InterviewDate]
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH (
			[EmployeeId]			BIGINT,
			[Result]				TINYINT,
			[Description]			NVARCHAR(1000),
			[InterviewDate]			DATETIME
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Religion]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Religion]
	@ReligionId			INT,
	@ReligionName		NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Religion]
	SET
		[ReligionName]	= @ReligionName,
		[Description]	= @Description
	WHERE ReligionId	= @ReligionId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_Salary]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_Salary]
	@SalaryId					VARCHAR(50),
	@EmployeeId					BIGINT,
	@BasicSalary				MONEY,
	@BasicCoefficient			DECIMAL(18,2),
	@ProfessionalCoefficient	DECIMAL(18,2),
	@ResponsibilityCoefficient	DECIMAL(18,2),
	@PercentProfessional		DECIMAL(18,1),
	@ApplyDate					DATE,
	@CreateDate					DATETIME,
	@CreateBy					INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[Salary]
	SET
		[EmployeeId]				= @EmployeeId,
		[BasicSalary]				= @BasicSalary,
		[BasicCoefficient]			= @BasicCoefficient,
		[ProfessionalCoefficient]	= @ProfessionalCoefficient,
		[ResponsibilityCoefficient]	= @ResponsibilityCoefficient,
		[PercentProfessional]		= @PercentProfessional,
		[ApplyDate]					= @ApplyDate
	WHERE [SalaryId]				= @SalaryId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_School]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_School]
	@SchoolId			INT,
	@SchoolName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[School]
	SET
		[SchoolName]		= @SchoolName,
		[Description]		= @Description
	WHERE SchoolId			= @SchoolId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_ShiftWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_ShiftWork]
	@ShiftWorkId	INT,
	@ShiftWorkCode	VARCHAR(50),
	@StartTime		VARCHAR(50),
	@EndTime		VARCHAR(50),
	@RelaxStartTime	VARCHAR(50),
	@RelaxEndTime	VARCHAR(50),
	@Description	NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[ShiftWork]
	SET
		[ShiftWorkCode]		= @ShiftWorkCode,
		[StartTime]			= @StartTime,
		[EndTime]			= @EndTime,
		[RelaxStartTime]	= @RelaxStartTime,
		[RelaxEndTime]		= @RelaxEndTime,
		[Description]		= @Description
	WHERE ShiftWorkId		= @ShiftWorkId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_TimeSheet]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_TimeSheet]
	@TimeSheetId			VARCHAR(50),
	@EmployeeId				BIGINT,
	@TimeSheetDate			DATE,
	@Checkin				DATETIME,
	@Checkout				DATETIME,
	@ShiftWorkId			INT
AS

BEGIN
	IF EXISTS(SELECT TimeSheetId FROM [hrm].[TimeSheet] WHERE [EmployeeId] = @EmployeeId AND CAST(TimeSheetDate AS DATE) = CAST(@TimeSheetDate AS DATE))
		BEGIN
			UPDATE [hrm].[TimeSheet]
			SET
				[EmployeeId]	= @EmployeeId,
				[Checkin]		= @Checkin,
				[Checkout]		= @Checkout,
				[ShiftWorkId]	= @ShiftWorkId
			WHERE 
					[EmployeeId] = @EmployeeId 
				AND 
					CAST(TimeSheetDate AS DATE) = CAST(@TimeSheetDate AS DATE)
		END
	ELSE
		BEGIN
			INSERT INTO [hrm].[TimeSheet]
			(
				[TimeSheetId],
				[TimeSheetDate],
				[EmployeeId],
				[Checkin],
				[Checkout],
				[ShiftWorkId]
			)
			VALUES
			(
				@TimeSheetId,
				@TimeSheetDate,
				@EmployeeId,
				@Checkin,
				@Checkout,
				@ShiftWorkId
			)
		END
END
GO
/****** Object:  StoredProcedure [hrm].[Update_TimeSheetOt]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [hrm].[Update_TimeSheetOt]
	@TimeSheetOtId			VARCHAR(50),
	@DayDate				DATETIME,
	@EmployeeId				BIGINT,
	@Hours					DECIMAL(18,2),
	@CoefficientPoint		DECIMAL(18,2),
	@DayPoints				DECIMAL(18,2),
	@CreateDate				DATETIME,
	@Description			NVARCHAR(MAX)
AS

BEGIN
	UPDATE [hrm].[TimeSheetOt]
	SET
		[DayDate]			= @DayDate,
		[EmployeeId]		= @EmployeeId,
		[Hours]				= @Hours,
		[CoefficientPoint]	= @CoefficientPoint,
		[DayPoints]			= @DayPoints,
		[Description]		= @Description
	WHERE 
		[TimeSheetOtId]		= @TimeSheetOtId
END
GO
/****** Object:  StoredProcedure [hrm].[Update_TrainingLevel]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [hrm].[Update_TrainingLevel]
	@TrainingLevelId	INT,
	@LevelCode			VARCHAR(50),
	@LevelName			NVARCHAR(255),
	@Description		NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [hrm].[TrainingLevel]
	SET
		[LevelCode]		= @LevelCode,
		[LevelName]		= @LevelName,
		[Description]	= @Description
	WHERE TrainingLevelId	= @TrainingLevelId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_AssignWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_AssignWork]
	@AssignWorkId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[AssignWork]	WHERE [AssignWorkId]	= @AssignWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_Complain]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_Complain]
	@ComplainId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[Complain]	WHERE [ComplainId]	= @ComplainId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_Performer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_Performer]
	@PerformerBy		INT,
	@WorkStreamId		VARCHAR(50)
AS
BEGIN
	DELETE [kpi].[Performer]	WHERE [PerformerBy]	= @PerformerBy AND [WorkStreamId] = @WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_SuggesWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_SuggesWork]
	@SuggesWorkId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[SuggesWork]	WHERE [SuggesWorkId]	= @SuggesWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_Task]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Delete_Task]
	@TaskId	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE [kpi].[Task]
	WHERE [TaskId] = @TaskId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkPlan]
	@WorkPlanId		VARCHAR(50)
AS

BEGIN	
	DELETE [kpi].[WorkPlanDetail]	WHERE [WorkPlanId]	= @WorkPlanId
	DELETE [kpi].[WorkPlan]			WHERE [WorkPlanId]	= @WorkPlanId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkPlanDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkPlanDetail]
	@WorkPlanDetailId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[WorkPlanDetail]	WHERE [WorkPlanDetailId]	= @WorkPlanDetailId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkPlanDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkPlanDetails]
	@WorkPlanId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[WorkPlanDetail]	WHERE [WorkPlanId]	= @WorkPlanId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkStream]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkStream]
	@WorkStreamId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[WorkStream]	WHERE [WorkStreamId]	= @WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkStreamDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkStreamDetail]
	@WorkStreamDetailId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[WorkStreamDetail]	WHERE [WorkStreamDetailId]	= @WorkStreamDetailId
END
GO
/****** Object:  StoredProcedure [kpi].[Delete_WorkStreamDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Delete_WorkStreamDetails]
	@WorkStreamId		VARCHAR(50)
AS

BEGIN
	DELETE [kpi].[WorkStreamDetail]	WHERE [WorkStreamId]	= @WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_AcceptConfigCheck]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_AcceptConfigCheck]
	@AcceptConfigId		INT,
	@ConditionMax		DECIMAL(18,2),
	@ConditionMin		DECIMAL(18,2)
AS
BEGIN
	SELECT
		[AcceptConfigId],
		[AcceptType],
		[AcceptPointMin],
		[AcceptPointMax],
		[AcceptConditionMin],
		[AcceptConditionMax]
	FROM [kpi].[AcceptConfig]
	WHERE 
		(	
		
			ISNULL([AcceptConditionMin],@ConditionMax) < @ConditionMax AND @ConditionMax <ISNULL([AcceptConditionMax],@ConditionMax)
		OR
			ISNULL([AcceptConditionMin],@ConditionMin) < @ConditionMin AND @ConditionMin <ISNULL([AcceptConditionMax],@ConditionMin)
			)
		AND
			[AcceptConfigId] != @AcceptConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_AcceptConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_AcceptConfigs]
AS
BEGIN
	SELECT
		[AcceptConfigId],
		[AcceptType],
		[AcceptPointMin],
		[AcceptPointMax],
		[AcceptConditionMin],
		[AcceptConditionMax]
	FROM [kpi].[AcceptConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_AssignWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_AssignWork]
	@AssignWorkId   VARCHAR(50)
AS
BEGIN
	SELECT
		[AssignWorkId]			= CAST(aw.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(aw.[TaskId] AS VARCHAR(50)),
		[CreateBy]				= aw.[CreateBy],
		[AssignBy]				= aw.[AssignBy],
		[CreateDate]			= aw.[CreateDate],
		[FromDate]				= aw.[FromDate],
		[Explanation]			= aw.[Explanation],
		[WorkingNote]			= aw.[WorkingNote],
		[Status]				= aw.[Status],
		[ToDate]				= aw.[ToDate],
		[Description]			= aw.[Description],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[AssignName]			= u.[UserName],
		[AssignFullName]		= u.[FullName],
		[UsefulHours]			= aw.[UsefulHours],
		[ApprovedFisnishBy]		= aw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= aw.[ApprovedFisnishDate],
		[FisnishDate]			= aw.[FisnishDate],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= aw.[WorkPointType],
		[Quantity]				= aw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[WorkPoint]				= aw.[WorkPoint],
		[FileConfirm],
		[DepartmentFollowBy],
		[DirectorFollowBy],
		[TypeAssignWork]
	FROM [kpi].[AssignWork] aw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = aw.[TaskId]
	INNER JOIN [dbo].[User] u ON u.[UserId] = aw.[AssignBy]
	WHERE aw.[AssignWorkId] = @AssignWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_AssignWorkByFollows]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_AssignWorkByFollows]
	@EmployeeId		INT
AS
BEGIN
	SELECT
		[AssignWorkId]			= CAST(aw.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(aw.[TaskId] AS VARCHAR(50)),
		[CreateBy]				= aw.[CreateBy],
		[AssignBy]				= aw.[AssignBy],
		[CreateDate]			= aw.[CreateDate],
		[FromDate]				= aw.[FromDate],
		[Status]				= aw.[Status],
		[ToDate]				= aw.[ToDate],
		[Explanation]			= aw.[Explanation],
		[WorkingNote]			= aw.[WorkingNote],
		[Description]			= aw.[Description],
		[UsefulHours]			= aw.[UsefulHours],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[AssignName]			= ued.[FullName],
		[AssignCode]			= ued.[EmployeeCode],
		[CreateByName]			= e.[FullName],
		[ApprovedFisnishBy]		= aw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= aw.[ApprovedFisnishDate],
		[FisnishDate]			= aw.[FisnishDate],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= aw.[WorkPointType],
		[Quantity]				= aw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[FileConfirm],
		[DepartmentFollowBy],
		[DirectorFollowBy],
		[TypeAssignWork]
	FROM [kpi].[AssignWork] aw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = aw.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = aw.[AssignBy]
	INNER JOIN [dbo].[User] e ON e.[UserId] = aw.[CreateBy]
	WHERE	aw.[DepartmentFollowBy]	= @EmployeeId
	 OR	
	 		aw.[DirectorFollowBy]	= @EmployeeId

END
GO
/****** Object:  StoredProcedure [kpi].[Get_AssignWorks]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_AssignWorks]
	@Status			TINYINT,
	@CreateBy		BIGINT,
	@AssignBy		BIGINT,
	@FromDate		DATETIME,
	@ToDate			DATETIME		
AS
BEGIN
	SELECT
		[AssignWorkId]			= CAST(aw.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(aw.[TaskId] AS VARCHAR(50)),
		[CreateBy]				= aw.[CreateBy],
		[AssignBy]				= aw.[AssignBy],
		[CreateDate]			= aw.[CreateDate],
		[FromDate]				= aw.[FromDate],
		[Status]				= aw.[Status],
		[ToDate]				= aw.[ToDate],
		[Explanation]			= aw.[Explanation],
		[WorkingNote]			= aw.[WorkingNote],
		[Description]			= aw.[Description],
		[UsefulHours]			= aw.[UsefulHours],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[AssignName]			= ued.[FullName],
		[AssignCode]			= ued.[EmployeeCode],
		[CreateByName]			= e.[FullName],
		[ApprovedFisnishBy]		= aw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= aw.[ApprovedFisnishDate],
		[FisnishDate]			= aw.[FisnishDate],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= aw.[WorkPointType],
		[Quantity]				= aw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[FileConfirm],
		[DepartmentFollowBy],
		[DirectorFollowBy],
		[TypeAssignWork]
	FROM [kpi].[AssignWork] aw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = aw.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = aw.[AssignBy]
	INNER JOIN [dbo].[User] e ON e.[UserId] = aw.[CreateBy]
	WHERE	aw.[Status]	= ISNULL(@Status,aw.[Status])
	AND		aw.[CreateBy] = ISNULL(@CreateBy,aw.[CreateBy])
	AND		aw.[AssignBy] = ISNULL(@AssignBy,aw.[AssignBy])
	AND		CAST(aw.[ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(aw.[ToDate] AS DATE)) AND ISNULL(@ToDate,CAST(aw.[ToDate] AS DATE))
	AND		CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE))
END
GO
/****** Object:  StoredProcedure [kpi].[Get_AssignWorks_ByUserId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_AssignWorks_ByUserId]
	@AssignBy		BIGINT,
	@CreateBy		BIGINT	
AS
BEGIN
	SELECT
		[AssignWorkId]			= CAST(aw.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(aw.[TaskId] AS VARCHAR(50)),
		[CreateBy]				= aw.[CreateBy],
		[AssignBy]				= aw.[AssignBy],
		[CreateDate]			= aw.[CreateDate],
		[FromDate]				= aw.[FromDate],
		[Status]				= aw.[Status],
		[ToDate]				= aw.[ToDate],
		[Explanation]			= aw.[Explanation],
		[WorkingNote]			= aw.[WorkingNote],
		[Description]			= aw.[Description],
		[UsefulHours]			= aw.[UsefulHours],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[AssignName]			= ued.[FullName],
		[AssignCode]			= ued.[EmployeeCode],
		[CreateByName]			= e.[FullName],
		[ApprovedFisnishBy]		= aw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= aw.[ApprovedFisnishDate],
		[FisnishDate]			= aw.[FisnishDate],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= aw.[WorkPointType],
		[Quantity]				= aw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[FileConfirm],
		[DepartmentFollowBy],
		[DirectorFollowBy],
		[TypeAssignWork]
	FROM [kpi].[AssignWork] aw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = aw.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = aw.[AssignBy]
	INNER JOIN [dbo].[User] e ON e.[UserId] = aw.[CreateBy]
	WHERE  [Status] != 4 AND [Status] != 3 AND [Status] != 5 AND aw.[AssignBy] = ISNULL(@AssignBy, aw.[AssignBy]) AND aw.[CreateBy] = ISNULL(@CreateBy, aw.[CreateBy])
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Complain]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_Complain]
		@ComplainId			VARCHAR(50)
AS
BEGIN
	SELECT
		[ComplainId]		= CAST(c.[ComplainId] AS VARCHAR(50)),
		[CreateBy]			= c.[CreateBy],
		[AccusedBy]			= c.[AccusedBy],
		[Description]		= c.[Description],
		[CreateDate]		= c.[CreateDate],
		[ConfirmedBy]		= c.[ConfirmedBy],
		[ConfirmedDate]		= c.[ConfirmedDate],
		[Status]			= c.[Status],
		[CreateByName]		= s.[UserName],
		[ConfirmedByName]	= e.[UserName],
		[AccusedByName]		= ued.[FullName],
		[AccusedByCode]		= ued.[EmployeeCode]
	FROM [kpi].[Complain] c
	INNER JOIN [dbo].[User] s ON s.[UserId] =  c.[CreateBy]
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = c.[AccusedBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] =  c.[ConfirmedBy]
	WHERE c.[ComplainId] = @ComplainId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_ComplainKpis]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_ComplainKpis]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@Path			VARCHAR(50)
AS
BEGIN
SELECT
	temp.*,
	Rating			= ISNULL((SELECT TOP 1 AcceptType FROM [kpi].[AcceptConfig] WHERE temp.TotalFinish BETWEEN [AcceptConditionMin] AND ISNULL([AcceptConditionMax],temp.TotalFinish)),''),
	RatingPoint		= ISNULL((SELECT TOP 1 AcceptPointMax FROM [kpi].[AcceptConfig] WHERE temp.TotalFinish BETWEEN [AcceptConditionMin] AND ISNULL([AcceptConditionMax],temp.TotalFinish)),0)
FROM 
(
	SELECT 
		e.EmployeeId,
		e.EmployeeCode,
		e.FullName,
		u.UserName,
		u.UserId,
		u.DepartmentName,
		TotalQuantity = ISNULL((SELECT COUNT(ComplainId) FROM [kpi].[Complain]  WHERE  CAST([CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate AND AccusedBy = ISNULL(u.UserId,0)),0),
		TotalFinish = ISNULL((SELECT COUNT(ComplainId) FROM [kpi].[Complain] WHERE  [Status] = 2 AND CAST([CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate AND AccusedBy = ISNULL(u.UserId,0)),0) 
	FROM [hrm].[Employee] e
	LEFT JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId]
	WHERE
		u.[Path] LIKE ''+ISNULL(@Path,u.[Path])+'%'
) AS temp
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Complains]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_Complains]
	@FromDate   DATETIME,
	@ToDate		DATETIME,
	@CreateBy	INT,
	@Action		TINYINT
AS
BEGIN
	SELECT
		[ComplainId]		= CAST(c.[ComplainId] AS VARCHAR(50)),
		[CreateBy]			= c.[CreateBy],
		[AccusedBy]			= c.[AccusedBy],
		[Description]		= c.[Description],
		[CreateDate]		= c.[CreateDate],
		[ConfirmedBy]		= c.[ConfirmedBy],
		[ConfirmedDate]		= c.[ConfirmedDate],
		[Status]			= c.[Status],
		[CreateByName]		= ued2.[FullName],
		[CreateByCode]		= ued2.[EmployeeCode],
		[ConfirmedByName]	= e.[UserName],
		[AccusedByName]		= ued.[FullName],
		[AccusedByCode]		= ued.[EmployeeCode],
		[DepartmentName]	= ued.[DepartmentName]
	FROM [kpi].[Complain] c
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = c.[AccusedBy]
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued2 ON ued2.[UserId] = c.[CreateBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] =  c.[ConfirmedBy]
	
	WHERE 
				c.[CreateBy] = ISNULL(@CreateBy,c.[CreateBy])
		AND
			(		( @Action = 0)
				OR
					(c.[Status] = 1  AND @Action = 1)			
				OR
					(c.[Status] = 2 AND @Action = 2 AND ( CAST(c.[CreateDate] AS DATE) BETWEEN ISNULL(@FromDate,c.[CreateDate]) AND ISNULL(@ToDate,c.[CreateDate])))
				OR
					(c.[Status] = 3 AND @Action = 3 AND (CAST(c.[CreateDate] AS DATE) BETWEEN ISNULL(@FromDate,c.[CreateDate]) AND ISNULL(@ToDate,c.[CreateDate])))
			)
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Complains_AccusedBy]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_Complains_AccusedBy]
	@FromDate   DATETIME,
	@ToDate		DATETIME,
	@AccusedBy	INT,
	@Action		INT
AS
BEGIN
	SELECT
		[ComplainId]		= CAST(c.[ComplainId] AS VARCHAR(50)),
		[CreateBy]			= c.[CreateBy],
		[AccusedBy]			= c.[AccusedBy],
		[Description]		= c.[Description],
		[CreateDate]		= c.[CreateDate],
		[ConfirmedBy]		= c.[ConfirmedBy],
		[ConfirmedDate]		= c.[ConfirmedDate],
		[Status]			= c.[Status],
		[CreateByName]		= ued2.[FullName],
		[CreateByCode]		= ued2.[EmployeeCode],
		[ConfirmedByName]	= e.[UserName],
		[AccusedByName]		= ued.[FullName],
		[AccusedByCode]		= ued.[EmployeeCode],
		[DepartmentName]	= ued.[DepartmentName]
	FROM [kpi].[Complain] c
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = c.[AccusedBy]
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued2 ON ued2.[UserId] = c.[CreateBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] =  c.[ConfirmedBy]
	
	WHERE 
				c.[AccusedBy] = ISNULL(@AccusedBy,c.[AccusedBy])
		AND
				CAST(c.[CreateDate] AS DATE) BETWEEN ISNULL(@FromDate,c.[CreateDate]) AND ISNULL(@ToDate,c.[CreateDate])
		AND
	(
				([Status] = 2 AND @Action = 2)
		OR
				( @Action = 1)
	)

END
GO
/****** Object:  StoredProcedure [kpi].[Get_EmployeeUsefulHours]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_EmployeeUsefulHours]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@DepartmentId	BIGINT
AS
BEGIN
SELECT 
	e.*,
	u.UserName,
	u.UserId,
	u.DepartmentName,
	TotalQuantity = ISNULL((SELECT COUNT(WorkDetailId) FROM [kpi].[WorkDetail] WHERE CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST([FromDate] AS DATE)) AND  CAST([ToDate] AS DATE) BETWEEN @FromDate AND @ToDate AND CreateBy = ISNULL(u.UserId,0)),0),
	ToTalUsefulHoursReal = ISNULL((SELECT SUM(Quantity*UsefulHours) FROM [kpi].[WorkDetail] WHERE   Status = 4 AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST([FromDate] AS DATE)) AND  CAST([ToDate] AS DATE) BETWEEN @FromDate AND @ToDate AND CreateBy= ISNULL(u.UserId,0)),0),
	ToTalUsefulHoursTask = ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE   Status = 4 AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST([FromDate] AS DATE)) AND CAST([ToDate] AS DATE) BETWEEN @FromDate AND @ToDate AND CreateBy= ISNULL(u.UserId,0)),0),
	TotalFinish = ISNULL((SELECT COUNT(WorkDetailId) FROM [kpi].[WorkDetail] WHERE CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST([FromDate] AS DATE)) AND Status = 4 AND  CAST([ToDate] AS DATE) BETWEEN @FromDate AND @ToDate AND CreateBy= ISNULL(u.UserId,0)),0) 
FROM [hrm].[Employee] e
LEFT JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId]
WHERE
	 e.DepartmentId = ISNULL(@DepartmentId,e.DepartmentId)
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorConfigCheck]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FactorConfigCheck]
	@FactorConfigId		INT,
	@ConditionMax		DECIMAL(18,2),
	@ConditionMin		DECIMAL(18,2)	
AS
BEGIN
	SELECT
		[FactorConfigId],
		[FactorType],
		[FactorPointMin],
		[FactorPointMax],
		[FactorConditionMin],
		[FactorConditionMax]
	FROM [kpi].[FactorConfig]
	WHERE 
			(	
		
			ISNULL([FactorConditionMin],@ConditionMax) < @ConditionMax AND @ConditionMax <ISNULL([FactorConditionMax],@ConditionMax)
		OR
			ISNULL([FactorConditionMin],@ConditionMin) < @ConditionMin AND @ConditionMin <ISNULL([FactorConditionMax],@ConditionMin)
			)
		AND
			[FactorConfigId] != @FactorConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FactorConfigs]
AS
BEGIN
	SELECT
		[FactorConfigId],
		[FactorType],
		[FactorPointMin],
		[FactorPointMax],
		[FactorConditionMin],
		[FactorConditionMax]
	FROM [kpi].[FactorConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorWorkKpi]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_FactorWorkKpi] 
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@TotalDay		DECIMAL(18,2), -- ngày làm việc thực tế theo quy định của cty
	@EmployeeId		BIGINT

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MinHourConfig DECIMAL(18,1)
	DECLARE @MinHour DECIMAL(18,1)
	SET @MinHourConfig = ISNULL((SELECT TOP 1 [MinHours] FROM [kpi].[KpiConfig]),0)
	SET @MinHour = @MinHourConfig*@TotalDay

	CREATE TABLE #ComplainKpi(
		EmployeeId		BIGINT,
		EmployeeCode	VARCHAR(50),
		FullName		NVARCHAR(255),
		UserName		VARCHAR(255),
		UserId			INT,
		DepartmentName	NVARCHAR(255),
		TotalQuantity	INT,
		TotalFinish		INT,
		Rating			VARCHAR(50),
		RatingPoint		DECIMAL(18,2)
	)
	INSERT INTO #ComplainKpi EXECUTE [kpi].[Get_ComplainKpis] @FromDate,@ToDate,NULL

	CREATE TABLE #FinishWorkKpi(
		EmployeeId				BIGINT,
		EmployeeCode			VARCHAR(50),
		FullName				NVARCHAR(255),
		UserName				VARCHAR(255),
		UserId					INT,
		DepartmentName			NVARCHAR(255),
		TotalUsefulHoursTask	INT,
		TotalUsefulHoursReal	INT,
		Rating					VARCHAR(50),
		RatingPoint				DECIMAL(18,2)
	)
	INSERT INTO #FinishWorkKpi EXECUTE [kpi].[Get_FinishWorkKpis] @FromDate,@ToDate,NULL

	CREATE TABLE #SuggesWorkKpi(
		EmployeeId				BIGINT,
		EmployeeCode			VARCHAR(50),
		FullName				NVARCHAR(255),
		UserName				VARCHAR(255),
		UserId					INT,
		DepartmentName			NVARCHAR(255),
		TotalQuantity			INT,
		TotalFinish				INT,
		WorkPoint				DECIMAL(18,2),
		Rating					VARCHAR(50),
		RatingPoint				DECIMAL(18,2)
	)
	INSERT INTO #SuggesWorkKpi EXECUTE [kpi].[Get_SuggesWorkKpis] @FromDate,@ToDate,NULL,NULL

	SELECT
		temp.*,
		[FactorType]	= CASE
							WHEN [AvgPoint] <= 0 THEN '' 
						  ELSE
						  (SELECT TOP 1 [FactorType] FROM [kpi].[FactorConfig] WHERE temp.[AvgPoint] >= [FactorConditionMin] AND temp.[AvgPoint]< [FactorConditionMax])
						   END,
		[FactorPoint]	= CASE
							WHEN [AvgPoint] <= 0 THEN 0
						  ELSE
							(SELECT TOP 1 [FactorPointMax] FROM [kpi].[FactorConfig] WHERE temp.[AvgPoint] >= [FactorConditionMin] AND temp.[AvgPoint]< [FactorConditionMax])
						  END
	FROM
	(
		SELECT
			[EmployeeId]		= e.[EmployeeId],
			[EmployeeCode]		= e.[EmployeeCode],
			[FullName]			= e.[FullName],
			[DepartmentId]		= e.[DepartmentId],
			[DepartmentName]	= d.[DepartmentName],
			[UsefullHourMin]	= @MinHour,
			[TotalUsefulHours]	= ISNULL((SELECT TOP 1 TotalUsefulHoursTask FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[SuggesPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #SuggesWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[ApprovedPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[ComplainPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #ComplainKpi WHERE EmployeeId = e.EmployeeId),0),
			[AvgPoint]			= CASE
										WHEN ISNULL((SELECT TOP 1 TotalUsefulHoursTask FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0) < @MinHour THEN 0
										ELSE (
												ISNULL((SELECT TOP 1 RatingPoint FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0) 
											+ 
												ISNULL((SELECT TOP 1 RatingPoint FROM #SuggesWorkKpi WHERE EmployeeId = e.EmployeeId),0) 
											+ 
												ISNULL((SELECT TOP 1 RatingPoint FROM #ComplainKpi WHERE EmployeeId = e.EmployeeId),0)
											)/3
									END
		FROM [hrm].[Employee] e
		LEFT JOIN [hrm].[Department] d ON e.[DepartmentId] = d.[DepartmentId]
		WHERE   e.[EmployeeId] = ISNULL(@EmployeeId,e.[EmployeeId])
	) AS temp


	DROP TABLE #ComplainKpi
	DROP TABLE #FinishWorkKpi
	DROP TABLE #SuggesWorkKpi
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorWorkKpiNew]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FactorWorkKpiNew]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@EmployeeId		BIGINT,
	@TotalDay		INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MinHourConfig DECIMAL(18,1)
	SET @MinHourConfig = ISNULL((SELECT TOP 1 [MinHours] FROM [kpi].[KpiConfig]),0)
	SELECT
		temp.*,
		[FactorType]	= CASE
							WHEN (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) <= 0 THEN '' 
						  ELSE
						  (SELECT TOP 1 [FactorType] FROM [kpi].[FactorConfig] WHERE (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) >= [FactorConditionMin] AND (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])< [FactorConditionMax])
						   END,
		[FactorPoint]	= CASE
							WHEN (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])<= 0 THEN 0
						  ELSE
							(SELECT TOP 1 [FactorPointMax] FROM [kpi].[FactorConfig] WHERE (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) >= [FactorConditionMin] AND (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])< [FactorConditionMax])
						  END
		
		
	FROM
		(SELECT
			[EmployeeId]		= e.[EmployeeId],
			[EmployeeCode]		= e.[EmployeeCode],
			[FullName]			= e.[FullName],
			[DepartmentCompany]	= e.[DepartmentCompany],
			[DepartmentId]		= e.[DepartmentId],
			[DepartmentName]	= ued.[DepartmentName],
			TotalComplain = ISNULL((SELECT COUNT(ComplainId) FROM [kpi].[Complain] WHERE  [Status] = 2 AND CAST([CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate AND AccusedBy = ISNULL(ued.UserId,0)),0),
			[UsefullHourMin]	= (@TotalDay-(SELECT COUNT(HolidayId) FROM hrm.Holiday WHERE CAST([HolidayDate] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE)) - ISNULL((SELECT SUM([NumberDays]) FROM [hrm].[HolidayDetail] hd 
											INNER JOIN [hrm].[EmployeeHoliday] eh ON eh.[EmployeeHolidayId] = hd.[EmployeeHolidayId]
											WHERE CAST([DateDay] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) 
											AND eh.[EmployeeId] = e.[EmployeeId]),0))*@MinHourConfig,
			[UsefulHoursTask]	= ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE  [Status] = 4 AND WorkType !=3   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(ued.UserId,0)),0),
			[UsefulHoursSuggesWork]	= ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE  [Status] = 4 AND WorkType =3    AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(ued.UserId,0)),0)
		FROM [hrm].[Employee] e
		LEFT JOIN [dbo].[UserEmployeeDepartment] ued on ued.[EmployeeId] = e.[EmployeeId]
		WHERE 
			e.[EmployeeId] = ISNULL(@EmployeeId,e.[EmployeeId])
			) AS temp
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorWorkKpis]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_FactorWorkKpis] 
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@TotalDay		INT, -- ngày làm việc thực tế theo quy định của cty
	@Path			VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MinHourConfig DECIMAL(18,1)
	SET @MinHourConfig = ISNULL((SELECT TOP 1 [MinHours] FROM [kpi].[KpiConfig]),0)

	CREATE TABLE #ComplainKpi(
		EmployeeId		BIGINT,
		EmployeeCode	VARCHAR(50),
		FullName		NVARCHAR(255),
		UserName		VARCHAR(255),
		UserId			INT,
		DepartmentName	NVARCHAR(255),
		TotalQuantity	INT,
		TotalFinish		INT,
		Rating			VARCHAR(50),
		RatingPoint		DECIMAL(18,2)
	)
	INSERT INTO #ComplainKpi EXECUTE [kpi].[Get_ComplainKpis] @FromDate,@ToDate,NULL

	CREATE TABLE #FinishWorkKpi(
		EmployeeId				BIGINT,
		EmployeeCode			VARCHAR(50),
		FullName				NVARCHAR(255),
		UserName				VARCHAR(255),
		UserId					INT,
		DepartmentName			NVARCHAR(255),
		TotalUsefulHoursTask	INT,
		TotalUsefulHoursReal	INT,
		Rating					VARCHAR(50),
		RatingPoint				DECIMAL(18,2)
	)
	INSERT INTO #FinishWorkKpi EXECUTE [kpi].[Get_FinishWorkKpis] @FromDate,@ToDate,NULL

	CREATE TABLE #SuggesWorkKpi(
		EmployeeId				BIGINT,
		EmployeeCode			VARCHAR(50),
		FullName				NVARCHAR(255),
		UserName				VARCHAR(255),
		UserId					INT,
		DepartmentName			NVARCHAR(255),
		TotalQuantity			INT,
		TotalFinish				INT,
		WorkPoint				DECIMAL(18,2),
		Rating					VARCHAR(50),
		RatingPoint				DECIMAL(18,2)
	)
	INSERT INTO #SuggesWorkKpi EXECUTE [kpi].[Get_SuggesWorkKpis] @FromDate,@ToDate,NULL,NULL

	SELECT
		temp.*,
		[FactorType]	= CASE
							WHEN [AvgPoint] <= 0 THEN '' 
						  ELSE
						  (SELECT TOP 1 [FactorType] FROM [kpi].[FactorConfig] WHERE temp.[AvgPoint] >= [FactorConditionMin] AND temp.[AvgPoint]< [FactorConditionMax])
						   END,
		[FactorPoint]	= CASE
							WHEN [AvgPoint] <= 0 THEN 0
						  ELSE
							(SELECT TOP 1 [FactorPointMax] FROM [kpi].[FactorConfig] WHERE temp.[AvgPoint] >= [FactorConditionMin] AND temp.[AvgPoint]< [FactorConditionMax])
						  END
	FROM
	(
		SELECT
			[EmployeeId]		= e.[EmployeeId],
			[EmployeeCode]		= e.[EmployeeCode],
			[FullName]			= e.[FullName],
			[DepartmentId]		= e.[DepartmentId],
			[DepartmentName]	= ued.[DepartmentName],
			[UsefullHourMin]	= (@TotalDay - ISNULL((SELECT SUM([NumberDays]) FROM [hrm].[HolidayDetail] hd 
											INNER JOIN [hrm].[EmployeeHoliday] eh ON eh.[EmployeeHolidayId] = hd.[EmployeeHolidayId]
											WHERE CAST([DateDay] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) 
											AND eh.[EmployeeId] = e.[EmployeeId]),0))*@MinHourConfig,
			[TotalUsefulHours]	= ISNULL((SELECT TOP 1 TotalUsefulHoursTask FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[SuggesPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #SuggesWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[ApprovedPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0),
			[ComplainPoint]		= ISNULL((SELECT TOP 1 RatingPoint FROM #ComplainKpi WHERE EmployeeId = e.EmployeeId),0),
			[AvgPoint]			= CASE
										WHEN ISNULL((SELECT TOP 1 TotalUsefulHoursTask FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0) < ((@TotalDay - ISNULL((SELECT SUM([NumberDays]) FROM [hrm].[HolidayDetail] hd 
											INNER JOIN [hrm].[EmployeeHoliday] eh ON eh.[EmployeeHolidayId] = hd.[EmployeeHolidayId]
											WHERE CAST([DateDay] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) 
											AND eh.[EmployeeId] = e.[EmployeeId]),0))*@MinHourConfig) THEN 0
										ELSE (
												ISNULL((SELECT TOP 1 RatingPoint FROM #FinishWorkKpi WHERE EmployeeId = e.EmployeeId),0) 
											+ 
												ISNULL((SELECT TOP 1 RatingPoint FROM #SuggesWorkKpi WHERE EmployeeId = e.EmployeeId),0) 
											+ 
												ISNULL((SELECT TOP 1 RatingPoint FROM #ComplainKpi WHERE EmployeeId = e.EmployeeId),0)
											)/3
									END
		FROM [hrm].[Employee] e
		LEFT JOIN [dbo].[UserEmployeeDepartment] ued on ued.[EmployeeId] = e.[EmployeeId]
		WHERE 
			ued.[Path] LIKE ''+ISNULL(@Path,ued.[Path])+'%'
	) AS temp


	DROP TABLE #ComplainKpi
	DROP TABLE #FinishWorkKpi
	DROP TABLE #SuggesWorkKpi
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FactorWorkKpisNew]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FactorWorkKpisNew]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@Path			VARCHAR(50),
	@TotalDay		INT
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @MinHourConfig DECIMAL(18,1)
	SET @MinHourConfig = ISNULL((SELECT TOP 1 [MinHours] FROM [kpi].[KpiConfig]),0)
	SELECT
		temp.*,
		[FactorType]	= CASE
							WHEN (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) <= 0 THEN '' 
						  ELSE
						  (SELECT TOP 1 [FactorType] FROM [kpi].[FactorConfig] WHERE (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) >= [FactorConditionMin] AND (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])< [FactorConditionMax])
						   END,
		[FactorPoint]	= CASE
							WHEN (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])<= 0 THEN 0
						  ELSE
							(SELECT TOP 1 [FactorPointMax] FROM [kpi].[FactorConfig] WHERE (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin]) >= [FactorConditionMin] AND (([UsefulHoursTask]+[UsefulHoursSuggesWork]*2)/[UsefullHourMin])< [FactorConditionMax])
						  END
		

	FROM
		(SELECT
			[EmployeeId]		= e.[EmployeeId],
			[EmployeeCode]		= e.[EmployeeCode],
			[FullName]			= e.[FullName],
			[DepartmentCompany]	= e.[DepartmentCompany],
			[DepartmentId]		= e.[DepartmentId],
			[DepartmentName]	= ued.[DepartmentName],
			TotalComplain = ISNULL((SELECT COUNT(ComplainId) FROM [kpi].[Complain] WHERE  [Status] = 2 AND CAST([CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate AND AccusedBy = ISNULL(ued.UserId,0)),0),
			[UsefullHourMin]	= (@TotalDay-(SELECT COUNT(HolidayId) FROM hrm.Holiday WHERE CAST([HolidayDate] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE)) - ISNULL((SELECT SUM([NumberDays]) FROM [hrm].[HolidayDetail] hd 
											INNER JOIN [hrm].[EmployeeHoliday] eh ON eh.[EmployeeHolidayId] = hd.[EmployeeHolidayId]
											WHERE CAST([DateDay] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE) 
											AND eh.[EmployeeId] = e.[EmployeeId]),0))*@MinHourConfig,
			[UsefulHoursTask]	= ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE  [Status] = 4 AND WorkType !=3   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(ued.UserId,0)),0),
			[UsefulHoursSuggesWork]	= ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE  [Status] = 4 AND WorkType =3    AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(ued.UserId,0)),0)
		FROM [hrm].[Employee] e
		LEFT JOIN [dbo].[UserEmployeeDepartment] ued on ued.[EmployeeId] = e.[EmployeeId]
		WHERE 
			ued.[Path] LIKE ''+ISNULL(@Path,ued.[Path])+'%'
			) AS temp
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FinishJobConfigCheck]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FinishJobConfigCheck]
	@FinishConfigId		INT,
	@ConditionMax		DECIMAL(18,2),
	@ConditionMin		DECIMAL(18,2)	
AS
BEGIN
	SELECT
		[FinishConfigId],
		[FinishType],
		[FinishPointMin],
		[FinishPointMax],
		[FinishConditionMin],
		[FinishConditionMax]
	FROM [kpi].[FinishJobConfig]
	WHERE 
			(	
		
			ISNULL([FinishConditionMin],@ConditionMax) < @ConditionMax AND @ConditionMax <ISNULL([FinishConditionMax],@ConditionMax)
		OR
			ISNULL([FinishConditionMin],@ConditionMin) < @ConditionMin AND @ConditionMin <ISNULL([FinishConditionMax],@ConditionMin)
			)
		AND
			[FinishConfigId] != @FinishConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FinishJobConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_FinishJobConfigs]

AS
BEGIN
	SELECT
		[FinishConfigId],
		[FinishType],
		[FinishPointMin],
		[FinishPointMax],
		[FinishConditionMin],
		[FinishConditionMax]
	FROM [kpi].[FinishJobConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_FinishWorkKpis]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_FinishWorkKpis]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@Path			VARCHAR(50)
AS
BEGIN
SELECT
	temp.*,
	Rating			= ISNULL((SELECT TOP 1 FinishType FROM [kpi].[FinishJobConfig] WHERE temp.TotalUsefulHoursTask >0 AND ( [FinishConditionMin]<=temp.TotalUsefulHoursReal/temp.TotalUsefulHoursTask AND temp.TotalUsefulHoursReal/temp.TotalUsefulHoursTask < [FinishConditionMax])),''),
	RatingPoint		= ISNULL((SELECT TOP 1 FinishPointMax FROM [kpi].[FinishJobConfig] WHERE temp.TotalUsefulHoursTask >0 AND ( [FinishConditionMin]<=temp.TotalUsefulHoursReal/temp.TotalUsefulHoursTask AND temp.TotalUsefulHoursReal/temp.TotalUsefulHoursTask < [FinishConditionMax])),0)
FROM 
(
	SELECT 
		e.EmployeeId,
		e.EmployeeCode,
		e.FullName,
		u.UserName,
		u.UserId,
		u.DepartmentName,
		TotalUsefulHoursTask	= ISNULL((SELECT SUM(Quantity*UsefulHoursTask) FROM [kpi].[WorkDetail] WHERE  [Status] = 4   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(u.UserId,0)),0),
		TotalUsefulHoursReal	= ISNULL((SELECT SUM(Quantity*UsefulHours) FROM [kpi].[WorkDetail] WHERE  [Status] = 4   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy = ISNULL(u.UserId,0)),0)
	FROM [hrm].[Employee] e
	LEFT JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId]
	WHERE
		 u.[Path] LIKE ''+ISNULL(@Path,u.[Path])+'%'
) AS temp
END
GO
/****** Object:  StoredProcedure [kpi].[Get_KpiConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_KpiConfig]
AS
BEGIN
	SELECT
		[KpiConfigId],
		[MinHours],
		[MaxHours],
		[PlanningDay],
		[PlanningHourMax],
		[PlanningHourMin],
		[HourConfirmMax],
		[HourConfirmMin],
		[Notification]
	FROM [kpi].[KpiConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Performer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_Performer]
 @PerformerBy		INT,
 @WorkStreamId		VARCHAR(50)
AS
BEGIN
	SELECT
		[PerformerBy],
		[WorkStreamId]	= CAST([WorkStreamId] AS VARCHAR(50))
	FROM [kpi].[Performer]
	WHERE	[PerformerBy] = @PerformerBy AND [WorkStreamId] = @WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Performers]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_Performers]
	@WorkStreamId	VARCHAR(50)
AS
BEGIN
	SELECT
		[PerformerBy],
		[WorkStreamId] = CAST([WorkStreamId] AS VARCHAR(50))
	FROM [kpi].[Performer]
	WHERE [WorkStreamId]=@WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggestJobCheckPoint]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_SuggestJobCheckPoint]
	@JobConfigId		INT,
	@ConditionMax		DECIMAL(18,2),
	@ConditionMin		DECIMAL(18,2)
AS
BEGIN
	SELECT
		[JobConfigId],
		[JobType],
		[JobPointMin],
		[JobPointMax],
		[JobConditionMin],
		[JobConditionMax]
	FROM [kpi].[SuggestJobConfig]
	WHERE 
			(	
			ISNULL([JobConditionMin],@ConditionMax) < @ConditionMax AND @ConditionMax <ISNULL([JobConditionMax],@ConditionMax)
		OR
			ISNULL([JobConditionMin],@ConditionMin) < @ConditionMin AND @ConditionMin <ISNULL([JobConditionMax],@ConditionMin)
			)
		AND
			[JobConfigId] != @JobConfigId	
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggestJobConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_SuggestJobConfigs]
AS
BEGIN
	SELECT
		[JobConfigId],
		[JobType],
		[JobPointMin],
		[JobPointMax],
		[JobConditionMin],
		[JobConditionMax]
	FROM [kpi].[SuggestJobConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggesWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_SuggesWork]
	@SuggesWorkId  VARCHAR(50)
AS
BEGIN
	SELECT			
		[SuggesWorkId]			= CAST(sw.[SuggesWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(sw.[TaskId] AS VARCHAR(50)),
		[FromDate]				= sw.[FromDate],
		[ToDate]				= sw.[ToDate],
		[Status]				= sw.[Status],
		[CreateBy]				= sw.[CreateBy],
		[CreateDate]			= sw.[CreateDate],
		[VerifiedBy]			= sw.[VerifiedBy],
		[VerifiedDate]			= sw.[VerifiedDate],
		[ApprovedFisnishBy]		= sw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= sw.[ApprovedFisnishDate],
		[Description]			= sw.[Description],
		[FisnishDate]			= sw.[FisnishDate],
		[Explanation]			= sw.[Explanation],
		[WorkingNote]			= sw.[WorkingNote],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CategoryKpiId]			= t.[CategoryKpiId],
		[UsefulHours]			= sw.[UsefulHours],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= sw.[WorkPointType],
		[Quantity]				= sw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[WorkPoint]				= sw.[WorkPoint],
		[FileConfirm]
	FROM [kpi].[SuggesWork] sw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  sw.[TaskId]
	WHERE
			[SuggesWorkId] = @SuggesWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggesWorkByTaskId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_SuggesWorkByTaskId]
	@TaskId			 VARCHAR(50),
	@SuggesWorkId	 VARCHAR(50)
AS
BEGIN
	SELECT
		[SuggesWorkId]			= CAST(sw.[SuggesWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(sw.[TaskId] AS VARCHAR(50)),
		[FromDate]				= sw.[FromDate],
		[ToDate]				= sw.[ToDate],
		[Status]				= sw.[Status],
		[CreateBy]				= sw.[CreateBy],
		[CreateDate]			= sw.[CreateDate],
		[VerifiedBy]			= sw.[VerifiedBy],
		[VerifiedDate]			= sw.[VerifiedDate],
		[ApprovedFisnishBy]		= sw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= sw.[ApprovedFisnishDate],
		[FisnishDate]			= sw.[FisnishDate],
		[Explanation]			= sw.[Explanation],
		[WorkingNote]			= sw.[WorkingNote],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[CategoryKpiId]			= t.[CategoryKpiId],
		[UsefulHours]			= sw.[UsefulHours],
		[WorkPointType]			= sw.[WorkPointType]
	FROM [kpi].[SuggesWork] sw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  sw.[TaskId]
	WHERE
			sw.[TaskId] = @TaskId
		AND
			sw.[SuggesWorkId] != @SuggesWorkId
			
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggesWorkKpis]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_SuggesWorkKpis]
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@Path			VARCHAR(50),
	@UserId			INT
AS
BEGIN
SELECT
	temp.*,
	Rating		= ISNULL((SELECT TOP 1 JobType FROM [kpi].[SuggestJobConfig] WHERE [JobConditionMin] <= WorkPoint AND WorkPoint < [JobConditionMax]),''),
	RatingPoint	= ISNULL((SELECT TOP 1 JobPointMax FROM [kpi].[SuggestJobConfig] WHERE [JobConditionMin] <= WorkPoint AND WorkPoint < [JobConditionMax]),0)
FROM 
(
	SELECT 
		e.EmployeeId,
		e.EmployeeCode,
		e.FullName,
		u.UserName,
		u.UserId,
		u.DepartmentName,
		TotalQuantity = ISNULL((SELECT COUNT(WorkDetailId) FROM [kpi].[WorkDetail] WHERE   CAST([ToDate] AS DATE) BETWEEN @FromDate AND @ToDate AND CreateBy = ISNULL(u.UserId,0)),0),
		TotalFinish = ISNULL((SELECT COUNT(WorkDetailId) FROM [kpi].[WorkDetail] WHERE [Status] = 4 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CreateBy= ISNULL(u.UserId,0)),0),
		WorkPoint = ISNULL((SELECT SUM(WorkPoint*Quantity) FROM [kpi].[WorkDetail] WHERE  [Status] = 4 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  AND CreateBy= ISNULL(u.UserId,0)),0) 
	FROM [hrm].[Employee] e
	LEFT JOIN [dbo].[UserEmployeeDepartment] u on u.[EmployeeId] = e.[EmployeeId]
	WHERE
		u.[Path] LIKE ''+ISNULL(@Path,u.[Path])+'%' AND u.UserId = ISNULL(@UserId,u.UserId )
) AS temp
END
GO
/****** Object:  StoredProcedure [kpi].[Get_SuggesWorks]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_SuggesWorks]
	@CreateBy		INT,
	@FromDate		DATETIME,
	@ToDate			DATETIME,
	@Action			INT
AS
BEGIN
	SELECT
		[SuggesWorkId]			= CAST(sw.[SuggesWorkId] AS VARCHAR(50)),
		[TaskId]				= CAST(sw.[TaskId] AS VARCHAR(50)),
		[FromDate]				= sw.[FromDate],
		[ToDate]				= sw.[ToDate],
		[Status]				= sw.[Status],
		[CreateBy]				= sw.[CreateBy],
		[CreateDate]			= sw.[CreateDate],
		[VerifiedBy]			= sw.[VerifiedBy],
		[VerifiedDate]			= sw.[VerifiedDate],
		[ApprovedFisnishBy]		= sw.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= sw.[ApprovedFisnishDate],
		[FisnishDate]			= sw.[FisnishDate],
		[Explanation]			= sw.[Explanation],
		[WorkingNote]			= sw.[WorkingNote],
		[Description]			= sw.[Description],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[UsefulHoursTask]		= t.[UsefulHours],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[CategoryKpiId]			= t.[CategoryKpiId],
		[UsefulHours]			= sw.[UsefulHours],
		[WorkPointType]			= sw.[WorkPointType],
		[VerifiedByName]		= u.[FullName],
		[Quantity]				= sw.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[FileConfirm]
	FROM [kpi].[SuggesWork] sw
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  sw.[TaskId]
	LEFT JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId]	= sw.[VerifiedBy]
	WHERE
			(
				((sw.[Status] = 1 OR sw.[Status] = 2) AND @Action = 10)
			 OR
				((sw.[Status] =3 OR sw.[Status]=4 OR sw.[Status]=5) AND @Action = 11)
			)
		AND
			sw.[CreateBy] = ISNULL(@CreateBy,sw.[CreateBy])
		AND 
			CAST(sw.[ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(sw.[ToDate] AS DATE)) AND ISNULL(@ToDate,CAST(sw.[ToDate] AS DATE))
		AND 
			CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE))
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Task]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_Task] 
	-- Add the parameters for the stored procedure here
	@TaskId		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[TaskId]				= CAST([TaskId] AS VARCHAR(50)),
		[TaskCode],
		[TaskName],
		[CalcType],
		[WorkPointConfigId],
		[UsefulHours],
		[Frequent],
		[Description],
		[IsSystem],
		[CreateDate],
		[CreateBy],
		[GroupName],
		[CategoryKpiId]
	FROM [kpi].[Task]
	WHERE [TaskId] = @TaskId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Task_ByTaskCode]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_Task_ByTaskCode] 
	-- Add the parameters for the stored procedure here
	@TaskCode		VARCHAR(50),
	@TaskId			VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[TaskId]				= CAST([TaskId] AS VARCHAR(50)),
		[TaskCode],
		[TaskName],
		[CalcType],
		[WorkPointConfigId],
		[UsefulHours],
		[Frequent],
		[Description],
		[IsSystem],
		[CreateDate],
		[CreateBy]
	FROM [kpi].[Task]
	WHERE [TaskCode] = @TaskCode AND [TaskId] != @TaskId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_Tasks]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [kpi].[Get_Tasks]
	-- Add the parameters for the stored procedure here
	@Keyword			NVARCHAR(255),
	@IsSystem			BIT,
	@CategoryKpiId		BIGINT,
	@UserId				INT
AS
BEGIN
	SET @Keyword = ISNULL(@Keyword,'')
	SET NOCOUNT ON;
	SELECT
		[TaskId]				= CAST([TaskId] AS VARCHAR(50)),
		[TaskCode],
		[TaskName],
		[CalcType],
		[WorkPointConfigId],
		[UsefulHours],
		[Frequent],
		t.[Description],
		[IsSystem],
		[CreateDate],
		[CreateBy],
		[GroupName],
		[CategoryKpiId]
	FROM [kpi].[Task] t
	WHERE	(
			t.[CategoryKpiId] = ISNULL(@CategoryKpiId,t.[CategoryKpiId])
		OR
			t.[CategoryKpiId] IS NULL
		)
		AND
			[CreateBy] = ISNULL(@UserId,[CreateBy])
		AND 
			[IsSystem] = ISNULL(@IsSystem,[IsSystem])
		AND
		(
				[TaskCode] LIKE '%' + @Keyword + '%'
			OR
				[TaskName] LIKE '%' + @Keyword + '%'
		)
	ORDER BY [TaskCode] ASC

END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_WorkDetail] 
	@WorkDetailId	VARCHAR(50),
	@WorkType		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [kpi].[WorkDetail]
	WHERE [WorkDetailId] = @WorkDetailId AND [WorkType] = ISNULL(@WorkType,[WorkType])
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_WorkDetails] 
	@UserId INT,
	@Action INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[TaskId]						= CAST(w.[TaskId] AS VARCHAR(50)),
		[FromDate],
		[ToDate],
		[UsefulHours]					= (w.[UsefulHours]*w.[Quantity]),
		[Status],
		[WorkDetailId]					= CAST([WorkDetailId] AS VARCHAR(50)),
		[WorkType],
		w.[Description],
		w.[CreateBy],
		[WorkingNote],
		[Explanation],
		w.[TaskName],
		w.[TaskCode],
		[CreateByName],
		[FisnishDate],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		w.[CreateDate],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[VerifiedBy],
		[EmployeeId],
		[UsefulHoursTask]				= (w.[UsefulHoursTask]*w.[Quantity]),
		[WorkPointType],
		w.[WorkPointConfigId],
		[WorkPoint],
		w.[CalcType],
		[DescriptionTask],
		[EmployeeCode],
		[DepartmentId],
		[Quantity],
		[FileConfirm],
		w.[DepartmentCompany]


	FROM [kpi].[WorkDetail] w
	LEFT JOIN kpi.Task t On t.TaskId =w.TaskId
	LEFT JOIN kpi.WorkPointConfig wp On wp.WorkPointConfigId =t.WorkPointConfigId
	WHERE
			(wp.WorkPointA > 0 AND [Status]!=4 AND CAST(GETDATE () AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE)   AND @Action =25 AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])  )
		OR
			(CAST(GETDATE () AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE)   AND @Action =20  )
		OR
			([VerifiedBy] IS NOT NULL AND ([Status] =1 OR  [Status] =2 OR ([Status] = 5 AND [DepartmentFisnishBy] IS  NULL)) AND CAST([ToDate] AS DATE) >= CAST(GETDATE () AS DATE) AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action =0 )
		OR
			(wp.WorkPointA = 0 AND [VerifiedBy] IS NOT NULL AND ([Status] =1 OR  [Status] =2 OR ([Status] = 5 AND [DepartmentFisnishBy] IS  NULL)) AND CAST([ToDate] AS DATE) >= CAST(GETDATE () AS DATE) AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action =101 )
		OR
			([VerifiedBy] IS NOT NULL AND [Status]=4   AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action =1 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  )
		OR
			([VerifiedBy] IS NOT NULL   AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action =11 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)) )
		OR
			([VerifiedBy] IS NOT NULL AND @Action =3 AND ([Status] =3 ) AND [DepartmentFisnishBy] IS NOT NULL  AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		OR
			([VerifiedBy] IS NOT NULL  AND  @Action = 4 AND ( [Status] =4 OR [Status] = 5)  AND [ApprovedFisnishBy] IS NOT NULL AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE))    AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			(@Action =5 AND ([Status] =5 AND [DepartmentFisnishBy] IS NULL) AND [VerifiedBy] IS NOT NULL  AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		OR
			(@Action =22  AND CAST([ToDate] AS DATE) < CAST(GETDATE () AS DATE)AND [Status] !=4 AND [Status] !=5  AND [Status] !=3 AND [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND [Explanation] IS NULL )
		OR
			(@Action =2 AND [Status] !=4 AND [Status] !=5 AND CAST([ToDate] AS DATE) < CAST(GETDATE () AS DATE) AND [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)) )
		OR
			(@Action =6 AND [Status] !=4 AND [Status] !=3 AND [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])  AND  (CAST(GETDATE () AS DATE) < CAST([ToDate] AS DATE) ))
		OR
			(@Action =59 AND  [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])  AND   CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND  ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			(@Action =60 AND  [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])  AND   CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND  ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			(@Action =61 AND  [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND ([Status] = 3 OR [Status] = 4) AND   CAST([ToDate] AS DATE) = ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			(@Action =62 AND  [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])AND [Status] != 3 AND [Status] != 5  AND [Status] != 4 AND   CAST([ToDate] AS DATE) = ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			(@Action =63 AND  [VerifiedBy] IS NOT NULL AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])  AND ([Status] =5 OR  ([Status] !=4 AND   CAST(GETDATE () AS DATE) > CAST([ToDate] AS DATE) ))AND   CAST([ToDate] AS DATE) = ISNULL(@ToDate,CAST([ToDate] AS DATE)))
		OR
			([VerifiedBy] IS NOT NULL AND [Status] =4   AND w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action =9 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))  )
		OR
			( CAST(w.[EmployeeId] AS INT) = ISNULL(@UserId,CAST(w.[EmployeeId] AS INT)) AND @Action =10 AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))
			AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE))  )
		OR
			(w.[CreateBy] = ISNULL(@UserId,w.[CreateBy]) AND @Action = 7 AND ([Status] = 4 OR [Status] = 3 OR ([Status] = 5 AND [DepartmentFisnishBy] IS NOT NULL)) AND  CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
	--ORDER BY [TaskCode] ASC
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkDetails_NextWeek]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_WorkDetails_NextWeek] 
	@UserId INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		[TaskId]				= CAST(wp.[TaskId] AS VARCHAR(50)),
		[FromDate]				= wp.[FromDate],
		[ToDate]				= wp.[ToDate],
		[UsefulHours]			= (wp.[UsefulHours] * wp.[Quantity]),
		[Status]				= wp.[Status],
		[WorkDetailId]			= CAST([WorkPlanDetailId] AS VARCHAR(50)),
		[WorkType]				= 1,
		[Description]			= wp.[Description],
		[CreateBy]				= w.[CreateBy],
		[WorkingNote]			= [WorkingNote], 
		[Explanation]			= [Explanation],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateByName]			= u.[FullName],
		[FisnishDate]			= [FisnishDate],
		[ApprovedFisnishBy]		= [ApprovedFisnishBy],
		[ApprovedFisnishDate]	= [ApprovedFisnishDate],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[CreateDate]			= w.[CreateDate],
		[VerifiedBy]			= w.[ApprovedBy],
		[EmployeeId]			= u.[EmployeeId],
		[UsefulHoursTask]		= (t.[UsefulHours] * wp.[Quantity]),
		[WorkPointType]			= wp.[WorkPointType],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPoint]				= [WorkPoint],
		[CalcType]				= t.[CalcType],
		[DescriptionTask]		= t.[Description],
		[EmployeeCode]			= u.[EmployeeCode],
		[DepartmentId]			= u.[DepartmentId],
		[Quantity]				= wp.[Quantity]
	FROM [kpi].[WorkPlanDetail] wp
	INNER JOIN [kpi].[WorkPlan]  w ON w.[WorkPlanId] = wp.[WorkPlanId]
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = wp.[TaskId]
	INNER JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] =  w.[CreateBy]
	WHERE 
			CAST(wp.[FromDate] AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE)
		AND
			w.[CreateBy] = @UserId
	ORDER BY [TaskCode] ASC
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkDetails_Path]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Get_WorkDetails_Path] 
	@Path VARCHAR(50),
	@Action INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		[TaskId]						= CAST([TaskId] AS VARCHAR(50)),
		[FromDate],
		[ToDate],
		[UsefulHours]					= (w.[UsefulHours]*w.[Quantity]),
		[Status],
		[WorkDetailId]					= CAST([WorkDetailId] AS VARCHAR(50)),
		[WorkType],
		[Description],
		[CreateBy],
		[WorkingNote],
		[Explanation],
		[TaskName],
		[TaskCode],
		[CreateByName],
		[FisnishDate],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[CreateDate],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[VerifiedBy],
		[EmployeeId]					= w.[EmployeeId],
		[UsefulHoursTask]				= (w.[UsefulHoursTask]*w.[Quantity]),
		[WorkPointType],
		[WorkPointConfigId],
		[WorkPoint],
		[CalcType],
		[DescriptionTask],
		[EmployeeCode]					= w.[EmployeeCode],
		[DepartmentId]					= w.[DepartmentId],
		[Quantity],
		[FileConfirm]
	FROM [kpi].[WorkDetail] w
	LEFT JOIN [dbo].[UserEmployeeDepartment]  ued ON ued.UserId = w.CreateBy
	WHERE
		(

			(@Action =7 AND  [VerifiedBy] IS NULL AND ( [WorkType] =2 OR [WorkType]=3)   AND CAST(GETDATE () AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE))
		OR
			(@Action =8 AND  [VerifiedBy] IS NOT NULL AND ( [WorkType] =2 OR [WorkType]=3)   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		OR
			(@Action =6 AND [Status] =4  AND  [VerifiedBy] IS NOT NULL   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) )
		OR
			(@Action =3 AND ([Status] =3 AND [DepartmentFisnishBy] IS NULL )AND [ApprovedFisnishDate] IS  NULL AND [VerifiedBy] IS NOT NULL   AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		OR
			([VerifiedBy] IS NOT NULL  AND @Action =4 AND ([Status] =3 OR [Status] =4  OR [Status] =5)  AND [DepartmentFisnishBy] IS NOT NULL  AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		OR
			(@Action =5 AND ([Status] =5 AND [DepartmentFisnishBy] IS NULL) AND [VerifiedBy] IS NOT NULL  AND CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE)) AND CAST([FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([FromDate] AS DATE)) AND ISNULL(@ToDate,CAST([FromDate] AS DATE)))
		)
		AND
			ued.[Path] LIKE ''+@Path+'%'
	ORDER BY [TaskCode] ASC
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlan]
	@WorkPlanId		VARCHAR(50)
AS
BEGIN
	SELECT
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode],
		[CreateBy],
		[FromDate],
		[ToDate],
		[CreateDate],
		[Description],
		[ConfirmedBy],
		[ApprovedBy],
		[ConfirmedDate],
		[ApprovedDate]
	FROM [kpi].[WorkPlan] w
	WHERE [WorkPlanId] = @WorkPlanId

	SELECT
		[WorkPlanDetailId]		= CAST(wd.[WorkPlanDetailId] AS VARCHAR(50)),
		[TaskId]				= CAST(wd.[TaskId] AS VARCHAR(50)),
		[WorkPlanId]			= CAST(wd.[WorkPlanId] AS VARCHAR(50)),
		[FromDate]				= wd.[FromDate],
		[ToDate]				= wd.[ToDate],
		[Description]			= wd.[Description],
		[Explanation]			= wd.[Explanation],
		[WorkingNote]			= wd.[WorkingNote],
		[Status]				= wd.[Status],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= wd.[WorkPointType],
		[UsefulHours]			= wd.[UsefulHours],
		[UsefulHourTask]		= t.[UsefulHours],
		[ApprovedFisnishBy]		= wd.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= wd.[ApprovedFisnishDate],
		[FisnishDate]			= wd.[FisnishDate],
		[WorkPlanCode]			= w.[WorkPlanCode],
		[CreateByUserName]		= s.[UserName],
		[CreateByFullName]		= s.[FullName],
		[ConfirmByName]			= e.[UserName],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[Quantity]				= [Quantity]
	FROM [kpi].[WorkPlanDetail] wd
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  wd.[TaskId]
	INNER JOIN [kpi].[WorkPlan] w ON w.[WorkPlanId] =  wd.[WorkPlanId]
	INNER JOIN [dbo].[User] s ON s.[UserId] =  w.[CreateBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] =  wd.[ApprovedFisnishBy]
	WHERE wd.[WorkPlanId] = @WorkPlanId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlan_ByDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlan_ByDate]
	@UserId				INT, --- nguuoi tạo
	@Date				DATE
AS
BEGIN
	SELECT
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[FromDate]			= wp.[FromDate],
		[ToDate]			= wp.[ToDate],
		[CreateDate]		= wp.[CreateDate],
		[Description]		= wp.[Description],
		[ConfirmedBy]		= wp.[ConfirmedBy],
		[ApprovedBy]		= wp.[ApprovedBy],
		[ConfirmedDate]		= wp.[ConfirmedDate],
		[ApprovedDate]		= wp.[ApprovedDate]
	FROM [kpi].[WorkPlan] wp
	WHERE 
			wp.[CreateBy] = @UserId
		AND 
			@Date BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE)
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlan_NeedComplete]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlan_NeedComplete]
	@PercentTime	DECIMAL(18,2)
AS
BEGIN
	SELECT
		[WorkPlanId],
		[WorkPlanCode],
		[CreateBy],
		[FromDate],
		[ToDate],
		[CreateDate],
		[Description],
		[ConfirmedBy],
		[ApprovedBy],
		[NeedCompleted]
	FROM
	(
		SELECT
			 [WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
			 [WorkPlanCode],
			 [CreateBy],
			 [FromDate],
			 [ToDate],
			 [CreateDate],
			 [Description],
			 [ConfirmedBy],
			 [ApprovedBy],
			 [PercentTime]		= (CAST(DATEDIFF(HOUR,CAST([FromDate] AS DATE),CAST([ToDate] AS DATE)) AS DECIMAL(18,2))/CAST(DATEDIFF(HOUR,CAST([FromDate] AS DATE),CAST(GETDATE() AS DATE)) AS DECIMAL(18,2)))*100,
			 [NeedCompleted]	= ISNULL((SELECT COUNT([WorkPlanDetailId]) FROM [kpi].[WorkPlanDetail] WHERE [WorkPlanId] = wp.[WorkPlanId] AND [Status] = 1),0)
		FROM [kpi].[WorkPlan] wp
		WHERE 
				CAST(GETDATE() AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE)
			AND
				wp.[ApprovedBy] IS NOT NULL
	) wp
	WHERE 
			wp.[NeedCompleted] > 0
		AND
			wp.[PercentTime] BETWEEN @PercentTime AND 100
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlanCheckDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlanCheckDate]
	@CheckDate  DATETIME,
	@WorkPlanId VARCHAR(50),
	@UserId		INT
AS
BEGIN
	SELECT TOP 1
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode],
		[CreateBy],
		[FromDate],
		[ToDate],
		[CreateDate],
		[Description],
		[ConfirmedBy],
		[ApprovedBy],
		[ConfirmedDate],
		[ApprovedDate]
	FROM [kpi].[WorkPlan] 
	WHERE
			 @CheckDate BETWEEN [FromDate] AND [ToDate]
		 AND
			[WorkPlanId] != @WorkPlanId
		AND 
			[CreateBy] = @UserId		
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlanDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlanDetail]
	@WorkPlanDetailId  VARCHAR(50)
AS
BEGIN
	SELECT
		[WorkPlanDetailId]	 = CAST(wd.[WorkPlanDetailId] AS VARCHAR(50)),
		[TaskId]				= CAST(wd.[TaskId] AS VARCHAR(50)),
		[WorkPlanId]			= CAST(wd.[WorkPlanId] AS VARCHAR(50)),
		[FromDate]				= wd.[FromDate],
		[ToDate]				= wd.[ToDate],
		[Description]			= wd.[Description],
		[Explanation]			= wd.[Explanation],
		[WorkingNote]			= wd.[WorkingNote],
		[Status]				= wd.[Status],
		[TaskName]				= t.[TaskName],
		[TaskCode]				= t.[TaskCode],
		[CreateTaskName]		= u.[UserName],
		[UsefulHours]			= wd.[UsefulHours],
		[ApprovedFisnishBy]		= wd.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	= wd.[ApprovedFisnishDate],
		[FisnishDate]			= wd.[FisnishDate],
		[WorkPointConfigId]		= t.[WorkPointConfigId],
		[WorkPointType]			= wd.[WorkPointType],
		[Quantity]				= wd.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[WorkPoint]				= wd.[WorkPoint],
		[FileConfirm]
	FROM [kpi].[WorkPlanDetail] wd
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  wd.[TaskId]
	INNER JOIN [dbo].[User] u ON u.[UserId] =  t.[CreateBy]
	WHERE wd.[WorkPlanDetailId] = @WorkPlanDetailId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlanDetail_NeedCompleted]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlanDetail_NeedCompleted]
	@Date		DATETIME
AS
BEGIN
	SELECT
		[WorkPlanDetailId]  = CAST(wd.[WorkPlanDetailId] AS VARCHAR(50)),
		[TaskId]			= CAST(wd.[TaskId] AS VARCHAR(50)),
		[WorkPlanId]		= CAST(wd.[WorkPlanId] AS VARCHAR(50)),
		[FromDate]			= wd.[FromDate],
		[ToDate]			= wd.[ToDate],
		[TaskName]			= t.[TaskName],
		[TaskCode]			= t.[TaskCode],
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[Quantity]			= wd.[Quantity]
	FROM [kpi].[WorkPlanDetail] wd
	INNER JOIN [kpi].[WorkPlan] wp ON wd.[WorkPlanId] = wp.[WorkPlanId]
	INNER JOIN [kpi].[Task] t ON t.[TaskId] =  wd.[TaskId]
	WHERE 
			wp.[ApprovedBy] IS NOT NULL
		AND
			CAST(wd.[ToDate] AS DATE) = CAST(@Date AS DATE)
		AND
			wd.[Status] = 1
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlans]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlans]
	@UserId					INT, --- nguuoi tạo
	@FromDate				DATE,
	@ToDate					DATE
AS
BEGIN
	SELECT
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[FromDate]			= wp.[FromDate],
		[ToDate]			= wp.[ToDate],
		[CreateDate]		= wp.[CreateDate],
		[Description]		= wp.[Description],
		[ConfirmedBy]		= wp.[ConfirmedBy],
		[ApprovedBy]		= wp.[ApprovedBy],
		[ConfirmedDate]		= wp.[ConfirmedDate],
		[ApprovedDate]		= wp.[ApprovedDate],
		[ConfirmedByName]	= u.[UserName],
		[ApprovedByName]	= e.[UserName],
		[CreateByName]		= s.[UserName]
	FROM [kpi].[WorkPlan] wp
	LEFT JOIN [dbo].[User] u ON u.[UserId] = wp.[ConfirmedBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] = wp.[ApprovedBy]
	LEFT JOIN [dbo].[User] s ON s.[UserId] = wp.[CreateBy]
	WHERE 
			wp.[CreateBy] = ISNULL(@UserId,wp.[CreateBy])
		AND 
			CAST(wp.[FromDate] AS DATE) BETWEEN @FromDate AND @ToDate AND  CAST([ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST([ToDate] AS DATE)) AND ISNULL(@ToDate,CAST([ToDate] AS DATE))
	ORDER BY wp.[FromDate] DESC
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlans_ByDepartmentId]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlans_ByDepartmentId]
	@Path					VARCHAR(50),
	@FromDate				DATE,
	@ToDate					DATE,
	@Action					TINYINT
AS
BEGIN
	SELECT
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[FromDate]			= wp.[FromDate],
		[ToDate]			= wp.[ToDate],
		[CreateDate]		= wp.[CreateDate],
		[Description]		= wp.[Description],
		[ConfirmedBy]		= wp.[ConfirmedBy],
		[ApprovedBy]		= wp.[ApprovedBy],
		[ConfirmedDate]		= wp.[ConfirmedDate],
		[ApprovedDate]		= wp.[ApprovedDate],
		[ConfirmedByName]	= u.[UserName],
		[ApprovedByName]	= e.[UserName],
		[CreateByName]		= ued.[UserName],
		[DepartmentId]		= ued.[DepartmentId],
		[EmployeeCode]		= ued.[EmployeeCode],
		[FullName]			= ued.[FullName]
	FROM [kpi].[WorkPlan] wp
	LEFT JOIN [dbo].[User] u ON u.[UserId] = wp.[ConfirmedBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] = wp.[ApprovedBy]
	INNER JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = wp.[CreateBy]
	WHERE 
	(
			(wp.[ConfirmedBy] IS NULL AND @Action=1 AND CAST(GETDATE () AS DATE) <= CAST([ToDate] AS DATE) )
		OR
			(wp.[ConfirmedBy] IS NOT NULL AND @Action=2 )
		OR
			(wp.[ConfirmedBy] IS NOT NULL AND wp.[ApprovedBy] IS NULL  AND @Action=3 )
		OR
			(wp.[ConfirmedBy] IS NOT NULL AND wp.[ApprovedBy] IS NOT NULL  AND @Action=4)
		OR
			(wp.[ConfirmedBy] IS NULL  AND @Action=5 AND CAST(GETDATE () AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE))
		OR
			(wp.[ConfirmedBy] IS NOT NULL AND wp.[ApprovedBy] IS NULL  AND @Action=6 AND CAST(GETDATE () AS DATE) BETWEEN CAST([FromDate] AS DATE) AND CAST([ToDate] AS DATE))
			)
		AND
			CAST(wp.[FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(wp.[FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST(wp.[FromDate] AS DATE)) AND CAST(wp.[ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(wp.[ToDate] AS DATE)) AND  ISNULL(@ToDate,CAST(wp.[ToDate] AS DATE))
		AND
			ued.[Path] LIKE ''+ISNULL(@Path,ued.[Path])+'%'
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlans_ByToDate]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlans_ByToDate]
	@UserId						INT, --- nguuoi tạo
	@ToDateOld					DATE
AS
BEGIN
	SELECT TOP 1
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[FromDate]			= wp.[FromDate],
		[ToDate]			= wp.[ToDate],
		[CreateDate]		= wp.[CreateDate],
		[Description]		= wp.[Description],
		[ConfirmedBy]		= wp.[ConfirmedBy],
		[ApprovedBy]		= wp.[ApprovedBy],
		[ConfirmedDate]		= wp.[ConfirmedDate],
		[ApprovedDate]		= wp.[ApprovedDate],
		[CreateByName]		= s.[UserName]
	FROM [kpi].[WorkPlan] wp
	INNER JOIN [dbo].[User] s ON s.[UserId] = wp.[CreateBy]
	WHERE 
			wp.[CreateBy] = ISNULL(@UserId,wp.[CreateBy])
		AND
			[FromDate] > @ToDateOld
	ORDER BY wp.[FromDate] 
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPlans_IsActive]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPlans_IsActive]
AS
BEGIN
	SELECT
		[WorkPlanId]		= CAST([WorkPlanId] AS VARCHAR(50)),
		[WorkPlanCode]		= wp.[WorkPlanCode],
		[CreateBy]			= wp.[CreateBy],
		[FromDate]			= wp.[FromDate],
		[ToDate]			= wp.[ToDate],
		[CreateDate]		= wp.[CreateDate],
		[Description]		= wp.[Description],
		[ConfirmedBy]		= wp.[ConfirmedBy],
		[ApprovedBy]		= wp.[ApprovedBy],
		[ConfirmedDate]		= wp.[ConfirmedDate],
		[ApprovedDate]		= wp.[ApprovedDate],
		[ConfirmedByName]	= u.[UserName],
		[ApprovedByName]	= e.[UserName],
		[CreateByName]		= s.[UserName]
	FROM [kpi].[WorkPlan] wp
	LEFT JOIN [dbo].[User] u ON u.[UserId] = wp.[ConfirmedBy]
	LEFT JOIN [dbo].[User] e ON e.[UserId] = wp.[ApprovedBy]
	LEFT JOIN [dbo].[User] s ON s.[UserId] = wp.[CreateBy]
	WHERE 
			[ToDate] >= GETDATE ( ) 
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPointConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPointConfig]
	@WorkPointConfigId INT
AS
BEGIN
	SELECT
		[WorkPointConfigId],
		[WorkPointName],
		[Description],
		[WorkPointA],
		[WorkPointB],
		[WorkPointC],
		[WorkPointD]
	FROM [kpi].[WorkPointConfig]
	WHERE [WorkPointConfigId] = @WorkPointConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkPointConfigs]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkPointConfigs]
AS
BEGIN
	SELECT
		[WorkPointConfigId],
		[WorkPointName],
		[Description],
		[WorkPointA],
		[WorkPointB],
		[WorkPointC],
		[WorkPointD]
	FROM [kpi].[WorkPointConfig]
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkStream]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_WorkStream]
	@WorkStreamId    VARCHAR(50)
AS
BEGIN
	SELECT
		[WorkStreamId] = CAST(ws.[WorkStreamId] AS VARCHAR(50)),
		[WorkStreamCode],
		[CreateBy]		= ws.[CreateBy],
		[CreateDate]	= ws.[CreateDate],
		[FromDate]		= ws.[FromDate],
		[ToDate]		= ws.[ToDate],
		[AssignWorkId]  = CAST(ws.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]		= CAST(ws.[TaskId] AS VARCHAR(50)),
		[Description]	= ws.[Description],
		[Status]		= ws.[Status],
		[ApprovedBy],
		[ApprovedDate],
		[TaskCode]		= t.[TaskCode],
		[TaskAssCode]	= t2.[TaskCode]
	FROM [kpi].[WorkStream] ws
	LEFT JOIN [kpi].[Task] t ON t.[TaskId] = ws.[TaskId]
	LEFT JOIN [kpi].[AssignWork] a ON a.[AssignWorkId] = ws.[AssignWorkId]
	LEFT JOIN [kpi].[Task] t2 ON t2.[TaskId] = a.TaskId
	WHERE [WorkStreamId] = @WorkStreamId
	SELECT
		[WorkStreamDetailId]	= CAST([WorkStreamDetailId] AS VARCHAR(50)),
		[TaskId]				 = CAST(ws.[TaskId] AS VARCHAR(50)),
		[WorkStreamId]			 = CAST([WorkStreamId] AS VARCHAR(50)),
		[FromDate],
		[ToDate],
		[CreateBy]				 = ws.[CreateBy],
		[CreateDate]			 = ws.[CreateDate],
		[Status]				 = ws.[Status],
		[Description]			 = ws.[Description],
		[IsDefault],
		[UsefulHours]			 = ws.[UsefulHours],
		[WorkPointConfigId]		 = t.[WorkPointConfigId],
		[WorkPointType]			 = ws.[WorkPointType],
		[VerifiedBy]			 = ws.[VerifiedBy],
		[VerifiedDate]			 = ws.[VerifiedDate],
		[ApprovedFisnishBy]		 = ws.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	 = ws.[ApprovedFisnishDate],
		[FisnishDate]			 = ws.[FisnishDate],
		[TaskCode]				 = t.[TaskCode],
		[TaskName]				 = t.[TaskName],
		[CreateByName]			 = u.[FullName],
		[CreateByCode]			 = u.[EmployeeCode],
		[DepartmentName]		 = u.[DepartmentName],
		[Explanation]			 = ws.[Explanation],
		[WorkingNote]	 		 = ws.[WorkingNote],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[Quantity]				= [Quantity]
	FROM [kpi].[WorkStreamDetail] ws
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = ws.[TaskId]
	LEFT JOIN [dbo].[UserEmployeeDepartment] u ON u.[UserId] = ws.[CreateBy]
	WHERE 
			ws.[WorkStreamId] = @WorkStreamId
		AND
			ws.[IsDefault] = 0
			

	SELECT
		[PerformerBy],
		[WorkStreamId]	= CAST([WorkStreamId] AS VARCHAR(50))

	FROM [kpi].[Performer] p
	WHERE	p.[WorkStreamId] = @WorkStreamId 

END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkStreamDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Get_WorkStreamDetail]
	@WorkStreamDetailId   VARCHAR(50)
AS
BEGIN
	SELECT
		[WorkStreamDetailId]	= CAST([WorkStreamDetailId] AS VARCHAR(50)),
		[TaskId]				 = CAST(ws.[TaskId] AS VARCHAR(50)),
		[WorkStreamId]			 = CAST(ws.[WorkStreamId] AS VARCHAR(50)),
		[FromDate]				 = ws.[FromDate],
		[ToDate]				 = ws.[ToDate],
		[CreateBy]				 = ws.[CreateBy],
		[CreateDate]			 = ws.[CreateDate],
		[Status]				 = ws.[Status],
		[Description]			 = ws.[Description],
		[IsDefault],
		[UsefulHours]			 = ws.[UsefulHours],
		[VerifiedBy]			 = ws.[VerifiedBy],
		[VerifiedDate]			 = ws.[VerifiedDate],
		[ApprovedFisnishBy]		 = ws.[ApprovedFisnishBy],
		[ApprovedFisnishDate]	 = ws.[ApprovedFisnishDate],
		[FisnishDate]			 = ws.[FisnishDate],
		[TaskCode]				 = t.[TaskCode],
		[TaskName]				 = t.[TaskName],
		[WorkPointConfigId]		 = t.[WorkPointConfigId],
		[WorkPointType]			 = ws.[WorkPointType],
		[CreateByName]			 = ued.[FullName],
		[CreateByCode]			 = ued.[EmployeeCode],
		[WorkStreamCode]		 = w.[WorkStreamCode],
		[Explanation]			 = ws.[Explanation],
		[WorkingNote]	 		 = ws.[WorkingNote],
		[Quantity]				 = ws.[Quantity],
		[DepartmentFisnishBy]	= [DepartmentFisnishBy],
		[DepartmentFisnishDate]	= [DepartmentFisnishDate],
		[WorkPoint]				= ws.[WorkPoint],
		[FileConfirm]
	FROM [kpi].[WorkStreamDetail] ws
	INNER JOIN [kpi].[Task] t ON t.[TaskId] = ws.[TaskId]
	INNER JOIN [kpi].[WorkStream] w ON w.[WorkStreamId] = ws.[WorkStreamId]
	LEFT JOIN [dbo].[UserEmployeeDepartment] ued ON ued.[UserId] = ws.[CreateBy]
	WHERE [WorkStreamDetailId] = @WorkStreamDetailId
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkStreams]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkStreams]
	@UserId					INT, --- nguuoi tạo
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Action					INT
AS
BEGIN
	SELECT
		[WorkStreamId] = CAST(w.[WorkStreamId] AS VARCHAR(50)),
		[WorkStreamCode],
		[CreateBy]		= w.[CreateBy],
		[CreateDate]	= w.[CreateDate],
		[FromDate]		= w.[FromDate],
		[ToDate]		= w.[ToDate],
		[AssignWorkId]  = CAST(w.[AssignWorkId] AS VARCHAR(50)),
		[TaskId]		= CAST(w.[TaskId] AS VARCHAR(50)),
		[Description]	= w.[Description],
		[Status]		= w.[Status],
		[ApprovedBy]	= w.[ApprovedBy],
		[ApprovedDate]	= w.[ApprovedDate],
		[TaskCode]		= t.[TaskCode],
		[TaskAssCode]	= t2.[TaskCode],
		[ApprovedByName]= u.[FullName],
		[TaskName]		= t.[TaskName],
		[TaskAssName]	= t2.[TaskName],
		[CreateByName]	= u2.[FullName]
	FROM [kpi].[WorkStream] w
	LEFT JOIN [kpi].[Task] t ON t.[TaskId] = w.[TaskId]
	LEFT JOIN [kpi].[AssignWork] a ON a.[AssignWorkId] = w.[AssignWorkId]
	LEFT JOIN [kpi].[Task] t2 ON t2.[TaskId] = a.TaskId
	INNER JOIN [kpi].[Performer] pf ON pf.WorkStreamId = w.WorkStreamId
	LEFT JOIN [dbo].[User] u ON w.[ApprovedBy] = u.[UserId]
	INNER JOIN [dbo].[User] u2 ON w.[CreateBy] = u2.[UserId]
	WHERE
			(
			(w.[ApprovedBy] IS NULL AND @Action=1 )
		OR
			(w.[ApprovedBy] IS NOT NULL AND @Action=2)
		OR
			(@Action=0)
			)
		AND
			(
				w.[CreateBy] = ISNULL(@UserId,w.[CreateBy])
			OR
				pf.PerformerBy = ISNULL(@UserId,pf.PerformerBy)
			)
		AND 
			CAST(w.[FromDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(w.[FromDate] AS DATE)) AND  ISNULL(@ToDate,CAST(w.[FromDate] AS DATE)) AND  CAST(w.[ToDate] AS DATE) BETWEEN ISNULL(@FromDate,CAST(w.[ToDate] AS DATE)) AND ISNULL(@ToDate,CAST(w.[ToDate] AS DATE))

	GROUP BY w.[WorkStreamId],
			[WorkStreamCode],
			w.[CreateBy],
			w.[CreateDate],
			w.[FromDate],
			 w.[ToDate],
			w.[AssignWorkId],
			w.[TaskId],
			w.[Description],
			w.[Status],
			[ApprovedBy],
			[ApprovedDate],
			t.[TaskCode],
			t2.[TaskCode],
			u.[FullName],
			t.[TaskName],
			t2.[TaskName],
			u2.[FullName]
	ORDER BY w.[CreateDate] DESC
END
GO
/****** Object:  StoredProcedure [kpi].[Get_WorkStreams_NeedVerify]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Get_WorkStreams_NeedVerify]
AS
BEGIN
	SELECT
		[WorkStreamId]     = CAST([WorkStreamId] AS VARCHAR(50)),
		[WorkStreamCode],
		[CreateBy],
		[CreateDate],
		[FromDate],
		[ToDate],
		[AssignWorkId]		= CAST([AssignWorkId] AS VARCHAR(50)),
		[TaskId]			= CAST([TaskId] AS VARCHAR(50)),
		[Description],
		[Status],
		[ApprovedBy],
		[ApprovedDate]
	FROM [kpi].[WorkStream]
	WHERE [ApprovedBy] IS NULL OR [ApprovedBy] = 0
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_AssignWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_AssignWork]
	@AssignWorkId			VARCHAR(50),
	@TaskId					VARCHAR(50),
	@CreateBy				INT,
	@AssignBy				INT,
	@CreateDate				DATETIME,
	@FromDate				DATETIME,
	@Status					TINYINT,
	@ToDate					DATETIME,
	@Description			NVARCHAR(500),
	@UsefulHours			DECIMAL(18,1),
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDatE	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT,
	@DepartmentFollowBy		INT,
	@DirectorFollowBy		INT,
	@TypeAssignWork			INT
AS

BEGIN
	INSERT INTO [kpi].[AssignWork]
	(
		[AssignWorkId],
		[TaskId],
		[CreateBy],
		[AssignBy],
		[CreateDate],
		[FromDate],
		[Status],
		[ToDate],
		[Description],
		[UsefulHours],
		[Explanation],
		[WorkingNote],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[FisnishDate],
		[WorkPointType],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[Quantity],
		[DepartmentFollowBy],
		[DirectorFollowBy],
		[TypeAssignWork]
	)
	VALUES
	(
		@AssignWorkId,
		@TaskId,
		@CreateBy,
		@AssignBy,
		@CreateDate,
		@FromDate,
		@Status,
		@ToDate,
		@Description,
		@UsefulHours,
		@Explanation,
		@WorkingNote,
		@ApprovedFisnishBy,
		@ApprovedFisnishDate,
		@FisnishDate,
		@WorkPointType,
		@DepartmentFisnishBy,
		@DepartmentFisnishDate,
		@Quantity,
		@DepartmentFollowBy,
		@DirectorFollowBy,
		@TypeAssignWork
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_Complain]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_Complain]
	@ComplainId			VARCHAR(50),
	@CreateBy			INT,
	@AccusedBy			INT,
	@Description		NVARCHAR(MAX),
	@CreateDate			DATETIME,
	@ConfirmedBy		INT,
	@ConfirmedDate		DATETIME,
	@Status				TINYINT
AS

BEGIN
	INSERT INTO [kpi].[Complain]
	(
		[ComplainId],
		[CreateBy],
		[AccusedBy],
		[Description],
		[CreateDate],
		[ConfirmedBy],
		[ConfirmedDate],
		[Status]
	)
	VALUES
	(
		@ComplainId,
		@CreateBy,
		@AccusedBy,
		@Description,
		@CreateDate,
		@ConfirmedBy,
		@ConfirmedDate,
		@Status
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_Performer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_Performer]
	@PerformerBy		INT,
	@WorkStreamId		VARCHAR(50)

AS

BEGIN
	INSERT INTO [kpi].[Performer]
	(
		[PerformerBy],
		[WorkStreamId]

	)
	VALUES
	(
		@PerformerBy,
		@WorkStreamId

	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_Performers]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [kpi].[Insert_Performers]
	@WorkStreamId				VARCHAR(50),
	@XML						NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/Performer'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [kpi].[Performer] WHERE  [WorkStreamId] = @WorkStreamId

	INSERT INTO [kpi].[Performer]
	(
		[PerformerBy],
		[WorkStreamId]

	)
	SELECT 
		[PerformerBy]		= x.PerformerBy,
		[WorkStreamId]		= @WorkStreamId


	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
			PerformerBy			INT,
			WorkStreamId		VARCHAR(50)
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_SuggesWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_SuggesWork]
	@SuggesWorkId			VARCHAR(50),
	@TaskId					VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Status					TINYINT,
	@CreateBy				INT,
	@CreateDate				DATETIME,
	@Description			NVARCHAR(500),
	@UsefulHours			DECIMAL(18,1),
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@VerifiedBy				INT,
	@VerifiedDate			DATETIME,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT
AS

BEGIN
	INSERT INTO [kpi].[SuggesWork]
	(
		[SuggesWorkId],
		[TaskId],
		[FromDate],
		[ToDate],
		[Status],
		[CreateBy],
		[CreateDate],
		[Description],
		[VerifiedBy],
		[VerifiedDate],
		[UsefulHours],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[FisnishDate],
		[WorkPointType],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[Quantity]
	)
	VALUES
	(
		@SuggesWorkId,
		@TaskId,
		@FromDate,
		@ToDate,
		@Status,
		@CreateBy,
		@CreateDate,
		@Description,
		@VerifiedBy,
		@VerifiedDate,
		@UsefulHours,
		@ApprovedFisnishBy,
		@ApprovedFisnishDate,
		@FisnishDate,
		@WorkPointType,
		@DepartmentFisnishBy,
		@DepartmentFisnishDate,
		@Quantity
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_Task]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [kpi].[Insert_Task]
	-- Add the parameters for the stored procedure here
	@TaskId					VARCHAR(50),
	@TaskCode				VARCHAR(50),
	@TaskName				NVARCHAR(255),
	@CalcType				TINYINT,
	@WorkPointConfigId		INT,
	@UsefulHours			DECIMAL(18,1),
	@Frequent				BIT,
	@Description			NVARCHAR(500),
	@IsSystem				BIT,
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@GroupName				NVARCHAR(500),
	@CategoryKpiId			INT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	IF NOT EXISTS (SELECT TaskId FROM [kpi].[Task] WHERE [TaskCode] = @TaskCode)
		BEGIN
			INSERT INTO [kpi].[Task]
			( 
				[TaskId],
				[TaskCode],
				[TaskName],
				[CalcType],
				[WorkPointConfigId],
				[UsefulHours],
				[Frequent],
				[Description],
				[IsSystem],
				[CreateDate],
				[CreateBy],
				[GroupName],
				[CategoryKpiId]
			 )
			 VALUES
			 (  
				@TaskId,
				@TaskCode,
				@TaskName,
				@CalcType,
				@WorkPointConfigId,
				@UsefulHours,
				@Frequent,
				@Description,
				@IsSystem,
				@CreateDate,
				@CreateBy,
				@GroupName,
				@CategoryKpiId
			 )
		END
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_WorkPlan]
	@WorkPlanId			VARCHAR(50),
	@WorkPlanCode		VARCHAR(50) OUTPUT,
	@CreateBy			INT,
	@FromDate			DATETIME,
	@ToDate				DATETIME,
	@CreateDate			DATETIME,
	@Description		NVARCHAR(500),
	@ConfirmedBy		INT,
	@ApprovedBy			INT,
	@ConfirmedDate		DATETIME,
	@ApprovedDate		DATETIME

AS

BEGIN
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @WorkPlanCode,@WorkPlanCode  OUTPUT 
	SELECT @WorkPlanCode
	INSERT INTO [kpi].[WorkPlan]
	(
		[WorkPlanId],
		[WorkPlanCode],
		[CreateBy],
		[FromDate],
		[ToDate],
		[CreateDate],
		[Description],
		[ConfirmedBy],
		[ApprovedBy],
		[ConfirmedDate],
		[ApprovedDate]
	)
	VALUES
	(
		@WorkPlanId,
		@WorkPlanCode,
		@CreateBy,
		@FromDate,
		@ToDate,
		@CreateDate,
		@Description,
		@ConfirmedBy,
		@ApprovedBy,
		@ConfirmedDate,
		@ApprovedDate
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkPlanDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Insert_WorkPlanDetail]
	@WorkPlanDetailId		VARCHAR(50),
	@TaskId					VARCHAR(50),
	@WorkPlanId				VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Status					INT,
	@Description			NVARCHAR(500),
	@Explanation			NVARCHAR(MAX),
	@UsefulHours			DECIMAL(18,1),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@Quantity				INT
AS

BEGIN
	INSERT INTO [kpi].[WorkPlanDetail]
	(
		[WorkPlanDetailId],
		[TaskId],
		[WorkPlanId],
		[FromDate],
		[ToDate],
		[Status],
		[Description],
		[Explanation],
		[UsefulHours],
		[WorkingNote],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[FisnishDate],
		[WorkPointType],
		[Quantity]
	)
	VALUES
	(
		@WorkPlanDetailId,
		@TaskId,
		@WorkPlanId,
		@FromDate,
		@ToDate,
		@Status,
		@Description,
		@Explanation,
		@UsefulHours,
		@WorkingNote,
		@ApprovedFisnishBy,
		@ApprovedFisnishDate,
		@FisnishDate,
		@WorkPointType,
		@Quantity
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkPlanDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [kpi].[Insert_WorkPlanDetails]
	@WorkPlanId				VARCHAR(50),
	@XML					NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/WorkPlanDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [kpi].[WorkPlanDetail] WHERE  [WorkPlanId] = @WorkPlanId

	INSERT INTO [kpi].[WorkPlanDetail]
	(
		[WorkPlanDetailId],
		[TaskId],
		[WorkPlanId],
		[FromDate],
		[ToDate],
		[Status],
		[Description],
		[Explanation],
		[WorkingNote],
		[UsefulHours],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[FisnishDate],
		[WorkPointType],
		[WorkPoint],
		[Quantity],
		[FileConfirm]
	)
	SELECT 
		[WorkPlanDetailId]		= x.WorkPlanDetailId,
		[TaskId]				= x.TaskId,
		[WorkPlanId]			= @WorkPlanId,
		[FromDate]				= x.FromDate,
		[ToDate]				= x.ToDate,
		[Status]				= x.Status,
		[Description]			= x.Description,
		[Explanation]			= x.Explanation,
		[WorkingNote]			= x.WorkingNote,
		[UsefulHours]			= CASE WHEN x.UsefulHours IS NULL OR x.UsefulHours = '' THEN NULL ELSE CAST(x.UsefulHours AS DECIMAL(18,1)) END,
		[ApprovedFisnishBy]		= CASE WHEN x.[ApprovedFisnishBy] IS NULL OR x.[ApprovedFisnishBy] = '' THEN NULL ELSE CAST(x.[ApprovedFisnishBy] AS INT) END,
		[ApprovedFisnishDate]	= CASE WHEN x.[ApprovedFisnishDate] IS NULL OR x.[ApprovedFisnishDate] = '' THEN NULL ELSE CAST(x.[ApprovedFisnishDate] AS DATETIME) END,
		[DepartmentFisnishBy]	= CASE WHEN x.[DepartmentFisnishBy] IS NULL OR x.[DepartmentFisnishBy] = '' THEN NULL ELSE CAST(x.[DepartmentFisnishBy] AS INT) END,
		[DepartmentFisnishDate]	= CASE WHEN x.[DepartmentFisnishDate] IS NULL OR x.[DepartmentFisnishDate] = '' THEN NULL ELSE CAST(x.[DepartmentFisnishDate] AS DATETIME) END,
		[FisnishDate]			= CASE WHEN x.[FisnishDate] IS NULL OR x.[FisnishDate] = '' THEN NULL ELSE CAST(x.[FisnishDate] AS DATETIME) END,
		[WorkPointType]			= x.WorkPointType,
		[WorkPoint]				= CASE WHEN x.[WorkPoint] IS NULL OR x.[WorkPoint] = '' THEN NULL ELSE CAST(x.[WorkPoint] AS DECIMAL(18,2)) END,
		[Quantity]				= x.[Quantity],
		[FileConfirm]			= x.FileConfirm
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
			WorkPlanDetailId			VARCHAR(50),
			TaskId						VARCHAR(50),
	        WorkPlanId					VARCHAR(50) ,
			FromDate					DATETIME,
			ToDate						DATETIME,
			Status						INT,
			Description					NVARCHAR(500),
			Explanation					NVARCHAR(MAX),
			WorkingNote					NVARCHAR(MAX),
			UsefulHours					VARCHAR(50),
			ApprovedFisnishBy			VARCHAR(50),
			ApprovedFisnishDate			VARCHAR(50),
			DepartmentFisnishBy			VARCHAR(50),
			DepartmentFisnishDate		VARCHAR(50),
			FisnishDate					VARCHAR(50),
			WorkPointType				VARCHAR(1),
			WorkPoint					VARCHAR(50),
			Quantity					INT,
			FileConfirm				NVARCHAR(255)
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkStream]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_WorkStream]
	@WorkStreamId		VARCHAR(50),
	@WorkStreamCode		VARCHAR(50) OUTPUT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@FromDate			DATETIME,
	@ToDate				DATETIME,
	@AssignWorkId		VARCHAR(50),
	@TaskId				VARCHAR(50),
	@Description		NVARCHAR(500),
	@Status				TINYINT,
	@ApprovedBy			INT,
	@ApprovedDate		DATETIME
AS

BEGIN
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @WorkStreamCode,@WorkStreamCode  OUTPUT 
	SELECT @WorkStreamCode
	INSERT INTO [kpi].[WorkStream]
	(
		[WorkStreamId],
		[WorkStreamCode],
		[CreateBy],
		[CreateDate],
		[FromDate],
		[ToDate],
		[AssignWorkId],
		[TaskId],
		[Description],
		[Status]
	)
	VALUES
	(
		@WorkStreamId,
		@WorkStreamCode,
		@CreateBy,
		@CreateDate,
		@FromDate,
		@ToDate,
		@AssignWorkId,
		@TaskId,
		@Description,
		@Status
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkStreamDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Insert_WorkStreamDetail]
	@WorkStreamDetailId		VARCHAR(50),
	@TaskId					VARCHAR(50),
	@WorkStreamId			VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@CreateBy				INT,
	@CreateDate				DATETIME,
	@Status					INT,
	@Description			NVARCHAR(500),
	@IsDefault				BIT,
	@UsefulHours			DECIMAL(18,1),
	@VerifiedBy				INT,
	@VerifiedDate			DATETIME,
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@WorkPointType			VARCHAR(1),
	@Quantity				INT
AS

BEGIN
	INSERT INTO [kpi].[WorkStreamDetail]
	(
		[WorkStreamDetailId],
		[TaskId],
		[WorkStreamId],
		[FromDate],
		[ToDate],
		[CreateBy],
		[CreateDate],
		[Status],
		[Description],
		[IsDefault],
		[UsefulHours],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[VerifiedBy],
		[VerifiedDate],
		[FisnishDate],
		[WorkPointType],
		[Quantity]
	)
	VALUES
	(
		@WorkStreamDetailId,
		@TaskId,
		@WorkStreamId,
		@FromDate,
		@ToDate,
		@CreateBy,
		@CreateDate,
		@Status,
		@Description,
		@IsDefault,
		@UsefulHours,
		@VerifiedBy,
		@VerifiedDate,
		@ApprovedFisnishBy,
		@ApprovedFisnishDate,
		@FisnishDate,
		@WorkPointType,
		@Quantity
	)
END
GO
/****** Object:  StoredProcedure [kpi].[Insert_WorkStreamDetails]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [kpi].[Insert_WorkStreamDetails]
	@WorkStreamId				VARCHAR(50),
	@XML						NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/WorkStreamDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [kpi].[WorkStreamDetail] WHERE  [WorkStreamId] = @WorkStreamId AND [IsDefault] = 0

	INSERT INTO [kpi].[WorkStreamDetail]
	(
		[WorkStreamDetailId],
		[TaskId],
		[WorkStreamId],
		[FromDate],
		[ToDate],
		[CreateBy],
		[CreateDate],
		[Status],
		[Description],
		[IsDefault],
		[UsefulHours],
		[ApprovedFisnishBy],
		[ApprovedFisnishDate],
		[DepartmentFisnishBy],
		[DepartmentFisnishDate],
		[VerifiedBy],
		[VerifiedDate],
		[FisnishDate],
		[WorkPointType],
		[WorkPoint],
		[Quantity],
		[FileConfirm]
	)
	SELECT 
		[WorkStreamDetailId]	= x.WorkStreamDetailId,
		[TaskId]				= x.TaskId,
		[WorkStreamId]			= @WorkStreamId,
		[FromDate]				= x.FromDate,
		[ToDate]				= x.ToDate,
		[CreateBy]				= x.CreateBy,
		[CreateDate]			= x.CreateDate,
		[Status]				= x.Status,
		[Description]			= x.Description,
		[IsDefault]				=  x.IsDefault,
		[UsefulHours]			= CASE WHEN x.UsefulHours IS NULL OR x.UsefulHours = '' THEN NULL ELSE CAST(x.UsefulHours AS DECIMAL(18,1)) END,
		[ApprovedFisnishBy]		= CASE WHEN x.[ApprovedFisnishBy] IS NULL OR x.[ApprovedFisnishBy] = '' THEN NULL ELSE CAST(x.[ApprovedFisnishBy] AS INT) END,
		[ApprovedFisnishDate]	= CASE WHEN x.[ApprovedFisnishDate] IS NULL OR x.[ApprovedFisnishDate] = '' THEN NULL ELSE CAST(x.[ApprovedFisnishDate] AS DATETIME) END,
		[DepartmentFisnishBy]	= CASE WHEN x.[DepartmentFisnishBy] IS NULL OR x.[DepartmentFisnishBy] = '' THEN NULL ELSE CAST(x.[DepartmentFisnishBy] AS INT) END,
		[DepartmentFisnishDate]	= CASE WHEN x.[DepartmentFisnishDate] IS NULL OR x.[DepartmentFisnishDate] = '' THEN NULL ELSE CAST(x.[DepartmentFisnishDate] AS DATETIME) END,
		[VerifiedBy]			= CASE WHEN x.[VerifiedBy] IS NULL OR x.[VerifiedBy] = '' THEN NULL ELSE CAST(x.[VerifiedBy] AS INT) END,
		[VerifiedDate]			= CASE WHEN x.[VerifiedDate] IS NULL OR x.[VerifiedDate] = '' THEN NULL ELSE CAST(x.[VerifiedDate] AS DATETIME) END,
		[FisnishDate]			= CASE WHEN x.[FisnishDate] IS NULL OR x.[FisnishDate] = '' THEN NULL ELSE CAST(x.[FisnishDate] AS DATETIME) END,
		[WorkPointType]			= x.WorkPointType,
		[WorkPoint]				=  CASE WHEN x.[WorkPoint] IS NULL OR x.[WorkPoint] = '' THEN NULL ELSE CAST(x.[WorkPoint] AS DECIMAL(18,2)) END,
		[Quantity]				= x.[Quantity],
		[FileConfirm]			= X.[FileConfirm]
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
			WorkStreamDetailId		VARCHAR(50),
			TaskId					VARCHAR(50),
			WorkStreamId			VARCHAR(50),
			FromDate				DATETIME,
			ToDate					DATETIME,
			CreateBy				INT,
			CreateDate				DATETIME,
			Status					INT,
			Description				NVARCHAR(500),
			IsDefault				BIT,
			UsefulHours				VARCHAR(50),
			ApprovedFisnishBy		VARCHAR(50),
			ApprovedFisnishDate		VARCHAR(50),
			DepartmentFisnishBy		VARCHAR(50),
			DepartmentFisnishDate	VARCHAR(50),
			VerifiedBy				VARCHAR(50),
			VerifiedDate			VARCHAR(50),
			FisnishDate				VARCHAR(50),
			WorkPointType			VARCHAR(1),
			WorkPoint				VARCHAR(50),
			Quantity				INT,
			FileConfirm				NVARCHAR(255)
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_AcceptConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_AcceptConfig]
	@AcceptConfigId			INT,
	@AcceptType				VARCHAR(50),
	@AcceptPointMin			DECIMAL(18,2),
	@AcceptPointMax			DECIMAL(18,2),
	@AcceptConditionMin		DECIMAL(18,1),
	@AcceptConditionMax		DECIMAL(18,1)
AS

BEGIN
	UPDATE [kpi].[AcceptConfig]
	SET
		[AcceptType]			= @AcceptType,
		[AcceptPointMin]		= @AcceptPointMin,
		[AcceptPointMax]		= @AcceptPointMax,
		[AcceptConditionMin]	= @AcceptConditionMin,
		[AcceptConditionMax]	= @AcceptConditionMax
	WHERE [AcceptConfigId]		= @AcceptConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_AssignWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_AssignWork]
	@AssignWorkId			VARCHAR(50),
	@TaskId					VARCHAR(50),
	@CreateBy				INT,
	@AssignBy				INT,
	@CreateDate				DATETIME,
	@FromDate				DATETIME,
	@Status					TINYINT,
	@ToDate					DATETIME,
	@Description			NVARCHAR(500),
	@UsefulHours			DECIMAL(18,1),
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDatE	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@WorkPoint				DECIMAL(18,2),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT,
	@FileConfirm			NVARCHAR(255),
	@DepartmentFollowBy		INT,
	@DirectorFollowBy		INT,
	@TypeAssignWork			INT
AS

BEGIN
	UPDATE [kpi].[AssignWork]
	SET	
		[TaskId]				= @TaskId,
		[CreateBy]				= @CreateBy,
		[AssignBy]				= @AssignBy,
		[CreateDate]			= @CreateDate,
		[FromDate]				= @FromDate,
		[Status]				= @Status,
		[ToDate]				= @ToDate,
		[Description]			= @Description,
		[UsefulHours]			= @UsefulHours,
		[Explanation]			= @Explanation,
		[WorkingNote]			= @WorkingNote,
		[ApprovedFisnishBy]		= @ApprovedFisnishBy,
		[ApprovedFisnishDate]	= @ApprovedFisnishDate,
		[FisnishDate]			= @FisnishDate,
		[WorkPointType]			= @WorkPointType,
		[WorkPoint]				= @WorkPoint,
		[DepartmentFisnishBy]	= @DepartmentFisnishBy,
		[DepartmentFisnishDate] = @DepartmentFisnishDate,
		[Quantity]				= @Quantity,
		[FileConfirm]			= @FileConfirm,
		[DepartmentFollowBy]	= @DepartmentFollowBy,
		[DirectorFollowBy]		= @DirectorFollowBy,
		[TypeAssignWork]		= @TypeAssignWork
	WHERE [AssignWorkId]	= @AssignWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_Complain]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_Complain]
	@ComplainId			VARCHAR(50),
	@CreateBy			INT,
	@AccusedBy			INT,
	@Description		NVARCHAR(MAX),
	@CreateDate			DATETIME,
	@ConfirmedBy		INT,
	@ConfirmedDate		DATETIME,
	@Status				TINYINT
AS

BEGIN
	UPDATE [kpi].[Complain]
	SET
		[AccusedBy]		= @AccusedBy,
		[Description]	= @Description,
		[ConfirmedBy]	= @ConfirmedBy,
		[ConfirmedDate]	= @ConfirmedDate,
		[Status]		= @Status
	WHERE [ComplainId]	= @ComplainId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_FactorConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_FactorConfig]
	@FactorConfigId			INT,
	@FactorType				VARCHAR(50),
	@FactorPointMin			DECIMAL(18,2),
	@FactorPointMax			DECIMAL(18,2),
	@FactorConditionMin		DECIMAL(18,2),
	@FactorConditionMax		DECIMAL(18,2)
AS

BEGIN
	UPDATE [kpi].[FactorConfig]
	SET
		[FactorType]			= @FactorType,
		[FactorPointMin]		= @FactorPointMin,
		[FactorPointMax]		= @FactorPointMax,
		[FactorConditionMin]	= @FactorConditionMin,
		[FactorConditionMax]	= @FactorConditionMax
	WHERE [FactorConfigId]		= @FactorConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_FinishJobConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Update_FinishJobConfig]
	@FinishConfigId			INT,
	@FinishType				VARCHAR(50),
	@FinishPointMin			DECIMAL(18,2),
	@FinishPointMax			DECIMAL(18,2),
	@FinishConditionMin		DECIMAL(18,2),
	@FinishConditionMax		DECIMAL(18,2)
AS

BEGIN
	UPDATE [kpi].[FinishJobConfig]
	SET
		[FinishType]			= @FinishType,
		[FinishPointMin]		= @FinishPointMin,
		[FinishPointMax]		= @FinishPointMax,
		[FinishConditionMin]	= @FinishConditionMin,
		[FinishConditionMax]	= @FinishConditionMax
	WHERE [FinishConfigId]		= @FinishConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_KpiConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Update_KpiConfig]
	@KpiConfigId		INT,
	@MinHours			DECIMAL(18,1),
	@MaxHours			DECIMAL(18,1),
	@PlanningDay		TINYINT,
	@PlanningHourMax	VARCHAR(50),
	@PlanningHourMin	VARCHAR(50),
	@HourConfirmMax		VARCHAR(50),
	@HourConfirmMin		VARCHAR(50),
	@Notification		DECIMAL(18,3)
AS

BEGIN
	UPDATE [kpi].[KpiConfig]
	SET
		[MinHours]			= @MinHours,
		[MaxHours]			= @MaxHours,
		[PlanningDay]		= @PlanningDay,
		[PlanningHourMax]	= @PlanningHourMax,
		[PlanningHourMin]	= @PlanningHourMin,
		[HourConfirmMax]	= @HourConfirmMax,
		[HourConfirmMin]	= @HourConfirmMin,
		[Notification]		= @Notification

	WHERE [KpiConfigId] = @KpiConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_SuggestJobConfig]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Update_SuggestJobConfig]
	@JobConfigId			INT,
	@JobType				VARCHAR(50),
	@JobPointMin			DECIMAL(18,2),
	@JobPointMax			DECIMAL(18,2),
	@JobConditionMin		DECIMAL(18,1),
	@JobConditionMax		DECIMAL(18,1)
AS

BEGIN
	UPDATE [kpi].[SuggestJobConfig]
	SET
		[JobType]			= @JobType,
		[JobPointMin]		= @JobPointMin,
		[JobPointMax]		= @JobPointMax,
		[JobConditionMin]	= @JobConditionMin,
		[JobConditionMax]	= @JobConditionMax
	WHERE [JobConfigId]		= @JobConfigId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_SuggesWork]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_SuggesWork]
	@SuggesWorkId			VARCHAR(50),
	@TaskId					VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Status					TINYINT,
	@CreateBy				INT,
	@CreateDate				DATETIME,
	@Description			NVARCHAR(500),
	@UsefulHours			DECIMAL(18,1),
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@WorkPoint				DECIMAL(18,2),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT,
	@FileConfirm			NVARCHAR(255)
AS

BEGIN
	UPDATE [kpi].[SuggesWork]
	SET
		[TaskId]				= @TaskId,
		[FromDate]				= @FromDate,
		[ToDate]				= @ToDate,
		[Status]				= @Status,
		[Description]			= @Description,
		[ApprovedFisnishBy]		= @ApprovedFisnishBy,
		[ApprovedFisnishDate]	= @ApprovedFisnishDate,
		[FisnishDate]			= @FisnishDate,
		[UsefulHours]			= @UsefulHours,
		[Explanation]			= @Explanation,
		[WorkingNote]			= @WorkingNote,
		[WorkPointType]			= @WorkPointType,
		[WorkPoint]				= @WorkPoint,
		[DepartmentFisnishBy]	= @DepartmentFisnishBy,
		[DepartmentFisnishDate] = @DepartmentFisnishDate,
		[Quantity]				= @Quantity,
		[FileConfirm]			= @FileConfirm
	WHERE [SuggesWorkId] = @SuggesWorkId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_Task]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [kpi].[Update_Task]
	@TaskId					VARCHAR(50),
	@TaskCode				VARCHAR(50),
	@TaskName				NVARCHAR(255),
	@CalcType				TINYINT,
	@WorkPointConfigId		INT,
	@UsefulHours			DECIMAL(18,1),
	@Frequent				BIT,
	@Description			NVARCHAR(500),
	@IsSystem				BIT,
	@CreateDate				DATETIME,
	@CreateBy				INT,
	@GroupName				NVARCHAR(500),
	@CategoryKpiId			INT

AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [kpi].[Task]
	SET
		[TaskCode]				= @TaskCode,
		[TaskName]				= @TaskName,
		[CalcType]				= @CalcType,
		[WorkPointConfigId]		= @WorkPointConfigId,
		[UsefulHours]			= @UsefulHours,
		[Frequent]				= @Frequent,
		[Description]			= @Description,
		[GroupName]				= @GroupName,
		[CategoryKpiId]			= @CategoryKpiId
	WHERE [TaskId]				= @TaskId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_WorkPlan]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_WorkPlan]
	@WorkPlanId			VARCHAR(50),
	@WorkPlanCode		VARCHAR(50) OUTPUT,
	@CreateBy			INT,
	@FromDate			DATETIME,
	@ToDate				DATETIME,
	@CreateDate			DATETIME,
	@Description		NVARCHAR(500)
AS

BEGIN
	UPDATE [kpi].[WorkPlan]
	SET
		[WorkPlanCode]		= @WorkPlanCode,
		[FromDate]			= @FromDate,
		[ToDate]			= @ToDate,
		[Description]		= @Description
	WHERE		[WorkPlanId] = @WorkPlanId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_WorkPlanDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_WorkPlanDetail]
	@WorkPlanDetailId		VARCHAR(50),
	@TaskId					VARCHAR(50),
	@WorkPlanId				VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@Status					INT,
	@Description			NVARCHAR(500),
	@Explanation			NVARCHAR(MAX),
	@UsefulHours			DECIMAL(18,1),
	@WorkingNote			NVARCHAR(MAX),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@WorkPointType			VARCHAR(1),
	@WorkPoint				DECIMAL(18,2),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT,
	@FileConfirm			NVARCHAR(255)
AS

BEGIN
	UPDATE [kpi].[WorkPlanDetail]
	SET
		[TaskId]				= @TaskId,
		[WorkPlanId]			= @WorkPlanId,
		[FromDate]				= @FromDate,
		[ToDate]				= @ToDate,
		[Status]				= @Status,
		[Description]			= @Description,
		[Explanation]			= @Explanation,
		[UsefulHours]			= @UsefulHours,
		[WorkingNote]			= @WorkingNote,
		[ApprovedFisnishBy]		= @ApprovedFisnishBy,
		[ApprovedFisnishDate]	= @ApprovedFisnishDate,
		[FisnishDate]			= @FisnishDate,
		[WorkPointType]			= @WorkPointType,
		[WorkPoint]				= @WorkPoint,
		[DepartmentFisnishBy]	= @DepartmentFisnishBy,
		[DepartmentFisnishDate] = @DepartmentFisnishDate,
		[Quantity]				= @Quantity,
		[FileConfirm]			= @FileConfirm
	WHERE [WorkPlanDetailId]	= @WorkPlanDetailId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_WorkStream]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_WorkStream]
	@WorkStreamId		VARCHAR(50),
	@WorkStreamCode		NVARCHAR(50) OUTPUT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@FromDate			DATETIME,
	@ToDate				DATETIME,
	@AssignWorkId		VARCHAR(50),
	@TaskId				VARCHAR(50),
	@Description		NVARCHAR(500),
	@Status				TINYINT,
	@ApprovedBy			INT,
	@ApprovedDate		DATETIME

AS

BEGIN
	UPDATE [kpi].[WorkStream]
	SET
		[FromDate]			= @FromDate,
		[ToDate]			= @ToDate,
		[AssignWorkId]		= @AssignWorkId,
		[TaskId]			= @TaskId,
		[Description]		= @Description,
		[Status]			= @Status,
		[ApprovedBy]		= @ApprovedBy,
		[ApprovedDate]		= @ApprovedDate
	WHERE [WorkStreamId]		= @WorkStreamId
END
GO
/****** Object:  StoredProcedure [kpi].[Update_WorkStreamDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [kpi].[Update_WorkStreamDetail]
	@WorkStreamDetailId		VARCHAR(50),
	@TaskId					VARCHAR(50),
	@WorkStreamId			VARCHAR(50),
	@FromDate				DATETIME,
	@ToDate					DATETIME,
	@CreateBy				INT,
	@CreateDate				DATETIME,
	@Status					INT,
	@Despcription			NVARCHAR(500),
	@IsDefault				BIT,
	@UsefulHours			DECIMAL(18,1),
	@ApprovedFisnishBy		INT,
	@ApprovedFisnishDate	DATETIME,
	@FisnishDate			DATETIME,
	@Explanation			NVARCHAR(MAX),
	@WorkingNote			NVARCHAR(MAX),
	@WorkPointType			VARCHAR(1),
	@WorkPoint				DECIMAL(18,2),
	@DepartmentFisnishBy	INT,
	@DepartmentFisnishDate	DATETIME,
	@Quantity				INT,
	@FileConfirm			NVARCHAR(255)
AS

BEGIN
	UPDATE [kpi].[WorkStreamDetail]
	SET
		[TaskId]				= @TaskId,
		[FromDate]				= @FromDate,
		[ToDate]				= @ToDate,
		[Status]				= @Status,
		[Description]			= @Despcription,
		[IsDefault]				= @IsDefault,
		[UsefulHours]			= @UsefulHours,		
		[Explanation]			= @Explanation,
		[WorkingNote]			= @WorkingNote,
		[ApprovedFisnishBy]		= @ApprovedFisnishBy,
		[ApprovedFisnishDate]	= @ApprovedFisnishDate,
		[FisnishDate]			= @FisnishDate,
		[WorkPointType]			= @WorkPointType,
		[WorkPoint]				= @WorkPoint,
		[DepartmentFisnishBy]	= @DepartmentFisnishBy,
		[DepartmentFisnishDate] = @DepartmentFisnishDate,
		[Quantity]				= @Quantity,
		[FileConfirm]			= @FileConfirm
	WHERE [WorkStreamDetailId]	= @WorkStreamDetailId
END
GO
/****** Object:  StoredProcedure [sale].[Delete_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Delete_Contract]
	@ContractId		BIGINT
AS

BEGIN
	DELETE [sale].[Contract]	WHERE [ContractId]	= @ContractId
END
GO
/****** Object:  StoredProcedure [sale].[Delete_Customer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Delete_Customer]
	@CustomerId		BIGINT
AS

BEGIN
	DELETE [sale].[Customer]	WHERE [CustomerId]	= @CustomerId
END
GO
/****** Object:  StoredProcedure [sale].[Delete_Investor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Delete_Investor]
	@InvestorId		VARCHAR(50)
AS

BEGIN
	DELETE [sale].[Investor]	WHERE [InvestorId]	= @InvestorId
END
GO
/****** Object:  StoredProcedure [sale].[Delete_Product]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Delete_Product]
	@ProductId		BIGINT
AS

BEGIN
	DELETE [sale].[Product]	WHERE [ProductId]	= @ProductId
END
GO
/****** Object:  StoredProcedure [sale].[Delete_Project]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Delete_Project]
	@ProjectId		VARCHAR(50)
AS

BEGIN
	DELETE [sale].[Project]	WHERE [ProjectId]	= @ProjectId
END
GO
/****** Object:  StoredProcedure [sale].[Detele_ContractDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Detele_ContractDetail]
	@ContractDetailId		VARCHAR(50)
AS

BEGIN
	DELETE [sale].[ContractDetail]	WHERE [ContractDetailId]	= @ContractDetailId
END
GO
/****** Object:  StoredProcedure [sale].[Get_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Get_Contract]
	@ContractId		BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		[ContractId]		= ct.[ContractId],
		[ContractCode]		= ct.[ContractCode],
		[CustomerId]		= ct.[CustomerId],
		[EmployeeId]		= ct.[EmployeeId],
		[CreateBy]			= ct.[CreateBy],				
		[CreateDate]		= ct.[CreateDate],
		[TotalPrice]		= ct.[TotalPrice],
		[Description]		= ct.[Description],
		[Status]			= ct.[Status],
		[ContractNumber]	= ct.[ContractNumber],
		[CustomerName]		= a.[FullName],
		[CustomerCode]		= a.[CustomerCode],
		[EmployeeCode]		= b.[EmployeeCode],
		[EmployeeName]		= b.[FullName]
	FROM [sale].[Contract] ct
	INNER JOIN [sale].[Customer] a ON a.[CustomerId]	= ct.[CustomerId]
	LEFT JOIN [hrm].[Employee] b ON b.[EmployeeId]		= ct.[EmployeeId]
	WHERE ct.[ContractId] = @ContractId

	SELECT
		[ContractDetailId]		= CAST(cd.[ContractDetailId] AS VARCHAR(50)),
		[ContractId]			= cd.[ContractId],
		[ProductId]				= cd.[ProductId],
		[Quantity]				= cd.[Quantity],
		[Price]					= p.[Price],
		[ProductName]			= p.[ProductName]
	FROM [sale].[ContractDetail] cd
	INNER JOIN [sale].[Product] p ON p.[ProductId] = cd.[ProductId]
	WHERE [ContractId] = @ContractId
END
GO
/****** Object:  StoredProcedure [sale].[Get_ContractDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Get_ContractDetail]
		@ContractDetailId VARCHAR(50)
AS
BEGIN
	SELECT
		[ContractDetailId]			= CAST([ContractDetailId] AS VARCHAR(50)),
		[ContractId],
		[ProductId],
		[Quantity]
	FROM [sale].[ContractDetail]
	WHERE [ContractDetailId] = @ContractDetailId
END
GO
/****** Object:  StoredProcedure [sale].[Get_Contracts]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Get_Contracts]
	@FromDate	DATE,
	@ToDate		DATE,
	@Status		TINYINT
AS
BEGIN
	SELECT
		[ContractId],
		[ContractCode],
		[CustomerId],
		[EmployeeId],
		[CreateBy],
		[CreateDate],
		[TotalPrice],
		[Description],
		[Status],
		[ContractNumber]
	FROM [sale].[Contract]
	WHERE
			CAST([CreateDate] AS DATE) BETWEEN @FromDate AND @ToDate
		AND
			[Status]	=	ISNULL(@Status,[Status])
	ORDER BY [CreateDate] DESC		

END
GO
/****** Object:  StoredProcedure [sale].[Get_ContractsBySell]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Get_ContractsBySell]
AS
BEGIN
SELECT
	ProductId		= p.ProductId,
	ProductCode		= p.ProductCode,
	ProductName		= p.ProductName,
	Price			= p.Price,
	TotalQuantity	= SUM(p.BuyQuantity)
FROM (
	SELECT
		p.*,
		BuyQuantity = cd.Quantity
	FROM [sale].[Product] p
	INNER JOIN [sale].[ContractDetail] cd ON cd.[ProductId] = p.[ProductId]
) AS p
GROUP BY p.ProductId,p.ProductCode,p.ProductName,p.Price
END
GO
/****** Object:  StoredProcedure [sale].[Get_Customer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Get_Customer]
	@CustomerId		BIGINT
AS
BEGIN
	SELECT
		[CustomerId],
		[CustomerCode],
		[FullName],
		[IdentityCardDate],
		[CityIdentityCard],
		[IdentityCard],
		[Email],
		[PhoneNumber],
		[Description],
		[Address],
		[CityId],
		[DistrictId],
		[TaxCode],
		[CompanyName],
		[BankAccountNumber],
		[Status],
		[CreateDate],
		[CreateBy]
	FROM	[sale].[Customer]
	WHERE	[CustomerId]	= @CustomerId
END
GO
/****** Object:  StoredProcedure [sale].[Get_Customers]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sale].[Get_Customers]
	-- Add the parameters for the stored procedure here
	@Keyword	NVARCHAR(255),
	@Status		TINYINT
AS
BEGIN
	SET @Keyword = ISNULL(@Keyword,'')
	SET NOCOUNT ON;
	SELECT
		[CustomerId],
		[CustomerCode],
		[Fullname],
		[IdentityCardDate],
		[CityIdentityCard],
		[IdentityCard],
		[Email],
		[PhoneNumber],
		[Description],
		[CityId],
		[Address],
		[DistrictId],
		[TaxCode],
		[CompanyName],
		[BankAccountNumber],
		[Status],
		[CreateDate],
		[CreateBy]
	FROM [sale].[Customer]
	WHERE 
			[Status] = ISNULL(@Status,[Status])
		AND
		(
				[CustomerCode] LIKE '%' + @Keyword + '%'
			OR
				[Fullname] LIKE '%' + @Keyword + '%'
			OR
				[Email] LIKE '%' + @Keyword + '%'
			OR
				[PhoneNumber] LIKE '%' + @Keyword + '%'
		)
	ORDER BY [Fullname] ASC
END
GO
/****** Object:  StoredProcedure [sale].[Get_Investor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Get_Investor]
	@InvestorId   VARCHAR(50)
AS
BEGIN
	SELECT
		[InvestorId]		= CAST([InvestorId] AS VARCHAR(50)),
		[InvestorCode],
		[FullName],
		[Company],
		[CompanyAddress],
		[Address],
		[CityId],
		[DistrictId],
		[Position],
		[MsEnterprise],
		[FoundedYear],
		[CharterCapital],
		[Status],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [sale].[Investor] i
	WHERE i.[InvestorId] = @InvestorId


	SELECT
		[EmployeeInvestorId]		= CAST([EmployeeInvestorId] AS VARCHAR(50)),
		[FullName],
		[SpecialName],
		[TrainingLevelId],
		[MaritalStatus],
		[DateOfBirth],
		[Gender],
		[EducationLevelId],
		[Position],
		[Phone],
		[IdentityCardNumber],
		[Status],
		[Description],
		[Address],
		[CityId],
		[DistrictId],
		[Skill],
		[Experience],
		[Email],
		[InvestorId]				= CAST([InvestorId] AS VARCHAR(50))
	FROM [sale].[EmployeeInvestor] ei
	WHERE ei.[InvestorId] = @InvestorId
END
GO
/****** Object:  StoredProcedure [sale].[Get_Investors]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Get_Investors]
AS
BEGIN
	SELECT
		[InvestorId]		= CAST([InvestorId] AS VARCHAR(50)),
		[InvestorCode],
		[FullName],
		[Company],
		[CompanyAddress],
		[Address],
		[CityId],
		[DistrictId],
		[Position],
		[MsEnterprise],
		[FoundedYear],
		[CharterCapital],
		[Status],
		[CreateBy],
		[CreateDate],
		[Description]
	FROM [sale].[Investor]
END
GO
/****** Object:  StoredProcedure [sale].[Get_Product]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Get_Product]
	@ProductId	BIGINT
AS
BEGIN
	SELECT
		[ProductId],
		[ProductCode],
		[ProductName],
		[Price],
		[Quantity],
		[IsActive],
		[Description],
		[CreateDate],
		[CreateBy]
	FROM [sale].[Product]
	WHERE [ProductId] = @ProductId
END
GO
/****** Object:  StoredProcedure [sale].[Get_Products]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author],],Name>
-- Create date: <Create Date],],>
-- Description:	<Description],],>
-- =============================================
CREATE PROCEDURE [sale].[Get_Products]
	-- Add the parameters for the stored procedure here
	@IsActive	BIT,
	@Keyword	NVARCHAR(255)
AS
BEGIN
	SET @Keyword = ISNULL(@Keyword,'')
	SET NOCOUNT ON;
	SELECT
		[ProductId],
		[ProductCode],
		[ProductName],
		[Price],
		[Quantity],
		[IsActive],
		[Description],
		[CreateDate],
		[CreateBy]
	FROM [sale].[Product]
	WHERE 
			[IsActive] = ISNULL(@IsActive,[IsActive])
		AND
		(
				[ProductCode] LIKE '%' + @Keyword + '%'
			OR
				[ProductName] LIKE '%' + @Keyword + '%'
		) 
	ORDER BY [ProductName] ASC
END
GO
/****** Object:  StoredProcedure [sale].[Get_Project]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Get_Project]
AS
BEGIN
	SELECT
		[ProjectId]		= CAST([ProjectId] AS VARCHAR(50)),
		[ProjectCode],
		[FullName],
		[Status],
		[InvestorId],
		[FromDate],
		[CreateDate],
		[CreateBy],
		[ToDate],
		[Description]
	FROM [sale].[Project]
END
GO
/****** Object:  StoredProcedure [sale].[Get_Projects]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Get_Projects]
AS
BEGIN
	SELECT
		[ProjectId]			= CAST([ProjectId] AS VARCHAR(50)),
		[ProjectCode],
		[FullName],
		[Status],
		[InvestorId]		= CAST([InvestorId] AS VARCHAR(50)),
		[FromDate],
		[CreateDate],
		[CreateBy],
		[ToDate],
		[Description]
	FROM [sale].[Project]
END
GO
/****** Object:  StoredProcedure [sale].[Insert_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Insert_Contract]
	@ContractId			BIGINT OUTPUT,
	@ContractCode		VARCHAR(50) OUTPUT,
	@CustomerId			BIGINT,
	@EmployeeId			INT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@TotalPrice			DECIMAL(18),
	@Description		NVARCHAR(500),
	@Status				INT,
	@ContractNumber		VARCHAR(50)
AS


BEGIN
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @ContractCode,@ContractCode  OUTPUT 
	SELECT @ContractCode
	INSERT INTO [sale].[Contract]
	(
		[ContractCode],
		[CustomerId],
		[EmployeeId],
		[CreateBy],
		[CreateDate],
		[TotalPrice],
		[Description],
		[Status],
		[ContractNumber]
	)
	VALUES
	(
		@ContractCode,
		@CustomerId,
		@EmployeeId,
		@CreateBy,
		@CreateDate,
		@TotalPrice,
		@Description,
		@Status,
		@ContractNumber
	)
	SET @ContractId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [sale].[Insert_ContractDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sale].[Insert_ContractDetail]
	@ContractId				INT,
	@XML					NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @XmlId           INT,
	        @XmlRootName     VARCHAR(100)
	
	SET @Xml = dbo.ufn_Replace_XmlChars(@Xml)
	SET @XmlRootName = dbo.ufn_Get_Root_Element_Name(@Xml) +'/ContractDetail'
	
	EXEC sp_xml_preparedocument @XmlId OUT, @Xml

	DELETE [sale].[ContractDetail] WHERE  [ContractId] = @ContractId

	INSERT INTO [sale].[ContractDetail]
	(
		[ContractDetailId],
		[ContractId],
		[ProductId],
		[Quantity]
	)
	SELECT 
		[ContractDetailId]	= x.ContractDetailId,
		[ContractId]		= @ContractId,
		[ProductId]			= x.ProductId,
		[Quantity]			= x.Quantity
	FROM OPENXML(@XmlId, @XmlRootName, 2)
	WITH ( 
			ContractDetailId	VARCHAR(50),
	        ContractId			BIGINT ,
			ProductId			BIGINT ,
			Quantity			INT
	     ) x
	EXEC sp_xml_removedocument @XmlId
END
GO
/****** Object:  StoredProcedure [sale].[Insert_Customer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Insert_Customer]
	@CustomerId				BIGINT OUTPUT,
	@CustomerCode			VARCHAR(50) OUTPUT,
	@FullName				NVARCHAR(255),
	@IdentityCardDate		DATE,
	@CityIdentityCard		INT,
	@IdentityCard			VARCHAR(50),
	@Email					NVARCHAR(255),
	@PhoneNumber			VARCHAR(50),
	@Description			NVARCHAR(500),
	@Address				NVARCHAR(255),
	@CityId					INT,
	@DistrictId				INT,
	@TaxCode				VARCHAR(50),
	@CompanyName			VARCHAR(255),
	@BankAccountNumber		VARCHAR(50),
	@Status					TINYINT,
	@CreateDate				DATETIME,
	@CreateBy				INT
AS


BEGIN
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @CustomerCode,@CustomerCode  OUTPUT 
	SELECT @CustomerCode
	INSERT INTO [sale].[Customer]
	(
		[CustomerCode],
		[FullName],
		[IdentityCardDate],
		[CityIdentityCard],
		[IdentityCard],
		[Email],
		[PhoneNumber],
		[Description],
		[Address],
		[CityId],
		[DistrictId],
		[TaxCode],
		[CompanyName],
		[BankAccountNumber],
		[Status],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@CustomerCode,
		@FullName,
		@IdentityCardDate,
		@CityIdentityCard,
		@IdentityCard,
		@Email,
		@PhoneNumber,
		@Description,
		@Address,
		@CityId,
		@DistrictId,
		@TaxCode,
		@CompanyName,
		@BankAccountNumber,
		@Status,
		@CreateDate,
		@CreateBy
	)
		SET @CustomerId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [sale].[Insert_Investor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Insert_Investor]
	@InvestorId			VARCHAR(50),
	@InvestorCode		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Company			NVARCHAR(255),
	@CompanyAddress		NVARCHAR(500),
	@Address			NVARCHAR(500),
	@CityId				INT,
	@DistrictId			INT,
	@Position			VARCHAR(255),
	@MsEnterprise		VARCHAR(50),
	@FoundedYear		INT,
	@CharterCapital		DECIMAL(18,2),
	@Status				INT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@Description		NVARCHAR(500)
AS
BEGIN
	INSERT INTO [sale].[Investor]
	(
		[InvestorId],
		[InvestorCode],
		[FullName],
		[Company],
		[CompanyAddress],
		[Address],
		[CityId],
		[DistrictId],
		[Position],
		[MsEnterprise],
		[FoundedYear],
		[CharterCapital],
		[Status],
		[CreateBy],
		[CreateDate],
		[Description]
	)
	VALUES
	(
		@InvestorId,
		@InvestorCode,
		@FullName,
		@Company,
		@CompanyAddress,
		@Address,
		@CityId,
		@DistrictId,
		@Position,
		@MsEnterprise,
		@FoundedYear,
		@CharterCapital,
		@Status,
		@CreateBy,
		@CreateDate,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [sale].[Insert_Product]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Insert_Product]
	@ProductId			BIGINT OUTPUT,
	@ProductCode		VARCHAR(50) OUTPUT,
	@ProductName		NVARCHAR(255),
	@Price				MONEY,
	@Quantity			INT,
	@IsActive			BIT,
	@Description		NVARCHAR(500),
	@CreateDate			DATETIME,
	@CreateBy			INT
AS	

BEGIN
	SET NOCOUNT ON;
	EXEC dbo.Generate_AutoNumber @ProductCode,@ProductCode  OUTPUT 
	SELECT @ProductCode
	INSERT INTO [sale].[Product]
	(
		[ProductCode],
		[ProductName],
		[Price],
		[Quantity],
		[IsActive],
		[Description],
		[CreateDate],
		[CreateBy]
	)
	VALUES
	(
		@ProductCode,
		@ProductName,
		@Price,
		@Quantity,
		@IsActive,
		@Description,
		@CreateDate,
		@CreateBy
	)
	SET @ProductId = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [sale].[Insert_Project]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Insert_Project]
	@ProjectId			VARCHAR(50),
	@ProjectCode		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Status				INT,
	@InvestorId			VARCHAR(50),
	@FromDate			DATETIME,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@ToDate				DATETIME,
	@Description		NVARCHAR(500)
AS

BEGIN
	INSERT INTO [sale].[Project]
	(
		[ProjectId],
		[ProjectCode],
		[FullName],
		[Status],
		[InvestorId],
		[FromDate],
		[CreateDate],
		[CreateBy],
		[ToDate],
		[Description]
	)
	VALUES
	(
		@ProjectId,
		@ProjectCode,
		@FullName,
		@Status,
		@InvestorId,
		@FromDate,
		@CreateDate,
		@CreateBy,
		@ToDate,
		@Description
	)
END
GO
/****** Object:  StoredProcedure [sale].[Update_Contract]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROC [sale].[Update_Contract]
	@ContractId			BIGINT,
	@ContractCode		VARCHAR(50),
	@CustomerId			BIGINT,
	@EmployeeId			INT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@TotalPrice			DECIMAL(18),
	@Description		NVARCHAR(500),
	@Status				INT,
	@ContractNumber	VARCHAR(50)
AS
BEGIN
	UPDATE [sale].[Contract]
	SET
		[CustomerId]		= @CustomerId,
		[EmployeeId]		= @EmployeeId,
		[TotalPrice]		= @TotalPrice,
		[Description]		= @Description,
		[Status]			= @Status,
		[ContractNumber]	= @ContractNumber
	WHERE	[ContractId] = @ContractId
END
GO
/****** Object:  StoredProcedure [sale].[Update_ContractDetail]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Update_ContractDetail]
	@ContractDetailId		VARCHAR(50),
	@ContractId				BIGINT,
	@ProductId				BIGINT,
	@Quantity				INT
AS

BEGIN
	UPDATE [sale].[ContractDetail]
	SET
		[ContractId]	= @ContractId,
		[ProductId]		= @ProductId,
		[Quantity]		= @Quantity
	WHERE	[ContractDetailId]	= @ContractDetailId
END
GO
/****** Object:  StoredProcedure [sale].[Update_Customer]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Update_Customer]
	@CustomerId				BIGINT,
	@CustomerCode			VARCHAR(50),
	@FullName				NVARCHAR(255),
	@IdentityCardDate		DATE,
	@CityIdentityCard		INT,
	@IdentityCard			VARCHAR(50),
	@Email					NVARCHAR(255),
	@PhoneNumber			VARCHAR(50),
	@Description			NVARCHAR(500),
	@Address				NVARCHAR(255),
	@CityId					INT,
	@DistrictId				INT,
	@TaxCode				VARCHAR(50),
	@CompanyName			VARCHAR(255),
	@BankAccountNumber		VARCHAR(50),
	@Status					TINYINT,
	@CreateDate				DATETIME,
	@CreateBy				INT
AS

BEGIN
	UPDATE [sale].[Customer]
	SET
		[FullName]			= @FullName,
		[IdentityCardDate]	= @IdentityCardDate,
		[CityIdentityCard]	= @CityIdentityCard,
		[IdentityCard]		= @IdentityCard,
		[Email]				= @Email,
		[PhoneNumber]		= @PhoneNumber,
		[Description]		= @Description,
		[Address]			= @Address,
		[CityId]			= @CityId,
		[DistrictId]		= @DistrictId,
		[TaxCode]			= @TaxCode,
		[CompanyName]		= @CompanyName,
		[BankAccountNumber]	= @BankAccountNumber,
		[Status]			= @Status
	WHERE	[CustomerId]	= @CustomerId
END
GO
/****** Object:  StoredProcedure [sale].[Update_Investor]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Update_Investor]
	@InvestorId			VARCHAR(50),
	@InvestorCode		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Company			NVARCHAR(255),
	@CompanyAddress		NVARCHAR(500),
	@Address			NVARCHAR(500),
	@CityId				INT,
	@DistrictId			INT,
	@Position			VARCHAR(255),
	@MsEnterprise		VARCHAR(50),
	@FoundedYear		INT,
	@CharterCapital		DECIMAL(18,2),
	@Status				INT,
	@CreateBy			INT,
	@CreateDate			DATETIME,
	@Description		NVARCHAR(500)
AS

BEGIN
	UPDATE [sale].[Investor]
	SET
		[FullName]			= @FullName,
		[Company]			= @Company,
		[CompanyAddress]	= @CompanyAddress,
		[Address]			= @Address,
		[CityId]			= @CityId,
		[DistrictId]		= @DistrictId,
		[Position]			= @Position,
		[MsEnterprise]		= @MsEnterprise,
		[FoundedYear]		= @FoundedYear,
		[CharterCapital]	= @CharterCapital,
		[Status]			= @Status,
		[Description]		= @Description
	WHERE 
		[InvestorId] = @InvestorId
END
GO
/****** Object:  StoredProcedure [sale].[Update_Product]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [sale].[Update_Product]
	@ProductId			BIGINT OUTPUT,
	@ProductCode		VARCHAR(50) OUTPUT,
	@ProductName		NVARCHAR(255),
	@Price				MONEY,
	@Quantity			INT,
	@IsActive			BIT,
	@Description		NVARCHAR(500),
	@CreateDate			DATETIME,
	@CreateBy			INT
AS

BEGIN
	UPDATE [sale].[Product]
	SET
		[ProductCode]	= @ProductCode,
		[ProductName]	= @ProductName,
		[Price]			= @Price,
		[Quantity]		= @Quantity,
		[IsActive]		= @IsActive,
		[Description]	= @Description
	WHERE [ProductId]	= @ProductId
END
GO
/****** Object:  StoredProcedure [sale].[Update_Project]    Script Date: 01/09/2020 15:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE PROC [sale].[Update_Project]
	@ProjectId			VARCHAR(50),
	@ProjectCode		VARCHAR(50),
	@FullName			NVARCHAR(255),
	@Status				INT,
	@InvestorId			VARCHAR(50),
	@FromDate			DATETIME,
	@CreateDate			DATETIME,
	@CreateBy			INT,
	@ToDate				DATETIME,
	@Description		NVARCHAR(500)
AS

BEGIN
	UPDATE [sale].[Project]
	SET


		[FullName]		= @FullName,
		[Status]		= @Status,
		[InvestorId]	= @InvestorId,
		[FromDate]		= @FromDate,
		[ToDate]		= @ToDate,
		[Description]	= @Description
	WHERE [ProjectId] = @ProjectId
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng lưu trữ tự động sinh mã' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tỉnh thành phố' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quốc tịch' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Country'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quận huyện' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'District'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'bảng lưu trữ chức năng hệ thống' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Function'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng lưu trữ module' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Module'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng lưu trữ phân hệ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ModuleGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hiển thị chinh sách văn hóa công ty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tài khoản hệ thống' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nhóm tài khoản' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quyền của nhóm người dùng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroupRights'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Xã phường' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ward'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hồ sơ ứng viên' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Applicant'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nghành nghề' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Career'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hợp đồng lao động' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Contract'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'loại hợp đồng' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'ContractType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phòng ban, cơ cấu tổ chức công ty' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Department'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'trình độ đào tạo' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'EducationLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thông tin nhân viên' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý nghỉ phép' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'EmployeeHoliday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngày nghỉ trong năm' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Holiday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cấu hình ngày nghỉ của nhân viên trong năm' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'HolidayConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngày nghỉ phép chi tiết detail' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'HolidayDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lý do nghỉ' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'HolidayReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lương phát sinh' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'IncurredSalary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thông tin bảo hiểm xã hội' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Insurance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảo hiểm y tế' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'InsuranceMedical'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quá trình tham gia bảo hiểm' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'InsuranceProcess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thuyên chuyển công tác' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'JobChange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vị trí đi theo phòng ban' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'LocationEmployee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chế độ thai sản (chấm công)' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Maternity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nơi đăng ký khám chữa bệnh' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Medical'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nhiệm vụ theo vị trí' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Mission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nhiệm vụ công việc chi tiết' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'MissionDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dân tộc' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Nation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chức vụ' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Position'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Khen thưởng kỷ luật' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'PraiseDiscipline'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chi tiết khen thưởng kỷ luật' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'PraiseDisciplineDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kênh tuyển dụng' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'RecruitChanel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kế hoạch tuyển dụng' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'RecruitPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kết quả tuyển dụng' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'RecruitResult'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chi tiết phỏng vấn, kết quả tuyển dụng' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'RecruitResultDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tôn giáo' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Religion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý lương nhân viên' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'Salary'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trường đào tạo' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'School'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ca làm việc' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'ShiftWork'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tính thời gian OT' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'TimeSheetOt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trình độ đào tạo' , @level0type=N'SCHEMA',@level0name=N'hrm', @level1type=N'TABLE',@level1name=N'TrainingLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mức độ hài lòng' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'AcceptConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Công việc được giao cho nhân viên' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'AssignWork'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gửi phàn nàn về hành vi nào đó của cán bộ khác' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'Complain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hệ số năng suất lao động tháng' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'FactorConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tỷ lệ hoàn thành công việc' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'FinishJobConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dữ liệu giờ hưu ích ,deadline' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'KpiConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Người thực hiện' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'Performer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'khả năng tự đề xuất công việc' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'SuggestJobConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Đề xuất công việc' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'SuggesWork'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng chứa danh mục công việc giao cho nhân viên tính thời gian hữu ích' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'Task'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lập kế hoạch công việc' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'WorkPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kế hoạch công việc chi tiết ứng với table Task' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'WorkPlanDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cấu hình điểm công việc' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'WorkPointConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Công việc cần phối hợp giữa các phòng' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'WorkStream'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Giao việc chi tiết cho nhân viên trong workstream' , @level0type=N'SCHEMA',@level0name=N'kpi', @level1type=N'TABLE',@level1name=N'WorkStreamDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý hợp đồng' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'Contract'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hợp đồng chi tiết detail' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'ContractDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý thông tin khách hàng' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'Customer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý nhân viên của chủ đầu tư' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'EmployeeInvestor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý chủ đầu tư' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'Investor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý hàng kinh doanh' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'Product'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quản lý dự án' , @level0type=N'SCHEMA',@level0name=N'sale', @level1type=N'TABLE',@level1name=N'Project'
GO
USE [master]
GO
ALTER DATABASE [DongLucDb] SET  READ_WRITE 
GO
