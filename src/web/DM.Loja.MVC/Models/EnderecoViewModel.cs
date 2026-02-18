using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DM.Loja.MVC.Models;

public class EnderecoViewModel
{
    public required string Logradouro { get; set; }
    [DisplayName("Número")]
    public required string Numero { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    [DisplayName("CEP")]
    public required string Cep { get; set; }
    public required string Cidade { get; set; }
    public required string Estado { get; set; }

    public override string ToString()
    {
        return $"{Logradouro}, {Numero} {Complemento} - {Bairro} - {Cidade} - {Estado}";
    }
}