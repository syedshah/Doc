(function ($, dp) {
  
  var create = dp.session = dp.session || {};
  var utility = dp.Utility = dp.Utility || {};

  var errorMessage, errorDivTxt, loginError;
  
  var initialize = function () {
	  errorMessage = $("#Message");
	  errorDivTxt = $(".alert-danger").text();

	  if (errorDivTxt.contains('required')) {
	    errorMessage.show(); 
	  } else {
	    errorMessage.hide();
	  }
	};

	(function () {
	  initialize();
	})();

}(jQuery, DocProcessing));
