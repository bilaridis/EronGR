(function () {
    const predefinedTextArray = JSON.parse(textLocalised);

    $(function () {
        $('.stepper').mdbStepper();

        $(".next-step").on('click', function (event) {
            var form = document.querySelector("form#main");
            event.preventDefault();
            //$('#horizontal-stepper').nextStep();
            // validate the form against the constraints
            var errors = validate(form, constraints, { fullMessages: false });
            // then we update the form to reflect the results
            showErrors(form, errors || {});
        });
        //validate.options = { format: "flat" };
        //validate.async.options = { format: "flat", cleanAttributes: true };
        validate.validators.presence.options = {
            message: function () {
                //predefinedTextArray = JSON.parse(textLocalised);
                return predefinedTextArray.obligatory;
            }
        };

        // Hook up the form so we can prevent it from being posted
        var form = document.querySelector("form#main");
        $(".btn-outline-success").on("click", function (ev) {
            ev.preventDefault();
            handleFormSubmit(form);
        });

        if ($("#uploader").length > 0) {
            var uploader = new qq.FineUploader({
                element: document.getElementById("uploader"),
                request: {
                    endpoint: '/Posts/Submit/Upload/' + $("#Post_id").val()
                },
                validation: {
                    allowedExtensions: ['jpeg', 'jpg', 'png'],
                    itemLimit: 21,
                    sizeLimit: 5242880 // 50 kB = 50 * 1024 bytes
                },
                session: {
                    endpoint: '/Posts/Submit/GetUploads/' + $("#Post_id").val()
                },
                deleteFile: {
                    enabled: true,
                    forceConfirm: true,
                    endpoint: '/Posts/Submit/DeletePhoto'
                },
                autoUpload: true,
                debug: false
            });
        }

        function loadParentAreas() {
            //AreaOfPost 
            $.ajax({
                url: domainUrl + "/api/GetAreas/0",
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var childAreasForAppend = JSON.parse(msg);
                    $("#selectParentArea").materialSelect("destroy");
                    $("#selectParentArea").empty();
                    $.each(childAreasForAppend, function (index, value) {
                        var checked = value.Value == parsedObject.Area ? 'selected' : '' || '';
                        $("#selectParentArea").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    $("#selectParentArea").materialSelect();
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        }

        function loadSubAreas() {
            //TypeOfPost
            var parsedObject = JSON.parse(PostObject);
            //SubAreasOfPost
            $.ajax({
                url: domainUrl + "/api/GetAreas/" + parsedObject.Area,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var childAreasForAppend = JSON.parse(msg);
                    $("#selectArea").materialSelect("destroy");
                    $("#selectArea").empty();
                    $.each(childAreasForAppend, function (index, value) {
                        var checked = value.Value == parsedObject.SubAreaId ? 'selected' : '' || '';
                        $("#selectArea").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    $("#selectArea").materialSelect();
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        }

        $(`label[data-value="${$("#Energy").val()}"]`).addClass("btn-outline-info");
        $('.efficiency').click(function () {
            $(".energyEfficiency").find("label").removeClass("btn-outline-info");
            //alert($(this).data("value"));
            $("#Energy").val($(this).data("value"));
            $(this).addClass("btn-outline-info");
        });


        $('#selectType').change(function () {
            var parsedObject = JSON.parse(PostObject);
            $.ajax({
                url: domainUrl + "/api/Types/" + $(this).find(":selected").val(),
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var jObj = JSON.parse(msg);
                    $("#selectSubType").materialSelect("destroy");
                    $("#selectSubType").empty();
                    $("#selectSubType").append('<option value=""></option>');
                    $.each(jObj, function (index, value) {
                        var checked = value.Value == parsedObject.SubTypeId ? 'selected' : '' || '';
                        $("#selectSubType").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    $("#selectSubType").materialSelect();


                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request failed: " + textStatus);
                });
        });

        $("#selectParentArea").change(function () {
            $.ajax({
                url: domainUrl + "/api/GetAreas/" + $(this).val(),
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var jObj1 = JSON.parse(msg);
                    $("#selectArea").empty();
                    $("#selectArea").append('<option value=""></option>');
                    $.each(jObj1, function (index, value) {
                        $("#selectArea").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    $("#selectArea").materialSelect("destroy");

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        });

        $("#toggle_Premium").change(function () {
            if ($(this).is(':checked')) {
                $('#postId').val($(this).data('postid'));
                $('.bd-example-modal-xl').modal('show');
                $("#toggle_Premium").prop('checked', false);
            }
            else {
                
            }
        });
        $('.product-activate').on('click', function () {
            var parsedObject = JSON.parse(PostObject);
            var jsonData = {
                productId: $(this).data('product')
            };
            $.ajax({
                url: `${domainUrl}/api/Order/${parsedObject.id}`,
                method: "POST",
                data: jsonData
            })
                .done(function (msg) {
                    $(`#toggle_Premium`).attr({ 'disabled': 'disabled' });
                    $("#toggle_Premium").prop('checked', true);
                    $('.bd-example-modal-xl').modal('hide');
                    $('#Post_Premium').val('True');
                    $('.premium-feature').removeAttr('disabled');
                    toastr.success('Ενεργοποιήθηκε επιτυχώς η αγγελία με τα Premium Χαρακτηριστικά.');
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request failed: " + textStatus);
                    $('.bd-example-modal-xl').modal('show');
                    toastr.error('Κάτι πήγε στραβά. Θα το κοιτάξουμε αμέσως.');
                    $(`#toggle_Premium`).removeAttr('disabled');
                    $("#toggle_Premium").prop('checked', false);
                    $('#Post_Premium').val('False');
                });
        });

        var parsedObject = JSON.parse(PostObject);
        $('#inp_Category option[value="' + parsedObject.SaleCategory + '"]').attr('selected', 'selected');
        $('#selectType option[value="' + parsedObject.TypeId + '"]').attr('selected', 'selected');
        loadParentAreas();
        loadSubAreas();
        $('#selectType').change();
        $('.material-tooltip-main').tooltip({
            template: '<div class="tooltip md-tooltip"><div class="tooltip-arrow md-arrow"></div><div class="tooltip-inner md-inner"></div></div>'
        });

        $(`input[data-activates="select-options-selectConstruction"]`).attr('required', false);
        $(`input[data-activates="select-options-selectRennovation"]`).attr('required', false);
    });

    // These are the constraints used to validate the form
    var constraints = {
        'Post.SaleCategory': {
            presence: true
        },
        'Post.TypeId': {
            presence: true
        },
        'Post.SubTypeId': {
            presence: true
        },
        'Post.PriceTotal': {
            // You need to pick a username too
            presence: false,
            // And it must be between 3 and 20 characters long
            length: {
                minimum: 3,
                maximum: 20,
                message: predefinedTextArray.moreThan100
            },
            format: {
                // We don't allow anything that a-z and 0-9
                pattern: "[0-9]+",
                // but we don't care if the username is uppercase or lowercase
                flags: "i",
                message: predefinedTextArray.onlyInteger
            }
        },
        'Post.Area': {
            presence: true
        },
        'Post.SubAreaId': {
            presence: true
        },
        'Post.Address': {
            // The user needs to give a birthday
            presence: false,
            // And it must be between 3 and 20 characters long
            length: {
                minimum: 3,
                message: predefinedTextArray.minimumThreeCharacters
            }
        },
        'Post.Square': {
            // The user needs to give a birthday
            presence: true
        },

        'Post.PostalCode': {
            presence: false,
            // Zip is optional but if specified it must be a 5 digit long number
            format: {
                pattern: "\\d{5}",
                message: predefinedTextArray.maximumCharacters5
            }
        },
    };
    // Before using it we must add the parse and format functions
    // Here is a sample implementation using moment.js
    validate.extend(validate.validators.datetime, {
        // The value is guaranteed not to be null or undefined but otherwise it
        // could be anything.
        parse: function (value, options) {
            return + moment.utc(value);
        },
        // Input is a unix timestamp
        format: function (value, options) {
            var format = options.dateOnly ? "YYYY-MM-DD" : "YYYY-MM-DD hh:mm:ss";
            return moment.utc(value).format(format);
        }
    });

    function handleFormSubmit(form, input) {
        // validate the form against the constraints
        var errors = validate(form, constraints, { fullMessages: false });
        // then we update the form to reflect the results
        showErrors(form, errors || {});
        if (!errors) {
            showSuccessSubmit();
        }
        else {

        }
    }

    // Updates the inputs with the validation errors
    function showErrors(form, errors) {
        // We loop through all the inputs and show the errors for that input
        _.each(form.querySelectorAll("input[name], select[name]"), function (input) {
            // Since the errors can be null if no errors were found we need to handle
            // that
            showErrorsForInput(input, errors && errors[input.name]);
        });
    }

    // Shows the errors for a specific input
    function showErrorsForInput(input, errors) {
        // This is the root of the input
        var formGroup = closestParent(input.parentNode, "form-group");
        // Find where the error messages will be insert into
        if (formGroup == null) return;
        messages = formGroup.querySelector(".messages");
        // First we remove any old messages and resets the classes
        resetFormGroup(formGroup);
        // If we have errors
        if (errors) {
            // we first mark the group has having errors
            formGroup.classList.add("has-error");
            // then we append all the errors
            _.each(errors, function (error) {
                addError(messages, `${error}`);
            });
        } else {
            // otherwise we simply mark it as success
            formGroup.classList.add("has-success");
        }
    }

    // Recusively finds the closest parent that has the specified class
    function closestParent(child, className) {
        if (!child || child == document) {
            return null;
        }
        if (child.classList.contains(className)) {
            return child;
        } else {
            return closestParent(child.parentNode, className);
        }
    }

    function resetFormGroup(formGroup) {
        // Remove the success and error classes
        formGroup.classList.remove("has-error");
        formGroup.classList.remove("has-success");
        // and remove any old messages
        _.each(formGroup.querySelectorAll(".help-block.error"), function (el) {
            el.parentNode.removeChild(el);
        });
    }

    // Adds the specified error with the following markup
    // <p class="help-block error">[message]</p>
    function addError(messages, error) {
        var block = document.createElement("p");
        block.classList.add("help-block");
        block.classList.add("error");
        block.classList.add("text-danger");
        block.innerText = error;
        messages.appendChild(block);

        //toastr.error(error, "Error Validation");
    }

    function showSuccessSubmit() {
        var dataString = $("form#main").serialize();
        //console.log(dataString);
        $.ajax({
            type: "POST",
            url: window.location.href,
            data: dataString,
            success: function () {
                //toastr.success('Η Αγγελία δημοσιεύτηκε με επιτυχία !');
                window.location.href = domainUrl + "/Administration/#1";
                // Display message back to the user here
            },
            failed: function () {
                window.location.href = domainUrl + "/Administration/#2";
            }
            
        });
        window.location.href = domainUrl + "/Administration/#3";
    }
})();