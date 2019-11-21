-- Switch to the system (aka master) database
USE master;
GO

-- Delete the UserSecurity Database (IF EXISTS)
IF EXISTS(select * from sys.databases where name='[NPGeek]')
DROP DATABASE [NPGeek];
GO

/****** Object:  Database [NPGeek]    Script Date: 11/11/2019 10:26:08 AM ******/
CREATE DATABASE [NPGeek]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NPGeek', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\NPGeek.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NPGeek_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\NPGeek_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NPGeek] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NPGeek].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NPGeek] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NPGeek] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NPGeek] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NPGeek] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NPGeek] SET ARITHABORT OFF 
GO
ALTER DATABASE [NPGeek] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [NPGeek] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NPGeek] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NPGeek] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NPGeek] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NPGeek] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NPGeek] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NPGeek] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NPGeek] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NPGeek] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NPGeek] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NPGeek] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NPGeek] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NPGeek] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NPGeek] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NPGeek] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NPGeek] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NPGeek] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [NPGeek] SET  MULTI_USER 
GO
ALTER DATABASE [NPGeek] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NPGeek] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NPGeek] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NPGeek] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NPGeek] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NPGeek] SET QUERY_STORE = OFF
GO
USE [NPGeek]
GO
/****** Object:  Table [dbo].[park]    Script Date: 11/11/2019 10:26:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[park](
	[parkCode] [varchar](10) NOT NULL,
	[parkName] [varchar](200) NOT NULL,
	[state] [varchar](30) NOT NULL,
	[acreage] [int] NOT NULL,
	[elevationInFeet] [int] NOT NULL,
	[milesOfTrail] [real] NOT NULL,
	[numberOfCampsites] [int] NOT NULL,
	[climate] [varchar](100) NOT NULL,
	[yearFounded] [int] NOT NULL,
	[annualVisitorCount] [int] NOT NULL,
	[inspirationalQuote] [varchar](max) NOT NULL,
	[inspirationalQuoteSource] [varchar](200) NOT NULL,
	[parkDescription] [varchar](max) NOT NULL,
	[entryFee] [int] NOT NULL,
	[numberOfAnimalSpecies] [int] NOT NULL,
 CONSTRAINT [pk_park] PRIMARY KEY CLUSTERED 
(
	[parkCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleItem]    Script Date: 11/11/2019 10:26:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleItem](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey_result]    Script Date: 11/11/2019 10:26:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey_result](
	[surveyId] [int] IDENTITY(1,1) NOT NULL,
	[parkCode] [varchar](10) NOT NULL,
	[emailAddress] [varchar](100) NOT NULL,
	[state] [varchar](30) NOT NULL,
	[activityLevel] [varchar](100) NOT NULL,
 CONSTRAINT [pk_survey_result] PRIMARY KEY CLUSTERED 
(
	[surveyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/11/2019 10:26:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VendUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[weather]    Script Date: 11/11/2019 10:26:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[weather](
	[parkCode] [varchar](10) NOT NULL,
	[fiveDayForecastValue] [int] NOT NULL,
	[low] [int] NOT NULL,
	[high] [int] NOT NULL,
	[forecast] [varchar](100) NOT NULL,
 CONSTRAINT [pk_weather] PRIMARY KEY CLUSTERED 
(
	[parkCode] ASC,
	[fiveDayForecastValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'CVNP', N'Cuyahoga Valley National Park', N'Ohio', 32832, 696, 125, 0, N'Woodland', 2000, 2189849, N'Of all the paths you take in life, make sure a few of them are dirt.', N'John Muir', N'Though a short distance from the urban areas of Cleveland and Akron, Cuyahoga Valley National Park seems worlds away. The park is a refuge for native plants and wildlife, and provides routes of discovery for visitors. The winding Cuyahoga River gives way to deep forests, rolling hills, and open farmlands. Walk or ride the Towpath Trail to follow the historic route of the Ohio & Erie Canal', 0, 390)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'ENP', N'Everglades National Park', N'Florida', 1508538, 0, 35, 0, N'Tropical', 1934, 1110901, N'There are no other Everglades in the world. They are, they have always been, one of the unique regions of the earth; remote, never wholly known. Nothing anywhere else is like them.', N'Marjory Stoneman Douglas', N'The Florida Everglades, located in southern Florida, is one of the largest wetlands in the world. Several hundred years ago, this wetlands was a major part of a 5,184,000 acre watershed that covered almost a third of the entire state of Florida. The Everglades consist of a shallow sheet of fresh water that rolls slowly over the lowlands and through billions of blades of sawgrass. As water moves through the Everglades, it causes the sawgrass to ripple like green waves; this is why the Everglades received the nickname "River of Grass."', 8, 760)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'GCNP', N'Grand Canyon National Park', N'Arizona', 1217262, 8000, 115, 120, N'Desert', 1919, 4756771, N'It is the one great wonders. . . every American should see.', N'Theodore Roosevelt', N'If there is any place on Earth that puts into perspective the grandiosity of Mother Nature, it is the Grand Canyon. The natural wonder, located in northern Arizona, is a window into the regio''s geological and Native American past. As one of the country''s first national parks, the Grand Canyon has long been considered a U.S. treasure, and continues to inspire scientific study and puzzlement.', 8, 450)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'GNP', N'Glacier National Park', N'Montana', 1013322, 6646, 745.6, 923, N'Temperate', 1910, 2338528, N'Far away in northwestern Montana, hidden from view by clustering mountain peaks, lies an unmapped corner—the Crown of the Continent.', N'George Bird Grinnell', N'Glacier might very well be the most beautiful of America''s national parks. John Muir called it "the best care-killing scenery on the continent." The mountains are steep, snowcapped, and punctuated by stunning mountain lakes and creeks. Much of the land remains wild and pristine, a result of its remote location and the lack of visitation in the 19th century.  ', 12, 300)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'GSMNP', N'Great Smoky Mountains National Park', N'Tennessee', 522419, 6500, 850, 939, N'Temperate', 1934, 10099276, N'May your trails be crooked, winding, lonesome, dangerous, leading to the most amazing view. May your mountains rise into and above the clouds.', N'Edward Abbey', N'The Great Smoky Mountains are a mountain range rising along the Tennessee–North Carolina border in the southeastern United States. They are a subrange of the Appalachian Mountains, and form part of the Blue Ridge Physiographic Province. The range is sometimes called the Smoky Mountains and the name is commonly shortened to the Smokies. The Great Smokies are best known as the home of the Great Smoky Mountains National Park, which protects most of the range. The park was established in 1934, and, with over 9 million visits per year, it is the most-visited national park in the United States', 0, 400)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'GTNP', N'Grand Teton National Park', N'Wyoming', 310000, 6800, 200, 1206, N'Temperate', 1929, 2791392, N'We can not have freedom without wilderness.', N'Edward Abbey', N'The peaks of the Teton Range, regal and imposing as they stand nearly 7,000 feet above the valley floor, make one of the boldest geologic statements in the Rockies. Unencumbered by foothills, they rise through steep coniferous forest into alpine meadows strewn with wildflowers, past blue and white glaciers to naked granite pinnacles. The Grand, Middle, and South Tetons form the heart of the range. But their neighbors, especially Mount Owen, Teewinot Mountain, and Mount Moran, are no less spectacular.', 15, 380)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'MRNP', N'Mount Rainier National Park', N'Washington', 236381, 5500, 260, 573, N'Rainforest', 1899, 1038229, N'Of all the fire mountains which like beacons, once blazed along the Pacific Coast, Mount Rainier is the noblest.', N'Unknow', N'Mt. Rainier National Park is one of three national parks in the state of Washington and is one of America''s oldest parks, being one of only five founded in the 19th century. The park was created to preserve one of America''s most spectacular scenic wonders, the snow-capped volcano known as Tahcoma to Indians in ages past and as Mt. Rainier now. While the mountain is unquestionably the centerpiece of the park, its 235,612 acres (378 square miles) also contain mountain ranges, elaborate glaciers, rivers, deep forests, lush meadows covered with wildflowers during the summer, and over 300 miles of trails. 96% of the park is classified as wilderness.', 20, 280)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'RMNP', N'Rocky Mountain National Park', N'Colorado', 265761, 7800, 300, 660, N'Woodland', 1915, 3176941, N'It''s not the mountain we conquer, but ourselves.', N'Sir Edmund Hillary', N'Rocky Mountain National Park is one of the highest national parks in the nation, with elevations from 7,860 feet to 14,259 feet. Sixty mountain peaks over 12,000 feet high result in world-renowned scenery. The Continental Divide runs north - south through the park, and marks a climatic division. Ancient glaciers carved the topography into an amazing range of ecological zones. What you see within short distances at Rocky is similar to the wider landscape changes seen on a drive from Denver to northern Alaska.', 20, 360)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'YNP', N'Yellowstone National Park', N'Wyoming', 2219791, 8000, 900, 1900, N'Temperate', 1872, 3394326, N'Yellowstone Park is no more representative of America than is Disneyland.', N'John Steinbeck', N'Yellowstone National Park is a protected area showcasing significant geological phenomena and processes. It is also a unique manifestation of geothermal forces, natural beauty, and wild ecosystems where rare and endangered species thrive. As the site of one of the few remaining intact large ecosystems in the northern temperate zone of earth, Yellowstone’s ecological communities provide unparalleled opportunities for conservation, study, and enjoyment of large-scale wildland ecosystem processes.', 15, 390)
GO
INSERT [dbo].[park] ([parkCode], [parkName], [state], [acreage], [elevationInFeet], [milesOfTrail], [numberOfCampsites], [climate], [yearFounded], [annualVisitorCount], [inspirationalQuote], [inspirationalQuoteSource], [parkDescription], [entryFee], [numberOfAnimalSpecies]) VALUES (N'YNP2', N'Yosemite National Park', N'California', 747956, 5000, 800, 1720, N'Forest', 1890, 3882642, N'Yosemite Valley, to me, is always a sunrise, a glitter of green and golden wonder in a vast edifice of stone and space.', N'Ansel Adams', N'Yosemite National Park vividly illustrates the effects of glacial erosion of granitic bedrock, creating geologic features that are unique in the world. Repeated glaciations over millions of years have resulted in a concentration of distinctive landscape features, including soaring cliffs, domes, and free-falling waterfalls. There is exceptional glaciated topography, including the spectacular Yosemite Valley, a 914-meter (1/2 mile) deep, glacier-carved cleft with massive sheer granite walls. These geologic features provide a scenic backdrop for mountain meadows and giant sequoia groves, resulting in a diverse landscape of exceptional natural and scenic beauty.', 15, 420)
GO
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (1, N'Administrator')
GO
INSERT [dbo].[RoleItem] ([Id], [Name]) VALUES (2, N'StandardUser')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'CVNP', 1, 38, 62, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'CVNP', 2, 38, 56, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'CVNP', 3, 51, 66, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'CVNP', 4, 55, 65, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'CVNP', 5, 53, 69, N'thunderstorms')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'ENP', 1, 70, 82, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'ENP', 2, 70, 81, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'ENP', 3, 70, 81, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'ENP', 4, 71, 82, N'thunderstorms')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'ENP', 5, 70, 85, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GCNP', 1, 35, 66, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GCNP', 2, 34, 69, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GCNP', 3, 32, 57, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GCNP', 4, 34, 62, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GCNP', 5, 31, 62, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GNP', 1, 27, 40, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GNP', 2, 31, 43, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GNP', 3, 28, 40, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GNP', 4, 24, 34, N'cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GNP', 5, 25, 32, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GSMNP', 1, 58, 70, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GSMNP', 2, 56, 70, N'thunderstorms')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GSMNP', 3, 56, 74, N'cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GSMNP', 4, 53, 68, N'thunderstorms')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GSMNP', 5, 52, 66, N'thunderstorms')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GTNP', 1, 27, 46, N'cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GTNP', 2, 30, 49, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GTNP', 3, 31, 46, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GTNP', 4, 28, 41, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'GTNP', 5, 22, 38, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'MRNP', 1, 23, 30, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'MRNP', 2, 24, 32, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'MRNP', 3, 21, 27, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'MRNP', 4, 23, 27, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'MRNP', 5, 21, 25, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'RMNP', 1, 30, 47, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'RMNP', 2, 35, 55, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'RMNP', 3, 34, 50, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'RMNP', 4, 33, 47, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'RMNP', 5, 30, 43, N'rain')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP', 1, 23, 43, N'cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP', 2, 26, 47, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP', 3, 25, 44, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP', 4, 21, 37, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP', 5, 16, 36, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP2', 1, 34, 51, N'partly cloudy')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP2', 2, 25, 39, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP2', 3, 29, 40, N'sunny')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP2', 4, 32, 38, N'snow')
GO
INSERT [dbo].[weather] ([parkCode], [fiveDayForecastValue], [low], [high], [forecast]) VALUES (N'YNP2', 5, 23, 34, N'snow')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_User_Username]    Script Date: 11/11/2019 10:26:09 AM ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [UC_User_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[survey_result]  WITH CHECK ADD  CONSTRAINT [fk_survey_result_park] FOREIGN KEY([parkCode])
REFERENCES [dbo].[park] ([parkCode])
GO
ALTER TABLE [dbo].[survey_result] CHECK CONSTRAINT [fk_survey_result_park]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[weather]  WITH CHECK ADD  CONSTRAINT [fk_weather_park] FOREIGN KEY([parkCode])
REFERENCES [dbo].[park] ([parkCode])
GO
ALTER TABLE [dbo].[weather] CHECK CONSTRAINT [fk_weather_park]
GO
USE [master]
GO
ALTER DATABASE [NPGeek] SET  READ_WRITE 
GO
