using campus.DBContext.Models.DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = campus.DBContext.Models.DTO.AccountDTO.LoginRequest;

namespace campus.Services.IServices;

public interface IAccountService
{
    public Task<TokenResponse> Login(LoginRequest loginRequest);
    public Task<TokenResponse> Registration(RegistrationRequest registrationRequest);
    public Task EditProfile(string token, EditProfileRequest editProfileRequest);
    public Task<GetProfileResponse> GetProfile(string token);
    public Task Logout(string token);
}