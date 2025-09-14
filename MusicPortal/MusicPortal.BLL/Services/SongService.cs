using AutoMapper;
using Microsoft.AspNetCore.Http;
using MusicPortal.BLL.DTO;
using MusicPortal.BLL.Interfaces;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.Entities;
using NAudio.Wave;
using System.IO;

namespace MusicPortal.BLL.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SongService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SongDTO>> GetAllSongs()
        {
            var songs = await _unitOfWork.Songs.GetAllSongs();
            return _mapper.Map<IEnumerable<SongDTO>>(songs);
        }

        public async Task<SongDTO?> GetSongById(int id)
        {
            var song = await _unitOfWork.Songs.GetSongById(id);
            return _mapper.Map<SongDTO>(song);
        }

        public async Task<bool> AddSong(SongDTO songDto, IFormFile? upload)
        {
            try
            {
                var song = _mapper.Map<Song>(songDto);
                if (upload != null && upload.Length > 0)
                {
                    song.FilePath = await SaveFile(upload);
                    song.Duration = GetAudioDuration(song.FilePath);
                }
                await _unitOfWork.Songs.AddSong(song);
                await _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding song: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSong(SongDTO songDto, IFormFile? upload)
        {
            try
            {
                var existingSong = await _unitOfWork.Songs.GetSongById(songDto.Id);
                if (existingSong == null)
                {
                    return false;
                }

                existingSong.Name = songDto.Name;
                existingSong.GenreId = songDto.GenreId;
                existingSong.UserId = songDto.UserId;

                if (upload != null && upload.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingSong.FilePath))
                    {
                        DeleteFile(existingSong.FilePath);
                    }
                    existingSong.FilePath = await SaveFile(upload);
                    existingSong.Duration = GetAudioDuration(existingSong.FilePath);
                }

                await _unitOfWork.Songs.UpdateSong(existingSong);
                await _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating song: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Name: {songDto.Name}, Id: {songDto.Id}");
                return false;
            }
        }

        public async Task<bool> DeleteSong(int id)
        {
            try
            {
                var song = await _unitOfWork.Songs.GetSongById(id);
                if (song != null)
                {
                    if (!string.IsNullOrEmpty(song.FilePath))
                    {
                        DeleteFile(song.FilePath);
                    }
                    await _unitOfWork.Songs.DeleteSong(id);
                }
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(byte[] fileBytes, string fileName)?> GetSongFile(int id)
        {
            try
            {
                var song = await _unitOfWork.Songs.GetSongById(id);
                if (song == null || string.IsNullOrEmpty(song.FilePath)) 
                {
                    return null;
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + song.FilePath);
                if (!File.Exists(filePath))
                {
                    return null;
                }

                var fileBytes = await File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);

                return (fileBytes, fileName);
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> SaveFile(IFormFile upload)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, upload.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            return "/Uploads/" + upload.FileName;
        }

        private double? GetAudioDuration(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + filePath);
                using (var reader = new Mp3FileReader(fullPath))
                {
                    return reader.TotalTime.TotalSeconds;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting duration: {ex.Message}");
                return null;
            }
        }

        private void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}