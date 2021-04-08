(function () {
    $(function () {
        function addMarkerToGroup(coordinate, html) {
            var outerElement = document.createElement('div'),
                innerElement = document.createElement('div');
            outerElement.className = "eronMarker"
            innerElement.className = "eronIcon"
            outerElement.appendChild(innerElement);
            // Add text to the DOM element
            innerElement.innerHTML = '1';
            function isPin(elem) {
                if (elem.la != undefined) {
                    return elem;
                }

            }
            function changeOpacity(evt) {
                infoBubbleCloseFlag = true;
                evt.target.style.opacity = 0.6;
                var coords = map.screenToGeo(evt.layerX, evt.layerY);
                var minDist = 300;
                var markerDist;
                var objects = map.getObjects().filter(isPin);
                var nearestPin;
                // iterate over objects and calculate distance between them
                for (var i = 0; i < objects.length; i += 1) {
                    markerDist = objects[i].getGeometry().distance(coords);
                    if (markerDist < minDist) {
                        minDist = markerDist;
                        nearestPin = objects[i];
                    }
                }
                var bubble = new H.ui.InfoBubble(nearestPin.getGeometry(), {
                    content: nearestPin.getData()
                });
                bubble.addEventListener('pointerup', function (e) {
                    infoBubbleCloseFlag = true;
                });
                bubble.addEventListener('tap', function (e) {
                    infoBubbleCloseFlag = true;
                });

                ui.addBubble(bubble);
                bubble.open();
                infoBubbleCloseFlag = false;
            };
            function goToPost(evt) {
                infoBubbleCloseFlag = true;
                evt.target.style.opacity = 0.6;
                var coords = map.screenToGeo(evt.layerX, evt.layerY);
                var minDist = 300;
                var markerDist;
                var objects = map.getObjects().filter(isPin);
                var nearestPin;
                // iterate over objects and calculate distance between them
                for (var i = 0; i < objects.length; i += 1) {
                    markerDist = objects[i].getGeometry().distance(coords);
                    if (markerDist < minDist) {
                        minDist = markerDist;
                        nearestPin = objects[i];
                    }
                }
                var htmlBubble = nearestPin.getData();
                var itemImg = $(htmlBubble).find("div.view > a");
                var itemSrc = itemImg[0].href;
                window.location.href = itemSrc;
            };
            function changeOpacityToOne(evt) {
                //while (!infoBubbleCloseFlag) { }
                evt.target.style.opacity = 1;
                var bubbles = ui.getBubbles();
                for (i = 0; i < bubbles.length; i += 1) {
                    ui.removeBubble(bubbles[i]);
                }
            };
            //create dom icon and add/remove opacity listeners
            var domIcon = new H.map.DomIcon(outerElement, {
                // the function is called every time marker enters the viewport
                onAttach: function (clonedElement, domIcon, domMarker) {
                    clonedElement.addEventListener('tap', goToPost);
                    clonedElement.addEventListener('pointerup', goToPost);
                    clonedElement.addEventListener('mouseover', changeOpacity);
                    clonedElement.addEventListener('mouseout', changeOpacityToOne);
                },
                // the function is called every time marker leaves the viewport
                onDetach: function (clonedElement, domIcon, domMarker) {
                    clonedElement.addEventListener('tap', goToPost);
                    clonedElement.addEventListener('pointerup', goToPost);
                    clonedElement.removeEventListener('mouseover', changeOpacity);
                    clonedElement.removeEventListener('mouseout', changeOpacityToOne);
                }
            });
            var bearsMarker = new H.map.DomMarker(coordinate, {
                icon: domIcon
            });
            bearsMarker.setData(html);
            map.addObject(bearsMarker);
        }

        var platform = new H.service.Platform({
            apikey: 'QgHR1cmAgWieJND3aw1CLqa_DzY3OtGjMsx4JVTvoco'
        });

        var defaultLayers = platform.createDefaultLayers();

        var map = new H.Map(document.getElementById('MapBox'),
            defaultLayers.vector.normal.map, {
            center: { lat: 37.939283, lng: 23.654527 },
            zoom: 11,
            pixelRatio: window.devicePixelRatio || 1
        });

        var service = platform.getSearchService();

        window.addEventListener('resize', () => map.getViewPort().resize());

        var behavior = new H.mapevents.Behavior(new H.mapevents.MapEvents(map));

        var ui = H.ui.UI.createDefault(map, defaultLayers);


        //HERE Maps Service Search Address 
        if (postModel.Address != "" && postModel.NumOfAddress != "" && postModel.PostalCode != "") {
            var position;
            service.geocode({
                q: `${postModel.Address} ${postModel.NumOfAddress} ${postModel.PostalCode}`
            }, (result) => {
                // Add a marker for each location found
                result.items.forEach((item) => {
                    map.setCenter(item.position);
                    map.setZoom(14);
                    position = item.position;
                    addMarkerToGroup(position, createBubbleInfo(postModel));
                });
            }, alert);

        }

        $("form#main").on("submit", function (e) {
            var dataString = $(this).serialize();
            //console.log(dataString);
            $.ajax({
                type: "POST",
                url: domainUrl + "/Posts/Details/ContactUs/" + postModel.OwnerId + "?postId=" + postModel.id,
                data: dataString,
                success: function () {
                    //toastr.success('To email στάθηκε με επιτυχία!');
                }
            });
            e.preventDefault();
        });

        $("form#notesForm").on("submit", function (e) {
            e.preventDefault();
            var dataString = $(this).serialize();
            //console.log(dataString);
            $.ajax({
                type: "POST",
                url: domainUrl + "/Posts/Details/SaveNotes/" + postModel.id,
                data: dataString
            })
                .done(function (msg) {
                    //toastr.success('Οι Σημειώσεις αποθηκεύτηυκαν με επιτυχία!');
                })
                .fail(function (jqXHR, textStatus) {
                    //alert(jqXHR.status);
                    if (jqXHR.status == 401) {
                        window.location.href = domainUrl + "/Identity/Account/Login?ReturnUrl=%2FPosts%2FDetails%2F" + postModel.id + "%23SaveNotes_" + dataString
                    }
                    else { }
                });
        });



        $("#getContactInfo").click(function (e) {
            var postId = $(this).data("postid");
            $.ajax({
                url: domainUrl + "/Posts/Details/ShowPhones/" + postId,
                method: "GET",
                dataType: 'text'
            })
                .done(function (msg) {
                    //$("#ContactInfo").empty();
                    $("#getContactInfo").attr('disabled', 'disabled');
                    $("#ContactInfo").append(createPhoneTemplate(postTile, PostObject));
                })
                .fail(function (jqXHR, textStatus) {
                    if (jqXHR.status == 401) {
                        window.location.href = domainUrl + "/Identity/Account/Login?ReturnUrl=%2FPosts%2FDetails%2F" + postId + "%23ContactInfo"
                    }
                    else { }
                });
            e.preventDefault();
        });

        $("#printClick").click(function () {
            window.print();
            //printJS({
            //    printable: 'mainContainer',
            //    type: 'html',
            //    css: [
            //        "https://use.fontawesome.com/releases/v5.11.2/css/all.css",
            //        "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap",
            //        "/css/bootstrap.min.css",
            //        "/css/mdb.css",
            //        "/css/style.css",
            //        "/lib/fine-uploader/fine-uploader-gallery.css",
            //        "/css/print.min.css",
            //        "/lib/fine-uploader/fine-uploader-new.css" ,
            //        "/css/site.css",
            //        "/css/flag-icon.css",
            //        "https://js.api.here.com/v3/3.1/mapsjs-ui.css"
            //    ],
            //    showModal: true,
            //    targetStyles: ['*']
            //})
        });

        if (window.location.hash) {
            switch (window.location.hash.split('_')[0]) {
                case "#WishList": {
                    if ($(".details-heart").hasClass("details-wish")) {
                        $(".details-heart").click();
                    }
                    break;
                }
                case "#ContactInfo": {
                    $("#ContactInfo").empty();
                    $("#ContactInfo").append(createPhoneTemplate(postTile, PostObject));
                    break;
                }
                case "#SaveNotes": {
                    $('#centralModalNotes').on('shown.bs.modal', function () {
                        $('#form107').focus();
                        $("#saveNotes").click();
                    })
                    $("#centralModalNotes").modal('show');
                    var notes = decodeURI(window.location.hash.split('_')[1].replace("notes=", ""));
                    $("#form107").html(notes);
                    //$("#form107").focus();
                    break;
                }
                default: {

                    break;
                }
            }
        }
    });

    function createPhoneTemplate(title, phone) {
        return `
<button type="button" class="btn btn-info mt-1" data-mdb-toggle="modal" data-mdb-target="#modalContact">
   <i class="fas fa-phone ms-2 fa-1x">  <strong>${phone}</strong>  </i>
</button>
<button type="button" class="btn btn-info mt-1" data-mdb-toggle="modal" data-mdb-target="#modalContact">
    <i class="fas fa-paper-plane ms-2 fa-1x"></i> <strong>Email</strong>
</button>
`;

    }

    function createBubbleInfo(product) {
        return `
<div class="card" style="width:200px;">
  <div class="bg-image hover-overlay ripple view" data-mdb-ripple-color="light">
    <img src="${product.UrlImage}" class="img-fluid" />
    <a href="/Posts/Details/${product.id}">
      <div class="mask" style="background-color: rgba(251, 251, 251, 0.15)"></div>
    </a>
  </div>
  <div class="card-body">
    <h6 class="card-title">
        <a>
            ${(product.Bathroom || "")}  <i class="fas fa-bath"></i>  |
            ${(product.Bedroom || "")}  <i class="fas fa-bed"></i>  |
            ${(product.ConstructionYear || "")}  <i class="fas fa-home"></i>
        </a>
    </h>

  </div>
</div> `

    }
})();
