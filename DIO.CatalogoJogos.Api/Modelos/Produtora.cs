namespace DIO.CatalogoJogos.Api.Modelos
{
  public class Produtora : EntidadeBase
  {
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