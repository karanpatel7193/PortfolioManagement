CREATE PROCEDURE [dbo].[Account_Insert]
    @Name VARCHAR(20),
	@PmsId INT,
    @BrokerIds XML
	-- XML parameter for broker IDs
AS
BEGIN
    BEGIN TRANSACTION AccountInsert
    BEGIN TRY 
        DECLARE @AccountId TINYINT

        -- Insert into Account table
        INSERT INTO [Account] ([Name],[PmsId])
        VALUES (@Name, @PmsId)
        SET @AccountId = SCOPE_IDENTITY();

        -- Process the BrokerIds XML
        DECLARE @DocHandle INT
        EXEC sp_xml_preparedocument @DocHandle OUTPUT, @BrokerIds

        -- Insert data from the XML into the AccountBroker table
        INSERT INTO [AccountBroker] ([AccountId], [BrokerId])
        SELECT @AccountId, TMP.[Id]
        FROM OPENXML (@DocHandle, '/ArrayOfAccountBrokerSelectEntity/AccountBrokerSelectEntity', 2)
        WITH ([Id] TINYINT, [IsSelected] BIT) AS TMP
        WHERE TMP.[IsSelected] = 1

        EXEC sp_xml_removedocument @DocHandle

        -- Return the newly created AccountId
        SELECT @AccountId AS AccountId;

        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION AccountInsert
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AccountInsert

            EXEC sp_xml_removedocument @DocHandle

        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE()
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END