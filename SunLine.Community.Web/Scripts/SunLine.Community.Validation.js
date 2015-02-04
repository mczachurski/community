
/* Initialize validation */
$(function() {
    jQuery.validator.setDefaults({
        highlight: function (element, error, valid) {
            var formGroup = $(element).closest('.form-group');
            formGroup.removeClass(valid);
            formGroup.addClass('validate-has-error');
        },
        unhighlight: function (element, error, valid) {
            var formGroup = $(element).closest('.form-group');
            formGroup.removeClass('validate-has-error');
            formGroup.addClass(valid);
        }
    });
});