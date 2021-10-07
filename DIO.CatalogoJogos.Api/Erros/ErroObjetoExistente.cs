namespace DIO.CatalogoJogos.Api.Erros
{
  public class ErroObjetoExistente : IErro
  {
    public string Mensagem { get; set; }

    public int StatusCode { get; set; }

    public ErroObjetoExistente(string objeto, string atributo)
    {
      this.Mensagem = $"{objeto} já cadastrado(a) com este {atributo}!";
      this.StatusCode = 400;
    }
  }
}