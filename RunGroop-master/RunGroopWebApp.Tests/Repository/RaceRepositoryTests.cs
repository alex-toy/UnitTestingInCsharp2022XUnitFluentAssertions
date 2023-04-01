using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using System;
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

            Race? actual = await raceRepository.GetByIdAsync(id);

            actual.Should().NotBeNull();
            actual.Title.Should().Be("Race 1");
            actual.Should().BeOfType<Task<Race>>();
        }

        //[Fact]
        //public async void ClubRepository_GetAll_ReturnsList()
        //{
        //    //Arrange
        //    var dbContext = await GetDbContext();
        //    var clubRepository = new ClubRepository(dbContext);

        //    //Act
        //    var result = await clubRepository.GetAll();

        //    //Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<List<Club>>();
        //}

        //[Fact]
        //public async void ClubRepository_SuccessfulDelete_ReturnsTrue()
        //{
        //    //Arrange
        //    var club = new Club()
        //    {
        //        Title = "Running Club 1",
        //        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //        Description = "This is the description of the first cinema",
        //        ClubCategory = ClubCategory.City,
        //        Address = new Address()
        //        {
        //            Street = "123 Main St",
        //            City = "Charlotte",
        //            State = "NC"
        //        }
        //    };
        //    var dbContext = await GetDbContext();
        //    var clubRepository = new ClubRepository(dbContext);

        //    //Act
        //    clubRepository.Add(club);
        //    var result = clubRepository.Delete(club);
        //    var count = await clubRepository.GetCountAsync();

        //    //Assert
        //    result.Should().BeTrue();
        //    count.Should().Be(0);
        //}

        //[Fact]
        //public async void ClubRepository_GetCountAsync_ReturnsInt()
        //{
        //    //Arrange
        //    var club = new Club()
        //    {
        //        Title = "Running Club 1",
        //        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //        Description = "This is the description of the first cinema",
        //        ClubCategory = ClubCategory.City,
        //        Address = new Address()
        //        {
        //            Street = "123 Main St",
        //            City = "Charlotte",
        //            State = "NC"
        //        }
        //    };
        //    var dbContext = await GetDbContext();
        //    var clubRepository = new ClubRepository(dbContext);

        //    //Act
        //    clubRepository.Add(club);
        //    var result = await clubRepository.GetCountAsync();

        //    //Assert
        //    result.Should().Be(1);
        //}

        //[Fact]
        //public async void ClubRepository_GetAllStates_ReturnsList()
        //{
        //    //Arrange
        //    var dbContext = await GetDbContext();
        //    var clubRepository = new ClubRepository(dbContext);

        //    //Act
        //    var result = await clubRepository.GetAllStates();

        //    //Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<List<State>>();
        //}

        //[Fact]
        //public async void ClubRepository_GetClubsByState_ReturnsList()
        //{
        //    //Arrange
        //    var state = "NC";
        //    var club = new Club()
        //    {
        //        Title = "Running Club 1",
        //        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //        Description = "This is the description of the first cinema",
        //        ClubCategory = ClubCategory.City,
        //        Address = new Address()
        //        {
        //            Street = "123 Main St",
        //            City = "Charlotte",
        //            State = "NC"
        //        }
        //    };
        //    var dbContext = await GetDbContext();
        //    var clubRepository = new ClubRepository(dbContext);

        //    //Act
        //    clubRepository.Add(club);
        //    var result = await clubRepository.GetClubsByState(state);

        //    //Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<List<Club>>();
        //    result.First().Title.Should().Be("Running Club 1");
        //}
    }
}
