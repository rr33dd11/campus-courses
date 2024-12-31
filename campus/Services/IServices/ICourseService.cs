using campus.DBContext.DTO.CourseDTO;

namespace campus.Services.IServices;

public interface ICourseService
{
    public Task<CourseDetailsResponse> GetCourseDetails(string token, Guid courseId);
    public Task SignUpOnCourse(string token, Guid courseId);
    public Task EditCourseStatus(EditCourseStatusRequest editCourseStatus, string token, Guid courseId);
    public Task EditStudentStatus(EditStudentStatusRequest editCourseStudent, string token, Guid courseId, Guid studentId);
    public Task CreateNotification(CreateCourseNotificationRequest createNotification, string token, Guid courseId);
    public Task EditStudentMarks(EditStudentMarkRequest editStudentMarks, string token, Guid courseId, Guid studentId);
    public Task CreateCourse(CreateCourseRequest createCourseRequest, string token, Guid groupId);
    public Task EditAnnotationsAndRequirements(EditAnnotationsAndRequirementsRequest editAnnotationsAndRequirementsRequest, string token, Guid courseId);
    public Task EditCourse(EditCourseRequest editCourseRequest, string token, Guid courseId);
    public Task DeleteCourse(string token, Guid courseId);
    public Task CreateCourseTeacher(CreateTeacherRequest teacherRequest, string token, Guid courseId);
    public Task<List<CourseResponse>> GetMyCourses(string token);
    public Task<List<CourseResponse>> GetTeachingCourses(string token);


}