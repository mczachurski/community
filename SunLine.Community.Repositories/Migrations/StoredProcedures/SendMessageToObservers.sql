IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SendMessageToObservers')
DROP PROCEDURE [dbo].[SendMessageToObservers]
GO

CREATE PROCEDURE [dbo].[SendMessageToObservers]
    @messageId [uniqueidentifier],
    @userNameToReply varchar(100)
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
    
    DECLARE @messageUserId UNIQUEIDENTIFIER
    SET @messageUserId = (SELECT [User_Id] FROM [Messages] WHERE [Id] = @messageId)
    
    -- User can publish message only once.
    IF EXISTS (SELECT 1 FROM [UserMessages] WHERE [Message_Id] = @messageId AND [User_Id] = @messageUserId)
    BEGIN
    	RETURN
    END
    
    DECLARE @utcDateNow DATETIME = GETUTCDATE()
    
    DECLARE @unreadStateId UNIQUEIDENTIFIER
    SET @unreadStateId = (SELECT [Id] FROM [UserMessageStates] WHERE [UserMessageStateEnum] = 1)
    
    DECLARE @byHimselfId UNIQUEIDENTIFIER 
    SET @byHimselfId = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 1)
    
    DECLARE @byObservedId UNIQUEIDENTIFIER 
    SET @byObservedId = (SELECT [Id] FROM [UserMessageCreationModes] WHERE [UserMessageCreationModeEnum] = 2)
    
    -- Add new user messages to author of message.
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
    VALUES
    (
    	0,
    	0,
    	0,
    	1,
    	@utcDateNow,
    	NULL,
    	@messageId,
    	NULL,
    	@messageUserId,
    	@byHimselfId,
    	@unreadStateId,
    	0,
    	@utcDateNow,
    	0
    )
    
    -- Add users to observers (if it's reply to specyfic user we have to add message only to mutual observers)
    IF @userNameToReply IS NULL
    BEGIN
    
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
    		NULL, 
    		[FromUser_Id], 
    		@byObservedId,
    		@unreadStateId,
    		0,
    		@utcDateNow,
    		0
    	FROM [UserConnections] 
    	WHERE 
    		[ToUser_Id] = @messageUserId
    		AND NOT EXISTS (SELECT 1 FROM [UserMessages] WHERE [UserMessages].[Message_Id] = @messageId AND [UserMessages].[User_Id] = [UserConnections].[FromUser_Id])
    
    END
    ELSE
    BEGIN
    
		DECLARE @userToReplyId UNIQUEIDENTIFIER
		SET @userToReplyId = (SELECT [Id] FROM [Users] WHERE [UserName] = @userNameToReply)

    	CREATE TABLE #MutualObservers (User_Id [uniqueidentifier])
    
		-- Choosing mutual observers
    	INSERT INTO #MutualObservers
    		SELECT DISTINCT [c1].[FromUser_Id] FROM [UserConnections] [c1]
    			INNER JOIN [UserConnections] [c2] ON [c1].[FromUser_Id] = [c2].[FromUser_Id]
    			WHERE 
    				[c1].[ToUser_Id] = @messageUserId 
    				AND [c2].[ToUser_Id] = @userToReplyId
    
    	-- We have to send message also to user which we reply when he is observer also
    	IF EXISTS (SELECT 1 FROM [UserConnections] WHERE [FromUser_Id] = @userToReplyId AND [ToUser_Id] = @messageUserId)
    	BEGIN
    		INSERT INTO #MutualObservers VALUES (@userToReplyId)
    	END
    
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
    		NULL, 
    		[User_Id], 
    		@byObservedId,
    		@unreadStateId,
    		0,
    		@utcDateNow,
    		0
    	FROM #MutualObservers 
    	WHERE 
    		NOT EXISTS (SELECT 1 FROM [UserMessages] WHERE [UserMessages].[Message_Id] = @messageId AND [UserMessages].[User_Id] = #MutualObservers.[User_Id])
    
    END
END