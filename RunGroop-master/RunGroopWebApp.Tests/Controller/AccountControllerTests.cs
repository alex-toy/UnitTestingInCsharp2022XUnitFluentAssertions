using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace RunGroopWebApp.Tests.Controller
{
    public class AccountControllerTests
    {
        private AccountController _accountController;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILocationService _locationService;

        public AccountControllerTests()
        {
            _userManager = A.Fake<UserManager<AppUser>>();
            _signInManager = A.Fake<SignInManager<AppUser>>();
            _locationService = A.Fake<ILocationService>();

            _accountController = new AccountController(_userManager, _signInManager, _locationService);
        }

        [Fact]
        public void Login_return_success_when_user_is_not_authorized()
        {
            string email = "user@test.fr";
            string password = "abc123";
            AppUser user = new AppUser()
            {
                Email = email
            };
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(user);
            A.CallTo(() => _userManager.CheckPasswordAsync(user, password)).Returns(true);

            //SignInResult? signInResult = A.Fake<SignInResult>();
            //signInResult.(x => x.Succeeded).Returns(false);
            //signInResult.Succeeded<bool>(p => p.Succeeded, true);
            //A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, false)).Returns(signInResult);

            LoginViewModel loginViewModel = new LoginViewModel() { EmailAddress = email, Password = password };
            Task<IActionResult>? actual = _accountController.Login(loginViewModel);

            actual.Should().BeOfType<Task<IActionResult>>();
            _accountController.TempData["Error"].Should().Be("Wrong credentials. Please try again");
        }

        [Fact]
        public void Register_should_return_success_when_user_does_not_exits()
        {
            string email = "user@test.fr";
            string password = "abc123";
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<AppUser>(null));

            RegisterViewModel loginViewModel = new RegisterViewModel() { EmailAddress = email, Password = password };
            Task<IActionResult>? actual = _accountController.Register(loginViewModel);

            actual.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public async void Register_should_return_fail_when_user_exists()
        {
            string email = "user@test.fr";
            string password = "abc123";
            AppUser user = new AppUser() { Email = email };
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<AppUser>(user));

            RegisterViewModel loginViewModel = new RegisterViewModel() { EmailAddress = email, Password = password };
            await _accountController.Register(loginViewModel);

            _accountController.TempData["Error"].Should().Be("This email address is already in use");
        }
    }
}
