namespace Api.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenFunctionRepository _tokenFunctionRepository;

        public AuthService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IPasswordService passwordService, ITokenService tokenService, IUserRepository userRepository, ITokenFunctionRepository tokenFunctionRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _tokenFunctionRepository = tokenFunctionRepository;
        }

        public async Task<UserResponse> Register(UserRequest userRequest)
        {
            _passwordService.CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = _mapper.Map<User>(userRequest);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            IList<UserRole>? userRoles = user.UserRoles;

            _tokenService.SetVerifyEmail(out string token, out DateTime created, out DateTime expires);

            TokenFunction tokenFunction = new TokenFunction
            {
                Token = token,
                Type = (int)TokenFunctionEnum.VerifyEmail,
                ExpiresAt = expires,
                Created = created
            };

            //Send Email

            user = await _userRepository.InsertAsync(user, userRoles, tokenFunction);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<TokenResponse> Login(LoginRequest loginRequest)
        {
            User? user = await _userRepository.FindAsync(loginRequest.Identification);

            if (user == null || !_passwordService.VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                throw new BadRequestException("Unfound user or wrong password.");
            else if (user.VerifiedAt == null)
                throw new BadRequestException("Unverified e-mail.");

            _tokenService.SetRefreshToken(out string refreshToken, out DateTime created, out DateTime expires);

            TokenResponse tokenResponse = new TokenResponse
            {
                Token = _tokenService.CreateToken(user),
                RefreshToken = refreshToken,
                ExpiresAt = expires
            };

            TokenFunction tokenFunction = user.TokenFunctions?.Where(x => x.Type == (int)TokenFunctionEnum.RefreshToken).FirstOrDefault() ?? new TokenFunction();
            tokenFunction.Token = refreshToken;
            tokenFunction.Type = (int)TokenFunctionEnum.RefreshToken;
            tokenFunction.ExpiresAt = expires;
            tokenFunction.Created = created;
            if (tokenFunction.Id != 0)
                await _tokenFunctionRepository.UpdateAsync(tokenFunction);
            else
            {
                tokenFunction.UserId = user.Id;
                await _tokenFunctionRepository.InsertAsync(tokenFunction);
            }

            return tokenResponse;
        }

        public async Task<TokenResponse> RefreshToken(string token)
        {
            var cookieToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            TokenFunction? tokenFunction = await _tokenFunctionRepository.FindAsync(token);

            if (tokenFunction == null || tokenFunction.User == null)
                throw new BadRequestException("Unfound token.");
            else if (!tokenFunction.Token.Equals(cookieToken))
                throw new UnauthorizedException("Invalid token.");
            else if (tokenFunction.ExpiresAt < DateTime.Now)
                throw new UnauthorizedException("Expired token.");

            _tokenService.SetRefreshToken(out string refreshToken, out DateTime created, out DateTime expires);

            TokenResponse tokenResponse = new TokenResponse
            {
                Token = _tokenService.CreateToken(tokenFunction.User),
                RefreshToken = refreshToken,
                ExpiresAt = expires,
            };

            tokenFunction.Token = refreshToken;
            tokenFunction.Type = (int)TokenFunctionEnum.RefreshToken;
            tokenFunction.ExpiresAt = expires;
            tokenFunction.Created = created;
            await _tokenFunctionRepository.UpdateAsync(tokenFunction);

            return tokenResponse;
        }

        public async Task ResendVerifyEmail(string identification)
        {
            User? user = await _userRepository.FindAsync(identification);

            if (user == null)
                throw new NotFoundException("Unfound user.");
            else if (user.VerifiedAt != null)
                throw new BadRequestException("E-mail already verified.");

            _tokenService.SetVerifyEmail(out string token, out DateTime created, out DateTime expires);

            TokenFunction tokenFunction = user.TokenFunctions?.Where(x => x.Type == (int)TokenFunctionEnum.VerifyEmail).FirstOrDefault() ?? new TokenFunction();
            tokenFunction.Token = token;
            tokenFunction.Type = (int)TokenFunctionEnum.VerifyEmail;
            tokenFunction.ExpiresAt = expires;
            tokenFunction.Created = created;

            //Send Email

            if (tokenFunction.Id != 0)
                await _tokenFunctionRepository.UpdateAsync(tokenFunction);
            else
            {
                tokenFunction.UserId = user.Id;
                await _tokenFunctionRepository.InsertAsync(tokenFunction);
            }
        }

        public async Task VerifyEmail(string token)
        {
            var cookieToken = _httpContextAccessor.HttpContext?.Request.Cookies["verifyEmail"];
            TokenFunction? tokenFunction = await _tokenFunctionRepository.FindAsync(token);

            if (tokenFunction == null || tokenFunction.User == null)
                throw new BadRequestException("Unfound token.");
            else if (!tokenFunction.Token.Equals(cookieToken))
                throw new UnauthorizedException("Invalid token.");
            else if (tokenFunction.ExpiresAt < DateTime.Now)
                throw new UnauthorizedException("Expired token.");

            User user = tokenFunction.User;
            user.VerifiedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ForgotPassword(string identification)
        {
            User? user = await _userRepository.FindAsync(identification);

            if (user == null)
                throw new NotFoundException("Unfound user.");

            _tokenService.SetResetPassword(out string token, out DateTime created, out DateTime expires);

            TokenFunction tokenFunction = user.TokenFunctions?.Where(x => x.Type == (int)TokenFunctionEnum.ResetPassword).FirstOrDefault() ?? new TokenFunction();
            tokenFunction.Token = token;
            tokenFunction.Type = (int)TokenFunctionEnum.ResetPassword;
            tokenFunction.ExpiresAt = expires;
            tokenFunction.Created = created;

            //Send Email

            if (tokenFunction.Id != 0)
                await _tokenFunctionRepository.UpdateAsync(tokenFunction);
            else
            {
                tokenFunction.Id = user.Id;
                await _tokenFunctionRepository.InsertAsync(tokenFunction);
            }
        }

        public async Task ResetPassword(string token, ResetPasswordRequest resetPasswordRequest)
        {
            var cookieToken = _httpContextAccessor.HttpContext?.Request.Cookies["resetPassword"];
            TokenFunction? tokenFunction = await _tokenFunctionRepository.FindAsync(token);

            if (tokenFunction == null || tokenFunction.User == null)
                throw new BadRequestException("Unfound token.");
            else if (!tokenFunction.Token.Equals(cookieToken))
                throw new UnauthorizedException("Invalid token.");
            else if (tokenFunction.ExpiresAt < DateTime.Now)
                throw new UnauthorizedException("Expired token.");

            _passwordService.CreatePasswordHash(resetPasswordRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = tokenFunction.User;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _userRepository.UpdateAsync(user);
        }
    }
}
