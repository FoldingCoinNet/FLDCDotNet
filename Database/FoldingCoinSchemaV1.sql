USE [master]
GO
/****** Object:  Database [FoldingCoin]    Script Date: 4/14/2018 3:49:40 PM ******/
CREATE DATABASE [FoldingCoin]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FoldingCoin', FILENAME = N'F:\Databases\FoldingCoin.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FoldingCoin_log', FILENAME = N'F:\Databases\FoldingCoin_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FoldingCoin] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FoldingCoin].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FoldingCoin] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FoldingCoin] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FoldingCoin] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FoldingCoin] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FoldingCoin] SET ARITHABORT OFF 
GO
ALTER DATABASE [FoldingCoin] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FoldingCoin] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FoldingCoin] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FoldingCoin] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FoldingCoin] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FoldingCoin] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FoldingCoin] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FoldingCoin] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FoldingCoin] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FoldingCoin] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FoldingCoin] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FoldingCoin] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FoldingCoin] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FoldingCoin] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FoldingCoin] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FoldingCoin] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FoldingCoin] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FoldingCoin] SET RECOVERY FULL 
GO
ALTER DATABASE [FoldingCoin] SET  MULTI_USER 
GO
ALTER DATABASE [FoldingCoin] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FoldingCoin] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FoldingCoin] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FoldingCoin] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [FoldingCoin] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FoldingCoin] SET QUERY_STORE = OFF
GO
USE [FoldingCoin]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [FoldingCoin]
GO
/****** Object:  Schema [FoldingCoin]    Script Date: 4/14/2018 3:49:40 PM ******/
CREATE SCHEMA [FoldingCoin]
GO
/****** Object:  Table [FoldingCoin].[Downloads]    Script Date: 4/14/2018 3:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Downloads](
	[DownloadId] [int] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NOT NULL,
	[FileId] [int] NOT NULL,
	[DownloadDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[DownloadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[FAHData]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[FAHData](
	[FAHDataId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[TotalPoints] [bigint] NOT NULL,
	[WorkUnits] [bigint] NOT NULL,
	[TeamNumber] [bigint] NOT NULL,
 CONSTRAINT [PK_FAHData] PRIMARY KEY CLUSTERED 
(
	[FAHDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[FAHDataRuns]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[FAHDataRuns](
	[FAHDataRunId] [int] IDENTITY(1,1) NOT NULL,
	[FAHDataId] [int] NOT NULL,
	[DownloadId] [int] NOT NULL,
	[TeamMemberId] [int] NOT NULL,
 CONSTRAINT [PK_FAHDataRun] PRIMARY KEY CLUSTERED 
(
	[FAHDataRunId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[Files]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Files](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](50) NULL,
	[FileExtension] [nvarchar](5) NULL,
	[FileData] [nvarchar](max) NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[Rejections]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Rejections](
	[RejectionId] [int] IDENTITY(1,1) NOT NULL,
	[DownloadId] [int] NOT NULL,
	[LineNumber] [int] NULL,
	[Reason] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Rejections] PRIMARY KEY CLUSTERED 
(
	[RejectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[Statuses]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Statuses](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[StatusDescription] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[TeamMembers]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[TeamMembers](
	[TeamMemberId] [int] IDENTITY(1,1) NOT NULL,
	[TeamId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_TeamMembers] PRIMARY KEY CLUSTERED 
(
	[TeamMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[Teams]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Teams](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamNumber] [bigint] NOT NULL,
	[TeamName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[Users]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[FriendlyName] [nvarchar](50) NULL,
	[BitcoinAddress] [nvarchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FoldingCoin].[UserStats]    Script Date: 4/14/2018 3:49:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FoldingCoin].[UserStats](
	[UserStatId] [int] IDENTITY(1,1) NOT NULL,
	[FAHDataRunId] [int] NOT NULL,
	[Points] [bigint] NOT NULL,
	[WorkUnits] [bigint] NOT NULL,
 CONSTRAINT [PK_UserStats] PRIMARY KEY CLUSTERED 
(
	[UserStatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [FoldingCoin].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Files] FOREIGN KEY([FileId])
REFERENCES [FoldingCoin].[Files] ([FileId])
GO
ALTER TABLE [FoldingCoin].[Downloads] CHECK CONSTRAINT [FK_Downloads_Files]
GO
ALTER TABLE [FoldingCoin].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Statuses] FOREIGN KEY([StatusId])
REFERENCES [FoldingCoin].[Statuses] ([StatusId])
GO
ALTER TABLE [FoldingCoin].[Downloads] CHECK CONSTRAINT [FK_Downloads_Statuses]
GO
ALTER TABLE [FoldingCoin].[FAHData]  WITH CHECK ADD  CONSTRAINT [FK_FAHData_Users] FOREIGN KEY([UserName])
REFERENCES [FoldingCoin].[Users] ([UserName])
GO
ALTER TABLE [FoldingCoin].[FAHData] CHECK CONSTRAINT [FK_FAHData_Users]
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns]  WITH CHECK ADD  CONSTRAINT [FK_FAHDataRuns_Downloads] FOREIGN KEY([DownloadId])
REFERENCES [FoldingCoin].[Downloads] ([DownloadId])
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns] CHECK CONSTRAINT [FK_FAHDataRuns_Downloads]
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns]  WITH CHECK ADD  CONSTRAINT [FK_FAHDataRuns_FAHData] FOREIGN KEY([FAHDataId])
REFERENCES [FoldingCoin].[FAHData] ([FAHDataId])
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns] CHECK CONSTRAINT [FK_FAHDataRuns_FAHData]
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns]  WITH CHECK ADD  CONSTRAINT [FK_FAHDataRuns_TeamMembers] FOREIGN KEY([TeamMemberId])
REFERENCES [FoldingCoin].[TeamMembers] ([TeamMemberId])
GO
ALTER TABLE [FoldingCoin].[FAHDataRuns] CHECK CONSTRAINT [FK_FAHDataRuns_TeamMembers]
GO
ALTER TABLE [FoldingCoin].[Rejections]  WITH CHECK ADD  CONSTRAINT [FK_Rejections_Downloads] FOREIGN KEY([DownloadId])
REFERENCES [FoldingCoin].[Downloads] ([DownloadId])
GO
ALTER TABLE [FoldingCoin].[Rejections] CHECK CONSTRAINT [FK_Rejections_Downloads]
GO
ALTER TABLE [FoldingCoin].[TeamMembers]  WITH CHECK ADD  CONSTRAINT [FK_TeamMembers_Teams] FOREIGN KEY([TeamMemberId])
REFERENCES [FoldingCoin].[Teams] ([TeamId])
GO
ALTER TABLE [FoldingCoin].[TeamMembers] CHECK CONSTRAINT [FK_TeamMembers_Teams]
GO
ALTER TABLE [FoldingCoin].[TeamMembers]  WITH CHECK ADD  CONSTRAINT [FK_TeamMembers_Users] FOREIGN KEY([TeamId])
REFERENCES [FoldingCoin].[Users] ([UserId])
GO
ALTER TABLE [FoldingCoin].[TeamMembers] CHECK CONSTRAINT [FK_TeamMembers_Users]
GO
ALTER TABLE [FoldingCoin].[UserStats]  WITH CHECK ADD  CONSTRAINT [FK_UserStats_FAHDataRun] FOREIGN KEY([FAHDataRunId])
REFERENCES [FoldingCoin].[FAHDataRuns] ([FAHDataRunId])
GO
ALTER TABLE [FoldingCoin].[UserStats] CHECK CONSTRAINT [FK_UserStats_FAHDataRun]
GO
USE [master]
GO
ALTER DATABASE [FoldingCoin] SET  READ_WRITE 
GO
