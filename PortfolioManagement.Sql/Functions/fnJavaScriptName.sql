CREATE FUNCTION [dbo].[fnJavaScriptName]
(@string varchar(500))
RETURNS varchar(500)
BEGIN
	DECLARE @finalString varchar(500)
	SELECT @finalString = LOWER(SUBSTRING(@string,1,1)) + SUBSTRING(@string, 2, LEN(@string))	
	RETURN @finalString
END
