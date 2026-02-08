using DM.Loja.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Extensions;

public class PaginacaoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList modeloPaginado)
    {
        return View(modeloPaginado);
    }
}