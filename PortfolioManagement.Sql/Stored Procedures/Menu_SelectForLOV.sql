
CREATE   PROCEDURE [dbo].[Menu_SelectForLOV]
    @ParentId       INT = NULL,
    @IsOnlyParent   BIT = NULL
AS
BEGIN
    SELECT 
        M.[Id], 
        M.[Name]
    FROM 
        [Menu] M
   WHERE
		(@IsOnlyParent = 1 AND M.[ParentId] IS NULL) 
        OR (@IsOnlyParent IS NULL OR @IsOnlyParent = 0) 
		AND (@ParentId IS NULL OR M.[ParentId] = @ParentId)

END

