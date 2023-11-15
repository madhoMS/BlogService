Create database [BlogServer]
GO
USE [BlogServer]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 14-11-2023 7.27.31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 14-11-2023 7.27.31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [uniqueidentifier] NOT NULL,
	[PostId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 14-11-2023 7.27.31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Replies]    Script Date: 14-11-2023 7.27.31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Replies](
	[Id] [uniqueidentifier] NOT NULL,
	[parentReplyId] [uniqueidentifier] NULL,
	[CommentId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Replies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14-11-2023 7.27.31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[UserName] [nvarchar](max) NULL,
	[NormalizedUserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[NormalizedEmail] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231110053107_InitialCreate', N'7.0.13')
GO
INSERT [dbo].[Posts] ([Id], [UserId], [Title], [Content], [ImageUrl], [CreatedAt], [ModifiedAt]) VALUES (N'a1d12a70-be56-4c58-4eeb-08dbe4fd00ad', N'56afb7cc-c6fa-447b-531b-08dbe4e29e76', N'Post 1', N'Post Description', N'', CAST(N'2023-11-14T16:02:29.2013908' AS DateTime2), NULL)
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [IsDeleted], [ImageUrl], [CreatedAt], [ModifiedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'56afb7cc-c6fa-447b-531b-08dbe4e29e76', N'Test', N'User', 0, N'', CAST(N'2023-11-14T07:23:34.1752484' AS DateTime2), NULL, N'testingexample0@gmail.com', N'TESTINGEXAMPLE0@GMAIL.COM', N'testingexample0@gmail.com', N'TESTINGEXAMPLE0@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAELDehCQecrDooHIT36nCXQpEENPd7ztmt+cO90x+VJ5WWmbcF0HdnEEmEyLmGg2fXg==', N'XBARQEXF27WFNMD5FMWAOUZI5WI7XHMX', N'7da9380c-3e99-4bb3-9537-c68c1d7468db', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [IsDeleted], [ImageUrl], [CreatedAt], [ModifiedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'439087b4-f572-4626-531c-08dbe4e29e76', N'Test', N'User', 0, N'', CAST(N'2023-11-14T07:23:44.1012217' AS DateTime2), NULL, N'testingexample0gmail.com', N'TESTINGEXAMPLE0GMAIL.COM', N'testingexample0gmail.com', N'TESTINGEXAMPLE0GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEJmRYX5Wy3xYlUZQalMpGm03Trekgpl04wOhMTsteCMkUfd35jV0gFCV7bLeC9mLUg==', N'2PGYI4UBNSWXGXP53TRCWTXF7ONRNGQV', N'60caf110-4bdb-4e62-973a-8044220de632', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [IsDeleted], [ImageUrl], [CreatedAt], [ModifiedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0215bf75-5f45-402b-d97f-08dbe4e2c5ae', N'test', N'User', 0, N'', CAST(N'2023-11-14T07:24:30.5942706' AS DateTime2), NULL, N'testexam', N'TESTEXAM', N'testexam', N'TESTEXAM', 0, N'AQAAAAEAACcQAAAAENwk28SoMSf9sH6CFtikUrkVLv/b+3GXlEcSdLa7g/0cfT81S5zCn9ddAiiFcJ3SDA==', N'VGVTLYRODRY2X657GA6OOXADRIRIM2WT', N'7f208533-3835-4f92-9641-338608f067f2', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [IsDeleted], [ImageUrl], [CreatedAt], [ModifiedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7b164f4a-8220-47e9-d287-08dbe4f3b1b7', N'user', N'name', 0, N'', CAST(N'2023-11-14T09:25:43.4415953' AS DateTime2), NULL, N'test@gmail.com', N'TEST@GMAIL.COM', N'test@gmail.com', N'TEST@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEEL4pUvo3/VsLbzg2ylHizY1bfoZR4B+egEhoAq04ne89eUB0eOwzIWhMho98r3TPg==', N'K5AZRVWLJXNBJYU4EKCNCQ5UTMNKWRDZ', N'b4973f39-a021-4184-b947-7fc7dd61c170', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [IsDeleted], [ImageUrl], [CreatedAt], [ModifiedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8764895c-e8f7-4ca5-cb86-08dbe4f3eb1a', N'User', N'Name', 0, N'', CAST(N'2023-11-14T09:27:22.7006075' AS DateTime2), NULL, N'test1@gmail.com', N'TEST1@GMAIL.COM', N'test1@gmail.com', N'TEST1@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEOF9Oj7MvFREtvulkporgV5TCpppBRzTYkjAqeT11Vs84ordjoETgQL7NBGr2HYpKw==', N'QUFSPLBSTEERCNLU266DVNCOMTFA36LQ', N'ed92fdf7-1df3-4c68-b1f3-0b1cf91e0d71', NULL, 0, 0, NULL, 1, 0)
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Posts_PostId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users_UserId]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Users_UserId]
GO
ALTER TABLE [dbo].[Replies]  WITH CHECK ADD  CONSTRAINT [FK_Replies_Comments_CommentId] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Replies] CHECK CONSTRAINT [FK_Replies_Comments_CommentId]
GO
ALTER TABLE [dbo].[Replies]  WITH CHECK ADD  CONSTRAINT [FK_Replies_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Replies] CHECK CONSTRAINT [FK_Replies_Users_UserId]
GO
