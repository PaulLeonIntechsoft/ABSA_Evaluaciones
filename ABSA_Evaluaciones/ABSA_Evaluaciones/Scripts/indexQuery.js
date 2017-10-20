$(document).ready(function () {
    $("#Proyecto").change(function () {
        listarPersonal(this);
        $("#proyectoVacio").empty();
    });

    if (recargarProyecto()) {
    }

    $("#Personal").change(function () {
        $("#personalVacio").empty();
    });

    $("#btnListar").click(function (event) {
        if ($("#Proyecto").val() === "") {
            event.preventDefault();
            $(".modal-body").text("Seleccione proyecto para continuar.");
            $("#errorModal").modal('show');
            return;
        } else {
            if ($("#Personal").val() === "") {
                event.preventDefault();
                $(".modal-body").text("Seleccione personal para continuar.");
                $("#errorModal").modal('show');
                return;
            } else {
                $("form").attr("action", "/Tests/Listado");
            }
        }
    });

    $("#btnEvaluar").click(function (event) {
        if ($("#Proyecto").val() === "") {
            event.preventDefault();
            $(".modal-body").text("Seleccione proyecto para continuar.");
            $("#errorModal").modal('show');
            return;
        } else {
            if ($("#Personal").val() === "") {
                event.preventDefault();
                $(".modal-body").text("Seleccione personal para continuar.");
                $("#errorModal").modal('show');
                return;
            } else {
                $("form").attr("action", "/Tests/Preguntas");
            }
        }
    });
});

function listarPersonal(campo) {
    var id = $(campo).val();
    var arr = id.trim().split('-');
    var elegido = '';
    elegido = recargarPersonal();

    $("#Personal").empty();
    $.get("Personal_Bind", { codProyecto: arr[0], codAdicional: arr[1] }, function (data) {
        var v = "<option value=\"\"> --- Seleccione Personal --- </option>";
        var selected = '';

        $.each(data, function (i, v1) {

            if (elegido == v1.Value) {
                selected = 'selected';
            } else {
                selected = '';
            }

            v += "<option value=\"" + v1.Value + " \" " + selected + " >" + v1.Text + "</option>";
        });
        $("#Personal").html(v);
    });
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function recargarProyecto() {
    try{
        var cpo = getParameterByName('cpo');
        var ca = getParameterByName('ca');
        var cpe = getParameterByName('cpe');

        if (cpo != null || cpo.trim() !== "") {
            $("#Proyecto").val(cpo.trim() + "-" + ca.trim());
            listarPersonal("#Proyecto");
            return true;
        } else {
            return false;
        }
    } catch(err){
        return false;
    }
}

function recargarPersonal() {
    var cpo = '';
    var ca = '';
    var cpe = '';
    var elegido = '';
    try {
        cpo = getParameterByName('cpo');
        ca = getParameterByName('ca');
        cpe = getParameterByName('cpe');

        if(ca == null || ca == ""){
            return cpo + "-" + cpe;
        } else {
            return cpo + "-" + ca + "-" + cpe;
        }

    } catch (e) {
        return "";
    }
}