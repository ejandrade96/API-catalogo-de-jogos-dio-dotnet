using System;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.DTOs;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public interface IJogo : IDisposable
  {
    Task<IResposta<DTOs.Jogo>> Inserir(JogoInputModel dadosJogo, Guid produtoraId);
  }
}