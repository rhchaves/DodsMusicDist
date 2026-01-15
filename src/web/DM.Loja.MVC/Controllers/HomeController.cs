using DM.Loja.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers;

public class HomeController : BaseController
{
    [Route("sistema-indisponivel")]
    public IActionResult SistemaIndisponivel()
    {
        var modelErro = new ErrorViewModel
        {
            Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
            Titulo = "Sistema indisponível.",
            CodigoErro = 500
        };

        return View("Error", modelErro);
    }

    [Route("error/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if (id == 500)
        {
            modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.Titulo = "Ocorreu um erro!";
            modelErro.CodigoErro = id;
        }
        else if (id == 404)
        {
            modelErro.Mensagem =
                "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.Titulo = "Ops! Página não encontrada.";
            modelErro.CodigoErro = id;
        }
        else if (id == 403)
        {
            modelErro.Mensagem = "Você não tem permissão para fazer isto.";
            modelErro.Titulo = "Acesso Negado";
            modelErro.CodigoErro = id;
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelErro);
    }
}
