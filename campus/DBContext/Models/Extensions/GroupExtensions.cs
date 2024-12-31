using campus.DBContext.DTO.GroupDTO;
using campus.DBContext.Models;

namespace campus.DBContext.Extensions;

public static class GroupExtensions
{
    public static GetGroupResponse ToGetGroupResponse(this Group group)
    {
        return new GetGroupResponse()
        {
            id = group.Id,
            name = group.Name
        };
    }
    
    
}