using DM.Loja.MVC.Models;

namespace DM.Loja.MVC.Services;

public interface IAutenticacaoServico
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);

    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
}
