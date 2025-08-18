using Microsoft.EntityFrameworkCore;

namespace Films.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Film> Films { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                Films?.Add(new Film { Name = "Inception", Director = "Christopher Nolan", Genre = "Sci-Fi, Thriller", Year = 2010, Description = "A skilled thief uses dream-sharing technology to steal corporate secrets.", PosterPath = "images/film1.png" });
                Films?.Add(new Film { Name = "The Matrix", Director = "Lana and Lilly Wachowski", Genre = "Sci-Fi, Action", Year = 1999, Description = "A hacker learns about the true nature of his reality and his role in the war against its controllers.", PosterPath = "images/film2.png" });
                Films?.Add(new Film { Name = "Interstellar", Director = "Christopher Nolan", Genre = "Sci-Fi, Drama", Year = 2014, Description = "A group of explorers travel through a wormhole in search of a new home for humanity.", PosterPath = "images/film3.png" });
                Films?.Add(new Film { Name = "The Godfather", Director = "Francis Ford Coppola", Genre = "Crime, Drama", Year = 1972, Description = "The aging patriarch of an organized crime dynasty transfers control to his reluctant son.", PosterPath = "images/film4.png" });
                Films?.Add(new Film { Name = "Pulp Fiction", Director = "Quentin Tarantino", Genre = "Crime, Drama", Year = 1994, Description = "The lives of two mob hitmen, a boxer, and others intertwine in a series of tales.", PosterPath = "images/film5.png" });
                Films?.Add(new Film { Name = "Fight Club", Director = "David Fincher", Genre = "Drama", Year = 1999, Description = "An office worker and a soap maker form an underground fight club with unexpected consequences.", PosterPath = "images/film6.png" });
                Films?.Add(new Film { Name = "Forrest Gump", Director = "Robert Zemeckis", Genre = "Drama, Romance", Year = 1994, Description = "The story of Forrest Gump, a man with a low IQ who achieves remarkable things.", PosterPath = "images/film7.png" });
                Films?.Add(new Film { Name = "The Dark Knight", Director = "Christopher Nolan", Genre = "Action, Crime", Year = 2008, Description = "Batman faces off against the Joker, a criminal mastermind causing chaos in Gotham.", PosterPath = "images/film8.png" });
                Films?.Add(new Film { Name = "The Shawshank Redemption", Director = "Frank Darabont", Genre = "Drama", Year = 1994, Description = "Two imprisoned men form a deep friendship while seeking redemption.", PosterPath = "images/film9.png" });
                Films?.Add(new Film { Name = "Spirited Away", Director = "Hayao Miyazaki", Genre = "Animation, Fantasy", Year = 2001, Description = "A young girl enters a world of spirits and gods after her parents are transformed.", PosterPath = "images/film10.png" });
                SaveChanges();
            }
        }
    }
}
