CREATE PROCEDURE [dbo].[Role_SelectForRecord]
	@Id smallint
AS
/***********************************************************************************************
	 NAME     :  Role_SelectForRecord
	 PURPOSE  :  This SP select perticular record from table Role for open page in edit / view mode.
	 REVISIONS:
	 Ver        Date				        Author              Description
	 ---------  ----------					---------------		-------------------------------
	 1.0        02/03/2018					Rekansh Patel             1. Initial Version.	 
************************************************************************************************/
BEGIN
	SELECT R.[Id], R.[Name], R.IsPublic
	FROM [Role] R
	WHERE R.[Id] = @Id;
END

