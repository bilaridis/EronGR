(function () {
    $(function () {
        console.log("Before Load" + moment().toDate());
        $.ajax({
            url: domainUrl + "/api/GetAreas/0",
            method: "GET",
            dataType: 'text',
            async: true,
        })
            .done(function (msg) {
                console.log("After Load" + moment().toDate());
                var childAreasForAppend = JSON.parse(msg);
                //$("#selectParentArea").materialSelect("destroy");
                $("#selectParentArea").empty();
                $.each(childAreasForAppend, function (index, value) {
                    $("#selectParentArea").append('<option value="' + value.Value + '" >' + value.Text + '</option>');
                });
                //$("#selectParentArea").materialSelect();
                console.log("After Implement" + moment().toDate());
            })
            .fail(function (jqXHR, textStatus) {
                ////alert("Request api/GetArea/ failed: " + textStatus);
            });

        $('#selectType').change(function () {
            $.ajax({
                url: domainUrl + "/api/Types/" + $(this).find(":selected").val(),
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var jObj = JSON.parse(msg);
                    $("#selectSubType").materialSelect("destroy");
                    //$("#selectSubType").empty();
                    $("#selectSubType").append('<option value=""></option>');
                    $.each(jObj, function (index, value) {
                        //this wrapped in jQuery will give us the current .letter-q div
                        var urii = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
                        $("#selectSubType").append('<option value="' + value.Value + '">' + value.Text + '</option>');
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
                    //$("#selectArea").materialSelect("destroy");
                    $("#selectArea").empty();
                    $("#selectArea").append('<option value=""></option>');
                    $.each(jObj1, function (index, value) {
                        $("#selectArea").append('<option value="' + value.Value + '">' + value.Text + '</option>');
                    });
                    //$("#selectArea").materialSelect();

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request api/GetArea/ failed: " + textStatus);
                });
        });
        $('.multi-carousel .multi-carousel-inner').each(function () {
           
            $(".details-heart").unbind('click');
            $(".details-heart").click(function (e) {
                var postId = $(this).data("postid");
                if ($(this).hasClass("details-wish"))
                    ////alert("delete " + postId);
                    $.ajax({
                        url: domainUrl + "/Posts/Details/WishList/" + postId,
                        method: "GET",
                        dataType: 'text'
                    })
                        .done(function (msg) {
                            $(`a.details-heart[data-postid="${postId}"]`).removeClass("details-wish").addClass("details-unwish");
                            $(`a.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("far").addClass("fas");
                        })
                        .fail(function (jqXHR, textStatus) {
                            //alert(jqXHR.status);
                            if (jqXHR.status == 401) {
                                window.location.href = domainUrl + "/Identity/Account/Login?ReturnUrl=%2FHome%2F"
                            }
                            else { }
                        });
                else {
                    $.ajax({
                        url: domainUrl + "/Posts/Details/UnWishList/" + postId,
                        method: "GET",
                        dataType: 'text'
                    })
                        .done(function (msg) {
                            $(`a.details-heart[data-postid="${postId}"]`).removeClass("details-unwish").addClass("details-wish");
                            $(`a.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("fas").addClass("far");
                        })
                        .fail(function (jqXHR, textStatus) {
                            //alert("Request Administration/Delete/ failed: " + textStatus);
                        });
                }
                e.preventDefault();
            });
        });

        $('#modalCoupon').modal('show');
        $('#onlyFirst').on('click', function () {
            $('#modalCoupon').modal('hide');
        });
    });
})();



