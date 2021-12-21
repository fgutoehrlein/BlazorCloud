using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlazorCloud.Areas.Authorization
{
    public interface IBasicAuthorization
    {
        Task<bool> BasicAuthIsValid(HttpContext httpContext);
    }
}