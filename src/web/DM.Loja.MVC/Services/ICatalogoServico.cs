using DM.Loja.MVC.Models;
using Refit;

namespace DM.Loja.MVC.Services;

public interface ICatalogoServico
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

public interface ICatalogoServicoRefit
{
    [Get("/catalogo/produtos/")]
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();

    [Get("/catalogo/produtos/{id}")]
    Task<ProdutoViewModel> ObterPorId(Guid id);
}