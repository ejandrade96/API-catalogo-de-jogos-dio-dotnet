using AutoMapper;

namespace DIO.CatalogoJogos.Api.Mapeamentos
{
  public class Produtora : Profile
  {
    public Produtora()
    {
      CreateMap<Modelos.Produtora, DTOs.Produtora>().ReverseMap();
    }
  }
}