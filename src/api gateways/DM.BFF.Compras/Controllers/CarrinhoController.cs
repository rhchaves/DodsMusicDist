using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Bff.Compras.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        return ValidarResposta();
    }

    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<IActionResult> ObterQuantidadeCarrinho()
    {
        return ValidarResposta();
    }

    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho()
    {
        return ValidarResposta();
    }

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho()
    {
        return ValidarResposta();
    }

    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho()
    {
        return ValidarResposta();
    }
}
