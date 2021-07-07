using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastucture.Interfaces;
using System;

namespace SocialMedia.Infrastucture.Services
{
    public class UriService : IUriService
    {
        private readonly string BaseUri;

        public UriService(string baseUri)
        {
            BaseUri = baseUri;
        }

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{BaseUri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
