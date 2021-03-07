(function () {
    function noteTemplate(note) {
        return `
          <div id="note_${note.Id}" class="d-flex justify-content-start bg-light rounded-3 shadow-1 w-100">
            <p class="p-3 col-11">
                
              ${note.Note}
              <small class="float-end text-dark mt-4 mt-auto">${moment(note.CreatedAt).locale('el').format('LL')}</small>
                
            </p>
            <button type="button" class="btn-close btn-sm mb-auto ms-auto  deleteNote" aria-label="Close" data-noteid="${note.Id}"></button>
          </div>`

    }

 
    $(function () {
        $(".noteHeader").click(function () {
            $('.friend-list').find('li').each(function (elem) {
                $(this).removeClass("gradient-new-york").addClass("grey").addClass("lighten-3");
            });
            $(this).closest('li').removeClass("grey").removeClass("lighten-3").addClass("gradient-new-york");
            $(".chatList").find('div').each(function () {
                $(this).remove();
            });
            let noteid = $(this).data("noteid");
            let postid = $(this).data("postid");
            $.ajax({
                url: `${domainUrl}/api/GetNotes/${noteid}`,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    var jObj = JSON.parse(msg);
                    $.each(jObj.AspNetUserNotesDetails, function (index, value) {
                        $(".chatList").append(noteTemplate(value));
                    });

                    $(`.sendButton[type="button"]`).data("noteid", postid);

                    $(".deleteNote").click(function () {
                        let noteid = $(this).data("noteid");
                        $.ajax({
                            url: `${domainUrl}/api/DeleteNote/${noteid}`,
                            method: "POST",
                            dataType: 'text'
                        })
                            .done(function (msg) {
                                $(`#note_${noteid}`).remove();
                            })
                            .fail(function (jqXHR, textStatus) {
                                //alert("Request failed: " + textStatus);
                            });
                    });
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request failed: " + textStatus);
                });
            
        });

        $(".deleteNoteGroup").click(function () {
            let noteGroupId = $(this).data("postid");
            $.ajax({
                url: `${domainUrl}/api/DeleteNotes/${noteGroupId}`,
                method: "POST",
                dataType: 'text'
            })
                .done(function (t) {
                    $(`#noteGroup_${noteGroupId}`).remove();
                })
                .fail(function (jqXHR, textStatus) {
                    //alert("Request failed: " + textStatus);
                });
        });

        $(".sendButton").click(function () {
            //alert($(this).data("noteid"));
            let noteGroupId = $(this).data("noteid");
            var dataString = {
                notes : $(`textarea[name="note"]`).val()
            };
            //console.log(dataString);
            $.ajax({
                type: "POST",
                url: `${domainUrl}/api/SaveNote/${noteGroupId}`,
                contentType: "application/json charset=utf-8",
                dataType: "json",
                data: JSON.stringify(dataString)
            })
                .done(function (t) {
                    $(`.noteHeader[data-postid="${noteGroupId}"]`).click();
                }).fail(function (jqXHR, textStatus) {
                    alert("Request failed: " + textStatus);
                });
            $(`textarea[name="note"]`).val("");
        });



    });
})();