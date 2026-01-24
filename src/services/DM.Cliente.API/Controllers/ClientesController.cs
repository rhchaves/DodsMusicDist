using DM.Clientes.API.Application.Commands;
using DM.Core.Mediator;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Clientes.API.Controllers;

public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public ClientesController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    [HttpGet("clientes")]
    public async Task<IActionResult> Index()
    {
        var resultado = await _mediatorHandler.EnviarComando(
            new RegistrarClienteCommand(Guid.NewGuid(), "Rodolfo", "contato@dodsmusic.com.br", "30314299076"));

        return ValidarResposta(resultado);
    }
}