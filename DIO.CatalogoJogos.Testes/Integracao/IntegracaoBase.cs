using System.Net.Http;
using System.Text;
using DIO.CatalogoJogos.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace DIO.CatalogoJogos.Testes.Integracao
{
  public class IntegracaoBase
  {
    protected HttpClient _api;

    public IntegracaoBase()
    {
      var appFactory = new WebApplicationFactory<Startup>()
           .WithWebHostBuilder(builder =>
           {
             builder.ConfigureServices(services =>
             {
             });
           });

      _api = appFactory.CreateClient();

    }

    protected HttpContent ConverterParaJSON<T>(T valor) => new StringContent(JsonConvert.SerializeObject(valor), Encoding.UTF8, "application/json");

    protected T Converter<T>(string json) => JsonConvert.DeserializeObject<T>(json);
  }
}