$(document).ready(function() {
    $("#grabarEvaluacion").click(function () {
        getDatos();
    });
    $("#confirmado").click(function() {
        returnToIndex();
    });

});

function getDatos() {
    var datos = {
        codigoPersonal : $("#evaluado div div h3").attr('id'),
         codRespuestaUno : $("#cboPregunta_1").val(),
         codRespuestaDos : $("#cboPregunta_2").val(),
         codRespuestaTres : $("#cboPregunta_3").val(),
         codRespuestaCuatro : $("#cboPregunta_4").val(),
         codRespuestaCinco : $("#cboPregunta_5").val(),
         codRespuestaSeis : $("#cboPregunta_6").val(),
         codRespuestaSiete : $("#cboPregunta_7").val(),
         proyectoGeneral : $("#proyectoElegido").text()
    }

    $.ajax({
        url: "/Tests/GuardarPreguntas",
        data: datos,
        type: 'POST',
        success: function (data) {
            $("#exampleModal").modal('hide');
            $("#modalAceptar").modal('show');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#falloConexionModal").modal('show');
        }
    });

}

function returnToIndex() {
    var arrProy = $("#proyectoElegido").text().trim().split('-');
    var idProyecto = arrProy[0].trim();
    var idAdicional = arrProy[1].trim();
    var idPersonal = $("#evaluado div div h3").attr('id');
    window.location.href = "/Home/Index?cpo=" + idProyecto + "&ca=" + idAdicional + "&cpe=" + idPersonal;
}