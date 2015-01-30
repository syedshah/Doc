(function ($, dp) {
  
  var session = dp.session = dp.session || {};
  var utility = dp.Utility = dp.Utility || {};
  var idleChecker = dp.IdleChecker = dp.IdleChecker || {};

  var errorMessage, errorDivTxt, loginError;
  
  var sessionTimeOutWarning, timerHandler, timer, theTime;

  var loadTimeOutModalCallback = function () {

    $('#pleaseSelectModal').modal({
      show: true,
      backdrop: 'static',
      keyboard: false
    });

    $('button#continue').click(function (e) {
      closeDialog();
    });
    timer = 60;
    timerClock();

  };

  var resetSession = function() {
    $.ajax({ url: utility.getBaseUrl() + "/Session/SessionReset", type: "GET", cache: false, async: false });
  };

  var idleManagerOptions = {
    threshold: 1200,
    callbackFunction: loadTimeOutModalCallback,
    stopCountingOnThreshold: true,
    resetInactiveCallBacK: resetSession,
    resetInactive: true
  };
  
  var sessionIdleManager = idleChecker.initializeIdleManager(idleManagerOptions);

  var timerClock = function () {
    timer = timer - 1;
    if (timer == 0) {
      clearTimeout(timerHandler);
      window.location.href = utility.getBaseUrl() + '/Session/Expired'; //redirect to session out
    }
    theTime.html(timer);
    timerHandler = setTimeout(timerClock, 1000);
  };

  var onSessionWarningClosing = function() {
    $('#pleaseSelectModal').on('hide.bs.modal', function (e) {
      clearTimeout(timerHandler);
      resetSession();
      sessionIdleManager.StartManager();
    });
  };

  var closeDialog = function () {
    sessionTimeOutWarning.modal('hide');
  };
  
  var initialize = function () {
    
	    errorMessage = $("#Message");
	    errorDivTxt = $(".alert-danger").text();
    
	    theTime = $('#the_time');
	    sessionTimeOutWarning = $('#pleaseSelectModal');
	    onSessionWarningClosing();
    
	    if (errorDivTxt.length > 20) {
	      errorMessage.show(); 
	    } else {
	      errorMessage.hide();
	    }
	};

	(function () {
	  initialize();
	  sessionIdleManager.StartManager();
	})();

}(jQuery, DocProcessing));
