USE [AUXPACEDB1]
GO

/****** Object:  Table [dbo].[OAuthProviders]    Script Date: 12/15/2020 7:53:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OAuthProviders](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ClientId] [varchar](100) NOT NULL,
	[ClientSecret] [varchar](100) NOT NULL,
	[Logo] [nvarchar](200) NULL,
	[RedirectUrl] [nvarchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[GrantTypesSupported] [varchar](100) NULL,
	[ClaimsSupported] [nvarchar](max) NULL,
	[ScopesSupported] [nvarchar](max) NULL,
	[JwksUri] [nvarchar](max) NOT NULL,
	[RevocationEndpoint] [nvarchar](max) NULL,
	[UserinfoEndpoint] [nvarchar](max) NULL,
	[TokenEndpoint] [nvarchar](max) NOT NULL,
	[DeviceAuthorizationEndpoint] [nvarchar](max) NULL,
	[AuthorizationEndpoint] [nvarchar](max) NULL,
	[Issuer] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[OAuthProviders] ADD  CONSTRAINT [DF_Providers_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[OAuthProviders] ADD  CONSTRAINT [DF_Providers_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

