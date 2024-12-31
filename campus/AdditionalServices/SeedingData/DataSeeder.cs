using campus.DBContext.Models;
using campus.DBContext.Models.Enums;

namespace campus.AdditionalServices.SeedingData;

// public DbSet<Account> Accounts { get; set; }
// public DbSet<Group> Groups { get; set; }
// public DbSet<Course> Courses { get; set; }
// public DbSet<Student> Students { get; set; }
// public DbSet<Teacher> Teachers { get; set; }
// public DbSet<Notification> Notifications { get; set; }

public class DataSeeder
{
    public List<Account> GetAccounts()
    {
        var accounts = new List<Account>();

        accounts.AddRange(new[]
        {
            new Account // админ
            {
                Id = Guid.Parse("a08d8946-8bac-45e2-afe3-a0ce7795008a"),
                FullName = "Александр Сигмов",
                Password = BCrypt.Net.BCrypt.HashPassword("123123"),
                BirthDate = DateTime.Parse("2004-11-23T14:53:29.390Z").ToUniversalTime(),
                CreatedDate = DateTime.Parse("2024-11-24T18:12:00.390Z").ToUniversalTime(),
                Email = "sanyasigmagucci@example.com",
                isTeacher = true,
                isAdmin = true,
                isStudent = false
            },
            new Account
            {
                Id = Guid.Parse("31f3d270-9cdc-46e4-a807-0a672ad72a6b"),
                FullName = "Дэнжер Лёня",
                Password = BCrypt.Net.BCrypt.HashPassword("123123"),
                BirthDate = DateTime.Parse("2005-11-23T14:53:29.390Z").ToUniversalTime(),
                CreatedDate = DateTime.Parse("2024-11-24T18:12:01.390Z").ToUniversalTime(),
                Email = "dangerlyonya@example.com",
                isTeacher = true,
                isAdmin = false,
                isStudent = true
            },
            new Account
            {
                Id = Guid.Parse("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"),
                FullName = "Антонио Бекэндрос",
                Password = BCrypt.Net.BCrypt.HashPassword("123123"),
                BirthDate = DateTime.Parse("1995-11-23T14:53:29.390Z").ToUniversalTime(),
                CreatedDate = DateTime.Parse("2024-11-24T18:12:02.390Z").ToUniversalTime(),
                Email = "lovebackend@example.com",
                isTeacher = true,
                isAdmin = false,
                isStudent = false
            },
            new Account
            {
                Id = Guid.Parse("f0ed1390-59f0-4e80-8559-3a7850d47a2f"),
                FullName = "Леша Подпивнов 228",
                Password = BCrypt.Net.BCrypt.HashPassword("123123"),
                BirthDate = DateTime.Parse("2006-10-01T14:53:29.390Z").ToUniversalTime(),
                CreatedDate = DateTime.Parse("2024-11-24T18:12:03.390Z").ToUniversalTime(),
                Email = "beerlesha@example.com",
                isTeacher = false,
                isAdmin = false,
                isStudent = true
            },
            new Account
            {
                Id = Guid.Parse("0572342d-fde4-48f6-a754-350a15edbcc0"),
                FullName = "Пашка",
                Password = BCrypt.Net.BCrypt.HashPassword("123123"),
                BirthDate = DateTime.Parse("2005-06-17T14:53:29.390Z").ToUniversalTime(),
                CreatedDate = DateTime.Parse("2024-11-24T18:12:04.390Z").ToUniversalTime(),
                Email = "yanepashka@example.com",
                isTeacher = false,
                isAdmin = false,
                isStudent = true
            }
        });
        return accounts;
    }

    public List<Group> GetGroups()
    {
        var groups = new List<Group>();
        groups.AddRange(new[]
        {
            new Group
            {
                Id = Guid.Parse("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"),
                Name = "Backend",
            },
            new Group
            {
                Id = Guid.Parse("7a4ca155-ada6-47e4-b9d0-f2d4b6467039"),
                Name = "Frontend",
            },
        });
        return groups;
        
    }
    
    public List<Course> GetCourses()
    {
        var courses = new List<Course>();
        
        courses.AddRange(new []
        {
            new Course // курс открытый для записи
            {
                Id = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Name = "ASPNET",
                StartYear = 2025,
                MaximumStudentsCount = 10,
                RemainingSlotsCount = 9,
                Semester = Semesters.Spring,
                Status = CourseStatuses.OpenForAssigning,
                Requirements = "Сдать хотя бы с 4го раза",
                Annotations = "Энтити фрамерворк рулит",
                GroupId = Guid.Parse("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"),
                CreatedDate = DateTime.Parse("2023-11-24T18:12:10.390Z").ToUniversalTime(),
            },
            new Course // курс, который уже стартовал
            {
                Id = Guid.Parse("cce72df6-7f8f-4fea-a36a-25875849c1a6"),
                Name = "PHP",
                StartYear = 2024,
                MaximumStudentsCount = 5,
                RemainingSlotsCount = 4,
                Semester = Semesters.Autumn,
                Status = CourseStatuses.Started,
                Requirements = "Какие-то требования",
                Annotations = "Какая-то аннотация",
                GroupId = Guid.Parse("4fe8a005-f169-45f8-adbd-1e5fe1d7af50"),
                CreatedDate = DateTime.Parse("2023-11-24T18:12:11.390Z").ToUniversalTime(),
            },
            new Course // курс, который уже закончился
            {
                Id = Guid.Parse("4288c2aa-c310-41a3-b2d8-04a8a970504d"),
                Name = "React",
                StartYear = 2023,
                MaximumStudentsCount = 2,
                RemainingSlotsCount = 0,
                Semester = Semesters.Autumn,
                Status = CourseStatuses.Finished,
                Requirements = "Красивые отступы",
                Annotations = "Будем кнопки красить",
                GroupId = Guid.Parse("7a4ca155-ada6-47e4-b9d0-f2d4b6467039"),
                CreatedDate = DateTime.Parse("2023-11-24T18:12:12.390Z").ToUniversalTime(),
            }
        });
        return courses;
    }

