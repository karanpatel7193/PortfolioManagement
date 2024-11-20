CREATE PROCEDURE dbo.Index_SelectForFiiDiiChart
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN

    SELECT 
        I.Date,         
        I.Nifty,        
        I.Sensex,       
        I.FII,          
        I.DII           
    FROM 
        dbo.[Index] I 
    WHERE 
        I.Date >= @FromDate		
        AND I.Date <= @ToDate;	
END;