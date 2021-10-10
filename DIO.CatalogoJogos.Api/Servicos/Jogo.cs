using System;
using System.Collections.Generic;
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

    public async Task<IResposta<List<DTOs.Jogo>>> Listar(int pagina, int quantidade, Guid? produtoraId)
    {
      IResposta<List<DTOs.Jogo>> resposta = new Resposta<List<DTOs.Jogo>>();
      var jogos = new List<Modelos.Jogo>();

      if (produtoraId != Guid.Empty)
      {
        var produtora = await _produtoras.ObterPorId(produtoraId.Value);

        if (produtora == null)
        {
          resposta.Erro = new ErroObjetoNaoEncontrado("Produtora");
          return resposta;
        }

        jogos = await _jogos.Listar(pagina, quantidade, produtoraId.Value);
      }

      else
        jogos = await _jogos.Listar(pagina, quantidade, Guid.Empty);

      resposta.Resultado = _mapper.Map<List<DTOs.Jogo>>(jogos);
      return resposta;
    }

    public async Task<IResposta<DTOs.Jogo>> ObterPorId(Guid id)
    {
      IResposta<DTOs.Jogo> resposta = new Resposta<DTOs.Jogo>();

      var jogo = await _jogos.ObterPorId(id);

      if (jogo == null)
        resposta.Erro = new ErroObjetoNaoEncontrado("Jogo");

      else
        resposta.Resultado = _mapper.Map<DTOs.Jogo>(jogo);

      return resposta;
    }

    public void Dispose() => _jogos?.Dispose();
  }
}