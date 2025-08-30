namespace DM.Loja.MVC.Models;

public class ErrorViewModel
{
    public int CodigoErro { get; set; }
    public string Titulo { get; set; }
    public string Mensagem { get; set; }
}

public class ResultadoResposta
{
    public string Title { get; set; }
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
}

public class ResponseErrorMessages
{
    public List<string> Mensagens { get; set; }
}
