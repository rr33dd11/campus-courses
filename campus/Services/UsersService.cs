using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.UsersDTO;
using campus.DBContext.Extensions;
using campus.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace campus.Services;

public class UsersService : IUsersService
{
    private readonly AppDBContext _db;
    private readonly TokenInteraction _tokenHelper;

    public UsersService(AppDBContext db, TokenInteraction tokenHelper)
    {
        _db = db;
        _tokenHelper = tokenHelper;
    }

    public async Task<List<UserResponse>> GetUsers(string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        var isMainTeacher = account.TeachingCourses.Any(t => t.IsMain);
        if (!(account.isAdmin || isMainTeacher)) throw new ForbiddenException("У вас недостаточно прав");
        var users = await _db.Accounts.OrderBy(acc => acc.FullName)
            .Select(acc => acc.ToUserResponse())
            .ToListAsync();
        return users;
    }

    public async Task<RolesResponse> GetRoles(string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);

        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        return account.ToRolesResponse();
    }
}