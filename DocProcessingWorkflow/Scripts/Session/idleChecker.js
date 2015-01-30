(function ($, dp) {
  
  var idleChecker = dp.IdleChecker = dp.IdleChecker || {};

  var active, inActiveSeconds, inactiveSecondsThreshold, thresholdCallback, counterVar;

  var inactivityCounter = function() {
    counterVar = setInterval(function() {
      inActiveSeconds = inActiveSeconds + 1;
      checkThreshold(inActiveSeconds);
    }, 1000);
  };

  var startCounting = function () {
    inactivityCounter();
  };

  var stopCounting = function() {
    clearInterval(counterVar);
  };

  var checkForUserActivity = function () {
    
    $(".container-fluid").mousedown(function(e) {
      resetInactiveSeconds();
      //e.preventDefault();
    });
    
    $(".container-fluid").keyup(function (e) {
      resetInactiveSeconds();
     // e.preventDefault();
    });

    /////Commented because when I hit Ctrl+V pastes twice
    // $(".container-fluid").keydown(function (e) {
    //  resetInactiveSeconds();
    // // e.preventDefault();
    //});

  };


  var checkThreshold = function (secondsInactive) {
    if (secondsInactive >= inactiveSecondsThreshold) {
      resetInactiveSeconds();
      thresholdCallback();
    }
  };

  var resetInactiveSeconds = function() {
    inActiveSeconds = 0;
  };

  idleChecker.resetInactivity = function() {
    resetInactiveSeconds();
  };
 
  idleChecker.initialize = function (threshold, callFunction) {
    $(document).ready(function () {

      active = true;
      inActiveSeconds = 0;
      inactiveSecondsThreshold = threshold;
      startCounting();
      checkForUserActivity();
      thresholdCallback = callFunction;
    });
  };

  (function () {
    
  })();

}(jQuery, DocProcessing));