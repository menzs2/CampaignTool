using Backend.Data;
using Shared;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ConnectionDto?> GetConnectionByIdAsync(int id)
        {
            var connection = await _context.Connections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<ConnectionDto> CreateConnectionAsync(ConnectionDto connectionDto)
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
            return connection.ToDto();
        }
        public async Task<ConnectionDto?> UpdateConnectionAsync(int id, ConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Connection DTO cannot be null");
            }
            var existingConnection = await _context.Connections.FindAsync(id);
            if (existingConnection == null)
            {
                return null; // Connection not found
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task<bool> DeleteConnectionAsync(int id)
        {
            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                return false; // Connection not found
            }
            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Character to Character Connection Methods

        public async Task<IEnumerable<CharCharConnectionDto>?> GetCharacterToCharacterConnectionsAsync()
        {
            var connections = await _context.CharCharConnections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<CharCharConnectionDto?> GetCharacterToCharacterConnectionByIdAsync(int id)
        {
            var connection = await _context.CharCharConnections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<CharCharConnectionDto> CreateCharacterToCharacterConnectionAsync(CharCharConnectionDto connectionDto)
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

        public async Task<CharCharConnectionDto?> UpdateCharacterToCharacterConnectionAsync(int id, CharCharConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Character Connection DTO cannot be null");
            }
            var existingConnection = await _context.CharCharConnections.FindAsync(id);
            if (existingConnection == null)
            {
                return null; // Connection not found
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task<bool> DeleteCharacterToCharacterConnectionAsync(int id)
        {
            var connection = await _context.CharCharConnections.FindAsync(id);
            if (connection == null)
            {
                return false; // Connection not found
            }
            _context.CharCharConnections.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Character to Organization Connection Methods

        public async Task<IEnumerable<CharOrgConnectionDto>?> GetCharacterToOrganizationConnectionsAsync()
        {
            var connections = await _context.CharOrgConnections.ToListAsync();
            return connections?.ToDto();
        }

        public async Task<CharOrgConnectionDto?> GetCharacterToOrganizationConnectionByIdAsync(int id)
        {
            var connection = await _context.CharOrgConnections.FindAsync(id);
            return connection?.ToDto();
        }

        public async Task<CharOrgConnectionDto> CreateCharacterToOrganizationConnectionAsync(CharOrgConnectionDto connectionDto)
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

        public async Task<CharOrgConnectionDto?> UpdateCharacterToOrganizationConnectionAsync(int id, CharOrgConnectionDto connectionDto)
        {
            if (connectionDto == null)
            {
                throw new ArgumentNullException(nameof(connectionDto), "Character to Organization Connection DTO cannot be null");
            }
            var existingConnection = await _context.CharOrgConnections.FindAsync(id);
            if (existingConnection == null)
            {
                return null; // Connection not found
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task<bool> DeleteCharacterToOrganizationConnectionAsync(int id)
        {
            var connection = await _context.CharOrgConnections.FindAsync(id);
            if (connection == null)
            {
                return false; // Connection not found
            }
            _context.CharOrgConnections.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
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
                return null; // Connection not found
            }
            _context.Update(existingConnection);
            await _context.SaveChangesAsync();
            return existingConnection.ToDto();
        }

        public async Task<bool> DeleteOrganizationToOrganizationConnectionAsync(int id)
        {
            var connection = await _context.OrgOrgConnections.FindAsync(id);
            if (connection == null)
            {
                return false; // Connection not found
            }
            _context.OrgOrgConnections.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}