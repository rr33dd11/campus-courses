using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.DTO.GroupDTO;

namespace campus.Services.IServices;

public interface IGroupService
{
    public Task<List<GetGroupResponse>> GetGroups(string token);
    public Task CreateGroup(CreateGroupRequest createGroupRequest, string token);
    public Task EditGroup(EditGroupRequest editGroupRequest, string token, Guid groupId);
    public Task DeleteGroup(string token, Guid groupId);
    public Task<List<CourseResponse>> GetCampusesByGroupId(string token, Guid groupId);
}