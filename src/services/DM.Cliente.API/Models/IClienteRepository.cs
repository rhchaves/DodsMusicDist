using DM.Core.Data;

namespace DM.Clientes.API.Models;

public interface IClienteRepository : IRepository<Cliente>
{
    void Adicionar(Cliente cliente);
    Task<IEnumerable<Cliente>> ObterTodos();
    Task<Cliente> ObterPorCpf(string cpf);
}