using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("ApplicationUser")]
        public virtual User User { get; set; }
    }
}
