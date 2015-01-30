(function ($, dp) {
  
  var environment = dp.session = dp.session || {};
  var utility = dp.Utility = dp.Utility || {};

  var environmentDropDown, returnUrl;
  
  environment.change = function () {
    var criteria = environmentDropDown.val();
    var url = returnUrl.val();
    environment.changeEnvironment(criteria, url);
  };
  
  environment.changeEnvironment = function (searchCriteria, url) {
      var ajaxObject = {
          data: JSON.stringify({ environment: searchCriteria, returnUrl: url }),
          url: utility.getBaseUrl() + "/" + "Environment/Change",
          type: "POST",
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          async: false,
          traditional: true,
          success: function (result) {
              if (result.Error == "") {
                  window.location.href = result.Url
          } else {
              if ($.isArray(result.Error)) {
                  utility.addBootStrapErrorArray(result.Error, "Message");
              } else {
                  utility.addBootStrapError(result.Error, "Message");
              }

              errorMessage.show();
          }
          },
          error: function (jqXHR, texStatus, errorThrown) {
              alert("There is a server error");
          }
      };

      utility.sendAjaxRequest(ajaxObject);
  };

  var environmentChange = function () {
    environmentDropDown.change(function () {
      environment.change();
    });
  };

  var initialize = function () {
    environmentDropDown = $("#SelectedEnvironment");
    returnUrl = $("#hdnUrl");
    environmentChange();
  };

	(function () {
	  initialize();
	})();

}(jQuery, DocProcessing));
