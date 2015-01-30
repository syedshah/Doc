(function (dp) {
	var jobs = dp.Jobs = dp.Jobs || {}; //DocProcessing.Jobs namespace

	jobs.ViewJobsModel = function (viewJobs) {
		var self = this;

		self.Jobs = ko.observableArray(viewJobs);

	};

}(DocProcessing));
