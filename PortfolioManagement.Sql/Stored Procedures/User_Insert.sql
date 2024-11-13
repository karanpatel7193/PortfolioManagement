CREATE PROCEDURE [dbo].[User_Insert]
    @FirstName          nvarchar(50), 
    @MiddleName         nvarchar(50)    = NULL, 
    @LastName           nvarchar(50), 
    @RoleId             smallint,
    @Email              nvarchar(250),
    @BirthDate          DATETIME        = NULL,
    @Gender             INT,
    @PhoneNumber        VARCHAR(15), 
    @Username           nvarchar(50), 
    @Password           varchar(max), 
    @PasswordSalt       varchar(max),
    @ImageSrc           VARCHAR(MAX)    = NULL,
    @IsActive           BIT,
    @LastUpdateDateTime DATETIME        = NULL,
    @PmsId              BIGINT 
AS
BEGIN

    DECLARE @Id BIGINT;

    IF NOT EXISTS(SELECT [Id] FROM [User] WHERE ([Username] = @Username OR [Email] = @Email))
    BEGIN
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
