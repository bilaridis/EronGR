(function () {
    const predefinedTextArray = JSON.parse(textLocalised);
    $(function () {

        //validate.options = { format: "flat" };
        //validate.async.options = { format: "flat", cleanAttributes: true };
        validate.validators.presence.options = {
            message: function () {
                //predefinedTextArray = JSON.parse(textLocalised);
                return predefinedTextArray.obligatory;
            }
        };

        //$(".mdb-select").materialSelect();
        var $valueSpan = $('.valueSpan');
        var $value = $('#Post_PriceTotal');
        $valueSpan.html($value.val());
        $value.on('input change', () => {
            $valueSpan.html($value.val());
        });


        // Hook up the form so we can prevent it from being posted
        var form = document.querySelector("form#main");
        form.addEventListener("submit", function (ev) {
            ev.preventDefault();
            handleFormCreate(form);
        });

        // Hook up the inputs to validate on the fly
        var inputs = document.querySelectorAll("input, textarea, select")
        for (var i = 0; i < inputs.length; ++i) {
            inputs.item(i).addEventListener("change", function (ev) {
                var errors = validate(form, constraints) || {};
                showErrorsForInput(this, errors[this.name])
            });
        }

        $('#selectType').change(function () {
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
                        //this wrapped in jQuery will give us the current .letter-q div
                        var urii = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
                        $("#selectSubType").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    $("#selectSubType").materialSelect();
                    $("#selectSubType").parent().find('input')
                        .val("")
                        .removeAttr('readonly')
                        //.attr("placeholder", "Είδος Ακινήτου 1")
                        .prop('required', true);

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
                    $("#selectArea").materialSelect("destroy");
                    $("#selectArea").empty();
                    $("#selectArea").append('<option value=""></option>');
                    $.each(jObj1, function (index, value) {
                        $("#selectArea").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    $("#selectArea").materialSelect();
                    $("#selectArea").parent().find('input')
                        .val("")
                        .removeAttr('readonly')
                        //.attr("placeholder", "Δήμο/Συνοικία 1")
                        .prop('required', true);

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        });
        $('#selectType').change();
        $("#selectParentArea").change();

    });
    var constraints = constraints = {
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


    function verifyForm(formName) {
        var verified = '<i class="fa fa-check-circle" aria-hidden="true" style="color:green;float:right;">';
        var warning = '<i class="fa fa-check-circle" aria-hidden="true" style="color:orange;float:right;">';
        var denied = '<i class="fa fa-exclamation-circle" aria-hidden="true" style="color:green;float:right;">'

        var divId = "#" + formName + " > div > input";
        var divIdValid = "#" + formName + " > div";

        var formControlsAll = $(divId);
        var formControlsInvalid = $(divIdValid).find("input").filter(function () { return !this.value; });

        if (formControlsInvalid.length > 0) {
            //console.log("Warning : " + formName + " Invalid :" + formControlsValid.length);
            //console.log(formControlsValid);
            changeFontAwesome(formName, warning);
        } else {
            //console.log("Valid :" + formName + " Invalid :" + formControlsValid.length);
            //console.log(formControls);
            changeFontAwesome(formName, verified);
        }
    }

    function changeFontAwesome(formName, fontElement) {
        $("div").find("[data-form='" + formName + "'] > div > h5 > i").remove()
        $("div").find("[data-form='" + formName + "'] > div > h5").prepend(fontElement);
    }

    function removePropFromCss(element, property) {
        var styleObject = element.removeProp(property);
    }

    function updateLabelValue(val, id) {
        ////alert(val + '  ' + id);
        document.getElementById(id).innerHTML = val;
    }


    function handleFormCreate(form, input) {
        // validate the form against the constraints
        var errors = validate(form, constraints, { fullMessages: false });
        // then we update the form to reflect the results
        showErrors(form, errors || {});
        if (!errors) {
            showSuccessCreate();
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
                addError(messages, error);
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
    }
    function showSuccessCreate() {

        var dataString = $("form#main").serialize();
        //console.log(dataString);
        $.ajax({
            type: "POST",
            url: window.location.href,
            data: dataString,
            success: function (data) {
                window.location.href = domainUrl + "/Posts/Submit/" + data;
            }
        });
    }
})();
