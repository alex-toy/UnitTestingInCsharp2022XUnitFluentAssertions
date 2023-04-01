using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Controller
{
    public class ClubControllerTests
    {
        private ClubController _clubController;
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        public ClubControllerTests()
        {
            //Dependencies
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _clubController = new ClubController(_clubRepository, _photoService);
        }

        [Fact]
        public void ClubController_Index_ReturnsSuccess()
        {
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            
            var actual = _clubController.Index();
            
            actual.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ClubController_Detail_ReturnsSuccess()
        {
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
            
            var actual = _clubController.DetailClub(id, "RunningClub");
            
            actual.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
