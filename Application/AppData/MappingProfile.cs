using AutoMapper;
using Domain.Entities;

namespace Application.AppData
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Domain.DTO.Catalogo, Catalogo>();
            this.CreateMap<Domain.DTO.Produto, Produto>();
            this.CreateMap<Domain.DTO.Categoria, Categoria>();
        }
    }
}
