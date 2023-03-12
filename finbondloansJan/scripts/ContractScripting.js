
$(document).ready(function () {
    setInterval(function () {
        $(".clsSign1").fadeTo(1000, 0.4);
        //$(".clsSubHeaderColor").fadeToggle("slow");
        $(".clsSign1").fadeTo(1000, 1);
    }, 3000);

    setInterval(function () {
        $(".clsSign2").fadeTo(1000, 0.4);
        //$(".clsSubHeaderColor").fadeToggle("slow");
        $(".clsSign2").fadeTo(1000, 1);
    }, 3000);
});