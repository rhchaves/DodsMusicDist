using DM.Core.Communication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers;

public class MainController : Controller
{
    protected bool RespostaPossuiErros(ResponseResult resposta)
    {
        if (resposta != null && resposta.Errors.Mensagens.Any())
        {
            foreach (var mensagem in resposta.Errors.Mensagens)
            {
                ModelState.AddModelError(string.Empty, mensagem);
            }

            return true;
        }

        return false;
    }

    protected void AdicionarErroValidacao(string mensagem)
    {
        ModelState.AddModelError(string.Empty, mensagem);
    }

    protected bool OperacaoValida()
    {
        return ModelState.ErrorCount == 0;
    }
}