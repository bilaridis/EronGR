(function () {
    var previousHistory = "";
    var init = false;
    var listElm = document.querySelector('#infinite-list');
    const apiUrl = "/Home/Query";
    const apiSaveUrl = "/Home/SaveQuery";
    $(function () {
        if ($("#getId").val() != undefined) {
            $saveButton = $('<a/>').attr({ type: 'button', id: 'btnModalSearch', class: 'btn-sm btn-primary white-text z-depth-1' });
            $saveButton.html('Αποθήκευση Αναζήτησης');
            $saveButton.attr("data-toggle", "modal");
            $saveButton.attr("data-target", "#modalLoginAvatar");
            $(".custom-buttons").append($saveButton);
            $("#btnSaveQuery").on('click', function () {
                var dataString = $("form").serialize();
                var title = $("#searchTitle").val() || "Search Title";
                $.ajax({
                    type: "POST",
                    url: `${apiSaveUrl}/${title}`,
                    data: dataString,
                })
                    .done(function (msg) {
                        $('#modalLoginAvatar').modal('hide');
                        toastr.success('Η Αναζήτηση αποθηκεύτηκε.');
                    })
                    .fail(function (jqXHR, textStatus) {
                        $('#modalLoginAvatar').modal('hide');
                        toastr.error('Κάτι πήγε στραβά. Θα το κοιτάξουμε αμέσως.');

                    });
            });
        }
        $(window).on('popstate', function () {
            init = true;
            initFilters();
            $("#numberOfPage").val("1");
            getProducts(apiUrl);
            init = false;
        });
        let parsedSearchData = JSON.parse(searchData);
        $('#inp_Category').find('option').each(function () {
            $(this).removeAttr('selected');
        });
        $('#inp_Category option[value="' + parsedSearchData.SaleCategory + '"]').attr('selected', 'selected').change();
        $('#selectType').find('option').each(function () {
            $(this).removeAttr('selected');
        });
        $('#selectType option[value="' + parsedSearchData.TypeDesc + '"]').attr('selected', 'selected').change();
        //$(".mdb-select").materialSelect();
        FillSubTypes();
        FillSubAreas();
        init = true;
        initFilters();
        $("#numberOfPage").val("1");
        getProducts(apiUrl);
        init = false;
    });
    function FillAreas(option = "") {
        $.ajax({
            url: `${domainUrl}/api/GetAreas/0`,
            method: "GET",
            dataType: 'text',
            async: true,
        })
            .done(function (msg) {
                var areas = JSON.parse(msg);
                var parseLoc = JSON.parse(textLocalised);
                //$("#selectParentArea").materialSelect('destroy');
                $("#selectParentArea").empty();
                $("#selectParentArea").append(`<option value="" disabled></option>`);
                $.each(areas, function (index, value) {
                    var checked = contains(option, value.Value) ? 'selected' : '' || '';
                    $("#selectParentArea").append(`<option value="${value.Value}" ${checked}>${value.Text}</option>`);
                });
                //$("#selectParentArea").materialSelect();
            })
            .fail(function (jqXHR, textStatus) {
                //alert("Request api/GetArea/ failed: " + textStatus);
            });
    }
    function fillWithClone(id, option = "") {
        //$(`#${id}`).materialSelect('destroy');
        var options = $(`#${id}`).find('option').clone();
        $(`#${id}`).empty();
        for (let i = 0; i < options.length; i++) {
            var checked = options[i].value == option ? 'selected' : '' || '';
            $(`#${id}`).append(`<option value="${options[i].value}" ${checked}>${options[i].innerText}</option>`);
        }
        //$(`#${id}`).materialSelect();
    }
    function FillSubTypes(option = []) {
        $.ajax({
            url: `${domainUrl}/api/Types/${$("#selectType").find(":selected").val()}`,
            method: "GET",
            dataType: 'text'
        })
            .done(function (t) {
                var typesForAppend = JSON.parse(t);
                var parseLoc = JSON.parse(textLocalised);
                //$("#selectSubType").materialSelect("destroy");
                $("#selectSubType").empty();
                $("#selectSubType").append(`<option value="" disabled>${parseLoc.subType}</option>`);
                $.each(typesForAppend, function (index, value) {
                    var checked = contains(option, value.Value) ? 'selected' : '' || '';
                    $("#selectSubType").append(`<option value="${value.Value}" ${checked}>${value.Text}</option>`);
                });
                //$("#selectType").change();
                //$("#selectSubType").materialSelect();
            })
            .fail(function (jqXHR, textStatus) {
                //alert("Request api/Types/ failed: " + textStatus);
            });
    }
    function FillSubAreas(option = []) {
        $.ajax({
            url: `${domainUrl}/api/GetAreas/${$("#selectParentArea").find(":selected").val()}`,
            method: "GET",
            dataType: 'text',
            async: true,
        })
            .done(function (t) {
                var subAreas = JSON.parse(t);
                var parseLoc = JSON.parse(textLocalised);
                //$("#selectArea").materialSelect('destroy');
                $("#selectArea").empty();
                $("#selectArea").append(`<option value="" disabled>${parseLoc.subArea}</option>`);
                $.each(subAreas, function (index, value) {
                    var checked = contains(option, value.Value) ? 'selected' : '' || '';
                    $("#selectArea").append(`<option value="${value.Value}" ${checked}>${value.Text}</option>`);
                });
                //$("#selectArea").materialSelect();
            })
            .fail(function (jqXHR, textStatus) {
                //alert("Request api/GetArea/ failed: " + textStatus);
            });
    }

    $(function () {
        $('#loader').css('display', 'none');

        $("#sort").change((e) => {
            $(`#SortList`).val($("#sort").find(":selected").val());
            $("#numberOfPage").val("1");
            $("#products").empty();
            getProducts(apiUrl);
        });
        
        $(".btn-minus").on('click', function (e) {
            e.preventDefault();
            this.parentNode.querySelector('input[type=number]').stepDown();
            $("#numberOfPage").val("1");
            $("#products").empty();
            if (e.target.id == "selectType") {
                FillSubTypes();
            }
            if (e.target.id == "selectParentArea") {
                FillSubAreas();
            }
            if (!init) pushHistoryState();
            getProducts(apiUrl);
            return false;
        });

        $(".btn-plus").on('click', function (e) {
            e.preventDefault();
            this.parentNode.querySelector('input[type=number]').stepUp();
            $("#numberOfPage").val("1");
            $("#products").empty();
            if (e.target.id == "selectType") {
                FillSubTypes();
            }
            if (e.target.id == "selectParentArea") {
                FillSubAreas();
            }
            if (!init) pushHistoryState();
            getProducts(apiUrl);
            return false;
        });

        $(".filter-option").change((e) => {
            e.stopImmediatePropagation();
            $("#numberOfPage").val("1");
            $("#products").empty();
            if (e.target.id == "selectType") {
                FillSubTypes();
            }
            if (e.target.id == "selectParentArea") {
                FillSubAreas();
            }
            if (!init) pushHistoryState();
            getProducts(apiUrl);

        });
    });
    function pushHistoryState() {
        var formData = decodeURIComponent($("form#searchDataForm").serialize());
        var newurl = `${domainUrl}/Home/Search/?${formData}`;
        if (previousHistory != newurl) {
            previousHistory = newurl;
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }
    function initFilters() {
        const filters = Array.from($("[filter]")).map(
            (el) => el.attributes.filter.value
        );
        const dict = {};

        filters.forEach((filter) => {
            dict[filter] = decodeURIComponent(gup(filter));
            let val = decodeURIComponent(gup(filter));
            let arrayVal = getUrlParameter(filter);
            //.find(`option[value="${val}"]`)
            $(`select[${filter}]`).each(function (el) {
                if ($(this).parent().attr("id") == "selectType") {
                    fillWithClone($(this).parent().attr("id"), val);
                }
                else if ($(this).parent().attr("id") == "selectParentArea") {
                    FillAreas(val);
                }
                else if ($(this).parent().attr("id") == "inp_Category") {
                    fillWithClone($(this).parent().attr("id"), val);
                }
                else if ($(this).parent().attr("id") == "EnergyEfficiency") {
                    fillWithClone($(this).parent().attr("id"), val);
                }
                else if ($(this).attr("id") == "selectSubType") {
                    FillSubTypes(arrayVal);
                }
                else if ($(this).attr("id") == "selectArea") {
                    FillSubAreas(arrayVal);
                }
                else {
                    var ttt = arrayVal;
                }
            });
            $(`input[type="checkbox"][${filter}]`).each(function (el) {
                if (val == "on") {
                    $(this).prop('checked', true).change();
                }
                else {
                    $(this).prop('checked', false).change();
                }
            });
            $(`input[type="number"][${filter}]`).val(val);
        });
    }
    function getProducts(url) {
        var dataString = $("form").serialize();
        $.ajax({
            type: "POST",
            url: url,
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
                                $(`a.details-heart[data-postid="${postId}"]`).removeClass("details-wish").addClass("details-unwish");
                                $(`a.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("far").addClass("fas");
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
                                $(`a.details-heart[data-postid="${postId}"]`).removeClass("details-unwish").addClass("details-wish");
                                $(`a.details-heart[data-postid="${postId}"]`).find(".fa-heart").removeClass("fas").addClass("far");
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
    // Detect when scrolled to bottom.
    listElm.addEventListener('scroll', function () {
        if (listElm.scrollTop + listElm.clientHeight >= listElm.scrollHeight) {
            let activePage = parseInt($("#numberOfPage").val()) + 1;
            $("#numberOfPage").val(activePage);
            getProducts(apiUrl);
            console.log("Bottom - Time to refresh");
        }
    });
    function renderProducts(products) {
        if (products.length > 0) {
            $(`#countOfAds`).html(products[0].CountOfPost);
        } else {
            //$(`#countOfAds`).html(`0`);
        }
        const template = products.length === 0 ? `` : products.map((product) => createTemplate(product)).join("\n");
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
})();