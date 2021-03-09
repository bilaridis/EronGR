(function () {
    var sidenav = document.getElementById("sidenav-admin");
    var sidenavInstance = mdb.Sidenav.getInstance(sidenav);
    var innerWidth = null;

    const setMode = (e) => {
        // Check necessary for Android devices
        if (window.innerWidth === innerWidth) {
            return;
        }

        innerWidth = window.innerWidth;

        if (window.innerWidth < 1400) {
            sidenavInstance.changeMode("over");
            sidenavInstance.hide();
        } else {
            sidenavInstance.changeMode("side");
            sidenavInstance.show();
        }
    };

    $(function () {


        setMode();

        // Event listeners
        window.addEventListener("resize", setMode);

        $(`td[data-mdb-field="field_0"]`).css("overflow", "visible");
    });

})();



