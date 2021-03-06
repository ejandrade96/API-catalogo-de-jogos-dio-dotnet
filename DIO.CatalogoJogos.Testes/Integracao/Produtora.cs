using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace DIO.CatalogoJogos.Testes.Integracao
{
  public class Produtora : IntegracaoBase
  {
    [Fact]
    public async Task Deve_Cadastrar_Uma_Produtora_Quando_Enviar_Dados_Certos()
    {
      var produtora = new
      {
        Nome = "EA Sports"
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras", ConverterParaJSON<Object>(produtora));
      var produtoraEmJson = await retorno.Content.ReadAsStringAsync();
      var produtoraCadastrada = Converter<Dictionary<string, string>>(produtoraEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.Created);
      retorno.Headers.Location.ToString().Should().Contain("/api/V1/produtoras/");
      produtoraCadastrada["id"].Should().NotBeNullOrWhiteSpace();
      produtoraCadastrada["id"].ToCharArray().Should().HaveCount(36);
      produtoraCadastrada["nome"].Should().Be(produtora.Nome);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Uma_Produtora_Com_Nome_Em_Branco()
    {
      var produtora = new
      {
        Nome = "   "
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras", ConverterParaJSON<Object>(produtora));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Nome"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("Não é possível salvar uma produtora com nome em branco.");
    }

    [Fact]
    public async Task Deve_Listar_Todas_As_Produtoras_Cadastradas()
    {
      var retorno = await _api.GetAsync("/api/V1/produtoras");
      var produtorasEmJson = await retorno.Content.ReadAsStringAsync();
      var produtoras = Converter<List<Dictionary<string, object>>>(produtorasEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.OK);
      produtoras.Should().HaveCountGreaterThan(0);
      produtoras.ForEach((produtora) =>
      {
        produtora.Should().ContainKey("id");
        produtora.Should().ContainKey("nome");
        produtora["id"].ToString().Should().NotBeNullOrWhiteSpace();
        produtora["id"].ToString().ToCharArray().Should().HaveCount(36);
        produtora["nome"].ToString().Should().NotBeNullOrWhiteSpace();
      });
    }

    [Fact]
    public async Task Deve_Retornar_Uma_Produtora_Por_Id()
    {
      var retorno = await _api.GetAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B");
      var produtoraEmJson = await retorno.Content.ReadAsStringAsync();
      var produtora = Converter<Dictionary<string, object>>(produtoraEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.OK);
      produtora.Should().ContainKey("id");
      produtora.Should().ContainKey("nome");
      produtora["id"].ToString().Should().NotBeNullOrWhiteSpace();
      produtora["id"].ToString().ToCharArray().Should().HaveCount(36);
      produtora["nome"].ToString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Uma_Produtora_Inexistente_Por_Id()
    {
      var retorno = await _api.GetAsync("/api/V1/produtoras/9A1DCF8D-034C-4EE1-82D2-2D6735B223EA");
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Produtora não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Atualizar_Uma_Produtora_Quando_Enviar_Dados_Certos()
    {
      var produtora = new
      {
        Nome = "Konami"
      };

      var retorno = await _api.PutAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B", ConverterParaJSON<Object>(produtora));

      retorno.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Produtora_Inexistente()
    {
      var produtora = new
      {
        Nome = "Konami"
      };

      var retorno = await _api.PutAsync("/api/V1/produtoras/9A1DCF8D-034C-4EE1-82D2-2D6735B223EA", ConverterParaJSON<Object>(produtora));
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Produtora não encontrado(a)!");

    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Produtora_Com_Nome_Em_Branco()
    {
      var produtora = new
      {
        Nome = "   "
      };

      var retorno = await _api.PutAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B", ConverterParaJSON<Object>(produtora));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Nome"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("Não é possível salvar uma produtora com nome em branco.");
    }

    [Fact]
    public async Task Deve_Deletar_Uma_Produtora()
    {
      var retorno = await _api.DeleteAsync("/api/V1/produtoras/69BBD13A-FC21-40FF-8925-A31A4A6C21CD");

      retorno.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Uma_Produtora_Inexistente()
    {
      var retorno = await _api.DeleteAsync("/api/V1/produtoras/9A1DCF8D-034C-4EE1-82D2-2D6735B223EA");
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Produtora não encontrado(a)!");
    }
  }
}