$(document).ready(function(){
    centrar(".centrado");
    $(window).resize(function(){
        centrar(".centrado");
    });
});

function centrar(divCentrado){
    var largoVentana = $(window).height();
    var largoDiv = $(divCentrado).height();
    var valor = (largoVentana - largoDiv) / 2;
    $(divCentrado).css("padding-top", valor);
};