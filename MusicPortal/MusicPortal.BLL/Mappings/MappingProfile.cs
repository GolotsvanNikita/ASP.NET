using AutoMapper;
using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Entities;

namespace MusicPortal.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

            CreateMap<Song, SongDTO>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<SongDTO, Song>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Genre, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}