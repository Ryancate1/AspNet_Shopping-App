
function display() {
    document.getElementById("navbar").style.display = "block";
}

function closeDisplay() {
    document.getElementById("navbar").style.display = "none";
} 


$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function dropdown() {
    document.getElementById("myDropdown").classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {

        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

$(document).ready(function() {
    $("#myModal4").modal('show');
})