--USE [TradingDB]
GO
/****** Object:  StoredProcedure [dbo].[CreateExchangeDetails]    Script Date: 7/29/2017 2:58:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateExchangeDetails]
(
	@Google_Security_ID NVARCHAR(50),
    @Exchange NVARCHAR(50), 
    @LastPrice MONEY, 
    @Price MONEY, 
    @L_Cur MONEY, 
    @S INT, 
    @LastTradeTime DATETIME, 
    @LastTrdetimeformated NVARCHAR(50), 
    @LastTradeDateTime NVARCHAR(50), 
    @Change MONEY, 
    @C_fix MONEY, 
    @ChangePercentage MONEY, 
    @Cp_fix MONEY, 
    @Ccol NVARCHAR(50), 
    @Pcls_Fix MONEY, 
    @Afterhourslastprice MONEY, 
    @Afterhourslasttradetimeformated DATETIME, 
    @GetActualRecordTime DATETIME
)
AS
	INSERT INTO 
			Exchange_Details 
				(
					GOOGLE_SECURITY_ID,
					EXCHANGE, 
					LASTPRICE, 
					PRICE, 
					L_CUR, 
					S, 
					LAST_TRADE_TIME, 
					LAST_TRDE_TIME_FORMATED, 
					LAST_TRADE_DATETIME, 
					CHANGE, 
					C_FIX, 
					CHANGE_PERCENTAGE, 
					CP_FIX, 
					CCOL, 
					PCLS_FIX, 
					AFTER_HOURS_LAST_PRICE, 
					AFTER_HOURS_LAST_TRADE_TIMEFORMATED,
					GET_ACTUAL_RECORDTIME, 
					CREATE_DATE
				) 
				VALUES
				(
					@Google_Security_ID,
					@Exchange, 
					@LastPrice, 
					@Price, 
					@L_Cur, 
					@S, 
					@LastTradeTime, 
					@LastTrdetimeformated, 
					@LastTradeDateTime, 
					@Change, 
					@C_fix, 
					@ChangePercentage, 
					@Cp_fix, 
					@Ccol, 
					@Pcls_Fix, 
					@Afterhourslastprice, 
					@Afterhourslasttradetimeformated, 
					@GetActualRecordTime, 
					GETDATE()
				)
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[CreateGoogleHistory]    Script Date: 7/29/2017 2:58:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateGoogleHistory]
	@TICKER_SYMBOL NVARCHAR(20),
	@OPEN_PRICE MONEY,
	@LOW_PRICE MONEY,
	@HIGH_PRICE MONEY,
	@CLOSE_PRICE MONEY,
	@DATE_WITH_INTERVAL INT
AS
BEGIN
	INSERT INTO GOOGLE_HISTORY
			(
				TICKER_SYMBOL,
				OPEN_PRICE,
				LOW_PRICE,
				HIGH_PRICE,
				CLOSE_PRICE,
				DATE_WITH_INTERVAL
			)
			VALUES
			(
				@TICKER_SYMBOL,
				@OPEN_PRICE,
				@LOW_PRICE,
				@HIGH_PRICE,
				@CLOSE_PRICE,
				@DATE_WITH_INTERVAL
			)
END

GO
/****** Object:  StoredProcedure [dbo].[CreateTickerDetails]    Script Date: 7/29/2017 2:58:27 PM ******/
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
