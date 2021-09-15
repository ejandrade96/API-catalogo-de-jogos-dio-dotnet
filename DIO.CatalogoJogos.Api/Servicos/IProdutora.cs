using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.DTOs;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public interface IProdutora : IDisposable
  {
    Task<DTOs.Produtora> Inserir(ProdutoraInputModel dadosProdutora);

    Task<List<DTOs.Produtora>> Listar();

    Task<IResposta<DTOs.Produtora>> ObterPorId(Guid id);

    Task<IResposta<DTOs.Produtora>> Atualizar(Guid id, ProdutoraInputModel dadosProdutora);

    Task<IResposta<DTOs.Produtora>> Remover(Guid id);
  }
}