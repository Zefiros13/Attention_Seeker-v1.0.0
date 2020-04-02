$(document).ready(function () {

    $("#reject").click(function () {
        $("#overlay").hide();
        $("#frame").hide();
    });

    function approveClick() {
        $("#currentConnection").hide();
    }

    $("#approve").click(function () {
        $("#currentConnection").hide();
    });

});

