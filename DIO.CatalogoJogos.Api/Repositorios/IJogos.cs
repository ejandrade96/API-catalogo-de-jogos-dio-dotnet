using System;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public interface IJogos : IDisposable
  {
    Task<Jogo> ObterPorNomeEProdutoraId(string nome, Guid produtoraId);

    Task<Modelos.Jogo> Inserir(Modelos.Jogo jogo);

    /* public void Dispose()
     {
          sqlConnection?.Close();
          sqlConnection?.Dispose();
     }*/
  }
}