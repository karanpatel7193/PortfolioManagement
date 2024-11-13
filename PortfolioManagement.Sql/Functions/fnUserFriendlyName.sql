CREATE FUNCTION [dbo].[fnUserFriendlyName]
(@string varchar(500))
RETURNS varchar(500)
BEGIN
	DECLARE @position int, @finalString varchar(500), @prevChar char(1);
	-- Initialize the variables.
	SET @position = 1;
	SET @string = REPLACE(@string, '_', ' ');
	SET @prevChar = UPPER(SUBSTRING(@string, @position, 1));
	SET @finalString = @prevChar;
	SET @position = @position + 1;

	WHILE @position <= DATALENGTH(@string)
	BEGIN
		IF(((ASCII(SUBSTRING(@string, @position, 1)) BETWEEN 65 AND 90) AND (ASCII(@prevChar) NOT BETWEEN 65 AND 90)) OR ((ASCII(SUBSTRING(@string, @position, 1)) BETWEEN 48 AND 57) AND (ASCII(@prevChar) NOT BETWEEN 48 AND 57)))
			SET @finalString = @finalString + ' ' + SUBSTRING(@string, @position, 1);
		ELSE
			SET @finalString = @finalString + SUBSTRING(@string, @position, 1);
		
		SET @prevChar = SUBSTRING(@string, @position, 1);
		SET @position = @position + 1
	END;
	RETURN @finalString
END
