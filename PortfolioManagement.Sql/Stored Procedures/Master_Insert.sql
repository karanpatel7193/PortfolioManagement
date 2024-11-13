CREATE PROCEDURE [dbo].[Master_Insert]
    @Id         SMALLINT,
    @Type       VARCHAR(20),
    @MasterValues XML -- XML parameter for multiple MasterValue records
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Insert into Master table
        INSERT INTO [dbo].[Master] ([Id], [Type])
        VALUES (@Id, @Type);

        -- Process the MasterValues XML
        DECLARE @DocHandle INT;
        EXEC sp_xml_preparedocument @DocHandle OUTPUT, @MasterValues;

        -- Insert data from the XML into MasterValue table
        INSERT INTO [dbo].[MasterValue] ([MasterId], [Value], [ValueText])
        SELECT @Id, TMP.[Value], TMP.[ValueText]
        FROM OPENXML (@DocHandle, '/ArrayOfMasterValueEntity/MasterValueEntity', 2)
        WITH ([Value] INT, [ValueText] NVARCHAR(250)) AS TMP;

        EXEC sp_xml_removedocument @DocHandle;

        -- Return the newly created Id
        SELECT @Id AS MasterId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        IF @DocHandle IS NOT NULL
            EXEC sp_xml_removedocument @DocHandle;

        -- Re-throw the error
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
