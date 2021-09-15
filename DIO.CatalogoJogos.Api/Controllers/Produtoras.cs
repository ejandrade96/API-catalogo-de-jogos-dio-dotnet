using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DIO.CatalogoJogos.Api.Servicos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DIO.CatalogoJogos.Api.Controllers
{
  [Route("api/V1/[controller]")]
  [ApiController]
  public class Produtoras : ControllerBase
  {
    private readonly IProdutora _servico;
    
    public Produtoras(IProdutora servico)
    {
      _servico = servico;
    }

    [HttpPost]
    public async Task<ActionResult<DTOs.Produtora>> Inserir([FromBody] DTOs.ProdutoraInputModel dadosProdutora)
    {
      var produtora = await _servico.Inserir(dadosProdutora);

      return Created(
        new Uri(HttpContext.Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + $"/api/V1/produtoras/{produtora.Id}",
        produtora);
    }

    [HttpGet]
    public async Task<ActionResult<List<DTOs.Produtora>>> Listar()
    {
      var produtoras = await _servico.Listar();

      return Ok(produtoras);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DTOs.Produtora>> ObterPorId(Guid id)
    {
      var resposta = await _servico.ObterPorId(id);

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return Ok(resposta.Resultado);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(Guid id, [FromBody] DTOs.ProdutoraInputModel dadosProdutora)
    {
      var resposta = await _servico.Atualizar(id, dadosProdutora);

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Remover(Guid id)
    {
      var resposta = await _servico.Remover(id);

      if (resposta.TemErro())
        return StatusCode(resposta.Erro.StatusCode, new { Mensagem = resposta.Erro.Mensagem });

      return NoContent();
    }
  }
}