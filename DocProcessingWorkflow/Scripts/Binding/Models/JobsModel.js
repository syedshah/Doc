(function(dp) {
	var jobs =  dp.Jobs = dp.Jobs || {}; //DocProcessing.Jobs namespace

	jobs.JobsModel = function(jobsView) {
		var self = this;

		self.ViewJobsModel = new jobs.ViewJobsModel(jobsView.ViewJobs.Jobs);
		self.PullJobsModel = new jobs.PullJobsModel();
		self.SubmitJobsModel = new jobs.SubmitJobsModel();
		self.InsertJobsModel = new jobs.InsertJobsModel();

	};

}(DocProcessing));

