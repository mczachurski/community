SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.UserConnection = SunLine.Community.UserConnection || {};

SunLine.Community.UserConnection = function () {

	var settings = {
		ToggleConnectionUrl: null,
		AntiForgeryToken: null,
		ConnectionWasCreatedMessage: null,
		ConnectionWasRemovedMessage: null,
	    ErrorWhileChangingConnectionMessage: null
	};

	var init = function (options) {
		$.extend(settings, options);
		settings.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
	};

	var toggleConnection = function (sender, userId, hasConnectionClass) {

		webApp.ShowPageLoader();

		$.ajax({
			type: "POST",
			url: settings.ToggleConnectionUrl,
			data: { toUserId: userId, __RequestVerificationToken: settings.AntiForgeryToken }
		})
        .done(function (result) {

            if (result.success) {

            	if(hasConnectionClass == null)
            	{
            		hasConnectionClass = 'btn-success';
            	}

            	if(result.usersHasConnection)
            	{
            	    $(sender).removeClass(hasConnectionClass);
            	    $(sender).children('i').removeClass('fa-check');
            		$(sender).addClass('btn-danger');
					$(sender).children('i').addClass('fa-remove');
					$(sender).children('span').html('Stop observing');

					webApp.ShowMessage(true, settings.ConnectionWasCreatedMessage, null);
            	}
            	else
            	{
            		$(sender).removeClass('btn-danger');
            		$(sender).children('i').removeClass('fa-remove');
            		$(sender).addClass(hasConnectionClass);
					$(sender).children('i').addClass('fa-check');
					$(sender).children('span').html('Start observing');

					webApp.ShowMessage(true, settings.ConnectionWasRemovedMessage, null);
            	}

            } else {
            	if (result.error) {
            		webApp.ShowMessage(false, result.error, null);
            	} else {
            	    webApp.ShowMessage(false, settings.ErrorWhileChangingConnectionMessage, null);
            	}
            }

        })
		.fail(function() {
		    webApp.ShowMessage(false, settings.ErrorWhileChangingConnectionMessage, null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});

	};
  		
	return {
		Init: init,
		ToggleConnection: toggleConnection
	};
};

	