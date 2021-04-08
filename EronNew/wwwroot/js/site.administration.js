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
        const dropdowns = document.querySelectorAll('.dropdown');
        dropdowns.forEach((dropdown) => {
            dropdown.addEventListener('show.bs.dropdown', (e) => {
                e.target.closest('td').classList.add('my-custom-class');
            });
            dropdown.addEventListener('hidden.bs.dropdown', (e) => {
                e.target.closest('td').classList.remove('my-custom-class');
            });
        });
        setMode();

        // Event listeners
        window.addEventListener("resize", setMode);

        $(`td[data-mdb-field="field_0"]`).css("overflow", "visible");
    });

})();



