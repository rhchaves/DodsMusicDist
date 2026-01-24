using DM.Loja.MVC.Models;

namespace DM.Loja.MVC.Services;

public interface ICarrinhoServico
{
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<ResultadoResposta> AdicionarItemCarrinho(ItemProdutoViewModel produto);
    Task<ResultadoResposta> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);
    Task<ResultadoResposta> RemoverItemCarrinho(Guid produtoId);
}