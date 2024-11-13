CREATE PROCEDURE [dbo].[MasterValue_SelectForCache]
AS
/***********************************************************************************************
	 NAME     :  MasterValue_Select
	 PURPOSE  :  This SP select records from table MasterValue
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        03/23/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT MVL.[MasterId], MVL.[Value], MVL.[ValueText] 
	FROM [MasterValue] MVL
END

