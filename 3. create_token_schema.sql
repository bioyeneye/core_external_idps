USE [AUXPACEDB1]
GO

/****** Object:  Table [dbo].[OAuthRefreshTokens]    Script Date: 12/15/2020 7:52:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OAuthRefreshTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Provider] [varchar](50) NOT NULL,
	[RefreshToken] [varchar](200) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
 CONSTRAINT [PK_OAuthRefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_OAuthRefreshTokens] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OAuthRefreshTokens] ADD  CONSTRAINT [DF_OAuthRefreshTokens_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[OAuthRefreshTokens] ADD  CONSTRAINT [DF_OAuthRefreshTokens_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

