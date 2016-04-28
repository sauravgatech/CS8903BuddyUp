using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.DomainModel;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.Communicator
{
    public class UserCommunicator
    {
        IUser user;
        public UserCommunicator(IUser user)
        {
            this.user = user;
        }

        public bool UpdateUser(UserUpdateRequest uur)
        {
            try
            {
                user.Update(uur);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUserToGroup(UpdateUserGroup request)
        {
            try
            {
                user.AddUserToGroup(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUserFromGroup(UpdateUserGroup request)
        {
            try
            {
                user.RemoveUserFromGroup(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UserGetResponse GetUser(string emailId)
        {
            try
            {
                return user.Get(emailId).FirstOrDefault();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public string GetUserRole(string emailId)
        {
            try
            {
                return user.GetUserRole(emailId);
            }
            catch
            {
                return null;
            }
        }
    }
}
