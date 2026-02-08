using DM.Loja.MVC.Models;
using DM.Loja.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DM.Loja.MVC.Controllers;

public class IdentidadeController : MainController
{
    private readonly IAutenticacaoServico _autenticacaoServico;

    public IdentidadeController(IAutenticacaoServico autenticacaoServico)
    {
        _autenticacaoServico = autenticacaoServico;
    }

    [HttpGet]
    [Route("nova-conta")]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [Route("nova-conta")]
    public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid) return View(usuarioRegistro);

        var resposta = await _autenticacaoServico.Registro(usuarioRegistro);

        if (RespostaPossuiErros(resposta.ResultadoResposta)) return View(usuarioRegistro);

        await _autenticacaoServico.RealizarLogin(resposta);

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(usuarioLogin);

        var resposta = await _autenticacaoServico.Login(usuarioLogin);

        if (RespostaPossuiErros(resposta.ResultadoResposta)) return View(usuarioLogin);

        await _autenticacaoServico.RealizarLogin(resposta);

        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalogo");

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Catalogo");
    }
}
