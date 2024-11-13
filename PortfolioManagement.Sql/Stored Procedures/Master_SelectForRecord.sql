CREATE PROCEDURE [dbo].[Master_SelectForRecord]
    @Id TINYINT
AS
BEGIN
    SELECT  M.[Id], M.[Type]
    FROM    [dbo].[Master] M
    WHERE   M.[Id] = @Id;

    SELECT  MV.[MasterId],  MV.[Value], MV.[ValueText]
    FROM    [dbo].[MasterValue] MV
    WHERE   MV.[MasterId] = @Id

END