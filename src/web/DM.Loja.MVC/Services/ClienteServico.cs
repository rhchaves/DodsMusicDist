using DM.Core.Communication;
using DM.Loja.MVC.Extensions;
using DM.Loja.MVC.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace DM.Loja.MVC.Services
{
    public interface IClienteServico
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }

    public class ClienteServico : Servico, IClienteServico
    {
        private readonly HttpClient _httpClient;

        public ClienteServico(HttpClient httpClient, IOptions<AppConfig> config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config.Value.ClienteUrl);
        }

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var resposta = await _httpClient.GetAsync("/cliente/endereco/");

            if (resposta.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResposta(resposta);

            return await DeserializarObjetoResposta<EnderecoViewModel>(resposta);
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var enderecoConteudo = ObterConteudo(endereco);

            var resposta = await _httpClient.PostAsync("/cliente/endereco/", enderecoConteudo);

            if (!TratarErrosResposta(resposta)) return await DeserializarObjetoResposta<ResponseResult>(resposta);

            return RetornoOk();
        }
    }
}