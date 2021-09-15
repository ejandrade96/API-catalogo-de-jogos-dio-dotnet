using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DIO.CatalogoJogos.Api.DTOs;
using DIO.CatalogoJogos.Api.Erros;
using DIO.CatalogoJogos.Api.Repositorios;

namespace DIO.CatalogoJogos.Api.Servicos
{
  public class Produtora : IProdutora
  {
    private readonly IProdutoras _produtoras;

    private readonly IMapper _mapper;

    public Produtora(IProdutoras produtoras, IMapper mapper)
    {
      _produtoras = produtoras;
      _mapper = mapper;
    }

    public async Task<DTOs.Produtora> Inserir(ProdutoraInputModel dadosProdutora)
    {
      var produtora = new Modelos.Produtora(dadosProdutora.Nome);

      var produtoraCadastrada = await _produtoras.Inserir(produtora);

      return _mapper.Map<DTOs.Produtora>(produtoraCadastrada);
    }

    public async Task<List<DTOs.Produtora>> Listar()
    {
      var produtoras = await _produtoras.Listar();

      return _mapper.Map<List<DTOs.Produtora>>(produtoras);
    }

    public async Task<IResposta<DTOs.Produtora>> ObterPorId(Guid id)
    {
      IResposta<DTOs.Produtora> resposta = new Resposta<DTOs.Produtora>();

      var produtora = await _produtoras.ObterPorId(id);

      if (produtora == null)
        resposta.Erro = new ErroObjetoNaoEncontrado("Produtora");

      else
        resposta.Resultado = _mapper.Map<DTOs.Produtora>(produtora);

      return resposta;
    }

    public async Task<IResposta<DTOs.Produtora>> Atualizar(Guid id, ProdutoraInputModel dadosProdutora)
    {
      IResposta<DTOs.Produtora> resposta = new Resposta<DTOs.Produtora>();

      var produtora = await _produtoras.ObterPorId(id);

      if (produtora == null)
        resposta.Erro = new ErroObjetoNaoEncontrado("Produtora");

      else
      {
        produtora.AtualizarNome(dadosProdutora.Nome);
        await _produtoras.Atualizar(produtora);
      }

      return resposta;
    }

    public async Task<IResposta<DTOs.Produtora>> Remover(Guid id)
    {
      IResposta<DTOs.Produtora> resposta = new Resposta<DTOs.Produtora>();

      var produtora = await _produtoras.ObterPorId(id);

      if (produtora == null)
        resposta.Erro = new ErroObjetoNaoEncontrado("Produtora");

      else
        await _produtoras.Remover(id);

      return resposta;
    }

    public void Dispose() => _produtoras?.Dispose();
  }
}