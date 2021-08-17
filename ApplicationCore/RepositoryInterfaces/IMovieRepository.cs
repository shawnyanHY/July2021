using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IMovieRepository: IAsyncRepository<Movie>
    {
        Task<IEnumerable<Movie>> Get30HighestRevenueMovies();
    }
}
