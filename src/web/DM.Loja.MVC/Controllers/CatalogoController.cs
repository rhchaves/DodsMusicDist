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
        public async Task<IActionResult> Index()
        {
            var produtos = await _catalogoServico.ObterTodos();

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