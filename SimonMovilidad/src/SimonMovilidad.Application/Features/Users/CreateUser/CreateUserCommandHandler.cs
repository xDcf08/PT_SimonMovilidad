using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Application.Abstractions.PasswordHash;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Application.Features.Users.CreateUser
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser is not null)
            {
                return Result.Failure<Guid>(UserError.AlreadyExists);
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                Password = passwordHash,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newUser.Id;
        }
    }
}
