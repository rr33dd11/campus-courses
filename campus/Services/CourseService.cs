using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.TokenHelpers;
using campus.AdditionalServices.Validators;
using campus.DBContext;
using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Extensions;
using campus.DBContext.Models;
using campus.DBContext.Models.Enums;
using campus.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace campus.Services;

public class CourseService : ICourseService
{
    private readonly TokenInteraction _tokenHelper;
    private readonly AppDBContext _db;

    public CourseService(TokenInteraction tokenInteraction, AppDBContext db)
    {
        _tokenHelper = tokenInteraction;
        _db = db;
    }

    public async Task CreateCourse(CreateCourseRequest createCourseRequest, string token, Guid groupId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
        
        var teacherAccount = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == createCourseRequest.mainTeacherId);
        if (teacherAccount == null) throw new BadRequestException("Пользователя, которого вы указали преподавателем, не существует");

        if (!CourseDateValidator.isValidCourseStartYear(createCourseRequest.startYear, createCourseRequest.semester))
            throw new BadRequestException("Нельзя создавать курс в прошедшем семестре");
        
        var course = new Course()
        {
            Id = new Guid(),
            Name = createCourseRequest.name,
            StartYear = createCourseRequest.startYear,
            MaximumStudentsCount = createCourseRequest.maximumStudentsCount,
            RemainingSlotsCount = createCourseRequest.maximumStudentsCount,
            Semester = createCourseRequest.semester,
            Requirements = createCourseRequest.requirements,
            Annotations = createCourseRequest.annotaitons,
            CreatedDate = DateTime.UtcNow,
            GroupId = groupId,
            Status = CourseStatuses.Created
        };
        _db.Courses.Add(course);

        var teacher = new Teacher()
        {
            AccountId = createCourseRequest.mainTeacherId,
            IsMain = true,
            CourseId = course.Id,
        };
        _db.Teachers.Add(teacher);
        
