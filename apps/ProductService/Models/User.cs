using Microsoft.AspNetCore.Identity;

namespace GrpcServices.Models
{
    public class User: IdentityUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
