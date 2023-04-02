using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApp.Tests.Repository
{
    public class CategoryRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Pokemon.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Categories.Add(
                    new Category()
                    {
                        Name = $"category {i}",
                        PokemonCategories = new List<PokemonCategory>()
                    });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void GetCategory_should_return_correct_category()
        {
            int id = 1;
            var dbContext = await GetDatabaseContext();
            var pokemonRepository = new CategoryRepository(dbContext);

            Category? actual = pokemonRepository.GetCategory(id);

            actual.Should().NotBeNull();
            actual.Should().BeOfType<Category>();
            actual.Name.Should().Be("category 1");
        }

        [Fact]
        public async void DeleteCategory_should_return_empty_list()
        {
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var categoryRepository = new CategoryRepository(dbContext);
            Category? category = categoryRepository.GetCategory(id);

            bool actual = categoryRepository.DeleteCategory(category);

            actual.Should().Be(true);
        }
    }
}
