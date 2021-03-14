(function () {
    const predefinedTextArray = JSON.parse(textLocalised);

    $(function () {

        var inputs = document.querySelectorAll("input[required], select[required], input[validation], textarea[required]");
        var form = document.querySelector("form#main");
        if (JSON.parse(PostObject).Premium) {
            $.each($(".premium-feature"), function (index, value) {
                $(value).removeAttr("disabled");
            });
        }
        document.querySelector('.stepper .stepper-step').addEventListener('onChangeStep.mdb.stepper', (event) => {
            if (validateConstraintsAndReturnErrors(form, false)) {
                //event.preventDefault();
                //e.target.querySelector('input').setCustomValidity('Invalid');
            }
        });

        for (var i = 0; i < inputs.length; ++i) {
            inputs.item(i).addEventListener("change", function (ev) {
                var errors = validate(form, constraints) || {};
                showErrorsForInput(this, errors[this.name])
            });
        }


        $(".submitForm").on("click", function (ev) {
            ev.preventDefault();
            validateConstraintsAndReturnErrors(form, true);
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
                    //$("#selectParentArea").materialSelect("destroy");
                    $("#selectParentArea").empty();
                    $.each(childAreasForAppend, function (index, value) {
                        var checked = value.Value == parsedObject.Area ? 'selected' : '' || '';
                        $("#selectParentArea").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    //$("#selectParentArea").materialSelect();
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
                    //$("#selectArea").materialSelect("destroy");
                    $("#selectArea").empty();
                    $.each(childAreasForAppend, function (index, value) {
                        var checked = value.Value == parsedObject.SubAreaId ? 'selected' : '' || '';
                        $("#selectArea").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    //$("#selectArea").materialSelect();
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        }

        //$(`label[data-value="${$("#Energy").val()}"]`).addClass("btn-outline-info");

        $('.btn-class').click(function () {
            $(".energyEfficiency").find("label").removeClass("btn-outline-info").removeClass("bg-white");
            //alert($(this).data("value"));
            //$("#Energy").val($(this).data("value"));
            $(this).addClass("btn-outline-info").addClass("bg-white");
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
                    //$("#selectSubType").materialSelect("destroy");
                    $("#selectSubType").empty();
                    $("#selectSubType").append('<option value=""></option>');
                    $.each(jObj, function (index, value) {
                        var checked = value.Value == parsedObject.SubTypeId ? 'selected' : '' || '';
                        $("#selectSubType").append('<option value="' + value.Value + '" ' + checked + '>' + value.Text + '</option>');
                    });
                    //$("#selectSubType").materialSelect();


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
                    //$("#selectArea").materialSelect("destroy");

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        });

        $(`input[name="Post.Premium"]`).click(function () {
            $('#postId').val($(this).data('postid'));
            $('.bd-example-modal-xl').modal('show');
            $("#option1").prop('checked', true);
            $("#option2").prop('checked', false);
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
                    $(`input[name="Post.Premium"]`).attr({ 'disabled': 'disabled' });
                    $("#option1").prop('checked', false);
                    $("#option2").prop('checked', true);
                    $('.bd-example-modal-xl').modal('hide');
                    $('.premium-feature').removeAttr('disabled');
                })
                .fail(function (jqXHR, textStatus) {
                    $('.bd-example-modal-xl').modal('show');
                    toastr.error('Κάτι πήγε στραβά. Θα το κοιτάξουμε αμέσως.');
                    $(`input[name="Post.Premium"]`).removeAttr('disabled');
                    $("#option1").prop('checked', true);
                    $("#option2").prop('checked', false);
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


    function validateConstraintsAndReturnErrors(form, submitFormFlag) {
        var errors = validate(form, constraints, { fullMessages: false });
        showErrors(form, errors || {});
        if (!errors) {
            if (submitFormFlag)
                showSuccessSubmit();
        }
        return errors;
    }

    function showErrors(form, errors) {
        //document.querySelector(".stepper-active").classList.remove("stepper-invalid");
        _.each(form.querySelectorAll("input[required], select[required], input[validation], textarea[required]"), function (element) {
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
        else if (errors != undefined) {
            if (element.hasAttribute("validation")) {
                element.classList.remove("is-invalid");
                element.classList.add("is-valid");
            }
            else {
                formGroup.querySelector("input").classList.remove("is-invalid");
                formGroup.querySelector("input").classList.add("is-valid");
            }
        }
        else {

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
        closestParent(element.parentNode, "stepper-step").classList.remove("stepper-invalid");
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
        closestParent(element.parentNode, "stepper-step").classList.add("stepper-invalid");
        if (element.hasAttribute("validation")) {
            element.classList.add("is-invalid");
        }
        else {
            formGroup.querySelector("input").classList.add("is-invalid");
        }
        //parent.classList.add("mb-4");
        //messages.classList.add("d-block");
        //messages.innerText = error;
    }

    function showSuccessSubmit() {
        var dataString = $("form#main").serialize();
        $.ajax({
            type: "POST",
            url: window.location.href,
            data: dataString,
            success: function () {
                window.location.href = domainUrl + "/Administration/#1";
            },
            failed: function () {
                window.location.href = domainUrl + "/Administration/#2";
            }

        });
        window.location.href = domainUrl + "/Administration/#3";
    }
})();