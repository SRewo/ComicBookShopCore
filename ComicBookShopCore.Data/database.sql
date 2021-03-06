USE [master]
GO
/****** Object:  Database [ComicBookShopCore]    Script Date: 17.08.2019 19:13:14 ******/
CREATE DATABASE [ComicBookShopCore]
 CONTAINMENT = NONE
GO
ALTER DATABASE [ComicBookShopCore] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ComicBookShopCore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ComicBookShopCore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET ARITHABORT OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ComicBookShopCore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ComicBookShopCore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ComicBookShopCore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ComicBookShopCore] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ComicBookShopCore] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ComicBookShopCore', N'ON'
GO
ALTER DATABASE [ComicBookShopCore] SET QUERY_STORE = OFF
GO
USE [ComicBookShopCore]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 17.08.2019 19:13:14 ******/
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
/****** Object:  Table [dbo].[Address]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StreetName] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[PostalCode] [nvarchar](max) NOT NULL,
	[Country] [nvarchar](max) NOT NULL,
	[Region] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artists]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
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
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[AddressId] [int] NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComicBookArtists]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComicBookArtists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ArtistId] [int] NOT NULL,
	[Type] [nvarchar](max) NULL,
	[ComicBookId] [int] NULL,
 CONSTRAINT [PK_ComicBookArtists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComicBooks]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComicBooks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[OnSaleDate] [datetime2](7) NOT NULL,
	[Price] [float] NOT NULL,
	[Quantity] [int] NOT NULL,
	[SeriesId] [int] NOT NULL,
	[ShortDescription] [nvarchar](120) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_ComicBooks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ComicBookId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [int] NOT NULL,
	[OrderId] [int] NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publishers]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publishers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreationDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Publishers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Series]    Script Date: 17.08.2019 19:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Series](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[PublisherId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Series] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190816070243_Initial', N'3.0.0-preview7.19362.6')
SET IDENTITY_INSERT [dbo].[Address] ON 

INSERT [dbo].[Address] ([Id], [StreetName], [City], [PostalCode], [Country], [Region]) VALUES (1, N'Street', N'City', N'43-555', N'Poland', N'Poland')
SET IDENTITY_INSERT [dbo].[Address] OFF
SET IDENTITY_INSERT [dbo].[Artists] ON 

INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (1, N'Scott', N'Snyder', N'Scott Snyder has written comics for both DC and Marvel, including the bestselling series AMERICAN VAMPIRE, BATMAN and SWAMP THING, and is the author of the story collection Voodoo Heart (The Dial Press). He teaches writing at Sarah Lawrence College, NYC and Columbia University. He lives on Long Island with his wife Jeanie, and his sons Jack and Emmett. He is a dedicated and un-ironic fan of Elvis Presley.')
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (2, N'Greg', N'Capullo', N'Greg Capullo is a self taught Illustrator and the current artist on the best-selling and highly acclaimed BATMAN series for DC Comics. Prior to that, he was best known for his 80 issue run on Image Comics'' SPAWN, created by Todd McFarlane. Other popular comics work includes Marvel Comics X-FORCE and QUASAR (as well as a slew of one-shot titles). He is also the creator of The Creech, a Sci-Fi/Horror comic published by Image Comics.')
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (3, N'Dan', N'Jurgens', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (4, N'Tyler', N'Kirkham', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (5, N'Donny', N'Cates', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (6, N'Niko', N'Henrichon', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (7, N'Juan', N'Gadeon', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (8, N'Cullen', N'Bunn', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (9, N'Kyle', N'Strahm', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (10, N'Mike', N'Mignola', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (11, N'John', N'Byrne', NULL)
INSERT [dbo].[Artists] ([Id], [FirstName], [LastName], [Description]) VALUES (12, N'Marco', N'Ghiglione', NULL)
SET IDENTITY_INSERT [dbo].[Artists] OFF
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'370f52d9-035d-4b1d-a1e0-adbc7caf539b', N'User', N'USER', N'594e5ff6-a977-4b81-bf58-e747260f402e')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'53d9b7cf-8c3a-41d8-900e-384037d311d5', N'Owner', N'OWNER', N'9ba37b7f-f2e8-4732-a825-f3674f43822d')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ab714e60-706a-4186-a700-bc96b9993260', N'Admin', N'ADMIN', N'b35795cf-7fbd-47c4-aeef-d0b960d213b4')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ba2202e2-bf02-42c5-bb42-d56ae22b059e', N'Employee', N'EMPLOYEE', N'1c34fd1f-b9e4-4a86-a04c-a20f853ae79c')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'38878ff9-b2f4-4601-8970-8424ab804bc4', N'ab714e60-706a-4186-a700-bc96b9993260')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [AddressId], [DateOfBirth]) VALUES (N'38878ff9-b2f4-4601-8970-8424ab804bc4', N'Admin', N'ADMIN', N'admin@admin.pl', N'ADMIN@ADMIN.PL', 0, N'AQAAAAEAACcQAAAAEEyajh6Q9Zk6bOYylQ6Qcg5c+WKz3JvZ1ZV8A+Lrkp7R/ROYhyp0i7y3FUDhDiDwsw==', N'EAGX5BXCVMUM2BNEB5OF747UNKE6G3XC', N'4521e049-1817-465a-a3aa-9a47fa9de6f3', NULL, 0, 0, NULL, 1, 0, N'Super', N'Admin', 1, CAST(N'1990-04-04T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ComicBookArtists] ON 

INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (1, 1, N'Writer', 1)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (2, 2, N'Cover Artist', 1)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (3, 4, N'Art', 2)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (4, 3, N'Writer', 2)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (5, 5, N'Writer', 3)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (7, 7, N'Penciler', 4)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (8, 5, N'Writer', 4)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (10, 6, N'Penciler', 3)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (13, 8, N'Writer', 5)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (14, 9, N'Writer', 5)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (15, 11, N'Writer', 6)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (16, 10, N'Writer & Art', 6)
INSERT [dbo].[ComicBookArtists] ([Id], [ArtistId], [Type], [ComicBookId]) VALUES (17, 12, N'Cover Artist', 7)
SET IDENTITY_INSERT [dbo].[ComicBookArtists] OFF
SET IDENTITY_INSERT [dbo].[ComicBooks] ON 

INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (1, N'Dark Nights Metal: #1', CAST(N'2017-08-16T00:00:00.0000000' AS DateTime2), 4.99, 20, 1, N'DARK NIGHTS: THE FORGE and THE CASTING hinted at dark corners of reality that have never been seen till now!', N'DARK NIGHTS: THE FORGE and THE CASTING hinted at dark corners of reality that have never been seen till now! Now, as DARK DAYS: METAL begins, the Dark Multiverse is revealed in all its devastating danger-and the threats it contains are coming for the DC Universe! DARK DAYS: METAL is a DC event unlike any other-one that will push Batman, Superman and heroes of the Justice League beyond their limits to take on threats unlike any our world has ever seen! It will take the combined might of the World''s Greatest Heroes as you''ve never seen them before to face what''s coming their way!')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (2, N'Action Comics #959', CAST(N'2016-06-13T00:00:00.0000000' AS DateTime2), 2.99, 15, 2, N'Clark Kent gets caught in the crossfire as Doomsday crashes through the streets of Metropolis!', N'“PATH TO DOOM” Chapter Three: Clark Kent gets caught in the crossfire as Doomsday crashes through the streets of Metropolis! As Lois struggles to keep young Jonathan out of the path of destruction, can former enemies Superman and Lex Luthor stop the monster that once destroyed the city and killed the Man of Steel—or does Luthor have other plans?')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (3, N'Doctor Strange (2015) #389', CAST(N'2019-04-02T00:00:00.0000000' AS DateTime2), 3.99, 10, 3, N'Stephen Strange’s first act back as Sorcerer Supreme backfired catastrophically!', N'Maybe the Vishanti were right about him…but will the world trust him to clean up his mess? There’s a new magical landscape after DAMNATION, and a long journey back to the Sanctum Sanctorum!')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (4, N'Venom (2018) #16', CAST(N'2019-06-17T00:00:00.0000000' AS DateTime2), 3.99, 20, 4, N'After weeks on the run and battling the monsters of Asgard, Eddie Brock finally has a moment to catch his breath.', N'After weeks on the run and battling the monsters of Asgard through the WAR OF THE REALMS, Eddie Brock finally has a moment to catch his breath. But without his symbiote, getting even the basic necessities will become a challenge for Eddie Brock. Which means that keeping his son, Dylan, alive will be too!')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (5, N'Unearth #1', CAST(N'2019-07-10T00:00:00.0000000' AS DateTime2), 3.99, 50, 5, NULL, N'When a flesh-warping disease ravages a remote village in Mexico, a scientific task force travels to the inhospitable area to investigate the contamination. Tracing the source of the disease to a nearby cave system, the team discovers a bizarre, hostile ecosystem and a supernatural revelation from which they may never escape. This new subterranean nightmare is brought to you by writers CULLEN BUNN (REGRESSION) and KYLE STRAHM (SPREAD), and rising-star artist BALDEMAR RIVAS!')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (6, N'OMNIBUS VOLUME 1: SEED OF DESTRUCTION', CAST(N'2018-05-23T00:00:00.0000000' AS DateTime2), 14.99, 5, 6, N'The Hellboy saga begins—with over 300 pages drawn by Mignola!', N'The Hellboy saga begins—with over 300 pages drawn by Mignola! For the first time, Hellboy''s complete story is presented in chronological order for the ultimate reading experience.The story jumps from Hellboy''s mysterious World War II origin to his 1994 confrontation with the man who summoned him to earth, and the earliest signs of the plague of frogs. Avoiding his supposed fate as the herald of the end of the world, Hellboy continues with the Bureau for Paranormal Research and Defense, fighting alongside Abe Sapien, Liz Sherman, and drafting Roger Homunculus into his own ill-fated service with the B.P.R.D.')
INSERT [dbo].[ComicBooks] ([Id], [Title], [OnSaleDate], [Price], [Quantity], [SeriesId], [ShortDescription], [Description]) VALUES (7, N'DuckTales: Treasure Trove', CAST(N'2018-01-23T00:00:00.0000000' AS DateTime2), 9.99, 10, 7, N'DuckTales returns! The fan-favorite cartoon gets a modern reboot on Disney Channel and an all-new comic series!', N'DuckTales (woo-oo!) returns! The fan-favorite cartoon gets a modern reboot on Disney XD and an all-new comic series! Return to Duckburg in a new generation of adventures featuring Uncle Scrooge, Donald Duck, and his nephews Huey, Dewey, and Louie! The incorrigible billionaire is up to his wacky, quacky hijinks, with a cast of characters new and old ready to jump into the mix!')
SET IDENTITY_INSERT [dbo].[ComicBooks] OFF
SET IDENTITY_INSERT [dbo].[Publishers] ON 

INSERT [dbo].[Publishers] ([Id], [Name], [Description], [CreationDateTime]) VALUES (1, N'DC Comics', N'DC Comics, Inc. is an American comic book publisher. It is the publishing unit of DC Entertainment, a subsidiary of Warner Bros. since 1967. DC Comics is one of the largest and oldest American comic book companies, and produces material featuring numerous culturally iconic heroic characters including: Batman, Superman, Wonder Woman, The Flash, Green Lantern, Aquaman, Shazam, Martian Manhunter, Nightwing, Green Arrow, Hawkman, Cyborg, Batgirl and Supergirl.', CAST(N'1937-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Publishers] ([Id], [Name], [Description], [CreationDateTime]) VALUES (2, N'Marvel Comics', N'Marvel Comics is the brand name and primary imprint of Marvel Worldwide Inc., formerly Marvel Publishing, Inc. and Marvel Comics Group, a publisher of American comic books and related media. In 2009, The Walt Disney Company acquired Marvel Entertainment, Marvel Worldwide''s parent company', CAST(N'1939-01-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Publishers] ([Id], [Name], [Description], [CreationDateTime]) VALUES (3, N'Dark Horse Comics', N'Dark Horse Comics is an American comic book and manga publisher. It was founded in 1986 by Mike Richardson in Milwaukie, Oregon.', CAST(N'1986-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Publishers] ([Id], [Name], [Description], [CreationDateTime]) VALUES (4, N'Image Comics', N'Image Comics is an American comic book publisher and is the third largest comic book and graphic novel publisher in the industry in both unit and market share. It was founded in 1992 by several high-profile illustrators as a venue for creator-owned properties, in which comics creators could publish material of their own creation without giving up the copyrights to those properties, as is normally the case in the work for hire-dominated American comics industry, in which the legal author is a publisher, such as Marvel Comics or DC Comics, and the creator is an employee of that publisher. Its output was originally dominated by superhero and fantasy series from the studios of the founding Image partners, but now includes comics in many genres by numerous independent creators. Its best-known publications include The Walking Dead, Spawn, Savage Dragon, Witchblade, The Darkness, Invincible, Saga, Chew, Bone, Deadly Class, I Kill Giants, Hit-Girl, Kick-Ass, Kingsman: The Secret Service, Monstress, Snotgirl, Bitch Planet, Criminal, The Wicked + The Divine, Outcast By Kirkman & Azaceta, Descender, Wytches, and more.', CAST(N'1992-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[Publishers] ([Id], [Name], [Description], [CreationDateTime]) VALUES (5, N'IDW Publishing', N'IDW Publishing is an American publisher of comic books, graphic novels, art books, and comic strip collections. It was founded in 1999 as the publishing division of Idea and Design Works, LLC (IDW), itself formed in 1999, and is regularly recognized as the fifth-largest comic book publisher in the United States, behind Marvel, DC, Dark Horse and Image Comics, ahead of other major comic book publishers such as Archie, Boom!, Dynamite and Oni Press. The company is perhaps best known for its licensed comic book adaptations of movies, television shows, video games, and cartoons.', CAST(N'1999-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Publishers] OFF
SET IDENTITY_INSERT [dbo].[Series] ON 

INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (1, N'Dark Nights: Metal 2017', 1, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (2, N'Action Comics 2016', 1, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (3, N'Doctor Strange', 2, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (4, N'Venom', 2, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (5, N'Unearth', 4, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (6, N'HELLBOY: OMNIBUS', 3, NULL)
INSERT [dbo].[Series] ([Id], [Name], [PublisherId], [Description]) VALUES (7, N'Duck Tales', 5, NULL)
SET IDENTITY_INSERT [dbo].[Series] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 17.08.2019 19:13:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_AddressId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_AddressId] ON [dbo].[AspNetUsers]
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 17.08.2019 19:13:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ComicBookArtists_ArtistId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_ComicBookArtists_ArtistId] ON [dbo].[ComicBookArtists]
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ComicBookArtists_ComicBookId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_ComicBookArtists_ComicBookId] ON [dbo].[ComicBookArtists]
(
	[ComicBookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ComicBooks_SeriesId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_ComicBooks_SeriesId] ON [dbo].[ComicBooks]
(
	[SeriesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_ComicBookId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_ComicBookId] ON [dbo].[OrderItems]
(
	[ComicBookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_OrderId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId] ON [dbo].[OrderItems]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Orders_UserId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_UserId] ON [dbo].[Orders]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Series_PublisherId]    Script Date: 17.08.2019 19:13:14 ******/
