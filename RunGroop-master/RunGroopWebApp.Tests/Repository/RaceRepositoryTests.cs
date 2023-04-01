using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Repository
{
    public class RaceRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            var racesInDb = await databaseContext.Races.CountAsync();
            var noRace = racesInDb <= 0;
            if (noRace)
            {
                for (int i = 0; i < 10; i++)
                {
                    var race = new Race()
                    {
                        Title = $"Race {i}",
                        Description = $"This is the description of the race {i}",
                        RaceCategory = RaceCategory.FiveK,
                        Address = new Address()
                        {
                            City = $"city {i}",
                            State = $"state {i}",
                            Street = $"street {i}"
                        }
                    };
                    databaseContext.Races.Add(race);
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void Add_should_return_bool()
        {
            var race = new Race()
            {
                Title = "Race BG",
                Description = "This is the description of the race BG",
            };
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);

            var actual = raceRepository.Add(race);

            actual.Should().BeTrue();
        }

        [Fact]
        public async void GetByIdAsync_should_return_race()
        {
            var id = 1;
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);

            Task<Race?>? actual = raceRepository.GetByIdAsync(id);

            actual.Should().BeOfType<Task<Race>>();
            string title = (await actual).Title;
            title.Should().Be("Race 0");
        }

        [Fact]
        public async void GetAll_should_return_list()
        {
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);

            var actual = await raceRepository.GetAll();

            actual.Should().NotBeNull();
            actual.Should().BeOfType<List<Race>>();
        }

        [Fact]
        public async void SuccessfulDelete_should_return_true()
        {
            var race = new Race()
            {
                Title = "Running Club 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = "This is the description of the first cinema",
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new RaceRepository(dbContext);

            clubRepository.Add(race);
            bool actual = clubRepository.Delete(race);
            int count = await clubRepository.GetCountAsync();

            actual.Should().BeTrue();
            count.Should().Be(10);
        }

        [Fact]
        public async void GetCountAsync_should_return_int()
        {
            var race = new Race()
            {
                Title = "Running Club 1",
                Description = "This is the description of the first cinema",
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);

            raceRepository.Add(race);
            int raceCount = await raceRepository.GetCountAsync();

            raceCount.Should().Be(11);
        }

        [Theory]
        [InlineData("city 0", 1)]
        [InlineData("city 1", 1)]
        [InlineData("city 2", 1)]
        public async void GetAllRacesByCity_should_return_correct_count(string city, int count)
        {
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);

            IEnumerable<Race>? actualRaces = await raceRepository.GetAllRacesByCity(city);

            actualRaces.Should().NotBeNull();
            actualRaces.Should().BeOfType<List<Race>>();
            actualRaces.ToList().Count.Should().Be(count);
        }

        [Fact]
        public async void GetCountByCategoryAsync_should_return_correct_count()
        {
            RaceCategory category = RaceCategory.Marathon;
            var race = new Race()
            {
                Title = "Running Club 1",
                Description = "This is the description of the first cinema",
                RaceCategory = category
            };
            var dbContext = await GetDbContext();
            var raceRepository = new RaceRepository(dbContext);
            raceRepository.Add(race);

            int actualRaceCount = await raceRepository.GetCountByCategoryAsync(category);

            actualRaceCount.Should().Be(1);
        }
    }
}
