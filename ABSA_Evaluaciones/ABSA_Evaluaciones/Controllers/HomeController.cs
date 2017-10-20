using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABSA_Evaluaciones.Models;

namespace ABSA_Evaluaciones.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        ABSA_EvaluacionEntities databaseManager = new ABSA_EvaluacionEntities();
 
        public ActionResult Index()
        {
            Proyecto_Bind();
            return this.View();
        }

        [HttpPost]
        public ActionResult Index(string codProy, string codAdicional, string codPersonal)
        {
            Proyecto_Bind();
            ViewBag.ReturnSelected = codProy + "-" + codAdicional + "-" + codPersonal;
            return View();
        }

        public void Proyecto_Bind()
        {
            List<sp_listarProyectos_Result> listaBase = new List<sp_listarProyectos_Result>();
            listaBase = this.databaseManager.sp_listarProyectos().ToList();
            List<SelectListItem> listaSalida = new List<SelectListItem>();
            foreach (var item in listaBase)
            {
                listaSalida.Add(new SelectListItem { Text = item.chrCodProyecto + " - " +
                                                            item.chrCodAdicional + " - " + item.chrNomProyecto,
                                                     Value = item.chrCodProyecto.Trim() + "-" + item.chrCodAdicional.Trim()
                                                    });
            }

            ViewBag.Proyectos = listaSalida;
        }

        public JsonResult Personal_Bind(string codProyecto, string codAdicional)
        {
            List<sp_personalXProyecto_Result> listaBase = new List<sp_personalXProyecto_Result>();
            listaBase = this.databaseManager.sp_personalXProyecto(codProyecto,codAdicional).ToList();
            List<SelectListItem> listaSalida = new List<SelectListItem>();
            foreach (var item in listaBase)
            {
                if (item.chrCodAdicional.Trim().Equals("") || item.chrCodAdicional == null)
                {
                    listaSalida.Add(new SelectListItem {
                        Text =item.chrNomPer+" "+item.chrApePer,
                        Value =item.chrCodProyecto+"-"+item.bytID
                    });
                }else
                {
                    listaSalida.Add(new SelectListItem {
                        Text =item.chrNomPer+" "+item.chrApePer,
                        Value =item.chrCodProyecto+"-"+item.chrCodAdicional+"-"+item.bytID
                    });
                }
            }

            return Json(listaSalida,JsonRequestBehavior.AllowGet);
        }



    }
}