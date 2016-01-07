$(document).ready(function () {
    var cantidadSalienteMaxima = 0;
    try {
        $(".ProductoID").select2({
            placeholder: "Seleccione un producto"
        });
    } catch (error) {

    }
    $(".fecha").datetimepicker({
        viewMode: 'days',
        format: 'DD/MM/YYYY'
    });
    $('.Int').bind('keypress', function (event) {
        var regex = new RegExp("^([0-9])$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    $("#ProductoID").change(function () {
        var codigo = $("#ProductoID").val();
        $.ajax({
            url: "/Maestro/obtenerNombreProducto",
            data: { "numeroDeParte": codigo },
            success: function (retorno) {
                cantidadSalienteMaxima = retorno.stockActual;
                $("#descripcionProducto").val(retorno.descripcion);
                $("#valorUnitario").val(retorno.precioUnitario);
                $("#cantidadSaliente").attr("placeholder", retorno.stockActual + " en Stock ...");
            }
        });
    });

    $("#valorUnitario").change(function () {
        obtenerTotal();
    });
    $("#cantidadEntrante").focusout(function () {
        obtenerTotal();
    });
    $("#cantidadSaliente").focusout(function () {
        var cantidadASalir = $("#cantidadSaliente").val();
        if (cantidadASalir > cantidadSalienteMaxima) {
            alert("Solo hay " + cantidadSalienteMaxima + " productos en stock. Ingrese una cantidad igual o menor.")
            $("#cantidadSaliente").focus();
        }
        obtenerTotal();
    });
});

function obtenerTotal() {
    var valorUnitario = $("#valorUnitario").val();
    var cantidadEntrante = $("#cantidadEntrante").val();
    var cantidadSaliente = $("#cantidadSaliente").val();

    if ((valorUnitario != '') && (cantidadEntrante != undefined)) {
        var valorTotal = parseInt(valorUnitario) * parseInt(cantidadEntrante);
        $("#valorTotal").val(valorTotal);
    } else if ((valorUnitario != '') && (cantidadSaliente != undefined)) {
        var valorTotal = parseInt(valorUnitario) * parseInt(cantidadSaliente);
        $("#valorTotal").val(valorTotal);
    }
}


function formato_precio(n) {
    var number = new String(n);
    var result = "";
    isNegative = false;
    if (number.indexOf("-") > -1) {
        number = number.substr(1);
        isNegative = true;
    }

    while (number.length > 3) {
        result = '.' + number.substr(number.length - 3) + result;

        number = number.substring(0, number.length - 3);

    }
    result = number + result;
    if (isNegative) result = '-' + result;

    return '$' + result;
}
