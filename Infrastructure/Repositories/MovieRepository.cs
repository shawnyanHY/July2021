using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> Get30HighestRevenueMovies()
        {
            var movies = await dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();

            return movies;
        }
    }
}