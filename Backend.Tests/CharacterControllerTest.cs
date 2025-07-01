using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests
{
    public class CharacterControllerTest
    {
        private CampaignToolContext GetDbContextWithData(List<Character> characters)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new CampaignToolContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Characters.AddRange(characters);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void Get_ReturnsAllCharacters_WhenCharactersExist()
        {
            var characters = new List<Character>
            {
                new Character { Id = 1L, CharacterName = "Alice", CampaignId = 1, DescriptionShort = "Test Desc" },
                new Character { Id = 2L, CharacterName = "Bob", CampaignId = 1, DescriptionShort = "Test Desc" }
            };
            using var context = GetDbContextWithData(characters);
            var controller = new CharacterController(context);

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<List<CharacterDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenNoCharactersExist()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var result = controller.Get();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_ById_ReturnsCharacter_WhenExists()
        {
            var character = new Character { Id = 1L, CharacterName = "Alice", CampaignId = 1, DescriptionShort = "Short description" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var controller = new CharacterController(context);

            var result = controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<CharacterDto>(okResult.Value);
            Assert.Equal(character.Id, returned.Id);
        }

        [Fact]
        public void Get_ById_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var result = controller.Get(99);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_CreatesCharacter_WhenValid()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var dto = new CharacterDto
            {
                CharacterName = "NewChar",
                CampaignId = 1,
                DescriptionShort = "New character description"
            };

            var result = controller.Post(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", created.ActionName);
            var addedCharacter = context.Characters.FirstOrDefault(c => c.CharacterName == "NewChar");
            Assert.NotNull(addedCharacter);
            Assert.Equal("NewChar", addedCharacter.CharacterName);
            Assert.Equal(1, addedCharacter.CampaignId);
            Assert.NotNull(context.Characters.FirstOrDefault(c => c.CharacterName == "NewChar"));
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenNull()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var result = controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_UpdatesCharacter_WhenValid()
        {
            var character = new Character { Id = 1L, CharacterName = "OldName", CampaignId = 1, DescriptionShort = "Test Desc" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var controller = new CharacterController(context);

            var dto = new CharacterDto
            {
                Id = 1,
                CharacterName = "UpdatedName",
                CampaignId = 2
            };
            var result = controller.Put(1, dto);

            Assert.IsType<NoContentResult>(result);
            var updatedCharacter = context.Characters.Find(1L);
            Assert.NotNull(updatedCharacter);
            Assert.Equal("UpdatedName", updatedCharacter.CharacterName);
            Assert.Equal(2, updatedCharacter.CampaignId);

        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenIdMismatch()
        {
            var character = new Character { Id = 1L, CharacterName = "OldName", CampaignId = 1, DescriptionShort = "Test Desc" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var controller = new CharacterController(context);

            var dto = new CharacterDto
            {
                Id = 2,
                CharacterName = "UpdatedName",
                CampaignId = 2
            };

            var result = controller.Put(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var dto = new CharacterDto
            {
                Id = 1,
                CharacterName = "UpdatedName",
                CampaignId = 2
            };

            var result = controller.Put(1, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Delete_RemovesCharacter_WhenExists()
        {
            var character = new Character { Id = 1L, CharacterName = "ToDelete", CampaignId = 1, DescriptionShort = "Test Desc" };
            using var context = GetDbContextWithData(new List<Character> { character });
            var controller = new CharacterController(context);
            var result = controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            context.Entry(character).Reload();
            Assert.Null(context.Characters.Find(1L));
            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Characters.Find(1L));
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Character>());
            var controller = new CharacterController(context);

            var result = controller.Delete(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}