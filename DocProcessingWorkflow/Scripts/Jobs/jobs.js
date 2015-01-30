(function ($, dp) {

    var jobs = dp.Jobs = dp.Jobs || {};
    var utility = dp.Utility = dp.Utility || {};
    var idleChecker = dp.IdleChecker = dp.IdleChecker || {};

    var searchField, searchButton, jobLogModal, jobReportModal, jobFilesModal, $body, pdfDownloadLink, excelDownloadLink, enclosedJobsRow, docketNumberTextBoxes, errorMessage, exportJobButton, cancelJobLinks;

    jobs.search = function () {
        var criteria = $("#SearchField").val();
        if (criteria === $("#SearchField").attr("placeholder"))
            criteria = "";
        jobs.searchJobs(criteria);
    };

    jobs.searchFieldVal = function () {
        var criteria = $("#SearchField").val();
        if (criteria === $("#SearchField").attr("placeholder"))
            criteria = "";
        $("#SearchCriteria").val(criteria);
        jobs.assignSearchValue(criteria);
        return criteria;
    };

    jobs.highlightCurrentSearch = function () {
        searchCriteria = $("#SearchValue").val();
        utility.highlightSearchText(searchCriteria, "JobList");
    };

    jobs.assignSearchValue = function (criteria) {

        var ajaxObject = {
            cache: false,
            data: { searchfield: criteria, rnd: new Date().getTime() },
            url: utility.getBaseUrl() + "/" + "Jobs/AssignSearchValue",
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: true,
            success: function (data, texStatus, jqXHR) {
                initialize();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);

    };

    jobs.getAuthoriseSelected = function () {
        var jobsToAuthorise = new Array();
        $(".authoriseBox").each(function () {
            if (this.checked) {
                jobsToAuthorise.push(this.getAttribute("data-jobid"));
            }
        });
        return jobsToAuthorise;
    };

    jobs.authoriseSelected = function () {
        var jobsToAuthorise = jobs.getAuthoriseSelected();

        var ajaxObject = {
            cache: false,
            data: { jobIds: jobsToAuthorise },
            url: utility.getBaseUrl() + "/" + "Jobs/Authorise",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: true,
            success: function (data, texStatus, jqXHR) {
                $("#JobList").empty();
                $("#JobList").html(data);
                initialize();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);

    };

    var authoriseSelectedEvent = function () {
        var authoriseSelected = $("#authoriseSelected");

        if (authoriseSelected != "undefined") {
            authoriseSelected.click(function () {
                jobs.authoriseSelected();
            });
        }
    };

    var exportToExcelEvent = function () {
        var exportToExcel = $("#exportToExcel");

        if (exportToExcel != "undefined") {
            exportToExcel.click(function () {
                jobs.getReport();
            });
        }
    };

    jobs.loadReportFiles = function (jobId, grid) {
        var ajaxObject = {
            cache: false,
            data: { jobId: jobId, grid: grid },
            url: utility.getBaseUrl() + "/" + "Jobs/GetJobReportList",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#report_" + jobId).empty();
                $("#report_" + jobId).html(data);
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.loadOneStepLog = function (jobId, grid) {
        var ajaxObject = {
            cache: false,
            data: { jobId: jobId, grid: grid },
            url: utility.getBaseUrl() + "/" + "Jobs/GetOneStepLog",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#jobLogModalBody").empty();
                $("#jobLogModalBody").html(data);
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.loadOneStepReport = function (fileName, filepath) {

        var ajaxObject = {
            cache: false,
            data: { filepath: filepath },
            url: utility.getBaseUrl() + "/" + "Jobs/GetOneStepReport",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#jobReportModalBody").empty();
                $("#jobReportModalBody").html(data);
                $("#myjobReportLabel").empty();
                $("#myjobReportLabel").html(fileName + " " + "Report");
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.loadInputFilesList = function (jobId) {
        var ajaxObject = {
            cache: false,
            data: { jobId: jobId },
            url: utility.getBaseUrl() + "/" + "Jobs/GetJobFiles",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#jobFilesModalBody").empty();
                $("#jobFilesModalBody").html(data);
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.searchJobs = function (searchCriteria) {
        $body.addClass("loading");
        var ajaxObject = {
            cache: false,
            data: { searchCriteria: searchCriteria },
            url: utility.getBaseUrl() + "/" + "Jobs/Search",
            type: "POST",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $body.removeClass("loading");
                $("#JobList").empty();
                $("#JobList").html(data);
                utility.highlightSearchText(searchCriteria, "JobList");
                initialize();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.reloadJobsPage = function (currentPage, searchValue) {

        window.location = utility.getBaseUrl() + "/" + "Jobs/View?page=" + currentPage + "&searchValue=" + searchValue;

    };

    jobs.reloadJobs = function (currentPage, searchCriteria) {
        $body.addClass("loading");
        var ajaxObject = {
            cache: false,
            data: { page: currentPage },
            url: utility.getBaseUrl() + "/" + "Jobs/ViewJobsAjax",
            type: "POST",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $body.removeClass("loading");
                $("#JobList").empty();
                $("#JobList").html(data);
                utility.highlightSearchText(searchCriteria, "JobList");
                initialize();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.getReport = function () {
        var criteria = $("#SearchField").val();
        if (criteria === $("#SearchField").attr("placeholder"))
            criteria = "";
        window.location = utility.getBaseUrl() + "/" + "Jobs/GetExcelReport/?searchCriteria=" + criteria;
    };


    jobs.getOneStepExcelReport = function (fileName, filePath) {
        var criteria = fileName + "&filePath=" + filePath;
        window.location = utility.getBaseUrl() + "/" + "Jobs/GetOneStepExcelReport/?fileName=" + criteria;
    };

    jobs.validateSearchCriteria = function () {
        var criteria = $("#SearchField").val();
        if (criteria === $("#SearchField").attr("placeholder"))
            criteria = "";
        if (criteria.length < 1) {
            utility.addBootStrapError('Provide a search criteria to search', "Message");
            return false;
        } else {
            return true;
        }
    };

    jobs.onPageChangeSuccess = function () {
        jobs.highlightCurrentSearch();
        initialize();
    };

    var searchClick = function () {
        searchButton.unbind().bind('click', function () {
            jobs.search();
        });
    };

    $('#SearchField').keypress(function (event) {
 
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            jobs.search();
        }
 
    });

    jobs.disableFormSubmissionOnEnter = function () {
        $("#viewJobsForm").keypress(function (e) {
            if (e.which == 13) {
                var tagName = e.target.id.toLowerCase();
                if (tagName == "searchfield") {
                    //jobs.search();
                    return false;
                } else {
                    return true;
                }
            } else if (e.which !== 13) {
                return true;
            }
        });
    };

    var onOneStepLogModalShown = function () {
        jobLogModal.on('shown.bs.modal', function (e) {
            var grid = e.relatedTarget.getAttribute("data-grid");
            jobs.loadOneStepLog(e.relatedTarget.id.replace("jobLog_", ""), grid);
        });
    };

    var onOneStepReportModalShown = function () {
        jobReportModal.on('shown.bs.modal', function (e) {
            var filepath = e.relatedTarget.getAttribute("data-filepath");
            var filename = e.relatedTarget.getAttribute("data-filename");
            jobs.loadOneStepReport(filename, filepath);
        });
    };

    var onInputFileListModalShown = function () {
        jobFilesModal.on('shown.bs.modal', function (e) {
            var jobid = e.relatedTarget.getAttribute("data-jobid");
            jobs.loadInputFilesList(jobid);
        });
    };

    var onPdfLinkClicked = function () {
        pdfDownloadLink.click(function () {
            var filePath = $(this).attr("data-path");
            var fileName = $(this).attr("data-fileName");
            jobs.downloadPdf(filePath, fileName);
        });
    };

    var onExcelDownloadClicked = function () {
        excelDownloadLink.click(function () {
            var filePath = $(this).attr("data-filepath");
            var fileName = $(this).attr("data-filename");
            jobs.getOneStepExcelReport(fileName, filePath);
        });
    };

    jobs.downloadPdf = function (filePath, fileName) {
        window.open(utility.getBaseUrl() + "/" + "Jobs/GetPdf/?filePath=" + filePath + "&fileName=" + fileName, '_blank');
    };

    var inactivityCallBack = function () {
        var currentPage = $("#CurrentPage").val();
        var searchValue = $("#SearchValue").val();
        jobs.assignSearchValue(searchValue);
        jobs.reloadJobs(currentPage, searchValue);
    };

    jobs.Initialize = function (jobsView) {
        var self = this;

        self.model = new jobs.JobsViewModel(jobsView);

        ko.applyBindings(self.model, $("#JobsContent")[0]);

        $(function () {
            $('#JobsTabs a:first').tab('show');
        });

    };

    jobs.pdfLinkInitialize = function () {
        pdfDownloadLink = $(".pdfDownload");
        onPdfLinkClicked();
    };

    jobs.excelDownloadReportInitialize = function () {
        excelDownloadLink = $(".excelReportDownload");
        onExcelDownloadClicked();
    };

    var OnEnclosedJobsShown = function () {
        enclosedJobsRow.on('shown.bs.collapse', function (e) {
            var jobId = e.currentTarget.id.replace("jobRow_", "");
            $("#" + jobId).html('<span class="glyphicon glyphicon-minus"></span>');
        });
    };

    var OnEnclosedJobsHidden = function () {
        enclosedJobsRow.on('hidden.bs.collapse', function (e) {
            var jobId = e.currentTarget.id.replace("jobRow_", "");
            $("#" + jobId).html('<span class="glyphicon glyphicon-plus-sign"></span>');
        });
    };

    var assignDocketNumberTextBoxStatus = function (docketNumber, enclosingJobId) {
        if (docketNumber != null && docketNumber.length == 0) {
            $("#" + enclosingJobId).html('<span class="glyphicon glyphicon-refresh"></span>');
        } else {
            $("#" + enclosingJobId).html('<span class="glyphicon glyphicon-ok"></span>');
        }
    };

    var OnDocketNumberSave = function () {
        docketNumberTextBoxes.focusout(function (e) {
            var textBox = $(this);
            var jobId = textBox.closest("div").attr("id").split('_')[1];
            UpdateDocketNumberStatus(textBox, jobId);
        });

        docketNumberTextBoxes.keyup(function (e) {
            var textBox = $(this);
            var jobId = textBox.closest("div").attr("id").split('_')[1];
            var enclosingJobId = textBox.attr("id");
            if (e.keyCode == 13) {
                UpdateDocketNumberStatus(textBox, jobId);
            }
            else if (!textBox.attr("readonly")) {
                assignDocketNumberTextBoxStatus('', enclosingJobId.split('_')[1]);
            }
        });
    };

    var UpdateDocketNumberStatus = function (textBox, jobId) {
        if (textBox == null || textBox.attr("readonly"))
            return false;

        var enclosingJobId = textBox.attr("id").split('_')[1];
        if (textBox.val().length > 0) {
            if (validateDocketNumber(textBox.val(), enclosingJobId)) {
                jobs.SaveDocketNumber(enclosingJobId, textBox.val(), jobId);
            }
        }
        else {
            hideErrorMessage(enclosingJobId);
            jobs.SaveDocketNumber(enclosingJobId, textBox.val(), jobId);
            assignDocketNumberTextBoxStatus(textBox.val(), enclosingJobId);
        }
    };

    var validateDocketNumber = function (docketNumber, enclosingJobId) {
        if (docketNumber == null)
            return false;
        //DocketNumber must not be greater than 25 characters and It must be AlphaNumeric
        if (docketNumber.length > 25 || docketNumber.length < 6) {
            showErrorMessage(enclosingJobId, errorMessage);
            return false;
        }
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        if (regex.test(docketNumber)) {
            // if (/\d/.test(docketNumber) && /[a-zA-Z]/.test(docketNumber)) {
            hideErrorMessage(enclosingJobId);
            return true;
        }
        else {
            errorMessage = "Only accepts alphanumeric.";
            assignDocketNumberTextBoxStatus("", enclosingJobId);
            showErrorMessage(enclosingJobId, errorMessage);
            return false;
        }
    };

    var hideErrorMessage = function (enclosingJobId) {
        $("#Error_" + enclosingJobId).hide();
        $("#Error_" + enclosingJobId).html("");
    };

    var showErrorMessage = function (enclosingJobId, message) {
        $("#Error_" + enclosingJobId).show();
        $("#Error_" + enclosingJobId).html(message);
    };

    jobs.SaveDocketNumber = function (enclosingJobId, docketNumber, jobId) {
        var jobStatusColumnId = $("#jobStatus_" + jobId);
        var ajaxObject = {
            cache: false,
            data: { enclosingJobId: enclosingJobId, postalDocketNumber: docketNumber, jobId: jobId },
            url: utility.getBaseUrl() + "/" + "Jobs/SaveDocketNumber",
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data) {
                if (data.Success == false) {
                    showErrorMessage(enclosingJobId, data.ErrorMessage);
                }
                else {
                    if (data.Status != null) {
                        jobStatusColumnId.text(data.Status);
                        highlightJobRowStatus(jobStatusColumnId, false);

                        $("#cancelJob_" + jobId).addClass("hidden");
                    }
                    else if (jobStatusColumnId.text() === "Dispatched") {
                        highlightJobRowStatus(jobStatusColumnId, true);

                        $("#cancelJob_" + jobId).removeClass("hidden");
                    }
                    hideErrorMessage(enclosingJobId);
                    assignDocketNumberTextBoxStatus(docketNumber, enclosingJobId);
                }
            },
            error: function (jqXHR, texStatus, errorThrown) {
                showErrorMessage(enclosingJobId, "There is server error.");
                assignDocketNumberTextBoxStatus('', enclosingJobId);
            }
        };
        utility.sendAjaxRequest(ajaxObject);
    };

    jobs.CancelJob = function (jobId) {
        var ajaxObject = {
            cache: false,
            data: { jobId: jobId },
            url: utility.getBaseUrl() + "/" + "Jobs/CancelJob",
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data) {
                var jobStatusColumnId = $("#jobStatus_" + jobId);
                jobStatusColumnId.text(data.Status);
                highlightJobRowStatus(jobStatusColumnId, false);
                var jobCancelLink = $("#cancelJob_" + jobId);
                jobCancelLink.addClass("hidden");
            },
            error: function (jqXHR, texStatus, errorThrown) {
                // Do Nothing
            }
        };
        utility.sendAjaxRequest(ajaxObject);
    };

    var highlightJobRowStatus = function (jobRowStatusId, highlight) {
        if (jobRowStatusId == null)
            return false;
        if (highlight) {
            jobRowStatusId.addClass("danger");
        }
        else {
            jobRowStatusId.removeClass("danger");
        }
    };

    var exportJobButtonClick = function () {
        exportJobButton.click(function (e) {
            var jobId = e.currentTarget.id.split('_')[1];
            jobs.ExportJobToExcel(jobId);
        });
    };

    var applyCancelJobEvents = function () {
        cancelJobLinks.click(function (e) {
            var jobId = e.currentTarget.id.split('_')[1];
            jobs.CancelJob(jobId);
        });
    }

    jobs.ExportJobToExcel = function (jobId) {
        window.location = utility.getBaseUrl() + "/" + "Jobs/ExportJobToExcel/?jobId=" + jobId;
    };

    var initialize = function () {
        searchField = $("#SearchField");
        searchButton = $("#SearchButton");
        jobLogModal = $("#jobLogModal");
        jobReportModal = $("#jobReportModal");
        jobFilesModal = $("#jobFilesModal");
        enclosedJobsRow = $(".collapse");
        docketNumberTextBoxes = $("input[id*='DocketNumber_']");
        errorMessage = "Must be of 6-25 characters and should be aplhanumeric.";
        exportJobButton = $("a[id*='exportJobToExcel_']");
        cancelJobLinks = $("a[id*='cancelJob_']");

        $body = $("body");
        searchClick();
        onOneStepLogModalShown();
        onOneStepReportModalShown();
        onInputFileListModalShown();
        authoriseSelectedEvent();
        exportToExcelEvent();
        jobs.pdfLinkInitialize();
        jobs.excelDownloadReportInitialize();
        jobs.disableFormSubmissionOnEnter();
        OnEnclosedJobsShown();
        OnEnclosedJobsHidden();
        OnDocketNumberSave();
        assignDocketNumberTextBoxStatus();
        hideErrorMessage();
        showErrorMessage();
        highlightJobRowStatus();
        exportJobButtonClick();
        applyCancelJobEvents();
    };

    var idleManagerOptions = {
        threshold: 120,
        callbackFunction: inactivityCallBack,
        stopCountingOnThreshold: false,
        resetInactiveCallBacK: {},
        resetInactive: false
    };

    (function () {
     $.ajaxSetup({
            cache: false
        });

        initialize();
        var jobIdleManager = idleChecker.initializeIdleManager(idleManagerOptions);
        jobIdleManager.StartManager();
    })();

}(jQuery, DocProcessing));
