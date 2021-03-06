using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DIO.CatalogoJogos.Api.Erros;
using DIO.CatalogoJogos.Api.Repositorios;
using DIO.CatalogoJogos.Api.Servicos;
using FluentAssertions;
using Moq;
using Xunit;
using DTOs = DIO.CatalogoJogos.Api.DTOs;
using Modelos = DIO.CatalogoJogos.Api.Modelos;

namespace DIO.CatalogoJogos.Testes.Unidade.Servicos
{
  public class Jogo
  {
    private readonly IJogo _servico;

    private readonly Mock<IJogos> _jogos;

    private readonly Mock<IProdutoras> _produtoras;

    private readonly Mock<IMapper> _mapper;

    public Jogo()
    {
      _jogos = new Mock<IJogos>();
      _produtoras = new Mock<IProdutoras>();
      _mapper = new Mock<IMapper>();
      _servico = new DIO.CatalogoJogos.Api.Servicos.Jogo(_jogos.Object, _produtoras.Object, _mapper.Object);
    }

    [Fact]
    public async Task Deve_Cadastrar_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 22",
        Preco = 288.29,
        Categoria = "Esportes"
      };
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 22", 288.29, "Esportes", produtora) { Id = Guid.NewGuid() };
      var dadosJogo = new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 22", Preco = 288.29, Categoria = "Esportes" };

      _produtoras.Setup(repository => repository.ObterPorId(produtora.Id)).Returns(Task.FromResult(produtora));
      _mapper.Setup(mapper => mapper.Map<DTOs.Jogo>(jogo)).Returns(dadosJogo);

      var resposta = await _servico.Inserir(jogoInputModel, produtora.Id);
      var jogoCadastrado = resposta.Resultado;

