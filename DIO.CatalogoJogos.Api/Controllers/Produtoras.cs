using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DIO.CatalogoJogos.Api.Controllers
{
  [Route("api/V1/[controller]")]
  [ApiController]
  public class Produtoras : ControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<DTOs.Produtora>> Post([FromBody] DTOs.ProdutoraInputModel produtora)
    {
      var produtoraCadastrada = new DTOs.Produtora { Id = Guid.NewGuid(), Nome = produtora.Nome };

      return Created(
        new Uri(HttpContext.Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + $"/api/V1/produtoras/{produtoraCadastrada.Id}",
        produtoraCadastrada);
    }

    [HttpGet]
    public async Task<ActionResult<List<DTOs.Produtora>>> Get()
    {
      var produtoras = new List<DTOs.Produtora>
      {
        new DTOs.Produtora { Id = Guid.NewGuid(), Nome = "Konami" },
        new DTOs.Produtora { Id = Guid.NewGuid(), Nome = "EA Sports" }
      };

      return Ok(produtoras);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DTOs.Produtora>> GetById(Guid id)
    {
      if (id == Guid.Parse("9A1DCF8D-034C-4EE1-82D2-2D6735B223EA"))
        return StatusCode(404, new { Mensagem = "Produtora não encontrado(a)!" });

      return Ok(new DTOs.Produtora { Id = Guid.NewGuid(), Nome = "Konami" });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] DTOs.ProdutoraInputModel produtora)
    {
      if (id == Guid.Parse("9A1DCF8D-034C-4EE1-82D2-2D6735B223EA"))
        return StatusCode(404, new { Mensagem = "Produtora não encontrado(a)!" });

      return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
      if (id == Guid.Parse("9A1DCF8D-034C-4EE1-82D2-2D6735B223EA"))
        return StatusCode(404, new { Mensagem = "Produtora não encontrado(a)!" });

      return NoContent();
    }
  }
}