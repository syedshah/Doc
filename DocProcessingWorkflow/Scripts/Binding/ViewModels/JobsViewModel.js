(function (dp) {
	var jobs = dp.Jobs = dp.Jobs || {}; //DocProcessing.Jobs namespace

	jobs.JobsViewModel = function (jobsView) {
		var self = this;

		self.ViewJobs = new jobs.ViewJobsModel(jobsView.ViewJobs.Jobs);
		self.PullJobs = new jobs.PullJobsModel();
		self.SubmitJobs = new jobs.SubmitJobsModel();
		self.InsertJobs = new jobs.InsertJobsModel();

	};

}(DocProcessing));
