using DM.Clientes.API.Application.Commands;
using DM.Clientes.API.Models;
using DM.Core.Mediator;
using DM.Loja.MVC.Extensions;
using DM.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Clientes.API.Controllers;

public class ClientesController : MainController
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMediatorHandler _mediator;
    private readonly IUsuario _user;

    public ClientesController(IClienteRepository clienteRepository, IMediatorHandler mediatorHandler, IUsuario user)
    {
        _clienteRepository = clienteRepository;
        _mediator = mediatorHandler;
        _user = user;
    }

    [HttpGet("cliente/endereco")]
    public async Task<IActionResult> ObterEndereco()
    {
        var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUsuarioId());

        return endereco == null ? NotFound() : ValidarResposta(endereco);
    }

    [HttpPost("cliente/endereco")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        endereco.ClienteId = _user.ObterUsuarioId();
        return ValidarResposta(await _mediator.EnviarComando(endereco));
    }
}