using FluentAssertions;
using NSubstitute;
using SimonMovilidad.Application.Abstractions.PasswordHash;
using SimonMovilidad.Application.Features.Users.CreateUser;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;
using Xunit;

namespace SimonMovilidad.Application.UnitTest.Users
{
    public class CreateUserCommandHandlerTest
    {
        private readonly CreateUserCommandHandler _handler;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IPasswordHasher _passwordHasherMock;

        public CreateUserCommandHandlerTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _passwordHasherMock = Substitute.For<IPasswordHasher>();

            _handler = new CreateUserCommandHandler(
                _userRepositoryMock,
                _unitOfWorkMock,
                _passwordHasherMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_When_UserAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand("test@example.com", "123456", Domain.Enums.RoleEnum.Viewer);

            _userRepositoryMock.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(new User { Email = command.Email, Password = command.Password, Role = Domain.Enums.RoleEnum.Viewer });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Error.Should().Be(UserError.AlreadyExists);

            await _userRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<User>());

            await _unitOfWorkMock
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
