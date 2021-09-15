using DIO.CatalogoJogos.Api.Erros;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public interface IResposta<T>
  {
    T Resultado { get; set; }

    IErro Erro { get; set; }

    bool TemErro();
  }
}