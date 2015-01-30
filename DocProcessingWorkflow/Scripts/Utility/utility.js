(function ($, dp) {
  
    var utility = dp.Utility = dp.Utility || {};

    var siteName;

    utility.sendAjaxRequest = function (ajaxObject) {
        var cache, data, url, dataType, type, contentType, async, traditional, done, fail;

        cache = ajaxObject.cache;
        data = ajaxObject.data;
        url = ajaxObject.url;
        async = ajaxObject.async;
        dataType = ajaxObject.dataType;
        contentType = ajaxObject.contentType;
        type = ajaxObject.type;

        traditional =
            ajaxObject.traditional != undefined ?
            ajaxObject.traditional : false;

        done = ajaxObject.success;
        fail = ajaxObject.error;

        $.ajax({
            cache: cache,
            type: type,
            url: url,
            data: data,
            dataType: dataType,
            contentType: contentType,
            async: async,
            traditional: traditional
        })
            .done(done)
            .fail(fail)
            .always();
        };

    utility.onSuccess = function (data, receiver) {
        receiver.success(data);
    };

    utility.onError = function (data, receiver) {
        receiver.success(data);
    };

    utility.getBaseUrl = function (address) {
    var path = location.protocol + "//" + location.host;
    var websitename = $("#SiteName").val();

    if (websitename != "/") {
        path = path + websitename;
    }

    if (address != undefined && address != "") {
        path = path + "/" + address;
    }

    return path;
    };

    utility.getSplit = function (dataString, delimiter) {
        return dataString.split(delimiter);
    };

    utility.getItemInArray = function (dataArray, itemNumber) {
        return dataArray[itemNumber];
    };

    utility.addBootStrapError = function(errorMessage, messageHolderId) {
        var errorDiv = document.createElement("div");
        var errorPara = document.createElement("p");
        var dismissButton = document.createElement("button");
        dismissButton.setAttribute('class','close');
        dismissButton.setAttribute('data-dismiss','alert');
        dismissButton.setAttribute('aria-hidden', 'true');
        dismissButton.innerHTML = '&times;';
        var errorMess = document.createTextNode(errorMessage);
        errorPara.appendChild(errorMess);
        errorDiv.setAttribute('class', 'alert alert-danger alert-dismissable');
        errorDiv.appendChild(dismissButton);
        errorDiv.appendChild(errorPara);
        $("#" + messageHolderId).append(errorDiv);
    };
  
    utility.addBootStrapErrorArray = function (errorMessageArray, messageHolderId) {
        var errorDiv = document.createElement("div");
        var dismissButton = document.createElement("button");
        dismissButton.setAttribute('class', 'close');
        dismissButton.setAttribute('data-dismiss', 'alert');
        dismissButton.setAttribute('aria-hidden', 'true');
        dismissButton.innerHTML = '&times;';
        errorDiv.setAttribute('class', 'alert alert-danger alert-dismissable');
        errorDiv.appendChild(dismissButton);
    
        $.each(errorMessageArray, function (key, value) {
            var errorPara = document.createElement("p");
            var errorMess = document.createTextNode(value.ErrorMessage);
            errorPara.appendChild(errorMess);
            errorDiv.appendChild(errorPara);
        });
    
        $("#" + messageHolderId).append(errorDiv);
    };
  
    utility.highlightSearchText = function (searchText, identifierId) {
        if (searchText != "") {
            $("#" + identifierId).highlight(searchText);
        }
    };

    var initialize = function () {
        siteName = $("#SiteName").val();
    };

    (function () {
        initialize();
    })();

})(jQuery, DocProcessing);