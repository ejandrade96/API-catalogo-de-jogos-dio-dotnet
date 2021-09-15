using System;

namespace DIO.CatalogoJogos.Api.Modelos
{
  public class Produtora
  {
    public Guid Id { get; set; }

    public string Nome { get; private set; }

    public Produtora(string nome)
    {
      Nome = nome;
    }

    public void AtualizarNome(string nome)
    {
      Nome = nome;
    }
  }
}