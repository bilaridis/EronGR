(function () {
    const predefinedTextArray = JSON.parse(textLocalised);
    $(function () {

        // Hook up the form so we can prevent it from being posted
        var form = document.querySelector("form#main");
        form.addEventListener("submit", function (ev) {
            ev.preventDefault();
            handleFormCreate(form);
        });

        // Hook up the inputs to validate on the fly
        var inputs = document.querySelectorAll("input[required], select[required], input[validation], textarea[required]")
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
                    //$("#selectSubType").materialSelect("destroy");
                    $("#selectSubType").empty();
                    $("#selectSubType").append('<option value=""></option>');
                    $.each(jObj, function (index, value) {
                        //this wrapped in jQuery will give us the current .letter-q div
                        var urii = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
                        $("#selectSubType").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    //$("#selectSubType").materialSelect();
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
                    //$("#selectArea").materialSelect("destroy");
                    $("#selectArea").empty();
                    $("#selectArea").append('<option value=""></option>');
                    $.each(jObj1, function (index, value) {
                        $("#selectArea").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    //$("#selectArea").materialSelect();
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

    function handleFormCreate(form) {
        var errors = validate(form, constraints, { fullMessages: false });
        showErrors(form, errors || {});
        if (!errors) {
            showSuccessCreate();
        }
    }

    function showErrors(form, errors) {
        document.querySelector(".stepper-active").classList.remove("stepper-invalid");
        _.each(form.querySelector('.stepper-active').querySelectorAll("input[required], select[required], input[validation], textarea[required]"), function (element) {
            showErrorsForInput(element, errors && errors[element.name]);
        });
    }

    function showErrorsForInput(element, errors) {
        var formGroup = closestParent(element.parentNode, "input-group");
        if (formGroup == null) return;
        messages = formGroup.querySelector(".invalid-tooltip");
        resetFormGroup(formGroup, element);
        if (errors) {
            _.each(errors, function (error) {
                addError(formGroup, element, messages, error);
            });
        }
        else {
            if (element.hasAttribute("validation")) {
                element.classList.remove("is-invalid");
                element.classList.add("is-valid");
            }
            else {
                formGroup.querySelector("input").classList.remove("is-invalid");
                formGroup.querySelector("input").classList.add("is-valid");
            }
        }
    }

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

    function resetFormGroup(formGroup, element) {
        if (element.hasAttribute("validation")) {
            element.classList.remove("is-invalid");
            element.classList.remove("is-valid");
        }
        else {
            formGroup.querySelector("input").classList.remove("is-invalid");
            formGroup.querySelector("input").classList.remove("is-valid");
        }
    }

    function addError(formGroup, element, messages, error) {
        document.querySelector(".stepper-active").classList.add("stepper-invalid");
        if (element.hasAttribute("validation")){
            element.classList.add("is-invalid");
        }
        else {
            formGroup.querySelector("input").classList.add("is-invalid");
        }
        //parent.classList.add("mb-4");
        //messages.classList.add("d-block");
        //messages.innerText = error;
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
