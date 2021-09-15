using DIO.CatalogoJogos.Api.Erros;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public class Resposta<T> : IResposta<T>
  {
    public T Resultado { get; set; }

    public IErro Erro { get; set; }

    public bool TemErro() => Erro != null;
  }
}