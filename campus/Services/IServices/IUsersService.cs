using campus.DBContext.DTO.UsersDTO;
using campus.DBContext.Models;

namespace campus.Services.IServices;

public interface IUsersService
{
    public Task<List<UserResponse>> GetUsers(string token);
    public Task<RolesResponse> GetRoles(string token);
}