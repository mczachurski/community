SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.UserConnection = SunLine.Community.UserConnection || {};

SunLine.Community.UserConnection = function () {

	var settings = {
		ToggleConnectionUrl: null,
		AntiForgeryToken: null
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
            	}
            	else
            	{
            		$(sender).removeClass('btn-danger');
            		$(sender).children('i').removeClass('fa-remove');
            		$(sender).addClass(hasConnectionClass);
					$(sender).children('i').addClass('fa-check');
					$(sender).children('span').html('Start observing');
            	}

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
    		webApp.ShowMessage(false, "Upppss... An error occurred while change connection. Please try again.", null);
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

	