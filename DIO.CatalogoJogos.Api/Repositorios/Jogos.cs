using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Api.Repositorios
{
  public class Jogos : IJogos
  {
    private readonly List<Jogo> _jogos = new List<Jogo>
    {
      new Modelos.Jogo("FIFA 21", 288.29, "Esportes", new Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") })
      { Id = Guid.Parse("A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9"), ProdutoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") },
      new Modelos.Jogo("FIFA 22", 288.29, "Esportes", new Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") })
      { Id = Guid.Parse("A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9"), ProdutoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") },
      new Modelos.Jogo("FIFA 21", 288.29, "Esportes", new Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") })
      { Id = Guid.Parse("A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9"), ProdutoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") },
      new Modelos.Jogo("FIFA 21", 288.29, "Esportes", new Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") })
      { Id = Guid.Parse("A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9"), ProdutoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") }
    };

    public Task<Jogo> Inserir(Jogo jogo)
    {
      return Task.FromResult(_jogos.First());
    }

    public Task<Jogo> ObterPorNomeEProdutoraId(string nome, Guid produtoraId)
    {
      return Task.FromResult(_jogos.FirstOrDefault(x => x.Nome.Equals(nome) && x.Produtora.Id == produtoraId));
    }

    public Task<List<Jogo>> Listar(int pagina, int quantidade, Guid? produtoraId)
    {
      if (produtoraId != Guid.Empty)
        return Task.FromResult(_jogos.Where(x => x.Produtora.Id == produtoraId).Skip((pagina - 1) * quantidade).Take(quantidade).ToList());

      return Task.FromResult(_jogos.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
    }

    public Task<Jogo> ObterPorId(Guid id) => Task.FromResult(_jogos.FirstOrDefault(x => x.Id == id));

    public Task Atualizar(Jogo jogo)
    {
      return Task.FromResult(jogo);
    }

    public async Task Remover(Guid id)
    {
      var jogo = await ObterPorId(id);

      await Task.Run(() => _jogos.Remove(jogo));
    }

    public void Dispose()
    {
    }
  }
}