    public List<Student> GetStudents()
    {
        var students = new List<Student>();
        
        students.AddRange(new []
        {
            new Student // студент реакта -- леня
            {
                AccountId = Guid.Parse("31f3d270-9cdc-46e4-a807-0a672ad72a6b"),
                CourseId = Guid.Parse("4288c2aa-c310-41a3-b2d8-04a8a970504d"),
                MidtermResult = StudentMarks.Passed,
                FinalResult = StudentMarks.Passed,
                Status = StudentStatuses.Accepted
            },
            new Student // студент реакта -- леша
            {
                AccountId = Guid.Parse("f0ed1390-59f0-4e80-8559-3a7850d47a2f"),
                CourseId = Guid.Parse("4288c2aa-c310-41a3-b2d8-04a8a970504d"),
                MidtermResult = StudentMarks.Failed,
                FinalResult = StudentMarks.Failed,
                Status = StudentStatuses.Accepted
            },
            
            
            new Student // отклоненный студент пхп -- леша
            {
                AccountId = Guid.Parse("f0ed1390-59f0-4e80-8559-3a7850d47a2f"),
                CourseId = Guid.Parse("cce72df6-7f8f-4fea-a36a-25875849c1a6"),
                MidtermResult = StudentMarks.NotDefined,
                FinalResult = StudentMarks.NotDefined,
                Status = StudentStatuses.Declined
            },
            new Student // студент пхп -- паша
            {
                AccountId = Guid.Parse("0572342d-fde4-48f6-a754-350a15edbcc0"),
                CourseId = Guid.Parse("cce72df6-7f8f-4fea-a36a-25875849c1a6"),
                MidtermResult = StudentMarks.NotDefined,
                FinalResult = StudentMarks.NotDefined,
                Status = StudentStatuses.Accepted
            },
            
            
            new Student // студент в очереди дотнет -- паша
            {
                AccountId = Guid.Parse("0572342d-fde4-48f6-a754-350a15edbcc0"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                MidtermResult = StudentMarks.NotDefined,
                FinalResult = StudentMarks.NotDefined,
                Status = StudentStatuses.InQueue
            },
            new Student // студент дотнет -- леша
            {
                AccountId = Guid.Parse("f0ed1390-59f0-4e80-8559-3a7850d47a2f"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                MidtermResult = StudentMarks.NotDefined,
                FinalResult = StudentMarks.NotDefined,
                Status = StudentStatuses.Accepted
            }
        });
        
        return students;
    }

    public List<Teacher> GetTeachers()
    {
        var teachers = new List<Teacher>();
        
        teachers.AddRange(new []
        {
            new Teacher // м препод для курса реакта -- саня
            {
                AccountId = Guid.Parse("a08d8946-8bac-45e2-afe3-a0ce7795008a"),
                CourseId = Guid.Parse("4288c2aa-c310-41a3-b2d8-04a8a970504d"),
                IsMain = true
            },
            new Teacher // м препод для дотнета -- антонио
            {
                AccountId = Guid.Parse("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                IsMain = true
            },
            new Teacher // м препод для пхп -- антонио
            {
                AccountId = Guid.Parse("cddc13d6-ce4a-472e-87ca-24b8a10f88e8"),
                CourseId = Guid.Parse("cce72df6-7f8f-4fea-a36a-25875849c1a6"),
                IsMain = true
            },
            new Teacher // доп препод для дотнета -- леня
            {
                AccountId = Guid.Parse("31f3d270-9cdc-46e4-a807-0a672ad72a6b"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                IsMain = false
            }
        });
        
        return teachers;
    }

    public List<Notification> GetNotifications()
    {
        var notifications = new List<Notification>();
        
        notifications.AddRange(new []
        {
            new Notification
            {
                Id = Guid.Parse("53e410fe-df1d-423b-86c5-84a75e113506"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "Анекдот",
                IsImportant = true,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:10.390Z").ToUniversalTime()
            },
            new Notification
            {
                Id = Guid.Parse("5b88985c-c0c2-4820-87cc-d7f954fdbfd1"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "Купается одетый мужик в речке",
                IsImportant = false,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:11.390Z").ToUniversalTime()
            },
            new Notification
            {
                Id = Guid.Parse("3ef63582-42af-41a8-bfb9-268681f24934"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "Ему говорят, мол мужик что ты в одежде купаешься?",
                IsImportant = false,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:12.390Z").ToUniversalTime()
            },
            new Notification
            {
                Id = Guid.Parse("1be6757b-5096-4a73-a8b1-5552b6c09931"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "Он отвечает, а я ее стираю",
                IsImportant = false,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:13.390Z").ToUniversalTime()
            },
            new Notification
            {
                Id = Guid.Parse("10d28aa7-7e7f-419c-84c2-ea8601233094"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "У него спрашивают, а что не в в машинке не стираешь?",
                IsImportant = false,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:14.390Z").ToUniversalTime()
            },
            new Notification
            {
                Id = Guid.Parse("ab00d7de-8c63-487d-a1da-417e6b994c0f"),
                CourseId = Guid.Parse("d5c75cec-b93e-42fb-9725-7f4aaf57dd28"),
                Text = "А он говорит: меня в ней укачивает",
                IsImportant = false,
                CreatedDate = DateTime.Parse("2024-11-24T18:13:15.390Z").ToUniversalTime()
            },
        });
        
        return notifications;
    }
}