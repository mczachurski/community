SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.EditProfile = SunLine.Community.EditProfile || {};

SunLine.Community.EditProfile = function () {

	var settings = {
	    AntiForgeryToken: null,
	    ErrorWhileSavingProfileMessage: null,
	    ProfileSavedSuccessfullyMessage: null,
	    ErrorWhileSavingCategoriesMessage: null,
	    CategoriesSavedSuccessfullyMessage: null,
	    ErrorWhileSavingCoverMessage: null,
	    CoverSavedSuccessfullyMessage: null
	};

	var init = function (options) {
		$.extend(settings, options);
		settings.AntiForgeryToken = $("input[name=__RequestVerificationToken]").val();
	};
		
    var onBeginSaveProfile = function () {
        webApp.ShowPageLoader();
        $("#save-profile").addClass("disabled");
    };

    var onCompleteSaveProfile = function () {
        webApp.HidePageLoader();
        $("#save-profile").removeClass("disabled");
    };

    var onFailureSaveProfile = function () {
        webApp.ShowMessage(false, settings.ErrorWhileSavingProfileMessage, null);
    };

    var onSuccessSaveProfile = function (result) {

        if (result.success) {

			webApp.ShowMessage(true, settings.ProfileSavedSuccessfullyMessage, null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, settings.ErrorWhileSavingProfileMessage, null);
            }
        }
    };

    var onBeginSaveCategories = function () {
        webApp.ShowPageLoader();
        $("#save-category").addClass("disabled");
    };

    var onCompleteSaveCategories = function () {
        webApp.HidePageLoader();
        $("#save-category").removeClass("disabled");
    };

    var onFailureSaveCategories = function () {
        webApp.ShowMessage(false, settings.ErrorWhileSavingCategoriesMessage, null);
    };

    var onSuccessSaveCategories = function (result) {

        if (result.success) {

            webApp.ShowMessage(true, settings.CategoriesSavedSuccessfullyMessage, null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, settings.ErrorWhileSavingCategoriesMessage, null);
            }
        }
    };

    var onBeginSaveCover = function () {
        webApp.ShowPageLoader();
        $("#save-cover").addClass("disabled");
    };

    var onCompleteSaveCover = function () {
        webApp.HidePageLoader();
        $("#save-cover").removeClass("disabled");
    };

    var onFailureSaveCover = function () {
        webApp.ShowMessage(false, settings.ErrorWhileSavingCoverMessage, null);
    };

    var onSuccessSaveCover = function (result) {

        if (result.success) {

            webApp.ShowMessage(true, settings.CoverSavedSuccessfullyMessage, null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, settings.ErrorWhileSavingCoverMessage, null);
            }
        }
    };

	return {
		Init: init,
		OnBeginSaveProfile: onBeginSaveProfile,
		OnCompleteSaveProfile: onCompleteSaveProfile,
		OnFailureSaveProfile: onFailureSaveProfile,
		OnSuccessSaveProfile: onSuccessSaveProfile,
		OnBeginSaveCategories: onBeginSaveCategories,
		OnCompleteSaveCategories: onCompleteSaveCategories,
		OnFailureSaveCategories: onFailureSaveCategories,
		OnSuccessSaveCategories: onSuccessSaveCategories,
		OnBeginSaveCover: onBeginSaveCover,
		OnCompleteSaveCover: onCompleteSaveCover,
		OnFailureSaveCover: onFailureSaveCover,
		OnSuccessSaveCover: onSuccessSaveCover
	};
};

	