(function($, dp) {
    var users = dp.Users = dp.Users || {};
    var utility = dp.Utility = dp.Utility || {};

    var searchField, searchButton, $body, searchCriteria;

    var searchClick = function() {
        searchButton.click(function() {
            users.search();
        });
    };

    users.search = function() {
        var criteria = searchField.val();
        if (criteria == searchField.attr("placeholder")) {
            criteria = "";
        }

        users.searchUsers(criteria, 1);
    };

    users.searchFieldVal = function () {
        var criteria = searchField.val();
        if (criteria === searchField.attr("placeholder"))
            criteria = "";
        $("#SearchCriteria").val(criteria);
        users.assignSearchValue(criteria);
        return criteria;
    };

    users.assignSearchValue = function (criteria) {
        var ajaxObject = {
            cache: false,
            data: { searchfield: criteria },
            url: utility.getBaseUrl() + "/" + "User/AssignSearchValue",
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: true,
            success: function (data, texStatus, jqXHR) {

            },
            error: function (jqXHR, texStatus, errorThrown) {
                //alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    users.searchUsers = function (searchCriteria, currentPage) {
        $body.addClass("loading");
        var ajaxObject = {
            cache: false,
            data: { searchCriteria: searchCriteria, page: currentPage },
            url: utility.getBaseUrl() + "/" + "User/SearchUsers",
            type: "POST",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function(data, texStatus, jqXHRHR) {
                $body.removeClass("loading");
                $("#userList").empty();
                $("#userList").html(data);
                utility.highlightSearchText(searchCriteria, "userList");
                initialize();
            },
            error: function (jqXHR, texStatus, errorThrown) {
               // alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    users.onPageChangeSuccess = function () {
        users.highlightCurrentSearch();
    };

    users.highlightCurrentSearch = function () {
        searchCriteria = $("#UserSearchValue").val();
        utility.highlightSearchText(searchCriteria, "userList");
    };
    var initialize = function() {
        searchField = $("#SearchUser");
        searchButton = $("#SearchButton");

        $body = $("body");
        searchClick();
    };

    (function() {
        initialize();
    })();
}(jQuery, DocProcessing));