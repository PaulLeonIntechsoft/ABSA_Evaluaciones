$(document).ready(function () {
    tablaResponsive();
    resizePersonal();
    $(window).resize(tablaResponsive);
    $(window).resize(resizePersonal);
    cargarTabla("vigentes");
    $("input:radio[name='listado']").change(function () {
        if ($(this).val() === 'listarVigentes') {
            cargarTabla("vigentes");
        } else {
            cargarTabla("todos");
        }
    });
    $("#regresarIndex").click(function () {
        returnToIndex();
    });
});

function cargarTabla(proyecto) {

    var id = $("#evaluado div div h3").attr("id");
    var v = "";

    $("#tablaProyectos").empty();

    $.get("ListadoResultados_Bind", { tipo : proyecto, codigoPersonal : id }, function (data) {
        v = "<table class=\"table\">" +
                "<thead>" +
                    "<tr>" +
                        "<th>Proyecto</th>" +
                        "<th class=\"text-center\">Cliente</th>" +
                        "<th class=\"text-center\">Fecha</th>" +
                        "<th class=\"text-center\">Cargo</th>" +
                        "<th class=\"text-center\">Evaluador</th>" +
                        "<th class=\"text-center\">Calificacion</th>" +
                    "</tr>" +
                "</thead>" +
                "<tbody>";

        $.each(data, function (i, v1) {
            v += "<tr>";
            v += "<th scope=\"row\">" + v1.chrCodProyecto + " - " + v1.chrCodAdicional + "</th>";
            v += "<td>" + v1.chrNomCli + "</td>";
            v += "<td>" + v1.dtmFecEval + "</td>";
            v += "<td>" + v1.chrPuesto + "</td>";
            v += "<td>" + v1.chrNomEval + "</td>";
            v += "<td>" + v1.chrResult + "</td>";
            v += "</tr>";
        });

        v += "</tbody></table>";
        $("#tablaProyectos").html(v);

    });

}

function tablaResponsive(){
    var anchoVentana = $(window).width();
    if(anchoVentana < 650){
        $("#tablaProyectos").addClass("table-responsive");
    }else{
        $("#tablaProyectos").removeClass("table-responsive");
    }
}

function resizePersonal() {
    var anchoVentana = $(window).width();
    if (anchoVentana < 546) {
        $("#imagenEvaluado").removeClass("col-xs-3");
        $("#imagenEvaluado").addClass("col-xs-12");
        $("#evaluado").removeClass("col-xs-9");
        $("#evaluado").addClass("col-xs-12");
    } else {
        $("#imagenEvaluado").addClass("col-xs-3");
        $("#imagenEvaluado").removeClass("col-xs-12");
        $("#evaluado").addClass("col-xs-9");
        $("#evaluado").removeClass("col-xs-12");
    }
}

function returnToIndex() {
    var arrProy = $("#proyectoElegido").text().trim().split('-');
    var idProyecto = arrProy[0].trim();
    var idAdicional = arrProy[1].trim();
    var idPersonal = $("#evaluado div div h3").attr('id');
    window.location.href = "/Home/Index?cpo=" + idProyecto + "&ca=" + idAdicional + "&cpe=" + idPersonal;
}