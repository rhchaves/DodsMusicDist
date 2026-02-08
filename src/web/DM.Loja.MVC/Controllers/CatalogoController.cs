using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        private readonly ICatalogoServico _catalogoServico;

        public CatalogoController(ICatalogoServico catalogoServico)
        {
            _catalogoServico = catalogoServico;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var produtos = await _catalogoServico.ObterTodos(ps, page, q);
            ViewBag.Pesquisa = q;
            produtos.ReferenceAction = "Index";

            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produto = await _catalogoServico.ObterPorId(id);

            return View(produto);
        }
    }
}