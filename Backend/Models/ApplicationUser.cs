using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("ApplicationUser")]
        public virtual User User { get; set; }
    }
}