        if (teacherAccount.TeachingCourses.Count == 1)
        {
            teacherAccount.isTeacher = true;
            _db.Update(teacherAccount);
        }
        await _db.SaveChangesAsync();
    }

    public async Task EditCourse(EditCourseRequest editCourseRequest, string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
        
        var newTeacherAccount = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == editCourseRequest.mainTeacherId);
        if (newTeacherAccount == null)
            throw new NotFoundException("Не найден пользователь, указанный главным преподавателем");
        
        var course = await _db.Courses
            .Include(c => c.Teachers)
                .ThenInclude(t => t.Account)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Данного курса не существует");
        
        if (course.RemainingSlotsCount > editCourseRequest.maximumStudentsCount)
            throw new BadRequestException("На курс уже записано больше человек, чем вы хотите поставить");
        
        var prevMainTeacher = course.Teachers.FirstOrDefault(t => t.IsMain);

        if (prevMainTeacher?.AccountId != editCourseRequest.mainTeacherId)
        {
            if (prevMainTeacher?.Account.TeachingCourses.Count == 1)
            {
                prevMainTeacher.Account.isTeacher = false;
                _db.Update(prevMainTeacher.Account);
            }
            
            _db.Remove(prevMainTeacher);
            var newTeacher = new Teacher()
            {
                AccountId = editCourseRequest.mainTeacherId,
                CourseId = course.Id,
                IsMain = true
            };
            _db.Teachers.Add(newTeacher);
        }
        
        course.Name = editCourseRequest.name;
        course.StartYear = editCourseRequest.startYear;
        course.MaximumStudentsCount = editCourseRequest.maximumStudentsCount;
        course.Semester = editCourseRequest.semester;
        course.Requirements = editCourseRequest.requirements;
        course.Annotations = editCourseRequest.annotaitons;
        
        _db.Courses.Update(course);
        
        if (newTeacherAccount.TeachingCourses.Count == 0)
        {
            newTeacherAccount.isTeacher = true;
            _db.Update(newTeacherAccount);
        }
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteCourse(string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token); 
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");
        
        var course = await _db.Courses
            .Include(course => course.Teachers)
                .ThenInclude(teacher => teacher.Account)
                    .ThenInclude(acc => acc.TeachingCourses)
            .Include(course => course.Students)
                .ThenInclude(student => student.Account)
                    .ThenInclude(acc => acc.MyCourses)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Данного курса не существует");
        
        course.Teachers.ToList().ForEach(teacher =>
        {
            if (teacher.Account.TeachingCourses.Count == 1)
            {
                teacher.Account.isTeacher = false;
                _db.Update(teacher.Account);
            }
        });
        
        course.Students.ToList().ForEach(student =>
        {
            if (student.Account.MyCourses.Count == 1)
            {
                student.Account.isStudent = false;
                _db.Update(student.Account);
            }
        });
        
        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
    }

    public async Task CreateNotification(CreateCourseNotificationRequest createCourseNotification, string token, Guid courseId)
    { 
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        var isTeacher = account.TeachingCourses.Any(t => t.CourseId == courseId);
        if (!(account.isAdmin || isTeacher)) throw new ForbiddenException("У вас недостаточно прав");
        
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");
        
        var newNotification = new Notification()
        {
            CourseId = course.Id,
            IsImportant = createCourseNotification.isImportant,
            Text = createCourseNotification.text,
            CreatedDate = DateTime.UtcNow
        };
        
        _db.Notifications.Add(newNotification);
        await _db.SaveChangesAsync();
    }

    public async Task<CourseDetailsResponse> GetCourseDetails(string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.MyCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var course = await _db.Courses
            .Include(c => c.Students)
                .ThenInclude(s => s.Account)
            .Include(c => c.Teachers)
                .ThenInclude(t => t.Account)
            .Include(c => c.Notifications)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Данного курса не существует");

        var courseDetails = course.toCourseDetailsResponse(account);
        return courseDetails;
    }

    public async Task<List<CourseResponse>> GetMyCourses(string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(a => a.MyCourses)
                .ThenInclude(s => s.Course)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var myCourses = account.MyCourses
            .OrderBy(s => s.Course.Name)
            .Select(s => s.Course.ToCourseResponse())
            .ToList();
        return myCourses;
    }
    
    public async Task<List<CourseResponse>> GetTeachingCourses(string token)
    { 
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(a => a.TeachingCourses)
                .ThenInclude(t => t.Course)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        var teachingCourses = account.TeachingCourses
            .OrderBy(t => t.Course.Name)
            .Select(t => t.Course.ToCourseResponse())
            .ToList();
        return teachingCourses;
    }

    public async Task SignUpOnCourse(string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(a => a.MyCourses)
            .Include(a => a.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        if (account.MyCourses.Any(s => s.CourseId == courseId))
            throw new BadRequestException("Вы уже записаны на этот курс");
        if (account.TeachingCourses.Any(t => t.CourseId == courseId))
            throw new BadRequestException("Вы являетесь учителем на этом курсе");
        
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");
        if (course.Status != CourseStatuses.OpenForAssigning)
            throw new BadRequestException("Курс недоступен для записи");

        var newStudent = new Student()
        {
            AccountId = account.Id,
            CourseId = courseId,
            Status = StudentStatuses.InQueue,
            MidtermResult = StudentMarks.NotDefined,
            FinalResult = StudentMarks.NotDefined,
        };
        _db.Students.Add(newStudent);

        if (!account.isStudent)
        {
            account.isStudent = true;
            _db.Accounts.Update(account);
        }
        
        await _db.SaveChangesAsync();
    }

    public async Task CreateCourseTeacher(CreateTeacherRequest teacherRequest, string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var isMainTeacher = account.TeachingCourses.Any(t => t.IsMain);
        if (!(account.isAdmin || isMainTeacher)) throw new ForbiddenException("У вас недостаточно прав");
        
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Данного курса не существует");
        
        var teacher = await _db.Accounts.
            Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == teacherRequest.userId);
        if (teacher == null) throw new NotFoundException("Данного пользователя не существует");
        if (teacher.TeachingCourses.Any(t => t.CourseId == courseId))
            throw new BadRequestException("Данный пользователь уже является учителем на этом курсе");
        
        var newTeacher = new Teacher()
        {
            AccountId = teacherRequest.userId,
            CourseId = courseId,
            IsMain = false
        };
        
        _db.Teachers.Add(newTeacher);
        await _db.SaveChangesAsync();
    }

    public async Task EditAnnotationsAndRequirements(
        EditAnnotationsAndRequirementsRequest editAnnotationsAndRequirementsRequest, string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var isTeacher = account.TeachingCourses.Any(t => t.CourseId == courseId);
        if (!(account.isAdmin || isTeacher)) throw new ForbiddenException("У вас недостаточно прав");
        
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");
        
        course.Annotations = editAnnotationsAndRequirementsRequest.annotations;
        course.Requirements = editAnnotationsAndRequirementsRequest.requirements;
        
        _db.Courses.Update(course);
        await _db.SaveChangesAsync();
    }

    public async Task EditCourseStatus(EditCourseStatusRequest editCourseStatus, string token, Guid courseId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        
        var isTeacher = account.TeachingCourses.Any(t => t.CourseId == courseId);
        if (!(account.isAdmin || isTeacher)) throw new ForbiddenException("У вас недостаточно прав");
        
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");
        
        course.Status = editCourseStatus.status;
        
        _db.Courses.Update(course);
        await _db.SaveChangesAsync();
    }

    public async Task EditStudentStatus(EditStudentStatusRequest editStudentStatus, string token, Guid courseId, 
        Guid studentId)
    {

        var accountId = _tokenHelper.GetAccountIdFromToken(token); 
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

        var isTeacher = account.TeachingCourses.Any(t => t.CourseId == courseId);
        if (!(account.isAdmin || isTeacher)) throw new ForbiddenException("У вас недостаточно прав");

        var course = await _db.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");

        var student = course.Students.FirstOrDefault(s => s.AccountId == studentId);
        if (student == null) throw new NotFoundException("Такого студента не существует");
        if (student.Status != StudentStatuses.Accepted 
            && editStudentStatus.status == StudentStatuses.Accepted 
            && course.RemainingSlotsCount == 0)
            throw new BadRequestException("На курсе нет мест");
        
        student.Status = editStudentStatus.status;
        _db.Update(student);

        course.RemainingSlotsCount = course.MaximumStudentsCount - course.Students.Count(s => s.Status == StudentStatuses.Accepted);
        _db.Update(course);
        await _db.SaveChangesAsync();
    }

    public async Task EditStudentMarks(EditStudentMarkRequest editStudentMarks, string token, Guid courseId,
        Guid studentId)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts
            .Include(acc => acc.TeachingCourses)
            .FirstOrDefaultAsync(acc => acc.Id == accountId);
        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

        var isTeacher = account.TeachingCourses.Any(t => t.CourseId == courseId);
        if (!(account.isAdmin || isTeacher)) throw new ForbiddenException("У вас недостаточно прав");

        var course = await _db.Courses
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        if (course == null) throw new NotFoundException("Такого курса не существует");

        var student = course.Students.FirstOrDefault(s => s.AccountId == studentId);
        if (student == null) throw new NotFoundException("Такого студента не существует");

        if (student.Status != StudentStatuses.Accepted) throw new BadRequestException("Студент не принят на курс");
        switch (course.Status)
        {
            case CourseStatuses.Created or CourseStatuses.OpenForAssigning:
                throw new BadRequestException("Курс еще не стартовал");
            case CourseStatuses.Finished:
                throw new BadRequestException("Курс уже закоончился");
        }

        if (editStudentMarks.markType == MarkType.Midterm)
        {
            student.MidtermResult = editStudentMarks.mark;
        }
        else
        {
            student.FinalResult = editStudentMarks.mark;
        }
        _db.Update(student);
        await _db.SaveChangesAsync();
    }
}