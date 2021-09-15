namespace DIO.CatalogoJogos.Api.Erros
{
  public class ErroObjetoNaoEncontrado : IErro
  {
    public string Mensagem { get; set; }

    public int StatusCode { get; set; }

    public ErroObjetoNaoEncontrado(string entidade)
    {
      Mensagem = $"{entidade} não encontrado(a)!";
      StatusCode = 404;
    }
  }
}