using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.DTOs;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public interface IJogo : IDisposable
  {
    Task<IResposta<DTOs.Jogo>> Inserir(JogoInputModel dadosJogo, Guid produtoraId);

    Task<IResposta<List<DTOs.Jogo>>> Listar(int pagina, int quantidade, Guid? produtoraId);

    Task<IResposta<DTOs.Jogo>> ObterPorId(Guid id);

    Task<IResposta<DTOs.Jogo>> Atualizar(JogoInputModel dadosJogo, Guid id);

    Task<IResposta<DTOs.Jogo>> Remover(Guid id);
  }
}