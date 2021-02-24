(function () {
    $(function () {
        $(`.close`).on('click', function () {
            var queryId = $(this).data("queryid");
            ////alert("delete " + postId);
            $.ajax({
                url: domainUrl + "/Search/Delete/" + queryId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    //window.location.href = window.location.href;
                    $(`#${queryId}`).remove();
                })
                .fail(function (jqXHR, textStatus) {
                    ////alert("Request Administration/Delete/ failed: " + textStatus);
                });
        });
    });

})();