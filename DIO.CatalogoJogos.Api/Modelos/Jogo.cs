using System;

namespace DIO.CatalogoJogos.Api.Modelos
{
  public class Jogo : EntidadeBase
  {
    public string Nome { get; private set; }

    public double Preco { get; private set; }

    public string Categoria { get; private set; }

    public Guid ProdutoraId { get; private set; }

    public Produtora Produtora { get; private set; }

    public Jogo(string nome, double preco, string categoria, Produtora produtora)
    {
      Nome = nome;
      Preco = preco;
      Categoria = categoria;
      Produtora = produtora;
      ProdutoraId = produtora.Id;
    }

    public Jogo(string nome, double preco, string categoria, Guid produtoraId)
    {
      Nome = nome;
      Preco = preco;
      Categoria = categoria;
      ProdutoraId = produtoraId;
    }

    public void Atualizar(DTOs.JogoInputModel jogo)
    {
      Nome = jogo.Nome;
      Preco = jogo.Preco;
      Categoria = jogo.Categoria;
    }

    public void AtualizarNome(string nome)
    {
      Nome = nome;
    }
  }
}