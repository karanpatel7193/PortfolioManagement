CREATE PROCEDURE Index_SelectForChart
    @DateRange VARCHAR(20),
    @TodayDate DATETIME 
AS
BEGIN
    DECLARE @StartDate DATETIME;

    -- Determine the start date based on the @DateRange parameter
    IF @DateRange = '1D'
    BEGIN
        SET @StartDate = DATEADD(day, -1, @TodayDate); 
    END
    ELSE IF @DateRange = '1W'
    BEGIN
        SET @StartDate = DATEADD(week, -1, @TodayDate);
    END
    ELSE IF @DateRange = '1M'
    BEGIN
        SET @StartDate = DATEADD(month, -1, @TodayDate);
    END
    ELSE IF @DateRange = '3M'
    BEGIN
        SET @StartDate = DATEADD(month, -3, @TodayDate);
    END
    ELSE IF @DateRange = '6M'
    BEGIN
        SET @StartDate = DATEADD(month, -6, @TodayDate);
    END
    ELSE IF @DateRange = '1Y'
    BEGIN
        SET @StartDate = DATEADD(year, -1, @TodayDate);
    END
    ELSE
    BEGIN
        -- Default case: if no valid parameter, return the last 7 days
        SET @StartDate = DATEADD(day, -7, @TodayDate);
    END

    
    IF @DateRange = '1D'
    BEGIN
        SELECT [DateTime] AS [Date], Sensex, Nifty
        FROM [IndexRaw]
        WHERE [DateTime] >= @StartDate
          AND [DateTime] <= @TodayDate;
    END
    ELSE
    BEGIN
        SELECT Date,  Sensex, Nifty
        FROM [Index]
        WHERE [Date] >= @StartDate
          AND [Date] <= @TodayDate;
    END
END;
