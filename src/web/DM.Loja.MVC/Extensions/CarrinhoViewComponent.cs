using DM.Loja.MVC.Models;
using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DM.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly ICarrinhoServico _carrinhoServico;

        public CarrinhoViewComponent(ICarrinhoServico carrinhoServico)
        {
            _carrinhoServico = carrinhoServico;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _carrinhoServico.ObterCarrinho() ?? new CarrinhoViewModel());
        }
    }
}