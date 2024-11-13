CREATE PROCEDURE [dbo].[SplitBonus_Delete]
    @Id INT
AS

    BEGIN
        DELETE FROM [SplitBonus]
        WHERE [Id] = @Id;
    END

