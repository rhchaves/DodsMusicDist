using DM.Carrinho.API.Data;
using DM.Carrinho.API.Model;
using DM.WebAPI.Core.Controllers;
using DM.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DM.Carrinho.API.Controllers;

[Authorize]
[Route("carrinho")]
public class CarrinhoController : MainController
{
    private readonly ICollection<string> _erros = new List<string>();
    private readonly IUsuario _user;
    private readonly CarrinhoContext _context;

    public CarrinhoController(IUsuario user, CarrinhoContext context)
    {
        _user = user;
        _context = context;
    }

    [HttpGet("")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await ObterCarrinhoCliente() ?? new CarrinhoCliente(_user.ObterUsuarioId());
    }

    [HttpPost("")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho == null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);

        if (_erros.Any()) return ValidarResposta();

        await PersistirDados();
        return ValidarResposta();
    }

    [HttpPut("{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();
        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
        if (itemCarrinho == null) return ValidarResposta();

        carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

        ValidarCarrinho(carrinho);
        if (_erros.Any()) return ValidarResposta();

        _context.CarrinhoItens.Update(itemCarrinho);
        _context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();
        return ValidarResposta();
    }

    [HttpDelete("{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var carrinho = await ObterCarrinhoCliente();

        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
        if (itemCarrinho == null) return ValidarResposta();

        ValidarCarrinho(carrinho);
        if (_erros.Any()) return ValidarResposta();

        carrinho.RemoverItem(itemCarrinho);

        _context.CarrinhoItens.Remove(itemCarrinho);
        _context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();
        return ValidarResposta();
    }

    [HttpPost]
    [Route("aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher(Voucher voucher)
    {
        var carrinho = await ObterCarrinhoCliente();

        carrinho.AplicarVoucher(voucher);

        _context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();
        return ValidarResposta();
    }

    #region Métodos privados
    private async Task<CarrinhoCliente> ObterCarrinhoCliente()
    {
        return await _context.CarrinhoCliente.Include(c => c.Itens).FirstOrDefaultAsync(c => c.ClienteId == _user.ObterUsuarioId());
    }

    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        var carrinho = new CarrinhoCliente(_user.ObterUsuarioId());
        carrinho.AdicionarItem(item);

        ValidarCarrinho(carrinho);
        _context.CarrinhoCliente.Add(carrinho);
    }

    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        var produtoItemExistente = carrinho.CarrinhoItemExistente(item);

        carrinho.AdicionarItem(item);
        ValidarCarrinho(carrinho);

        if (produtoItemExistente)
            _context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
        else
            _context.CarrinhoItens.Add(item);

        _context.CarrinhoCliente.Update(carrinho);
    }

    private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
    {
        if (item != null && produtoId != item.ProdutoId)
        {
            AdicionarErroProcessamento("O item não corresponde ao informado");
            return null;
        }

        if (carrinho == null)
        {
            AdicionarErroProcessamento("Carrinho não encontrado");
            return null;
        }

        var itemCarrinho = await _context.CarrinhoItens
            .FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

        if (itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
        {
            AdicionarErroProcessamento("O item não está no carrinho");
            return null;
        }

        return itemCarrinho;
    }

    private async Task PersistirDados()
    {
        var result = await _context.SaveChangesAsync();
        if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
    }

    private bool ValidarCarrinho(CarrinhoCliente carrinho)
    {
        if (carrinho.EhValido()) return true;

        carrinho.ValidationResult.Errors.ToList().ForEach(e => AdicionarErroProcessamento(e.ErrorMessage));
        return false;
    }
    #endregion
}
