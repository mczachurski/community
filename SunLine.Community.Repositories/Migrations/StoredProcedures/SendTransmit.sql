IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SendTransmit')
DROP PROCEDURE [dbo].[SendTransmit]
GO

CREATE PROCEDURE [dbo].[SendTransmit]
    @messageId [uniqueidentifier],
    @userId [uniqueidentifier]
AS
BEGIN
    
    IF @messageId IS NULL
    BEGIN
		RETURN
    END
    
    IF @userId IS NULL
    BEGIN
		RETURN
    END
    
    IF NOT EXISTS (SELECT 1 FROM [Messages] WHERE [Id] = @messageId)
    BEGIN
		RETURN
    END
    
    IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Id] = @userId)
    BEGIN
		RETURN
    END
    
    DECLARE @messageUserId UNIQUEIDENTIFIER
    SET @messageUserId = (SELECT [User_Id] FROM [Messages] WHERE [Id] = @messageId)
    
    DECLARE @utcDateNow DATETIME = GETUTCDATE()
    
    DECLARE @unreadStateId UNIQUEIDENTIFIER
    SET @unreadStateId = (SELECT [Id] FROM [UserMessageStates] WHERE [UserMessageStateEnum] = 1)
    
    DECLARE @transmittedUserMessageId UNIQUEIDENTIFIER
    
    -- User can transmit message only once. 
    IF EXISTS (SELECT 1 FROM [UserMessages] WHERE [Message_Id] = @messageId AND [User_Id] = @userId AND [WasTransmitted] = 1)
    BEGIN
		RETURN
    END
    
    SET @transmittedUserMessageId = (SELECT [Id] FROM [UserMessages] WHERE [Message_Id] = @messageId AND [User_Id] = @userId)
    
    IF @transmittedUserMessageId IS NOT NULL
    BEGIN
    
		-- When exists we must set information that message was transmitted.
		UPDATE 
			[UserMessages] 
		SET 
			[WasTransmitted] = 1,
			[Version] = [Version] + 1,
			[ModificationDate] = @utcDateNow
		WHERE 
			[Id] = @transmittedUserMessageId
    
    END
    ELSE
    BEGIN
    
		DECLARE @ids TABLE([Id] UNIQUEIDENTIFIER ) 
    
		DECLARE @byHimselfTransmitedFromUnknownFeed UNIQUEIDENTIFIER 
		SET @byHimselfTransmitedFromUnknownFeed = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 7)
    
		-- When user transmiting message not on his own feed, we must create message on his feed first.
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
		OUTPUT inserted.Id INTO @ids
		VALUES
		(
			0, 
			0, 
			1, 
			1, 
			@utcDateNow, 
			NULL, 
			@messageId, 
			NULL, 
			@userId, 
			@byHimselfTransmitedFromUnknownFeed,
			@unreadStateId,
			0,
			@utcDateNow,
			0
		)
    
		SET @transmittedUserMessageId = (SELECT TOP 1 [Id] FROM @ids)
    
    END
    
    
    IF @transmittedUserMessageId IS NULL
    BEGIN
		RETURN
    END
    
    DECLARE @byObservedTransmitCreationModeId UNIQUEIDENTIFIER 
    SET @byObservedTransmitCreationModeId = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 3)
    
    DECLARE @byNotObserverCreationModeId UNIQUEIDENTIFIER 
    SET @byNotObserverCreationModeId = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 4)
    
    -- Update user message, that people see only by mentions (now they will see message on his feeds).
    UPDATE 
		[UserMessages] 
	SET 
		[UserMessageCreationMode_Id] = @byObservedTransmitCreationModeId, 
		[TransmittedUserMessage_Id] = @transmittedUserMessageId,
		[Version] = [Version] + 1,
		[ModificationDate] = @utcDateNow
    WHERE 
		[Message_Id] = @messageId 
		AND [User_Id] IN (SELECT [FromUser_Id] FROM [UserConnections] WHERE [ToUser_Id] = @userId) 
		AND [HaveMention] = 1 
		AND [UserMessageCreationMode_Id] = @byNotObserverCreationModeId
    
    
    -- Add new user messages to all other observers.
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
    SELECT 
		0, 
		0, 
		0, 
		1, 
		@utcDateNow, 
		NULL, 
		@messageId, 
		@transmittedUserMessageId, 
		[FromUser_Id], 
		@byObservedTransmitCreationModeId,
		@unreadStateId,
		0,
		@utcDateNow,
		0
    FROM [UserConnections] 
    WHERE 
		[ToUser_Id] = @userId
		AND NOT EXISTS (SELECT 1 FROM [UserMessages] WHERE [UserMessages].[Message_Id] = @messageId AND [UserMessages].[User_Id] = [UserConnections].[FromUser_Id])
    
    -- Now we must increase amount of trasnsmits on message.
    UPDATE 
		[Messages] 
    SET 
		[AmountOfTransmitted] = [AmountOfTransmitted] + 1,
		[Version] = [Version] + 1,
		[ModificationDate] = @utcDateNow
    WHERE [Id] = @messageId
END