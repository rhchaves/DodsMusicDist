using DM.Loja.MVC.Models;
using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers;

[Authorize]
public class ClienteController : MainController
{
    private readonly IClienteServico _clienteServico;

    public ClienteController(IClienteServico clienteServico)
    {
        _clienteServico = clienteServico;
    }

    [HttpPost]
    public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
    {
        var response = await _clienteServico.AdicionarEndereco(endereco);

        if (RespostaPossuiErros(response)) TempData["Erros"] =
            ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

        return RedirectToAction("EnderecoEntrega", "Pedido");
    }
}
