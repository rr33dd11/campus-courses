using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.HashPassword;
using campus.AdditionalServices.TokenHelpers;
using campus.AdditionalServices.Validators;
using campus.DBContext;
using campus.DBContext.Extensions;
using campus.DBContext.Models;
using campus.DBContext.Models.DTO.AccountDTO;
using campus.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace campus.Services;

public class AccountService : IAccountService
{
    private readonly AppDBContext _db;
    private readonly TokenInteraction _tokenHelper;
    private readonly RedisRepository _redisRepository;

    public AccountService(AppDBContext db, TokenInteraction tokenHelper, RedisRepository redisRepository)
    {
        _db = db;
        _tokenHelper = tokenHelper;
        _redisRepository = redisRepository;
    }

    public async Task<TokenResponse> Login(LoginRequest loginRequest)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Email == loginRequest.email);
            
        if (account == null) throw new BadRequestException("Неправильный Email или пароль"); 
        if (!HashPassword.VerifyPassword(loginRequest.password, account.Password)) throw new BadRequestException("Неправильный Email или пароль");
        
        var token = _tokenHelper.GenerateToken(account);
        return new TokenResponse(token);
    }

    public async Task<TokenResponse> Registration(RegistrationRequest registrationRequest)
    {
        var existingAccount = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Email == registrationRequest.email);
        if (existingAccount != null) throw new BadRequestException("Данный email уже используется");

        if (registrationRequest.password != registrationRequest.confirmPassword) 
            throw new BadRequestException("Пароли должны быть одинаковыми");

        if (!EmailValidator.IsValidEmail(registrationRequest.email))
            throw new BadRequestException("Некорректный email");

        if (!BirthdateValidator.IsValidBirthday(registrationRequest.birthDate))
            throw new BadRequestException("Вам должно быть от 14 до 99 лет");

        if (!PasswordValidator.IsValidPassword(registrationRequest.password))
            throw new BadRequestException("Пароль должен быть не менее 6 символов и содержать хотя бы одну цифру");
            
        var account = new Account()
        {
            FullName = registrationRequest.fullName,
            Password = HashPassword.HashingPassword(registrationRequest.password),
            BirthDate = registrationRequest.birthDate.ToUniversalTime(),
            Email = registrationRequest.email,
            isStudent = false,
            isTeacher = false,
            isAdmin = false,
            CreatedDate = DateTime.UtcNow
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        var token = _tokenHelper.GenerateToken(account);
        return new TokenResponse(token);
    }

    public async Task<GetProfileResponse> GetProfile(string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        return account.ToGetProfileResponse();
    }

    public async Task EditProfile(string token, EditProfileRequest editProfileRequest)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        if (!BirthdateValidator.IsValidBirthday(editProfileRequest.birthDate))
            throw new BadRequestException("Вам должно быть от 14 до 99 лет");
        
        account.FullName = editProfileRequest.fullName;
        account.BirthDate = editProfileRequest.birthDate;

        _db.Accounts.Update(account);
        await _db.SaveChangesAsync();
    }

    public async Task Logout(string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

        await _redisRepository.AddTokenBlackList(token);
    }
}