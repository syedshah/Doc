(function ($, dp) {

    var promoteManCoDocs = dp.PromoteManCoDocs = dp.PromoteManCoDocs || {};
    var utility = dp.Utility = dp.Utility || {};

    var manCoDropDown, sourceEnvironmentDropDown, targetEnvironmentDropDown, docSearchTextBox;
    var availableManCoDocs, selectedManCoDocs, divManCoDocs, docSelectedMessage, allDocs;
    var addAllButton, addButton, removeButton, removeAllButton, promoteDocsButton;
    var promoteManCoDocsForm, errorMessage;

    $.fn.filterByText = function (textbox, selectSingleMatch) {
        return this.each(function () {
            var select = this;
            var options = [];
            $(select).find('option').each(function () {
                options.push({ value: $(this).val(), text: $(this).text() });
            });
            $(select).data('options', options);
            $(textbox).bind('change keyup cut input', function () {
                var options = allDocs;// $(select).empty().scrollTop(0).data('options');
                var allChoosenDocs = [];
                $('#SelectedManCoDocs').find('option').each(function () {
                    allChoosenDocs.push($(this).text());
                });
                $(select).empty().scrollTop(0);
                var search = $.trim($(this).val());
                search = search.replace(/\*/g, '\\w+');
                var regex = new RegExp(search, 'gi');

                $.each(options, function (i) {
                    var option = options[i];
                    if (option.text.match(regex) !== null && $.inArray(option.text, allChoosenDocs) < 0) {
                        $(select).append($('<option>').text(option.text).val(option.value));
                    }
                });
                if (selectSingleMatch === true &&
                  $(select).children().length === 1) {
                    $(select).children().get(0).selected = true;
                }
                reorderSelects();
            });
        });
    };


    promoteManCoDocs.getManCoDocsByManCoEnvironment = function (manCo, sourceEnvironemt) {
        var ajaxObject = {
            data: { manCoCode: manCo, selectedSourceEnvironment: sourceEnvironemt },
            url: utility.getBaseUrl() + "/" + "PromoteManCoDocs/GetManCoDocs",
            type: "GET",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#divManCoDocs").empty();
                $("#divManCoDocs").html(data);
                initializeDrocument();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                alert("There is a server error");
            }
        };
        utility.sendAjaxRequest(ajaxObject);
    };

    promoteManCoDocs.availableManCoDocs = function () {
        $("#AvailableManCoDocs option").each(function () {
            $(this).remove();
        });
    };

    promoteManCoDocs.selectedManCoDocs = function () {
        $("#SelectedManCoDocs option").each(function () {
            $(this).remove();
        });
    };

    promoteManCoDocs.emptySelected = function () {
        docSearchTextBox.val("");
        promoteManCoDocs.availableManCoDocs();
        promoteManCoDocs.selectedManCoDocs();
        promoteManCoDocs.ShowSelectedMessage();
    };

    promoteManCoDocs.getManCoDocs = function () {
        var sourceEnvironemt = sourceEnvironmentDropDown.val();
        var manCo = manCoDropDown.val();

        promoteManCoDocs.getManCoDocsByManCoEnvironment(manCo, sourceEnvironemt);
    };

    var manCoDropDownChange = function () {
        manCoDropDown.change(function () {
            var $theAlert = $('.alert');
            $theAlert.hide();
            promoteManCoDocs.getManCoDocs();
        });
    };

    var sourceEnvironmentDropDownChange = function () {
        sourceEnvironmentDropDown.change(function () {
            targetEnvironmentDropDown.val("");
            manCoDropDown.val("");
            promoteManCoDocs.emptySelected();
        });
    };

    var targetEnvironmentDropDownChange = function () {
        targetEnvironmentDropDown.change(function () {

        });
    };

    promoteManCoDocs.ShowSelectedMessage = function () {
        var count = $("#SelectedManCoDocs option").size();
        docSelectedMessage.text(count + " total file(s) to be processed");

        if (!isNaN(count) && count > 0) {
            var $theAlert = $('.alert');
            $theAlert.hide();
        }
    };

    promoteManCoDocs.addAll = function () {
        $("#AvailableManCoDocs option").appendTo("#SelectedManCoDocs");
        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
        $("#SelectedManCoDocs option").attr("selected", false);
    };

    promoteManCoDocs.addItem = function () {
        $("#AvailableManCoDocs option:selected").appendTo("#SelectedManCoDocs");
        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
        $("#SelectedManCoDocs option").attr("selected", false);
    };

    promoteManCoDocs.removeAll = function () {
        $("#SelectedManCoDocs option").appendTo("#AvailableManCoDocs");
        $("#SelectedManCoDocs option").remove();
        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
        $("#AvailableManCoDocs option").attr("selected", false);
        $("#docSearch").trigger("change");
    };

    promoteManCoDocs.removeItem = function () {
        $("#SelectedManCoDocs option:selected").appendTo("#AvailableManCoDocs");
        $("#SelectedManCoDocs option:selected").remove();
        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
        $("#AvailableManCoDocs option").attr("selected", false);
        $("#docSearch").trigger("change");
    };

    var addAllButtonClick = function () {
        addAllButton.unbind().bind('click', function () {
            promoteManCoDocs.addAll();
            promoteManCoDocs.ShowSelectedMessage();
            reorderSelects();
        });
    };

    var addSelectedButtonClick = function () {
        addButton.unbind().bind('click', function () {
            promoteManCoDocs.addItem();
            promoteManCoDocs.ShowSelectedMessage();
            reorderSelects();
        });
    };

    var removeAllButtonClick = function () {
        removeAllButton.unbind().bind('click', function () {
            promoteManCoDocs.removeAll();
            promoteManCoDocs.ShowSelectedMessage();
            reorderSelects();
        });
    };

    var removeButtonClick = function () {
        removeButton.unbind().bind('click', function () {
            promoteManCoDocs.removeItem();
            promoteManCoDocs.ShowSelectedMessage();
            reorderSelects();
        });
    };

    var promoteDocsButtonClick = function () {
        promoteDocsButton.unbind().bind('click', function () {
            //$("#SelectedManCoDocs option").attr("selected", true);
        });
    };

    promoteManCoDocs.validateForm = function () {
        var $theAlert = $('.alert');
        $theAlert.hide();
        if (sourceEnvironmentDropDown.val().trim().length === 0 || targetEnvironmentDropDown.val().trim().length === 0) {
            utility.addBootStrapError('Please select source and target environments.', "Message");

            errorMessage.show();
            return false;
        } else if (sourceEnvironmentDropDown.val() === targetEnvironmentDropDown.val()) {
            utility.addBootStrapError('Source and target environments should not be equal.', "Message");

            errorMessage.show();
            return false;
        } else if ($("#SelectedManCoDocs option").length < 1) {
            utility.addBootStrapError('Please select at least one Doc to promote.', "Message");

            errorMessage.show();
            return false;
        } else {
            return true;
        }
    };

    var promoteManCoDocsFormSubmit = function () {
        promoteManCoDocsForm.submit(function () {
            $("#SelectedManCoDocs option").each(function () {
                $(this).prop("selected", true);
            });
            if (promoteManCoDocs.validateForm()) {
                return true;
            } else {
                return false;
            }
        });
    };


    var reorderSelects = function () {
        var options = $('#SelectedManCoDocs option');
        var arr = options.map(function (_, o) { return { t: $(o).text(), v: o.value }; }).get();
        arr.sort(function (o1, o2) { return o1.t > o2.t ? 1 : o1.t < o2.t ? -1 : 0; });
        options.each(function (i, o) {
            o.value = arr[i].v;
            $(o).text(arr[i].t);
        });

        var options = $("#AvailableManCoDocs option");
        var arr = options.map(function (_, o) { return { t: $(o).text(), v: o.value }; }).get();
        arr.sort(function (o1, o2) { return o1.t > o2.t ? 1 : o1.t < o2.t ? -1 : 0; });
        options.each(function (i, o) {
            o.value = arr[i].v;
            $(o).text(arr[i].t);
        });
    };

    var initializeDrocument = function () {
        errorMessage = $("#Message");
        availableManCoDocs = $("#AvailableManCoDocs");
        selectedManCoDocs = $("#SelectedManCoDocs");
        divManCoDocs = $("#divManCoDocs");
        addAllButton = $("#btnAddAll");
        addButton = $("#btnAdd");
        removeButton = $("#btnRemove");
        removeAllButton = $("#btnRemoveAll");
        docSelectedMessage = $("#docSelectedMessage");

        addAllButtonClick();
        addSelectedButtonClick();
        removeButtonClick();
        removeAllButtonClick();

        allDocs = [];

        $('#AvailableManCoDocs').find('option').each(function () {
            allDocs.push({ value: $(this).val(), text: $(this).text() });
        });

        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
    };

    var initialize = function () {
        errorMessage = $("#Message");
        sourceEnvironmentDropDown = $("#SelectedSourceEnvironment");
        targetEnvironmentDropDown = $("#SelectedTargetEnvironment");
        manCoDropDown = $("#SelectedManCoCode");
        availableManCoDocs = $("#AvailableManCoDocs");
        selectedManCoDocs = $("#SelectedManCoDocs");
        divManCoDocs = $("#divManCoDocs");
        addAllButton = $("#btnAddAll");
        addButton = $("#btnAdd");
        removeButton = $("#btnRemove");
        removeAllButton = $("#btnRemoveAll");
        docSelectedMessage = $("#docSelectedMessage");
        docSearchTextBox = $("#docSearch");
        promoteDocsButton = $("#promoteDocs");
        promoteManCoDocsForm = $("#promoteManCoDocsForm");

        addAllButtonClick();
        addSelectedButtonClick();
        removeButtonClick();
        removeAllButtonClick();
        promoteDocsButtonClick();
        promoteManCoDocsFormSubmit();

        manCoDropDownChange();
        sourceEnvironmentDropDownChange();
        targetEnvironmentDropDownChange();

        allDocs = [];

        $('#AvailableManCoDocs').find('option').each(function () {
            allDocs.push({ value: $(this).val(), text: $(this).text() });
        });

        $('#AvailableManCoDocs').filterByText($('#docSearch'), true);
    };


    (function () {
        initialize();
    })();

    (function () {
        if (errorMessage.html().trim().length > 0) {
            var $theAlert = $('.alert');
            $theAlert.hide();
            var error = errorMessage.html().trim();
            errorMessage.empty();
            utility.addBootStrapError(error, "Message");
            errorMessage.show();
        }
    })();

}(jQuery, DocProcessing));