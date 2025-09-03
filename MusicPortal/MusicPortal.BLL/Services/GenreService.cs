using AutoMapper;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.Entities;

namespace MusicPortal.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GenreDTO>> GetAllGenres()
        {
            var genres = await _unitOfWork.Genres.GetAllGenres();
            return _mapper.Map<List<GenreDTO>>(genres);
        }

        public async Task<GenreDTO?> GetGenreById(int id)
        {
            var genre = await _unitOfWork.Genres.GetGenreById(id);
            return _mapper.Map<GenreDTO>(genre);
        }

        public async Task<bool> AddGenre(GenreDTO genreDto)
        {
            try
            {
                var genre = _mapper.Map<Genre>(genreDto);
                await _unitOfWork.Genres.AddGenre(genre);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGenre(GenreDTO genreDto)
        {
            try
            {
                var genre = _mapper.Map<Genre>(genreDto);
                await _unitOfWork.Genres.UpdateGenre(genre);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteGenre(int id)
        {
            try
            {
                await _unitOfWork.Genres.DeleteGenre(id);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}