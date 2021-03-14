(function () {
    $(function () {

        $(".admin-publish").click(function (e) {
            var postId = $(this).data("postid");
            ////alert("publish " + postId);
            $.ajax({
                url: domainUrl + "/Administration/Publish/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;

                })
                .fail(function (jqXHR, textStatus) {
                    ////alert("Request Administration/Publish/ failed: " + textStatus);
                });
            e.preventDefault();
            toastr.success('Η Αγγελία δημοσιεύτηκε με επιτυχία ! Είναι και πάλι εμφανίσιμη στις διαθέσιμες αγγελίες σας. ');
        });

        $(".admin-delete").click(function (e) {
            var postId = $(this).data("postid");
            ////alert("delete " + postId);
            $.ajax({
                url: domainUrl + "/Administration/Delete/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;

                })
                .fail(function (jqXHR, textStatus) {
                    ////alert("Request Administration/Delete/ failed: " + textStatus);
                });
            e.preventDefault();
            toastr.success('Η Αγγελία αποκρύφθηκε με επιτυχία! Δεν θα είναι πια εμφανίσιμη στις διαθέσιμες αγγελίες σας. ');

        });

        $(".admin-sold").click(function (e) {
            var postId = $(this).data("postid");
            ////alert("sold " + postId);
            $.ajax({
                url: domainUrl + "/Administration/Sold/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;

                })
                .fail(function (jqXHR, textStatus) {
                    ////alert("Request Administration/Sold/ failed: " + textStatus);
                });
            e.preventDefault();
            toastr.success('Η Αγγελία ολοκληρώθηκε με επιτυχία! Δεν θα είναι πια εμφανίσιμη στις διαθέσιμες αγγελίες σας. ');
        });

        $(".admin-reserve").click(function (e) {
            var postId = $(this).data("postid");
            ////alert("sold " + postId);
            $.ajax({
                url: domainUrl + "/Administration/Reserve/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request Administration/Reserve/ failed: " + textStatus);
                });
            e.preventDefault();
            toastr.success('Η Αγγελία κλειδώθηκε με επιτυχία!');
        });

        $(".admin-hide").click(function (e) {
            var postId = $(this).data("postid");
            ////alert("sold " + postId);
            $.ajax({
                url: domainUrl + "/Administration/Hide/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;

                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request Administration/Hide/ failed: " + textStatus);
                });
            e.preventDefault();
            toastr.success('Η Αγγελία ξεκλειδώθηκε με επιτυχία!');
        });

        $(".previewPost").click(function () {

            window.location.href = domainUrl + "/Posts/Details/" + $(this).data("postid");
        });

        $(`input[name="Post.Premium"]`).click(function () {
            $('#postId').val($(this).data('postid'));
            $('.bd-example-modal-xl').modal('show');
            $("#option1").prop('checked', true);
            $("#option2").prop('checked', false);
        });

        $('.product-activate').on('click', function () {
            var jsonData = {
                productId: $(this).data('product')
            };
            $.ajax({
                url: `${domainUrl}/api/Order/${$("#postId").val()}`,
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
        //$(".toggle-premium").change(function () {

        //    if ($(this).is(':checked')) {
        //        $('#postId').val($(this).data('postid'));
        //        $('.bd-example-modal-xl').modal('show');
        //        $(this).prop('checked', false);
        //    }
        //    else {
        //        //$('#Post_Premium').val('False');
        //    }
        //});

        //$('.product-activate').on('click', function () {
        //    var jsonData = {
        //        productId: $(this).data('product')
        //    };
        //    $.ajax({
        //        url: `${domainUrl}/api/Order/${$("#postId").val()}`,
        //        method: "POST",
        //        dataType: 'text',
        //        data: jsonData
        //    })
        //        .done(function (msg) {
        //            //alert(msg);
        //            $('.bd-example-modal-xl').modal('hide');
        //            $(`input[class="toggle-premium"][data-postid="${$("#postId").val()}"]`).attr({ 'disabled': 'disabled' });
        //            $(`input[class="toggle-premium"][data-postid="${$("#postId").val()}"]`).prop('checked', true);
        //        })
        //        .fail(function (jqXHR, textStatus) {
        //            toastr.error('Κάτι πήγε στραβά. Θα το κοιτάξουμε αμέσως.');
        //            $(`input[class="toggle-premium"][data-postid="${$("#postId").val()}"]`).prop('checked', false);
        //        });
        //});

        //$('#dtMaterialDesignExample').DataTable({
        //    "searching": true,
        //    "order": [[1, "desc"]],
        //    "language": {
        //        "info": "Ads _START_ to _END_  -  _TOTAL_ total rows",
        //        "lengthMenu": " _MENU_ ",
        //        "decimal": "",
        //        "emptyTable": "Δεν υπάρχουν δεδομένα στη βάση.",
        //        "infoEmpty": "Εμφανίζονται 0 έως 0 από 0 εγγραφές",
        //        "infoFiltered": "(filtered from _MAX_ total entries)",
        //        "infoPostFix": "",
        //        "thousands": ",",
        //        "loadingRecords": "Σε λίγο θα φορτώσουμε τα δεδομένα...",
        //        "processing": "Επεξεργαζόμαστε κάτι τελευταίο...",
        //        "search": "Αναζήτηση:",
        //        "zeroRecords": "Δεν βρέθηκαν εγγραφες σύμφωνα με την αναζήτηση σας.",
        //        "paginate": {
        //            "first": "First",
        //            "last": "Last",
        //            "next": ">",
        //            "previous": "<"
        //        },
        //        "aria": {
        //            "sortAscending": ": activate to sort column ascending",
        //            "sortDescending": ": activate to sort column descending"
        //        }
        //    }
        //});
        //$('#dtMaterialDesignExample_wrapper .dataTables_filter').find('label').each(function () {
        //    $(this).parent().append($(this).children());
        //});
        //$('#dtMaterialDesignExample_wrapper .dataTables_filter').find('input').each(function () {
        //    const $this = $(this);
        //    $this.attr("placeholder", "Search");
        //    $this.removeClass('form-control-sm');
        //});
        //$('#dtMaterialDesignExample_wrapper .dataTables_length').addClass('d-flex flex-row');
        //$('#dtMaterialDesignExample_wrapper .dataTables_filter').addClass('md-form');
        //$('#dtMaterialDesignExample_wrapper select').removeClass('custom-select custom-select-sm form-control form-control-sm');
        //$('#dtMaterialDesignExample_wrapper select').addClass('mdb-select');
        ////$('#dtMaterialDesignExample_wrapper .mdb-select').materialSelect();
        //$('#dtMaterialDesignExample_wrapper .dataTables_filter').find('label').remove();

    });

})();



