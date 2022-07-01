$(document).ready(function () {
    $('body').css('cursor', 'default');

    $('#file-upload').change(function () {
        //debugger
        var i = $(this).prev('label').clone();
        var file = $('#file-upload')[0].files[0].name;
        $(this).prev('label').text(file);
    });
    $("#btnSalvar").on("click", function (event) {
        var fileName = $("#file-upload").val();

        if (!fileName) {
            $("#file-upload").notify(
                "Nenhum arquivo selecionado",
                { position: "right" },
                "BOOM!"
            );
            return false;
        }
        $('body').css('cursor', 'wait');
        $("#btnSalvar").attr("disabled", true);
        var result = $("#formROA").submit();
    });
    $("btnVoltar").on("click", function (event) {
        history.back();
    });
});