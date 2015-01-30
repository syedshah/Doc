(function ($, dp) {

    var submitJobs = dp.Jobs = dp.Jobs || {};
    var utility = dp.Utility = dp.Utility || {};

    var manCoDropDown, documentDropDown, selectedFolderDropDown, additionalSetup;
    var addAllButton, addButton, removeButton, removeAllButton;
    var fileSelectedMessage;
    var submitjobButton, allowReprocessingCheckBox, copyFolderLocationButton;
    var sortCodeOneTextBox, sortCodeTwoTextBox, sortCodeThreeTextBox, accountNumberTextBox, chequeNumberTextBox;
    var errorMessage, errorDivTxt;
    var requiresAdditionalInfoHiddenField;
    var saveAdditionalInfoButton, sortCodeError, accountNumerError, chequeNumberError;
    var fundInfotxt;
    var showAdditionalInfo, availableFilesSearch, allFiles;

    var additionalSetupClick = function () {
        additionalSetup.click(function () {
            submitJobs.getAdditionalSetup();
        });
    };

    submitJobs.getAdditionalSetup = function () {
        sortCodeOneTextBox.val(hiddenSortCodeOne.val());
        sortCodeTwoTextBox.val(hiddenSortCodeTwo.val());
        sortCodeThreeTextBox.val(hiddenSortCodeThree.val());
        accountNumberTextBox.val(hiddenAccountNumber.val());
        chequeNumberTextBox.val(hiddenChequeNumber.val());

        $('#additionalSetupModal').modal('show');
    };

    submitJobs.getInputFiles = function () {
        var manCo = manCoDropDown.val();
        var docTypeCode = documentDropDown.val();
        var docTypeName = $("#SelectedDocTypeId option:selected").text();
        submitJobs.searchInputFiles(manCo, docTypeCode, docTypeName);
    };

    submitJobs.searchInputFiles = function (manCo, docTypeCode, docTypeName) {
        var ajaxObject = {
            cache: false,
            data: { manCo: manCo, docTypeCode: docTypeCode, docTypeName: docTypeName },
            url: utility.getBaseUrl() + "/" + "SubmitJobs/GetFiles",
            type: "POST",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#divFileInfo").empty();
                $("#divFileInfo").html(data);
                initializeDrocument();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                var $theAlert = $('.alert');
                $theAlert.hide();

                utility.addBootStrapError('Man co and doc type directory has not been set up.', "Message");
                errorMessage.show();
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    submitJobs.getDocTypes = function () {
        var criteria = manCoDropDown.val();
        submitJobs.searchDocTypes(criteria);
    };

    submitJobs.searchDocTypes = function (manCo) {
        var url = utility.getBaseUrl() + "/" + "DocTypes/Search";

        $.getJSON(url, { manCo: manCo }, function (data) {
            documentDropDown.empty();

            documentDropDown.append($('<option/>', {
                value: "",
                text: "Document"
            }));

            $.each(data, function (index, code) {
                documentDropDown.append($('<option/>', {
                    value: code.Code,
                    text: code.Description
                }));
            });

            //initialize();
        });
    };

    submitJobs.getFolderFiles = function () {
        var path = selectedFolderDropDown.val();
        var manCo = manCoDropDown.val();
        var docType = documentDropDown.val();
        submitJobs.searchInputFolders(path, manCo, docType);
    };

    submitJobs.searchInputFolders = function (path, manCo, docType) {
        var ajaxObject = {
            cache: false,
            data: { path: path, manCo: manCo, docType: docType },
            url: utility.getBaseUrl() + "/" + "SubmitJobs/GetFolderFiles",
            type: "POST",
            dataType: "html",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#divFileInfo").empty();
                $("#divFileInfo").html(data);
                initializeFoler();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    var manCoDropDownChange = function () {
        manCoDropDown.change(function () {
            var $theAlert = $('.alert');
            $theAlert.hide();

            submitJobs.getDocTypes();
            submitJobs.emptySelected();
            availableFilesSearch.val("");
        });
    };

    submitJobs.emptyChosenFiles = function () {
        $("#ChosenFiles").empty();
    };

    submitJobs.emptyAvailableFiles = function () {
        $("#AvailableFiles").empty();
    };

    submitJobs.emptySelected = function () {
        documentDropDown.empty();
        selectedFolderDropDown.empty();
        submitJobs.emptyAvailableFiles();
        submitJobs.emptyChosenFiles();
        additionalSetup.hide();
    };

    var documentDropDownChange = function () {
        documentDropDown.change(function () {
            var $theAlert = $('.alert');
            $theAlert.hide();

            selectedFolderDropDown.empty();
            submitJobs.emptyChosenFiles();
            submitJobs.emptyAvailableFiles();
            submitJobs.getInputFiles();
        });
    };

    var selectedFolderDropDownChange = function () {
        selectedFolderDropDown.change(function () {
            var $theAlert = $('.alert');
            $theAlert.hide();

            copyFolderLocationButton.show();
            submitJobs.getFolderFiles();
        });
    };

    $.fn.filterByText = function (textbox, selectSingleMatch) {
        return this.each(function () {
            var select = this;
            var options = [];
            $(select).find('option').each(function () {
                options.push({ value: $(this).val(), text: $(this).text() });
            });
            $(select).data('options', options);
            $(textbox).bind('change keyup cut input', function () {
                var options = allFiles;// $(select).empty().scrollTop(0).data('options');
                var allChoosenFiles = [];
                $('#ChosenFiles').find('option').each(function () {
                    allChoosenFiles.push($(this).text());
                });
                $(select).empty().scrollTop(0);
                var search = $.trim($(this).val());
                search = search.replace(/\*/g, '\\w+');
                var regex = new RegExp(search, 'gi');

                $.each(options, function (i) {
                    var option = options[i];
                    if (option.text.match(regex) !== null && $.inArray(option.text, allChoosenFiles) < 0) {
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

    var addAllButtonClick = function () {
        addAllButton.unbind().bind('click', function () {
            submitJobs.addAll();
            submitJobs.ShowSelectedMessage();
            reorderSelects();
            checkAdditionalInfo();
        });
    };

    submitJobs.addAll = function () {
        $("#AvailableFiles option").appendTo("#ChosenFiles");
        $('#AvailableFiles').filterByText($('#filesSearch'), true);
        $("#ChosenFiles option").attr("selected", false);
    };

    var addSelectedButtonClick = function () {
        addButton.unbind().bind('click', function () {
            submitJobs.addItem();
            submitJobs.ShowSelectedMessage();
            reorderSelects();
            checkAdditionalInfo();
        });
    };

    submitJobs.addItem = function () {
        $("#AvailableFiles option:selected").appendTo("#ChosenFiles");
        $('#AvailableFiles').filterByText($('#filesSearch'), true);
        $("#ChosenFiles option").attr("selected", false);
    };

    var addRemoveButtonClick = function () {
        removeButton.unbind().bind('click', function () {
            submitJobs.removeItem();
            submitJobs.ShowSelectedMessage();
            reorderSelects();
            checkAdditionalInfo();
        });
    };

    submitJobs.removeItem = function () {
        $("#ChosenFiles option:selected").appendTo("#AvailableFiles");
        $("#ChosenFiles option:selected").remove();
        $('#AvailableFiles').filterByText($('#filesSearch'), true);
        $("#AvailableFiles option").attr("selected", false);
        $("#filesSearch").trigger("change");
    };

    var addRemoveAllButtonClick = function () {
        removeAllButton.unbind().bind('click', function () {
            submitJobs.removeAll();
            submitJobs.ShowSelectedMessage();
            reorderSelects();
            checkAdditionalInfo();
        });
    };

    submitJobs.removeAll = function () {
        $("#ChosenFiles option").appendTo("#AvailableFiles");
        $("#ChosenFiles option").remove();
        $('#AvailableFiles').filterByText($('#filesSearch'), true);
        $("#AvailableFiles option").attr("selected", false);
        $("#filesSearch").trigger("change");
    };

    submitJobs.ShowSelectedMessage = function () {
        var count = $("#ChosenFiles option").size();
        fileSelectedMessage.text(count + " total file(s) to be processed");

        if (!isNaN(count) && count > 0) {
            var $theAlert = $('.alert');
            $theAlert.hide();
        }
    };

    var checkAdditionalInfo = function () {
        var requiresAdditionalInfo = requiresAdditionalInfoHiddenField.val();
        var path = selectedFolderDropDown.val();

        if (requiresAdditionalInfo == "True") {
            var chosenFiles = [];
            $("#ChosenFiles option").each(function () {
                chosenFiles.push($(this).val());
            });
            getAdditionalInfo(chosenFiles, path);
        }
    };

    var getAdditionalInfo = function (files, path) {
        var ajaxObject = {
            cache: false,
            data: JSON.stringify({ files: files, path: path }),
            url: utility.getBaseUrl() + "/" + "SubmitJobs/GetAdditionalInfo",
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            async: true,
            traditional: false,
            success: function (data, texStatus, jqXHR) {
                $("#additionalInfo").empty();
                $("#additionalInfo").html(data.message);
                initializeFoler();
            },
            error: function (jqXHR, texStatus, errorThrown) {
                alert("There is a server error");
            }
        };

        utility.sendAjaxRequest(ajaxObject);
    };

    var reorderSelects = function () {
        var options = $('#ChosenFiles option');
        var arr = options.map(function (_, o) { return { t: $(o).text(), v: o.value }; }).get();
        arr.sort(function (o1, o2) { return o1.t > o2.t ? 1 : o1.t < o2.t ? -1 : 0; });
        options.each(function (i, o) {
            o.value = arr[i].v;
            $(o).text(arr[i].t);
        });

        var options = $("#AvailableFiles option");
        var arr = options.map(function (_, o) { return { t: $(o).text(), v: o.value }; }).get();
        arr.sort(function (o1, o2) { return o1.t > o2.t ? 1 : o1.t < o2.t ? -1 : 0; });
        options.each(function (i, o) {
            o.value = arr[i].v;
            $(o).text(arr[i].t);
        });
    };

    var submitjobButtonClick = function () {
        submitjobButton.unbind().bind('click', function () {
            submitJobs.getSubmitData();
        });
    };

    var copyFolderLocationButtonClick = function () {
        copyFolderLocationButton.click(function () {
            var location = selectedFolderDropDown.find("option:selected").text();
            $("#copyFolderTextdialog").empty();
            var text = $('<p>' + location + '</p>').appendTo("#copyFolderTextdialog");
            $("#copyLoactionTextModal").modal();
            return false;
        });
    };
    var saveAdditionalInfoClick = function () {
        saveAdditionalInfoButton.click(function () {
            submitJobs.ValidateAdditionalInfo();
            var $theAlert = $('.alert');
            $theAlert.hide();
        });
    };

    submitJobs.ValidateAdditionalInfo = function () {
        var sortCodeValid = true;
        var accountNumberValid = true;
        var chequeuNumberValid = true;

        if (!$.isNumeric(sortCodeOneTextBox.val()) || sortCodeOneTextBox.val().length != 2) {
            sortCodeValid = false;
        }

        if (!$.isNumeric(sortCodeTwoTextBox.val()) || sortCodeTwoTextBox.val().length != 2) {
            sortCodeValid = false;
        }

        if (!$.isNumeric(sortCodeThreeTextBox.val()) || sortCodeThreeTextBox.val().length != 2) {
            sortCodeValid = false;
        }

        if (sortCodeOneTextBox.val() == '00' && sortCodeTwoTextBox.val() == '00' && sortCodeThreeTextBox.val() == '00') {
            sortCodeValid = false;
        }

        if (sortCodeOneTextBox.val() == '' || sortCodeTwoTextBox.val() == '' || sortCodeThreeTextBox.val() == '') {
            sortCodeValid = false;
        }

        if (!sortCodeValid) {
            sortCodeError.text("Sort code is not valid");
        } else {
            sortCodeError.text("");
        }

        if (accountNumberTextBox.val() == '00000000') {
            accountNumberValid = false;
        }

        if (accountNumberTextBox.val() == '') {
            accountNumberValid = false;
        }

        if (!$.isNumeric(accountNumberTextBox.val()) || accountNumberTextBox.val().length != 8) {
            accountNumberValid = false;
        }

        if (!accountNumberValid) {
            accountNumerError.text("Account number is not valid");
        } else {
            accountNumerError.text("");
        }

        if (chequeNumberTextBox.val() == '000000') {
            chequeuNumberValid = false;
        }

        if (chequeNumberTextBox.val() == '') {
            chequeuNumberValid = false;
        }

        if (!$.isNumeric(chequeNumberTextBox.val()) || chequeNumberTextBox.val().length != 6) {
            chequeuNumberValid = false;
        }

        if (!chequeuNumberValid) {
            chequeNumberError.text("Cheque number is not valid");
        } else {
            chequeNumberError.text("");
        }

        if (sortCodeValid && accountNumberValid && chequeuNumberValid) {
            $('#additionalSetupModal').modal('hide');
        }

        hiddenSortCodeOne.val(sortCodeOneTextBox.val());
        hiddenSortCodeTwo.val(sortCodeTwoTextBox.val());
        hiddenSortCodeThree.val(sortCodeThreeTextBox.val());
        hiddenAccountNumber.val(accountNumberTextBox.val());
        hiddenChequeNumber.val(chequeNumberTextBox.val());
    };

    submitJobs.getSubmitData = function () {
        if (showAdditionalInfo.val() == 'True') {
            sortCodeOneTextBox.val(hiddenSortCodeOne.val());
            sortCodeTwoTextBox.val(hiddenSortCodeTwo.val());
            sortCodeThreeTextBox.val(hiddenSortCodeThree.val());
            accountNumberTextBox.val(hiddenAccountNumber.val());
            chequeNumberTextBox.val(hiddenChequeNumber.val());

            if (sortCodeOneTextBox.val() == '') {
                sortCodeOneTextBox.val("00");
            }

            if (sortCodeTwoTextBox.val() == '') {
                sortCodeTwoTextBox.val("00");
            }

            if (sortCodeThreeTextBox.val() == '') {
                sortCodeThreeTextBox.val("00");
            }

            if (accountNumberTextBox.val() == '') {
                accountNumberTextBox.val("00000000");
            }

            if (chequeNumberTextBox.val() == '') {
                chequeNumberTextBox.val("000000");
            }
        }

        var sortCodeOne = sortCodeOneTextBox.val();
        var sortCodeTwo = sortCodeTwoTextBox.val();
        var sortCodeThree = sortCodeThreeTextBox.val();
        var accountNumber = accountNumberTextBox.val();
        var chequeNumber = chequeNumberTextBox.val();
        var requiresAdditionalInfo = requiresAdditionalInfoHiddenField.val();
        var additionalFileContent = $('#fundInfo').text();

        var additionalSetupViewModel = {
            SortCodeOne: sortCodeOne,
            SortCodeTwo: sortCodeTwo,
            SortCodeThree: sortCodeThree,
            AccountNumber: accountNumber,
            ChequeNumber: chequeNumber,
            AdditionalFileContent: additionalFileContent
        };

        var manCo = manCoDropDown.val();
        var docTypeCode = documentDropDown.val();
        var selectedFolder = selectedFolderDropDown.val();
        var docTypeName = $("#SelectedDocTypeId option:selected").text();
        var allowReprocessing = allowReprocessingCheckBox.prop('checked');

        var chosenFiles = [];
        $("#ChosenFiles option").each(function () {

            chosenFiles.push($(this).val());
        });

        var createJobViewModel = {
            manCo: manCo,
            docTypeCode: docTypeCode,
            docTypeName: docTypeName,
            chosenFiles: chosenFiles,
            allowReprocessing: allowReprocessing,
            additionalSetupViewModel: additionalSetupViewModel,
            additionalInfoRequired: requiresAdditionalInfo,
            selectedFolder: selectedFolder
        };

        if (submitJobs.validateJobCreationData(createJobViewModel)) {
            submitJobs.saveJobData(createJobViewModel);
        }
    };

    submitJobs.validateJobCreationData = function (createJobViewModel) {
        if (createJobViewModel.chosenFiles < 1) {
            var $theAlert = $('.alert');
            $theAlert.hide();
            utility.addBootStrapError('Please select at least one file to process.', "Message");

            errorMessage.show();
            return false;
        } else {
            return true;
        }
    };

    submitJobs.saveJobData = function (createJobViewModel) {
        var ajaxObject = {
            data: JSON.stringify({ createJobViewModel: createJobViewModel }),
            url: utility.getBaseUrl() + "/" + "SubmitJobs/Create",
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            traditional: true,
            success: function (result) {
                if (result.Error == "") {
                    window.location.href = result.Url;
                } else {
                    //$('#fileAlreadyProcessed').modal('show');
                    //alert(result.Error);
                    var $theAlert = $('.alert');
                    $theAlert.hide();

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

    var setAdditionalValueDefaults = function () {
        sortCodeOneTextBox = $("#SortCodeOne");
        sortCodeTwoTextBox = $("#SortCodeTwo");
        sortCodeThreeTextBox = $("#SortCodeThree");
        accountNumberTextBox = $("#AccountNumber");
        chequeNumberTextBox = $("#ChequeNumber");

        hiddenSortCodeOne = $("#hSortCodeOne");
        hiddenSortCodeTwo = $("#hSortCodeTwo");
        hiddenSortCodeThree = $("#hSortCodeThree");
        hiddenAccountNumber = $("#hAccountNumber");
        hiddenChequeNumber = $("#hChequeNumber");

        sortCodeOneTextBox.val("00");
        sortCodeTwoTextBox.val("00");
        sortCodeThreeTextBox.val("00");
        accountNumberTextBox.val("00000000");
        chequeNumberTextBox.val("000000");

        sortCodeOneTextBox.keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        sortCodeTwoTextBox.keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        sortCodeThreeTextBox.keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        accountNumberTextBox.keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        chequeNumberTextBox.keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        hiddenSortCodeOne.val(sortCodeOneTextBox.val());
        hiddenSortCodeTwo.val(sortCodeTwoTextBox.val());
        hiddenSortCodeThree.val(sortCodeThreeTextBox.val());
        hiddenAccountNumber.val(accountNumberTextBox.val());
        hiddenChequeNumber.val(chequeNumberTextBox.val());
    };

    var initialize = function () {
        errorMessage = $("#Message");
        errorDivTxt = $(".alert-danger").text();
        manCoDropDown = $("#SelectedManCoId");
        documentDropDown = $("#SelectedDocTypeId");
        selectedFolderDropDown = $("#SelectedFolder");
        manCoDropDownChange();
        documentDropDownChange();
        selectedFolderDropDownChange();

        addAllButton = $("#btnAddAll");
        addButton = $("#btnAdd");
        removeButton = $("#btnRemove");
        removeAllButton = $("#btnRemoveAll");
        fileSelectedMessage = $("#fileSelectedMessage");

        availableFilesSearch = $("#filesSearch");

        //addAllButtonClick();
        //addSelectedButtonClick();
        //addRemoveButtonClick();
        //addRemoveAllButtonClick();
        copyFolderLocationButton = $("#copyFolderLocation");
        copyFolderLocationButton.hide();

        additionalSetup = $("#additionalSetup");
        additionalSetupClick();

        setAdditionalValueDefaults();

        allowReprocessingCheckBox = $("#AllowReprocessing");

        requiresAdditionalInfoHiddenField = $("#RequiresAdditionalInfo");
        saveAdditionalInfoButton = $("#saveAdditionalInfo");
        saveAdditionalInfoClick();
        sortCodeError = $("#sortCodeError");
        accountNumerError = $("#accountNumerError");
        chequeNumberError = $("#chequeNumberError");
        fundInfotxt = $("#fundInfo");

        showAdditionalInfo = $("#ShowAdditionalInfo");

        additionalSetup.hide();
        if (showAdditionalInfo.val() == "True") {
            additionalSetup.show();
        }

        allFiles = [];

        $('#AvailableFiles').find('option').each(function () {
            allFiles.push({ value: $(this).val(), text: $(this).text() });
        });

        $('#AvailableFiles').filterByText($('#filesSearch'), true);

        //if (errorDivTxt.length > 64) {
        //  errorMessage.show();
        //} else {
        // errorMessage.hide();
        //}
    };


    var initializeDrocument = function () {
        errorMessage = $("#Message");
        errorDivTxt = $(".alert-danger").text();
        manCoDropDown = $("#SelectedManCoId");
        documentDropDown = $("#SelectedDocTypeId");
        selectedFolderDropDown = $("#SelectedFolder");
        manCoDropDownChange();
        //documentDropDownChange();
        selectedFolderDropDownChange();
        availableFilesSearch = $("#filesSearch");

        addAllButton = $("#btnAddAll");
        addButton = $("#btnAdd");
        removeButton = $("#btnRemove");
        removeAllButton = $("#btnRemoveAll");
        fileSelectedMessage = $("#fileSelectedMessage");

        addAllButtonClick();
        addSelectedButtonClick();
        addRemoveButtonClick();
        addRemoveAllButtonClick();

        additionalSetup = $("#additionalSetup");
        additionalSetupClick();

        submitjobButton = $("#submitjob");
        submitjobButtonClick();

        copyFolderLocationButton = $("#copyFolderLocation");
        copyFolderLocationButtonClick();

        allowReprocessingCheckBox = $("#AllowReprocessing");
        setAdditionalValueDefaults();

        requiresAdditionalInfoHiddenField = $("#RequiresAdditionalInfo");
        saveAdditionalInfoButton = $("#saveAdditionalInfo");
        saveAdditionalInfoClick();
        sortCodeError = $("#sortCodeError");
        accountNumerError = $("#accountNumerError");
        chequeNumberError = $("#chequeNumberError");
        fundInfotxt = $("#fundInfo");

        showAdditionalInfo = $("#ShowAdditionalInfo");

        additionalSetup.hide();
        if (showAdditionalInfo.val() == "True") {
            additionalSetup.show();
        }

        allFiles = [];

        $('#AvailableFiles').find('option').each(function () {
            allFiles.push({ value: $(this).val(), text: $(this).text() });
        });

        $('#AvailableFiles').filterByText($('#filesSearch'), true);
    };

    var initializeFoler = function () {
        errorMessage = $("#Message");
        errorDivTxt = $(".alert-danger").text();
        manCoDropDown = $("#SelectedManCoId");
        documentDropDown = $("#SelectedDocTypeId");
        selectedFolderDropDown = $("#SelectedFolder");
        manCoDropDownChange();
        //documentDropDownChange();
        selectedFolderDropDownChange();

        addAllButton = $("#btnAddAll");
        addButton = $("#btnAdd");
        removeButton = $("#btnRemove");
        removeAllButton = $("#btnRemoveAll");
        fileSelectedMessage = $("#fileSelectedMessage");

        addAllButtonClick();
        addSelectedButtonClick();
        addRemoveButtonClick();
        addRemoveAllButtonClick();

        additionalSetup = $("#additionalSetup");
        submitjobButton = $("#submitjob");
        allowReprocessingCheckBox = $("#AllowReprocessing");

        submitjobButtonClick();

        copyFolderLocationButton = $("#copyFolderLocation");
        copyFolderLocationButtonClick();

        setAdditionalValueDefaults();

        requiresAdditionalInfoHiddenField = $("#RequiresAdditionalInfo");
        saveAdditionalInfoButton = $("#saveAdditionalInfo");
        saveAdditionalInfoClick();
        sortCodeError = $("#sortCodeError");
        accountNumerError = $("#accountNumerError");
        chequeNumberError = $("#chequeNumberError");
        fundInfotxt = $("#fundInfo");

        showAdditionalInfo = $("#ShowAdditionalInfo");

        additionalSetup.hide();
        if (showAdditionalInfo.val() == "True") {
            additionalSetup.show();
        }

        allFiles = [];

        $('#AvailableFiles').find('option').each(function () {
            allFiles.push({ value: $(this).val(), text: $(this).text() });
        });

        $('#AvailableFiles').filterByText($('#filesSearch'), true);
        //if (errorDivTxt.length > 64) {
        //  errorMessage.show();
        //} else {
        // errorMessage.hide();
        //}
    };


    (function () {
        initialize();
    })();



}(jQuery, DocProcessing));
