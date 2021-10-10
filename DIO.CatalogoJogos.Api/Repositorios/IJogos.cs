using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public interface IJogos : IDisposable
  {
    Task<Jogo> ObterPorNomeEProdutoraId(string nome, Guid produtoraId);

    Task<Jogo> Inserir(Jogo jogo);

    Task<List<Jogo>> Listar(int pagina, int quantidade, Guid? produtoraId);

    Task<Jogo> ObterPorId(Guid id);

    Task Atualizar(Jogo jogo);

    Task Remover(Guid id);

    /* public void Dispose()
     {
          sqlConnection?.Close();
          sqlConnection?.Dispose();
     }*/
  }
}