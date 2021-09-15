using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public class Produtoras : IProdutoras
  {
    public Task Atualizar(Produtora produtora)
    {
      throw new NotImplementedException();
    }

    public Task<Produtora> Inserir(Produtora produtora)
    {
      throw new NotImplementedException();
    }

    public Task<List<Produtora>> Listar()
    {
      throw new NotImplementedException();
    }

    public Task<Produtora> ObterPorId(Guid id)
    {
      throw new NotImplementedException();
    }

    public Task Remover(Guid id)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}