using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using DM.Core.Communication;

namespace DM.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected ICollection<string> Erros = new List<string>();

    protected ActionResult ValidarResposta(object result = null)
    {
        if (OperacaoValida())
        {
            return Ok(result);
        }

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
    }

    protected ActionResult ValidarResposta(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return ValidarResposta();
    }

    protected ActionResult ValidarResposta(ValidationResult validationResult)
    {
        AdicionarErroProcessamento(validationResult.ErrorMessage);

        return ValidarResposta();
    }

    protected bool RespostaPossuiErros(ResponseResult resposta)
    {
        if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

        foreach (var mensagem in resposta.Errors.Mensagens)
        {
            AdicionarErroProcessamento(mensagem);
        }

        return true;
    }

    protected bool OperacaoValida()
    {
        return !Erros.Any();
    }

    protected void AdicionarErroProcessamento(string erro)
    {
        Erros.Add(erro);
    }

    protected void LimparErrosProcessamento()
    {
        Erros.Clear();
    }
}
