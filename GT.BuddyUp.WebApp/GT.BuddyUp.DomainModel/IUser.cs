using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.DomainModel
{
    public interface IUser
    {
        IEnumerable<UserGetResponse> Get(string emailId = "");

        //DomainModelResponse Add(UserAddRequest request);

        DomainModelResponse Update(UserUpdateRequest request);

        //DomainModelResponse Delete(string emailId);

        //DomainModelResponse AddUserToCourse(UpdateUserCourse request);

        DomainModelResponse AddUserToGroup(UpdateUserGroup request);

        DomainModelResponse RemoveUserFromGroup(UpdateUserGroup request);

        string GetUserRole(string email);
    }
}
