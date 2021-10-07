using DIO.CatalogoJogos.Api.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace DIO.CatalogoJogos.Api.Extensoes
{
  public static class Servicos
  {
    public static void AddServicos(this IServiceCollection services)
    {
      services.AddTransient<IProdutora, Produtora>();
      services.AddTransient<IJogo, Jogo>();
    }
  }
}