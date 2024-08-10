using Api.Data;
using Api.Models;
using Api.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;

namespace TestAPI.Repository
{
    public class UserRepositoryTest
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTest()
        {
            var userDbContext = GetDatabaseContext().Result;
            _userRepository = new UserRepository(userDbContext);
        }

        private async Task<UserDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlite("Data Source = UnitTestDb.db").Options;
            var userDbContext = new UserDbContext(options);
            // Elimina y vuelve a crear la base de datos antes de cada prueba
            await userDbContext.Database.EnsureDeletedAsync();
            await userDbContext.Database.EnsureCreatedAsync();

            userDbContext.Users.Add(new User() { Id = 2, Email = "Exmas@gmail.com", Name = "Test" });
            userDbContext.Users.Add(new User() { Id = 3, Email = "Exampole22@gmail.com", Name = "Test" });
            userDbContext.Users.Add(new User() { Id = 4, Email = "Example334@gmail.com", Name = "Test" });

            await userDbContext.SaveChangesAsync();
            return userDbContext;
        }

        //[Fact]
        //public async void CreateUserReturnTrue()
        //{
        //    var user = A.Fake<User>();

        //    var result = await _userRepository.CreateAsync(user);

        //    Assert.True(result);
        //}

        [Fact]
        public async void CreateUserReturnTrue2()
        {
            var user = A.Fake<User>();

            var result = await _userRepository.CreateAsync(user);

            result.Should().BeTrue();

        }

        [Theory]
        [InlineData(2)]
        public async void UpdateUserTestTrue(int id)
        {

            var user = await _userRepository.GetByIdAsync(id);

            // Aseguramos que el usuario exista antes de actualizar
            user.Should().NotBeNull("User should exist before updating");

            // Modificamos los datos para que hagamos el update
            user.Name = "NuevoTest";
            user.Email = "EmailNuevo@gmail.com";

            // Intentamos actualizar al respectivo usuario
            var result = await _userRepository.UpdateAsync(user);

            result.Should().BeTrue();
        }

        [Fact]
        public async void GetAllUserTestTrue()
        {
            var result = await _userRepository.GetAllAsync();
            result.Should().AllBeOfType<User>();
        }

        [Theory]
        [InlineData(3)]
        public async void GetUserByIdTestTrue(int id)
        {
            var result = await _userRepository.GetByIdAsync(id);

            result.Should().BeOfType<User>();
        }

        [Fact]
        public async void DeleteUserByIdTrue()
        {
            int id = 4;

            var result = await _userRepository.DeleteAsync(id);

            result.Should().BeTrue();
        }
    }
}
