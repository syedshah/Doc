(function ($, dp) {

    var session = dp.session = dp.session || {};

    var errorMessage, errorDivTxt, loginError, passwordExpiredWarning, lastLoginDate;

    var initialize = function () {
        errorMessage = $("#Message");
        errorDivTxt = $(".alert-danger").text();
        passwordExpiredWarning = $("#passwordResetModal");
        lastLoginDate = $("#lastLoginDate");

        if (errorDivTxt.length > 64) {
            errorMessage.show();
        } else {
            errorMessage.hide();
        }
    };

    var passwordExpiredWarningMOdel = function () {
        if (lastLoginDate.val() != "newUser") {
            var date = Date.minDate;
            passwordExpiredWarning.modal({
                show: true,
                backdrop: 'static',
                keyboard: false
            });
        }
    };
  
    (function () {
        initialize();
        passwordExpiredWarningMOdel();
    })();

}(jQuery, DocProcessing));
