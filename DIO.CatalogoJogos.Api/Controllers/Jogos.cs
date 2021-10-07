using System;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Servicos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DIO.CatalogoJogos.Api.Controllers
{
  [Route("api/V1/produtoras/{produtoraId}/[controller]")]
  [ApiController]
  public class Jogos : ControllerBase
  {
    private readonly IJogo _servico;

    public Jogos(IJogo servico)
    {
      _servico = servico;
    }

    [HttpPost]
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
  }
}