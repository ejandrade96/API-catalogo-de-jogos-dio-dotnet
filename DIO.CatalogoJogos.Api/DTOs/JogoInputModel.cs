using System;
using System.ComponentModel.DataAnnotations;

namespace DIO.CatalogoJogos.Api.DTOs
{
  public class JogoInputModel
  {
    [Required(ErrorMessage = "Não é possível salvar um jogo com nome em branco.", AllowEmptyStrings = false)]
    public string Nome { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O preço do jogo precisa ser maior do que {1}.")]
    public double Preco { get; set; }

    public string Categoria { get; set; }
  }
}