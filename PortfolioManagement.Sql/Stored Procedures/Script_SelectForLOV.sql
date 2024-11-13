CREATE PROCEDURE [dbo].[Script_SelectForLOV]
	@Id smallint = NULL, 
	@Name varchar(30)  = NULL
AS
/***********************************************************************************************
	 NAME     :  Script_SelectForLOV
	 PURPOSE  :  This SP select records from table Script for fill LOV
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/09/2024					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT S.[Id], CONVERT(VARCHAR, S.[BseCode]) + ' | ' + S.[NseCode]  + ' | ' + S.[Name] + ' | ' + S.[ISINCode] AS [Name]
	FROM [Script] S
	WHERE S.[Id] = COALESCE(@Id, S.[Id])
	 AND S.[Name] LIKE COALESCE(@Name, S.[Name]) + '%'
	 AND CONVERT(VARCHAR, S.[BseCode]) LIKE COALESCE(@Name, CONVERT(VARCHAR, S.[BseCode])) + '%'
	 AND CONVERT(VARCHAR,S.[NseCode]) LIKE COALESCE(@Name, CONVERT(VARCHAR,S.[NseCode])) + '%'
     AND CONVERT(VARCHAR,S.[ISINCode]) LIKE COALESCE(@Name, CONVERT(VARCHAR,S.[ISINCode])) + '%'
	ORDER BY S.[Name]	
END		
	