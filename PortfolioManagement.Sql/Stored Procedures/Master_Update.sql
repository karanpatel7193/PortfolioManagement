CREATE PROCEDURE [dbo].[Master_Update]
    @Id         SMALLINT,
    @Type       VARCHAR(20),
    @MasterValues XML = NULL  -- XML parameter for MasterValues
AS
BEGIN
    BEGIN TRANSACTION MasterUpdate
    BEGIN TRY
        IF NOT EXISTS (SELECT [Id] FROM [dbo].[Master] WHERE [Type] = @Type AND [Id] != @Id)
        BEGIN
            UPDATE [dbo].[Master]
            SET [Type] = @Type
            WHERE [Id] = @Id;

            DECLARE @DocHandle INT;
            EXEC sp_xml_preparedocument @DocHandle OUTPUT, @MasterValues;

            DELETE FROM [dbo].[MasterValue]
            WHERE [MasterId] = @Id
                AND [Value] NOT IN (
                    SELECT TMP.[Value]
                    FROM OPENXML (@DocHandle, '/ArrayOfMasterValueEntity/MasterValueEntity', 2)
                    WITH ([Value] INT, [ValueText] NVARCHAR(250)) AS TMP
                );

            UPDATE [dbo].[MasterValue]
            SET [ValueText] = TMP.[ValueText]
            FROM [dbo].[MasterValue] MV
            JOIN OPENXML (@DocHandle, '/ArrayOfMasterValueEntity/MasterValueEntity', 2)
                WITH ([Value] INT, [ValueText] NVARCHAR(250)) AS TMP
                ON MV.[MasterId] = @Id
                AND MV.[Value] = TMP.[Value];

            INSERT INTO [dbo].[MasterValue] ([MasterId], [Value], [ValueText])
            SELECT @Id, TMP.[Value], TMP.[ValueText]
            FROM OPENXML (@DocHandle, '/ArrayOfMasterValueEntity/MasterValueEntity', 2)
            WITH ([Value] INT, [ValueText] NVARCHAR(250)) AS TMP
            WHERE TMP.[Value] NOT IN (
                SELECT [Value]
                FROM dbo.MasterValue
                WHERE [MasterId] = @Id
            );

            EXEC sp_xml_removedocument @DocHandle;
        END
        ELSE
        BEGIN
            SET @Id = 0;
        END

        SELECT @Id AS MasterId;

        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION MasterUpdate;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION MasterUpdate;

        IF @DocHandle IS NOT NULL
            EXEC sp_xml_removedocument @DocHandle;

        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT, @ErrorNumber INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(), @ErrorNumber = ERROR_NUMBER();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorNumber);
    END CATCH
END
