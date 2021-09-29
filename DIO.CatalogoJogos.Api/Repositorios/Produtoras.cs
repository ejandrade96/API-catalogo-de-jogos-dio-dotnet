using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public class Produtoras : IProdutoras
  {
    private readonly List<Produtora> _produtoras = new List<Produtora>
    {
      new Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") }
    };

    public Task<Produtora> Inserir(Produtora produtora)
    {
      return Task.FromResult(_produtoras.First());
    }

    public Task<List<Produtora>> Listar()
    {
      return Task.FromResult(_produtoras);
    }
    
    public Task<Produtora> ObterPorId(Guid id)
    {
      return Task.FromResult(_produtoras.FirstOrDefault(x => x.Id.Equals(id)));
    }

    public async Task Atualizar(Produtora produtora)
    {
      await Task.Run(() =>
      {
        _produtoras[0] = produtora;
      });
    }

    public async Task Remover(Guid id)
    {
      var produtora = await ObterPorId(id);

      await Task.Run(() =>
      {
        _produtoras.Remove(produtora);
      });
    }

    public void Dispose()
    {
    }
  }
}