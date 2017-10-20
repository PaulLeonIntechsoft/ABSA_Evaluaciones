$(document).ready(function() {
    cargarRespuestas();
    resizePersonal();
    $(window).resize(resizePersonal);
    $("#mostrarModal").click(function () {
        validarCampos();
    });
    $("#cancelarIndex").click(function () {
        returnToIndex();
    });
});

function cargarRespuestas() {

    var a = "";
    var b = "";
    var c = "";
    var d = "";
    var e = "";
    var f = "";
    var g = "";

    for (var j = 1; j < 8; j++) {

        $("#cboPregunta_" + j).empty();
    }

    $.get('Respuestas_Bind', { nroPregunta: 1 }, function (dataRespuestas) {

        a += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            a += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_1").html(a);
    });

    $.get('Respuestas_Bind', { nroPregunta: 2 }, function (dataRespuestas) {

        b += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            b += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_2").html(b);
    });

    $.get('Respuestas_Bind', { nroPregunta: 3 }, function (dataRespuestas) {

        c += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            c += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_3").html(c);
    });

    $.get('Respuestas_Bind', { nroPregunta: 4 }, function (dataRespuestas) {

        d += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            d += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_4").html(d);
    });

    $.get('Respuestas_Bind', { nroPregunta: 5 }, function (dataRespuestas) {

        e += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            e += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_5").html(e);
    });

    $.get('Respuestas_Bind', { nroPregunta: 6 }, function (dataRespuestas) {

        f += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            f += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_6").html(f);
    });

    $.get('Respuestas_Bind', { nroPregunta: 7 }, function (dataRespuestas) {

        g += "<option selected value=\"\"> -- Seleccionar Respuesta -- </option>";
        $.each(dataRespuestas, function (i, v1) {
            g += "<option value=\"" + v1.Value + "\">" + v1.Text + "</option>";
        });
        $("#cboPregunta_7").html(g);
    });
}

function validarCampos() {
    var flag = true;
    for (var j = 1; j < 8; j++) {
        if ($("#cboPregunta_" + j).val() === "") {
            $("#errorModalBody").text("Ingrese respuesta en pregunta " + j);
            $("#errorModal").modal('show');
            flag = false;
            return;
        }
    }
    if (flag) {
        $("#exampleModal").modal('show');
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