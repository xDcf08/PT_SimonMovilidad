using SimonMovilidad.Application.Abstractions.Authentication;
using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Application.Abstractions.PasswordHash;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Application.Features.Users.LoginUser
{
    internal sealed class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginUserQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return Result.Failure<string>(UserError.InvalidCredentials);
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user!.Password);

            if (!isPasswordValid)
            {
                return Result.Failure<string>(UserError.InvalidCredentials);
            }

            var token = _jwtProvider.Generate(user);

            return token;
        }
    }
}
