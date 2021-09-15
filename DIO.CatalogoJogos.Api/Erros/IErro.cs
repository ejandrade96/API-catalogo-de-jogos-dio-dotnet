namespace DIO.CatalogoJogos.Api.Erros
{
  public interface IErro
  {
    string Mensagem { get; set; }

    int StatusCode { get; set; }
  }
}