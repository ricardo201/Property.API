using PropertyBuilding.Core.QueryFilters;
using System;

namespace PropertyBuilding.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPorpertyPaginationUri(PropertyQueryFilter filter, string actionUrl);
        Uri GetUrlImage(string pathImage);
    }
}
