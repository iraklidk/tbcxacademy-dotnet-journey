using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity;

public class User : IdentityUser<int>
{
    public bool IsActive { get; set; } = true;
}
