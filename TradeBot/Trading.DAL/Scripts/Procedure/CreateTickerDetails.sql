USE [TradingDB]
GO

/****** Object:  StoredProcedure [dbo].[CreateTickerDetails]    Script Date: 7/20/2017 12:39:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateTickerDetails] 
	@TICKER_NAME nvarchar(50),
	@TICKER_SYMBOL nvarchar(20),
	@TICKER_ISIN nvarchar(20),
	@TICKER_MARKETCAP money,
	@TICKER_PERATIO money,
	@TICKER_DIVYIELD money,
	@TICKER_STATUS nvarchar(20),
	@TICKER_VWAP money,
	@TICKER_FACE_VALUE int
AS
BEGIN
		INSERT INTO TICKER_DETAILS
		(
			TICKER_NAME,
			TICKER_SYMBOL,
			TICKER_ISIN,
			TICKER_MARKETCAP,
			TICKER_PERATIO,
			TICKER_DIVYIELD,
			TICKER_STATUS,
			TICKER_VWAP,
			TICKER_FACE_VALUE
		)
		VALUES
		(
			@TICKER_NAME,
			@TICKER_SYMBOL,
			@TICKER_ISIN,
			@TICKER_MARKETCAP,
			@TICKER_PERATIO,
			@TICKER_DIVYIELD,
			@TICKER_STATUS,
			@TICKER_VWAP,
			@TICKER_FACE_VALUE
		)
END

GO


