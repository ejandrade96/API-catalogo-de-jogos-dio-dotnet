using System;

namespace DIO.CatalogoJogos.Api.DTOs
{
  public class Jogo
  {
    public Guid Id { get; set; }

    public string Nome { get; set; }

    public double Preco { get; set; }

    public string Categoria { get; set; }
  }
}