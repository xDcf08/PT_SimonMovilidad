using FluentAssertions;
using NSubstitute;
using SimonMovilidad.Application.Abstractions.Authentication;
using SimonMovilidad.Application.Abstractions.PasswordHash;
using SimonMovilidad.Application.Features.Users.LoginUser;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;
using Xunit;

namespace SimonMovilidad.Application.UnitTest.Users
{
    public class LoginUserQueryHandlerTest
    {
        private readonly LoginUserQueryHandler _handler;

        private readonly IUserRepository _userRepositoryMock;
        private readonly IPasswordHasher _passwordHasherMock;
        private readonly IJwtProvider _jwtProviderMock;

        public LoginUserQueryHandlerTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();
            _jwtProviderMock = Substitute.For<IJwtProvider>();

            _handler = new LoginUserQueryHandler(
                _userRepositoryMock,
                _passwordHasherMock,
                _jwtProviderMock    
                );
        }

        [Fact]
        public async Task Handle_Should_ReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                Password = "eBjw89dPXQx8uf02h21FIw==;UanwTCiMP+dZRKBxWQBg6N1u/zlVOLLS7xyeZgBC/Bk=",
                Role = Domain.Enums.RoleEnum.Admin
            };
            var query = new LoginUserQuery(user.Email, user.Password);

            //Configurar los mocks
            _userRepositoryMock.GetByEmailAsync(query.Email, Arg.Any<CancellationToken>())
                .Returns(user);
            _passwordHasherMock.VerifyPassword(query.Password, user.Password)
                .Returns(true);
            _jwtProviderMock.Generate(user)
                .Returns("fake.jwt.token");

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.Value.Should().Be("fake.jwt.token");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var query = new LoginUserQuery("nonexistent@user.com", "123456");
            _userRepositoryMock.GetByEmailAsync(query.Email, Arg.Any<CancellationToken>())
                .Returns((User?)null);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.Error.Should().Be(UserError.InvalidCredentials);

            _passwordHasherMock
                .DidNotReceive()
                .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());

            _jwtProviderMock
                .DidNotReceive()
                .Generate(Arg.Any<User>());
        }
    }
}
