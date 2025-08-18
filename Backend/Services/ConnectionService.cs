using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.Services
{
    public class ConnectionService
    {
        private readonly CampaignToolContext _context;

        public ConnectionService(CampaignToolContext context)
        {
            _context = context;
        }

        #region Connection Methods
        public async Task<IEnumerable<ConnectionDto>?> GetConnectionsAsync()
        {
            var connections = await _context.Connections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<ConnectionDto?> GetConnectionByIdAsync(long id)
        {
            var connection = await _context.Connections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<IEnumerable<ConnectionDto>?> GetConnectionByCampaignIdAsync(long id)
        {
            var connections = await _context.Connections.Where(c => c.CampaignId == id).ToListAsync();
            return connections?.ToDto();
        }

        public async Task<ConnectionDto?> CreateConnectionAsync(ConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Connection DTO cannot be null");
            }
            var connection = connectionDto.ToModel();
            if (connection == null)
            {
                throw new InvalidOperationException("Failed to convert ConnectionDto to Connection model");
            }
            _context.Connections.Add(connection);
            await _context.SaveChangesAsync();
            return connection?.ToDto();
        }
        public async Task<ConnectionDto?> UpdateConnectionAsync(long id, ConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Connection DTO cannot be null");
            }
            var existingConnection = await _context.Connections.FindAsync(id);
            if (existingConnection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task DeleteConnectionAsync(long id)
        {
            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Character to Character Connection Methods

        public async Task<IEnumerable<CharCharConnectionDto>?> GetAllCharToCharConnectionsAsync()
        {
            var connections = await _context.CharCharConnections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<CharCharConnectionDto?> GetCharToCharConnectionByIdAsync(long id)
        {
            var connection = await _context.CharCharConnections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<IEnumerable<CharCharConnectionDto>?> GetCharToCharConnectionsByCampaignAsync(long id)
        {
            var connections = await _context.CharCharConnections.Include(c => c.Connection).Where(c => c.Connection.CampaignId == id).ToListAsync();
            return connections?.ToDto();
        }

        public async Task<CharCharConnectionDto> CreateCharToCharConnectionAsync(CharCharConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Character Connection DTO cannot be null");
            }
            var connection = connectionDto.ToModel();
            if (connection == null)
            {
                throw new InvalidOperationException("Failed to convert CharCharConnectionDto to CharCharConnection model");
            }
            _context.CharCharConnections.Add(connection);
            await _context.SaveChangesAsync();
            return connection.ToDto();
        }

        public async Task<CharCharConnectionDto?> UpdateCharToCharConnectionAsync(long id, CharCharConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Character Connection DTO cannot be null");
            }
            var existingConnection = await _context.CharCharConnections.FindAsync(id);
            if (existingConnection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            existingConnection.Description = connectionDto.Description;
            existingConnection.GmOnlyDescription = connectionDto.GmOnlyDescription;
            existingConnection.GmOnly = connectionDto.GmOnly;
            existingConnection.Direction = connectionDto.Direction;
            existingConnection.CharOneId = connectionDto.CharOneId;
            existingConnection.CharTwoId = connectionDto.CharTwoId;
            existingConnection.ConnectionId = connectionDto.ConnectionId;

            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task DeleteCharToCharConnectionAsync(long id)
        {
            var connection = await _context.CharCharConnections.FindAsync(id);
            if (connection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.CharCharConnections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Character to Organization Connection Methods

        public async Task<IEnumerable<CharOrgConnectionDto>?> GetAllCharToOrgConnectionsAsync()
        {
            var connections = await _context.CharOrgConnections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<CharOrgConnectionDto?> GetCharToOrgConnectionByIdAsync(long id)
        {
            var connection = await _context.CharOrgConnections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<CharOrgConnectionDto> CreateCharToOrgConnectionAsync(CharOrgConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Organization Connection DTO cannot be null");
            }
            var connection = connectionDto.ToModel();
            if (connection == null)
            {
                throw new InvalidOperationException("Failed to convert CharOrgConnectionDto to CharOrgConnection model");
            }
            _context.CharOrgConnections.Add(connection);
            await _context.SaveChangesAsync();
            return connection.ToDto();
        }

        public async Task<CharOrgConnectionDto?> UpdateCharacterToOrganizationConnectionAsync(long id, CharOrgConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Organization Connection DTO cannot be null");
            }
            var existingConnection = await _context.CharOrgConnections.FindAsync(id);
            if (existingConnection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            existingConnection.Description = connectionDto.Description;
            existingConnection.GmOnlyDescription = connectionDto.GmOnlyDescription;
            existingConnection.GmOnly = connectionDto.GmOnly;
            existingConnection.Direction = connectionDto.Direction;
            existingConnection.CharId = connectionDto.CharId;
            existingConnection.OrganisationId = connectionDto.OrganisationId;
            existingConnection.ConnectionId = connectionDto.ConnectionId;
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task DeleteCharacterToOrganizationConnectionAsync(long id)
        {
            var connection = await _context.CharOrgConnections.FindAsync(id);
            if (connection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.CharOrgConnections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Organization to Organization Connection Methods

        public async Task<IEnumerable<OrgOrgConnectionDto>?> GetOrganizationToOrganizationConnectionsAsync()
        {
            var connections = await _context.OrgOrgConnections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<OrgOrgConnectionDto?> GetOrganizationToOrganizationConnectionByIdAsync(int id)
        {
            var connection = await _context.OrgOrgConnections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<OrgOrgConnectionDto> CreateOrganizationToOrganizationConnectionAsync(OrgOrgConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Organization to Organization Connection DTO cannot be null");
            }
            var connection = connectionDto.ToModel();
            if (connection == null)
            {
                throw new InvalidOperationException("Failed to convert OrgOrgConnectionDto to OrgOrgConnection model");
            }
            _context.OrgOrgConnections.Add(connection);
            await _context.SaveChangesAsync();
            return connection.ToDto();
        }

        public async Task<OrgOrgConnectionDto?> UpdateOrganizationToOrganizationConnectionAsync(int id, OrgOrgConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Organization to Organization Connection DTO cannot be null");
            }
            var existingConnection = await _context.OrgOrgConnections.FindAsync(id);
            if (existingConnection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task DeleteOrganizationToOrganizationConnectionAsync(int id)
        {
            var connection = await _context.OrgOrgConnections.FindAsync(id);
            if (connection == null)
            {
                throw new KeyNotFoundException($"Connection with ID {id} not found.");
            }
            _context.OrgOrgConnections.Remove(connection);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}