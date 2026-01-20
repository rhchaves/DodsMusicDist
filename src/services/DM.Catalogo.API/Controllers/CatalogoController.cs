using DM.Catalogo.API.Models;
using DM.WebAPI.Core.Controllers;
using DM.WebAPI.Core.Identidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Catalogo.API.Controllers;

[ApiController]
[Authorize]
public class CatalogoController : MainController
{
    private readonly IProdutoRepository _produtoRepository;

    public CatalogoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [AllowAnonymous]
    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index()
    {
        return await _produtoRepository.ObterTodos();
    }

    //Foi aplicado ClaimsAuthorize apenas em carater de teste, será removido para deixar o acesso aberto após API finalizada
    [ClaimsAuthorize("Catalogo", "Ler")]
    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto> ProdutoDetalhe(Guid id)
    {
        return await _produtoRepository.ObterPorId(id);
    }
}