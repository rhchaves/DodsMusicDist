using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly IComprasBffServico _comprasBffServico;

        public CarrinhoViewComponent(IComprasBffServico comprasBffServico)
        {
            _comprasBffServico = comprasBffServico;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _comprasBffServico.ObterQuantidadeCarrinho());
        }
    }
}