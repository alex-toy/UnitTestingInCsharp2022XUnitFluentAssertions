using FluentAssertions;
using NetworkUtility.Functions;
using Xunit;

namespace NetworkUtility.Tests.FunctionsTests
{
    public static class WorldDumbestFunctionsTests
    {
        private static readonly WorldDumbestFunctions _function = new WorldDumbestFunctions();
        [Fact]
        public static void ReturnPickachuIfZero_should_return_Pichachu_if_param_is_zero()
        {
            string result = _function.ReturnPickachuIfZero(0);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be("Pickachu");
        }

        [Fact]
        public static void ReturnPickachuIfZero_should_not_return_Pichachu_if_param_is_not_zero()
        {
            string result = _function.ReturnPickachuIfZero(1);

            result.Should().NotBeNullOrEmpty();
            result.Should().NotBe("Pickachu");
        }
    }
}
