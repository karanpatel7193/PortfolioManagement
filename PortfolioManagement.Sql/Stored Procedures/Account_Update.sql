CREATE PROCEDURE [dbo].[Account_Update]
    @Id TINYINT,
    @Name VARCHAR(20),
    @BrokerIds XML = NULL  -- XML parameter for broker IDs
AS
BEGIN
    BEGIN TRANSACTION AccountUpdate
    BEGIN TRY
        -- Check if an account with the same name exists, excluding the current account
        IF NOT EXISTS (SELECT [Id] FROM [dbo].[Account] WHERE [Name] = @Name AND [Id] != @Id)
        BEGIN
            -- Update the Account table
            UPDATE [dbo].[Account]
            SET [Name] = @Name
            WHERE [Id] = @Id;

            -- Process the BrokerIds XML
            DECLARE @DocHandle INT;
            EXEC sp_xml_preparedocument @DocHandle OUTPUT, @BrokerIds;

            -- Remove existing broker associations for this account
            DELETE FROM [dbo].[AccountBroker]
            WHERE [AccountId] = @Id
                AND [BrokerId] NOT IN (
                    SELECT TMP.[Id] 
                    FROM OPENXML (@DocHandle, '/ArrayOfAccountBrokerSelectEntity/AccountBrokerSelectEntity', 2)
                    WITH ([Id] TINYINT, [IsSelected] BIT) AS TMP
                    WHERE TMP.[IsSelected] = 1
                );

            -- Insert new broker associations
            INSERT INTO [dbo].[AccountBroker] ([AccountId], [BrokerId])
            SELECT @Id, TMP.[Id]
            FROM OPENXML (@DocHandle, '/ArrayOfAccountBrokerSelectEntity/AccountBrokerSelectEntity', 2)
            WITH ([Id] TINYINT, [IsSelected] BIT) AS TMP
            WHERE TMP.[IsSelected] = 1
                    AND TMP.[Id] NOT IN (
                        SELECT BrokerId
                        FROM dbo.AccountBroker
                        WHERE [AccountId] = @Id
                    );

            -- Clean up the XML document
            EXEC sp_xml_removedocument @DocHandle;
        END
        ELSE
            SET @Id = 0;

        -- Return the account ID
        SELECT @Id;

        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION AccountUpdate;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AccountUpdate;

        -- Handle XML document cleanup if an error occurs
        IF @DocHandle IS NOT NULL
            EXEC sp_xml_removedocument @DocHandle;

        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorNumber);
    END CATCH
END


---- Example XML data for BrokerIds
--DECLARE @BrokerIds XML = '
--<Brokers>
--    <Broker>
--        <BrokerId>1</BrokerId>
--    </Broker>
--    <Broker>
--        <BrokerId>2</BrokerId>
--    </Broker>
--</Brokers>';

---- Execute the stored procedure
--EXEC dbo.Account_Update
--    @Id = 1,  -- Replace with the actual Account Id you want to update
--    @Name = 'Updated Account Name',  -- Replace with the new account name
--    @BrokerIds = @BrokerIds;  -- XML parameter for broker IDs
