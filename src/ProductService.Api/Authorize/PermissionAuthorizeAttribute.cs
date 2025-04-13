using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ProductService.Api.Authorize;
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(string permission)
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        Policy = permission;
    }
}