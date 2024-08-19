namespace Rattrapage_API.Profile
{
    using AutoMapper;
    using Rattrapage_API.Dto;
    using Rattrapage_API.Models;

    public class ArtisteMappingProfile : Profile
    {
        public ArtisteMappingProfile()
        {
            // Mapping from ArtisteDTO to Artiste
            CreateMap<ArtisteDto, Artiste>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id when mapping from DTO to model

            // Mapping from Artiste to ArtisteDTO
            CreateMap<Artiste, ArtisteDto>();
        }
    }

}
