USE [TradingDB]
GO

/****** Object:  StoredProcedure [dbo].[CreateExchangeDetails]    Script Date: 7/20/2017 12:38:31 AM ******/
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


