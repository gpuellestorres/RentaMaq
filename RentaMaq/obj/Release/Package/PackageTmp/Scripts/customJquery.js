$(document).ready(function () {

    $("#numeroDeParte").change(function () {
        var codigo = $("#numeroDeParte").val();
        $.ajax({
            url: "/Producto/verificarExistenciaCodigo",
            data: { "numeroDeParte": codigo },

            success: function (retorno) {

                if (retorno == 'false') {

                    $("#mensajeErrorCodigo").addClass("hidden");
                    $("#BtnCrear").removeAttr("disabled");
                } else {

                    $("#mensajeErrorCodigo").removeClass("hidden");
                    $("#BtnCrear").attr("disabled", "disabled");
                }
            }
        });


    });


});
