using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public interface IProdutoras : IDisposable
  {
    Task<Modelos.Produtora> Inserir(Modelos.Produtora produtora);

    Task<List<Modelos.Produtora>> Listar();

    Task<Modelos.Produtora> ObterPorId(Guid id);

    Task Atualizar(Modelos.Produtora produtora);

    Task Remover(Guid id);

    /* public void Dispose()
     {
          sqlConnection?.Close();
          sqlConnection?.Dispose();
     }*/
  }
}