$(document).ready(function () {
    $('#selection').on('change', function () {
        change($(this).val());
    });

});


function change(sourceUrl) {
    var audio = document.getElementById("player");
    var source = document.getElementById("mp3_src");

    $("#play").show();
    $("#pause").hide();

    $('#play').on('click', function () {
        if (sourceUrl) {
            source.src = sourceUrl;
            audio.load();
            audio.play();
            $("#play").hide();
            $("#pause").show();
        }
    });

    $('#pause').on('click', function () {
        audio.pause();
        $("#play").show();
        $("#pause").hide();
    });

}