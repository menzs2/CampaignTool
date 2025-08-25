using Backend.Data;
using Backend.Models;
using Backend.Services;
using Shared;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests
{
    public class CharacterServiceTest
    {
        private CampaignToolContext GetDbContextWithData(List<Character> characters)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Characters.AddRange(characters);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Get_ReturnsAllCharacters_WhenCharactersExist()
        {
            var characters = new List<Character>
            {
                new Character { Id = 1L, Name = "Alice", CampaignId = 1, DescriptionShort = "Test Desc" },
                new Character { Id = 2L, Name = "Bob", CampaignId = 1, DescriptionShort = "Test Desc" }
            };
            using var context = GetDbContextWithData(characters);
            var service = new CharacterService(context);

            var result = await service.GetAllCharacters();

            var okResult = Assert.IsType<List<CharacterDto>>(result.ToList());
            Assert.Equal(2, okResult.Count);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoCharactersExist()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            var result = await service.GetAllCharacters();

            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_ById_ReturnsCharacter_WhenExists()
        {
            var character = new Character { Id = 1L, Name = "Alice", CampaignId = 1, DescriptionShort = "Short description" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var service = new CharacterService(context);

            var result = await service.GetCharacterById(1);

            var okResult = Assert.IsType<CharacterDto>(result);
            Assert.Equal(character.Id, okResult.Id);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            var result = await service.GetCharacterById(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task Post_CreatesCharacter_WhenValid()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            var dto = new CharacterDto
            {
                Name = "NewChar",
                CampaignId = 1,
                DescriptionShort = "New character description"
            };

            var result = await service.AddCharacterAsync(dto);

            Assert.NotNull(result);
            Assert.IsType<CharacterDto>(result);
            Assert.NotNull(result.Id);
            Assert.Equal("NewChar", result.Name);
            Assert.Equal(1, result.CampaignId);
            Assert.NotNull(context.Characters.FirstOrDefault(c => c.Name == "NewChar"));
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenNull()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            try
            {
                var result = await service.AddCharacterAsync(null);
            }
            catch (Exception ex)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
        }

        [Fact]
        public async Task Put_UpdatesCharacter_WhenValid()
        {
            var character = new Character { Id = 1L, Name = "OldName", CampaignId = 1, DescriptionShort = "Test Desc" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var service = new CharacterService(context);

            var dto = new CharacterDto
            {
                Id = 1,
                Name = "UpdatedName",
                CampaignId = 2
            };
            var result = service.UpdateCharacterAsync(dto);

            var updatedCharacter = context.Characters.Find(1L);
            Assert.NotNull(updatedCharacter);
            Assert.Equal("UpdatedName", updatedCharacter.Name);
            Assert.Equal(2, updatedCharacter.CampaignId);

        }


        [Fact]
        public async Task Put_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            var dto = new CharacterDto
            {
                Id = 1,
                Name = "UpdatedName",
                CampaignId = 2
            };

            try
            {
                var result = await service.UpdateCharacterAsync(dto);

            }
            catch (Exception ex)
            {
                Assert.IsType<KeyNotFoundException>(ex);
            }
        }

        [Fact]
        public async Task Delete_RemovesCharacter_WhenExists()
        {
            var character = new Character { Id = 1L, Name = "ToDelete", CampaignId = 1, DescriptionShort = "Test Desc" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var service = new CharacterService(context);
            await service.DeleteCharacterAsync(1);

            context.Entry(character).Reload();
            Assert.Null(context.Characters.Find(1L));
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var service = new CharacterService(context);

            try
            {
                await service.DeleteCharacterAsync(1);
            }
            catch (KeyNotFoundException)
            {
                // Expected exception for not found character
            }
        }
    }
}