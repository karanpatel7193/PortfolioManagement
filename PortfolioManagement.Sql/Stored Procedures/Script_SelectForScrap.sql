CREATE PROCEDURE [dbo].[Script_SelectForScrap]
AS
/***********************************************************************************************
	 NAME     :  Script_SelectForScrap
	 PURPOSE  :  This SP select records from table Script for scrap data.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        10/30/2020					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT	S.[Id], S.[Name], S.[BseCode], S.[NseCode], S.[ISINCode], S.[MoneyControlURL], S.[FetchURL]
	FROM	[Script] S
	WHERE	S.[IsActive] = 1
		AND	S.[IsFetch] = 1
END