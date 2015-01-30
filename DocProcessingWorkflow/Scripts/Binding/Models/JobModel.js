(function (dp) {
	var jobs = dp.Jobs = dp.Jobs || {}; //DocProcessing.Jobs namespace

	jobs.JobModel = function () {
		var self = this;

		self.JobReference = "";
		self.Company = "";
		self.Document = "";
		self.Version = "";
		self.Owner = "";
		self.SubmitDate = "";
		self.Status = "";
		self.Authorise = "";

	};

}(DocProcessing));
