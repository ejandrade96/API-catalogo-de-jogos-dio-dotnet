using System;
using System.Threading.Tasks;
using AutoMapper;
using DIO.CatalogoJogos.Api.DTOs;
using DIO.CatalogoJogos.Api.Erros;
using DIO.CatalogoJogos.Api.Repositorios;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public class Jogo : IJogo
  {
    private readonly IJogos _jogos;

    private readonly IProdutoras _produtoras;

    private readonly IMapper _mapper;

    public Jogo(IJogos jogos, IProdutoras produtoras, IMapper mapper)
    {
      _jogos = jogos;
      _produtoras = produtoras;
      _mapper = mapper;
    }

    public async Task<IResposta<DTOs.Jogo>> Inserir(JogoInputModel dadosJogo, Guid produtoraId)
    {
      IResposta<DTOs.Jogo> resposta = new Resposta<DTOs.Jogo>();

      var produtora = await _produtoras.ObterPorId(produtoraId);

      if (produtora == null)
      {
        resposta.Erro = new ErroObjetoNaoEncontrado("Produtora");
        return resposta;
      }

      var jogoRepetido = await _jogos.ObterPorNomeEProdutoraId(dadosJogo.Nome, produtoraId);

      if (jogoRepetido != null)
        resposta.Erro = new ErroObjetoExistente("Jogo", "nome");

      else
      {
        var jogo = new Modelos.Jogo(dadosJogo.Nome, dadosJogo.Preco, dadosJogo.Categoria, produtora);

        var jogoCadastrado = await _jogos.Inserir(jogo);

        resposta.Resultado = _mapper.Map<DTOs.Jogo>(jogoCadastrado);
      }

      return resposta;
    }

    public void Dispose() => _jogos?.Dispose();
  }
}