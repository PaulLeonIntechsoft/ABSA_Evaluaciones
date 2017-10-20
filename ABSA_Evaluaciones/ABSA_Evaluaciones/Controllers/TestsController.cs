using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABSA_Evaluaciones.Models;

namespace ABSA_Evaluaciones.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {

        ABSA_EvaluacionEntities databaseManager = new ABSA_EvaluacionEntities();

        public ActionResult Lista()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Listado(String Personal)
        {
            if (Personal.Trim().Equals("") || Personal == null)
            {
                return View();
            }
            else
            {
                String[] arr = Personal.Split('-');
                List<sp_personalSeleccionado_Result> listaInicial = new List<sp_personalSeleccionado_Result>();
                try
                {
                    listaInicial = this.databaseManager.sp_personalSeleccionado(arr[0].ToString(), arr[1].ToString(), Convert.ToInt32(arr[2])).ToList();
                }
                catch (IndexOutOfRangeException)
                {
                    listaInicial = this.databaseManager.sp_personalSeleccionado(arr[0].ToString(), "", Convert.ToInt32(arr[1])).ToList();
                }

                sp_personalSeleccionado_Result seleccionado = new sp_personalSeleccionado_Result();
                seleccionado = listaInicial.First();

                return View(seleccionado);
            }
        }

