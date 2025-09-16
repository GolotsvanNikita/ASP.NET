using System.ComponentModel.DataAnnotations;
using TopFilms.Annotations;

namespace TopFilms.Models
{
    public class Film
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Director is required")]
        [ForbiddenDirectorsAnnotation(["DirectorX", "DirectorY", "DirectorZ", "DirectorJ"], ErrorMessage = "This director is forbidden")]
        public string? Director { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        public string? Genre { get; set; }
        [Range(1880, 2025, ErrorMessage = "Year must be between 1880 and 2025")]
        public int Year { get; set; }
        public string? Description { get; set; }
        public string? PosterPath { get; set; }
    }
}
