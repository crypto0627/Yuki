using Microsoft.AspNetCore.Identity;

namespace GrpcServices.Models
{
    public class User: IdentityUser
    {
         public required string Role { get; set; }
    }
}
