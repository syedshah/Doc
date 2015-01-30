(function ($, dp) {

    var idleChecker = dp.IdleChecker = dp.IdleChecker || {};

    idleChecker.initializeIdleManager = function (initialOptions) {

        var idleManager = new idleChecker.IdleManager(initialOptions);

        return idleManager;
    };

    idleChecker.IdleManager = function (initialOptions) {

        var self = this;

        self.inactiveSecondsThreshold = initialOptions.threshold;
        self.thresholdCallback = initialOptions.callbackFunction;
        self.inActiveSeconds = 0;
        self.active = true;
        self.counterVar = {};
        self.stopCountingOnThreshold = initialOptions.stopCountingOnThreshold;
        self.resetInactive = initialOptions.resetInactive;
        self.resetInactiveCallBacK = initialOptions.resetInactiveCallBacK;

        var checkThreshold = function (secondsInactive) {
            if (secondsInactive >= self.inactiveSecondsThreshold) {
                resetInactiveSeconds();
                if (self.stopCountingOnThreshold) {
                    self.stopCounting();
                }
                self.thresholdCallback();
            }
        };

        var inactivityCounter = function () {
            self.counterVar = setInterval(function () {
                self.inActiveSeconds++;
                checkThreshold(self.inActiveSeconds);
            }, 1000);
        };

        self.startCounting = function () {
            inactivityCounter();
        };

        self.stopCounting = function () {
            clearInterval(self.counterVar);
        };

        self.checkForUserActivity = function () {

            $(".container-fluid").mousedown(function (e) {
                resetInactiveSeconds();
                //e.preventDefault();
            });

            $(".container-fluid").keyup(function (e) {
                resetInactiveSeconds();
                // e.preventDefault();
            });

            /////Commented because when I hit Ctrl+V pastes twice
            //$(".container-fluid").keydown(function (e) {
            //  resetInactiveSeconds();
            //  // e.preventDefault();
            //});

        };

        var resetInactiveSeconds = function () {
            self.inActiveSeconds = 0;
            if (self.resetInactive) {
                self.resetInactiveCallBacK();
            }
        };

    };

    idleChecker.IdleManager.prototype.StartManager = function () {
        var self = this;
        self.checkForUserActivity();
        self.startCounting();
    };

    idleChecker.IdleManager.prototype.StopManager = function () {
        var self = this;
        self.stopCounting();
    };

    idleChecker.IdleManager.prototype.SetupResetInactivity = function (resetInactivityCallBacK, resetInactivity) {
        self.resetInactiveCallBacK = resetInactivityCallBacK;
        self.resetInactive = resetInactivity;
    };

}(jQuery, DocProcessing));