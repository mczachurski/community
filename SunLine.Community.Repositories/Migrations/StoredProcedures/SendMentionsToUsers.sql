IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SendMentionsToUsers')
DROP PROCEDURE [dbo].[SendMentionsToUsers]
GO

CREATE PROCEDURE [dbo].[SendMentionsToUsers]
    @messageId [uniqueidentifier],
	@isMentionInComment BIT,
	@mentionedUserNames varchar(max)
AS
BEGIN
    
    IF @messageId IS NULL
    BEGIN
		RETURN
    END
    
    IF NOT EXISTS (SELECT 1 FROM [Messages] WHERE [Id] = @messageId)
    BEGIN
		RETURN
    END

	DECLARE @users AS TABLE (userId [uniqueidentifier])
	INSERT INTO @users
		SELECT [Id] FROM [Users] 
		WHERE 
			[UserName] IN (SELECT Item FROM [dbo].[SplitStrings](@mentionedUserNames, '|')) -- All mentioned users.
			AND [Id] != (SELECT [User_Id] FROM [Messages] WHERE [Id] = @messageId)			-- Without author of message.

	IF (SELECT COUNT(1) FROM @users) = 0
	BEGIN
		RETURN
	END

	DECLARE @utcDateNow DATETIME = GETUTCDATE()

	UPDATE 
		[UserMessages] 
	SET 
		[HaveMention] = 1,
		[HaveMentionInComments] = @isMentionInComment,
		[Version] = [Version] + 1,
		[ModificationDate] = @utcDateNow
	WHERE
		[Message_Id] = @messageId
		AND [User_Id] IN (SELECT [userId] FROM @users)
		

	DECLARE @unreadStateId UNIQUEIDENTIFIER
    SET @unreadStateId = (SELECT [Id] FROM [UserMessageStates] WHERE [UserMessageStateEnum] = 1)
    
    DECLARE @byNotObserved UNIQUEIDENTIFIER 
    SET @byNotObserved = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 4)

	INSERT INTO [UserMessages]
    (
    	IsMarkerSet, 
    	HaveMention, 
    	WasTransmitted, 
    	Version, 
    	CreationDate, 
    	ModificationDate, 
    	Message_Id, 
    	TransmittedUserMessage_Id,
    	User_Id,
    	UserMessageCreationMode_Id,
    	UserMessageState_Id,
    	HaveMentionInComments,
    	SortingDate,
    	UpdateSortingDateOnNewComment 
    )
	SELECT DISTINCT 
    	0, 
    	1, 
    	0, 
    	1, 
    	@utcDateNow, 
    	NULL, 
    	@messageId, 
    	NULL, 
    	[u].[userId] , 
    	@byNotObserved,
    	@unreadStateId,
    	@isMentionInComment,
    	@utcDateNow,
		0
	FROM @users [u]
		LEFT JOIN [UserMessages] [um] ON [um].[Message_Id] = @messageId AND [um].[User_Id] = [u].[userId]
	WHERE 
		[um].[Id] IS NULL

END