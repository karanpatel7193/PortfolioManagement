CREATE PROCEDURE [dbo].[PMS_Delete]
    @Id INT
AS
BEGIN
    -- Update the IsActive field to false (0) instead of deleting the record
    UPDATE [PMS]
    SET [IsActive] = 0
    WHERE [Id] = @Id;
END
