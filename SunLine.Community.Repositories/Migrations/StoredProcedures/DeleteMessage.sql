IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteMessage')
DROP PROCEDURE [dbo].[DeleteMessage]
GO

CREATE PROCEDURE [dbo].[DeleteMessage]
    @messageId [uniqueidentifier]
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

	DECLARE @deletedUserMessageState UNIQUEIDENTIFIER 
	SET @deletedUserMessageState = (SELECT [Id] FROM [UserMessageStates] WHERE [UserMessageStateEnum] = 3)

	UPDATE [UserMessages] SET [UserMessageState_Id] = @deletedUserMessageState WHERE [Message_Id] = @messageId

	DECLARE @deletedMessageState UNIQUEIDENTIFIER 
	SET @deletedMessageState = (SELECT [Id] FROM [MessageStates] WHERE [MessageStateEnum] = 3)
	
	UPDATE [Messages] SET [MessageState_Id] = @deletedMessageState WHERE [Id] = @messageId
	
	DECLARE @quotedMessageId UNIQUEIDENTIFIER
	SET @quotedMessageId = (SELECT [QuotedMessage_Id] FROM [Messages] WHERE [Id] = @messageId)
	
	IF @quotedMessageId IS NOT NULL
	BEGIN
	
		UPDATE [Messages] SET [AmountOfQuotes] = [AmountOfQuotes] - 1 WHERE [Id] = @quotedMessageId
		
	END
	
END