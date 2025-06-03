using Microsoft.AspNetCore.Authorization;

namespace RaritetBooks.API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public HasPermissionAttribute(string permission)
        : base(policy: permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}