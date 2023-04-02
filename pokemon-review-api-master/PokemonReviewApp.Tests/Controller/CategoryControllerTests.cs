using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;
using Xunit;

namespace PokemonReviewApp.Tests.Controller
{
    public class CategoryControllerTests
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryControllerTests()
        {
            _categoryRepository = A.Fake<ICategoryRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void GetCategories_should_return_OK()
        {
            var categories = A.Fake<ICollection<CategoryDto>>();
            var categoryList = A.Fake<List<CategoryDto>>();
            A.CallTo(() => _mapper.Map<List<CategoryDto>>(categories)).Returns(categoryList);
            var controller = new CategoryController(_categoryRepository, _mapper);

            IActionResult? result = controller.GetCategories();

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void CreateCategory_should_return_OK()
        {
            A.CallTo(() => _categoryRepository.GetCategories()).Returns(new List<Category>());
            var categoryDto = A.Fake<CategoryDto>();
            var category = A.Fake<Category>();
            A.CallTo(() => _mapper.Map<Category>(categoryDto)).Returns(category);
            var categoryMap = A.Fake<Category>();
            A.CallTo(() => _categoryRepository.CreateCategory(categoryMap)).Returns(true);
            var controller = new CategoryController(_categoryRepository, _mapper);

            IActionResult? actual = controller.CreateCategory(categoryDto);

            actual.Should().NotBeNull();
        }

        [Fact]
        public void DeleteCategory_should_return_NotFound_when_non_existing_category()
        {
            int nonExistingId = 12345;
            A.CallTo(() => _categoryRepository.CategoryExists(nonExistingId)).Returns(false);
            var controller = new CategoryController(_categoryRepository, _mapper);

            IActionResult? actual = controller.DeleteCategory(1234);

            actual.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public void UpdateCategory_should_return_BadRequest_when_non_corresponding_ids()
        {
            int someId = 12345;
            int differentId = 6789;
            var updatedCategory = new CategoryDto() { Id = differentId };
            var controller = new CategoryController(_categoryRepository, _mapper);

            IActionResult? actual = controller.UpdateCategory(someId, updatedCategory);

            actual.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void UpdateCategory_should_return_ObjectResult_when_something_wrong_with_update_happened()
        {
            int id = 1;
            Category? categoryMap = new Category();
            CategoryDto updatedCategory = new CategoryDto() { Id = id };
            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(true);
            A.CallTo(() => _mapper.Map<Category>(updatedCategory)).Returns(categoryMap);
            A.CallTo(() => _categoryRepository.UpdateCategory(categoryMap)).Returns(false);
            var controller = new CategoryController(_categoryRepository, _mapper);

            IActionResult? actual = controller.UpdateCategory(id, updatedCategory);

            actual.Should().BeOfType(typeof(ObjectResult));
        }
    }
}
