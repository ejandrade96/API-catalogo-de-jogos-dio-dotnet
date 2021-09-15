using System.ComponentModel.DataAnnotations;

namespace DIO.CatalogoJogos.Api.DTOs
{
  public class ProdutoraInputModel
  {
    [Required(ErrorMessage = "Não é possível salvar uma produtora com nome em branco.", AllowEmptyStrings = false)]
    public string Nome { get; set; }
  }
}