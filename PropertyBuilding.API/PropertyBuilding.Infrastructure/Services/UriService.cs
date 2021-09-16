using PropertyBuilding.Core.QueryFilters;
using PropertyBuilding.Infrastructure.Interfaces;
using System;

namespace PropertyBuilding.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPorpertyPaginationUri(PropertyQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }

        public Uri GetUrlImage(string pathImage)
        {
            string baseUrl = $"{_baseUri}{pathImage}";
            return new Uri(baseUrl);
        }
    }
}