        public JsonResult ListadoResultados_Bind(string tipo, string codigoPersonal)
        {
            if (tipo.Trim().Equals("vigentes"))
            {
                List<sp_listarResultadosVigentes_Result> listaBase = new List<sp_listarResultadosVigentes_Result>();
                listaBase = this.databaseManager.sp_listarResultadosVigentes(Convert.ToInt32(codigoPersonal)).ToList();

                var lst = (from n in listaBase
                           select new { n.chrCodProyecto, n.chrCodAdicional, n.chrNomCli ,dtmFecEval = n.dtmFecEval.ToShortDateString(), n.chrPuesto, n.chrNomEval, n.chrResult }).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<sp_listarResultados_Result> listaBase = new List<sp_listarResultados_Result>();
                listaBase = this.databaseManager.sp_listarResultados(Convert.ToInt32(codigoPersonal)).ToList();

                var lst = (from n in listaBase
                           select new { n.chrCodProyecto, n.chrCodAdicional, n.chrNomCli, dtmFecEval = n.dtmFecEval.ToShortDateString(), n.chrPuesto, n.chrNomEval, n.chrResult }).ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Preguntas(String Personal)
        {

            DAOPreguntas general = new DAOPreguntas();

            if (Personal.Trim().Equals("") || Personal == null)
            {
                return View();
            }
            else
            {
                String[] arr = Personal.Split('-');
                List<sp_personalSeleccionado_Result> listaInicial = new List<sp_personalSeleccionado_Result>();
                try
                {
                    listaInicial = this.databaseManager.sp_personalSeleccionado(arr[0].ToString(), arr[1].ToString(), Convert.ToInt32(arr[2])).ToList();
                }
                catch (IndexOutOfRangeException)
                {
                    listaInicial = this.databaseManager.sp_personalSeleccionado(arr[0].ToString(), "", Convert.ToInt32(arr[1])).ToList();
                }

                sp_personalSeleccionado_Result elegido = new sp_personalSeleccionado_Result();
                elegido = listaInicial.First();

                List<sp_preguntas_Result> listaBase = new List<sp_preguntas_Result>();
                listaBase = this.databaseManager.sp_preguntas().ToList();
                List<SelectListItem> listaFinal = new List<SelectListItem>();
                foreach (var item in listaBase)
                {
                    listaFinal.Add(new SelectListItem
                    {
                        Text = item.chrDesPre,
                        Value = item.chrNroPre.ToString()
                    });
                }

                general.seleccionado = elegido;
                general.preguntas = listaFinal;

                return View(general);
            }
        }

        [HttpPost]
        public JsonResult GuardarPreguntas(string codigoPersonal, string codRespuestaUno, string codRespuestaDos, string codRespuestaTres,
            string codRespuestaCuatro, string codRespuestaCinco, string codRespuestaSeis, string codRespuestaSiete, string proyectoGeneral)
        {
            try
            {
                var usuario = User.Identity.Name;

                string[] datosArreglo = datos(codRespuestaUno, codRespuestaDos, codRespuestaTres, codRespuestaCuatro, codRespuestaCinco, codRespuestaSeis, codRespuestaSiete);

                string[] sepProy = proyectoGeneral.Split('-');

                int rows = databaseManager.sp_insertarEvaluacion(Convert.ToInt32(codigoPersonal), datosArreglo[0], datosArreglo[1], datosArreglo[2], datosArreglo[3],
                    datosArreglo[4], datosArreglo[5], Convert.ToInt16(datosArreglo[6]), usuario, datosArreglo[7], Convert.ToDecimal(datosArreglo[8]),
                    sepProy[0].Trim(),sepProy[1].Trim(),"");
                return Json(new { success = true, message = "Evaluacion registrada correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error en el registro." }, JsonRequestBehavior.AllowGet);
            }
        }

        private string[] datos(string codRespuesta_1, string codRespuesta_2, string codRespuesta_3, string codRespuesta_4, string codRespuesta_5, string codRespuesta_6, string codRespuesta_7)
        {
            string valorResultado = "";
            double notaResultado = 0.0;

            // NOTA 1
            string[] despRpt1 = codRespuesta_1.Split('-');
            List < sp_nota_Result > lst_nota1 = databaseManager.sp_nota(Convert.ToInt16(despRpt1[0]), despRpt1[1]).ToList() ;
            double nota1 = 0.0;
            foreach (var item in lst_nota1)
            {
                nota1 = (((Double)item.intValRes)/10.0);
            }

            // NOTA 2
            string[] despRpt2 = codRespuesta_2.Split('-');
            List<sp_nota_Result> lst_nota2 = databaseManager.sp_nota(Convert.ToInt16(despRpt2[0]), despRpt2[1]).ToList();
            double nota2 = 0.0;
            foreach (var item in lst_nota2)
            {
                nota2 = (((Double)item.intValRes) / 10.0);
            }

            // NOTA 3
            string[] despRpt3 = codRespuesta_3.Split('-');
            List<sp_nota_Result> lst_nota3 = databaseManager.sp_nota(Convert.ToInt16(despRpt3[0]), despRpt3[1]).ToList();
            double nota3 = 0.0;
            foreach (var item in lst_nota3)
            {
                nota3 = (((Double)item.intValRes) / 10.0);
            }

            // NOTA 4
            string[] despRpt4 = codRespuesta_4.Split('-');
            List<sp_nota_Result> lst_nota4 = databaseManager.sp_nota(Convert.ToInt16(despRpt4[0]), despRpt4[1]).ToList();
            double nota4 = 0.0;
            foreach (var item in lst_nota4)
            {
                nota4 = (((Double)item.intValRes) / 10.0);
            }

            // NOTA 5
            string[] despRpt5 = codRespuesta_5.Split('-');
            List<sp_nota_Result> lst_nota5 = databaseManager.sp_nota(Convert.ToInt16(despRpt5[0]), despRpt5[1]).ToList();
            double nota5 = 0.0;
            foreach (var item in lst_nota5)
            {
                nota5 = (((Double)item.intValRes) / 10.0);
            }

            // NOTA 6
            string[] despRpt6 = codRespuesta_6.Split('-');
            List<sp_nota_Result> lst_nota6 = databaseManager.sp_nota(Convert.ToInt16(despRpt6[0]), despRpt6[1]).ToList();
            double nota6 = 0.0;
            foreach (var item in lst_nota6)
            {
                nota6 = (((Double)item.intValRes) / 10.0);
            }

            // NOTA 7
            string[] despRpt7 = codRespuesta_7.Split('-');
            int valorElegido = Convert.ToInt32(despRpt7[1]) + 1;
            List<sp_nota_Result> lst_nota7 = databaseManager.sp_nota(Convert.ToInt16(despRpt7[0]), Convert.ToString(valorElegido)).ToList();
            double nota7 = 0.0;
            foreach (var item in lst_nota7)
            {
                nota7 = (((Double)item.intValRes) / 10.0);
            }

            notaResultado = nota1 + nota2 + nota3 + nota4 + nota5 + nota6 + nota7;

            if (notaResultado <= 3.0)
            {
                valorResultado = "NR";
            }
            else if (notaResultado <= 5.0)
            {
                valorResultado = "PB";
            }
            else if (notaResultado <= 7.0)
            {
                valorResultado = "PA";
            }
            else if (notaResultado <= 9.0)
            {
                valorResultado = "R";
            }
            else
            {
                valorResultado = "AB";
            }

            string[] arreglo = new string[9];
            arreglo[0] = despRpt1[1];
            arreglo[1] = despRpt2[1];
            arreglo[2] = despRpt3[1];
            arreglo[3] = despRpt4[1];
            arreglo[4] = despRpt5[1];
            arreglo[5] = despRpt6[1];
            arreglo[6] = valorElegido.ToString();
            arreglo[7] = valorResultado;
            arreglo[8] = notaResultado.ToString();
            return arreglo;
        }

        public JsonResult Preguntas_Bind()
        {

            List<sp_preguntas_Result> listaBase = new List<sp_preguntas_Result>();
            listaBase = this.databaseManager.sp_preguntas().ToList();
            List<SelectListItem> listaFinal = new List<SelectListItem>();
            foreach (var item in listaBase)
            {
                listaFinal.Add(new SelectListItem { Text = item.chrDesPre,
                                                    Value = item.chrNroPre.ToString()
                                                    });
            }
            return Json(listaFinal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Respuestas_Bind(String nroPregunta)
        {
            List<sp_respuestas_Result> listaBase = new List<sp_respuestas_Result>();
            listaBase = this.databaseManager.sp_respuestas(Convert.ToInt16(nroPregunta)).ToList();
            List<SelectListItem> listaFinal = new List<SelectListItem>();
            foreach (var item in listaBase)
            {
                listaFinal.Add(new SelectListItem{
                                                    Text = item.chrDesRes,
                                                    Value = item.chrNroPre.ToString() + "-" + item.chrCodRes
                                                 });
            }
            return Json(listaFinal, JsonRequestBehavior.AllowGet);
        }

    }
}