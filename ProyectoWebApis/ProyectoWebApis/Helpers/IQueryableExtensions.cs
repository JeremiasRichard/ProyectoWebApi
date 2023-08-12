using ProyectoWebApis.DTOs;

namespace ProyectoWebApis.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ToPaginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage);
        }
    }
}
