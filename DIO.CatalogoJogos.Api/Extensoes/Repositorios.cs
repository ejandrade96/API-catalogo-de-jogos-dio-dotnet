using DIO.CatalogoJogos.Api.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace DIO.CatalogoJogos.Api.Extensoes
{
  public static class Repositorios
  {
    public static void AddRepositorios(this IServiceCollection services)
    {
      services.AddTransient<IProdutoras, Produtoras>();
      services.AddTransient<IJogos, Jogos>();
    }
  }
}