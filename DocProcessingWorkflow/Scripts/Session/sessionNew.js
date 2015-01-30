(function ($, dp) {
  
  var session = dp.session = dp.session || {};
  
  var errorMessage, errorDivTxt, loginError;
  
  var initialize = function () {

    errorMessage = $("#Message");
    errorDivTxt = $(".alert-danger").text();

    if (errorDivTxt.length > 20) {
      errorMessage.show();
    } else {
      errorMessage.hide();
    }
  };

  (function () {
    initialize();
  })();

}(jQuery, DocProcessing));