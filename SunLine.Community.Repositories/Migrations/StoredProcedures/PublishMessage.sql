IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PublishMessage')
DROP PROCEDURE [dbo].[PublishMessage]
GO

CREATE PROCEDURE [dbo].[PublishMessage]
    @messageId [uniqueidentifier],
	@userNameToReply varchar(100),
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

	DECLARE @utcDateNow DATETIME = GETUTCDATE()
	DECLARE @quotedMessageId [uniqueidentifier]
	DECLARE @commentedMessageId [uniqueidentifier]

	DECLARE @publishedState UNIQUEIDENTIFIER 
	SET @publishedState = (SELECT [Id] FROM [MessageStates] WHERE [MessageStateEnum] = 2)

	UPDATE 
		[Messages]
	SET
		[MessageState_Id] = @publishedState,
		[Version] = [Version] + 1,
		[ModificationDate] = @utcDateNow
	WHERE
		[Id] = @messageId

	SELECT 
		@quotedMessageId = QuotedMessage_Id, 
		@commentedMessageId = CommentedMessage_Id 
	FROM [Messages] 
	WHERE [Id] = @messageId

	IF @quotedMessageId IS NOT NULL
	BEGIN

		UPDATE 
			[Messages] 
		SET 
			[AmountOfQuotes] = [AmountOfQuotes] + 1,
			[Version] = [Version] + 1,
			[ModificationDate] = @utcDateNow
		WHERE 
			[Id] = @quotedMessageId

	END
    
	IF @commentedMessageId IS NOT NULL
	BEGIN
		
		UPDATE 
			[Messages] 
		SET 
			[AmountOfComments] = [AmountOfComments] + 1,
			[Version] = [Version] + 1,
			[ModificationDate] = @utcDateNow
		WHERE 
			[Id] = @commentedMessageId

		UPDATE 
			[UserMessages] 
		SET 
			[SortingDate] = @utcDateNow,
			[Version] = [Version] + 1,
			[ModificationDate] = @utcDateNow
		WHERE 
			[Message_Id] = @commentedMessageId 
			AND [UpdateSortingDateOnNewComment] = 1

		EXEC [dbo].[SendMentionsToUsers] @commentedMessageId, 1, @mentionedUserNames
	END
	ELSE
	BEGIN

		EXEC [dbo].[SendMessageToObservers] @messageId, @userNameToReply
		EXEC [dbo].[SendMentionsToUsers] @messageId, 0, @mentionedUserNames

	END
END