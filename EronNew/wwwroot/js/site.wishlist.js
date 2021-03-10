(function () {

    const apiUrl = "/WishList/Query";
    $(function () {
        $('#loader').css('display', 'none');
        $("#sort").change((e) => {
            e.stopImmediatePropagation();
            $(`#SortList`).val($("#sort").find(":selected").val());
            $("#products").empty();
            $("#numberOfPage").val("1");
            getProducts(apiUrl);
        });

        $(".filter-option").change((e) => {
            $("#products").empty();
            getProducts(apiUrl);
        });

        getProducts(apiUrl);
    });
    function getProducts(url) {
        var dataString = $("form").serialize();
        $.ajax({
            type: "POST",
            url: url,//,
            data: dataString,
        })
            .done(function (msg) {
                var productList = JSON.parse(msg);
                renderProducts(productList);
                $(".details-heart").unbind('click');
                $(".details-heart").click(function (e) {
                    var postId = $(this).data("postid");
                    if ($(this).hasClass("details-wish"))
                        $.ajax({
                            url: domainUrl + "/Posts/Details/WishList/" + postId,
                            method: "GET",
                            dataType: 'text'
                        })
                            .done(function (msg) {
                                $(`.details-heart[data-postid="${postId}"]`).removeClass("details-wish").addClass("details-unwish");
                                $(`.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("far").addClass("fas");
                            })
                            .fail(function (jqXHR, textStatus) {
                                //alert(jqXHR.status);
                                if (jqXHR.status == 401) {
                                    window.location.href = domainUrl + "/Identity/Account/Login?ReturnUrl=%2FHome%2FSearch%2F"
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
                                $(`.details-heart[data-postid="${postId}"]`).removeClass("details-unwish").addClass("details-wish");
                                $(`.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("fas").addClass("far");
                            })
                            .fail(function (jqXHR, textStatus) {
                                //alert("Request Administration/Delete/ failed: " + textStatus);
                            });
                    }
                    e.preventDefault();
                });
            })
            .fail(function (jqXHR, textStatus) {
                console.log("error");
                $('#loader').css('display', 'none');
                renderProducts([]);
            });
    }
    function renderProducts(products) {
        const template =
            products.length === 0 ? `` : products.map((product) => createTemplate(product)).join("\n");
        $("#products").append(template);

    }
    function createTemplate(product) {
        return `
<div class="col-sm-12 col-md-3 mb-3">
    <div class="card promoting-card m-2">
        <a href="/Posts/Details/${product.Id}">
            <div class="d-flex flex-row m-1" style="white-space: normal; overflow: visible; text-overflow: ellipsis;" >
                <small><span class="badge rounded-pill bg-primary shadow-1-strong">${$.i18n(product.Category)} </span> ${(product.Title || "")} </small>
            </div>
        </a>
        <div class="bg-image hover-overlay hover-zoom hover-shadow shadow-1">
            <img class="card-img-top rounded-0" src="${product.UrlImage || "/images/DefaultImage1.jpg"}" alt="Image Profile">
            <a href="/Posts/Details/${product.Id}">
                <div class="mask" style="background:rgba(75, 75, 75, 0.35);">
                    <span class="text-default btn-sm" style="position: absolute; top: 0px; left: 0px;">
                        #${product.Id}
                    </span>
                    <span class="text-default btn-sm" style="position: absolute; top: 0px; right: 0px;">
                        @${product.Premium}
                    </span>
                    <span class="form-check-label px-2 rounded mdb-color white-text" style="position: absolute; bottom: 10px; right: 10px;">
                        ${moment(product.CreatedDate).format('L')} <i class="fas fa-clock"></i>
                    </span>
                </div>
            </a>
        </div>
        <div class="card-body d-flex pe-0">
            <small class="p-0 pt-2">
                ${product.CurrencyConvertedLocal} - ${formatMoney(product.Square, 0, ',', '.') || ""}  <b>m<sup>2</sup></b> - ${product.ConstructionYear || ""} <i class="fas fa-home"></i>
            </small>
            <button href="#!" data-postid="${product.Id}" class="btn shadow-0 details-heart ${product.WishList == true ? "details-unwish" : "details-wish"} ms-auto justify-content-end">
                <i class="${product.WishList == true ? "fas" : "far"} fa-heart fa-1x text-danger"></i>
            </button>
        </div>
    </div>
</div>
`
            ;

    }
    var listElm = document.querySelector('#infinite-list');
    // Detect when scrolled to bottom.
    listElm.addEventListener('scroll', function () {
        if (listElm.scrollTop + listElm.clientHeight >= listElm.scrollHeight) {
            let activePage = parseInt($("#numberOfPage").val()) + 1;
            $("#numberOfPage").val(activePage)
            getProducts(apiUrl);
            console.log("Bottom - Time to refresh");
        }
    });
})();