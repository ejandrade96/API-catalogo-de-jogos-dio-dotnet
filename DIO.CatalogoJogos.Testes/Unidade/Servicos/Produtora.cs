using System;
using System.Threading.Tasks;
using AutoMapper;
using DIO.CatalogoJogos.Api.Erros;
using DIO.CatalogoJogos.Api.Repositorios;
using DIO.CatalogoJogos.Api.Servicos;
using FluentAssertions;
using Moq;
using Xunit;
using Modelos = DIO.CatalogoJogos.Api.Modelos;
using DTOs = DIO.CatalogoJogos.Api.DTOs;

namespace DIO.CatalogoJogos.Testes.Unidade.Servicos
{
  public class Produtora
  {
    private readonly IProdutora _servico;

    private readonly Mock<IProdutoras> _produtoras;

    private readonly Mock<IMapper> _mapper;

    public Produtora()
    {
      _produtoras = new Mock<IProdutoras>();
      _mapper = new Mock<IMapper>();
      _servico = new DIO.CatalogoJogos.Api.Servicos.Produtora(_produtoras.Object, _mapper.Object);
    }

    [Fact]
    public async Task Deve_Retornar_Uma_Produtora_Por_Id()
    {
      var id = Guid.NewGuid();
      var produtora = new Modelos.Produtora("Konami") { Id = Guid.NewGuid() };
      var dadosProdutora = new DTOs.Produtora { Id = produtora.Id, Nome = produtora.Nome };

      _produtoras.Setup(repository => repository.ObterPorId(id)).Returns(Task.FromResult(produtora));
      _mapper.Setup(mapper => mapper.Map<DTOs.Produtora>(produtora)).Returns(dadosProdutora);

      var resposta = await _servico.ObterPorId(id);
      var produtoraEncontrada = resposta.Resultado;

      resposta.Erro.Should().BeNull();
      produtoraEncontrada.Should().NotBeNull();
      produtoraEncontrada.Id.Should().NotBe(Guid.Empty);
      produtoraEncontrada.Nome.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Uma_Produtora_Inexistente_Por_Id()
    {
      var id = Guid.NewGuid();
      var resposta = await _servico.ObterPorId(id);

      resposta.Erro.Mensagem.Should().Be("Produtora não encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Atualizar_Uma_Produtora_Quando_Enviar_Dados_Certos()
    {
      var id = Guid.NewGuid();
      var produtoraInput = new DTOs.ProdutoraInputModel { Nome = "Konami" };
      var produtora = new Modelos.Produtora("Konami") { Id = Guid.NewGuid() };

      _produtoras.Setup(repository => repository.ObterPorId(id)).Returns(Task.FromResult(produtora));

      var resposta = await _servico.Atualizar(id, produtoraInput);

      resposta.Erro.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Produtora_Inexistente()
    {
      var id = Guid.NewGuid();
      var produtoraInput = new DTOs.ProdutoraInputModel { Nome = "Konami" };

      var resposta = await _servico.Atualizar(id, produtoraInput);

      resposta.Erro.Mensagem.Should().Be("Produtora não encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }

    [Fact]
    public async Task Deve_Deletar_Uma_Produtora()
    {
      var id = Guid.NewGuid();
      var produtora = new Modelos.Produtora("Konami") { Id = Guid.NewGuid() };

      _produtoras.Setup(repository => repository.ObterPorId(id)).Returns(Task.FromResult(produtora));

      var resposta = await _servico.Remover(id);

      resposta.Erro.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Uma_Produtora_Inexistente()
    {
      var id = Guid.NewGuid();

      var resposta = await _servico.Remover(id);

      resposta.Erro.Mensagem.Should().Be("Produtora não encontrado(a)!");
      resposta.Erro.StatusCode.Should().Be(404);
      resposta.Erro.GetType().Should().Be(typeof(ErroObjetoNaoEncontrado));
    }
  }
}