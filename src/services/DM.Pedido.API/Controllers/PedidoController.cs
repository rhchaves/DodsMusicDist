using DM.Core.Mediator;
using DM.Pedidos.API.Application.Commands;
using DM.Pedidos.API.Application.Queries;
using DM.WebAPI.Core.Controllers;
using DM.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Pedidos.API.Controllers;

[Authorize]
[Route("pedido")]
public class PedidoController : MainController
{
    private readonly IMediatorHandler _mediator;
    private readonly IUsuario _user;
    private readonly IPedidoQueries _pedidoQueries;

    public PedidoController(IMediatorHandler mediator, IUsuario user, IPedidoQueries pedidoQueries)
    {
        _mediator = mediator;
        _user = user;
        _pedidoQueries = pedidoQueries;
    }

    [HttpPost("")]
    public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
    {
        pedido.ClienteId = _user.ObterUsuarioId();
        return ValidarResposta(await _mediator.EnviarComando(pedido));
    }

    [HttpGet("ultimo")]
    public async Task<IActionResult> UltimoPedido()
    {
        var pedido = await _pedidoQueries.ObterUltimoPedido(_user.ObterUsuarioId());

        return pedido == null ? NotFound() : ValidarResposta(pedido);
    }

    [HttpGet("lista-cliente")]
    public async Task<IActionResult> ListaPorCliente()
    {
        var pedidos = await _pedidoQueries.ObterListaPorClienteId(_user.ObterUsuarioId());

        return pedidos == null ? NotFound() : ValidarResposta(pedidos);
    }
}