      resposta.Erro.Should().BeNull();
      jogo.Should().NotBeNull();
      jogo.Id.Should().NotBe(Guid.Empty);
      jogo.Nome.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Um_Jogo_Para_Uma_Produtora_Inexistente()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 22",
        Preco = 288.29,
        Categoria = "Esportes"
      };
      var produtoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B");

      var resposta = await _servico.Inserir(jogoInputModel, produtoraId);

      resposta.Erro.Mensagem.Should().Be("Produtora n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Um_Jogo_Com_Mesmo_Nome_Para_Mesma_Produtora()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 22",
        Preco = 288.29,
        Categoria = "Esportes"
      };
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 22", 288.29, "Esportes", produtora) { Id = Guid.NewGuid() };

      _produtoras.Setup(repository => repository.ObterPorId(produtora.Id)).Returns(Task.FromResult(produtora));
      _jogos.Setup(repository => repository.ObterPorNomeEProdutoraId(jogoInputModel.Nome, produtora.Id)).Returns(Task.FromResult(jogo));

      var resposta = await _servico.Inserir(jogoInputModel, produtora.Id);

      resposta.Erro.Mensagem.Should().Be("Jogo j?? cadastrado(a) com este nome!");
      resposta.Erro.StatusCode.Should().Be(400);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoExistente));
    }

    [Fact]
    public async Task Deve_Listar_Os_Jogos_Cadastrados()
    {
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogos = new List<Modelos.Jogo>
      {
        new Modelos.Jogo("FIFA 20", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() },
        new Modelos.Jogo("FIFA 21", 110.29, "Esportes", produtora) { Id = Guid.NewGuid() },
        new Modelos.Jogo("FIFA 22", 288.29, "Esportes", produtora) { Id = Guid.NewGuid() },
      };
      var dadosJogos = new List<DTOs.Jogo>
      {
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 20", Preco = 78.29, Categoria = "Esportes" },
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 21", Preco = 110.29, Categoria = "Esportes" },
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 22", Preco = 288.29, Categoria = "Esportes" }
      };

      _jogos.Setup(repository => repository.Listar(1, 3, Guid.Empty)).Returns(Task.FromResult<List<Modelos.Jogo>>(jogos));
      _mapper.Setup(mapper => mapper.Map<List<DTOs.Jogo>>(jogos)).Returns(dadosJogos);

      var resposta = await _servico.Listar(1, 3, Guid.Empty);
      var jogosEncontrados = resposta.Resultado;

      resposta.Erro.Should().BeNull();
      jogosEncontrados.Should().HaveCount(3);
    }

    [Fact]
    public async Task Deve_Listar_Os_Jogos_Cadastrados_De_Uma_Produtora()
    {
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogos = new List<Modelos.Jogo>
      {
        new Modelos.Jogo("FIFA 20", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() },
        new Modelos.Jogo("FIFA 21", 110.29, "Esportes", produtora) { Id = Guid.NewGuid() },
        new Modelos.Jogo("FIFA 22", 288.29, "Esportes", produtora) { Id = Guid.NewGuid() },
      };
      var dadosJogos = new List<DTOs.Jogo>
      {
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 20", Preco = 78.29, Categoria = "Esportes" },
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 21", Preco = 110.29, Categoria = "Esportes" },
        new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 22", Preco = 288.29, Categoria = "Esportes" }
      };

      _produtoras.Setup(repository => repository.ObterPorId(produtora.Id)).Returns(Task.FromResult(produtora));
      _jogos.Setup(repository => repository.Listar(1, 3, produtora.Id)).Returns(Task.FromResult<List<Modelos.Jogo>>(jogos));
      _mapper.Setup(mapper => mapper.Map<List<DTOs.Jogo>>(jogos)).Returns(dadosJogos);

      var resposta = await _servico.Listar(1, 3, produtora.Id);
      var jogosEncontrados = resposta.Resultado;

      resposta.Erro.Should().BeNull();
      jogosEncontrados.Should().HaveCount(3);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Listar_Os_Jogos_De_Uma_Produtora_Inexistente()
    {
      var produtoraId = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B");

      var resposta = await _servico.Listar(1, 3, produtoraId);

      resposta.Erro.Mensagem.Should().Be("Produtora n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Retornar_Um_Jogo_Por_Id()
    {
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 20", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() };
      var dadosJogo = new DTOs.Jogo { Id = Guid.NewGuid(), Nome = "FIFA 20", Preco = 78.29, Categoria = "Esportes" };

      _jogos.Setup(repository => repository.ObterPorId(jogo.Id)).Returns(Task.FromResult<Modelos.Jogo>(jogo));
      _mapper.Setup(mapper => mapper.Map<DTOs.Jogo>(jogo)).Returns(dadosJogo);

      var resposta = await _servico.ObterPorId(jogo.Id);
      var jogoEncontrado = resposta.Resultado;

      resposta.Erro.Should().BeNull();
      jogoEncontrado.Should().NotBeNull();
      jogoEncontrado.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Um_Jogo_Inexistente_Por_Id()
    {
      var resposta = await _servico.ObterPorId(Guid.NewGuid());

      resposta.Erro.Mensagem.Should().Be("Jogo n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Atualizar_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 23",
        Preco = 352.21,
        Categoria = "Esportes"
      };
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 20", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() };

      _jogos.Setup(repository => repository.ObterPorId(jogo.Id)).Returns(Task.FromResult<Modelos.Jogo>(jogo));

      var resposta = await _servico.Atualizar(jogoInputModel, jogo.Id);

      resposta.Erro.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Um_Jogo_Inexistente()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 23",
        Preco = 352.21,
        Categoria = "Esportes"
      };

      var resposta = await _servico.Atualizar(jogoInputModel, Guid.NewGuid());

      resposta.Erro.Mensagem.Should().Be("Jogo n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Um_Jogo_Com_Nome_Ja_Existente_Na_Mesma_Produtora()
    {
      var jogoInputModel = new DTOs.JogoInputModel
      {
        Nome = "FIFA 22",
        Preco = 288.29,
        Categoria = "Esportes"
      };
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 22", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() };
      var jogoRepetido = new Modelos.Jogo("FIFA 22", 154.68, "Esportes", produtora) { Id = Guid.NewGuid() };

      _jogos.Setup(repository => repository.ObterPorId(jogo.Id)).Returns(Task.FromResult<Modelos.Jogo>(jogo));
      _jogos.Setup(repository => repository.ObterPorNomeEProdutoraId(jogoInputModel.Nome, produtora.Id)).Returns(Task.FromResult(jogoRepetido));

      var resposta = await _servico.Atualizar(jogoInputModel, jogo.Id);

      resposta.Erro.Mensagem.Should().Be("Jogo j?? cadastrado(a) com este nome!");
      resposta.Erro.StatusCode.Should().Be(400);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoExistente));
    }

    [Fact]
    public async Task Deve_Deletar_Um_Jogo()
    {
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 22", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() };

      _jogos.Setup(repository => repository.ObterPorId(jogo.Id)).Returns(Task.FromResult<Modelos.Jogo>(jogo));

      var resposta = await _servico.Remover(jogo.Id);

      resposta.Erro.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Um_Jogo_Inexistente()
    {
      var resposta = await _servico.Remover(Guid.NewGuid());

      resposta.Erro.Mensagem.Should().Be("Jogo n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Atualizar_O_Nome_De_Um_Jogo_Quando_Enviar_Dados_Certos()
    {
      var produtora = new Modelos.Produtora("EA Sports") { Id = Guid.Parse("C8133002-F17A-465D-905B-F2EA6B69AF9B") };
      var jogo = new Modelos.Jogo("FIFA 20", 78.29, "Esportes", produtora) { Id = Guid.NewGuid() };

      _jogos.Setup(repository => repository.ObterPorId(jogo.Id)).Returns(Task.FromResult<Modelos.Jogo>(jogo));

      var resposta = await _servico.Atualizar("FIFA 23", jogo.Id);

      resposta.Erro.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_O_Nome_De_Um_Jogo_Inexistente()
    {
      var resposta = await _servico.Atualizar("FIFA 23", Guid.NewGuid());

      resposta.Erro.Mensagem.Should().Be("Jogo n??o encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }
  }
}