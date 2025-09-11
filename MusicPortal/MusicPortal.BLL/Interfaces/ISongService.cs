using Microsoft.AspNetCore.Http;
using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MusicPortal.BLL.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<SongDTO>> GetAllSongs();
        Task<SongDTO?> GetSongById(int id);
        Task<bool> AddSong(SongDTO song, IFormFile? upload);
        Task<bool> UpdateSong(SongDTO song, IFormFile? upload);
        Task<bool> DeleteSong(int id);
        Task<(byte[] fileBytes, string fileName)?> GetSongFile(int id);
    }
}
