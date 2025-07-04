using APTXHub.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface ISearchService
    {
        Task<SearchResultDto> SearchAsync(string query, ClaimsPrincipal user);
    }
}
