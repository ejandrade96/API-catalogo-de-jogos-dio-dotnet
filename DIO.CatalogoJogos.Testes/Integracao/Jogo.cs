using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace DIO.CatalogoJogos.Testes.Integracao
{
  public class Jogo : IntegracaoBase
  {
    [Fact]
    public async Task Deve_Cadastrar_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var jogo = new
      {
        Nome = "FIFA 23",
        Preco = 288.29,
        Categoria = "Esportes"
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B/jogos", ConverterParaJSON<Object>(jogo));
      var jogoEmJson = await retorno.Content.ReadAsStringAsync();
      var jogoCadastrado = Converter<Dictionary<string, string>>(jogoEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.Created);
      retorno.Headers.Location.ToString().Should().Contain("/api/V1/jogos/");
      jogoCadastrado["id"].Should().NotBeNullOrWhiteSpace();
      jogoCadastrado["id"].ToCharArray().Should().HaveCount(36);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Um_Jogo_Para_Uma_Produtora_Inexistente()
    {
      var jogo = new
      {
        Nome = "FIFA 22",
        Preco = 288.29,
        Categoria = "Esportes"
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras/9A1DCF8D-034C-4EE1-82D2-2D6735B223EA/jogos", ConverterParaJSON<Object>(jogo));
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Produtora não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Um_Jogo_Com_Nome_Em_Branco()
    {
      var jogo = new
      {
        Nome = "    ",
        Preco = 288.29,
        Categoria = "Esportes"
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B/jogos", ConverterParaJSON<Object>(jogo));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Nome"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("Não é possível salvar um jogo com nome em branco.");
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Um_Jogo_Com_Preco_Invalido()
    {
      var jogo = new
      {
        Nome = "FIFA 22",
        Preco = -1,
        Categoria = "Esportes"
      };

      var retorno = await _api.PostAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B/jogos", ConverterParaJSON<Object>(jogo));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Preco"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("O preço do jogo precisa ser maior do que 0.");
    }

    [Fact]
    public async Task Deve_Listar_Os_Jogos_Cadastrados_De_Uma_Produtora()
    {
      var retorno = await _api.GetAsync("/api/V1/produtoras/C8133002-F17A-465D-905B-F2EA6B69AF9B/jogos?pagina=1&quantidade=3");
      var jogosEmJson = await retorno.Content.ReadAsStringAsync();
      var jogos = Converter<List<Dictionary<string, object>>>(jogosEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.OK);
      jogos.Should().HaveCount(3);
      jogos.ForEach((jogo) =>
      {
        jogo.Should().ContainKey("id");
        jogo.Should().ContainKey("nome");
        jogo["id"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["id"].ToString().ToCharArray().Should().HaveCount(36);
        jogo["nome"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["preco"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["categoria"].ToString().Should().NotBeNullOrWhiteSpace();
      });
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Listar_Os_Jogos_De_Uma_Produtora_Inexistente()
    {
      var retorno = await _api.GetAsync("/api/V1/produtoras/9A1DCF8D-034C-4EE1-82D2-2D6735B223EA/jogos");
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Produtora não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Listar_Os_Jogos_Cadastrados()
    {
      var retorno = await _api.GetAsync("/api/V1/jogos?pagina=1&quantidade=3");
      var jogosEmJson = await retorno.Content.ReadAsStringAsync();
      var jogos = Converter<List<Dictionary<string, object>>>(jogosEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.OK);
      jogos.Should().HaveCount(3);
      jogos.ForEach((jogo) =>
      {
        jogo.Should().ContainKey("id");
        jogo.Should().ContainKey("nome");
        jogo.Should().ContainKey("preco");
        jogo.Should().ContainKey("categoria");
        jogo["id"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["id"].ToString().ToCharArray().Should().HaveCount(36);
        jogo["nome"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["preco"].ToString().Should().NotBeNullOrWhiteSpace();
        jogo["categoria"].ToString().Should().NotBeNullOrWhiteSpace();
      });
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Listar_Mais_De_50_Jogos_Cadastrados_Por_Pagina()
    {
      var retorno = await _api.GetAsync("/api/V1/jogos?pagina=1&quantidade=51");
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["quantidade"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("O limite máximo deve ser de 50 jogos por página.");
    }

    [Fact]
    public async Task Deve_Retornar_Um_Jogo_Por_Id()
    {
      var retorno = await _api.GetAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9");
      var jogoEmJson = await retorno.Content.ReadAsStringAsync();
      var jogo = Converter<Dictionary<string, object>>(jogoEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.OK);
      jogo.Should().ContainKey("id");
      jogo.Should().ContainKey("nome");
      jogo.Should().ContainKey("preco");
      jogo.Should().ContainKey("categoria");
      jogo["id"].ToString().Should().NotBeNullOrWhiteSpace();
      jogo["id"].ToString().ToCharArray().Should().HaveCount(36);
      jogo["nome"].ToString().Should().NotBeNullOrWhiteSpace();
      jogo["preco"].ToString().Should().NotBeNullOrWhiteSpace();
      jogo["categoria"].ToString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Um_Jogo_Inexistente_Por_Id()
    {
      var retorno = await _api.GetAsync("/api/V1/jogos/39D40DA2-3DD9-4A0C-9BE0-F59AA0DD3856");
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Jogo não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Atualizar_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var jogo = new
      {
        Nome = "FIFA 21",
        Preco = 140.54,
        Categoria = "Esportes"
      };

      var retorno = await _api.PutAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9", ConverterParaJSON<Object>(jogo));

      retorno.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Um_Jogo_Inexistente()
    {
      var jogo = new
      {
        Nome = "FIFA 23",
        Preco = 352.21,
        Categoria = "Esportes"
      };

      var retorno = await _api.PutAsync("/api/V1/jogos/39D40DA2-3DD9-4A0C-9BE0-F59AA0DD3856", ConverterParaJSON<Object>(jogo));
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Jogo não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Um_Jogo_Com_Nome_Em_Branco()
    {
      var jogo = new
      {
        Nome = "   ",
        Preco = 352.21,
        Categoria = "Esportes"
      };

      var retorno = await _api.PutAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9", ConverterParaJSON<Object>(jogo));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Nome"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("Não é possível salvar um jogo com nome em branco.");
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Um_Jogo_Com_Preco_Invalido()
    {
      var jogo = new
      {
        Nome = "FIFA 22",
        Preco = -1,
        Categoria = "Esportes"
      };

      var retorno = await _api.PutAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9", ConverterParaJSON<Object>(jogo));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson)["Preco"];

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("O preço do jogo precisa ser maior do que 0.");
    }

    [Fact]
    public async Task Deve_Atualizar_O_Nome_De_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var retorno = await _api.PatchAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9/nome", ConverterParaJSON<string>("FIFA 23"));

      retorno.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_O_Nome_De_Um_Jogo_E_Enviar_O_Dado_Em_Branco()
    {
      var retorno = await _api.PatchAsync("/api/V1/jogos/A4C69C2F-D2CA-4D03-AFD7-6022400DB8E9/nome", ConverterParaJSON<string>("   "));
      var mensagemEmJson = await retorno.Content.ReadAsStringAsync();
      var mensagem = Converter<Dictionary<string, object>>(mensagemEmJson);
      var errosEmJson = mensagem["errors"].ToString();
      var erros = Converter<Dictionary<string, string[]>>(errosEmJson).Values.ElementAt(0);

      retorno.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      erros[0].Should().Be("Não é possível salvar um jogo com nome em branco.");
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_O_Nome_De_Um_Jogo_Inexistente()
    {
      var retorno = await _api.PatchAsync("/api/V1/jogos/39D40DA2-3DD9-4A0C-9BE0-F59AA0DD3856/nome", ConverterParaJSON<string>("FIFA 23"));
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Jogo não encontrado(a)!");
    }

    [Fact]
    public async Task Deve_Deletar_Um_Jogo()
    {
      var retorno = await _api.DeleteAsync("/api/V1/jogos/18214600-A5A1-4D21-A9E2-DA9DC817AB0F");

      retorno.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Um_Jogo_Inexistente()
    {
      var retorno = await _api.DeleteAsync("/api/V1/jogos/39D40DA2-3DD9-4A0C-9BE0-F59AA0DD3856");
      var erroEmJson = await retorno.Content.ReadAsStringAsync();
      var erro = Converter<Dictionary<string, string>>(erroEmJson);

      retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
      retorno.StatusCode.Should().Be(404);
      erro["mensagem"].Should().Be("Jogo não encontrado(a)!");
    }
  }
}