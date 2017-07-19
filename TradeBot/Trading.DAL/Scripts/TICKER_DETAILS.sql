USE [TradingDB]
GO

/****** Object:  Table [dbo].[TICKER_DETAILS]    Script Date: 7/20/2017 12:37:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TICKER_DETAILS](
	[TICKER_SYMBOL] [nvarchar](20) NOT NULL,
	[TICKER_NAME] [nvarchar](50) NULL,
	[TICKER_ISIN] [nvarchar](20) NULL,
	[TICKER_MARKETCAP] [money] NULL,
	[TICKER_PERATIO] [money] NULL,
	[TICKER_DIVYIELD] [money] NULL,
	[TICKER_STATUS] [nvarchar](20) NULL,
	[TICKER_VWAP] [money] NULL,
	[TICKER_FACE_VALUE] [int] NULL,
 CONSTRAINT [PK_TICKER_DETAILS] PRIMARY KEY CLUSTERED 
(
	[TICKER_SYMBOL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


