SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.UserProfile = SunLine.Community.UserProfile || {};

SunLine.Community.UserProfile = function () {

	var settings = {
		UserId: null,
		TimelineUrl: null,
		ObserversUrl: null,
		ObservingUrl: null,
		AntiForgeryToken: null,
		ObserversId: '#user-observers',
		ObservingId: '#user-observing',
		TimelineId: '#user-timeline',
	    ErrorWhileRequestMessage: null
	};

	var init = function (options) {
		$.extend(settings, options);
		settings.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
	};
		
	var setActiveTab = function(sender) {
		
		$(sender).parents('.row').children('.active-link').removeClass('active-link');
		$(sender).parents('.col-sm-4').addClass('active-link');

	};

	var showTimelinePlaceholder = function() {
		$(settings.ObserversId).hide();
		$(settings.ObservingId).hide();		
		$(settings.TimelineId).show();
	};

	var showObserversPlaceholder = function() {
		$(settings.ObservingId).hide();		
		$(settings.TimelineId).hide();
		$(settings.ObserversId).show();
	};

	var showObservingPlaceholder = function() {
		$(settings.TimelineId).hide();
		$(settings.ObserversId).hide();
		$(settings.ObservingId).show();
	};

	var isNullOrWhitespace = function(input) {
    	if (typeof input === 'undefined' || input == null) return true;
    	return input.replace(/\s/g, '').length < 1;
	};

	var showTimeline = function(sender) {

		var timeline = $(settings.TimelineId);
		if(timeline.is(':visible'))
		{
			return;
		}

		setActiveTab(sender);

		if(isNullOrWhitespace(timeline.html()))
		{
			loadTimeline();
		}
		else
		{
			showTimelinePlaceholder();
		}
	};
		
	var showObservers = function(sender) {

		var observers = $(settings.ObserversId);
		if(observers.is(':visible'))
		{
			return;
		}

		setActiveTab(sender);

		if(isNullOrWhitespace(observers.html()))
		{
			loadObservers();
		}
		else
		{
			showObserversPlaceholder();
		}
	};

	var showObserving = function(sender) {

		var observing = $(settings.ObservingId);
		if(observing.is(':visible'))
		{
			return;
		}

		setActiveTab(sender);

		if(isNullOrWhitespace(observing.html()))
		{
			loadObserving();
		}
		else
		{
			showObservingPlaceholder();
		}
	};

	var loadTimeline = function() {

		webApp.ShowPageLoader();

		$.ajax({
			type: "GET",
			url: settings.TimelineUrl + '/' + settings.UserId
		})
        .done(function (result) {
			$(settings.TimelineId).html(result);
			showTimelinePlaceholder();
        })
		.fail(function() {
    		webApp.ShowMessage(false, settings.ErrorWhileRequestMessage, null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});

	};

	var loadObservers = function(userId) {

		webApp.ShowPageLoader();

		$.ajax({
			type: "GET",
			url: settings.ObserversUrl + '/' + settings.UserId
		})
        .done(function (result) {
			$(settings.ObserversId).html(result);
			showObserversPlaceholder();
        })
		.fail(function() {
		    webApp.ShowMessage(false, settings.ErrorWhileRequestMessage, null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});

	};

	var loadObserving = function(userId) {

		webApp.ShowPageLoader();

		$.ajax({
			type: "GET",
			url: settings.ObservingUrl + '/' + settings.UserId
		})
        .done(function (result) {
			$(settings.ObservingId).html(result);
			showObservingPlaceholder();
        })
		.fail(function() {
		    webApp.ShowMessage(false, settings.ErrorWhileRequestMessage, null);
  		})
  		.always(function() {
    		webApp.HidePageLoader();
  		});

	};

	return {
		Init: init,
		ShowTimeline: showTimeline,
		ShowObservers: showObservers,
		ShowObserving: showObserving
	};
};

