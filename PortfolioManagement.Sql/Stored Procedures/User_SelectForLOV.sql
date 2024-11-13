CREATE PROCEDURE [dbo].[User_SelectForLOV]
AS
/***********************************************************************************************
	 NAME     :  User_SelectForLOV
	 PURPOSE  :  This SP select records from table User for fill LOV
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT U.[Id], U.[FirstName], U.[LastName], U.[Email]
	FROM [User] U WITH (NOLOCK)
END