CREATE NONCLUSTERED INDEX [IX_Series_PublisherId] ON [dbo].[Series]
(
	[PublisherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_Address_AddressId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ComicBookArtists]  WITH CHECK ADD  CONSTRAINT [FK_ComicBookArtists_Artists_ArtistId] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artists] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ComicBookArtists] CHECK CONSTRAINT [FK_ComicBookArtists_Artists_ArtistId]
GO
ALTER TABLE [dbo].[ComicBookArtists]  WITH CHECK ADD  CONSTRAINT [FK_ComicBookArtists_ComicBooks_ComicBookId] FOREIGN KEY([ComicBookId])
REFERENCES [dbo].[ComicBooks] ([Id])
GO
ALTER TABLE [dbo].[ComicBookArtists] CHECK CONSTRAINT [FK_ComicBookArtists_ComicBooks_ComicBookId]
GO
ALTER TABLE [dbo].[ComicBooks]  WITH CHECK ADD  CONSTRAINT [FK_ComicBooks_Series_SeriesId] FOREIGN KEY([SeriesId])
REFERENCES [dbo].[Series] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ComicBooks] CHECK CONSTRAINT [FK_ComicBooks_Series_SeriesId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_ComicBooks_ComicBookId] FOREIGN KEY([ComicBookId])
REFERENCES [dbo].[ComicBooks] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_ComicBooks_ComicBookId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders_OrderId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_Publishers_PublisherId] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[Publishers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_Publishers_PublisherId]
GO
