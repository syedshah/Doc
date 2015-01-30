(function ($, dp) {
  
  var managePulls = dp.ManagePulls = dp.ManagePulls || {};
  var utility = dp.Utility = dp.Utility || {};

  var manCoDropDown, jobDropDown, searchField, searchButton, pullPackButton, cancelPullButton, selectedPullField, authorisePullListButton;

  var manCoDropDownChange = function () {
    manCoDropDown.change(function () {
      // selectedPullField.html('');
      $('#SelectedPull').html('');
      managePulls.getCompletedjobs();
    });
  };

  managePulls.getCompletedjobs = function () {
    var criteria = manCoDropDown.val();
    managePulls.searchJobs(criteria);
  };

  managePulls.searchJobs = function (manCo) {
    var url = utility.getBaseUrl() + "/" + "ManagePulls/GetCompletedJobs";

    $.getJSON(url, { manCo: manCo }, function (data) {
      jobDropDown.empty();

      jobDropDown.append($('<option/>', {
        value: "",
        text: "Choose Job"
      }));

      $.each(data, function (index, code) {
        jobDropDown.append($('<option/>', {
          value: code.Id,
          text: code.JobDisplay
        }));
      });

      //initialize();
    });
  };

  var jobDropDownChange = function () {
    jobDropDown.change(function () {
      managePulls.getPulledDocuments();
    });
  };
  
  managePulls.getPulledDocuments = function () {
    var criteria = jobDropDown.val();
    if (criteria != "") {
      managePulls.searchPulls(criteria);
    } else {
      $("#divPulledDocuments").empty();
    }
  };

  managePulls.onClickRemovePull = function() {
    $(".removePull").click(function() {
      var clientRef = this.dataset.clientref;
      managePulls.removePull(clientRef);
    });
  };

  managePulls.removePull = function(clientRef) {

    var ajaxObject = {
      data: { clientRef: clientRef, jobId: jobDropDown.val() },
      url: utility.getBaseUrl() + "/" + "ManagePulls/RemovePull",
      type: "GET",
      dataType: "html",
      contentType: "application/x-www-form-urlencoded; charset=UTF-8",
      async: true,
      traditional: false,
      success: function (data, texStatus, jqXHR) {
        $("#divPulledDocuments").empty();
        $("#divPulledDocuments").html(data);
        managePulls.onClickRemovePull();
      },
      error: function (jqXHR, texStatus, errorThrown) {
        alert("There is a server error");
      }
    };

    utility.sendAjaxRequest(ajaxObject);

  };
  
  managePulls.searchPulls = function (jobId) {
    var ajaxObject = {
      data: { jobId: jobId },
      url: utility.getBaseUrl() + "/" + "ManagePulls/GetPulls",
      type: "GET",
      dataType: "html",
      contentType: "application/x-www-form-urlencoded; charset=UTF-8",
      async: true,
      traditional: false,
      success: function (data, texStatus, jqXHR) {
        $("#divPulledDocuments").empty();
        $("#divPulledDocuments").html(data);
        managePulls.onClickRemovePull();

        authorisePullListButton = $('#authorisePullList');
        authorisePullListClick();
        //initialize();
      },
      error: function (jqXHR, texStatus, errorThrown) {
        alert("There is a server error");
      }
    };

    utility.sendAjaxRequest(ajaxObject);
  };
  
  var searchClick = function () {
    searchButton.click(function () {
      managePulls.search();
    });
  };
  
  managePulls.disableFormSubmissionOnEnter = function () {
    $("#manageJobsForm").keypress(function (e) {

      if (e.which == 13) {
        var tagName = e.target.id.toLowerCase();
        if (tagName == "searchfield") {
          managePulls.search();
          return false;
        } else {
          return true;
        }
      } else if (e.which !== 13) {
        return true;
      }
    });

  };
  
  managePulls.search = function () {
    var searchValue = searchField.val();
    var jobId = jobDropDown.val();
    managePulls.searchPacks(searchValue, jobId);
    utility.highlightSearchText(searchValue, "JobList");
  };

  managePulls.searchPacks = function (searchValue, jobId) {
    var ajaxObject = {
      data: JSON.stringify({ JobId: jobId, SearchCriteria: searchValue }),
      url: utility.getBaseUrl() + "/" + "ManagePulls/Search",
      type: "POST",
      dataType: "json",
      contentType: "application/json; charset=utf-8",
      async: false,
      traditional: true,
      success: function (result) {
        
        if (result.error == false) {
          //$('#searchPackResultsModal').modal('hide');
          $("#JobList").empty();
          $("#JobList").html(result.message);
          $('#searchPackResultsModal').modal('show');
        } else {
          if ($('#searchPackResultsModal').hasClass('in')) {
            $('#searchPackResultsModal').modal('hide');
          } else {
            if ($.isArray(result.Error)) {
              utility.addBootStrapErrorArray(result.Error, "Message");
            } else {
              utility.addBootStrapError(result.Error, "Message");
            }
          }
        }
  
        pullPackButton = $("#pullPack");
        cancelPullButton = $("#cancelPull");
        authorisePullListButton = $('#authorisePullList');

        cancelPullClick();
        pullPackClick();
        authorisePullListClick();
      },
      error: function (jqXHR, texStatus, errorThrown) {
        //alert("There is a server error");
      }
    };

    utility.sendAjaxRequest(ajaxObject);
  };
  
  var pullPackClick = function () {
    pullPackButton.click(function () {
      managePulls.pullSelectedPacks();
    });
  };
  
  managePulls.pullSelectedPacks = function () {

    var Packs = [];

    $("#JobList tr.data").each(function () {
      Packs.push({
        Selected: $(this).find('.docSelected').is(':checked'),
        ClientReference: $(this).find('.clientReference').val()
      });
    });

    var PullPacksViewModel = {
      jobId: jobDropDown.val(),
      packs: Packs
    };
    
    managePulls.pullPacks(PullPacksViewModel);
  };
  
  managePulls.pullPacks = function (pullPacksViewModel) {
    var ajaxObject = {
      data: JSON.stringify({ pullPacksViewModel: pullPacksViewModel }),
      url: utility.getBaseUrl() + "/" + "ManagePulls/Pull",
      type: "POST",
      dataType: "json",
      contentType: "application/json; charset=utf-8",
      async: false,
      traditional: true,
      success: function (result) {

        if (result.error == false) {
          //$('#searchPackResultsModal').modal('hide');
          managePulls.search();
          managePulls.getPulledDocuments();
        } else {
          //$('#fileAlreadyProcessed').modal('show');
          //alert(result.Error);

          if ($.isArray(result.Error)) {
            utility.addBootStrapErrorArray(result.Error, "Message");
          } else {
            utility.addBootStrapError(result.Error, "Message");
          }

          // errorMessage.show();
        }


        // initialize();
      },
      error: function (jqXHR, texStatus, errorThrown) {
        //alert("There is a server error");
      }
    };

    utility.sendAjaxRequest(ajaxObject);
  };
  
  var cancelPullClick = function () {
    cancelPullButton.click(function () {
      $('#searchPackResultsModal').modal('hide');
      $('#your-modal-id').modal('hide');
      $('body').removeClass('modal-open');
      $('.modal-backdrop').remove();
    });
  };
  
  var authorisePullListClick = function () {
    authorisePullListButton.click(function () {
      managePulls.authorise();
    });
  };
  
  managePulls.authorise = function () {

    var Packs = [];

    $("#divPulledDocuments div.pulls").each(function () {
      Packs.push({
        ClientReference: $(this).find('.clientReference').val(),
        Name: $(this).find('.name').val(),
      });
    });

    var PullPacksViewModel = {
      jobId: jobDropDown.val(),
      packs: Packs
    };

    managePulls.sendEmail(PullPacksViewModel);
  };
  
  managePulls.sendEmail = function (pullPacksViewModel) {
    var ajaxObject = {
      data: JSON.stringify({ pullPacksViewModel: pullPacksViewModel }),
      url: utility.getBaseUrl() + "/" + "ManagePulls/SendEmail",
      type: "POST",
      dataType: "json",
      contentType: "application/json; charset=utf-8",
      async: false,
      traditional: true,
      success: function (result) {
        if (result.Error == "") {
          window.location.href = result.Url
        } else {
          //$('#fileAlreadyProcessed').modal('show');
          //alert(result.Error);

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

  var searchButtonKeyDown = function () {
    searchField.keydown(function (e) {
      // Allow: backspace, delete, tab, escape, enter and .
      if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
        // Allow: Ctrl+A
          (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: space bar
          (e.keyCode == 32) ||
        // Allow: home, end, left, right
          (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
      }
      // Ensure that it is a number and stop the keypress
      if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 90)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
      }
    });
  };

  var initialize = function () {
    manCoDropDown = $("#SelectedManCoId");
    jobDropDown = $("#SelectedJobId");
    searchField = $("#SearchField");
    searchButton = $("#SearchButton");
    pullPackButton = $("#pullPack");
    cancelPullButton = $("#cancelPull");
    selectedPullField = $("#SelectedPull");
    authorisePullListButton = $('#authorisePullList');

    manCoDropDownChange();
    jobDropDownChange();
    searchClick();
    pullPackClick();
    searchButtonKeyDown();
    cancelPullClick();
    authorisePullListClick();
    managePulls.disableFormSubmissionOnEnter();
  };

  (function () {
    initialize();
  })();
  

}(jQuery, DocProcessing));