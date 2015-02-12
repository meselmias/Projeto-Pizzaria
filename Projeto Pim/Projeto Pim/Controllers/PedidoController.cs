using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projeto_Pim.Models;
using Projeto_Pim.Models.BLogic;

namespace Projeto_Pim.Controllers
{
    public class PedidoController : Controller
    {

        public ActionResult Lista()
        {
            return View(PedidoBL.ListaPedidos());
        }


        [HttpGet]
        public ActionResult Cadastrar()
        {
            ComboClientes();

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensagem = "Ops!Ocorreu um erro ao Cadastrar!";
                ComboClientes();
                return View();
            }
            var retorno = PedidoBL.Cadastrar(pedido);
            ComboClientes();
            return RedirectToAction("Detalhes", new { id = retorno });
        }

        [HttpGet]
        public ActionResult Detalhes(int id = 0)
        {
            var retorno = PedidoBL.ResgatarPedidoPorId(id);

            return View(retorno);
        }

        [HttpPost]
        public ActionResult Excluir(int id = 0)
        {
            var retorno = PedidoBL.ExcluiPedidoPorId(id);

            return Json(new
            {
                sucess = retorno,
                redirectUrl = Url.Action("Lista", "Pedido"),

            });
        }

        public ActionResult Alterar(int id = 0)
        {
            ComboClientes();
            var pedido = PedidoBL.ResgatarPedidoPorId(id);

            return View(pedido);
        }

        [HttpPost]
        public ActionResult Alterar(Pedido pedido)
        {
            ComboClientes();
            var retorno = PedidoBL.Atualizar(pedido);
            ViewBag.Mensagem = retorno ? "Alterado com sucesso" : "OPS! ocorreu algum erro com a alteração";
            return View(pedido);
        }

        public ActionResult _Modal()
        {
            return PartialView();
        }


        private void ComboClientes()
        {
            var listaClientes = ClienteBL.ListaClientes();
            listaClientes.Insert(0, new Cliente {Nome = "Selecione um cliente", Id = 0});
            ViewBag.ListaClientes = listaClientes;
        }

        [HttpPost]
        public ActionResult BuscarClientes(string termo)
        {
            var resultado = ClienteBL.ListaClientes().Where(p => p.Nome.Contains(termo)).ToList();
            
            if (resultado.Count > 0)
            {
                return Json(new { retorno = true, msg = "", resultado });
            }

            return Json(new { retorno = false, msg = "sem produtos disponiveis", resultado });
        }

    }
}
