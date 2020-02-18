using System;
using System.Diagnostics;
using System.Linq;
using Agenda.Business;
using Agenda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Controllers
{
    public class AtividadesController : Controller
    {
        AtividadeBusiness _atividadesBusiness = new AtividadeBusiness();

        public IActionResult Index()
        {
            var atividade = _atividadesBusiness.Obter();
            return View(atividade);
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            Atividades atividade = new Atividades
            {
                Nome = collection["Nome"].ToString(),
                Descricao = collection["Descricao"].ToString(),
                DataInicio = string.IsNullOrEmpty(collection["DataInicio"]) ? (DateTime?)null : Convert.ToDateTime(collection["DataInicio"]),
                DataFim = string.IsNullOrEmpty(collection["DataFim"]) ? (DateTime?)null : Convert.ToDateTime(collection["DataFim"]),
            };
            _atividadesBusiness.Salvar(atividade);
            return View();
        }

        public ActionResult Edit(int id)
        {
            // TODO : Validadr se a atividade não tem a data de inicio menor que a de fim
            var atividade = _atividadesBusiness.Obter(id);
            _atividadesBusiness.Alterar(atividade);
            return View(atividade);
        }

        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO : Validadr se a atividade não tem a data de inicio menor que a de fim
                Atividades atividade = new Atividades
                {
                    Id = id,
                    Nome = collection["Nome"].ToString(),
                    Descricao = collection["Descricao"].ToString(),
                    DataInicio = string.IsNullOrEmpty(collection["DataInicio"]) ? (DateTime?)null : Convert.ToDateTime(collection["DataInicio"]),
                    DataFim = string.IsNullOrEmpty(collection["DataFim"]) ? (DateTime?)null : Convert.ToDateTime(collection["DataFim"]),
                };

                _atividadesBusiness.Alterar(atividade);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var atividade = _atividadesBusiness.Obter(id);
            return View(atividade);
        }

        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _atividadesBusiness.Excluir(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Arquivo/Details/5
        public ActionResult Details(int id)
        {
            var atividade = _atividadesBusiness.Obter(id);
            return View(atividade);
        }
    }
}
