using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Servicos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DIO.CatalogoJogos.Api.Controllers
{
  [ApiController]
  public class Jogos : ControllerBase
  {
    private readonly IJogo _servico;

    public Jogos(IJogo servico)
    {
      _servico = servico;
    }

    [HttpPost("api/V1/produtoras/{produtoraId}/jogos")]
    public async Task<ActionResult<DTOs.Jogo>> Inserir([FromBody] DTOs.JogoInputModel dadosJogo, Guid produtoraId)
    {
      var resposta = await _servico.Inserir(dadosJogo, produtoraId);
      var jogo = resposta.Resultado;

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return Created(
        new Uri(HttpContext.Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + $"/api/V1/jogos/{jogo.Id}",
        jogo);
    }

    [HttpGet("api/V1/produtoras/{produtoraId}/jogos")]
    public async Task<ActionResult<List<DTOs.Jogo>>> ListarPorProdutora(Guid produtoraId,
                                                                        [FromQuery, Range(1, int.MaxValue)] int pagina = 1,
                                                                        [FromQuery, Range(1, 50, ErrorMessage = "O limite máximo deve ser de {2} jogos por página.")] int quantidade = 5)
    {
      var resposta = await _servico.Listar(pagina, quantidade, produtoraId);
      var jogos = resposta.Resultado;

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return Ok(jogos);
    }

    [HttpGet("api/V1/jogos")]
    public async Task<ActionResult<List<DTOs.Jogo>>> Listar([FromQuery, Range(1, int.MaxValue)] int pagina = 1,
                                                            [FromQuery, Range(1, 50, ErrorMessage = "O limite máximo deve ser de {2} jogos por página.")] int quantidade = 5)
    {
      var resposta = await _servico.Listar(pagina, quantidade, Guid.Empty);
      var jogos = resposta.Resultado;

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return Ok(jogos);
    }

    [HttpGet("api/V1/jogos/{id}")]
    public async Task<ActionResult<DTOs.Jogo>> ObterPorId(Guid id)
    {
      var resposta = await _servico.ObterPorId(id);
      
      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      var jogo = resposta.Resultado;

      return Ok(jogo);
    }
  }
}