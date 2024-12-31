using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.DTO.GroupDTO;
using campus.DBContext.Extensions;
using campus.DBContext.Models;
using campus.DBContext.Models.Enums;
using campus.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace campus.Services;

public class GroupService : IGroupService
{
    private readonly AppDBContext _db;
    private readonly TokenInteraction _tokenHelper;

    public GroupService(AppDBContext db, TokenInteraction tokenHelper)
    {
        _db = db;
        _tokenHelper = tokenHelper;
    }

    public async Task CreateGroup(CreateGroupRequest createGroupRequest, string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
                
        var group = new Group()
        {
            CreatedDate = DateTime.UtcNow,
            Name = createGroupRequest.name
        };
        _db.Groups.Add(group);
        await _db.SaveChangesAsync();
    }
    
    public async Task EditGroup(EditGroupRequest editGroupRequest, string token, Guid groupId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
                
        var group = await _db.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
        if (group == null) throw new NotFoundException("Данной группы не существует");
                    
        group.Name = editGroupRequest.name;
        _db.Groups.Update(group);
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteGroup(string token, Guid groupId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
                
        var group = await _db.Groups
            .Include(g => g.Courses)
                .ThenInclude(c => c.Teachers)
                    .ThenInclude(t => t.Account)
            .Include(g => g.Courses)
                .ThenInclude(c => c.Students)
                    .ThenInclude(s => s.Account)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        if (group == null) throw new NotFoundException("Данной группы не существует");

        group.Courses.ToList().ForEach(course =>
        {
            course.Teachers.ToList().ForEach(teacher =>
            {
                if (teacher.Account.TeachingCourses.Count == group.Courses.Count)
                {
                    teacher.Account.isTeacher = false;
                    _db.Update(teacher.Account);
                }
            });
        
            course.Students.ToList().ForEach(student =>
            {
                if (student.Account.MyCourses.Count == group.Courses.Count)
                {
                    student.Account.isStudent = false;
                    _db.Update(student.Account);
                }
            });
        });
        
        _db.Groups.Remove(group);
        await _db.SaveChangesAsync();
    }

    public async Task<List<GetGroupResponse>> GetGroups(string token)
    { 
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var groups = await _db.Groups
            .OrderBy(g => g.Name)
            .Select(group => group.ToGetGroupResponse())
            .ToListAsync();
        return groups;
    }

    public async Task<List<CourseResponse>> GetCampusesByGroupId(string token, Guid groupId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var group = await _db.Groups
            .Include(group => group.Courses)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        if (group == null) throw new NotFoundException("Данной группы не существует");
        
        var courses = group.Courses
            .OrderBy(c => c.Name)
            .Select(course => course.ToCourseResponse())
            .ToList();
        return courses;
    }
}

//У тебя в каждом сервисе повторяется логика проверки валидности токена и что юзер существует. Надо это вынести