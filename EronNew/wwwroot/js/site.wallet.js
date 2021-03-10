(function () {



    $(function () {
        $('.cancel-order').on('click', function () {
            $.ajax({
                url: `${domainUrl}/api/Order/${$(this).data('order')}`,
                method: "DELETE",
                dataType: 'text'
            })
                .done(function (msg) {
                    window.location.href = window.location.href;
                })
                .fail(function (jqXHR, textStatus) {
                    window.location.href = window.location.href;
                });
        });

        var dt = document.getElementById("datatable");
        var advancedSearchInstance = mdb.Datatable.getInstance(dt);
        var advancedSearchInput = document.getElementById('advanced-search-input');
        var search = (value) => {

            let [phrase, columns] = value.split(' in:').map((str) => str.trim());

            if (columns) {
                columns = columns.split(',').map((str) => str.toLowerCase().trim());
                advancedSearchInstance.search(phrase, columns);
            }
            else {
                advancedSearchInstance.search(phrase);
            }

            
        }

        document.getElementById('advanced-search-button').addEventListener('click', (e) => search(advancedSearchInput.value));

        advancedSearchInput.addEventListener('keydown', e => {
            if (e.keyCode === 13) search(e.target.value);
        })


        //$('#dtMaterialDesignExample').DataTable({
        //    "searching": false,
        //    "ordering": true, 
        //    "lengthChange": false,
        //    "order": [[0,"desc"],[1,"desc"]],
        //    "language": {
        //        "info": "Orders _START_ to _END_  of  _TOTAL_ rows",
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
        //$('#dtMaterialDesignExample_wrapper .mdb-select').materialSelect();
        //$('#dtMaterialDesignExample_wrapper .dataTables_filter').find('label').remove();

        //var client_token = "sandbox_x626wj83_2dx46p438cwdzt7m";
        //var form = document.querySelector('#payment-form');
        //braintree.dropin.create({
        //    authorization: client_token,
        //    container: '#bt-dropin',
        //    paypal: {
        //        flow: 'vault'
        //    }
        //}, function (createErr, instance) {
        //    form.addEventListener('submit', function (event) {
        //        event.preventDefault();
        //        instance.requestPaymentMethod(function (err, payload) {
        //            if (err) {
        //                console.log('Error', err);
        //                return;
        //            }
        //            // Add the nonce to the form and submit
        //            document.querySelector('#nonce').value = payload.nonce;
        //            form.submit();
        //        });
        //    });
        //});
    });

})();

/*
 <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
 
 */