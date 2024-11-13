CREATE PROCEDURE [dbo].[User_Registration]
    @FirstName          nvarchar(50), 
    @MiddleName         nvarchar(50)    = NULL, 
    @LastName           nvarchar(50), 
    @RoleId             smallint        = NULL,
    @Email              nvarchar(250),
    @BirthDate          DATETIME        = NULL,
    @Gender             INT             = NULL,
    @PhoneNumber        VARCHAR(15)     = NULL, 
    @Username           nvarchar(50), 
    @Password           varchar(max), 
    @PasswordSalt       varchar(max),
    @ImageSrc           VARCHAR(MAX)    = NULL,
    @IsActive           BIT             = NULL,
    @LastUpdateDateTime DATETIME        = NULL,
    @Type               varchar(100),   
    @PmsName			varchar(100)    = NULL
AS
BEGIN

    DECLARE @Id BIGINT;
	DECLARE @PmsId INT;

    IF NOT EXISTS(SELECT [Id] FROM [User] WHERE [FirstName] = @FirstName AND [LastName] = @LastName OR ([Username] = @Username OR [Email] = @Email))
    BEGIN
		
		INSERT INTO dbo.[PMS] (Name, Type)
            VALUES (@PmsName, @Type);

        SET @PmsId = SCOPE_IDENTITY();
		
        INSERT INTO [User] (
            [FirstName], 
            [MiddleName], 
            [LastName], 
            [RoleId], 
            [Email], 
            [PhoneNumber], 
            [Username], 
            [Password], 
            [PasswordSalt], 
            [IsActive], 
            [LastUpdateDateTime], 
            [BirthDate], 
            [ImageSrc], 
            [Gender],
            [PmsId]
        ) 
        VALUES (
            @FirstName, 
            @MiddleName, 
            @LastName, 
            @RoleId, 
            @Email, 
            @PhoneNumber, 
            @Username, 
            @Password, 
            @PasswordSalt, 
            @IsActive, 
            @LastUpdateDateTime, 
            @BirthDate, 
            @ImageSrc, 
            @Gender,
            @PmsId
        );
        
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SET @Id = 0;
    END

    SELECT @Id;
END
