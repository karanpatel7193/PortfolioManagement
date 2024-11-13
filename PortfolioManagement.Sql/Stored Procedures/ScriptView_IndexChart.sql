CREATE PROCEDURE dbo.[ScriptView_IndexChart]
    @FromDate	DATE,
    @ToDate		DATE
AS
BEGIN

    SELECT			I.[Date],
					I.[SensexPreviousDay],	
					I.[SensexOpen],		
					I.[SensexClose],	
					I.[SensexHigh],		
					I.[SensexLow],	
					I.[NiftyPreviousDay],	
					I.[NiftyOpen],	
					I.[NiftyClose],		
					I.[NiftyHigh],	
					I.[NiftyLow],	
					I.[FII],	
					I.[DII],	
					I.[Sensex],	
					I.[Nifty]
    FROM			dbo.[Index] I
    WHERE			[Date] >= @FromDate 
        AND			[Date] <= @ToDate;
END;
