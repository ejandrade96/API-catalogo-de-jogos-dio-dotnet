using AutoMapper;

namespace DIO.CatalogoJogos.Api.Mapeamentos
{
  public class Jogo : Profile
  {
    public Jogo()
    {
      CreateMap<Modelos.Jogo, DTOs.Jogo>().ReverseMap();
    }
  }
}