SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.UserMessage = SunLine.Community.UserMessage || {};

SunLine.Community.UserMessage = function () {

    var settings = {
        TransmitUrl: null,
        FavouriteUrl: null,
        FavouriteCommentUrl: null,
        ToggleConnectionUrl: null,
        PopoverUrl: null,
        BubbleUrl: null,
        DeleteMessageUrl: null,
        FileUploadUrl: null,
        PublishMessageUrl: null,
        AntiForgeryToken: null
    };

    var dropzone = null;

    var init = function (options) {
        $.extend(settings, options);
        settings.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
        calculateMindCharCounter();
        createProfilePopovers();
    };

    var createProfilePopovers = function () {

        $('body').on('click', '.preview', function () {
            var e = $(this);
            if (e.data('loaded') == undefined || e.data('loaded') == false) {
                e.data('loaded', true);

                var url = settings.PopoverUrl + '/' + e.data('user-id');
                $.get(url, function (response) {
                    e.data('content', response);
                    e.popover('destroy');
                    e.popover({ html: true, trigger: 'focus', placement: 'left' }).popover('show');
                });
            }
        });

    };

    var rebuildActionsTooltips = function(messageId) {

    	$('#actions-' + messageId).find('[data-toggle="tooltip"]').each(function(i, el)
		{
			var $this = $(el),
				placement = attrDefault($this, 'placement', 'top'),
				trigger = attrDefault($this, 'trigger', 'hover'),
				tooltip_class = $this.get(0).className.match(/(tooltip-[a-z0-9]+)/i);

			$this.tooltip({
				placement: placement,
				trigger: trigger
			});

			if(tooltip_class)
			{
				$this.removeClass(tooltip_class[1]);

				$this.on('show.bs.tooltip', function(ev)
				{
					setTimeout(function()
					{
						var $tooltip = $this.next();
						$tooltip.addClass(tooltip_class[1]);

					}, 0);
				});
			}
		});

    };

    var sendTransmit = function (sender, messageId) {

    	webApp.ShowPageLoader();

        $.ajax({
            type: "POST",
            url: settings.TransmitUrl,
            data: { messageId: messageId, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {
                var mediaButtons = $(sender).closest(".story-options-links");
                mediaButtons.html(result.view);
                rebuildActionsTooltips(messageId);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while reporting a vehicle. Please try again.", null);
                }
            }

        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while send transmit. Please try again.", null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});
    };

    var messageIdToDelete = 0;
    var setMessageToDelete = function(messageId) {
    	messageIdToDelete = messageId;
    };

    var deleteMessage = function () {

        $("#delete-loader").css("display", "inline-block");
        $("#delete-btn").addClass('disabled');

        $.ajax({
            type: "POST",
            url: settings.DeleteMessageUrl,
            data: { messageId: messageIdToDelete, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {
                
                $('#delete-confirm').modal('hide');
                webApp.ShowMessage(true, "Mind was removed.", null);
				deleteMessageCallback(messageIdToDelete);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while reporting a vehicle. Please try again.", null);
                }
            }

        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while delete message. Please try again.", null);
  		})
  		.always(function() {
    		$("#delete-loader").css("display", "none");
	        $("#delete-btn").removeClass('disabled');
  		});
    };

    var bubble = function (sender, messageId) {

    	webApp.ShowPageLoader();

        $.ajax({
            type: "POST",
            url: settings.BubbleUrl,
            data: { messageId: messageId, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {
                var mediaButtons = $(sender).closest(".story-options-links");
                mediaButtons.html(result.view);
                rebuildActionsTooltips(messageId);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while reporting a vehicle. Please try again.", null);
                }
            }
        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while bubbling. Please try again.", null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});
    };

    var favourite = function (sender, messageId) {

    	webApp.ShowPageLoader();

        $.ajax({
            type: "POST",
            url: settings.FavouriteUrl,
            data: { messageId: messageId, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {
                var mediaButtons = $(sender).closest(".story-options-links");
                mediaButtons.html(result.view);
                rebuildActionsTooltips(messageId);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while reporting a vehicle. Please try again.", null);
                }
            }
        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while favourite message. Please try again.", null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});
    };

    var favouriteComment = function (sender, messageId) {

    	webApp.ShowPageLoader();

        $.ajax({
            type: "POST",
            url: settings.FavouriteCommentUrl,
            data: { messageId: messageId, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {

                var mediaButtons = $(sender).closest(".comment-options-links");
                mediaButtons.html(result.view);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while reporting a vehicle. Please try again.", null);
                }
            }

        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while favourite comment. Please try again.", null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});
    };

    var onBeginCreateComment = function (messageId) {
        webApp.ShowPageLoader();
        $("#reply-" + messageId).find(".btn").addClass("disabled");
    };

    var onCompleteCreateComment = function (messageId) {
        webApp.HidePageLoader();
        $("#reply-" + messageId).find(".btn").removeClass("disabled");
    };

    var onFailureCreateComment = function () {
        webApp.ShowMessage(false, "Upppss... An error occurred while adding comment. Please try again.", null);
    };

    var onSuccessCreateComment = function (result) {

        if (result.success) {

            if ($("#comments-" + result.commentedMessageId).length == 0) {
                $("#actions-" + result.commentedMessageId).before("<ul class='list-unstyled story-comments' id='comments-" + result.commentedMessageId + "'></ul>");
            }
            var comments = $("#comments-" + result.commentedMessageId);

            var $jQueryObject = $($.parseHTML(result.commentView));
            $jQueryObject.hide();
            comments.append($jQueryObject);
            $jQueryObject.slideDown();

            $("#reply-" + result.commentedMessageId).find("textarea").val("");
            var element = $("#reply-" + result.commentedMessageId);
            element.hide();

			var mediaButtons = $("#reply-" + result.commentedMessageId).parent().find(".story-options-links")
        	mediaButtons.html(result.commentedActionView);
          	rebuildActionsTooltips(result.commentedMessageId);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while adding comment. Please try again.", null);
            }
        }
    };

    var toggleCommentForm = function (replyId) {
        var element = $("#reply-" + replyId);
        element.slideToggle();
    };

    var onSuccessCreateMind = function (result) {

        if (result.success) {
            var timeline = $(".user-timeline-stories");

            var $jQueryObject = $($.parseHTML(result.view));
            $jQueryObject.hide();
            timeline.prepend($jQueryObject);
            $jQueryObject.slideDown();

            $("#Mind").val("");
            $("#mind-char-counter").html(200);

            $(".mind-file").hide();
            dropzone.removeAllFiles();
            $('input[name^="Files"]:hidden').remove();

            rebuildActionsTooltips(result.messageId);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while adding mind. Please try again.", null);
            }
        }
    };

    var onFailureCreateMind = function () {
        webApp.ShowMessage(false, "Upppss... An error occurred while adding mind. Please try again.", null);
    };

    var onBeginCreateMind = function () {
        webApp.ShowPageLoader();
        $("#mind-submit").addClass("disabled");
    };

    var onCompleteCreateMind = function () {
        webApp.HidePageLoader();
        $("#mind-submit").removeClass("disabled");
    };

    var calculateCharCounter = function(value) {
        var messageLength = value.length;

        var re = /(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?/g;
        var urlArray = value.match(re);

        if (urlArray && urlArray.length > 0) {
            for (var i = 0; i < urlArray.length; i++) {
                var url = urlArray[i];
                if (url.length > 21) {
                    messageLength = (messageLength - url.length) + 21;
                }
            }
        }

        return (200 - messageLength);
    };

    var calculateMindCharCounter = function () {
        $("#Mind").on("change keyup paste", function () {
            $("#mind-char-counter").html(calculateCharCounter(this.value));
        });

        $(".story-comment-text").on("change keyup paste", function () {
            $(this).parent().children(".char-counter").html(calculateCharCounter(this.value));
        });

        var quote = $("#quotes-modal").find("textarea[name=Mind]");
        if (quote.length > 0) {
            $(quote).on("change keyup paste", function () {
                $("#quotes-modal").find(".quote-char-counter").html(calculateCharCounter(this.value));
            });
        }
    };

    var sendCommentQuote = function (sender, messageId) {
        $("#quotes-modal").find("#quotedMessageId").val(messageId);
        var content = $(sender).closest(".story-comment").find(".story-comment-message").html();
        var user = $(sender).closest(".story-comment").find(".story-comment-user-name").html();
        $("#quotes-modal").find("#quoted-message").html(content);
        $("#quotes-modal").find("#quoted-user").html(user);
    };

    var sendMindQuote = function (sender, messageId) {
        $("#quotes-modal").find("#quotedMessageId").val(messageId);
        var content = $(sender).closest(".story-content").find(".story-mind").html();
        var user = $(sender).closest(".timeline-story").find(".story-user").html();
        $("#quotes-modal").find("#quoted-message").html(content);
        $("#quotes-modal").find("#quoted-user").html(user);
    };

    var onSuccessCreateQuote = function (result) {

        if (result.success) {
            var timeline = $(".user-timeline-stories");

            var $jQueryObject = $($.parseHTML(result.view));
            $jQueryObject.hide();
            timeline.prepend($jQueryObject);
            $jQueryObject.slideDown();

            $("#quotes-modal").find("textarea[name=Mind]").val("");
            $("#quotes-modal").find(".quote-char-counter").html(200);

            $("#quotes-modal").modal('hide');

            rebuildActionsTooltips(result.messageId);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while adding mind. Please try again.", null);
            }
        }
    };

    var onFailureCreateQuote = function () {
        webApp.ShowMessage(false, "Upppss... An error occurred while adding mind. Please try again.", null);
    };

    var onBeginCreateQuote = function () {
        $("#quotes-modal").find(".ajax-loader").css("display", "inline-block");
        $("#quotes-modal").find("button[type=submit]").addClass('disabled');
    };

    var onCompleteCreateQuote = function () {
        $("#quotes-modal").find(".ajax-loader").css("display", "none");
        $("#quotes-modal").find("button[type=submit]").removeClass('disabled');
    };

    var toggleConnection = function (sender, userId) {

    	webApp.ShowPageLoader();

        $.ajax({
            type: "POST",
            url: settings.ToggleConnectionUrl,
            data: { toUserId: userId, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {

                var avatars = $('.preview[data-user-id="' + userId + '"]');
                avatars.data('loaded', false);
                avatars.data('content', '');
                avatars.popover('destroy');

                webApp.ShowMessage(true, "Connection between users was modified.", null);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while change connection. Please try again.", null);
                }
            }

        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while toggle connection. Please try again.", null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});
    };

    var toggleImageDropzone = function() {
        $('.mind-file').toggle();
    };
        
    var initializeDropzone = function() {

        dropzone = new Dropzone(".mind-file", {
            url: settings.FileUploadUrl,
            maxFilesize: 5,
            addRemoveLinks: true,
            maxFiles: 4,
            acceptedFiles: "image/*"
        });

        dropzone.on("success", function (file, result) {
            if (result.success) {
                for (var i = 0; i < result.fileIds.length; i++) {
                    $(".mind-file").parents("form").append("<input type='hidden' name='Files' value='" + result.fileIds[i] + "' />");
                    file.serverId = result.fileIds[i];
                }
            }
        });

        dropzone.on("removedfile", function (file) {
        	$("input[value=" + file.serverId + "]").remove();
        });

		dropzone.on('sending', function(file, xhr, formData){
            formData.append('__RequestVerificationToken', settings.AntiForgeryToken);
        });
    };

    var messageIdToPublish = 0;
    var setMessageToPublish = function(messageId) {
    	messageIdToPublish = messageId;
    };
    	
    var publishMessage = function () {

        $("#publish-loader").css("display", "inline-block");
        $("#publish-btn").addClass('disabled');

        $.ajax({
            type: "POST",
            url: settings.PublishMessageUrl,
            data: { messageId: messageIdToPublish, __RequestVerificationToken: settings.AntiForgeryToken }
        })
        .done(function (result) {

            if (result.success) {

            	$('#publish-confirm').modal('hide');
            	webApp.ShowMessage(true, "Speech was published.", null);
				publishMessageCallback(messageIdToPublish);

            } else {
                if (result.error) {
                    webApp.ShowMessage(false, result.error, null);
                } else {
                    webApp.ShowMessage(false, "Upppss... An error occurred while publish speech. Please try again.", null);
                }
            }

        })
        .fail(function() {
    		webApp.ShowMessage(false, "Upppss... An error occurred while publish speech. Please try again.", null);
  		})
  		.always(function() {
	        $("#publish-loader").css("display", "none");
	        $("#publish-btn").removeClass('disabled');
  		});
    };

    var deleteMessageCallback = function(messageId) {
		$("#actions-" + messageId).parents(".timeline-story").slideUp();
	};

	var publishMessageCallback = function(messageId) {

	};

	var setPublishMessageCallback = function(callback) {
		publishMessageCallback = callback;
	};

	var setDeleteMessageCallback = function(callback) {
		deleteMessageCallback = callback;
	};

    return {
        Init: init,
        SendTransmit: sendTransmit,
        Favourite: favourite,
        FavouriteComment: favouriteComment,
        OnBeginCreateComment: onBeginCreateComment,
        OnCompleteCreateComment: onCompleteCreateComment,
        OnFailureCreateComment: onFailureCreateComment,
        OnSuccessCreateComment: onSuccessCreateComment,
        OnSuccessCreateMind: onSuccessCreateMind,
        OnFailureCreateMind: onFailureCreateMind,
        OnBeginCreateMind: onBeginCreateMind,
        OnCompleteCreateMind: onCompleteCreateMind,
        ToggleCommentForm: toggleCommentForm,
        SendCommentQuote: sendCommentQuote,
        SendMindQuote: sendMindQuote,
        OnSuccessCreateQuote: onSuccessCreateQuote,
        OnFailureCreateQuote: onFailureCreateQuote,
        OnBeginCreateQuote: onBeginCreateQuote,
        OnCompleteCreateQuote: onCompleteCreateQuote,
        ToggleConnection: toggleConnection,
        DeleteMessage: deleteMessage,
        Bubble: bubble,
        SetMessageToDelete: setMessageToDelete,
        ToggleImageDropzone: toggleImageDropzone,
        InitializeDropzone: initializeDropzone,
        SetMessageToPublish: setMessageToPublish,
        PublishMessage: publishMessage,
        SetPublishMessageCallback: setPublishMessageCallback,
        SetDeleteMessageCallback: setDeleteMessageCallback
    };
};

