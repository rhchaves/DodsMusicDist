using DM.Clientes.API.Application.Commands;
using DM.Clientes.API.Models;
using DM.WebAPI.Core.Controllers;
using DM.WebAPI.Core.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DM.Clientes.API.Controllers;

[Route("cliente/endereco")]
public class ClientesController : MainController
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMediator _mediator;
    private readonly IUsuario _user;

    public ClientesController(IClienteRepository clienteRepository, IMediator mediator, IUsuario user)
    {
        _clienteRepository = clienteRepository;
        _mediator = mediator;
        _user = user;
    }

    [HttpGet("")]
    public async Task<IActionResult> ObterEndereco()
    {
        var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUsuarioId());

        return endereco == null ? NotFound() : ValidarResposta(endereco);
    }

    [HttpPost("")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        endereco.ClienteId = _user.ObterUsuarioId();
        return ValidarResposta(await _mediator.Send(endereco));
    }
}