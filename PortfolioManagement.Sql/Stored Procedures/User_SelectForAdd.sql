CREATE PROCEDURE [dbo].[User_SelectForAdd]
AS
/***********************************************************************************************
	 NAME     :  User_SelectForAdd
	 PURPOSE  :  This SP use for fill all LOV in User page for add mode
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/06/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	EXEC Role_SelectForLOV 

END

