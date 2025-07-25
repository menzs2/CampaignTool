using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend
{
    public class UserService
    {
        private readonly CampaignToolContext _context;

        public UserService(CampaignToolContext context)
        {
            _context = context;
        }
    }
}
