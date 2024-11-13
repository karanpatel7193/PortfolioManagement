CREATE PROCEDURE [dbo].[Portfolio_Insert_Update]
	@AccountId			TINYINT,
    @ScriptId			SMALLINT,
    @BrokerId			TINYINT,
    @DateTime           DATETIME,
    @IsDelete           BIT,
    @TransactionId      INT,
    @TransactionTypeId	INT
AS
BEGIN
    IF @TransactionTypeId = 3003
        RETURN

    DECLARE @ProtfolioIds TABLE (ProtfolioId INT)
    DECLARE @MinDateTime    DATETIME = @DateTime
    
    SELECT   @MinDateTime = MIN(SP.DateTime) 
    FROM     dbo.StockTransaction ST
    INNER JOIN dbo.[TransactionProtfolio] SP
        ON   SP.TransactionId   = ST.Id
    WHERE    ST.AccountId       = @AccountId 
         AND ST.BrokerId        = @BrokerId 
         AND ST.ScriptId        = @ScriptId
         AND (SP.TransactionId  = @TransactionId
                 OR SP.ProtfolioId IN (
                        SELECT      ISP.ProtfolioId
                        FROM        dbo.[TransactionProtfolio] ISP
                        WHERE       ISP.TransactionId = @TransactionId
                    ))

    INSERT INTO @ProtfolioIds
    SELECT   SP.ProtfolioId 
    FROM     dbo.StockTransaction ST
    INNER JOIN dbo.[TransactionProtfolio] SP
        ON  SP.TransactionId = ST.Id
    WHERE    ST.AccountId = @AccountId 
         AND ST.BrokerId  = @BrokerId 
         AND ST.ScriptId  = @ScriptId
         AND ST.[Date]    >= @MinDateTime  

    DELETE 
    FROM        [dbo].[TransactionProtfolio]
    WHERE       TransactionId IN (
       SELECT   Id 
       FROM     dbo.StockTransaction 
       WHERE    AccountId = @AccountId 
            AND BrokerId  = @BrokerId 
            AND ScriptId  = @ScriptId
            AND [Date]    >= @DateTime
    );

    DELETE 
    FROM        [dbo].[Portfolio]
    WHERE       AccountId = @AccountId 
            AND BrokerId  = @BrokerId  
            AND ScriptId  = @ScriptId
            AND Id  IN  (SELECT T.ProtfolioId FROM @ProtfolioIds T)
            AND Id NOT IN (
                SELECT      TP.ProtfolioId
                FROM        [dbo].[TransactionProtfolio] TP
                WHERE       TP.TransactionId IN (
                               SELECT   ST.Id 
                               FROM     dbo.StockTransaction ST
                               WHERE    ST.AccountId = @AccountId 
                                    AND ST.BrokerId  = @BrokerId 
                                    AND ST.ScriptId  = @ScriptId
                                                )
                            )

    -- Below is reverse condition means, If @TransactionTypeId is buy then sell = 0 vice versa...
    UPDATE      [dbo].[Portfolio]
    SET         [BuyQty]        = CASE @TransactionTypeId WHEN 3002 THEN 0 ELSE [BuyQty]               END,
                [BuyPrice]      = CASE @TransactionTypeId WHEN 3002 THEN 0 ELSE [BuyPrice]             END,
                [BuyBrokerage]  = CASE @TransactionTypeId WHEN 3002 THEN 0 ELSE [BuyBrokerage]         END,
                [BuyAmount]     = CASE @TransactionTypeId WHEN 3002 THEN 0 ELSE [BuyAmount]            END,
                [SellQty]       = CASE @TransactionTypeId WHEN 3001 THEN 0 ELSE [SellQty]              END,
                [SellPrice]     = CASE @TransactionTypeId WHEN 3001 THEN 0 ELSE [SellPrice]            END,
                [SellBrokerage] = CASE @TransactionTypeId WHEN 3001 THEN 0 ELSE [SellBrokerage]        END,
                [SellAmount]    = CASE @TransactionTypeId WHEN 3001 THEN 0 ELSE [SellAmount]           END
    WHERE       AccountId = @AccountId 
            AND BrokerId  = @BrokerId  
            AND ScriptId  = @ScriptId
            AND Id  IN  (SELECT T.ProtfolioId FROM @ProtfolioIds T)
            AND Id  IN (
                SELECT      TP.ProtfolioId
                FROM        [dbo].[TransactionProtfolio] TP
                WHERE       TP.TransactionId IN (
                               SELECT   ST.Id 
                               FROM     dbo.StockTransaction ST
                               WHERE    ST.AccountId = @AccountId 
                                    AND ST.BrokerId  = @BrokerId 
                                    AND ST.ScriptId  = @ScriptId
                                                )
                            )


    IF @IsDelete = 1
        DELETE 
		    FROM	[dbo].[StockTransaction]
		    WHERE	Id		=	@TransactionId	

    DECLARE @Table TABLE (
            [Index]             INT,
            Id                  INT,
            [Date]				DATETIME,
            TransactionTypeId	INT,
            Qty				    SMALLINT,
            Price				FLOAT,
            Brokerage			FLOAT,
            Buy				    FLOAT,
            Sell				FLOAT,
            PmsId               INT
    )

    INSERT INTO @Table                              ([Index],    [Id],    [Date],		[TransactionTypeId],	[Qty],		[Price],		[Brokerage],		[Buy],		[Sell],     [PmsId])
    SELECT      ROW_NUMBER() OVER(ORDER BY Id ASC) AS [Index],  [Id],    [Date],		[TransactionTypeId],	[Qty],		[Price],		[Brokerage],		[Buy],		[Sell],     [PmsId]
    FROM        [dbo].[StockTransaction]
    WHERE       AccountId           = @AccountId 
            AND BrokerId            = @BrokerId
            AND ScriptId            = @ScriptId
            AND TransactionTypeId   IN (3001, 3002)
            AND [Date]              >= @DateTime
           

    DECLARE @Index              INT = NULL,
            @Id                 INT,
            @Date				DATETIME,
            @Qty				SMALLINT,
            @Price				FLOAT,
            @Brokerage			FLOAT,
            @Buy				FLOAT,
            @Sell				FLOAT,
            @PmsId              INT

    
    SELECT      @Index              = [Index],  
                @ID                 = [Id],    
                @Date               = [Date],		
                @TransactionTypeId  = [TransactionTypeId],	
                @Qty                = [Qty],		
                @Price              = [Price],		
                @Brokerage          = [Brokerage],		
                @Buy                = [Buy],		
                @Sell               = [Sell],
                @PmsId              = [PmsId]
    FROM        @Table 
    WHERE       [Index]             = 1
    
    WHILE (@Index IS NOT NULL)
    BEGIN
        --TODO YOUR PROCESS
        EXEC [dbo].[Portfolio_Insert_Update_Inner]      @Id 
													   , @Date	
													   , @AccountId		
													   , @TransactionTypeId
													   , @ScriptId		
													   , @Qty			
													   , @Price			
													   , @BrokerId		
													   , @Brokerage		
													   , @Buy			
													   , @Sell
                                                       , @PmsId

        

        DECLARE @NextIndex INT = @Index + 1
        SET @Index = NULL
        SELECT      @Index              = [Index],  
                    @ID                 = [Id],    
                    @Date               = [Date],		
                    @TransactionTypeId  = [TransactionTypeId],	
                    @Qty                = [Qty],		
                    @Price              = [Price],		
                    @Brokerage           = [Brokerage],		
                    @Buy                = [Buy],		
                    @Sell               = [Sell]
        FROM        @Table 
        WHERE       [Index]             = @NextIndex
    END
END
