using Backend.Data;

namespace Backend.Services
{
    public class ConnectionService
    {
        private readonly CampaignToolContext _context;

        public ConnectionService(CampaignToolContext context)
        {
            _context = context;
        }
    }
}
