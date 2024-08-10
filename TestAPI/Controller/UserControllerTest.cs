using Api.Controllers;
using Api.Models;
using Api.Repository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI.Controller
{
    public class UserControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            // Seteamos las dependencias
            _userRepository = A.Fake<IUserRepository>();
            _userController = new UserController(_userRepository);
        }

        private static User CreateFakeUser() => A.Fake<User>();

        [Fact]
        public async void CreateUserTestSuccess()
        {
            // Arrange {Data}
            var user = CreateFakeUser();

            //Action 
            A.CallTo(() => _userRepository.CreateAsync(user)).Returns(true);
            var result = (CreatedAtActionResult)await _userController.Create(user);

            // Assert
            result.StatusCode.Should().Be(201);
            result.Should().NotBeNull();

        }

        [Fact]
        public async void GetUsersAllSuccess()
        {
            var users = A.Fake<List<User>>();
            users.Add(new User() { Name = "Ejemplo", Email = "email1@gmail.com" });

            A.CallTo(() => _userRepository.GetAllAsync()).Returns(users);
            var result = (OkObjectResult)await _userController.GetUsersAll();

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetUserByIdSuccess()
        {
            int id = 1;
            var user = CreateFakeUser();


            A.CallTo(() => _userRepository.GetByIdAsync(id)).Returns(user);

            var result = (OkObjectResult)await _userController.GetUserById(id);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public async void GetUserByIdSuccess2(int id)
        {
            var user = CreateFakeUser();
            user.Id = id;
            user.Name = "Test";
            user.Email = "Test@gmail.com";

            A.CallTo(() => _userRepository.GetByIdAsync(id)).Returns(user);

            var result = (OkObjectResult)await _userController.GetUserById(id);

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void UpdateUserTestSucces()
        {
            var user = CreateFakeUser();

            A.CallTo(() => _userRepository.UpdateAsync(user)).Returns(true);

            var result = (OkResult)await _userController.UpdateUser(user);

            result.StatusCode.Should().Be(200);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void DeleteUserSuccess()
        {
            int userId = 1;

            A.CallTo(() => _userRepository.DeleteAsync(userId)).Returns(true);

            var result = (NoContentResult)await _userController.DeleteUser(userId);

            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
            result.Should().NotBeNull();
        }

    }
}
