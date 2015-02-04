SunLine = window.SunLine || {};
SunLine.Community = SunLine.Community || {};
SunLine.Community.EditProfile = SunLine.Community.EditProfile || {};

SunLine.Community.EditProfile = function () {

	var settings = {
		AntiForgeryToken: null
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
        webApp.ShowMessage(false, "Upppss... An error occurred while saving profile. Please try again.", null);
    };

    var onSuccessSaveProfile = function (result) {

        if (result.success) {

			webApp.ShowMessage(true, "Profile was saved successfuly.", null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while saving profile. Please try again.", null);
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
        webApp.ShowMessage(false, "Upppss... An error occurred while saving categories. Please try again.", null);
    };

    var onSuccessSaveCategories = function (result) {

        if (result.success) {

			webApp.ShowMessage(true, "Categories was saved successfuly.", null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while saving categories. Please try again.", null);
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
        webApp.ShowMessage(false, "Upppss... An error occurred while saving cover. Please try again.", null);
    };

    var onSuccessSaveCover = function (result) {

        if (result.success) {

			webApp.ShowMessage(true, "Cover was saved successfuly.", null);

        } else {
            if (result.error) {
                webApp.ShowMessage(false, result.error, null);
            } else {
                webApp.ShowMessage(false, "Upppss... An error occurred while saving cover. Please try again.", null);
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

	