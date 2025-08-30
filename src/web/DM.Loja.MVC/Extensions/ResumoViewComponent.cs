using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Extensions;

public class ResumoViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
